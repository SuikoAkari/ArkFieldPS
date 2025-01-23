using EndFieldPS.Network;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using Google.Protobuf;
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

        [Server.Handler(CsMessageId.CsGachaTenPullReq)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsGachaTenPullReq req = packet.DecodeBody<CsGachaTenPullReq>();
            
            Random rng = new Random();
            List<string> chars = new List<string>();
            const double prob6Star = 0.008; // 0.8%
            const double prob5Star = 0.08;  // 8%
            GachaCharPoolTable table = ResourceManager.gachaCharPoolTable[req.GachaPoolId];
            int sixstarcount = 0;
            int fivestarcount = 0;
            for (int i=0; i < 10; i++)
            {
                double roll = rng.NextDouble();

                if (roll < prob6Star)
                {
                    sixstarcount++;
                    chars.Add(ResourceManager.characterTable[table.upCharIds[0]].charId);
                }
                else if (roll < prob6Star + prob5Star)
                {
                    fivestarcount++;
                    chars.Add(ResourceManager.characterTable[table.upCharIds[1]].charId);
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
                
                Star5GotCount= fivestarcount,
                Star6GotCount= sixstarcount,
                FinalResults =
                {

                },
                UpGotCount=chars.Count,
                
            };
            foreach(string ch in chars)
            {
                result.FinalResults.Add(new ScdGachaFinalResult()
                {
                    IsNew=true,
                    ItemId=ch,
                    RewardItemId=ch,
                    
                });
            }
           
           session.Send(ScMessageId.ScGachaSyncPullResult, result); 
        }
       
    }
}
