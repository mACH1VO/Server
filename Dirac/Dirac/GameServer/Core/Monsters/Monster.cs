using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Dirac.GameServer.Types;
using Dirac.Store.FileFormats;
using Dirac.GameServer.Core.AI.Brains;
using Dirac.Extensions;

using Dirac.Math;

//using BulletSharp;

using Vector3 = Dirac.Math.Vector3;

namespace Dirac.GameServer.Core
{
    public partial class Monster : Actor
    {
        public override ActorType ActorType { get { return ActorType.Monster; } }

        public virtual MUPath Path { get; set; } //nose si esto va aca o en brain.

        public override Dirac.Math.Vector3 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                /*if (this.Character != null)
                   this.Character.Warp((value + Vector3.Zero * 3).ToBulletV3());*/
                base.Position = value;
            }
        }

        /*public BulletSharp.PairCachingGhostObject GhostObject { get; set; }
        public BulletSharp.KinematicCharacterController Character { get; set; }*/

        public MonsterData Data { get; set; }
        public String FileData { get; set; }
        public virtual BaseBrain Brain { get; set; }

        public Boolean IsFighting { get; set; }
        public Boolean IsEvading { get; set; }
        public Boolean HasMaster { get; set; }
        public Monster Master { get; set; }

        public Monster(int snoId)
            : base(snoId)
        {
            this.FileData = Actor.SNOToFile[SNOId];
            this.Data = new MonsterData();
            this.Data = MonsterData.XMLDeserialize(this.FileData + ".xml");

            this.TranslateSpeed = 0.2f;
            
            //this.Attributes[GameAttributeStaticList.Hitpoints_Max] = 1510;
            //this.Attributes[GameAttributeStaticList.Hitpoints_Cur] = 802.1f;
            

            //this.Attributes.BroadcastChangedIfRevealed();
        }

        public Monster createDefaultBrain()
        {
            this.Brain = new BaseBrain(this, BrainState.Roam);
            this.Brain.Start();
            this.Brain.SourcePoint = new Vector3(0, 0, 0);
            return this;
        }

        public override void Die()
        {
            if (this.IsDead)
                return;
            //play anim death
            this.DropItem();
            base.Die();

        }

        public virtual void DropItem()
        {
            int _ITEM_SNO_ID = RandomHelper.Next(101, 106);
            _ITEM_SNO_ID = RandomHelper.Next(5033, 5040);
            Item item = ItemFactory.CreateItem(_ITEM_SNO_ID);
            this.World.Enter(item);
        }

        public override void OnGetHit()
        {
            // each time an arrow hits a mob, physics asset moves, so need to put it back
            // in the right place 
            // and this dont send pkt to client!
            this.Position = this.Position;
            //this.MoveTo(new Vector3(RandomHelper.Next(-20, 20), 1/*up*/, RandomHelper.Next(-20, 20)), 1f);
        }

        public override void OnKill()
        {
            Executor.Execute(3000, () =>
            {
                /*Monster m = MonsterFactory.Create(51011).createDefaultBrain();
                this.World.Enter(m);*/

                Vector3 post = new Vector3(RandomHelper.Next(0, 10), 0, RandomHelper.Next(0, 10));
                Monster m = MonsterFactory.Create(this.SNOId).createDefaultBrain();
                this.World.Enter(m, post);
                
            });
           
            base.OnKill();
        }

        public override void MoveTo(Vector3 destiny, float facingAngle)
        {
            //Managers.MovementManager.MoveTo(this, this.Position, destiny, this.Speed);
            this.Path = this.World.FindPath(this.Position, destiny);
            
            if (this.Path == null)
                return;

            //Logging.LogManager.DefaultLogger.Trace("LinearTrajectoriesCount " + Path.LinearTrajectoriesCount.ToString());
            this.IsMoving = true;

            /*var movementMessage = new ACDTranslateNormalMessage
            {
                ActorId = (int)this.DynamicID,
                Position = destiny,

                Angle = facingAngle,
                lookAt = destiny,
                TurnImmediately = false,
                Speed = this.TranslateSpeed, //deberia ser translate speed
                Field5 = 0,
            };

            this.World.BroadcastIfRevealed(movementMessage, this);*/

        }

        public override void StopMoving()
        {
            this.IsMoving = false;
            base.StopMoving();
        }

        public override void Update(TimeSpan elapsed)
        {
            if (this.Brain != null)
                this.Brain.Update(elapsed);

            this.updateMovement(elapsed);
            base.Update(elapsed);
        }

