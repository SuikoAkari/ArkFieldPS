using EndFieldPS.Network;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
                            ResourceManager.ToLongBitsetValue(ResourceManager.CalculateVoiceIdsBitset())
                        }
                    },
                    new BitsetData()
                    {
                        Type=(int)BitsetType.CharDoc,

                        Value =
                        {
                             ResourceManager.ToLongBitsetValue(ResourceManager.CalculateDocsIdsBitset())
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
           
            SetData(ScMessageId.ScSyncAllBitset, bitset);
        }

    }
}
