using EndFieldPS.Network;
using EndFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScSyncAllMail : Packet
    {

        public PacketScSyncAllMail(Player client) {

            ScSyncAllMail proto = new ScSyncAllMail()
            {
                
                
            };
            foreach (var mail in client.mails)
            {
                proto.MailIdList.Add(mail.guid);
            }
            SetData(ScMessageId.ScSyncAllMail, proto);
        }

    }
}
