/*
 * Copyright (c) 2015, 2018 Scott Bennett
 *           (c) 2018-2023 Kaarlo Räihä
 *
 * Permission to use, copy, modify, and distribute this software for any
 * purpose with or without fee is hereby granted, provided that the above
 * copyright notice and this permission notice appear in all copies.
 *
 * THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
 * WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
 * ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
 * WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
 * ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
 * OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
 */

using System;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;

namespace BeyondTools.VFS.Crypto;

/// <summary>
/// Chosen SIMD mode
/// </summary>
public enum SimdMode
{
    /// <summary>
    /// Autodetect
    /// </summary>
    AutoDetect = 0,

    /// <summary>
    /// 128 bit SIMD
    /// </summary>
    V128,

    /// <summary>
    /// 256 bit SIMD
    /// </summary>
    V256,

    /// <summary>
    /// 512 bit SIMD
    /// </summary>
    V512,

    /// <summary>
    /// No SIMD
    /// </summary>
    None
}

/// <summary>
/// Class for ChaCha20 encryption / decryption
/// </summary>
public sealed class CSChaCha20 : IDisposable
{
    /// <summary>
    /// Only allowed key lenght in bytes
    /// </summary>
    public const int allowedKeyLength = 32;

    /// <summary>
    /// Only allowed nonce lenght in bytes
    /// </summary>
    public const int allowedNonceLength = 12;

    /// <summary>
    /// How many bytes are processed per loop
    /// </summary>
    public const int processBytesAtTime = 64;

    private const int stateLength = 16;

    /// <summary>
    /// The ChaCha20 state (aka "context")
    /// </summary>
    private readonly uint[] state = new uint[stateLength];

    /// <summary>
    /// Determines if the objects in this class have been disposed of. Set to true by the Dispose() method.
    /// </summary>
    private bool isDisposed = false;

    /// <summary>
    /// Set up a new ChaCha20 state. The lengths of the given parameters are checked before encryption happens.
    /// </summary>
    /// <remarks>
    /// See <a href="https://tools.ietf.org/html/rfc7539#page-10">ChaCha20 Spec Section 2.4</a> for a detailed description of the inputs.
    /// </remarks>
    /// <param name="key">
    /// A 32-byte (256-bit) key, treated as a concatenation of eight 32-bit little-endian integers
    /// </param>
    /// <param name="nonce">
    /// A 12-byte (96-bit) nonce, treated as a concatenation of three 32-bit little-endian integers
    /// </param>
    /// <param name="counter">
    /// A 4-byte (32-bit) block counter, treated as a 32-bit little-endian integer
    /// </param>
    public CSChaCha20(byte[] key, byte[] nonce, uint counter)
    {
        KeySetup(key);
        IvSetup(nonce, counter);
    }

    /// <summary>
    /// Set up a new ChaCha20 state. The lengths of the given parameters are checked before encryption happens.
    /// </summary>
    /// <remarks>
    /// See <a href="https://tools.ietf.org/html/rfc7539#page-10">ChaCha20 Spec Section 2.4</a> for a detailed description of the inputs.
    /// </remarks>
    /// <param name="key">A 32-byte (256-bit) key, treated as a concatenation of eight 32-bit little-endian integers</param>
    /// <param name="nonce">A 12-byte (96-bit) nonce, treated as a concatenation of three 32-bit little-endian integers</param>
    /// <param name="counter">A 4-byte (32-bit) block counter, treated as a 32-bit little-endian unsigned integer</param>
    public CSChaCha20(ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, uint counter)
    {
        KeySetup(key.ToArray());
        IvSetup(nonce.ToArray(), counter);
    }

    /// <summary>
    /// The ChaCha20 state (aka "context"). Read-Only.
    /// </summary>
    public uint[] State
    {
        get
        {
            return state;
        }
    }


    // These are the same constants defined in the reference implementation.
    // http://cr.yp.to/streamciphers/timings/estreambench/submissions/salsa20/chacha8/ref/chacha.c
    private static readonly byte[] sigma = "expand 32-byte k"u8.ToArray();
    private static readonly byte[] tau = "expand 16-byte k"u8.ToArray();

