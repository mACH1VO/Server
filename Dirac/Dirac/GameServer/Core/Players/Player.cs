using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;

using Dirac;
using Dirac.Extensions;
using Dirac.Logging;
//using Dirac.BulletPhysics;

using Dirac.GameServer.Network;
using Dirac.GameServer.Network.Message;

using Dirac.Math;

using Dirac.DB;
using Dirac.DB.Tasks;


namespace Dirac.GameServer.Core
{
    public partial class Player : Actor
    {
        /// <summary>
        /// The ingame-client for player.
        /// </summary>
        public GameClient GameClient { get; set; }

        /// <summary>
        /// The player index.
        /// </summary>
        public int PlayerIndex { get; set; }

        /// <summary>
        /// The player index.
        /// </summary>
        public Boolean Busy { get; set; }

        /// <summary>
        /// The inventory of player's toon.
        /// </summary>
        public PlayerInventory Inventory { get; set; }

        /// <summary>
        /// ActorType = Player.
        /// </summary>
        public override ActorType ActorType { get { return ActorType.Player; } }

        /// <summary>
        /// Revealed objects to player. Should be ConcurrentDictionarys.
        /// </summary>
        public ConcurrentDictionary<int, IRevealable> RevealedObjects = new ConcurrentDictionary<int, IRevealable>();

        public PowerSet PowerSet { get; set; }

        

        public override Vector3 Position
        {
            get { return base.Position; }
            set
            {
                /*if (BasicDemo.Instance.character != null)
                    BasicDemo.Instance.character.Warp((value + Vector3.UnitY * 3).ToBulletV3());*/

                base.Position = value;
            }
        }

        public Boolean IsShopOpened { get; set; }

        public NPC OpenedNPC { get; set; }

        public Boolean IsVaultOpened { get; set; }

        public Boolean IsInventoryOpened { get; set; }

        public Dictionary<SkillSlot, SkillOpcode> SkillSlots; 

        public Dictionary<ushort, SkillOpcode> AvailableSkillSlots;

        public Player(GameClient client, int SnoId)
            : base(SnoId)
        {
            //this.Position = new Vector3(3143, 2829, 59);
            this.GameClient = client;
            this.PlayerIndex = Interlocked.Increment(ref Game.PlayerIndexCounter); // get a new playerId for the player and make it atomic.

            //this.RevealedObjects.Add(this.DynamicID, this);
            //this.RotationW = 0.05940768f;
            //this.RotationAxis = new Vector3(0f, 0f, 0.9982339f);

            this.PowerSet = new PowerSet(this);
            this.Inventory = new PlayerInventory(this);
            this.Busy = false;

            this.Attributes[GameAttributeStaticList.Hitpoints_Max] = 150f;
            this.Attributes[GameAttributeStaticList.Hitpoints_Cur] = 137f;

            this.Attributes[GameAttributeStaticList.Mana_Max] = 50f;
            this.Attributes[GameAttributeStaticList.Mana_Cur] = 48f;

            this.SkillSlots = new Dictionary<SkillSlot, SkillOpcode>();
            this.AvailableSkillSlots = new Dictionary<ushort, SkillOpcode>();

            this.fillAvailableSkillSlots();
            this.fillSkillSlots();

            this.SetAllStatsInCorrectOrder();
        }

        public void fillSkillSlots()
        {
            this.SkillSlots.Add(SkillSlot.Primary, SkillOpcode.Hand);
            this.SkillSlots.Add(SkillSlot.Secondary, SkillOpcode.Hand);

            this.SkillSlots.Add(SkillSlot._one, SkillOpcode.Hand);
            this.SkillSlots.Add(SkillSlot._two, SkillOpcode.Hand);
            this.SkillSlots.Add(SkillSlot._three, SkillOpcode.Hand);
            this.SkillSlots.Add(SkillSlot._four, SkillOpcode.Hand);
        }

        public void fillAvailableSkillSlots()
        {
            this.AvailableSkillSlots.Add(0, SkillOpcode.Cyclone);
            this.AvailableSkillSlots.Add(1, SkillOpcode.Cyclone);
            this.AvailableSkillSlots.Add(2, SkillOpcode.Cyclone);
            this.AvailableSkillSlots.Add(3, SkillOpcode.Cyclone);
            this.AvailableSkillSlots.Add(4, SkillOpcode.Cyclone);
            this.AvailableSkillSlots.Add(5, SkillOpcode.Cyclone);
            this.AvailableSkillSlots.Add(6, SkillOpcode.Cyclone);
        }

