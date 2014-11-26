using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Dirac.DB.Data
{
    public class DBCharacter
    {
        public int characterid { get; set; }
        public String account { get; set; }
        public int classtype { get; set; }
        public String name { get; set; }
        public int level { get; set; }
        public int experience { get; set; }
        public int strength { get; set; }
        public int agility { get; set; }
        public int stamina { get; set; }
        public int energy { get; set; }
        public int command { get; set; }

        public void load(MySqlDataReader mysqldatareader)
        {
            this.characterid = (int)mysqldatareader["characterid"];
            this.account = (String)mysqldatareader["account"];
            this.classtype = (int)mysqldatareader["classtype"];
            this.name = (String)mysqldatareader["name"];
            this.level = (int)mysqldatareader["level"];
            this.experience = (int)mysqldatareader["experience"];
            this.strength = (int)mysqldatareader["strength"];
            this.agility = (int)mysqldatareader["agility"];
            this.stamina = (int)mysqldatareader["stamina"];
            this.energy = (int)mysqldatareader["energy"];
            this.command = (int)mysqldatareader["command"];
        }
    }
}