using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkFieldPS.Resource.Table
{
    public class LevelGradeTable
    {
        public string name;
        public List<LevelGradeInfo> grades;
    }
    public class LevelGradeInfo
    {
        public int bandwidth;
        public int battleBuildingLimit;
        public int grade;
        public int monsterBaseLevel;
        public int prosperity;
        public int travelPoleLimit;
    }
}
