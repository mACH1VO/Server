using System;
using System.Collections.Concurrent;
using Dirac.GameServer.Types;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading;

using Dirac.Logging;
using Dirac.GameServer.Core;
using Dirac.GameServer.Network.Message;
using Dirac.GameServer.Network;
using Dirac.Math;

namespace Dirac.GameServer.Core
{
    public partial class Map : DynamicObject, IRevealable, IUpdateable
    {

        public QuadTree QuadTree { get; set; }

        public readonly ConcurrentDictionary<int, Actor> Actors = new ConcurrentDictionary<int, Actor>();

        public readonly ConcurrentDictionary<int, Player> Players = new ConcurrentDictionary<int, Player>();

        public readonly ConcurrentDictionary<int, Monster> Monsters = new ConcurrentDictionary<int, Monster>();

        public readonly ConcurrentDictionary<int, Item> Items = new ConcurrentDictionary<int, Item>();

        public readonly ConcurrentDictionary<int, Projectile> Projectiles = new ConcurrentDictionary<int, Projectile>();

        public bool HasPlayersIn { get { return this.Players.Count > 0; } }

        public int SNOId { get; set; }

        public Map(Int32 snoId)
            : base(Game.NewWorldID)
        {
            this.SNOId = snoId;
            this.QuadTree = new QuadTree(new Size(60, 60), 0);
            this.InitializeActors();
        }

        #region update & tick logic

        public void Update(TimeSpan elapsed)
        {
            //var actorsToUpdate = new List<IUpdateable>(); // list of actor to update.

            this.QuadTree.Update();
            foreach (var player in this.Players.Values) // get players in the world.
            {
                if (player.World != null) 
                {
                    //player.World should be always != null, but just in case... check it!
                    foreach (var actor in player.GetActorsInRange())
                    {
                        /*if (actor.ActorType == ActorType.Projectile)
                            (actor as Projectile).Update();*/
                    }
                }
            }

            foreach (var actor in this.Actors.Values)
            {
                if (actor.World != null)
                {
                    //actor.World should be always != null, but just in case... check it!
                    actor.Update(elapsed);
                }
            }

            /*foreach (var actor in actorsToUpdate) // trigger the updates.
            {
                actor.Update();
            }*/

            //Server.Form.textBox_pInRange.Text = Players.Values.FirstOrDefault().GetPlayersInRange(10).Count.ToString();

            //this.BuffManager.Update();
            //this.PowerManager.Update();

            //UpdateFlippy(tickCounter);

            foreach (var player in this.Players.Values)
            {
                if (player.GameClient != null)
                    if (player.GameClient.IsNetBufferDirty)
                        player.GameClient.SendTick(); // if there's available messages to send, will handle ticking and flush the outgoing buffer.
            }

            //Logging.LogManager.DefaultLogger.Trace("This Thread is {0}", Thread.CurrentThread.ManagedThreadId);
            /*foreach (var proj in Projectiles.Values)
            {
                proj.Update();
            }*/
        }

        #endregion

        #region message broadcasting

        /*public void BroadcastIfIsInRange(GameMessage message, Actor actor, float range)
        {
            //no deberia ser getplayersinrange instead of player.values?
            foreach (var player in this.Players.Values)
            {
                if (player.RevealedObjects.ContainsKey(actor.DynamicID))
                {
                    player.GameClient.SendMessage(message);
                }
            }
        }*/

        public void BroadcastIfRevealed(GameMessage message, Actor actor)
        {
            //no deberia ser getplayersinrange instead of player.values?
            foreach (var player in this.Players.Values)
            {
                //if (player.RevealedObjects.ContainsKey(actor.DynamicID))
                //{
                    //List<Actor> inRange = player.GetActorsInRange(80);
                    //if (inRange.Contains(actor))
                    if (player.GameClient != null)
                        player.GameClient.SendMessage(message);
                //}
            }
        }

        public void BroadcastGlobal(GameMessage message)
        {
            foreach (var player in this.Players.Values)
            {
                if (player.GameClient != null)
                player.GameClient.SendMessage(message);
            }
        }

        public void BroadcastInclusive(GameMessage message, Actor actor)
        {
            var players = actor.GetPlayersInRange();
            foreach (var player in players)
            {
                if (player.GameClient != null)
                player.GameClient.SendMessage(message);
            }
        }

