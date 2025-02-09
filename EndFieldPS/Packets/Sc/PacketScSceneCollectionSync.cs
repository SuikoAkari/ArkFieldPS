using EndFieldPS.Network;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EndFieldPS.Resource;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScSceneCollectionSync : Packet
    {

        public PacketScSceneCollectionSync(Player client) {

            ScSceneCollectionSync proto = new ScSceneCollectionSync()
            {
                CollectionList =
                {

                },
                
            };

            foreach (var item in ResourceManager.levelDatas)
            {
                foreach (var item1 in ResourceManager.collectionTable)
                {
                    proto.CollectionList.Add(new SceneCollection()
                    {
                        Count = 1,
                        PrefabId = item1.Value.prefabId,
                        SceneName = item.id,
                        
                    });
                }

            }

            SetData(ScMessageId.ScSceneCollectionSync, proto);
        }

    }
}
