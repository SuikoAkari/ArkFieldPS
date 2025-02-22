using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using static ArkFieldPS.Resource.ResourceManager;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScFactorySync : Packet
    {

        public PacketScFactorySync(Player client) {
            string json = File.ReadAllText("53_ScFactorySync.json");
            ScFactorySync proto = Newtonsoft.Json.JsonConvert.DeserializeObject<ScFactorySync>(json);
            /*ScFactorySync proto = new()
            {
                FormulaMan = new()
                {
                    Unlocked =
                    {
                        "power_diffuser_1"
                    },
                    
                },
                Stt = new()
                {
                    Nodes =
                    {
                    },
                    Layers =
                    {

                    },
                    Packages =
                    {
                        new ScdFactorySttPackage()
                        {
                            Id="tech_group_jinlong",
                            State=1
                        },
                        new ScdFactorySttPackage()
                        {
                            Id="tech_group_tundra",
                            State=1
                        }
                    }
                }
            };
            foreach(var node in facSTTNodeTable)
            {
                proto.Stt.Nodes.Add(new ScdFactorySttNode()
                {
                    Id=node.Value.techId,
                    State=1,
                    
                });
            }
            foreach(var layer in facSTTLayerTable)
            {
                proto.Stt.Layers.Add(new ScdFactorySttLayer()
                {
                    Id = layer.Value.layerId,
                    State = 1,
                });
            }*/

            SetData(ScMessageId.ScFactorySync, proto);
        }

    }
}
