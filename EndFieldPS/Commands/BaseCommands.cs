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
        [Server.Command("help", "Show list of commands", false)]
        public static void HelpCmd(string cmd, string[] args, Player target)
        {
            Logger.Print("List of possible commands: ");
            foreach(var command in CommandManager.s_notifyReqGroup)
            {
                Logger.Print($"/{command.Key} - {command.Value.Item1.desc} (Require Target: {command.Value.Item1.requiredTarget})");
            }

        }
        
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
        [Server.Command("spawn", "Spawn cmd test",true)]
        public static void SpawnCmd(string cmd, string[] args, Player target)
        {
            if (args.Length < 1) return;
            string templateId = args[0];

                ScObjectEnterView info = new()
                {
                    Detail =new()
                    {
                        
                        
                        MonsterList =
                        {
                            new SceneMonster()
                            {
                                Level=36,
                                CommonInfo = new()
                                {
                                    Hp=3000,
                                    Id=4755837084444680198,
                                    Templateid=templateId,
                                    
                                    SceneNumId=target.curSceneNumId,
                                    Position=target.position.ToProto(),
                                    Type=3,
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
                        BasicValue = attr.attrValue,
                        Value = attr.attrValue

                    });

                });
                enemyAttributeTemplateTable[templateId].levelIndependentAttributes.attrs.ForEach(attr =>
                {
                    info.Detail.MonsterList[0].Attrs.Add(new AttrInfo()
                    {
                        AttrType = attr.attrType,
                        BasicValue = attr.attrValue,
                        Value = attr.attrValue

                    });

                });
            for(int i=4; i < 61; i++)
            {
                info.Detail.MonsterList[0].Attrs.Add(new AttrInfo()
                {
                    AttrType = i,
                    BasicValue = 0,
                    Value = 0

                });
            }
            target.Send(ScMessageId.ScObjectEnterView, info);

            target.Send(ScMessageId.ScSpawnEnemy, new ScSpawnEnemy()
            {
                ClientKey=4,
                EnemyInstIds = { info.Detail.MonsterList[0].CommonInfo.Id }
            });
            
        }
    }
}