    /// <summary>
    /// Set up the ChaCha state with the given key. A 32-byte key is required and enforced.
    /// </summary>
    /// <param name="key">
    /// A 32-byte (256-bit) key, treated as a concatenation of eight 32-bit little-endian integers
    /// </param>
    private void KeySetup(byte[] key)
    {
        if (key == null)
        {
            throw new ArgumentNullException("Key is null");
        }

        if (key.Length != allowedKeyLength)
        {
            throw new ArgumentException($"Key length must be {allowedKeyLength}. Actual: {key.Length}");
        }

        state[4] = Util.U8To32Little(key, 0);
        state[5] = Util.U8To32Little(key, 4);
        state[6] = Util.U8To32Little(key, 8);
        state[7] = Util.U8To32Little(key, 12);

        byte[] constants = key.Length == allowedKeyLength ? sigma : tau;
        int keyIndex = key.Length - 16;

        state[8] = Util.U8To32Little(key, keyIndex + 0);
        state[9] = Util.U8To32Little(key, keyIndex + 4);
        state[10] = Util.U8To32Little(key, keyIndex + 8);
        state[11] = Util.U8To32Little(key, keyIndex + 12);

        state[0] = Util.U8To32Little(constants, 0);
        state[1] = Util.U8To32Little(constants, 4);
        state[2] = Util.U8To32Little(constants, 8);
        state[3] = Util.U8To32Little(constants, 12);
    }

    /// <summary>
    /// Set up the ChaCha state with the given nonce (aka Initialization Vector or IV) and block counter. A 12-byte nonce and a 4-byte counter are required.
    /// </summary>
    /// <param name="nonce">
    /// A 12-byte (96-bit) nonce, treated as a concatenation of three 32-bit little-endian integers
    /// </param>
    /// <param name="counter">
    /// A 4-byte (32-bit) block counter, treated as a 32-bit little-endian integer
    /// </param>
    private void IvSetup(byte[] nonce, uint counter)
    {
        if (nonce == null)
        {
            // There has already been some state set up. Clear it before exiting.
            Dispose();
            throw new ArgumentNullException("Nonce is null");
        }

        if (nonce.Length != allowedNonceLength)
        {
            // There has already been some state set up. Clear it before exiting.
            Dispose();
            throw new ArgumentException($"Nonce length must be {allowedNonceLength}. Actual: {nonce.Length}");
        }

        state[12] = counter;
        state[13] = Util.U8To32Little(nonce, 0);
        state[14] = Util.U8To32Little(nonce, 4);
        state[15] = Util.U8To32Little(nonce, 8);
    }

