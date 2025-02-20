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

            LongBitSet waypointsSet = new LongBitSet(ResourceManager.GetAllShortIds());
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
                            waypointsSet.Bits
                            /*503839996,
                            0,
                            17042430230528,
                            17180393728*/
                           // ResourceManager.GetBitset(ResourceManager.GetAllShortIds())
                          // ResourceManager.ToLongBitsetValue(ResourceManager.CalculateWaypointIdsBitset())
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
                            ResourceManager.ToLongBitsetValue(ResourceManager.CalculateBitsets(ResourceManager.strIdNumTable.wiki_id.dic.Values.ToList()))
                        }
                    },
                }
            };
           
            SetData(ScMessageId.ScSyncAllBitset, bitset);
        }

    }
}