        #region Attribute Setters
        public void SetAllStatsInCorrectOrder()
        {
            SetAttributesSkills();
            SetAttributesBuffs();
            SetAttributesDamage();
            SetAttributesRessources(); //mana etc fill 100
            SetAttributesMovement();
            SetAttributesMisc();
            SetAttributesSkillSets();
            SetAttributesOther(); // vit str HP etc
        }

        public void SetAttributesSkills()
        {
            /*this.Attributes[GameAttribute.Skill, 0x7545] = 1;
            //scripted //this.Attributes[GameAttribute.Skill_Total, 0x76B7] = 1; //Punch!
            this.Attributes[GameAttribute.Skill, 0x76B7] = 1;
            //scripted //this.Attributes[GameAttribute.Skill_Total, 0x6DF] = 1; //Use Item
            this.Attributes[GameAttribute.Skill, 0x6DF] = 1;
            //scripted //this.Attributes[GameAttribute.Skill_Total, 0x7780] = 1; //Basic Attack
            this.Attributes[GameAttribute.Skill, 0x7780] = 1;
            //scripted //this.Attributes[GameAttribute.Skill_Total, 0x0002EC66] = 0; //stone of recall
            //scripted //this.Attributes[GameAttribute.Skill_Total, 0xFFFFF] = 1;
            this.Attributes[GameAttribute.Skill, 0xFFFFF] = 1;*/
        }
        public void SetAttributesBuffs()
        {
            //Buffs
            /*this.Attributes[GameAttribute.Buff_Active, 0x33C40] = true;
            this.Attributes[GameAttribute.Buff_Icon_End_Tick0, 0x00033C40] = 0x000003FB;
            this.Attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x00033C40] = 0x00000077;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 0x00033C40] = 1;
            this.Attributes[GameAttribute.Buff_Active, 0xCE11] = true;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 0x0000CE11] = 1;
            this.Attributes[GameAttribute.Buff_Visual_Effect, 0xFFFFF] = true;*/
        }
        public void SetAttributesDamage()
        {

        }
        public void SetAttributesRessources()
        {

            this.Attributes[GameAttributeStaticList.AvailableStatsPoints] = 135;
            this.Attributes[GameAttributeStaticList.Strength]  = 90;
            this.Attributes[GameAttributeStaticList.Precision] = 88;
            this.Attributes[GameAttributeStaticList.Vitality] = 25;
            this.Attributes[GameAttributeStaticList.Intelligence] = 30;
        }
        public void SetAttributesResistance()
        {
            /*this.Attributes[GameAttribute.Resistance, 0xDE] = 0.5f;
            this.Attributes[GameAttribute.Resistance, 0x226] = 0.5f;*/
        }
        public void SetAttributesMovement()
        {
            //Movement
            /*this.Attributes[GameAttributeStaticList.Movement_Scalar] = 1f;
            this.Attributes[GameAttributeStaticList.Movement_Scalar_Subtotal] = 1f;//scripted
            this.Attributes[GameAttributeStaticList.Movement_Scalar_Capped_Total] = 1f;//scripted //
            this.Attributes[GameAttributeStaticList.Movement_Scalar_Total] = 1f;//scripted*/
            
            //this.Attributes[GameAttributeStaticList.Walking_Rate_Total] = 0.5f /*(int)Toon.HeroTable.WalkingRate*/;//scripted //
            //this.Attributes[GameAttributeStaticList.Running_Rate_Total] = 0.5f/*(int)Toon.HeroTable.RunningRate*/;//scripted //
            //this.Attributes[GameAttributeStaticList.Sprinting_Rate_Total] = 0;
            //this.Attributes[GameAttributeStaticList.Strafing_Rate_Total] = 0;//data.F17; //scripted // //These two are guesses -Egris
            
            //this.Attributes[GameAttributeStaticList.Walking_Rate] = 0.30f;//Toon.HeroTable.WalkingRate;
            //this.Attributes[GameAttributeStaticList.Running_Rate] = 0.40f;//Toon.HeroTable.RunningRate;*/
        }
        public void SetAttributesMisc()
        {

        }
        public void SetAttributesByItems()
        {

        }
        public void SetAttributesSkillSets()
        {

        }
        public void SetAttributesOther()
        {

        }


        public void AllTheScriptedStats()
        {


        }


        #endregion

        #region update-logic

        public void Update()
        {
        }

        #endregion

