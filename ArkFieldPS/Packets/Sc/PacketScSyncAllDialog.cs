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
    public class PacketScSyncAllDialog : Packet
    {
        public PacketScSyncAllDialog(Player player) {
            ScSyncAllDialog dialogues = new()
            {

            };
            foreach (var item in ResourceManager.dialogIdTable.dialogStrToNum)
            {
                try
                {
                    dialogues.DialogList.Add(new Dialog()
                    {
                        DialogId = item.Value,

                    });
                }
                catch (Exception ex)
                {

                }

            }

            SetData(ScMessageId.ScSyncAllDialog, dialogues);
        }
    }
}
