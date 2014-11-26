using System;
using System.Collections.Generic;
using System.Linq;
using Dirac.Logging;
using System.Text;

using Dirac.GameServer.Network;

namespace Dirac.GameServer.Core
{
    public abstract class BaseInventory
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public Dictionary<int, IInventoriable> Items { get; set; } //dynamicID -> item

        public readonly Int32[,] backpack;


        public BaseInventory(int _rows, int _columns)
        {
            this.Rows = _rows;
            this.Columns = _columns;
            this.backpack = new int[_rows, _columns];
            this.Items = new Dictionary<int, IInventoriable>();
        }

        public Int32[,] GetBackPackMatrix()
        {
            return this.backpack;
        }

        /// <summary>
        /// Checks whether the inventory contains an item
        /// </summary>
        public bool Contains(int itemID)
        {
            return Items.ContainsKey(itemID);
        }

        /// <summary>
        /// Removes an item from the backpack
        /// </summary>
        public Boolean removeItem(InventoryItem item)
        {
            if (!Items.ContainsKey(item.DynamicID))
            {
                Logging.LogManager.DefaultLogger.Error("trying to remove an item that does not exist!");
                return false;
            }

            Items.Remove(item.DynamicID);

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    if (backpack[r, c] == item.DynamicID) //hmmm interesting
                    {
                        backpack[r, c] = 0;
                    }
                }
            }

            item.EquipmentSlot = EquipmentSlotId.Inventory; //tendria que ser world quizas pero bueno...
            // item.InventorySlot = new inventoryslot(R, C) //nose q poner... tampoco importa?
            return true;
        }

        /// <summary>
        /// Adds an item to the backpack, used for moving from to inv. (for Pickup use AddItem)
        /// </summary>
        protected Boolean addItemAtPosition(InventoryItem item, InventorySlot _slot)
        {
            int row = _slot.R;
            int column = _slot.C;

            InventorySize size = item.InventorySize;

            //check backpack boundaries
            if (row + size.Height > Rows || column + size.Width > Columns)
                return false;

            if (!Items.ContainsKey(item.DynamicID)) //hmm OJO AL DUPE...
                Items.Add(item.DynamicID, item);

            for (int h = 0; h < size.Height; h++)
            {
                for (int w = 0; w < size.Width; w++)
                {
                    //System.Diagnostics.Debug.Assert(_backpack[r, c] == 0, "You need to remove an item from the backpack before placing another item there");
                    backpack[row + h, column + w] = item.DynamicID; // para cada slot ocupado, relleno con id
                }
            }
            item.InventorySlot.R = row;
            item.InventorySlot.C = column;
            item.EquipmentSlot = EquipmentSlotId.Inventory;

            return true;
        }

        protected Boolean hasFreeSpace(InventoryItem item)
        {
            Boolean result = findSlotForItem(item) != null;
            return result;
        }


        /// <summary>
        /// Find an inventory slot with enough space for an item
        /// </summary>
        /// <returns>Slot or null if there is no space in the backpack</returns>
        protected InventorySlot findSlotForItem(InventoryItem item)
        {
            InventorySize size = item.InventorySize;

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (canPutitemThere(item, r, c) == true)
                        return new InventorySlot() { R = r, C = c };
            return null;
        }

        protected bool canPutitemThere(InventoryItem item, int row, int colum)
        {
            InventorySize itemsize = item.InventorySize;
            for (int r = 0; r < itemsize.Height; r++)
            {
                for (int c = 0; c < itemsize.Width; c++)
                {
                    if ((row + r) >= this.Rows || (colum + c) >= this.Columns)
                        return false;
                    if (backpack[row + r, colum + c] != 0)
                        return false; //no puede, algun lugar de los q ocupa esta ocupado
                }
            }
            return true;
        }
        protected bool canPutitemThere_checking_self_item(InventoryItem item, int row, int colum)
        {
            InventorySize itemsize = item.InventorySize;
            for (int r = 0; r < itemsize.Height; r++)
            {
                for (int c = 0; c < itemsize.Width; c++)
                {
                    if (backpack[row + r, colum + c] != 0)
                    {
                        if (backpack[row + r, colum + c] != item.DynamicID)
                            return false; //no puede, algun lugar de los q ocupa esta ocupado
                    }
                }
            }
            return true;
        }
    }

    public class InventorySize
    {
        public int Width;
        public int Height;

        public InventorySize() { }
        public InventorySize(int x, int y)
        {
            Width = x;
            Height = y;
        }
    }

    public class InventorySlot
    {
        public int R;
        public int C;

        public InventorySlot() { }
        public InventorySlot(int r, int c)
        {
            R = r;
            C = c;
        }

        public void Parse(GameBitBuffer buffer)
        {
            R = buffer.ReadInt(32);
            C = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, R);
            buffer.WriteInt(32, C);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventorySlot:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("X: " + R.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Y: " + C.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

        public string toDbString()
        {
            string ret = this.R.ToString("00") + this.C.ToString("00");
            return ret;
        }
        public void fromDbString(string str)
        {
            string str_r = str.Substring(0, 2);
            string str_c = str.Substring(2, 2);

            int r = int.Parse(str_r);
            int c = int.Parse(str_c);

            this.R = r;
            this.C = c;
        }
    }

    public enum InventoryWindowsID : int
    {
        PlayerInventory = 1,
        Vault = 2,
        NPC = 3,
        _SHOP = 4,
    }


}