        #region game-message handling & consumers


        
        /// <summary>
        /// Consumes the given game-message.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="message">The GameMessage.</param>
        public void Consume(GameClient client, GameMessage message)
        {
            switch (message.opcodes)
            {
                case Opcodes.ChatMessage:
                    ChatMessage msg = (ChatMessage)message;
                    Logging.LogManager.DefaultLogger.Trace("[CHAT]" + msg.Text);
                    this.ProcessCommand(msg.Text);
                    ChatMessage echo = new ChatMessage();
                    echo.ActorDynamicID = this.DynamicID;
                    echo.Type = msg.Type;
                    echo.Text = msg.Text;
                    echo.Lengh = msg.Lengh;
                    if (!echo.Text.Contains("/"))
                        this.World.BroadcastIfRevealed(echo, this);

                    PlayerSaveTask pst = new PlayerSaveTask();
                    DBManager.Instance.ConcurrentExecuteTask(pst);

                    PlayerLoadTask plt = new PlayerLoadTask();
                    DBManager.Instance.ConcurrentExecuteTask(plt);

                    break;
                case Opcodes.HeartbeatMessage:
                    this.OnHeartbeatMessage(client, (HeartbeatMessage)message);
                    break;
                case Opcodes.ACDClientTranslateMessage:
                    this.OnPlayerMovement(client, (ACDClientTranslateMessage)message);
                    break;
                case Opcodes.TargetMessage:
                    this.OnObjectTargeted(client, (TargetMessage)message);
                    break;
                case Opcodes.AssignSkillMessage1:
                    this.OnAssignActiveSkill(client, (AssignSkillMessage)message);
                    break;
                case Opcodes.AssignStatMessage:
                    this.OnAssignStat(client, (AssignStatMessage)message);
                    break;
                default:
                    break;
            }
        }

        private void OnHeartbeatMessage(GameClient client, HeartbeatMessage message)
        {
            if (this.GameClient != null)
                this.GameClient.SendMessage(new HeartbeatMessage(), true);
            //response
        }

        private void OnPlayerMovement(GameClient client, ACDClientTranslateMessage message)
        {
            Logging.LogManager.DefaultLogger.Trace("Player moved to " + message.Position.ToString());
            // here we should also be checking the position and see if it's valid. If not we should be resetting player to a good position with ACDWorldPositionMessage
            // so we can have a basic precaution for hacks & exploits .
            if (message.Position == null)
                return; //throw ex?
                
            this.Position = message.Position;

            this.RevealActorsToPlayer();


            //this.SetFacingRotation(message.Angle);

            /*ACDTranslateNormalMessage msgToBroadcast = new ACDTranslateNormalMessage
            {
                ActorId = (int)this.DynamicID,
                Position = this.Position,
                Angle = 0,
                TurnImmediately = false,
                Speed = 1,
                AnimationTag = 1
            };*/

            //this.RevealScenesToPlayer();
            //this.RevealActorsToPlayer();

            //this.World.BroadcastExclusive(msgToBroadcast, this);

            /*foreach (var actor in GetActorsInRange())
                actor.OnPlayerApproaching(this);*/

            //this.CollectGold();
            //this.CollectHealthGlobe();
        }

        private void OnObjectTargeted(GameClient client, TargetMessage message)
        {

            Actor targetActor = this.World.Actors[message.TargetID];
            if (targetActor != null)
            {
                if (targetActor.ActorType == ActorType.Item)
                {
                    targetActor.OnTargeted(this, message);
                    return;
                }

                if (targetActor.ActorType == ActorType.NPC)
                {
                    targetActor.OnTargeted(this, message);
                    return;
                }

                if (targetActor.ActorType == Dirac.GameServer.Core.ActorType.Monster)
                {
                    SkillManager.RunPower(this, message);
                }
                /*if (targetActor.ActorType == ActorType.Monster)
                {
                    bool powerHandled = PowerManager.RunPower(this, targetActor, message);
                }*/
            }
            else
            {
                bool powerHandled = SkillManager.RunPower(this, message);
            }

            /*if (!powerHandled)
            {
                Actor actor = this.World.GetActorByDynamicId(message.TargetID);
                if (actor == null) 
                    return;

                actor.OnTargeted(this, message);
            }*/

            //this.ExpBonusData.Check(2);
        }

        private void OnAssignActiveSkill(GameClient client, AssignSkillMessage message)
        {

        }

