using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dirac;
using Dirac.GameServer;
using Dirac.GameServer.Network;
using Dirac.GameServer.Network.Message;

namespace Dirac.Login
{
    public class LoginManager //: IMessageConsumer
    {
        public static void Consume(GameClient client, GameMessage message)
        {
            switch(message.opcodes)
            {
                case Opcodes.LoginMessage:
                    String account = (message as LoginMessage).Account;
                    String pwd = (message as LoginMessage).Password;
                    OnLogin(client, message);
                    break;
            }
        }

        public static void OnLogin(GameClient client, GameMessage messages)
        {
            GameChangeSceneMessage outmessage1 = new GameChangeSceneMessage();
            outmessage1.NewSceneId = 2;
            outmessage1.Field1 = 0;
            client.SendMessage(outmessage1, true);

            ToonListMessage outmessage2 = new ToonListMessage();
            outmessage2.ToonList = new ToonData[5];

            VisualEquipment ve = new VisualEquipment();
            ve.Equipment = new VisualItem[8];
            for (int i = 0; i < ve.Equipment.Length; i++)
            {
                ve.Equipment[i] = new VisualItem();
                ve.Equipment[i].snoId = 0;
                ve.Equipment[i].SlotID = 0;
                ve.Equipment[i].Field2 = 0;
                ve.Equipment[i].Field3 = 0;
            }
            //ve.Equipment = GetCurrentVisualEquipment();
            for (int i = 0; i < 5; i++)
            {
                outmessage2.ToonList[i] = new ToonData();
                outmessage2.ToonList[i].EquipmentList = ve;
                outmessage2.ToonList[i].PlayerIndex = 0;
                outmessage2.ToonList[i].SNO = 10;
                outmessage2.ToonList[i].Level = 50;
                outmessage2.ToonList[i].ToonName = "machivo" + i.ToString();
            }
            client.SendMessage(outmessage2, true);
        }
        
        /*public static VisualItem[] GetCurrentVisualEquipment()
        {
            Item item;
            VisualItem empty_visualitem;
            VisualItem[] result = new VisualItem[8];

            for (int i = 0; i < result.Length; i++)
            {
                
            }
            item = this.GetItembySlot(EquipmentSlotId.Helm);
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Helm;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.GbId = 0;
                result[0] = empty_visualitem;
            }
            else
            {
                result[0] = item.GetVisualItem();
            }

            item = this.GetItembySlot(EquipmentSlotId.Armor);
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Armor;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.GbId = 0;
                result[1] = empty_visualitem;
            }
            else
            {
                result[1] = item.GetVisualItem();
            }

            item = this.GetItembySlot(EquipmentSlotId.Gloves);
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Gloves;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.GbId = 0;
                result[2] = empty_visualitem;
            }
            else
            {
                result[2] = item.GetVisualItem();
            }

            item = this.GetItembySlot(EquipmentSlotId.Pants);
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Pants;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.GbId = 0;
                result[3] = empty_visualitem;
            }
            else
            {
                result[3] = item.GetVisualItem();
            }

            item = this.GetItembySlot(EquipmentSlotId.Boots);
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Boots;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.GbId = 0;
                result[4] = empty_visualitem;
            }
            else
            {
                result[4] = item.GetVisualItem();
            }


            item = this.GetItembySlot(EquipmentSlotId.Main_Hand);
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Main_Hand;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.GbId = 0;
                result[5] = empty_visualitem;
            }
            else
            {
                result[5] = item.GetVisualItem();
            }

            item = this.GetItembySlot(EquipmentSlotId.Wing);
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Wing;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.GbId = 0;
                result[6] = empty_visualitem;
            }
            else
            {
                result[6] = item.GetVisualItem();
            }

            empty_visualitem = new VisualItem();
            empty_visualitem.SlotID = 0;
            empty_visualitem.Field2 = 0;
            empty_visualitem.Field3 = 0;
            empty_visualitem.GbId = 0;
            result[5] = empty_visualitem;

            empty_visualitem = new VisualItem();
            empty_visualitem.SlotID = 0;
            empty_visualitem.Field2 = 0;
            empty_visualitem.Field3 = 0;
            empty_visualitem.GbId = 0;
            result[6] = empty_visualitem;

            empty_visualitem = new VisualItem();
            empty_visualitem.SlotID = 0;
            empty_visualitem.Field2 = 0;
            empty_visualitem.Field3 = 0;
            empty_visualitem.GbId = 0;
            result[7] = empty_visualitem;

            return result;
        }*/
    
    }
}
