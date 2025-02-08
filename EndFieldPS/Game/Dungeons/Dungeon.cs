using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Game.Dungeons
{
    public class Dungeon
    {
        public DungeonTable table;
        public Vector3f prevPlayerPos;
        public Vector3f prevPlayerRot;
        public int prevPlayerSceneNumId;
        public Player player;
        public Dungeon()
        {

        }
    }
}
