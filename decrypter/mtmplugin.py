from mitmproxy import tcp, ctx
import base64
import pyshark
import json
from Crypto.Cipher import PKCS1_v1_5, ChaCha20
from Crypto.PublicKey import RSA
from dissect.cstruct import u8, u16
from io import BytesIO

import Proto_pb2
from google.protobuf.message import Message
CLIENT_PRIVATE_KEY = RSA.import_key(
base64.b64decode("""
MIICXwIBAAKBgQDZzQ1I1YbpehALLFCDe5rAxGvNfl6fehRrCCfmvBtWk4cQP1Ey
zkPeudYPoA0L4Z8FPMnB57NimwKvPL7VuzSjqTqYbr0q8rVY70eR1SPnXBn0D/mY
PwF+pZMFvZ2iU0E6ADO4N14Px9VTq+/lWucX1IcCmr3NBBNVUXhxWnEAawIDAQAB
AoGBALq84oz19owryaGqxwVUbuSkBEHV/U8Cnor+HSfpVA8wwfaeMwI6c1p9Pxl6
gnUTZwJYkiuceuMqQGz6yiv6LDAoiLMZ7MpRAcQYeDYKq3gjZDrXhpTFVGciEy/D
1bd1YI51RGlWJAbo47wQTjTL15quAecqYdmDMqUrfCWOAhGRAkEA+lDvuB+mAvd9
bjl8WQG6QRDrmaCAiSVNL5vJTOa8+vhNzSia0esxMEYvPMkYXhA1w4deHFd2dSsq
ULkveJBZ7wJBAN6/G6FbjozubZzvqCWN1tbEVknbZrFvfFKE08j085BJalriz1mY
0zdViEOyC14nMX85HYbAM1hMXs3ALESibUUCQQCHTPhGLdUuBVhnG+t6sNRcFylC
AN95QhBWi35jctTzUwO6wRfuH5KW5VjjIk9piJmG9sSHzT7aVlqpB3ABWwVBAkEA
kHST8LM14YQHJk3uWFyCRcoSw9c75DqO/90QlDT3eE2EYejR3CNWZ35a3rwqzybP
3Ngno+ol4k+08+57Rw+nmQJBAMMjv4/XM/KX126fs0/wevFvFE4Lg76PXChX5uSH
pWwZmKuKtdxZ/L3MA9Ys3MNbWXK8UL5sv39Ch6BCCYLTUYo=
""".replace("\n", "")))

RSA_CIPHER = PKCS1_v1_5.new(CLIENT_PRIVATE_KEY)

cs_id_to_packet: dict[int, type[Message]] = {}
sc_id_to_packet: dict[int, type[Message]] = {}
class TcpInterceptor:
    def __init__(self):
        print(CLIENT_PRIVATE_KEY)
        self.target_port = 30000 

    def tcp_start(self, flow: tcp.TCPFlow):
        if flow.server_conn.address.port == self.target_port:
            ctx.log.info(f"TCP connection received {self.target_port}")
    
    def tcp_message(self, flow: tcp.TCPFlow):
        ctx.log.info(f"Received msg idk: {flow.messages[-1].content.decode(errors='ignore')}")
        is_target_port = flow.server_conn.address.port == self.target_port
        
        data = flow.messages[-1].content

        on_packet(data, is_target_port)

    def tcp_end(self, flow: tcp.TCPFlow):
        print("Connection end")


with open('SCMessageID.json', 'r') as f:
    message_data = json.load(f)
with open('CSMessageID.json', 'r') as f:
    message_data2 = json.load(f)
# Cicla sui dati caricati
for name, id in message_data.items():
    type_name = name
    if hasattr(Proto_pb2, type_name):
        sc_id_to_packet[id] = getattr(Proto_pb2, type_name)
for name, id in message_data2.items():
    type_name = name
    if hasattr(Proto_pb2, type_name):
        cs_id_to_packet[id] = getattr(Proto_pb2, type_name)
remaining_c2s = b""
remaining_s2c = b""

packets = []

cipher_initialized = False
c2s_cipher = None
s2c_cipher = None

def setup_cipher(sc_login: Proto_pb2.ScLogin):
    global cipher_initialized, c2s_cipher, s2c_cipher
    assert not cipher_initialized, "Cannot initialize cipher twice"

    key = RSA_CIPHER.decrypt(sc_login.server_public_key, None)
    assert key is not None

    nonce = sc_login.server_encryp_nonce
    c2s_cipher = ChaCha20.new(key=key, nonce=nonce)
    c2s_cipher.seek(64) # to simulate counter 1
    s2c_cipher = ChaCha20.new(key=key, nonce=nonce)
    s2c_cipher.seek(64)

    cipher_initialized = True
def process_packets(data: bytes, is_c2s: bool):
    global packets, c2s_cipher, s2c_cipher, cipher_initialized

    cipher = c2s_cipher if is_c2s else s2c_cipher

    parsed_until = 0

    with BytesIO(data) as f:
        while f.tell() != len(data):
            header_len = u8(f.read(1))
            body_len = u16(f.read(2))

            header_data = f.read(header_len)
            if len(header_data) < header_len:
                print(f"not enough data to read header: {len(header_data)} vs. {header_len}")
                break

            body_data = f.read(body_len)
            if len(body_data) < body_len:
                print(f"not enough data to read body: {len(body_data)} vs. {body_len}")
                break

            parsed_until = f.tell()

            #print(body_data.hex())

            if cipher:
                header_data = cipher.decrypt(header_data)
                body_data = cipher.decrypt(body_data)

            header = Proto_pb2.CSHead()
            header.ParseFromString(header_data)

            print(f"{'c2s' if is_c2s else 's2c'}")
            print(header)

            body_type = (cs_id_to_packet if is_c2s else sc_id_to_packet)[header.msgid]
            print(f"type: {body_type}")

            body = body_type()
            body.ParseFromString(body_data)

            print(body)
            if isinstance(body, Proto_pb2.ScLogin) and not cipher_initialized:
                setup_cipher(body)

    return data[parsed_until:]

def on_packet(data: bytes, is_c2s: bool):
    global remaining_c2s
    global remaining_s2c
    try:
        if is_c2s:
            remaining_c2s = process_packets(remaining_c2s + data, is_c2s)
        else:
            remaining_s2c = process_packets(remaining_s2c + data, is_c2s)
    except Exception as e:
        pass
addons = [TcpInterceptor()]