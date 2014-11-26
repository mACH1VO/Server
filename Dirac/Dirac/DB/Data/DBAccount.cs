using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Dirac.DB.Data
{
    public class DBAccount
    {
        public int accountid { get; set; }
        public String account { get; set; }
        public String password { get; set; }
        public String mail { get; set; }
        public int securitycode { get; set; }

        public void load(MySqlDataReader mysqldatareader)
        {

        }
    }
}
