using System;
using System.Collections.Generic;
using System.Linq;
using Dirac.Logging;
using Dirac.GameServer;
using Dirac.GameServer.Types;
using Dirac;

namespace Dirac.GameServer.Core
{
    public static class ItemFactory
    {
        public static List<int> SNOIds = new List<int>();
        public static void Initialize()
        {
        }

        public static Item CreateItem(Int32 SNOId)
        {
            Item itemcreated = new Item(SNOId);
            return itemcreated;
        }

        /*public static Item CreateRandomItem(Map world)
        {
            int SNOId = 101;
            SNOId = SNOIds[RandomHelper.Next(0, SNOIds.Count)];
            Item itemcreated = new Item(world, SNOId);
            return itemcreated;
        }*/
    }
}