/*public override void OnTargeted(Player player, TargetMessage message)
        {
            player.Attributes[GameAttribute.Last_ACD_Attacked] = (int)this.DynamicID;
            player.Attributes[GameAttribute.Projectile_Speed] = 3.05f;
            player.Attributes.BroadcastChangedIfRevealed();


            if (this.Attributes[GameAttribute.Hitpoints_Cur] - 55 < 0) // si se muere con el prox hit
            {

                FlippyMessage flip = new FlippyMessage()
                {
                    ActorID = (int)this.DynamicID,
                    SNOParticleEffect = 0x00006D82,
                    SNOFlippyActor = 0x000010AB,
                    Destination = this.Position,
                };

                // player.InGameClient.SendMessage(flip);

                player.InGameClient.SendMessage(
                    new PlayHitEffectMessage()
                    {
                        ActorID = this.DynamicID,
                        HitDealer = player.DynamicID,
                        Field2 = 0x00000000,
                        Field3 = false
                    }
                    );

                player.InGameClient.SendMessage(
                    new PlayEffectMessage()
                    {
                        ActorId = this.DynamicID,
                        Effect = Effect.Hit,
                        OptionalParameter = 1,
                    }
                    );

                this.Attributes[GameAttribute.Mana_Cur] = 0;
                this.Attributes.BroadcastChangedIfRevealed();


                SetIdleAnimationMessage idle = new SetIdleAnimationMessage()
                {
                    ActorID = this.DynamicID,
                    AnimationSNO = 0x00011710,
                };
                player.InGameClient.SendMessage(idle);


                player.InGameClient.SendMessage(
                    new ACDDestroyActorMessage()
                    {
                        ActorId = this.DynamicID,
                    }
                    );

                player.InGameClient.SendMessage(
                    new PlayAnimationMessage()
                    {
                        ActorID = this.DynamicID,
                        Field1 = 11,
                        Field2 = 0,
                        tAnim = new PlayAnimationMessageSpec[]
                        {
                            new PlayAnimationMessageSpec()
                            {
                                Duration = 2,
                                AnimationSNO = 11512, //die
                                PermutationIndex = 0,
                                Speed = 1
                            }
                        }

                    });


                this.Attributes[GameAttribute.Deleted_On_Server] = true; //estos 2 van si o si con un playanimationmessage antes sino no anda nada
                this.Attributes[GameAttribute.Could_Have_Ragdolled] = true;
                this.Attributes.BroadcastChangedIfRevealed();


                //this.Attributes[GameAttribute.Hitpoints_Cur] = 0;
                //this.Attributes[GameAttribute.Last_Damage_ACD] = 0x789F004E;



               


                player.InGameClient.SendMessage(
                    new PlayEffectMessage()
                    {
                        ActorId = this.DynamicID,
                        Effect = Effect.Hit, 
                        OptionalParameter = 1,
                    }
                    );

                player.InGameClient.SendMessage(
                    new PlayEffectMessage()
                    {
                        ActorId = this.DynamicID,
                        Effect = Effect.Unknown12, 
                    }
                    );

                player.InGameClient.SendMessage(
                    new PlayEffectMessage()
                    {
                        ActorId = this.DynamicID,
                        Effect = Effect.Gore//Effect.Gore
                    }
                    );

               

                player.InGameClient.SendMessage(
                    new PlayEffectMessage()
                    {
                        ActorId = this.DynamicID,
                        Effect = Effect.BloodSplash,
                        OptionalParameter = (int)player.DynamicID,
                    }
                    );


                player.InGameClient.SendMessage(
                    new PlayAnimationMessage()
                    {
                        ActorID = this.DynamicID,
                        Field1 = 11,
                        Field2 = 0,
                        tAnim = new PlayAnimationMessageSpec[]
                        {
                            new PlayAnimationMessageSpec()
                            {
                                Duration = 30,
                                AnimationSNO = 0x00002CF8, //die
                                PermutationIndex = 69648,
                                Speed = 0
                            }
                        }

                    });


                player.InGameClient.SendMessage(
                    new ACDDestroyActorMessage()
                    {
                        ActorId = this.DynamicID,
                    }
                    );

                Item todrop = ItemFactory.GenerateRandombyLevel(this.World, this.MonsterData.Level.Normal);
                todrop.EnterWorld(this.Position);

            }
            else
            {
                int tosend = RandomHelper.Next(1,69);
                player.InGameClient.SendMessage(
                   new PlayEffectMessage()
                   {
                       ActorId = this.DynamicID,
                       Effect = (Effect)tosend,
                   }
                   );

                Dirac.Logging.LogManager.DefaultLogger.Trace("EffectPlayed [{0}]", Enum.GetName(typeof(Effect),tosend));
                GlobalForm.Form.richTextBox_Main.ScrollToCaret();

                this.Attributes[GameAttribute.Hitpoints_Cur] = this.Attributes[GameAttribute.Hitpoints_Cur] - 55;
                this.Attributes.BroadcastChangedIfRevealed();
                player.InGameClient.SendMessage(
                    new FloatingNumberMessage()
                    {
                        ActorID = this.DynamicID,
                        Number = 1.4f,
                        Type = FloatingNumberMessage.FloatType.White,
                    }
                    );
                player.InGameClient.SendMessage(
                    new FloatingNumberMessage()
                    {
                        ActorID = this.DynamicID,
                        Number = 10f,
                        Type = FloatingNumberMessage.FloatType.White,
                    }
                    );

                
               
                player.InGameClient.SendMessage(new PlayAnimationMessage
                {
                    ActorID = this.DynamicID,
                    Field1 = 6,
                    Field2 = 0,
                    tAnim = new PlayAnimationMessageSpec[]
                    {
                        new PlayAnimationMessageSpec()
                        {
                            Duration = 26,
                            AnimationSNO =  AnimationSet.TagMapAnimDefault[AnimationSetKeys.GetHit],
                            PermutationIndex = 0,
                            Speed = 1.82f
                        }
                }
                });
            }*/


        public override void OnEnter(Map world)
        {
            /*this.GhostObject = new PairCachingGhostObject();
            this.GhostObject.WorldTransform = Matrix.Translation(this.Position.x, this.Position.y, this.Position.z);
            this.GhostObject.UserObject = this;
            //Broadphase.OverlappingPairCache.SetInternalGhostPairCallback(new GhostPairCallback());

            ConvexShape capsule = new CylinderShape(new Vector3(5f, 5f, 5f).ToBulletV3());
            this.GhostObject.CollisionShape = capsule;
            this.GhostObject.CollisionFlags = CollisionFlags.CharacterObject;

            //const float stepHeight = 9.35f;
            this.Character = new KinematicCharacterController(GhostObject, capsule, 1);
            this.Character.SetFallSpeed(0);

            BulletPhysics.BasicDemo.Instance.Execute(() =>
                {
                    BulletPhysics.BasicDemo.Instance.World.AddCollisionObject(GhostObject, CollisionFilterGroups.CharacterFilter, CollisionFilterGroups.StaticFilter | CollisionFilterGroups.DefaultFilter);
                    BulletPhysics.BasicDemo.Instance.World.AddAction(Character);
                });*/

            base.OnEnter(world);
        }

        public override void Destroy()
        {
            /*BulletPhysics.BasicDemo.Instance.Execute(() =>
                {
                    try
                    {
                        if (this.GhostObject != null)
                        {
                            if (this.GhostObject.CollisionShape != null)
                            {
                                //this.GhostObject.CollisionShape.Dispose(); //sin ser nada null a veces tira error.
                            }
                            BulletPhysics.BasicDemo.Instance.World.RemoveCollisionObject(this.GhostObject);
                        }

                        if (this.Character != null)
                        {
                            BulletPhysics.BasicDemo.Instance.World.RemoveAction(this.Character);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.LogManager.DefaultLogger.Error(ex.Message);
                    }

                });*/

            base.Destroy();
        }
    }
}