    private static SimdMode DetectSimdMode()
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return SimdMode.V512;
        }
        else if (Vector256.IsHardwareAccelerated)
        {
            return SimdMode.V256;
        }
        else if (Vector128.IsHardwareAccelerated)
        {
            return SimdMode.V128;
        }

        return SimdMode.None;
    }

    #region Encryption methods

    /// <summary>
    /// Encrypt arbitrary-length byte array (input), writing the resulting byte array to preallocated output buffer.
    /// </summary>
    /// <remarks>Since this is symmetric operation, it doesn't really matter if you use Encrypt or Decrypt method</remarks>
    /// <param name="output">Output byte array, must have enough bytes</param>
    /// <param name="input">Input byte array</param>
    /// <param name="numBytes">Number of bytes to encrypt</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    public void EncryptBytes(byte[] output, byte[] input, int numBytes, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (output == null)
        {
            throw new ArgumentNullException("output", "Output cannot be null");
        }

        if (input == null)
        {
            throw new ArgumentNullException("input", "Input cannot be null");
        }

        if (numBytes < 0 || numBytes > input.Length)
        {
            throw new ArgumentOutOfRangeException("numBytes", "The number of bytes to read must be between [0..input.Length]");
        }

        if (output.Length < numBytes)
        {
            throw new ArgumentOutOfRangeException("output", $"Output byte array should be able to take at least {numBytes}");
        }

        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        WorkBytes(output, input, numBytes, simdMode);
    }

    /// <summary>
    /// Encrypt arbitrary-length byte stream (input), writing the resulting bytes to another stream (output)
    /// </summary>
    /// <param name="output">Output stream</param>
    /// <param name="input">Input stream</param>
    /// <param name="howManyBytesToProcessAtTime">How many bytes to read and write at time, default is 1024</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    public void EncryptStream(Stream output, Stream input, int howManyBytesToProcessAtTime = 1024, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        WorkStreams(output, input, simdMode, howManyBytesToProcessAtTime);
    }

    /// <summary>
    /// Async encrypt arbitrary-length byte stream (input), writing the resulting bytes to another stream (output)
    /// </summary>
    /// <param name="output">Output stream</param>
    /// <param name="input">Input stream</param>
    /// <param name="howManyBytesToProcessAtTime">How many bytes to read and write at time, default is 1024</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    public async Task EncryptStreamAsync(Stream output, Stream input, int howManyBytesToProcessAtTime = 1024, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        await WorkStreamsAsync(output, input, simdMode, howManyBytesToProcessAtTime);
    }

    /// <summary>
    /// Encrypt arbitrary-length byte array (input), writing the resulting byte array to preallocated output buffer.
    /// </summary>
    /// <remarks>Since this is symmetric operation, it doesn't really matter if you use Encrypt or Decrypt method</remarks>
    /// <param name="output">Output byte array, must have enough bytes</param>
    /// <param name="input">Input byte array</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    public void EncryptBytes(byte[] output, byte[] input, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (output == null)
        {
            throw new ArgumentNullException("output", "Output cannot be null");
        }

        if (input == null)
        {
            throw new ArgumentNullException("input", "Input cannot be null");
        }

        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        WorkBytes(output, input, input.Length, simdMode);
    }

    /// <summary>
    /// Encrypt arbitrary-length byte array (input), writing the resulting byte array that is allocated by method.
    /// </summary>
    /// <remarks>Since this is symmetric operation, it doesn't really matter if you use Encrypt or Decrypt method</remarks>
    /// <param name="input">Input byte array</param>
    /// <param name="numBytes">Number of bytes to encrypt</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    /// <returns>Byte array that contains encrypted bytes</returns>
    public byte[] EncryptBytes(byte[] input, int numBytes, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (input == null)
        {
            throw new ArgumentNullException("input", "Input cannot be null");
        }

        if (numBytes < 0 || numBytes > input.Length)
        {
            throw new ArgumentOutOfRangeException("numBytes", "The number of bytes to read must be between [0..input.Length]");
        }

        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        byte[] returnArray = new byte[numBytes];
        WorkBytes(returnArray, input, numBytes, simdMode);
        return returnArray;
    }

    /// <summary>
    /// Encrypt arbitrary-length byte array (input), writing the resulting byte array that is allocated by method.
    /// </summary>
    /// <remarks>Since this is symmetric operation, it doesn't really matter if you use Encrypt or Decrypt method</remarks>
    /// <param name="input">Input byte array</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    /// <returns>Byte array that contains encrypted bytes</returns>
    public byte[] EncryptBytes(byte[] input, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (input == null)
        {
            throw new ArgumentNullException("input", "Input cannot be null");
        }

        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        byte[] returnArray = new byte[input.Length];
        WorkBytes(returnArray, input, input.Length, simdMode);
        return returnArray;
    }

    /// <summary>
    /// Encrypt string as UTF8 byte array, returns byte array that is allocated by method.
    /// </summary>
    /// <remarks>Here you can NOT swap encrypt and decrypt methods, because of bytes-string transform</remarks>
    /// <param name="input">Input string</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    /// <returns>Byte array that contains encrypted bytes</returns>
    public byte[] EncryptString(string input, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (input == null)
        {
            throw new ArgumentNullException("input", "Input cannot be null");
        }

        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(input);
        byte[] returnArray = new byte[utf8Bytes.Length];

        WorkBytes(returnArray, utf8Bytes, utf8Bytes.Length, simdMode);
        return returnArray;
    }

    #endregion // Encryption methods


    #region // Decryption methods

    /// <summary>
    /// Decrypt arbitrary-length byte array (input), writing the resulting byte array to the output buffer.
    /// </summary>
    /// <remarks>Since this is symmetric operation, it doesn't really matter if you use Encrypt or Decrypt method</remarks>
    /// <param name="output">Output byte array</param>
    /// <param name="input">Input byte array</param>
    /// <param name="numBytes">Number of bytes to decrypt</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    public void DecryptBytes(byte[] output, byte[] input, int numBytes, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (output == null)
        {
            throw new ArgumentNullException("output", "Output cannot be null");
        }

        if (input == null)
        {
            throw new ArgumentNullException("input", "Input cannot be null");
        }

        if (numBytes < 0 || numBytes > input.Length)
        {
            throw new ArgumentOutOfRangeException("numBytes", "The number of bytes to read must be between [0..input.Length]");
        }

        if (output.Length < numBytes)
        {
            throw new ArgumentOutOfRangeException("output", $"Output byte array should be able to take at least {numBytes}");
        }

        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        WorkBytes(output, input, numBytes, simdMode);
    }

    /// <summary>
    /// Decrypt arbitrary-length byte stream (input), writing the resulting bytes to another stream (output)
    /// </summary>
    /// <param name="output">Output stream</param>
    /// <param name="input">Input stream</param>
    /// <param name="howManyBytesToProcessAtTime">How many bytes to read and write at time, default is 1024</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    public void DecryptStream(Stream output, Stream input, int howManyBytesToProcessAtTime = 1024, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        WorkStreams(output, input, simdMode, howManyBytesToProcessAtTime);
    }

    /// <summary>
    /// Async decrypt arbitrary-length byte stream (input), writing the resulting bytes to another stream (output)
    /// </summary>
    /// <param name="output">Output stream</param>
    /// <param name="input">Input stream</param>
    /// <param name="howManyBytesToProcessAtTime">How many bytes to read and write at time, default is 1024</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    public async Task DecryptStreamAsync(Stream output, Stream input, int howManyBytesToProcessAtTime = 1024, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        await WorkStreamsAsync(output, input, simdMode, howManyBytesToProcessAtTime);
    }

    /// <summary>
    /// Decrypt arbitrary-length byte array (input), writing the resulting byte array to preallocated output buffer.
    /// </summary>
    /// <remarks>Since this is symmetric operation, it doesn't really matter if you use Encrypt or Decrypt method</remarks>
    /// <param name="output">Output byte array, must have enough bytes</param>
    /// <param name="input">Input byte array</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    public void DecryptBytes(byte[] output, byte[] input, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (output == null)
        {
            throw new ArgumentNullException("output", "Output cannot be null");
        }

        if (input == null)
        {
            throw new ArgumentNullException("input", "Input cannot be null");
        }

        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        WorkBytes(output, input, input.Length, simdMode);
    }

    /// <summary>
    /// Decrypt arbitrary-length byte array (input), writing the resulting byte array that is allocated by method.
    /// </summary>
    /// <remarks>Since this is symmetric operation, it doesn't really matter if you use Encrypt or Decrypt method</remarks>
    /// <param name="input">Input byte array</param>
    /// <param name="numBytes">Number of bytes to encrypt</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    /// <returns>Byte array that contains decrypted bytes</returns>
    public byte[] DecryptBytes(byte[] input, int numBytes, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (input == null)
        {
            throw new ArgumentNullException("input", "Input cannot be null");
        }

        if (numBytes < 0 || numBytes > input.Length)
        {
            throw new ArgumentOutOfRangeException("numBytes", "The number of bytes to read must be between [0..input.Length]");
        }

        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        byte[] returnArray = new byte[numBytes];
        WorkBytes(returnArray, input, numBytes, simdMode);
        return returnArray;
    }

    /// <summary>
    /// Decrypt arbitrary-length byte array (input), writing the resulting byte array that is allocated by method.
    /// </summary>
    /// <remarks>Since this is symmetric operation, it doesn't really matter if you use Encrypt or Decrypt method</remarks>
    /// <param name="input">Input byte array</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    /// <returns>Byte array that contains decrypted bytes</returns>
    public byte[] DecryptBytes(byte[] input, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (input == null)
        {
            throw new ArgumentNullException("input", "Input cannot be null");
        }

        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        byte[] returnArray = new byte[input.Length];
        WorkBytes(returnArray, input, input.Length, simdMode);
        return returnArray;
    }

    /// <summary>
    /// Decrypt UTF8 byte array to string.
    /// </summary>
    /// <remarks>Here you can NOT swap encrypt and decrypt methods, because of bytes-string transform</remarks>
    /// <param name="input">Byte array</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    /// <returns>Byte array that contains encrypted bytes</returns>
    public string DecryptUTF8ByteArray(byte[] input, SimdMode simdMode = SimdMode.AutoDetect)
    {
        if (input == null)
        {
            throw new ArgumentNullException("input", "Input cannot be null");
        }

        if (simdMode == SimdMode.AutoDetect)
        {
            simdMode = DetectSimdMode();
        }

        byte[] tempArray = new byte[input.Length];

        WorkBytes(tempArray, input, input.Length, simdMode);
        return System.Text.Encoding.UTF8.GetString(tempArray);
    }

    #endregion // Decryption methods

    private void WorkStreams(Stream output, Stream input, SimdMode simdMode, int howManyBytesToProcessAtTime = 1024)
    {
        int readBytes;

        byte[] inputBuffer = new byte[howManyBytesToProcessAtTime];
        byte[] outputBuffer = new byte[howManyBytesToProcessAtTime];

        while ((readBytes = input.Read(inputBuffer, 0, howManyBytesToProcessAtTime)) > 0)
        {
            // Encrypt or decrypt
            WorkBytes(output: outputBuffer, input: inputBuffer, numBytes: readBytes, simdMode);

            // Write buffer
            output.Write(outputBuffer, 0, readBytes);
        }
    }

    private async Task WorkStreamsAsync(Stream output, Stream input, SimdMode simdMode, int howManyBytesToProcessAtTime = 1024)
    {
        byte[] readBytesBuffer = new byte[howManyBytesToProcessAtTime];
        byte[] writeBytesBuffer = new byte[howManyBytesToProcessAtTime];
        int howManyBytesWereRead = await input.ReadAsync(readBytesBuffer, 0, howManyBytesToProcessAtTime);

        while (howManyBytesWereRead > 0)
        {
            // Encrypt or decrypt
            WorkBytes(output: writeBytesBuffer, input: readBytesBuffer, numBytes: howManyBytesWereRead, simdMode);

            // Write
            await output.WriteAsync(writeBytesBuffer, 0, howManyBytesWereRead);

            // Read more
            howManyBytesWereRead = await input.ReadAsync(readBytesBuffer, 0, howManyBytesToProcessAtTime);
        }
    }

    /// <summary>
    /// Encrypt or decrypt an arbitrary-length byte array (input), writing the resulting byte array to the output buffer. The number of bytes to read from the input buffer is determined by numBytes.
    /// </summary>
    /// <param name="output">Output byte array</param>
    /// <param name="input">Input byte array</param>
    /// <param name="numBytes">How many bytes to process</param>
    /// <param name="simdMode">Chosen SIMD mode (default is auto-detect)</param>
    private void WorkBytes(byte[] output, byte[] input, int numBytes, SimdMode simdMode)
    {
        if (isDisposed)
        {
            throw new ObjectDisposedException("state", "The ChaCha state has been disposed");
        }

        uint[] x = new uint[stateLength];    // Working buffer
        byte[] tmp = new byte[processBytesAtTime];  // Temporary buffer
        int offset = 0;

        int howManyFullLoops = numBytes / processBytesAtTime;
        int tailByteCount = numBytes - howManyFullLoops * processBytesAtTime;

        for (int loop = 0; loop < howManyFullLoops; loop++)
        {
            UpdateStateAndGenerateTemporaryBuffer(state, x, tmp);

            if (simdMode == SimdMode.V512)
            {
                // 1 x 64 bytes
                Vector512<byte> inputV = Vector512.Create(input, offset);
                Vector512<byte> tmpV = Vector512.Create(tmp, 0);
                Vector512<byte> outputV = inputV ^ tmpV;
                outputV.CopyTo(output, offset);
            }
            else if (simdMode == SimdMode.V256)
            {
                // 2 x 32 bytes
                Vector256<byte> inputV = Vector256.Create(input, offset);
                Vector256<byte> tmpV = Vector256.Create(tmp, 0);
                Vector256<byte> outputV = inputV ^ tmpV;
                outputV.CopyTo(output, offset);

                inputV = Vector256.Create(input, offset + 32);
                tmpV = Vector256.Create(tmp, 32);
                outputV = inputV ^ tmpV;
                outputV.CopyTo(output, offset + 32);
            }
            else if (simdMode == SimdMode.V128)
            {
                // 4 x 16 bytes
                Vector128<byte> inputV = Vector128.Create(input, offset);
                Vector128<byte> tmpV = Vector128.Create(tmp, 0);
                Vector128<byte> outputV = inputV ^ tmpV;
                outputV.CopyTo(output, offset);

                inputV = Vector128.Create(input, offset + 16);
                tmpV = Vector128.Create(tmp, 16);
                outputV = inputV ^ tmpV;
                outputV.CopyTo(output, offset + 16);

                inputV = Vector128.Create(input, offset + 32);
                tmpV = Vector128.Create(tmp, 32);
                outputV = inputV ^ tmpV;
                outputV.CopyTo(output, offset + 32);

                inputV = Vector128.Create(input, offset + 48);
                tmpV = Vector128.Create(tmp, 48);
                outputV = inputV ^ tmpV;
                outputV.CopyTo(output, offset + 48);
            }
            else
            {
                for (int i = 0; i < processBytesAtTime; i += 4)
                {
                    // Small unroll
                    int start = i + offset;
                    output[start] = (byte)(input[start] ^ tmp[i]);
                    output[start + 1] = (byte)(input[start + 1] ^ tmp[i + 1]);
                    output[start + 2] = (byte)(input[start + 2] ^ tmp[i + 2]);
                    output[start + 3] = (byte)(input[start + 3] ^ tmp[i + 3]);
                }
            }

            offset += processBytesAtTime;
        }

        // In case there are some bytes left
        if (tailByteCount > 0)
        {
            UpdateStateAndGenerateTemporaryBuffer(state, x, tmp);

            for (int i = 0; i < tailByteCount; i++)
            {
                output[i + offset] = (byte)(input[i + offset] ^ tmp[i]);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void UpdateStateAndGenerateTemporaryBuffer(uint[] stateToModify, uint[] workingBuffer, byte[] temporaryBuffer)
    {
        // Copy state to working buffer
        Buffer.BlockCopy(stateToModify, 0, workingBuffer, 0, stateLength * sizeof(uint));

        for (int i = 0; i < 10; i++)
        {
            QuarterRound(workingBuffer, 0, 4, 8, 12);
            QuarterRound(workingBuffer, 1, 5, 9, 13);
            QuarterRound(workingBuffer, 2, 6, 10, 14);
            QuarterRound(workingBuffer, 3, 7, 11, 15);

            QuarterRound(workingBuffer, 0, 5, 10, 15);
            QuarterRound(workingBuffer, 1, 6, 11, 12);
            QuarterRound(workingBuffer, 2, 7, 8, 13);
            QuarterRound(workingBuffer, 3, 4, 9, 14);
        }

        for (int i = 0; i < stateLength; i++)
        {
            Util.ToBytes(temporaryBuffer, Util.Add(workingBuffer[i], stateToModify[i]), 4 * i);
        }

        stateToModify[12] = Util.AddOne(stateToModify[12]);
        if (stateToModify[12] <= 0)
        {
            /* Stopping at 2^70 bytes per nonce is the user's responsibility */
            stateToModify[13] = Util.AddOne(stateToModify[13]);
        }
    }

    /// <summary>
    /// The ChaCha Quarter Round operation. It operates on four 32-bit unsigned integers within the given buffer at indices a, b, c, and d.
    /// </summary>
    /// <remarks>
    /// The ChaCha state does not have four integer numbers: it has 16. So the quarter-round operation works on only four of them -- hence the name. Each quarter round operates on four predetermined numbers in the ChaCha state.
    /// See <a href="https://tools.ietf.org/html/rfc7539#page-4">ChaCha20 Spec Sections 2.1 - 2.2</a>.
    /// </remarks>
    /// <param name="x">A ChaCha state (vector). Must contain 16 elements.</param>
    /// <param name="a">Index of the first number</param>
    /// <param name="b">Index of the second number</param>
    /// <param name="c">Index of the third number</param>
    /// <param name="d">Index of the fourth number</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void QuarterRound(uint[] x, uint a, uint b, uint c, uint d)
    {
        x[a] = Util.Add(x[a], x[b]);
        x[d] = Util.Rotate(Util.XOr(x[d], x[a]), 16);

        x[c] = Util.Add(x[c], x[d]);
        x[b] = Util.Rotate(Util.XOr(x[b], x[c]), 12);

        x[a] = Util.Add(x[a], x[b]);
        x[d] = Util.Rotate(Util.XOr(x[d], x[a]), 8);

        x[c] = Util.Add(x[c], x[d]);
        x[b] = Util.Rotate(Util.XOr(x[b], x[c]), 7);
    }

    #region Destructor and Disposer

    /// <summary>
    /// Clear and dispose of the internal state. The finalizer is only called if Dispose() was never called on this cipher.
    /// </summary>
    ~CSChaCha20()
    {
        Dispose(false);
    }

    /// <summary>
    /// Clear and dispose of the internal state. Also request the GC not to call the finalizer, because all cleanup has been taken care of.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        /*
			* The Garbage Collector does not need to invoke the finalizer because Dispose(bool) has already done all the cleanup needed.
			*/
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// This method should only be invoked from Dispose() or the finalizer. This handles the actual cleanup of the resources.
    /// </summary>
    /// <param name="disposing">
    /// Should be true if called by Dispose(); false if called by the finalizer
    /// </param>
    private void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                /* Cleanup managed objects by calling their Dispose() methods */
            }

            /* Cleanup any unmanaged objects here */
            Array.Clear(state, 0, stateLength);
        }

        isDisposed = true;
    }

    #endregion // Destructor and Disposer
}

