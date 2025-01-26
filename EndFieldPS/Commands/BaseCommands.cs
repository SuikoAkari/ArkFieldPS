using EndFieldPS.Database;
using EndFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Commands
{
    public static class BaseCommands
    {

        [Server.Command("scene", "Change scene",true)]
        public static void SceneCmd(string cmd, string[] args, Player target)
        {
            if (args.Length < 1) return;
            int sceneNumId = int.Parse(args[0]);
            target.EnterScene(sceneNumId);

        }
        [Server.Command("target", "Set a target uid", false)]
        public static void TargetCmd(string cmd, string[] args, Player target)
        {
            if (args.Length < 1)
            {
                Logger.PrintError("Use: /target (uid)");
                return;
            }
            string id = args[0];
            Player player = Server.clients.Find(c=>c.accountId == id);
            if (player == null)
            {
                Logger.PrintError("Only online players can be set as target");
                return;
            }
            CommandManager.targetId = id;
            Logger.Print("Set Target player to "+id);
        }
        [Server.Command("account", "account command")]
        public static void AccountCmd(string cmd, string[] args, Player target)
        {
            if (args.Length < 2) return;
            switch (args[0])
            {
                case "create":
                    DatabaseManager.db.CreateAccount(args[1]);
                    break;
                default:
                    Logger.Print("Example: account create (username)");
                    break;
            }
        }
        [Server.Command("spawn", "Spawn cmd test")]
        public static void SpawnCmd(string cmd, string[] args, Player target)
        {
            if (args.Length < 1) return;
            string templateId = args[0];

            foreach (var item in Server.clients)
            {

                ScObjectEnterView info = new()
                {
                    
                    
                    Detail =new()
                    {
                        
                        TeamIndex = item.teamIndex,
                        
                        MonsterList =
                        {
                            new SceneMonster()
                            {
                                Level=5,
                                CommonInfo = new()
                                {
                                    Hp=100,
                                    Id=item.random.Next(),
                                    Templateid=templateId,
                                    
                                    SceneNumId=item.curSceneNumId,
                                    Position=item.position.ToProto(),
                                    
                                },
                                
                                BattleInfo = new()
                                {
                                    
                                },
                                
                            }
                        },
                        
                    },
                };
                enemyAttributeTemplateTable[templateId].levelDependentAttributes[5].attrs.ForEach(attr =>
                {
                    info.Detail.MonsterList[0].Attrs.Add(new AttrInfo()
                    {
                        AttrType = attr.attrType,
                        BasicValue = 0,
                        Value = attr.attrValue

                    });

                });
                enemyAttributeTemplateTable[templateId].levelIndependentAttributes.attrs.ForEach(attr =>
                {
                    info.Detail.MonsterList[0].Attrs.Add(new AttrInfo()
                    {
                        AttrType = attr.attrType,
                        BasicValue = 0,
                        Value = attr.attrValue

                    });

                });
                item.Send(ScMessageId.ScObjectEnterView, info);
                
                item.Send(ScMessageId.ScSpawnEnemy, new ScSpawnEnemy() 
                {
                    EnemyInstIds =
                    {
                        info.Detail.MonsterList[0].CommonInfo.Id
                    },
                    
                    
                });
                
            }
        }
    }
}