

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Dirac.Logging;

using Dirac.GameServer.Network;
using Dirac.GameServer.Network.Message;
using Dirac.GameServer.Core;

using Dirac.DB;

namespace Dirac.GameServer
{
    public static class Game //: IMessageConsumer
    {
        public static int GS_UPDATE_THREAD_ID;
        public static TickTimer gcCollecttick = new TickTimer(30000);
        public static TickTimer UpdateFormStatic = new TickTimer(1000);
        public static int GameId { get; set; }

        public static ConcurrentDictionary<GameClient, Player> Players = new ConcurrentDictionary<GameClient, Player>();

        public static readonly ConcurrentDictionary<int, DynamicObject> _objects = new ConcurrentDictionary<int, DynamicObject>();

        public static int CurrentFPS;
        /// <summary>
        /// Dictionary that tracks world.
        /// NOTE: This tracks by WorldSNO rather than by DynamicID; this.Objects _does_ still contain the world since it is a DynamicObject
        /// </summary>
        public static ConcurrentDictionary<int, Map> Worlds = new ConcurrentDictionary<int, Map>();

        /// <summary>
        /// Starting world's sno id.
        /// </summary>
        public static int StartingWorldSNOId { get; set; }

        /// <summary>
        /// Starting world for the game.
        /// </summary>
        public static Map StartingMap { get; set; }

        /// <summary>
        /// Player index counter.
        /// </summary>
        public static int PlayerIndexCounter = -1;

        /// <summary>
        /// Update frequency for the game - 100 ms.
        /// </summary>
        public static readonly TimeSpan UpdateFrequency = TimeSpan.FromMilliseconds(52);


        private static Stopwatch SleeperWatch;
        private static Stopwatch GlobalFPSWatch;

        private static int _lastObjectID = 0x00000001;
        private static int _lastWorldID = 0x07000000;

        public static int NewObjectID { get { return Interlocked.Increment(ref _lastObjectID); } }
        public static int NewActorID { get { return Interlocked.Increment(ref _lastObjectID); } }
        public static int NewWorldID { get { return Interlocked.Increment (ref _lastWorldID); } }

        public static object locker = new object();
        private static object locker1 = new object();

