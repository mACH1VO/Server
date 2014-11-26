using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dirac.GameServer;

namespace Dirac.GameServer
{
    public class StatisticsTest
    {
        private StatisticsTest() { }

        private static StatisticsTest instance;

        public static StatisticsTest Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StatisticsTest();
                }
                return instance;
            }
        }

        public int GS_UPDATE_THREAD_ID
        {
            get { return Game.GS_UPDATE_THREAD_ID; }
        }

        public int Players
        {
            get { return Game.Players.Count; }
        }

        public int Actors
        {
            get { return Game.StartingMap.Actors.Count; }
        }

        /*public int MovingActors
        {
            get { return Managers.MovementManager.ProjectilesMoving.Count; }
        }*/

        public int SystemPageSize
        {
            get { return Environment.SystemPageSize; }
        }

        public int PendingExecutorActions
        {
            get { return Dirac.GameServer.Executor.PendingActionsCount; }
        }

        public int WorldActors
        {
            get { return Dirac.GameServer.Game.StartingMap.Actors.Count; }
        }

        public int WorldMonsters
        {
            get { return Dirac.GameServer.Game.StartingMap.Monsters.Count; }
        }

        public int WorldProjectiles
        {
            get { return Dirac.GameServer.Game.StartingMap.Projectiles.Count; }
        }

        private object locker = new object();
        public String inventoryText
        {
            get
            {
                lock (locker)
                {

                    StringBuilder sb = new StringBuilder();

                    if (Game.StartingMap == null)
                        return "null";

                    if (Game.StartingMap.Players.Count == 0)
                        return "null";

                    if (Game.StartingMap.Players.FirstOrDefault().Value.Inventory == null)
                        return "null";

                    Int32[,] backpackMatrix = Game.StartingMap.Players.FirstOrDefault().Value.Inventory.GetBackPackMatrix();

                    if (backpackMatrix == null)
                        return "null";

                    int rows = Game.StartingMap.Players.FirstOrDefault().Value.Inventory.Rows;
                    int columns = Game.StartingMap.Players.FirstOrDefault().Value.Inventory.Columns;

                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < columns; j++)
                        {
                            sb.Append(backpackMatrix[i, j].ToString() + "  ");
                        }
                        sb.Append(Environment.NewLine);
                    }

                    return sb.ToString();
                }
            }
        }

        public String vaultText
        {
            get
            {
                lock (locker)
                {

                    StringBuilder sb = new StringBuilder();

                    if (Game.StartingMap == null)
                        return "null";

                    if (Game.StartingMap.Players.Count == 0)
                        return "null";

                    if (Game.StartingMap.Players.FirstOrDefault().Value.Inventory == null)
                        return "null";

                    Int32[,] backpackMatrix = Game.StartingMap.Players.FirstOrDefault().Value.Inventory.Vault.GetBackPackMatrix();

                    if (backpackMatrix == null)
                        return "null";

                    int rows = Game.StartingMap.Players.FirstOrDefault().Value.Inventory.Vault.Rows;
                    int columns = Game.StartingMap.Players.FirstOrDefault().Value.Inventory.Vault.Columns;

                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < columns; j++)
                        {
                            sb.Append(backpackMatrix[i, j].ToString() + "  ");
                        }
                        sb.Append(Environment.NewLine);
                    }

                    return sb.ToString();
                }
            }
        }
    }
}
