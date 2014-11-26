using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using Dirac.GameServer;
using Dirac.GameServer.Core;

namespace Dirac.DB.Data
{
    public class DBInventory
    {
        public int characterid { get; set; }
        public String items { get; set; }
        public int gold { get; set; }

        public Dictionary<InventorySlot, int> ItemsDictionary = new Dictionary<InventorySlot, int>();
        public Dictionary<InventorySlot, DBItem> ItemsDefinition = new Dictionary<InventorySlot, DBItem>();

        public void load(MySqlDataReader mysqldatareader)
        {
            this.characterid = (int)mysqldatareader["characterid"];
            this.items = (String)mysqldatareader["items"];
            this.gold = (int)mysqldatareader["gold"];
            this.loadItemDictionary();
            this.loadItemDefinition(mysqldatareader);
        }

        public void loadItemDictionary()
        {
            // parse items dictionary, manually :(
            // 2 + 2 + 11 // R + C + ItemCode
            int total_items = (this.items.Length / 15);

            for (int i = 0; i < total_items; i++)
            {
                string rc = this.items.Substring(i * 15, 4);
                InventorySlot slot = new InventorySlot();
                slot.fromDbString(rc);
                string item_string_id = items.Substring(i * 15 + 4, 11);
                int item_id = int.Parse(item_string_id);

                this.ItemsDictionary.Add(slot, item_id);
            }
        }

        public void loadItemDefinition(MySqlDataReader mysqldatareader)
        {
            foreach (var keyval in this.ItemsDictionary)
            {
                DBItem dbitem = new DBItem();
                dbitem.load(mysqldatareader);
            }
        }
    }
}
