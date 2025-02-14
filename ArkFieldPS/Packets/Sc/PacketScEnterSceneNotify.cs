using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScEnterSceneNotify : Packet
    {

        public PacketScEnterSceneNotify(Player client, int sceneNumId = 21, Vector3f pos=null, PassThroughData data=null) {


            ScEnterSceneNotify proto = new ScEnterSceneNotify()
            {
                Position = pos==null ? client.position.ToProto() : pos.ToProto(),
                PassThroughData= data,
                SceneId = client.sceneManager.GetSceneGuid(sceneNumId),
                RoleId = client.roleId,
                SceneNumId = sceneNumId,
                
            };

            SetData(ScMessageId.ScEnterSceneNotify, proto);
        }

    }
}
