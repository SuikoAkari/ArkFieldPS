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
            Logger.Print("Connecting to MongoDB..."); 
            try
            {
                db = new Database(Server.config.mongoDatabase.uri, Server.config.mongoDatabase.collection);
                Logger.Print("Connected to MongoDB database");
            }
            catch (Exception ex)
            {
                Logger.PrintError(ex.Message);
                Logger.PrintError("Without initialized database the game server will crash. You can't run this server without MongoDB");
            }
           
        }
    }
}
