using System;
using System.Linq;
using System.Collections.Generic;

using Dirac.Logging;
using Dirac.GameServer.Types;
using Dirac.GameServer;
using Dirac.Extensions;
using Dirac.GameServer.Core;
//using Dirac.BulletPhysics;
using Dirac.GameServer.Network;
using Dirac.GameServer.Network.Message;
using Dirac.Math;

namespace Dirac.GameServer.Core
{
    public class Projectile : Actor
    {
        public override ActorType ActorType { get { return ActorType.Projectile; } }

        //public BulletSharp.RigidBody RigidBody { get; set; }

        //public static BulletSharp.CollisionShape ProjectileCollisionShape = new BulletSharp.SphereShape(0.3f);

        public SkillContext Context;

        public Action<Actor> OnCollision = null;
        public Action OnUpdate = null;
        public Action OnArrival = null;
        public Action OnTimeout = null;

        public LinearTrajectorie LinearTrajectorie { get; set; }
        
        public bool DestroyOnArrival = false;
        public TickTimer Timer;
        public Vector3 LastPosition;
        public Vector3 Direction;
        public int TimeOut { get; set; }
        public override float TranslateSpeed { get; set; }

        public override Vector3 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
            }
        }

        //private bool _spawned;  // using my own spawn flag cause Actor.Spawned isn't being used right now

        public Projectile(SkillContext context, int actorSNO, Vector3 UserPosition)
            : base(actorSNO)
        {
            this.World = context.World;
            IsAlreadyDestroyed = false;

            //ObjectDescription = new BoundingSphere(new Vector3(0, 0, 0), new Real(0.1f));

            TranslateSpeed = 1f;

            /*this.PxActor =*/ 
            /*Dirac.Engine.PhysXEngine.PhysXConcurrentActions.Enqueue(() =>
                );*/

            if (this.Scale == 0f)
                this.Scale = 1.00f;

            this.Context = context;
            this.Position = new Vector3(UserPosition.x, UserPosition.y, UserPosition.z); // sin este vector3d al cambiar pos se cambia user pos ric0

            this.TimeOut = 3000; //default
            
            // 2 second default timeout for projectiles
            //this.Timer = new TickTimer(2000);

            LastPosition = this.Position /*era null*/;
            
            //PowerManager.ProjectilesToUpdate.Add(this.DynamicID);
        }

        #region launchers

        public void LaunchDefaultPosition()
        {
            Executor.Execute(TimeOut, () =>
            {
                this.Destroy();
            });

            LastPosition = this.Position;

            //this.TranslateFacing(targetPosition, true);
            Vector3 targetPosition = new Vector3(this.Context.DestinationUserClick.x, this.Context.DestinationUserClick.y, this.Context.DestinationUserClick.z);
            //targetPosition.Z += this.ActorData.Cylinder.Ax1 - this.ActorData.Cylinder.Position.Z;

            //this.EnterWorld(this.Position); //esto llama enterworld en cliente
            this.World.Enter(this, this.Position);

            /*this.PxActor.LinearVelocity = new PxVector3(Direction.x, Direction.y, Direction.z) * 8f;
            this.MoveTo(targetPosition, 1f);*/
            this.MoveTo(targetPosition, 1f);
        }

        public void LaunchDefaultVector()
        {
            Executor.Execute(TimeOut, () =>
            {
                this.Destroy();
            });

            LastPosition = this.Position;

            //this.TranslateFacing(targetPosition, true);
            //Vector3 targetPosition = new Vector3(this.Context.DestinationUserClick.x, this.Context.DestinationUserClick.y, this.Context.DestinationUserClick.z);
            //targetPosition.Z += this.ActorData.Cylinder.Ax1 - this.ActorData.Cylinder.Position.Z;

            //this.EnterWorld(this.Position); //esto llama enterworld en cliente
            this.World.Enter(this, this.Position);

            /*this.PxActor.LinearVelocity = new PxVector3(Direction.x, Direction.y, Direction.z) * 8f;
            this.MoveTo(targetPosition, 1f);*/
            Vector3 FinalPointFromDirection = this.Position + this.Context.VectorDirector;

            this.MoveTo(FinalPointFromDirection, 1f);
        }

        public void LaunchToPosition(Vector3 TargetPosition)
        {
            Executor.Execute(this.TimeOut, () =>
            {
                this.Destroy();
            });

            LastPosition = this.Position;


            //this.EnterWorld(this.Position); //esto llama enterworld en cliente
            this.World.Enter(this, this.Position);

            /*this.PxActor.LinearVelocity = new PxVector3(Direction.x, Direction.y, Direction.z) * 8f;
            this.MoveTo(targetPosition, 1f);*/
            this.MoveTo(TargetPosition, 1f);
        }

        public void LaunchToVector(Vector3 vectorDirection)
        {
            Executor.Execute(TimeOut, () =>
            {
                this.Destroy();
            });

            LastPosition = this.Position;


            //this.EnterWorld(this.Position); //esto llama enterworld en cliente
            this.World.Enter(this, this.Position);

            Vector3 FinalPointFromDirection = this.Position + vectorDirection;
            /*this.PxActor.LinearVelocity = new PxVector3(Direction.x, Direction.y, Direction.z) * 8f;
            this.MoveTo(targetPosition, 1f);*/
            this.MoveTo(FinalPointFromDirection, 1f);
        }

        #endregion

        

        public override void MoveTo(Vector3 destiny, float facingAngle)
        {
            //esto se hace tambien en server, pero deberia ser mejor si ACDTranslateNormalMessage tuviera un field q sea
            //initPosition ademas de destiny.
            Vector3 newStartPointOutSide = (this.Position + (destiny - this.Position).NormalizedCopy * 2f) + Vector3.UnitY; //esta suma le da altura, pa q no salga del piso
            Vector3 newPosition_y_up = new Vector3(this.Position.x, newStartPointOutSide.y, this.Position.z);

            destiny = new Vector3(destiny.x, newStartPointOutSide.y, destiny.z); //dest es igual de alto que start, sino sale torcido.

            /*this.RigidBody = BulletPhysics.BasicDemo.Instance.LocalCreateRigidBody(0.0000000000001f, newStartPointOutSide, Projectile.ProjectileCollisionShape);
            this.RigidBody.Gravity = BulletSharp.Vector3.Zero;
            this.RigidBody.LinearVelocity = (newStartPointOutSide - newPosition_y_up).NormalizedCopy.ToBulletV3() * 100;
            this.RigidBody.UserObject = this;

            BulletPhysics.BasicDemo.Instance.AddRigidBodyToWorldConcurrent(this.RigidBody);*/

            this.LinearTrajectorie = new LinearTrajectorie(this.Position, destiny);

            //Managers.MovementManager.MoveTo(this, this.Position, destiny, this.Speed);

            var movementMessage = new ACDTranslateNormalMessage
            {
                ActorId = (int)this.DynamicID,
                Position = destiny,

                Angle = facingAngle,
                //lookAt = destiny,
                TurnImmediately = false,
                Speed = this.TranslateSpeed, //deberia ser translate speed
                Field5 = 0,
            };

            this.World.BroadcastIfRevealed(movementMessage, this);

        }

        public override void Destroy()
        {
            if (this.IsAlreadyDestroyed)
                return;

            //this.RigidBody.CollisionShape.Dispose();
            //BulletPhysics.BasicDemo.Instance.RemoveRigidBodyFromWorldConcurrent(this.RigidBody);

            this.StopMoving();
            base.Destroy();
        }

        public override void StopMoving()
        {
        }

        public override bool Reveal(Player player)
        {
            this.InitialLookAt = this.Context.DestinationUserClick;
            return base.Reveal(player);
        }

        public override void OnLeave(Map world)
        {
            base.OnLeave(world);
        }

        public override void OnEnter(Map world)
        {
            base.OnEnter(world);
        }

        #region IUpdateable
        public override void Update(TimeSpan elapsed)
        {
            base.Update(elapsed);

            /*if (this.LinearTrajectorie == null)
                return;*/

            //puede ser que RigidBody sea null porque el thread de Bullet no ejecuto el ActionConcurrent para crear el rigidBody.
            /*if (this.RigidBody != null) 
                this.Position = this.RigidBody.CenterOfMassPosition.ToDiracV3();*/


            //this.Position = this.LinearTrajectorie.Advance(lastElapsedticks, this.TranslateSpeed);
            //Logging.LogManager.DefaultLogger.Trace(this.LinearTrajectorie.CurrentLen + " / " + this.LinearTrajectorie.PathLen);
        }
        #endregion
    }
}
