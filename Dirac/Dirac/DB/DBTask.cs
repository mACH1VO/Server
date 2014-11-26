using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Dirac.DB
{
    public abstract class DBTask
    {
        public DBTask()
        {

        }

        public abstract void Execute(MySqlConnection connection);
    }
}
