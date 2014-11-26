using System;
using System.Collections.Concurrent;
using Dirac.GameServer.Types;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading;
using Dirac.Logging;
using Dirac.GameServer.Core;
using Dirac.Math;

namespace Dirac.GameServer.Core
{
    public partial class Map : DynamicObject, IRevealable, IUpdateable
    {
        public void InitializeActors()
        {
            //MonsterFactory.Create(this, 51000).EnterWorld(new Dirac.Math.Vector3(15, 0, 15));
            //MonsterFactory.Create(this, 51000).EnterWorld(new Dirac.Math.Vector3(10, 0, 10));
            //MonsterFactory.Create(this, 51000).EnterWorld(new Dirac.Math.Vector3(-10, 0, 31));
            Executor.Execute(1000, () =>
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector3 pos = new Vector3(RandomHelper.Next(0, 80), 1, RandomHelper.Next(0, 90));/*new Vector3(RandomHelper.Next(-250, -230), 1, RandomHelper.Next(5, 15))*/;
                    Vector3 pos2 = new Vector3(RandomHelper.Next(0, 80), 1, RandomHelper.Next(0, 90));/*new Vector3(RandomHelper.Next(-250, -230), 1, RandomHelper.Next(5, 15))*/;
                    Vector3 pos3 = new Vector3(RandomHelper.Next(0, 80), 1, RandomHelper.Next(0, 90));/*new Vector3(RandomHelper.Next(-250, -230), 1, RandomHelper.Next(5, 15))*/;

                    /*Monster m = MonsterFactory.Create(51005);
                    m.createDefaultBrain();
                    m.EnterWorld(pos);

                    Monster m2 = MonsterFactory.Create(51007);
                    m2.createDefaultBrain();
                    m2.EnterWorld(pos2);

                    Monster m3 = MonsterFactory.Create(51008);
                    m3.createDefaultBrain();
                    m3.EnterWorld(pos3);

                    MonsterFactory.Create(51001).createDefaultBrain().EnterWorld(pos);
                    MonsterFactory.Create(51003).createDefaultBrain().EnterWorld(pos);
                    MonsterFactory.Create(51009).createDefaultBrain().EnterWorld(pos);
                    MonsterFactory.Create(51010).createDefaultBrain().EnterWorld(pos);
                    MonsterFactory.Create(51011).createDefaultBrain().EnterWorld(pos);
                    MonsterFactory.Create(51012).createDefaultBrain().EnterWorld(pos);*/

                    /*for (int t = 11; t < 12; t++)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            Vector3 post = new Vector3(RandomHelper.Next(-50, 50), 0, RandomHelper.Next(-50, 50));
                            Monster m = MonsterFactory.Create(51000 + t).createDefaultBrain();
                            this.Enter(m, post);
                        }
                    }*/
                    int gg = 0;
                    for (int j = 0; j < /*10*/2; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            gg++;
                            Monster m = MonsterFactory.Create(51001 + gg).createDefaultBrain();
                            this.Enter(m, new Vector3(80 + 30 * j, 0, 30 * k));
                        }
                    }
                    

                    /*m = MonsterFactory.Create(51000 + 10).createDefaultBrain();
                    this.Enter(m, new Vector3(80, 0, 20));

                    m = MonsterFactory.Create(51000 + 12).createDefaultBrain();
                    this.Enter(m, new Vector3(80, 0, 40));

                    m = MonsterFactory.Create(51000 + 11).createDefaultBrain();
                    this.Enter(m, new Vector3(80, 0, 60));*/

                    //Vector3 posit = new Vector3(RandomHelper.Next(-60, 60), 0, RandomHelper.Next(-60, 60));
                    //Vector3 posit = new Vector3(100, -100, 0);
                    NPC npc = NPCFactory.Create(20000);
                    this.Enter(npc, new Vector3(0,0,0));

                    //posit = new Vector3(RandomHelper.Next(-60, 60), 0, RandomHelper.Next(-60, 60));
                    NPC npc2 = NPCFactory.Create(20001);
                    this.Enter(npc2, new Vector3(40, 0, 0));

                    /*Spider spd = new Spider(this);
                    spd.EnterWorld(Vector3.ZERO);

                    DarkSpider dspd = new DarkSpider(this);
                    spd.EnterWorld(Vector3.ZERO + Vector3.UNIT_X *5);

                    NPC npctoSpawn = NPCFactory.Create(20000);
                    npctoSpawn.InitialLookAt = Vector3.ZERO;
                    npctoSpawn.EnterWorld(new Vector3(50, 0, 50));*/

                    //MonsterFactory.Create(51001).createDefaultBrain().EnterWorld(pos);
                    /*MonsterFactory.Create(51002).createDefaultBrain().EnterWorld(pos);
                    MonsterFactory.Create(51003).createDefaultBrain().EnterWorld(pos);*/

                    for (int j = 0; j < 25; j++)
                    {
                        /*Monster monsterRandom = MonsterFactory.Create(51000 + RandomHelper.Next(1, 150));
                        Vector3 posR = new Vector3(RandomHelper.Next(0, 90), 1, RandomHelper.Next(0, 90));
                        if (monsterRandom != null)
                            monsterRandom.createDefaultBrain().EnterWorld(posR);*/
                    }
                }

                for (int i = 0; i < 1; i++)
                {
                    Vector3 pos = new Vector3(RandomHelper.Next(0, 80), 1, RandomHelper.Next(0, 90));/*new Vector3(RandomHelper.Next(-250, -230), 1, RandomHelper.Next(5, 15))*/;
                    //new DarkSpider(this).EnterWorld(pos);
                    //new M_Zombiedog01_SM(this).EnterWorld(pos);
                }

                for (int i = 0; i < 1; i++)
                {
                    Vector3 pos = new Vector3(RandomHelper.Next(0, 70), 1, RandomHelper.Next(0, 80));/*new Vector3(RandomHelper.Next(-250, -230), 1, RandomHelper.Next(5, 15))*/;
                    //new M_StoneScorpion01_Sm(this).EnterWorld(pos);
                }

                for (int i = 0; i < 2; i++)
                {
                    Vector3 pos = new Vector3(RandomHelper.Next(0, 50), 1, RandomHelper.Next(0, 50));/*new Vector3(RandomHelper.Next(-250, -230), 1, RandomHelper.Next(5, 15))*/;
                    //new Bull(this).EnterWorld(pos);
                }
            });


            //ItemFactory.CreateItem(this, 50000).EnterWorld(new Dirac.Math.Vector3(0, 0, 0));
        }
    }

    
}

