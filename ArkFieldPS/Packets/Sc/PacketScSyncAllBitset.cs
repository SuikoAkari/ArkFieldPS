using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScSyncAllBitset : Packet
    {

        public PacketScSyncAllBitset(Player client) {

            ScSyncAllBitset bitset = new()
            {


                Bitset =
                {
                    /*new BitsetData()
                    {
                        Type=(int)BitsetType.CharVoice,

                        Value =
                        {
                            ResourceManager.CalculateVoiceIdsBitset()
                        }
                    },
                    new BitsetData()
                    {
                        Type=(int)BitsetType.CharDoc,
                        Value =
                        {
                             ResourceManager.CalculateDocsIdsBitset()
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
                            531424959210205184,
                            590604267523,
                            17039360
                        }
                    },
                    new BitsetData()
                    {
                        Type=(int)BitsetType.InteractiveActive,
                        Value = 
                        {
                            /*503839996,
                            0,
                            17042430230528,
                            17180393728
                            ResourceManager.CalculateWaypointIdsBitset()
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
                    new BitsetData()
                    {
                        Type=(int)BitsetType.Wiki,
                        Value =
                        {
                            new LongBitSet(ResourceManager.strIdNumTable.wiki_id.dic.Values).Bits
                        }
                    },*/
                }
            };
            foreach (var keyValuePair in client.bitsetManager.bitsets)
            {
                bitset.Bitset.Add(new BitsetData()
                {
                    Type = keyValuePair.Key,
                    Value =
                    {
                        new LongBitSet(keyValuePair.Value).Bits
                    }
                });
            }
            SetData(ScMessageId.ScSyncAllBitset, bitset);
        }

    }
}