        private void OnAssignStat(GameClient client, AssignStatMessage message)
        {
            StatType statType = (StatType)message.StatType;
            if (this.Attributes[GameAttributeStaticList.AvailableStatsPoints] > 0)
            {
                switch (statType)
                {
                    case StatType.Strength:
                        {
                            this.Attributes[GameAttributeStaticList.Strength] += 1;
                            break;
                        }
                    case StatType.Agility:
                        {
                            this.Attributes[GameAttributeStaticList.Precision] += 1;
                            break;
                        }
                    case StatType.Stamina:
                        {
                            this.Attributes[GameAttributeStaticList.Vitality] += 1;
                            break;
                        }
                    case StatType.Energy:
                        {
                            this.Attributes[GameAttributeStaticList.Intelligence] += 1;
                            break;
                        }
                    default:
                        {
                            Logging.LogManager.DefaultLogger.Error("could not identify statType! " + statType.ToString());
                            break;
                        }
                }
                this.Attributes[GameAttributeStaticList.AvailableStatsPoints] -= 1;
            }

            this.Attributes.BroadcastChangestoOwner();
        }

        #endregion

        #region enter, leave, reveal handling

        /// <summary>
        /// Reveals actors in player's proximity.
        /// </summary>
        public void RevealActorsToPlayer()
        {
            var actors = this.GetActorsInRange();

            /*foreach (var act in actors)
            {
                Logging.LogManager.DefaultLogger.Warn(act.ToString());
            }
            Logging.LogManager.DefaultLogger.Warn("-----------");*/

            foreach (var actor in actors) // reveal actors in player's proximity.
            {
                if (actor.Visible == false)
                    continue;

                if (actor.IsRevealedToPlayer(this))
                    continue;

                /*if (actor.ActorType == ActorType.Gizmo || actor.ActorType == ActorType.Player
                    || actor.ActorType == ActorType.Enviroment
                    || actor.ActorType == ActorType.Critter || actor.ActorType == ActorType.Item || actor.ActorType == ActorType.ServerProp)
                    actor.Reveal(this);
                if (actor.ActorType == ActorType.Monster)
                    actor.Reveal(this);*/
                actor.Reveal(this);
            }
        }

        public override void OnEnter(Map world)
        {
            base.OnEnter(world);
            this.World.Reveal(this);

            //this.RevealScenesToPlayer(); // reveal scenes in players proximity.
            this.RevealActorsToPlayer(); // reveal actors in players proximity.

            // load all inventory items
            //if (!this.Inventory.Loaded)//why reload if already loaded?
                //this.Inventory.LoadFromDB();
            //else
                //this.Inventory.RefreshInventoryToClient();

            // generate visual update message
            //this.Inventory.SendVisualInventory(this);

            //SetAllStatsInCorrectOrder();

            /*this.GLSphere.Color = System.Drawing.Color.OrangeRed;
            this.GLSphere.Radius = 0.5;

            this.GLSphere.Color = System.Drawing.Color.White;
            this.GLSphere.Transformation.TranslateZ = 0.3f;

            if (this is Player)
            {
                this.GLSphere.Color = System.Drawing.Color.White;
                this.GLSphere.Transformation.TranslateX = (float)this.Position.X;
                this.GLSphere.Transformation.TranslateY = (float)this.Position.Y;
            }*/

            Spawned = true;
        }

        public override void OnTeleport()
        {
            this.RevealActorsToPlayer();
        }

        public override void OnLeave(Map world)
        {
            /*this.Conversations.StopAll();

            // save visual equipment
            this.Toon.HeroVisualEquipmentField.Value = this.Inventory.GetVisualEquipment();
            this.Toon.HeroLevelField.Value = this.Attributes[GameAttribute.Level];
            this.Toon.GameAccount.ChangedFields.SetPresenceFieldValue(this.Toon.HeroVisualEquipmentField);
            this.Toon.GameAccount.ChangedFields.SetPresenceFieldValue(this.Toon.HeroLevelField);

            // save all inventory items
            this.Inventory.SaveToDB();
            world.CleanupItemInstances();*/
        }

