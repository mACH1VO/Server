using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Dirac.DB.Data
{
    public class DBItem
    {
        public int itemid { get; set; }
        public bool exellent { get; set; }
        public int sno { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public int speed { get; set; }
        public int optiondef { get; set; }
        public int optiondmg { get; set; }
        public int critical { get; set; }
        public int serial { get; set; }
        public int luck { get; set; }

        public void load(MySqlDataReader mysqldatareader)
        {
            this.itemid = (int)mysqldatareader["itemid"];
            this.exellent = (bool)mysqldatareader["exellent"];
            this.sno = (int)mysqldatareader["sno"];
            this.min = (int)mysqldatareader["min"];
            this.max = (int)mysqldatareader["max"];
            this.speed = (int)mysqldatareader["speed"];
            this.optiondef = (int)mysqldatareader["optiondef"];
            this.optiondmg = (int)mysqldatareader["optiondmg"];
            this.critical = (int)mysqldatareader["critical"];
            this.serial = (int)mysqldatareader["serial"];
            this.luck = (int)mysqldatareader["luck"];
        }
    }
}
