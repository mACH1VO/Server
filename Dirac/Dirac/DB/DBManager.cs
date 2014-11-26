using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using Dirac.GameServer;
using MySql.Data.MySqlClient;

using System.Diagnostics;
using System.Timers;

namespace Dirac.DB
{
    public class DBManager
    {
        private static DBManager instance;

        private DBManager() { }

        public static DBManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBManager();
                }
                return instance;
            }
        }

        protected Queue<DBTask> QueuedTasks = new Queue<DBTask>();
        public void Update(TimeSpan elapsed)
        {
            while (this.QueuedTasks.Count > 0)
            {
                DBTask task = this.QueuedTasks.Dequeue();
                if (DBSessions.Instance.Connection != null)
                {
                    task.Execute(DBSessions.Instance.Connection);
                }
                else
                {
                    Logging.LogManager.DefaultLogger.Error("could not execute db task because connection is null!");
                }
            }
        }

        public void ConcurrentExecuteTask(DBTask dbtask)
        {
            this.QueuedTasks.Enqueue(dbtask);
        }
    }
}
