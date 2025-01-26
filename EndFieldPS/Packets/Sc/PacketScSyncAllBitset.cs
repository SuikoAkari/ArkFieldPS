using EndFieldPS.Network;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScSyncAllBitset : Packet
    {

        public PacketScSyncAllBitset(Player client) {

            ScSyncAllBitset bitset = new()
            {
                Bitset =
                {
                    new BitsetData()
                    {
                        Type=(int)BitsetType.CharVoice,
                        
                        Value =
                        {
                            0,
                            0
                        }
                    },
                    new BitsetData()
                    {
                        Type=(int)BitsetType.CharDoc,
                        
                        Value =
                        {
                            0,
                            1500,
                            300
                        }
                    }

                }
            };
            foreach (var item in strIdNumTable.char_voice_id.dic)
            {
                bitset.Bitset[0].Value.Add((ulong)item.Value);
                bitset.Bitset[0].Value.Add((ulong)0);
            }
            foreach (var item in strIdNumTable.char_doc_id.dic)
            {
                bitset.Bitset[1].Value.Add((ulong)item.Value);
            }
            SetData(ScMessageId.ScSyncAllBitset, bitset);
        }

    }
}
