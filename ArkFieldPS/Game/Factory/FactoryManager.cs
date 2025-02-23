using ArkFieldPS.Game.Factory.Components;
using ArkFieldPS.Packets.Sc;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        public void ExecOp(CsFactoryOp op, ulong seq)
        {
            FactoryChapter chapter = GetChapter(op.ChapterId);
            if (chapter != null)
            {
                chapter.ExecOp(op, seq);
                
            }
            else
            {
                ScFactoryOpRet ret = new()
                {
                    RetCode = FactoryOpRetCode.Fail,
                    
                };
                player.Send(ScMessageId.ScFactoryOpRet, ret, seq);
            }
        }
        public void Update()
        {
            foreach (FactoryChapter chapter in chapters)
            {
                chapter.Update();
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

        public void Update()
        {
            try
            {
                foreach (FactoryNode node in nodes)
                {
                    node.Update(this);
                }
            }
            catch (Exception e)
            {

            }

        }
        public List<FactoryNode> GetNodesInRange(Vector3f pos,float range)
        {
            return nodes.FindAll(n => n.position.Distance(pos) <= range);
        }
        
        public void ExecOp(CsFactoryOp op, ulong seq)
        {
            
            switch (op.OpType)
            {
                case FactoryOpType.Place:
                    CreateNode(op.Place, seq);
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
        private void CreateNode(CsdFactoryOpPlace place, ulong seq)
        {
            v++;
            uint nodeId = v;
            FactoryBuildingTable table = ResourceManager.factoryBuildingTable[place.TemplateId];
            FactoryNode node = new()
            {
                nodeId = nodeId,
                templateId = place.TemplateId,
                mapId = place.MapId,
                nodeType = table.GetNodeType(),
                position = new Vector3f(place.Position),
                direction = new Vector3f(place.Direction),
                guid = GetOwner().random.NextRand()
            };
            
            node.InitComponents(this);
            nodes.Add(node);
            ScFactoryModifyChapterNodes edit = new()
            {
                ChapterId = chapterId,
                Tms = DateTime.UtcNow.ToUnixTimestampMilliseconds(),
                
            };
            GetOwner().Send(new PacketScFactorySyncChapter(GetOwner(), chapterId));
            edit.Nodes.Add(node.ToProto());
            Logger.Print(Newtonsoft.Json.JsonConvert.SerializeObject(edit, Newtonsoft.Json.Formatting.Indented));
            GetOwner().Send(ScMessageId.ScFactoryModifyChapterNodes, edit);
            GetOwner().Send(new PacketScFactoryOpRet(GetOwner(), node.nodeId,FactoryOpType.Place),seq);
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
                deactive=true,
                guid = GetOwner().random.NextRand()
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
        public bool forcePowerOn = false;
        public List<FComponent> components = new();
        [BsonIgnore]
        public bool powered = false;
        public uint connectedPowerNode = 0;
        public ulong guid;
        public void Update(FactoryChapter chapter)
        {
            if(!templateId.Contains("hub"))
            if (GetComponent<FComponentPowerPole>() != null)
            {
                FactoryNode curEnergyNode = chapter.nodes.Find(n => n.nodeId == connectedPowerNode && n.position.Distance(position) <= 20 && n.InPower());
                if (templateId != "power_pole_2")
                {
                    FactoryNode energyNode = chapter.GetNodesInRange(position, 20).Find(n=>n.GetComponent< FComponentPowerPole>()!=null && n.InPower());
                    if (energyNode != null && curEnergyNode==null && energyNode.connectedPowerNode!=nodeId)
                    {
                        powered= true;
                        connectedPowerNode = energyNode.nodeId;
                        chapter.GetOwner().Send(ScMessageId.ScFactoryModifyChapterNodes, new ScFactoryModifyChapterNodes() { Tms = DateTime.UtcNow.ToUnixTimestampMilliseconds(), Nodes = { this.ToProto()} });
                    }
                    else
                    {
                        if (curEnergyNode == null && powered==true)
                        {
                            powered = false;
                            connectedPowerNode = 0;
                            chapter.GetOwner().Send(ScMessageId.ScFactoryModifyChapterNodes, new ScFactoryModifyChapterNodes() { Tms = DateTime.UtcNow.ToUnixTimestampMilliseconds(), Nodes = { this.ToProto() } });
                        }
                    }
                }
                else
                {
                    //Check near 
                }
            }
        }
        //public ulong guid;
        public bool InPower()
        {
            if (forcePowerOn)
            {
                return true;
            }
            return powered;
        }
        public FComponent GetComponent<FComponent>() where FComponent : class
        {
            return components.Find(c => c is FComponent) as FComponent;
        }
        public FMesh GetMesh()
        {
            FMesh mesh = new FMesh();

            if (ResourceManager.factoryBuildingTable.ContainsKey(templateId))
            {
                FactoryBuildingTable table = ResourceManager.factoryBuildingTable[templateId];

                // Calcola il centro della struttura (origine della rotazione)
                double centerX = position.x + table.range.width / 2.0;
                double centerZ = position.z + table.range.depth / 2.0;

                // Punto A (base)
                Vector3f p1 = new Vector3f(position.x, position.y, position.z);

                // Punto B (calcolato rispetto alla larghezza e profondità)
                Vector3f p2 = new Vector3f(
                    position.x + table.range.width,
                    position.y + table.range.height,
                    position.z + table.range.depth
                );

                // Ruota entrambi i punti attorno al centro della struttura
                p1 = RotateAroundY(p1, new Vector3f((float)centerX, position.y, (float)centerZ), direction.y);
                p2 = RotateAroundY(p2, new Vector3f((float)centerX, position.y, (float)centerZ), direction.y);

                // Aggiunge i due punti alla mesh
                mesh.points.Add(p1);
                mesh.points.Add(p2);
            }
            return mesh;
        }

        // Metodo per ruotare un punto attorno all'asse Y rispetto a un'origine
        private Vector3f RotateAroundY(Vector3f point, Vector3f origin, double angleDegrees)
        {
            double angleRadians = angleDegrees * (Math.PI / 180.0);
            double cosTheta = Math.Cos(angleRadians);
            double sinTheta = Math.Sin(angleRadians);

            double dx = point.x - origin.x;
            double dz = point.z - origin.z;

            double rotatedX = origin.x + (dx * cosTheta - dz * sinTheta);
            double rotatedZ = origin.z + (dx * sinTheta + dz * cosTheta);

            return new Vector3f((float)rotatedX, point.y, (float)rotatedZ);
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
                    InPower= InPower(),
                    NeedInPower=true,
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
                node.Flag = 0;
                node.InstKey = "";
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
            return 10000+nodeId;
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
            switch (nodeType)
            {
                case FCNodeType.PowerPole:
                    components.Add(new FComponentPowerPole(chapter.nextCompV()).Init());
                    break;
                case FCNodeType.PowerDiffuser:
                    components.Add(new FComponentPowerPole(chapter.nextCompV()).Init());
                    break;
                case FCNodeType.TravelPole:
                    components.Add(new FComponentTravelPole(chapter.nextCompV()).Init());
                    break;
                case FCNodeType.Hub:
                    components.Add(new FComponentSelector(chapter.nextCompV()).Init());
                    components.Add(new FComponentPowerPole(chapter.nextCompV()).Init());
                    components.Add(new FComponentPowerSave(chapter.nextCompV()).Init());
                    components.Add(new FComponentStablePower(chapter.nextCompV()).Init());
                    components.Add(new FComponentBusLoader(chapter.nextCompV()).Init());    
                    components.Add(new FComponentPortManager(chapter.nextCompV(),GetComponent<FComponentBusLoader>().compId).Init());
                    forcePowerOn = true;
                    break;
                case FCNodeType.SubHub:
                    components.Add(new FComponentSubHub(chapter.nextCompV()).Init());
                    components.Add(new FComponentSelector(chapter.nextCompV()).Init());
                    components.Add(new FComponentPowerPole(chapter.nextCompV()).Init());
                    components.Add(new FComponentPowerSave(chapter.nextCompV()).Init());
                    components.Add(new FComponentStablePower(chapter.nextCompV()).Init());
                    components.Add(new FComponentBusLoader(chapter.nextCompV()).Init());
                    components.Add(new FComponentPortManager(chapter.nextCompV(), GetComponent<FComponentBusLoader>().compId).Init());
                    forcePowerOn = true;
                    break;
                default:
                    components.Add(new FComponent(chapter.nextCompV(), GetMainCompType()).Init());
                    break;
            }

        }
        [BsonDiscriminator(Required = true)] // Abilita il campo "_t" per identificare il tipo
        [BsonKnownTypes(typeof(FComponentSelector))] // Dichiarazione dei tipi derivati

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
            
            public FComponent(uint id, FCComponentType t)
            {
                this.compId = id;
                this.type = t;
            }
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
                SetComponentInfo(proto);
                return proto;
            }

            public virtual void SetComponentInfo(ScdFacCom proto)
            {
                if (inventory != null)
                {
                    proto.Inventory = inventory.ToProto();
                }
                else if (type == FCComponentType.PowerPole)
                {
                    proto.PowerPole = new()
                    {

                    };
                }
            }

            public virtual FComponent Init()
            {
                switch (type)
                {
                    case FCComponentType.Inventory:
                        inventory = new();
                        break;
                    default:
                        break;
                }
                return this;    
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
