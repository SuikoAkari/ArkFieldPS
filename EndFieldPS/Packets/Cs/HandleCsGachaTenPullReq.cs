using EndFieldPS.Network;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Packets.Cs
{
    public class HandleCsGachaTenPullReq
    {
        [Server.Handler(CsMessageId.CsGachaSinglePullReq)]
        public static void HandleOnePull(Player session, CsMessageId cmdId, Packet packet)
        {
            CsGachaSinglePullReq req = packet.DecodeBody<CsGachaSinglePullReq>();
            session.gachaManager.DoGacha(req.GachaPoolId, 1);
        }
        [Server.Handler(CsMessageId.CsGachaTenPullReq)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsGachaTenPullReq req = packet.DecodeBody<CsGachaTenPullReq>();
            session.gachaManager.DoGacha(req.GachaPoolId, 10);
           /* Random rng = new Random();
            List<string> chars = new List<string>();
            const double prob6Star = 0.008; // 0.8%
            const double prob5Star = 0.08;  // 8%
            const double fiftyfifty = 0.50; // 50%
            GachaCharPoolTable table = ResourceManager.gachaCharPoolTable[req.GachaPoolId];
            GachaCharPoolContentTable content = ResourceManager.gachaCharPoolContentTable[req.GachaPoolId];
            int sixstarcount = 0;
            int fivestarcount = 0;
            List<GachaCharPoolItem> fiveStars = content.list.FindAll(c => c.starLevel == 5);
            List<GachaCharPoolItem> sixStars = content.list.FindAll(c => c.starLevel == 6);
            int fiveStarGuaranteedIndex = new Random().Next(9);
            for (int i=0; i < 10; i++)
            {
                double roll = rng.NextDouble();
                double fifty = rng.NextDouble();
                
                if (roll < prob6Star)
                {
                    sixstarcount++;
                    
                    if (table.upCharIds.Count > 0)
                    {
                        if (fifty >= fiftyfifty)
                        {
                            chars.Add(ResourceManager.characterTable[table.upCharIds[0]].charId);
                        }
                        else
                        {
                            chars.Add(sixStars[new Random().Next(sixStars.Count - 1)].charId);
                        }

                    }
                    else
                    {
                        chars.Add(sixStars[new Random().Next(sixStars.Count - 1)].charId);
                    }
                }
                else if (roll < prob6Star + prob5Star || fiveStarGuaranteedIndex == i)
                {
                    fivestarcount++;
                    if(table.upCharIds.Count > 1)
                    {
                        if(fifty >= fiftyfifty)
                        {
                            chars.Add(ResourceManager.characterTable[table.upCharIds[1]].charId);
                        }
                        else
                        {
                            chars.Add(fiveStars[new Random().Next(fiveStars.Count - 1)].charId);
                        }
                        
                    }
                    else
                    {
                        chars.Add(fiveStars[new Random().Next(fiveStars.Count-1)].charId);
                    }
                    
                }
                else
                {
                    chars.Add(ResourceManager.characterTable.Values.ToList().FindAll(c=>c.rarity == 4)[new Random().Next(ResourceManager.characterTable.Values.ToList().FindAll(c => c.rarity == 4).Count - 1)].charId);
                }
                
            }
            ScGachaSyncPullResult result = new ScGachaSyncPullResult()
            {
                GachaPoolId=req.GachaPoolId,
                GachaType=req.GachaType,
                
                OriResultIds =
                {
                },
                Star5GotCount= fivestarcount,
                Star6GotCount= sixstarcount,
                FinalResults =
                {

                },
                UpGotCount= fivestarcount+ sixstarcount,
                
            }; 
            foreach(string ch in chars)
            {
                bool exist = session.chars.Find(c => c.id == ch) != null;
                result.OriResultIds.Add(ch);
                result.FinalResults.Add(new ScdGachaFinalResult()
                {
                    IsNew= !exist,
                    ItemId=ch,

                });
            }


            //session.Send(Packet.EncodePacket((int)CsMessageId.CsGachaTenPullReq, req));
            
            session.Send(ScMessageId.ScGachaSyncPullResult, result); */
            //  session.Send(CsMessageId.CsGachaEnd, new Empty());
            // session.Send(ScMessageId.ScGachaBegin, new Empty());

        }
       
    }
}
