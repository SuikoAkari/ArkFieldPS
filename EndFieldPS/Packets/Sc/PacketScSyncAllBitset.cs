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
                            /*18302558512577773568,
                            10351514792732131327,
                            732826689654224995,
                            5453459960836881971,
                            2259954429851840863,
                            14082403297859863210,
                            321385*/
                            ResourceManager.ToBitsetValue(ResourceManager.CalculateVoiceIdsBitset())
                        }
                    },
                    new BitsetData()
                    {
                        Type=(int)BitsetType.CharDoc,

                        Value =
                        {
                            /*651555471184378880,
                            7572302140*/
                             ResourceManager.ToBitsetValue(ResourceManager.CalculateDocsIdsBitset())
                        }
                    },
                    new BitsetData()
                    {
                        Type=(int)BitsetType.LevelMapFirstView,

                        Value =
                        {
                            51810140172
                        }
                    },
                    new BitsetData()
                    {
                        Type=(int)BitsetType.AreaToastOnce,

                        Value =
                        {
                            9745508118585934080,
                            2531075783884815
                        }
                    },
                    new BitsetData()
                    {
                        Type=(int)BitsetType.NewSceneGradeUnlocked,

                        Value =
                        {
                            8
                        }
                    },
                    new BitsetData()
                    {
                        Type=(int)BitsetType.MapFilter,

                        Value =
                        {

                        }
                    },
                    new BitsetData()
                    {
                        Type=(int)BitsetType.LevelHaveBeen,

                        Value =
                        {
                            51810140172,
                            531424821771251712,
                            590604267522,
                            262144
                         //ResourceManager.ToBitsetValue(ResourceManager.CalculateChapterIdsBitset())
                        }
                    },
                    new BitsetData()
                    {
                        Type=(int)BitsetType.InteractiveActive,

                        Value =
                        {
                            503839996,
                            0,
                            17042430230528,
                            17180393728
                        }
                    },
                    new BitsetData()
                    {
                        Type=(int)BitsetType.NewChar,

                        Value =
                        {
                            0
                        }
                    },
                }
            };
           
          /*  foreach (var item in strIdNumTable.char_voice_id.dic)
            {
                bitset.Bitset[0].Value.Add((ulong)item.Value);
                bitset.Bitset[0].Value.Add((ulong)0);
            }
            foreach (var item in strIdNumTable.char_doc_id.dic)
            {
                bitset.Bitset[1].Value.Add((ulong)item.Value);
            }*/
            SetData(ScMessageId.ScSyncAllBitset, bitset);
        }

    }
}
