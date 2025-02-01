using EndFieldPS.Database;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Game.Gacha
{
    public class GachaManager
    {

        public Player player;
        const double fiftyfifty = 0.45; // 50% (make it less than real 50, because the randomness make win fifty fifty every time

        private static readonly Random random = new Random();
        public GachaManager(Player player)
        {
            this.player = player;
        }

        public (int fiveStarPity, int sixStarPity, GachaTransaction? lastSixStar, bool isFiftyFiftyLost) GetCurrentPity(string templateId)
        {
            List<GachaTransaction> transactionList = DatabaseManager.db.LoadGachaTransaction(player.roleId, templateId);
            transactionList = transactionList.OrderBy(g => g.transactionTime).ToList();

            int fiveStarPity = 0;
            int sixStarPity = 0;

            GachaTransaction? lastSixStar = null;
            foreach (var transaction in transactionList)
            {
                if (transaction.rarity == 5)
                {
                    fiveStarPity = 0;
                    sixStarPity++;
                }
                else if (transaction.rarity == 6)
                {
                    fiveStarPity = 0;
                    sixStarPity = 0;
                    lastSixStar = transaction; 
                }
                else
                {
                    fiveStarPity++;
                    sixStarPity++;
                }
                //Logger.Print("Current calculated: " + sixStarPity);
            }

            bool isFiftyFiftyLost = false;
            if (lastSixStar != null)
            {
                isFiftyFiftyLost = lastSixStar.itemId != templateId;
            }

            return (fiveStarPity, sixStarPity, lastSixStar, isFiftyFiftyLost);
        }
        public void DoGacha(string gachaId,int attempts)
        {
            const double prob6Star = 0.008; // 0.8%
            const double prob5Star = 0.08;  // 8%
           
            (int fiveStarPity, int sixStarPity, GachaTransaction? lastSixStar, bool isFiftyFiftyLost) 
                PityInfo = GetCurrentPity(gachaId);
            int increaseTime = 0;
            int pityforcalculate = PityInfo.sixStarPity-64;
            if(pityforcalculate < 1)
            {
                pityforcalculate = 0;
            }
            GachaCharPoolTable table = ResourceManager.gachaCharPoolTable[gachaId];
            GachaCharPoolContentTable content = ResourceManager.gachaCharPoolContentTable[gachaId];
            GachaCharPoolTypeTable type = ResourceManager.gachaCharPoolTypeTable[""+table.type];
            //Sanity check
            if (table==null || content == null || type==null)
            {
                return;
            }
            List<GachaCharPoolItem> fiveStars = content.list.FindAll(c => c.starLevel == 5);
            List<GachaCharPoolItem> sixStars = content.list.FindAll(c => c.starLevel == 6);
            List<GachaCharPoolItem> fourStars = content.list.FindAll(c => c.starLevel == 4);
            List<GachaTransaction> transactions = new();
            for (int i = 0; i < attempts; i++)
            {
                double roll = random.NextDouble();
                double fifty = random.NextDouble();
                double finalProb6Star = prob6Star + 0.05f * pityforcalculate;
                PityInfo.fiveStarPity++;
                PityInfo.sixStarPity++;
                GachaTransaction transaction = null;
                //Six star pull
                if (roll < finalProb6Star || PityInfo.sixStarPity>=type.softGuarantee)
                {
                    PityInfo.sixStarPity -= PityInfo.sixStarPity >= type.softGuarantee ? type.softGuarantee : PityInfo.sixStarPity;
                    if (table.upCharIds.Count > 0)
                    {
                        transaction = GetChar(table.upCharIds[0], PityInfo.isFiftyFiftyLost, fifty, sixStars, 6);
                        
                        if (transaction.itemId != table.upCharIds[0])
                        {
                            PityInfo.isFiftyFiftyLost = true;
                        }
                        else
                        {
                            PityInfo.isFiftyFiftyLost = false;
                        }
                    }
                    else
                    {
                        transaction = GetChar("", PityInfo.isFiftyFiftyLost, fifty, sixStars, 6);
                    }
                    pityforcalculate = 0;
                   
                    
                }
                else if (roll < prob5Star || PityInfo.fiveStarPity >= 10)
                {

                    PityInfo.fiveStarPity -= PityInfo.fiveStarPity >= 10 ? 10 : PityInfo.fiveStarPity;
                    
                    if (table.upCharIds.Count > 1)
                    {
                        transaction = GetChar(table.upCharIds[1], false, fifty, fiveStars, 5);

                    }
                    else
                    {
                        transaction = GetChar("", false, fifty, fiveStars, 5);
                    }
                }
                else
                {
                    transaction = GetChar("", false, fifty, fourStars, 4);
                }
                if(PityInfo.sixStarPity > 65)
                {
                    pityforcalculate++;
                }
                transactions.Add(transaction);
            }
            ScGachaSyncPullResult result = new ScGachaSyncPullResult()
            {
                GachaPoolId = gachaId,
                GachaType =table.type,
                
                OriResultIds =
                {
                },
                Star5GotCount = transactions.FindAll(t => t.rarity == 5).Count,
                Star6GotCount = transactions.FindAll(t => t.rarity == 6).Count,
                FinalResults =
                {

                },
                
                UpGotCount = transactions.FindAll(t => table.upCharIds.Contains(t.itemId)).Count,

            };
            foreach (GachaTransaction transaction in transactions)
            {
                transaction.gachaTemplateId = gachaId;
                bool exist = player.chars.Find(c => c.id == transaction.itemId) != null;
                result.OriResultIds.Add(transaction.itemId);
                result.FinalResults.Add(new ScdGachaFinalResult()
                {
                    IsNew = !exist,
                    ItemId = !exist ? transaction.itemId : "item_charpotentialup_" + transaction.itemId,
                    RewardItemId= !exist ? transaction.itemId : "item_charpotentialup_"+ transaction.itemId,
                    RewardIds =
                    {
                        $"reward_{transaction.rarity}starChar_weaponCoin"
                    }
                });
                
                DatabaseManager.db.AddGachaTransaction(transaction);
            }
            player.Send(ScMessageId.ScGachaSyncPullResult, result);
            ScGachaModifyPoolRoleData roleData = new()
            {
                GachaPoolId = gachaId,
                GachaType = table.type,
                GachaPoolCategoryRoleData = new()
                {
                    GachaPoolType = table.type,
                    Star5SoftGuaranteeProgress = PityInfo.fiveStarPity,
                    SoftGuaranteeProgress = PityInfo.sixStarPity,
                    TotalPullCount = PityInfo.sixStarPity
                },
                GachaPoolRoleData = new()
                {
                    GachaPoolId = gachaId,
                    HardGuaranteeProgress = PityInfo.sixStarPity,
                    SoftGuaranteeProgress = PityInfo.sixStarPity,

                }
            };
            player.Send(ScMessageId.ScGachaModifyPoolRoleData, roleData);
        }
        public GachaTransaction GetChar(string upChar,bool guaranteed, double fifty, List<GachaCharPoolItem> items, int rarity)
        {
            GachaTransaction transaction = new()
            {
                transactionTime = DateTime.UtcNow.Ticks,
                ownerId = player.roleId,
                rarity = rarity,
            };
            if((fifty >= fiftyfifty || guaranteed) && rarity != 4 && upChar.Length >0)
            {
                transaction.itemId = upChar;

            }
            else
            {
                int index = random.Next(0,items.Count); // Miglior randomizzazione
               //  index = (int)((1 - Math.Pow(random.NextDouble(), 2)) * (items.Count - 1));

                // Se vuoi evitare di prendere spesso i primi 2-3 elementi:
                // index = (int)Math.Pow(random.NextDouble(), 1.5) * items.Count;
                if (index > items.Count-1)
                {
                    index = items.Count-1;
                }
                if(index < 0)
                {
                    index = 0;
                }
                transaction.itemId = items[index].charId;
                transaction.hasLost = transaction.itemId != upChar;
            }
            return transaction;
        }

        public static GachaHistoryAPI GetGachaHistoryPage(PlayerData data, string banner, int p = 1)
        {
            GachaHistoryAPI api = new();
            int pageSize = 5; 

            List<GachaTransaction> transactionList = DatabaseManager.db.LoadGachaTransaction(data.roleId, banner);
            transactionList = transactionList.OrderByDescending(g => g.transactionTime).ToList();
            int maxPages=(int)Math.Ceiling((double)transactionList.Count / pageSize);
            api.maxPages = maxPages;
            api.curPage = p;
            api.transactionList= transactionList.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            return api;
        }
        public class GachaHistoryAPI
        {
            public int maxPages = 0;
            public int curPage = 0;
            public List<GachaTransaction> transactionList;
        }
    }
}