        public override bool Reveal(Player player)
        {
            return true;
            if (!base.Reveal(player))
                return false;

            if (this != player)
            {
                /*HeroStateData hsd = new HeroStateData()
                {
                    Field0 = 0,
                    Field1 = 0,
                    Field2 = 0,
                    Field3 = 0,
                    PlayerFlags = 0,
                    QuestRewardHistoryEntriesCount = 0,
                    PlayerSavedData = new PlayerSavedData()
                    {
                        HotBarButtons = new HotbarButtonData[6],
                    }
                };
                hsd.PlayerSavedData.HotBarButtons = this.PowerSet.HotbarButtonData;*/
                NewPlayerMessage npm = new NewPlayerMessage()
                {
                    dynamicId = 5, // player index
                    //ToonId = new EntityId() { High = (long)1, Low = (long)1 }, //Toon
                    //GameAccountId = new EntityId() { High = (long)1, Low = (long)1 }, //GameAccount
                    ToonName = "machivito",//joinedPlayer.name,
                    Field3 = 0x00000002, //party frame class
                    Field4 = 0x4, //party frame level
                    snoActorPortrait = 32001,//party frame portrait //ojo
                    Level = 15,
                    //StateData = hsd,
                    IsMainPlayer = false, //ACA ES FALSOOOOOOOOOOOO
                    Field9 = 0x00000001,
                    ActorID = this.DynamicID,
                    SNO = 32001
                };

                if (player.GameClient != null)
                    player.GameClient.SendMessage(npm);

                /*ACDCreateActorMessage createdActor = ACDEnterKnown();


                if (player.GameClient != null)
                    player.GameClient.SendMessage(createdActor);

                // Reveal actor (creates actor and makes it visible to the player)
                if (player.GameClient != null)
                    player.GameClient.SendMessage(new ACDEnterMapMessage(this.DynamicID));*/
            }
            /*if (this == player) // only send this when player's own actor being is revealed.
            {
                player.GameClient.SendMessage(new PlayerWarpedMessage()
                {
                    Field0 = 9,
                    Field1 = 0f,
                });
            }

            player.GameClient.SendMessage(new PlayerEnterKnownMessage()
            {
                PlayerIndex = this.PlayerIndex,
                ActorId = this.DynamicID,
            });

            //this.Inventory.SendVisualInventory(player);
            }*/

            //this.Inventory.Reveal(player);

            return true;
        }

        public override bool Unreveal(Player player)
        {
            if (!base.Unreveal(player))
                return false;

           // this.Inventory.Unreveal(player);

            return true;
        }

        public override void BeforeChangeWorld()
        {
            //this.Inventory.Unreveal(this);
        }

        public override void AfterChangeWorld()
        {
           // this.Inventory.Reveal(this);
        }

        #endregion

        #region gold, heath-glob collection

        /*private void CollectGold()
        {
            List<Item> itemList = this.GetItemsInRange(5);
            foreach (Item item in itemList)
            {
                if (item.ItemType != ItemType.Gold1 &&
                    item.ItemType != ItemType.Gold2 &&
                    item.ItemType != ItemType.Gold3 &&
                    item.ItemType != ItemType.Gold4 &&
                    item.ItemType != ItemType.Gold5) 
                    continue;

                List<Player> playersAffected = this.GetPlayersInRange(26f);
                //int amount = (int)Math.Max(1, Math.Round((double)item.Attributes[GameAttribute.Gold] / playersAffected.Count, 0));
                //item.Attributes[GameAttribute.Gold] = amount;
                int amount = item.Attributes[GameAttribute.Gold];
                foreach (Player player in playersAffected)
                {
                    player.InGameClient.SendMessage(new FloatingAmountMessage()
                    {
                        Place = new WorldPlace()
                        {
                            Position = player.Position,
                            WorldID = player.World.DynamicID,
                        },

                        Amount = amount,
                        Type = FloatingAmountMessage.FloatType.Gold,
                    });

                    player.Inventory.PickUpGold(item.DynamicID);
                }
                item.Destroy();
            }
        }*/

        /*private void CollectHealthGlobe()
        {
            var itemList = this.GetItemsInRange(5f);
            foreach (Item item in itemList)
            {
                if (!Item.IsHealthGlobe(item.ItemType)) continue;

                var playersAffected = this.GetPlayersInRange(26f);
                foreach (Player player in playersAffected)
                {
                    foreach (Player targetAffected in playersAffected)
                    {
                        player.InGameClient.SendMessage(new PlayEffectMessage()
                        {
                            ActorId = targetAffected.DynamicID,
                            Effect = Effect.HealthOrbPickup
                        });
                    }

                    //every summon and mercenary owned by you must broadcast their green text to you /H_DANILO
                    player.AddPercentageHP((int)item.Attributes[GameAttribute.Health_Globe_Bonus_Health]);
                }
                item.Destroy();
            }
        }*/

