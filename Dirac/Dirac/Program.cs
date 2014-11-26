using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
/*using Dirac.Logging;
using Dirac.GameServer;
using Dirac.GameServer.Types;
using Dirac.Window;
using Dirac.BulletPhysics;*/

namespace Dirac
{
    class Program
    {
        public static Window.ServerForm serverForm;
        static void Main(string[] args)
        {
           
            Thread formthread = new Thread(StartForm);
            formthread.IsBackground = true;
            formthread.Start();

            /*Thread BulletThread = new Thread(BulletPhysics.BasicDemoStart.Start);
            BulletThread.IsBackground = true;
            BulletThread.Start();*/

            Logging.LogManager.InitLoggers();
            /*Logging.LogManager.CreateLogger().Trace("TEST");
            Logging.LogManager.Loggers.Values.FirstOrDefault().Trace("AJJAJAJA");*/
            Thread.Sleep(1000);

            DateTime InitTime = DateTime.Now;

            //Dirac.Logging.Logger.Initialize(serverForm);
            //BulletEngine.Initialize();
            Dirac.GameServer.Core.GameAttributeStaticList.Initialize();
            Dirac.GameServer.Executor.Initialize();
            //Engine.PhysXEngine.Initialize();
            Dirac.GameServer.Core.MonsterFactory.Initialize();
            Dirac.GameServer.Core.Actor.Initialize();
            Dirac.GameServer.Core.ItemFactory.Initialize();

            //MegaServer.GameServer.PathFindingSystem.NavigationMesh.Initialize();

            Dirac.GameServer.Game.Initialize();


            TimeSpan StartupLasts = DateTime.Now - InitTime;

            Dirac.Logging.LogManager.DefaultLogger.Trace("[StartUPTime] " + StartupLasts.ToString());

            Dirac.GameServer.Network.GameServer gs = new GameServer.Network.GameServer();
            gs.Run();

            Thread.CurrentThread.IsBackground = true;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            GameServer.Game.enterUpdate();

            /*while(true)
            {
                Dirac.Logging.LogManager.DefaultLogger.Info("[StartUPTime] " + StartupLasts.ToString());
                Thread.Sleep(100);
            }*/
            /*Thread GameServerThread = new Thread(gs.Run);
            GameServerThread.Start();*/
            
            //Thread.Sleep(1000);

            
        }

        [STAThread]
        static void StartForm()
        {
            Window.ServerForm.CheckForIllegalCrossThreadCalls = false;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            serverForm = new Window.ServerForm();
            Application.Run(serverForm);
            
        }
    }
}
