using System.Collections.Generic;
using Dirac.GameServer.Types;
using System.Linq;
using System.Windows;
using System.Xml;
using System;
using System.Xml.Linq;
using Dirac.Logging;
using Dirac.Math;
using Rect = System.Windows.Rect;

using Dirac.Extensions;
using Dirac.GameServer.Network;
using Dirac.GameServer.Network.Message;


namespace Dirac.GameServer.Core
{
    public abstract class Actor : WorldObject , IUpdateable
    {

        public virtual float TranslateSpeed { get; set; }

        /*public Vector3 InitialPositionMovement { get; set; }
        public Vector3 DestinyPositionMovement { get; set; }*/

        public int SNOId { get; set; }

        public abstract ActorType ActorType { get; }

        public bool Spawned { get; set; }

        public virtual Boolean IsDead { get; set; }

        public const int DefaultQueryProximityLenght = 100;

        public const int DefaultQueryProximityRadius = 100;

        public GameAttributes Attributes { get; set; }

        public int CollFlags { get; set; }

        private bool visible;
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if (value != visible)
                {
                    /*this.World.BroadcastIfRevealed(new ACDChangeActorVisibility()
                        {
                            ActorID = this.DynamicID, 
                            Visible = value
                        }, this);*/
                }
                this.visible = value;
            }
        }

        public float WalkSpeed = 0.108f;

        public float RunSpeed = 0.3598633f;

        public virtual Vector3 InitialLookAt { get; set; }

        public virtual Boolean IsMoving { get; set; }

        protected Actor(int snoId)
            : base(Game.NewActorID)
        {
            Spawned = false;
            Attributes = new GameAttributes(this);
            //Attributes[GameAttribute.Account_Under_Review];
            this.SNOId = snoId;
            this.InitialLookAt = new Vector3(0, 0, 0);
            this.TranslateSpeed = 0.1f; //default
            this.IsMoving = false;
        }

        

        #region enter-world, change-world, teleport helpers


        public virtual void BeforeChangeWorld()
        {

        }

        public virtual void AfterChangeWorld()
        {

        }


        public void ChangeWorld(Map world, Vector3 position)
        {
            if (this.World == world)
                return;

            this.Position = position;

            if (this.World != null) // if actor is already in a existing-world
                this.World.Leave(this); // make him leave it first.

            BeforeChangeWorld();

            this.World = world;
            if (this.World != null) // if actor got into a new world.
                this.World.Enter(this); // let him enter first.

            AfterChangeWorld();
            //world.BroadcastIfRevealed(this.ACDWorldPositionMessage, this);
        }

        public void Teleport(Vector3 position)
        {
            this.Position = position;
            this.OnTeleport();
            //this.World.BroadcastIfRevealed(this.ACDWorldPositionMessage, this);
        }

        #endregion

        public override void Destroy()
        {
            //Projectiles destroy, but monsters DIE.
            base.Destroy();
        }

        public virtual void Die()
        {
            this.OnKill();
            this.Destroy();
            this.IsDead = true;
        }

        public void PlayAnimation(int animationType, int animationSNO, float speed = 1.0f, int? ticksToPlay = null)
        {
            if (this.World == null) 
                return;

            /*this.World.BroadcastIfRevealed(new PlayAnimationMessage
            {
                ActorID = this.DynamicID,
                Field1 = animationType,
                Field2 = 0,
                tAnim = new PlayAnimationMessageSpec[]
                {
                    new PlayAnimationMessageSpec
                    {
                        Duration = ticksToPlay.HasValue ? ticksToPlay.Value : -2,  // -2 = play animation once through
                        AnimationSNO = animationSNO,
                        PermutationIndex = 0x0,  // TODO: implement variations?
                        Speed = speed,
                    }
                }
            }, this);*/
        }

        public void PlayActionAnimation(int animationSNO, float speed = 1.0f, int? ticksToPlay = null)
        {
            PlayAnimation(3, animationSNO, speed, ticksToPlay);
        }


        #region IUpdateable
        public virtual void Update(TimeSpan elapsed)
        {

        }
        #endregion


        #region reveal & unreveal handling

        public bool IsRevealedToPlayer(Player player)
        {
            return player.RevealedObjects.ContainsKey(this.DynamicID);
        }


        /// <summary>
        /// Reveals an actor to a player.
        /// </summary>
        /// <returns>true if the actor was revealed or false if the actor was already revealed.</returns>
        public override bool Reveal(Player player)
        {
            if (player.RevealedObjects.ContainsKey(this.DynamicID)) 
                return false; // already revealed
            if (!player.RevealedObjects.TryAdd(this.DynamicID, this))
                Logging.LogManager.DefaultLogger.Error("player.RevealedObjects.TryAdd error");

            ACDCreateActorMessage createActorMsg =  new ACDCreateActorMessage
            {
                DynamicID = this.DynamicID,
                ActorSNOId = this.SNOId,
                ActorType = this.ActorType,
                Field2 = 0,
                Field3 = 0, //ojo
                /*WorldLocation = this.WorldLocationMessage,//this.HasWorldLocation ? this.WorldLocationMessage : null,
                InventoryLocation = null,//this.HasWorldLocation ? null : this.InventoryLocationMessage,
                Field7 = 0,
                NameSNOId = 0,
                Quality = 0,
                Field10 = 0,
                Field11 = 0,
                MarkerSetSNO = 0,
                MarkerSetIndex = 0,*/
            };

            // normaly when we send acdenterknown for players own actor it's set to 0x09. But while sending the acdenterknown for another player's actor we should set it to 0x01. /raist
            if ((this is Player) && this != player)
                createActorMsg.Field2 = 0x01;

            if (player.GameClient != null)
                player.GameClient.SendMessage(createActorMsg);


            // Collision Flags
            /*player.GameClient.SendMessage(new ACDCollFlagsMessage
            {
                ActorID = DynamicID,
                CollFlags = this.CollFlags
            });*/

            /*player.GameClient.SendMessage(new ACDGroupMessage
            {
                ActorID = DynamicID,
                Group1Hash = -1,
                Group2Hash = -1,
            });*/

            if (player.GameClient != null)
                player.GameClient.SendMessage(
                    new ACDEnterMapMessage()
                    {
                        ActorId = this.DynamicID,
                        Position = this.Position,
                        Orientation = this.Orientation,
                        MapId = this.World.DynamicID,
                        Scale = 1f
                    }
                    );

            this.Attributes.BroadcastAllAttributestoPlayer(player);

            return true;
        }

        /// <summary>
        /// Unreveals an actor from a player.
        /// </summary>
        /// <returns>true if the actor was unrevealed or false if the actor wasn't already revealed.</returns>
        public override bool Unreveal(Player player)
        {
            if (!player.RevealedObjects.ContainsKey(this.DynamicID)) 
                return false; // not revealed yet
            if (player.GameClient != null)
                player.GameClient.SendMessage(new ACDDestroyActorMessage((uint)this.DynamicID));

            IRevealable outt;
            if (!player.RevealedObjects.TryRemove(this.DynamicID, out outt))
                Logging.LogManager.DefaultLogger.Error("player.RevealedObjects.TryRemove");

            return true;
        }

        #endregion

        #region proximity-based query helpers

        #region circurlar region queries

        public List<Player> GetPlayersInRange(float radius = DefaultQueryProximityRadius)
        {
            return this.GetObjectsInRange<Player>(radius);
        }

        public List<Item> GetItemsInRange(float radius = DefaultQueryProximityRadius)
        {
            return this.GetObjectsInRange<Item>(radius);
        }

        public List<Monster> GetMonstersInRange(float radius = DefaultQueryProximityRadius)
        {
            return this.GetObjectsInRange<Monster>(radius);
        }

        public List<Actor> GetActorsInRange(float radius = DefaultQueryProximityRadius)
        {
            return this.GetObjectsInRange<Actor>(radius);
        }

        public List<T> GetActorsInRange<T>(float radius = DefaultQueryProximityRadius) where T : Actor
        {
            return this.GetObjectsInRange<T>(radius);
        }

        public List<WorldObject> GetObjectsInRange(float radius = DefaultQueryProximityRadius)
        {
            return this.GetObjectsInRange<WorldObject>(radius);
        }

        public List<T> GetObjectsInRange<T>(float radius = DefaultQueryProximityRadius) where T : WorldObject
        {
            var proximityCircle = new Circle(this.Position.x, this.Position.z, radius);
            return this.World.QuadTree.Query<T>(proximityCircle);
        }

        #endregion

        #region rectangluar region queries

        public List<Player> GetPlayersInRegion(int lenght = DefaultQueryProximityLenght)
        {
            return this.GetObjectsInRegion<Player>(lenght);
        }

        public List<Item> GetItemsInRegion(int lenght = DefaultQueryProximityLenght)
        {
            return this.GetObjectsInRegion<Item>(lenght);
        }

        public List<Monster> GetMonstersInRegion(int lenght = DefaultQueryProximityLenght)
        {
            return this.GetObjectsInRegion<Monster>(lenght);
        }

        public List<Actor> GetActorsInRegion(int lenght = DefaultQueryProximityLenght)
        {
            return this.GetObjectsInRegion<Actor>(lenght);
        }

        public List<T> GetActorsInRegion<T>(int lenght = DefaultQueryProximityLenght) where T : Actor
        {
            return this.GetObjectsInRegion<T>(lenght);
        }

        public List<WorldObject> GetObjectsInRegion(int lenght = DefaultQueryProximityLenght)
        {
            return this.GetObjectsInRegion<WorldObject>(lenght);
        }

        public List<T> GetObjectsInRegion<T>(int lenght = DefaultQueryProximityLenght) where T : WorldObject
        {
            var proximityRectangle = new Rect(this.Position.x - lenght / 2, this.Position.y - lenght / 2, lenght, lenght);
            return this.World.QuadTree.Query<T>(proximityRectangle);
        }

        #endregion
        #endregion

        #region events

        public virtual void OnEnter(Map world)
        {

            /*Entity = OgreUpdate.SceneManager.CreateEntity(SceneManager.PrefabType.PT_CUBE);
            Entity.SetMaterialName("facial.material");
            SceneNode = OgreUpdate.SceneManager.RootSceneNode.CreateChildSceneNode();
            SceneNode.AttachObject(Entity);*/
            Visible = true;
            Spawned = true;
            
        }

        public virtual void OnGetHit()
        {

        }

        public virtual void OnKill()
        {

        }

        public virtual void OnLeave(Map world)
        {

            //if (GlobalForm.Form.openGLControl1.Scene.SceneElements.Contains(GLSphere))
                //GlobalForm.Form.openGLControl1.Scene.RemoveElement(GLSphere); ;

            Spawned = false;

        }

        public virtual void OnTargeted(Player player, TargetMessage message)
        {

        }

        public virtual void OnTeleport()
        {

        }

        /// <summary>
        /// Called when a player moves close to the actor
        /// </summary>
        public virtual void OnPlayerApproaching(Player player)
        {

        }

        #endregion

        #region movement

        public virtual void MoveTo(Vector3 point, float facingAngle) 
        {
            //remember to use it with detourcrownd instead of manually moving it!

            /*this.InitialPositionMovement = this.Position.ToPxVector3();
            this.DestinyPositionMovement = point.ToPxVector3();

            PhysXEngine.PhysXConcurrentActions.Enqueue(() =>
                {
                    Managers.MovementManager.ActorsMoving.TryAdd(this.DynamicID, this);
                    PxVector3 PathVectorToVelocity = this.PathVector;
                    PathVectorToVelocity.Normalize();
                    this.PxActor.LinearVelocity = PathVectorToVelocity * this.Speed; 
                });*/
            

            //this.Position = point;  // TODO: will need update Position over time, not instantly.
            //this.SetFacingRotation(facingAngle);

            var movementMessage = new ACDTranslateNormalMessage
            {
                ActorId = (int)this.DynamicID,
                Position = point,
                
                Angle = facingAngle,
                //lookAt = point,
                TurnImmediately = false,
                Speed = this.WalkSpeed, 
                Field5 = 0,
            };

            this.World.BroadcastIfRevealed(movementMessage, this);
        }

        public virtual void StopMoving()
        {
            
        }

        #endregion

        public override string ToString()
        {
            return string.Format("[Actor] [Type: {0}] SNOId:{1} DynamicId: {2} Position: {3} Name: {4}", this.ActorType, this.SNOId, this.DynamicID, this.Position, "sasda");
        }

        public static Dictionary<int, String> SNOToFile = new Dictionary<int, String>();
        public static void Initialize()
        {
            Store.FileFormats.ActorData agd = new Store.FileFormats.ActorData();

            agd = Store.FileFormats.ActorData.XMLDeserialize("ActorSNOList.xml");


            foreach (var item in agd.ActorSNOMapList)
            {
                if (!Actor.SNOToFile.ContainsKey(item.SNO))
                    Actor.SNOToFile.Add(item.SNO, item.FileName);
                else
                    Logging.LogManager.DefaultLogger.Warn("KeyAlready added in sno List = {0}", item.SNO);
            }

            string basedir = "E:\\MegaProject\\MPQ\\";
            foreach (var item in SNOToFile)
            {
                if (System.IO.File.Exists(basedir + item.Value + ".xml"))
                {
                    String allText = System.IO.File.ReadAllText(basedir + item.Value + ".xml");
                    if (allText.Contains("ItemData"))
                        ItemFactory.SNOIds.Add(item.Key);
                }
            }
        }
    }

    public enum StatType
    {
        Strength = 0,
        Agility = 1,
        Stamina = 2,
        Energy = 3,
    }

    public enum ActorType
    {
        Invalid = 0,
        Monster = 1,
        Gizmo = 2,
        ClientEffect = 3,
        ServerProp = 4,
        Enviroment = 5,
        Critter = 6,
        Player = 7,
        Item = 8,
        AxeSymbol = 9,
        Projectile = 10,
        CustomBrain = 11,
        NPC = 12
    }
}