        public void AddPercentageHP(int percentage)
        {
            /*if ((percentage > 100) || (percentage < 0))
                return;
            float quantity = (percentage * this.Attributes[GameAttributeStaticList.Hitpoints_Max_Total]) / 100;
            this.AddHP(quantity);*/
        }

        public void AddHP(float quantity)
        {
            /*this.Attributes[GameAttribute.Hitpoints_Cur] = Math.Min(
                this.Attributes[GameAttribute.Hitpoints_Cur] + quantity,
                this.Attributes[GameAttribute.Hitpoints_Max_Total]);*/

            /*this.InGameClient.SendMessage(new FloatingNumberMessage()
            {
                ActorID = this.DynamicID,
                Number = quantity,
                Type = FloatingNumberMessage.FloatType.Green
            });*/

            //this.Attributes.BroadcastChangedIfRevealed();
        }

        #endregion

        #region experience handling

        //Max((Hitpoints_Max + Hitpoints_Total_From_Level + Hitpoints_Total_From_Vitality + Hitpoints_Max_Bonus) * (Hitpoints_Max_Percent_Bonus + Hitpoints_Max_Percent_Bonus_Item + 1), 1)
        private float GetMaxTotalHitpoints()
        {
            return 1;
           /* return (Math.Utility.Max((this.Attributes[GameAttributeStaticList.Hitpoints_Max] + this.Attributes[GameAttributeStaticList.Hitpoints_Total_From_Level] +
                this.Attributes[GameAttributeStaticList.Hitpoints_Max_Bonus]) *
                (this.Attributes[GameAttributeStaticList.Hitpoints_Max_Percent_Bonus] + this.Attributes[GameAttributeStaticList.Hitpoints_Max_Percent_Bonus_Item] + 1), 1));*/
        }

        //Max((Mana_Max + ((Level#NONE - 1) * Resource_Factor_Level) + Mana_Max_Bonus) * (Mana_Max_Percent_Bonus + 1), 0)
        /*private float GetMaxResource(int resourceId)
        {
            return (Math.Max((this.Attributes[GameAttribute.Mana_Max, resourceId] + ((this.Attributes[GameAttribute.Level] - 1) * this.Attributes[GameAttribute.Resource_Factor_Level, resourceId]) + this.Attributes[GameAttribute.Mana_Max_Bonus, resourceId]) * (this.Attributes[GameAttribute.Mana_Max_Percent_Bonus, resourceId] + 1), 0));
        }*/

        public static int[] LevelBorders =
        {
            0, 1200, 2700, 4500, 6600, 9000, 11700, 14700, 17625, 20800, 24225, /* Level 0-10 */
            27900, 31825, 36000, 41475, 38500, 40250, 42000, 43750, 45500, 47250, /* Level 11-20 */
            49000, 58800, 63750, 73625, 84000, 94875, 106250, 118125, 130500, 134125, /* Level 21-30 */
            77700, 81700, 85800, 90000, 94300, 98700, 103200, 107800, 112500, 117300, /* Level 31-40 */
            122200, 127200, 132300, 137500, 142800, 148200, 153700, 159300, 165000, 170800, /* Level 41-50 */
            176700, 182700, 188800, 195000, 201300, 207700, 214200, 220800, 227500, 234300, /* Level 51-60 */
            241200, 248200, 255300, 262500, 269800, 277200, 284700, 292300, 300000, 307800, /* Level 61-70 */
            315700, 323700, 331800, 340000, 348300, 356700, 365200, 373800, 382500, 391300, /* Level 71-80 */
            400200, 409200, 418300, 427500, 436800, 446200, 455700, 465300, 475000, 484800, /* Level 81-90 */
            494700, 504700, 514800, 525000, 535300, 545700, 556200, 566800, 577500 /* Level 91-99 */
        };

        public static int[] LevelUpEffects =
        {
            85186, 85186, 85186, 85186, 85186, 85190, 85190, 85190, 85190, 85190, /* Level 1-10 */
            85187, 85187, 85187, 85187, 85187, 85187, 85187, 85187, 85187, 85187, /* Level 11-20 */
            85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, /* Level 21-30 */
            85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, /* Level 31-40 */
            85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, /* Level 41-50 */
            85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, /* Level 51-60 */
            85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, /* Level 61-70 */
            85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, /* Level 71-80 */
            85195, 85195, 85195, 85195, 85195, 85195, 85195, 85195, 85195, 85195, /* Level 81-90 */
            85195, 85195, 85195, 85195, 85195, 85195, 85195, 85195, 85195, 85195 /* Level 91-99 */
        };

        #endregion

    }
}




        

        
        

