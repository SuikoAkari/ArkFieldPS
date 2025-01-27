using EndFieldPS.Resource;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.IdGenerators;

namespace EndFieldPS.Game
{
    public class Mail
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId _id { get; set; }
        public ulong owner;
        public long sendTime = 0;
        public long expireTime = 0;
        public ulong guid = 0;
        public bool isStar = false;
        public bool claimed = false;
        public bool isRead = false;
        public MailType mailType;
        public MailSubType mailSubType;
        public Mail_Content content;
        //todo add rewards
        public Mail() { }


        public CsMailDef ToProto()
        {
            return new CsMailDef()
            {
                ExpireTime = expireTime,
                MailId=guid,
                IsAttachmentGot=claimed,
                IsRead=isRead,
                IsStar=isStar,
                MailType=(int)mailType,
                MailSubType=(int)mailSubType,
                SendTime=sendTime,
                MailContent = new()
                {
                    Content=content.content,
                    SenderName=content.senderName,
                    Title=content.title,
                    TemplateId=content.templateId,
                    SenderIcon= "Mail/mail_endfield"
                }
            };
        }
    }

    public struct Mail_Content
    {
        public string templateId = "";
        public string title = "";
        public string content = "";
        public string senderName = "";

        public Mail_Content() { }
    }
}
