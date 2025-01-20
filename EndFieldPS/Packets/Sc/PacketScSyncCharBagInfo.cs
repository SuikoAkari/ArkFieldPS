using EndFieldPS.Network;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScSyncCharBagInfo : Packet
    {

        public PacketScSyncCharBagInfo(Client client) {

            ScSyncCharBagInfo proto = new()
            {
                ScopeName=1,
                
                CharInfo =
                {
                    new CharInfo()
                    {
                        Objid=1,
                        Templateid="chr_0003_endminf",
                        Level=1,
                        WeaponId=4611757057225326608,
                        NormalSkill="chr_0003_endminf_NormalSkill",
                        
                        CharType=CharType.DefaultType,
                        BattleInfo = new()
                        {
                            Hp=ResourceManager.characterTable["chr_0003_endminf"].attributes[1].Attribute.attrs.Find(A=>A.attrType==(int)AttributeType.MaxHp).attrValue,
                            
                        },
                        BattleMgrInfo = new()
                        {
                           
                        },
            
                        OwnTime=1737205147,
                        Talent = new()
                        {
                            
                        },
                        SkillInfo = new()
                        {
                            NormalSkill="chr_0003_endminf_NormalSkill",
                            ComboSkill="chr_0003_endminf_ComboSkill",
                            UltimateSkill="chr_0003_endminf_UltimateSkill",
                            DispNormalAttackSkill="chr_0003_endminf_NormalAttack",
                            LevelInfo = 
                            {
                                new SkillLevelInfo()
                                {
                                    SkillId="chr_0003_endminf_NormalAttack",
                                    SkillLevel=1,
                                    SkillMaxLevel=1,
                                    SkillEnhancedLevel=1
                                },
                                new SkillLevelInfo()
                                {
                                    SkillId="chr_0003_endminf_NormalSkill",
                                    SkillLevel=1,
                                    SkillMaxLevel=1,
                                    SkillEnhancedLevel=1
                                },
                                new SkillLevelInfo()
                                {
                                    SkillId="chr_0003_endminf_UltimateSkill",
                                    SkillLevel=1,
                                    SkillMaxLevel=1,
                                    SkillEnhancedLevel=1
                                },
                                new SkillLevelInfo()
                                {
                                    SkillId="chr_0003_endminf_ComboSkill",
                                    SkillLevel=1,
                                    SkillMaxLevel=1,
                                    SkillEnhancedLevel=1
                                },

                            }
                        }
                    },                    
                    new CharInfo()
                    {
                        Objid=2,
                        Templateid="chr_0015_lifeng",
                        Level=1,
                        WeaponId=4611757057225326607,
                        NormalSkill="chr_0015_lifeng_NormalSkill",
                        
                        CharType=CharType.DefaultType,
                        BattleInfo = new()
                        {
                            Hp=ResourceManager.characterTable["chr_0015_lifeng"].attributes[1].Attribute.attrs.Find(A=>A.attrType==(int)AttributeType.MaxHp).attrValue,
                        },
                        BattleMgrInfo = new()
                        {
                           
                        },
            
                        OwnTime=1737205147,
                        Talent = new()
                        {
                            
                        },
                        SkillInfo = new()
                        {
                            NormalSkill="chr_0015_lifeng_NormalSkill",
                            ComboSkill="chr_0015_lifeng_ComboSkill",
                            UltimateSkill="chr_0015_lifeng_UltimateSkill",
                            DispNormalAttackSkill="chr_0015_lifeng_NormalAttack",
                            LevelInfo = 
                            {
                                new SkillLevelInfo()
                                {
                                    SkillId="chr_0015_lifeng_NormalAttack",
                                    SkillLevel=1,
                                    SkillMaxLevel=1,
                                    SkillEnhancedLevel=1
                                },
                                new SkillLevelInfo()
                                {
                                    SkillId="chr_0015_lifeng_NormalSkill",
                                    SkillLevel=1,
                                    SkillMaxLevel=1,
                                    SkillEnhancedLevel=1
                                },
                                new SkillLevelInfo()
                                {
                                    SkillId="chr_0015_lifeng_UltimateSkill",
                                    SkillLevel=1,
                                    SkillMaxLevel=1,
                                    SkillEnhancedLevel=1
                                },
                                new SkillLevelInfo()
                                {
                                    SkillId="chr_0015_lifeng_ComboSkill",
                                    SkillLevel=1,
                                    SkillMaxLevel=1,
                                    SkillEnhancedLevel=1
                                },

                            }
                        }
                    }
                },
                CurrTeamIndex=0,
                MaxCharTeamMemberCount=4,
                
                TeamInfo =
                {
                    new CharTeamInfo()
                    {
                        CharTeam={1,2},
                        Leaderid= 2,
                        
                        
                    }
                },
               
            };

            SetData(ScMessageId.ScSyncCharBagInfo, proto);
        }

    }
}
