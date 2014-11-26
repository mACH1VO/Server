using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using Dirac;
using Dirac.GameServer.Core;
using Dirac.GameServer.Core.AI;
using Dirac.GameServer.Core.AI.Brains;
using Dirac.Extensions;
using Dirac.Logging;
//using Dirac.BulletPhysics;

using Dirac.Math;

using Math = System.Math;


namespace Dirac.GameServer.Core
{
    public partial class Player
    {
        public virtual BaseBrain Brain { get; set; }
        public Player createDefaultBrain()
        {
            /*this.Brain = new BaseBrain(this, BrainState.Roam);
            this.Brain.Start();
            this.Brain.SourcePoint = new Vector3(0, 0, 0);*/
            return this;
        }

        public void ProcessCommand(String command)
        {
            //test.-
            if (command == "killall")
            {
                foreach (var monster in this.GetMonstersInRange())
                {
                    monster.Die();
                }
            }
            if (command.First() == '/')
            {
                String cmd = command.Substring(1);

                if (Regex.IsMatch(cmd, @"^\d+$"))
                {
                    foreach (var item in Actor.SNOToFile)
                    {
                        if (item.Value == "ArmorMale" + cmd
                            || item.Value == "BootMale" + cmd
                            || item.Value == "HelmMale" + cmd
                            || item.Value == "GloveMale" + cmd
                            || item.Value == "PantMale" + cmd)
                        {
                            Item itemtoDrop = new Item(item.Key);
                            //itemtoDrop.Drop(this.Position);
                        }
                    }
                }
                if (cmd.ToLower() == "player")
                {
                   /* Player playerToSpawn = new Player(null, 32001);
                    playerToSpawn.EnterWorld(new Vector3(0, 0, 0));*/
                }
                if (cmd.Contains("spawn"))
                {
                    String cmd2 = command.Substring(command.LastIndexOf(' ')).Replace(" ","");
                    foreach (var item in Actor.SNOToFile)
                    {
                        if (item.Value == cmd2)
                        {
                            /*NPC npctoSpawn = new NPC(this.World, item.Key);
                            npctoSpawn.InitialLookAt = this.Position;
                            npctoSpawn.EnterWorld(this.Position + new Vector3(-10, -0.1f, 5));*/
                            //npctoSpawn.LookAt(Vector3.UNIT_X);
                        }
                    }
                }
                foreach (var item in Actor.SNOToFile)
                {
                    if (item.Value == cmd)
                    {
                        //Item itemtoDrop = new Item(this.World, item.Key);
                        //itemtoDrop.Drop(this.Position);
                    }
                }
            }

        }
    }
}
