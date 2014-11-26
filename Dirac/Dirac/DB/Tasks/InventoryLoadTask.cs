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
    public class InventoryLoadTask : DBTask
    {
        DBInventory DBInventory;
        public override void Execute(MySqlConnection connection)
        {
            String query = "SELECT * FROM muonline.characters WHERE account='matias9'";

            //create mysql command
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = query;
            cmd.Connection = connection;

            try
            {
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                if (dataReader.Read())
                {
                    this.DBInventory = new Data.DBInventory();
                    this.DBInventory.load(dataReader);
                }
                else 
                {
                    Logging.LogManager.DefaultLogger.Warn("InventoryLoadTask could not read");
                }

                //close Data Reader
                dataReader.Close();
                Logging.LogManager.DefaultLogger.Trace("InventoryLoadTask executed");
            }
            catch(MySqlException ex)
            {
                Logging.LogManager.DefaultLogger.Error(ex.Message);
                Logging.LogManager.DefaultLogger.Error(ex.StackTrace);
            }
        }
    }
}
