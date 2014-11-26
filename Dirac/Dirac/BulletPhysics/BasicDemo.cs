/*using System;
using System.Collections.Concurrent;
using BulletSharp;
using DemoFramework;

using Dirac.GameServer;
using Dirac.GameServer.Core;



namespace Dirac.BulletPhysics
{
    public class BasicDemo : Demo
    {
        public static BasicDemo Instance { get; set; }

        Vector3 eye = new Vector3(30, 20, 10) * 5;
        Vector3 target = new Vector3(0, 5, -4);

        // create 125 (5x5x5) dynamic objects
        const int ArraySizeX = 0, ArraySizeY = 0, ArraySizeZ = 0;

        // scaling of the objects (0.1 = 20 centimeter boxes )
        const float StartPosX = -5;
        const float StartPosY = -5;
        const float StartPosZ = -3;

        protected override void OnInitialize()
        {
            Freelook.SetEyeTarget(eye, target);
            
            Graphics.SetFormText("TheGreatWatcher - Physics Debug Engine");
            Graphics.SetInfoText("Move using mouse and WASD+shift\n" +
                "F3 - Toggle debug\n"
                //"F11 - Toggle fullscreen\n" +
                );

            //this.Graphics.Form.Visible = false;
        }

        protected override void OnInitializePhysics()
        {
            Instance = this;
            this.DebugDrawMode = DebugDrawModes.None;

            // collision configuration contains default setup for memory, collision setup
            CollisionConf = new DefaultCollisionConfiguration();
            Dispatcher = new CollisionDispatcher(CollisionConf);

            Broadphase = new DbvtBroadphase();

            World = new DiscreteDynamicsWorld(Dispatcher, Broadphase, null, CollisionConf);
            World.Gravity = new Vector3(0, -0, 0);

            // create the ground
            BoxShape groundShape = new BoxShape(50, 1, 50);
            StaticPlaneShape splaneShape = new StaticPlaneShape(Vector3.UnitY, 0);
            //groundShape.InitializePolyhedralFeatures();
            //CollisionShape groundShape = new StaticPlaneShape(new Vector3(0,1,0), 50);

            CollisionShapes.Add(splaneShape);
            CollisionObject ground = localCreateRigidBody(0, Matrix.Identity, splaneShape);
            ground.UserObject = "Ground";

            // create a few dynamic rigidbodies
            const float mass = 1.0f;

            //BoxShape colShape = new BoxShape(1);
            BoxShape colShape = new BoxShape(1);
            CollisionShapes.Add(colShape);
            Vector3 localInertia = colShape.CalculateLocalInertia(mass);

            const float start_x = StartPosX - ArraySizeX / 2;
            const float start_y = StartPosY;
            const float start_z = StartPosZ - ArraySizeZ / 2;

            int k, i, j;
            for (k = 0; k < ArraySizeY; k++)
            {
                for (i = 0; i < ArraySizeX; i++)
                {
                    for (j = 0; j < ArraySizeZ; j++)
                    {
                        Matrix startTransform = Matrix.Translation(
                            2 * i + start_x,
                            2 * k + start_y,
                            2 * j + start_z
                        );

                        // using motionstate is recommended, it provides interpolation capabilities
                        // and only synchronizes 'active' objects
                        DefaultMotionState myMotionState = new DefaultMotionState(startTransform);
                        RigidBodyConstructionInfo rbInfo =
                            new RigidBodyConstructionInfo(mass, myMotionState, colShape, localInertia);
                        RigidBody body = new RigidBody(rbInfo);
                        rbInfo.Dispose();

                        // make it drop from a height
                        body.Translate(new Vector3(0, 20, 0));

                        World.AddRigidBody(body);
                    }
                }
            }

            localCreateRigidBody(1, Matrix.Translation(0, 100, 0), new BoxShape(1.5f));
            localCreateRigidBody(1, Matrix.Translation(0, 70, 0), new CylinderShape(1.5f));

            World.SetInternalTickCallback(new DynamicsWorld.InternalTickCallback(internalTickCallback));


            ghostObject = new PairCachingGhostObject();
            ghostObject.WorldTransform = Matrix.Translation(0, 100, 0);
            Broadphase.OverlappingPairCache.SetInternalGhostPairCallback(new GhostPairCallback());

            ConvexShape capsule = new CylinderShape(new Vector3(1f, 3f, 1f));
            ghostObject.CollisionShape = capsule;
            ghostObject.CollisionFlags = CollisionFlags.CharacterObject;

            //const float stepHeight = 9.35f;
            character = new KinematicCharacterController(ghostObject, capsule, 1);
            World.AddCollisionObject(ghostObject, CollisionFilterGroups.CharacterFilter, CollisionFilterGroups.StaticFilter | CollisionFilterGroups.DefaultFilter);
            character.SetFallSpeed(0);
            World.AddAction(character);

        }

        public PairCachingGhostObject ghostObject;
        public KinematicCharacterController character;

        public void internalTickCallback(DynamicsWorld world, float timeStep)
        {
            try
            {
                int numManifolds = World.Dispatcher.NumManifolds;
                this.NumManifold = World.Dispatcher.NumManifolds;
                //this.Graphics.SetFormText("numManifolds " + numManifolds.ToString());
                for (int i = 0; i < numManifolds; i++)
                {
                    PersistentManifold contactManifold = World.Dispatcher.GetManifoldByIndexInternal(i);
                    CollisionObject obA = contactManifold.Body0 as CollisionObject;
                    CollisionObject obB = contactManifold.Body1 as CollisionObject;

                    int numContacts = contactManifold.NumContacts;
                    for (int j = 0; j < numContacts; j++)
                    {
                        ManifoldPoint pt = contactManifold.GetContactPoint(j);
                        if (pt.Distance < 0.0f)
                        {
                            if (obA.UserObject == null)
                                continue;
                            if (obB.UserObject == null)
                                continue;

                            if (!(obA.UserObject is Actor))
                                continue;
                            if (!(obB.UserObject is Actor))
                                continue;

                            if (obA.UserObject is Projectile)
                                (obA.UserObject as Projectile).OnCollision(obB.UserObject as Actor);

                            if (obB.UserObject is Projectile)
                                (obB.UserObject as Projectile).OnCollision(obA.UserObject as Actor);


                            break; //un solo pinto adentro (pt.Distance < 0.0) es contacto, y listo salgo
                         
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogManager.DefaultLogger.Error(ex.Message);
            }
        }

        


        public RigidBody LocalCreateRigidBody(float mass, Dirac.Math.Vector3 startPosition, CollisionShape shape)
        {
            Matrix startTransform = Matrix.Translation(startPosition.x, startPosition.y, startPosition.z);
            bool isDynamic = (mass != 0.0f);

            Vector3 localInertia = Vector3.Zero;
            if (isDynamic)
                shape.CalculateLocalInertia(mass, out localInertia);

            DefaultMotionState myMotionState = new DefaultMotionState(startTransform);

            RigidBodyConstructionInfo rbInfo = new RigidBodyConstructionInfo(mass, myMotionState, shape, localInertia);
            RigidBody body = new RigidBody(rbInfo);

            //World.AddRigidBody(body);

            return body;
        }

        public void Execute(Action action)
        {
            _concurrentQueueAction.Enqueue(action);
        }

        public void AddRigidBodyToWorldConcurrent(RigidBody rigidBody)
        {
            _concurrentQueueAction.Enqueue(() =>
            {
                if (this.World.NumCollisionObjects > 1000)
                    Logging.LogManager.DefaultLogger.Warn("NumCollisionObjects > 1000 [{0}]", this.World.NumCollisionObjects);

                this.World.AddRigidBody(rigidBody);
            });
        }

        public void RemoveRigidBodyFromWorldConcurrent(RigidBody rigidBody)
        {
            _concurrentQueueAction.Enqueue(() =>
            {
                if (rigidBody != null)
                {
                    this.World.RemoveRigidBody(rigidBody);
                }
            });
        }

        private ConcurrentQueue<Action> _concurrentQueueAction = new ConcurrentQueue<Action>();
        public override void OnUpdate()
        {
            try
            {
                while (_concurrentQueueAction.Count != 0)
                {
                    Action toExecute;
                    if (!_concurrentQueueAction.TryDequeue(out toExecute))
                    {
                        Logging.LogManager.DefaultLogger.Error("TryDequeue Action Bullet");
                    }
                    toExecute.Invoke();
                }
                base.OnUpdate();
            }
            catch (Exception ex)
            {
                Logging.LogManager.DefaultLogger.Error(ex.Message);
            }
        }
    }

    class DrawingResult : CollisionWorld.ContactResultCallback
    {
        DynamicsWorld world;
        Graphics Graphics;
        public DrawingResult(DynamicsWorld world, Graphics gpx)
        {
            this.Graphics = gpx;
            this.world = world;
        }

        public override float AddSingleResult(ManifoldPoint cp, CollisionObjectWrapper colObj0Wrap, int partId0, int index0, CollisionObjectWrapper colObj1Wrap, int partId1, int index1)
        {
            Vector3 ptA = cp.PositionWorldOnA;
            Vector3 ptB = cp.PositionWorldOnB;
            return 0;
        }
    };

    public static class BasicDemoStart
    {
        [STAThread]
        public static void Start()
        {
            using (Demo demo = new BasicDemo())
            {
                LibraryManager.Initialize(demo);
            }
        }
    }
}*/
