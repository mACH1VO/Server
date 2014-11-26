using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dirac.DB;
using Dirac.DB.Data;
using Dirac.GameServer;
using MySql.Data.MySqlClient;

namespace Dirac.DB.Tasks
{
    public class InventorySaveTask : DBTask
    {
        DBInventory DBInventory;
        public override void Execute(MySqlConnection connection)
        {
            String query = @"UPDATE muonline.inventories 
            SET 
            name='JAJAJADD', 
            level=115 
            WHERE characterid = 3";

            //create mysql command
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = query;
            cmd.Connection = connection;
            try
            {
                //Execute query
                int rowsAffected = cmd.ExecuteNonQuery();
                Logging.LogManager.DefaultLogger.Trace("InventorySaveTask executed, rows affected " + rowsAffected.ToString());
            }
            catch (MySqlException ex)
            {
                Logging.LogManager.DefaultLogger.Error(ex.Message);
                Logging.LogManager.DefaultLogger.Error(ex.StackTrace);
            }
        }
    }
}
