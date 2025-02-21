using ArkFieldPS.Packets.Sc;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;

namespace ArkFieldPS.Game.Factory
{
    public class FactoryManager
    {
        public Player player;
        public List<FactoryChapter> chapters = new();

        
        public FactoryManager(Player player)
        {

            this.player = player;
        }
        public void Load()
        {
            //TODO Save
            chapters.Add(new FactoryChapter("domain_1", player.roleId));
            chapters.Add(new FactoryChapter("domain_2", player.roleId));
        }
        public void PlaceOp(CsFactoryOp op)
        {
            FactoryChapter chapter = GetChapter(op.ChapterId);
            if (chapter != null)
            {
                chapter.PlaceOp(op);
                player.Send(new PacketScFactoryOpRet(player, op));
            }
            else
            {
                ScFactoryOpRet ret = new()
                {
                    RetCode = FactoryOpRetCode.Fail,
                    
                };
                player.Send(ScMessageId.ScFactoryOpRet, ret);
            }
        }
        public FactoryChapter GetChapter(string id)
        {
            return chapters.Find(c=>c.chapterId==id);
        }
    }
    public class FactoryChapter
    {
        public string chapterId;
        public ulong ownerId;
        public List<FactoryNode> nodes=new();
        public uint v = 1;
        public uint compV = 0;
        
        public void PlaceOp(CsFactoryOp op)
        {
            
            switch (op.OpType)
            {
                case FactoryOpType.Place:
                    CreateNode(op.Place);
                    break;
                default:
                    break;
            }
                
        }
        public uint nextCompV()
        {
            compV++;
            return compV;
        }
        private void CreateNode(CsdFactoryOpPlace place)
        {
            uint nodeId = v++;
            FactoryBuildingTable table = ResourceManager.factoryBuildingTable[place.TemplateId];
            FactoryNode node = new()
            {
                nodeId = nodeId,
                templateId=place.TemplateId,
                mapId=place.MapId,
                nodeType=table.GetNodeType(),
                position=new Vector3f(place.Position),
                direction = new Vector3f(place.Direction),
                
            };
            
            node.InitComponents(this);
            nodes.Add(node);
            ScFactoryModifyChapterNodes edit = new()
            {
                ChapterId = chapterId,
                Tms = DateTime.UtcNow.ToUnixTimestampMilliseconds(),
            };
            
            edit.Nodes.Add(node.ToProto());
            GetOwner().Send(ScMessageId.ScFactoryModifyChapterNodes, edit);
        }

        public FactoryChapter(string chapterId,ulong ownerId)
        {
            this.ownerId = ownerId;
            this.chapterId = chapterId;
            FactoryNode node = new()
            {
                nodeId = v,
                templateId= "__inventory__",
                nodeType=FCNodeType.Inventory,
                mapId=0,
                deactive=true
            };
            node.InitComponents(this);
            nodes.Add(node);
        }
        public Player GetOwner()
        {
            return Server.clients.Find(c => c.roleId == ownerId);
        }
    }
    public class FactoryNode
    {
        public uint nodeId;
        public FCNodeType nodeType;
        public string templateId;
        public Vector3f position=new();
        public Vector3f direction = new();
        public string instKey="";
        public bool deactive = false;
        public int mapId;
        public List<FComponent> components = new();
        public FMesh GetMesh()
        {
            FMesh mesh = new FMesh();
            
            if (ResourceManager.factoryBuildingTable.ContainsKey(templateId))
            {
                FactoryBuildingTable table = ResourceManager.factoryBuildingTable[templateId];
                mesh.points.Add(position);
                mesh.points.Add(new Vector3f()
                {
                    x = position.x+table.range.width,
                    z = position.z + table.range.depth,
                    y = position.y+table.range.height,
                });
            }
            return mesh;
        }

        public ScdFacNode ToProto()
        {
            ScdFacNode node = new ScdFacNode()
            {
                InstKey = instKey,
                NodeId=nodeId,
                TemplateId=templateId,
                StableId= GetStableId(),
                IsDeactive= deactive,
                Power = new()
                {

                },
                
                NodeType=(int)nodeType,
                Transform = new()
                {
                    Position = position.ToProtoScd(),
                    Direction=direction.ToProtoScd(),
                    MapId=mapId,

                    
                }
            };
            if(templateId!="__inventory__")
            {
                node.Transform.Mesh = GetMesh().ToProto();
                node.Transform.WorldPosition = position.ToProto();
                node.Transform.WorldRotation = direction.ToProto();
                node.InteractiveObject = new()
                {

                };
            }
            foreach(FComponent comp in components)
            {
                node.Components.Add(comp.ToProto());
                node.ComponentPos.Add((int)comp.GetComPos(), comp.compId);
            }

            return node;
        }
        public uint GetStableId()
        {
            if (templateId == "__inventory__")
            {
                return 0;
            }
            return 10000+nodeId-1;
        }
        public FCComponentType GetMainCompType()
        {
            string nodeTypeName = nodeType.ToString();
            if (Enum.TryParse(nodeTypeName, out FCComponentType fromName))
            {
                return fromName;
            }
            
            return FCComponentType.Invalid;
        }
        public void InitComponents(FactoryChapter chapter)
        {
            FComponent compBase = new FComponent()
            {
                compId=chapter.nextCompV(),
                type= GetMainCompType()
                
            };
            compBase.Init();

            components.Add(compBase);
            /*switch (nodeType)
            {
                case FCNodeType.PowerPole:


            }*/

        }

        public class FComponent
        {
            public class FCompInventory
            {
                public ScdFacComInventory ToProto()
                {
                    return new ScdFacComInventory()
                    {

                    };
                }
            }
            public uint compId;
            public FCComponentType type;
            public FCompInventory inventory;

            public FCComponentPos GetComPos()
            {

                string compTypeName = type.ToString();
                if (Enum.TryParse(compTypeName, out FCComponentPos fromName))
                {
                    return fromName;
                }
                switch (type)
                {
                    case FCComponentType.PowerPole:
                        return FCComponentPos.PowerPole;
                }
                return FCComponentPos.Invalid;
            }
            public ScdFacCom ToProto()
            {
                ScdFacCom proto = new ScdFacCom()
                {
                    ComponentType = (int)type,
                    ComponentId = compId,

                };
                if (inventory != null)
                {
                    proto.Inventory = inventory.ToProto();
                }else if(type == FCComponentType.PowerPole)
                {
                    proto.PowerPole = new()
                    {
                        
                    };
                }
                
                return proto;
            }
            public void Init()
            {
                switch (type)
                {
                    case FCComponentType.Inventory:
                        inventory = new();
                        break;
                    default:
                        break;
                }
            }
        }
        public class FMesh
        {
            public FCMeshType type;
            public List<Vector3f> points=new();
            public ScdFacMesh ToProto()
            {
                ScdFacMesh m = new ScdFacMesh()
                {
                    MeshType = (int)type
                };
                foreach (Vector3f p in points)
                {
                    m.Points.Add(new ScdVec3Int()
                    {
                        X = (int)p.x,
                        Y = (int)p.y,
                        Z = (int)p.z
                    });
                }
                return m;
            }
        }
    }

}