/// <summary>
/// Utilities that are used during compression
/// </summary>
public static class Util
{
    /// <summary>
    /// n-bit left rotation operation (towards the high bits) for 32-bit integers.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="c"></param>
    /// <returns>The result of (v LEFTSHIFT c)</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Rotate(uint v, int c)
    {
        unchecked
        {
            return v << c | v >> 32 - c;
        }
    }

    /// <summary>
    /// Unchecked integer exclusive or (XOR) operation.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="w"></param>
    /// <returns>The result of (v XOR w)</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint XOr(uint v, uint w)
    {
        return unchecked(v ^ w);
    }

    /// <summary>
    /// Unchecked integer addition. The ChaCha spec defines certain operations to use 32-bit unsigned integer addition modulo 2^32.
    /// </summary>
    /// <remarks>
    /// See <a href="https://tools.ietf.org/html/rfc7539#page-4">ChaCha20 Spec Section 2.1</a>.
    /// </remarks>
    /// <param name="v"></param>
    /// <param name="w"></param>
    /// <returns>The result of (v + w) modulo 2^32</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Add(uint v, uint w)
    {
        return unchecked(v + w);
    }

    /// <summary>
    /// Add 1 to the input parameter using unchecked integer addition. The ChaCha spec defines certain operations to use 32-bit unsigned integer addition modulo 2^32.
    /// </summary>
    /// <remarks>
    /// See <a href="https://tools.ietf.org/html/rfc7539#page-4">ChaCha20 Spec Section 2.1</a>.
    /// </remarks>
    /// <param name="v"></param>
    /// <returns>The result of (v + 1) modulo 2^32</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint AddOne(uint v)
    {
        return unchecked(v + 1);
    }

    /// <summary>
    /// Convert four bytes of the input buffer into an unsigned 32-bit integer, beginning at the inputOffset.
    /// </summary>
    /// <param name="p"></param>
    /// <param name="inputOffset"></param>
    /// <returns>An unsigned 32-bit integer</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint U8To32Little(byte[] p, int inputOffset)
    {
        unchecked
        {
            return p[inputOffset]
                | (uint)p[inputOffset + 1] << 8
                | (uint)p[inputOffset + 2] << 16
                | (uint)p[inputOffset + 3] << 24;
        }
    }

    /// <summary>
    /// Serialize the input integer into the output buffer. The input integer will be split into 4 bytes and put into four sequential places in the output buffer, starting at the outputOffset.
    /// </summary>
    /// <param name="output"></param>
    /// <param name="input"></param>
    /// <param name="outputOffset"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ToBytes(byte[] output, uint input, int outputOffset)
    {
        unchecked
        {
            output[outputOffset] = (byte)input;
            output[outputOffset + 1] = (byte)(input >> 8);
            output[outputOffset + 2] = (byte)(input >> 16);
            output[outputOffset + 3] = (byte)(input >> 24);
        }
    }
}