        public void BroadcastExclusive(GameMessage message, Actor actor)
        {
            var players = actor.GetPlayersInRange();
            foreach (var player in players.Where(player => player != actor))
            {
                if (player.GameClient != null)
                player.GameClient.SendMessage(message);
            }
        }

        #endregion

        #region reveal logic

        /// <summary>
        /// Reveals the world to given player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns></returns>
        public bool Reveal(Player player)
        {
            if (player.RevealedObjects.ContainsKey(this.DynamicID))
                return false;

            /*player.GameClient.SendMessage(new RevealWorldMessage() // Reveal world to player
            {
                WorldID = this.DynamicID,
                Field2 = 540,
                Field3 = -600, //es el center ese de World para las scenes
                Field4 = 60,
                Field5 = 60,
                Field6 = 3300,
                Field7 = 5040
            });

            player.GameClient.SendMessage(new EnterWorldMessage()
            {
                EnterPosition = player.Position,
                WorldID = this.DynamicID,
                //WorldSNO = this.Id
            });*/

            if (!player.RevealedObjects.TryAdd(this.DynamicID, this))
                Logging.LogManager.DefaultLogger.Error("World .RevealedObjects.TryAdd");

            return true;
        }

        /// <summary>
        /// Unreveals the world to player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns></returns>
        public bool Unreveal(Player player)
        {
            if (!player.RevealedObjects.ContainsKey(this.DynamicID))
                return false;

            // player.InGameClient.SendMessage(new WorldDeletedMessage() { WorldID = DynamicID });
            IRevealable outt;
            if (!player.RevealedObjects.TryRemove(this.DynamicID, out outt))
                Logging.LogManager.DefaultLogger.Error("RevealedObjects.TryRemove");

            return true;
        }

        #endregion

        #region PathFind
        public MUPath FindPath(Vector3 source, Vector3 dest)
        {
            //THIS should be avoided, use the recast crowd implementation to move & avoid characters automatically.
            /*List<Vector3> pathlines = PathFindingSystem.NavigationMesh.FindPath(source, dest);

            if (pathlines.Count == 0)
                return null;

            Core.Paths.Path PATH = new Core.Paths.Path(pathlines);
            //return PATH;*/
            return null;
        }
        #endregion

        #region actor enter & leave functionality

        /// <summary>
        /// Allows an actor to enter the world.
        /// </summary>
        /// <param name="actor">The actor entering the world.</param>
        public void Enter(Actor actor)
        {
            actor.World = this;

            this.addActor(actor); //add actor as projectile, monster, etc
            actor.OnEnter(this);

            // reveal actor to player's in-range.
            List<Player> playersInRange = actor.GetPlayersInRange();
            foreach (var player in playersInRange)
            {
                actor.Reveal(player);
            }
        }

        /// <summary>
        /// Allows an actor to enter the world.
        /// </summary>
        /// <param name="actor">The actor entering the world.</param>
        public void Enter(Actor actor, Vector3 position)
        {
            actor.World = this;

            this.addActor(actor); //add actor as projectile, monster, etc
            actor.OnEnter(this);

            actor.Position = position;

            // reveal actor to player's in-range.
            List<Player> playersInRange = actor.GetPlayersInRange();
            foreach (var player in playersInRange)
            {
                actor.Reveal(player);
            }
        }

        /// <summary>
        /// Allows an actor to enter the world.
        /// </summary>
        /// <param name="actor">The actor entering the world.</param>
        public void Enter(Actor actor, Vector3 position, Quaternion orientation)
        {
            actor.World = this;

            this.addActor(actor); //add actor as projectile, monster, etc
            actor.OnEnter(this);

            //estos positions deben ir antes de actor.Reveal! 
            actor.Position = position;
            actor.Orientation = orientation;

            // reveal actor to player's in-range.
            List<Player> playersInRange = actor.GetPlayersInRange();
            foreach (var player in playersInRange)
            {
                actor.Reveal(player);
            }
        }

