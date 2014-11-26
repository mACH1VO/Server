using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Dirac.Logging;

namespace Dirac.GameServer.Network.Message
{
    public abstract class GameMessage
    {
        public static GameMessage ParseMessage(GameBitBuffer buffer)
        {
            int id = buffer.ReadInt(9);
            Opcodes op = (Opcodes)id;
            GameMessage msg = null;
            switch (op)
            {
                case Opcodes.QuitGameMessage:
                    msg = new QuitGameMessage(); // Case 3
                    break;

                case Opcodes.VersionsMessage:
                    msg = new VersionsMessage(); // Case 13
                    break;

                case Opcodes.CreateHeroMessage:
                    msg = new CreateHeroMessage(); // Case 23
                    break;
                case Opcodes.OpenTradeWindow:
                    msg = new OpenTradeWindowMessage(); // Case 34
                    break;

                case Opcodes.InventoryRequestBuyItemMessage:
                    msg = new InventoryRequestBuyItemMessage(); // Case 35
                    break;

                case Opcodes.InventoryRequestSellItemMessage:
                    msg = new InventoryRequestSellItemMessage(); // Case 36
                    break;

                case Opcodes.LogoutContextMessage1:
                    msg = new LogoutContextMessage(Opcodes.LogoutContextMessage1); // Case 38
                    break;

                case Opcodes.LogoutComplete:
                    msg = new LogoutComplete(); // Case 40
                    break;

                case Opcodes.ConnectionEstablishedMessage:
                    msg = new ConnectionEstablishedMessage(); // Case 50
                    break;

                case Opcodes.GameSetupMessage:
                    msg = new GameSetupMessage(); // Case 51
                    msg.Consumer = Consumers.Game;
                    break;


                case Opcodes.NewPlayerMessage:
                    msg = new NewPlayerMessage(); // Case 53
                    msg.Consumer = Consumers.Factory;
                    break;


                case Opcodes.EnterWorldMessage:
                    msg = new EnterWorldMessage(); // Case 56
                    msg.Consumer = Consumers.Factory;
                    break;

                case Opcodes.RevealWorldMessage:
                    msg = new RevealWorldMessage(); // Case 60
                    break;

                case Opcodes.HeroStateMessage:
                    msg = new HeroStateMessage(); // Case 63
                    break;

                case Opcodes.ACDCreateActorMessage:
                    msg = new ACDCreateActorMessage(); // Case 64
                    msg.Consumer = Consumers.Factory;
                    break;

                case Opcodes.ACDDestroyActorMessage:
                    msg = new ACDDestroyActorMessage(); // Case 65
                    msg.Consumer = Consumers.Factory;
                    break;

                case Opcodes.PlayerEnterKnownMessage:
                    msg = new PlayerEnterKnownMessage(); // Case 66
                    break;

                case Opcodes.ACDEnterMapMessage:
                    msg = new ACDEnterMapMessage(); // Case 67
                    msg.Consumer = Consumers.Factory;
                    break;

                case Opcodes.AttributesSetValuesMessage:
                    msg = new AttributesSetValuesMessage(); // Case 82
                    msg.Consumer = Consumers.Actor;
                    break;

                case Opcodes.VisualEquipmentMessage:
                    msg = new VisualEquipmentMessage(); // Case 83
                    msg.Consumer = Consumers.Inventory;
                    break;

                case Opcodes.TargetMessage:
                    msg = new TargetMessage(); // Case 85
                    msg.Consumer = Consumers.Player; //ojo
                    break;

                case Opcodes.ChatMessage:
                    msg = new ChatMessage(); // Case 93
                    msg.Consumer = Consumers.Player;
                    break;

                case Opcodes.InventoryDropItemMessage:
                    msg = new InventoryDropItemMessage(); // Case 94
                    break;

                case Opcodes.InventoryShowWindowMessage:
                    msg = new InventoryShowWindowMessage(); // Case 94
                    break;

                case Opcodes.InventoryCloseWindowMessage:
                    msg = new InventoryCloseWindowMessage(); // Case 94
                    break;

                case Opcodes.InventoryRequestMoveMessage:
                    msg = new InventoryRequestMoveMessage(); // Case 95
                    msg.Consumer = Consumers.Inventory;
                    break;

                case Opcodes.InventoryRequestQuickMoveMessage:
                    msg = new InventoryRequestQuickMoveMessage(); // Case 96
                    msg.Consumer = Consumers.Inventory;
                    break;

                case Opcodes.InventoryRequestUseMessage:
                    msg = new InventoryRequestUseMessage(); // Case 106
                    break;

                case Opcodes.AssignSkillMessage1:
                    msg = new AssignSkillMessage(Opcodes.AssignSkillMessage1); // Case 108
                    msg.Consumer = Consumers.Player;
                    break;

                case Opcodes.UnassignSkillMessage:
                    msg = new UnassignSkillMessage(); // Case 109
                    break;

                case Opcodes.AssignTraitsMessage:
                    msg = new AssignTraitsMessage(); // Case 110
                    break;

                case Opcodes.AssignSkillMessage2:
                    msg = new AssignSkillMessage(Opcodes.AssignSkillMessage2); // Case 111
                    msg.Consumer = Consumers.Player;
                    break;

                case Opcodes.AssignSkillMessage3:
                    msg = new AssignSkillMessage(Opcodes.AssignSkillMessage3); // Case 112
                    msg.Consumer = Consumers.Player;
                    break;


                case Opcodes.HotbarButtonData:
                    msg = null; /*new HotbarButtonData(); // Case 114*/
                    break;

                case Opcodes.ACDClientTranslateMessage:
                    msg = new ACDClientTranslateMessage(); // Case 129
                    msg.Consumer = Consumers.Player;
                    break;

                case Opcodes.InventoryRequestJewelUse:
                    msg = new InventoryRequestJewelUse(); // Case 129
                    break;

                case Opcodes.PlayerTranslateFacingMessage:
                    msg = new PlayerTranslateFacingMessage(); // Case 130
                    break;

                case Opcodes.PlayEffectMessage:
                    msg = new PlayEffectMessage(); //case 132
                    msg.Consumer = Consumers.Effect;
                    break;

                case Opcodes.PlayNonPositionalSoundMessage:
                    msg = new PlayNonPositionalSoundMessage(); // Case 135
                    break;

                case Opcodes.PlayErrorSoundMessage:
                    msg = new PlayErrorSoundMessage(); // Case 136
                    break;

                case Opcodes.PlayMusicMessage:
                    msg = new PlayMusicMessage(); // Case 137
                    break;

                case Opcodes.GameTickMessage:
                    msg = new GameTickMessage(); // Case 149
                    break;

                case Opcodes.EndOfTickMessage:
                    msg = new EndOfTickMessage(); // Case 152
                    break;

                case Opcodes.AssignStatMessage:
                    msg = new AssignStatMessage(); 
                    break;

                case Opcodes.ACDPickupFailedMessage:
                    msg = new ACDPickupFailedMessage(); // Case 165
                    break;

                case Opcodes.PlayerBusyMessage:
                    msg = new PlayerBusyMessage(); // Case 170
                    break;

                case Opcodes.TradeMessage1:
                    msg = new TradeMessage(Opcodes.TradeMessage1); // Case 171
                    break;

                case Opcodes.TradeMessage2:
                    msg = new TradeMessage(Opcodes.TradeMessage2); // Case 172
                    break;

                case Opcodes.WorldStatusMessage:
                    msg = new WorldStatusMessage(); // Case 191
                    break;

                case Opcodes.WeatherOverrideMessage:
                    msg = new WeatherOverrideMessage(); // Case 192
                    break;

                case Opcodes.ACDChangeActorVisibility:
                    msg = new ACDChangeActorVisibility(); // Case 195
                    msg.Consumer = Consumers.Actor;
                    break;

                case Opcodes.HeartbeatMessage:
                    msg = new HeartbeatMessage(); // Case 196
                    msg.Consumer = Consumers.Player;
                    break;

                case Opcodes.DisplayGameTextMessage:
                    msg = new DisplayGameTextMessage(); // Case 211
                    break;

                case Opcodes.ACDLookAtMessage:
                    msg = new ACDLookAtMessage(); // Case 217
                    break;

                case Opcodes.AddBuffIconMessage:
                    msg = new AddBuffIconMessage(); // Case 220
                    msg.Consumer = Consumers.Player;
                    break;

                case Opcodes.DeleteBuffIconMessage:
                    msg = new DeleteBuffIconMessage();
                    msg.Consumer = Consumers.Player;
                    break;

                case Opcodes.WorldDeletedMessage:
                    msg = new WorldDeletedMessage(); // Case 229
                    break;

                case Opcodes.GameChangeSceneMessage:
                    msg = new GameChangeSceneMessage(); // Case 290
                    break;

                case Opcodes.BroadcastTextMessage:
                    msg = new BroadcastTextMessage(); // Case 302
                    break;
                case Opcodes.ToonListMessage:
                    msg = new ToonListMessage(); // Case 322
                    break;
                default:
                    throw new Exception("there is no opcode for that msg");
                    msg = null;
                    break;

            }

            if (msg == null) //ugly
                return null;

            msg.Id = id;
            msg.opcodes = op;
            msg.Parse(buffer);
            return msg;
        }

        protected GameMessage() { }

        protected GameMessage(int id)
        {
            this.Id = id;
        }

        protected GameMessage(Opcodes opcode)
        {
            this.Id = (int)opcode;
        }

        protected GameMessage(Opcodes opcode, Consumers consumer)
        {
            this.Id = (int)opcode;
            this.opcodes = opcode;
            this.Consumer = consumer;
        }

        public int Id { get; set; }
        public Opcodes opcodes;
        public Consumers Consumer { get; set; }

        public abstract void Parse(GameBitBuffer buffer);
        public abstract void Encode(GameBitBuffer buffer);
        public abstract void AsText(StringBuilder b, int pad);

        public string AsText()
        {
            var builder = new StringBuilder();
            builder.AppendLine("GameMessage(0x" + Id.ToString("X4") + ")");
            AsText(builder, 0);
            return builder.ToString();
        }
    }
}
