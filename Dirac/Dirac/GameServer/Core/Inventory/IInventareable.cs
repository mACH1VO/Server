using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dirac.GameServer.Core
{
    public interface IInventoriable
    {
        InventorySlot InventorySlot { get; set; }
        InventorySize InventorySize { get; set; }
    }
}