        /// <summary>
        /// Allows an actor to leave the world.
        /// </summary>
        /// <param name="actor">The actor leaving the world.</param>
        public void Leave(Actor actor)
        {
            actor.OnLeave(this);

            foreach (var player in this.Players.Values)
            {
                if (actor != player)
                {
                    actor.Unreveal(player);
                }
            }

            if (this.HasActor(actor.DynamicID))
                this.removeActor(actor);

            /*if (!(actor is Player)) 
                return; // if the leaving actors is a player, unreveal the actors revealed to him contained in the world.
            var revealedObjects = (actor as Player).RevealedObjects.Values.ToList(); // list of revealed actors.
            foreach (var @object in revealedObjects)
                if (@object != actor) // do not unreveal the player itself.
                    @object.Unreveal(actor as Player);*/
        }

        #endregion

        #region monster spawning & item drops

        /// <summary>
        /// Spawns a monster with given SNOId in given position.
        /// </summary>
        /// <param name="monsterSNOId">The SNOId of the monster.</param>
        /// <param name="position">The position to spawn it.</param>
        public void SpawnMonster(int monsterSNOId, Vector3 position)
        {
            /*var monster = ActorFactory.Create(this, monsterSNOId, new TagMap());
            monster.Scale = 1.35f;
            monster.EnterWorld(position);*/
        }

        //private List<TickTimer> _flippyTimers = new List<TickTimer>();
        private const int FlippyDurationInTicks = 30;
        private const int FlippyMaxDistanceManhattan = 10;  // length of one side of the square around the player where the item will appear
        private const int FlippyDefaultFlippy = 0x6d82;     // g_flippy.prt

        /// <summary>
        /// Spawns a random item drop for given player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The position for drop.</param>
        public void SpawnRandomItemDrop(Actor source, Player player)
        {
            /*var item = ItemGenerator.GenerateRandom(player);
            if ((item is SpellRune) && (item.Attributes[GameAttribute.Rune_Rank] == 0))
            {
                // favor player's class in attuned runes // TODO: remove or move this
                if (RandomHelper.NextDouble() > 0.6f)
                {
                    (item as SpellRune).ReAttuneToClass(player.Toon.Class);
                }
            }
            // NOTE: The owner field for an item is only set when it is in the owner's inventory. /komiga
            player.GroundItems[item.DynamicID] = item; // FIXME: Hacky. /komiga
            DropItem(source, null, item);*/
        }

        /// <summary>
        /// Spanws gold for given player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The position for drop.</param>
        public void SpawnGold(Actor source, Player player)
        {
             // TODO: Gold should be spawned for all players in range.
             //var item = ItemFactory.CreateGold(player, RandomHelper.Next(1000, 3000));
             //item.Drop(player.Position);
        }

        /// <summary>
        /// Spanws a health-globe for given player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The position for drop.</param>
        public void SpawnHealthGlobe(Actor source, Player player, Vector3 position)
        {
        }

        /// <summary>
        /// Update the flippy animations and remove them once they have timed out
        /// </summary>
        /// <param name="tickCounter"></param>
        private void UpdateFlippy(int tickCounter)
        {
            /*List<TickTimer> finished = new List<TickTimer>();
            foreach (var flippy in _flippyTimers)
            {
                flippy.Update(tickCounter);

                if (!flippy.Running)
                {
                    finished.Add(flippy);
                }
            }

            lock (_flippyTimers)
            {
                foreach (var finishedFlippy in finished)
                {
                    _flippyTimers.Remove(finishedFlippy);
                }
            }*/

        }

        /// <summary>
        /// Drops a given item to a random position close to the player
        /// </summary>
        /// <param name="player">Player to which to reveal the item</param>
        /// <param name="item">Item to reveal</param>
        /*public void DropItem(Player player, Item item)
        {
            DropItem(player, player, item);
        }*/

