using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using Dirac.GameServer;
using MySql.Data.MySqlClient;

namespace Dirac.DB
{
    public class DBSessions
    {
        private static DBSessions instance;

        private DBSessions() { }

        public static DBSessions Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBSessions();
                }
                return instance;
            }
        }

        public MySqlConnection Connection { get; set; }

        public void Connect(String server, String userid, String password)
        {
            if (this.Connection != null)
            {
                Logging.LogManager.DefaultLogger.Warn("Trying to connect, but connection status is " + this.Connection.State.ToString());
                Logging.LogManager.DefaultLogger.Warn("MySqlConnection will now close and try to reconnect!");
                this.Connection.Close();
            }

            String connStr = String.Format("server={0};user id={1}; password={2}; database=mysql; pooling=false",
				server, userid, password );

            try
            {
                this.Connection = new MySqlConnection(connStr);
                this.Connection.Open();
                Logging.LogManager.DefaultLogger.Warn("MySqlConnection created, status: " + this.Connection.State.ToString());
            }
			catch (MySqlException ex) 
			{
				MessageBox.Show( "Error connecting to the server: " + ex.Message );
			}
        }

       
    }
}
