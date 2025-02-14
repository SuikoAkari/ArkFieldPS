using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkFieldPS.Game
{
    public class Team
    {
        public string name = "";
        public ulong leader;
        public List<ulong> members = new();
    }
}
