using EndFieldPS.Game.Character;
using EndFieldPS.Game.Entities;
using EndFieldPS.Network;
using EndFieldPS.Protocol;
using Google.Protobuf;
using MongoDB.Driver.Core.Clusters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EndFieldPS.Packets.Cs
{
    public class HandleCsBattleOp
    {
        //TODO AbilityManager
        [Server.Handler(CsMessageId.CsBattleOp)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsBattleOp req = packet.DecodeBody<CsBattleOp>();
            
            foreach (BattleClientOpData data in req.ClientData.OpList)
            {
                switch (data.OpType)
                {
                   
                    case BattleActionOperateType.BattleOpEntityValueModify:
                        OnEntityValueModify(session, data);
                        break;
                    case BattleActionOperateType.BattleOpSkillStartCast:
                        OnSkillStartCast(session, data);
                        break;
                    case BattleActionOperateType.BattleOpSkillEndCast:
                        OnSkillEndCast(session, data);
                        break;
                    case BattleActionOperateType.BattleOpTriggerAction:
                        OnTriggerAction(session, data.TriggerActionOpData);
                        break;
                    case BattleActionOperateType.BattleOpEntityDie:
                        OnEntityDie(session, data.EntityDieOpData);
                        break;
                    default:
                        Logger.PrintWarn($"Unsupported BattleActionOperateType.{data.OpType}");
                        break;
                }
            }

        }

        private static void OnEntityDie(Player session, BattleEntityDieOpData data)
        {
            if (session.sceneManager.GetEntity(data.EntityInstId) != null)
            {
                if (Server.config.logOptions.debugPrint)
                {
                    Logger.PrintWarn("Killed entity with guid: "+data.EntityInstId);
                }
                session.sceneManager.KillEntity(data.EntityInstId);
            }
                
            
        }

        private static void OnTriggerAction(Player session, BattleTriggerActionOpData data)
        {
            
            switch (data.Action.ActionType)
            {
                case ServerBattleActionType.BattleActionDamage:
                    foreach(BattleDamageDetail item in data.Action.DamageAction.Details)
                    {
                        DamageEntity(session, item);
                        
                    }
                    break;
                case ServerBattleActionType.BattleActionHeal:
                    foreach (BattleHealActionDetail item in data.Action.HealAction.Details)
                    {
                        HealEntity(session, item);
                    }
                    break;
                
                default:
                    Logger.PrintWarn($"Unsupported ServerBattleActionType.{data.Action.ActionType}");
                    break;
            }
        }

        public static void HealEntity(Player session, BattleHealActionDetail detail)
        {
            Entity en = session.sceneManager.GetEntity(detail.TargetId);
            if (en != null)
            {
                Logger.Print("Healing +" + detail.Value + "hp");
                en.Heal(detail.Value);
            }
        }
        
        public static void DamageEntity(Player session, BattleDamageDetail detail)
        {
            Entity en=session.sceneManager.GetEntity(detail.TargetId);

            if (en != null)
            {
                en.Damage(detail.Value);
                if (Server.config.logOptions.debugPrint)
                {
                    Logger.PrintWarn("Damaged entity with dmg: "+detail.Value);
                }
            }
        }
        private static void OnSkillStartCast(Player session, BattleClientOpData data)
        {
            ulong casterId = data.OwnerId;
            
            Character character = session.chars.Find(c => c.guid == casterId);
            if (character != null)
            {
                ScCharSyncStatus s = new()
                {
                    BattleInfo = new()
                    {
                        Hp = character.curHp,
                        Ultimatesp = character.ultimateSp
                    },
                    Objid=character.guid,
                    
                };
                
                session.Send(ScMessageId.ScCharSyncStatus, s);
            }
            else
            {
                //Manage normal entity
            }
        }
        private static void OnSkillEndCast(Player session, BattleClientOpData data)
        {
            ulong casterId = data.OwnerId;
            
            Character character = session.chars.Find(c => c.guid == casterId);
            if (character != null)
            {
                ScCharSyncStatus s = new()
                {
                    BattleInfo = new()
                    {
                        Hp = character.curHp,
                        Ultimatesp = character.ultimateSp+1
                    },
                    Objid = character.guid,
                };
                session.Send(ScMessageId.ScCharSyncStatus, s);
            }
            else
            {
                //Manage normal entity
            }
        }

        private static void OnEntityValueModify(Player session, BattleClientOpData data)
        {
            Logger.PrintWarn("EntityValueModify called: " + data.EntityValueModifyData.ToString());
            
            Character character = session.chars.Find(c => c.guid == data.EntityValueModifyData.EntityInstId);
            if (character != null)
            {
                character.curHp = data.EntityValueModifyData.Value.Hp;
                ScCharSyncStatus s = new()
                {
                    BattleInfo = new()
                    {
                        Hp = data.EntityValueModifyData.Value.Hp,
                        Ultimatesp = character.ultimateSp
                    },
                    Objid = character.guid,
                };
                session.Send(ScMessageId.ScCharSyncStatus, s);
            }
            else
            {
                //Manage normal entity
            }
            // data.EntityValueModifyData.
        }
    }
}