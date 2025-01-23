using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Database
{
    public class DatabaseManager
    {
        public static Database db;
        public static void Init()
        {
            Logger.Print("Connecting to mongodb...");
            try
            {
                db = new Database("mongodb://localhost:27017", "endfieldps");
            }
            catch (Exception ex)
            {
                Logger.PrintError(ex.Message);
            }
           
        }
    }
}