        /// <summary>
        /// Drops a given item to a random position close to a source actor
        /// </summary>
        /// <param name="source">Source actor of the flippy animation</param>
        /// <param name="player">Player to which to reveal the item</param>
        /// <param name="item">Item to reveal</param>
        /*public void DropItem(Actor source, Player player, Item item)
        {
           /* lock (_flippyTimers)
            {
                _flippyTimers.Add(new RelativeTickTimer(
                    Game,
                    FlippyDurationInTicks,
                    (p) => item.Drop(null, item.Position)             // drop the item after FlippyDuration ticks
                    ));
            }

            // Get a random location close to the source
            // TODO Make sure the location actually allows items (not occupied by something)
            float x = (float)(RandomHelper.NextDouble() - 0.5) * FlippyMaxDistanceManhattan;
            float y = (float)(RandomHelper.NextDouble() - 0.5) * FlippyMaxDistanceManhattan;
            item.Position = source.Position + new Vector3D(x, y, 0);

            // Items send either only a particle effect or default particle and either FlippyTag.Actor or their own actorsno
            int particleSNO = -1;
            int actorSNO = -1;

            if (item.SnoFlippyParticle != null)
            {
                particleSNO = item.SnoFlippyParticle.Id;
            }
            else
            {
                actorSNO = item.SnoFlippyActory == null ? -1 : item.SnoFlippyActory.Id;
                particleSNO = FlippyDefaultFlippy;
            }

            BroadcastIfRevealed(new FlippyMessage
            {
                ActorID = (int)source.DynamicID,
                Destination = item.Position,
                SNOFlippyActor = actorSNO,
                SNOParticleEffect = particleSNO
            }, source);

        }*/

        #endregion

        #region collections managemnet

        /// <summary>
        /// Adds given actor to world.
        /// </summary>
        /// <param name="actor">The actor to add.</param>
        private void addActor(Actor actor)
        {
            if (actor.DynamicID == 0 || HasActor(actor.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", actor.DynamicID));

            this.Actors.TryAdd(actor.DynamicID, actor); // add to actors collection.
            this.QuadTree.Insert(actor); // add it to quad-tree too.

            if (actor.ActorType == ActorType.Player) // if actor is a player, add it to players collection too.
            {
                this.AddPlayer((Player)actor);
            }

            if (actor.ActorType == ActorType.Monster) // if actor is a player, add it to players collection too.
            {
                this.Monsters.TryAdd(actor.DynamicID, (Monster)actor);
            }

            if (actor.ActorType == ActorType.Projectile) // if actor is a player, add it to players collection too.
            {
                this.Projectiles.TryAdd(actor.DynamicID, (Projectile)actor);
            }

            if (actor.ActorType == ActorType.Item) // if actor is a player, add it to players collection too.
            {
                this.Items.TryAdd(actor.DynamicID, (Item)actor);
            }
        }

        /// <summary>
        /// Removes given actor from map.
        /// </summary>
        /// <param name="actor">The actor to remove.</param>
        private void removeActor(Actor actor)
        {
            if (actor.DynamicID == 0 || !this.Actors.ContainsKey(actor.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", actor.DynamicID));

            Actor removedActor;
            this.Actors.TryRemove(actor.DynamicID, out removedActor); // remove it from actors collection.
            this.QuadTree.Remove(actor); // remove from quad-tree too.

            if (actor.ActorType == ActorType.Player) 
            {
                this.RemovePlayer((Player)actor);
            }

            if (actor.ActorType == ActorType.Monster) 
            {
                Monster outt;
                this.Monsters.TryRemove(actor.DynamicID, out outt);
            }

            if (actor.ActorType == ActorType.Projectile) 
            {
                Projectile outt;
                this.Projectiles.TryRemove(actor.DynamicID, out outt);
            }

            if (actor.ActorType == ActorType.Item)
            {
                Item outt;
                this.Items.TryRemove(actor.DynamicID, out outt);
            }
        }

        /// <summary>
        /// Returns true if the map has an actor with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the actor.</param>
        /// <returns><see cref="bool"/></returns>
        public bool HasActor(int dynamicID)
        {
            return this.Actors.ContainsKey(dynamicID);
        }

        /// <summary>
        /// Adds given player to map.
        /// </summary>
        /// <param name="player">The player to add.</param>
        private void AddPlayer(Player player)
        {
            if (player.DynamicID == 0)
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", player.DynamicID));

            this.Players.TryAdd(player.DynamicID, player); // add it to players collection.
        }

        /// <summary>
        /// Removes given player from map.
        /// </summary>
        /// <param name="player"></param>
        private void RemovePlayer(Player player)
        {
            if (player.DynamicID == 0 || !this.Players.ContainsKey(player.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", player.DynamicID));

            Player removedPlayer;
            this.Players.TryRemove(player.DynamicID, out removedPlayer); // remove it from players collection.
        }

        #endregion

        #region destroy, ctor, finalizer

        public override void Destroy()
        {
        }

        #endregion

    }

    
}