        public static void Route(GameClient client, GameMessage message)
        {
            lock (locker)
            {
                try
                {
                    switch (message.Consumer)
                    {
                        case Consumers.Game:
                            Game.Consume(client, message);
                            break;
                        case Consumers.Inventory:
                            client.Player.Inventory.Consume(client, message);
                            break;
                        case Consumers.Player:
                            client.Player.Consume(client, message);
                            break;
                        case Consumers.Conversations:
                            //client.Player.Conversations.Consume(client, message);
                            break;
                        case Consumers.SelectedNPC:
                            /*if (client.Player.SelectedNPC != null)
                                client.Player.SelectedNPC.Consume(client, message);*/
                            break;
                        case Consumers.Login:
                            Login.LoginManager.Consume(client, message);
                            /*if (client.Player.SelectedNPC != null)
                                client.Player.SelectedNPC.Consume(client, message);*/
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogManager.DefaultLogger.Error(ex.Message);
                }
            }
            //Logger.PacketLog(message.AsText());
            
        }


        /// <summary>
        /// Allows a player to join the game.
        /// </summary>
        /// <param name="joinedPlayer">The new player.</param>
        /// 
        public static void Initialize()
        {
            //Game.StartingWorldSNOId = 1;
            Game.StartingMap = new Map(25);//25 es snoid no importa x ahora
            Game.Worlds.TryAdd(25, Game.StartingMap);

            //ItemFactory.CreateItem(30).EnterWorld(new Dirac.Math.Vector3(10, 0, 10));
            //ItemFactory.CreateItem(30).EnterWorld(new Dirac.Math.Vector3(3, 0, 3));
            


            Game.SleeperWatch = new Stopwatch();
            Game.GlobalFPSWatch = new Stopwatch();

            //DBSessions.Instance.Connect("localhost", "root", "riquelme");
            DBSessions.Instance.Connect(
                DataBaseConfig.Instance.Server,
                DataBaseConfig.Instance.User,
                DataBaseConfig.Instance.Password
                );


            //Thread UpdateThread = new Thread(Update) { IsBackground = true, CurrentCulture = CultureInfo.InvariantCulture }; // create the game update thread.
            //UpdateThread.Start();

            //Pathfinder = new Mooege.Core.GS.AI.Pather(this); //Creates the "Game"s single Pathfinder thread, Probably could be pushed further up and have a single thread handling all path req's for all running games. - DarkLotus
            //var patherThread = new Thread(Pathfinder.UpdateLoop) { IsBackground = true, CurrentCulture = CultureInfo.InvariantCulture };
            //patherThread.Start();

        }


        /// <summary>
        /// The main game loop.
        /// </summary>
        public static void enterUpdate()
        {
            //GS_UPDATE_THREAD_ID = AppDomain.GetCurrentThreadId();

            TimeSpan Elapsed = TimeSpan.FromSeconds(0);

            while (true)
            {
                try
                {
                    Game.GlobalFPSWatch.Stop();
                    CurrentFPS = (int)(1f / Game.GlobalFPSWatch.Elapsed.TotalSeconds);
                    Game.GlobalFPSWatch.Restart();

                    Game.SleeperWatch.Restart();


                    lock (locker1)
                    {
                        foreach (var world in Game.Worlds)
                        {
                            world.Value.Update(Elapsed);
                        }
                    }

                    DB.DBManager.Instance.Update(Elapsed);
                    //BulletPhysics.BulletEngine.Update(Elapsed);

                    SleeperWatch.Stop();

                    TimeSpan compensation = TimeSpan.FromMilliseconds((UpdateFrequency.TotalMilliseconds - SleeperWatch.Elapsed.TotalMilliseconds)); // the compensation value we need to sleep in order to get consistent 100 ms Game.Update().

                    if (SleeperWatch.Elapsed.TotalMilliseconds > UpdateFrequency.TotalMilliseconds)
                        Logging.LogManager.DefaultLogger.Warn("Game() took [{0}ms] / [{1}ms].", SleeperWatch.Elapsed.Milliseconds, UpdateFrequency.Milliseconds); // TODO: We may need to eventually use dynamic tickRate / updateFrenquencies. /raist.
                    else
                        Thread.Sleep(compensation); // sleep until next Update().

                }
                catch (Exception ex)
                {
                    Logging.LogManager.DefaultLogger.Error(ex.Message);
                    //Logging.LogManager.DefaultLogger.Error(ex.Message);
                    //Logging.LogManager.DefaultLogger.Error(ex.StackTrace);
                }
            }

        }

        public static void Consume(GameClient client, GameMessage message)
        {
            switch (message.opcodes)
            {
                case Opcodes.GameSetupMessage:
                    EnterPlayer(client, message);
                    break;
                /*case Opcodes.ACDTranslateNormalMessage:
                    //Dirac.GameServer.PathFindingSystem.NavigationMesh.Initialize(); //trick
                    client.Player.Consume(client, message);
                    break;*/
            }
        }


        public static void EnterPlayer(GameClient client, GameMessage message)
        {
            client.SendMessage(new GameChangeSceneMessage()
                {
                    NewSceneId = 3,
                    Field1 = 0,
                }, true);

            client.Player = new Player(client, 32202);

            Logging.LogManager.DefaultLogger.Trace("Player {0} [PlayerIndex: {1}] connected.", "test", client.Player.PlayerIndex);

            Players.TryAdd(client.Player.GameClient, client.Player);
            NewPlayerMessage npm = new NewPlayerMessage()
            {
                dynamicId = client.Player.DynamicID, // player index
                //ToonId = new EntityId() { High = (long)1, Low = (long)1 }, //Toon
                //GameAccountId = new EntityId() { High = (long)1, Low = (long)1 }, //GameAccount
                ToonName = "mACH1VO",//joinedPlayer.name,
                Field3 = 0x00000002, //party frame class
                Field4 = 0x4, //party frame level 
                snoActorPortrait = 32000,//party frame portrait //ojo
                Level = 10,
                IsMainPlayer = true, //announce party join
                Field9 = 0x00000001,
                ActorID = client.Player.DynamicID,
                SNO = client.Player.SNOId
            };

            client.Player.GameClient.SendMessage(npm);

            client.Player.GameClient.SendMessage(new ACDEnterMapMessage()
            {
                ActorId = client.Player.DynamicID,
                Position = client.Player.Position,
                Orientation = client.Player.Orientation,
                Scale = 1f,
                MapId = 1,
            });

            Game.StartingMap.Enter(client.Player);

            SkillListMessage skill_msg = new SkillListMessage();
            skill_msg.Primary = SkillOpcode.Hand;
            skill_msg.Secondary = SkillOpcode.Cyclone;

            skill_msg._one = SkillOpcode.Invalid_Skill;
            skill_msg._two = SkillOpcode.Invalid_Skill;
            skill_msg._three = SkillOpcode.Invalid_Skill;
            skill_msg._four = SkillOpcode.Invalid_Skill;
            client.Player.GameClient.SendMessage(skill_msg);

            
            /*for (int i = 0; i < ; i++)
            {
                available_skill_msg.availableSkillList.Add(SkillOpcode.Inner);
            }*/
            AvailableSkillListMessage available_skill_msg = new AvailableSkillListMessage();
            available_skill_msg.availableSkillList.Add(SkillOpcode.Hand);
            available_skill_msg.availableSkillList.Add(SkillOpcode.Cyclone);
            available_skill_msg.availableSkillList.Add(SkillOpcode.RagefulBlow);
            available_skill_msg.availableSkillList.Add(SkillOpcode.DeathStab);
            available_skill_msg.availableSkillList.Add(SkillOpcode.Inner);
            available_skill_msg.availableSkillList.Add(SkillOpcode.Twisting);
            client.Player.GameClient.SendMessage(available_skill_msg);

            client.Player.GameClient.TickingEnabled = true;
            client.SendTick();

            client.Player.Attributes.BroadcastAllAttributestoPlayer(client.Player);
        }


        public static void Enter(Player joinedPlayer)
        {
            
        }
    }
}
