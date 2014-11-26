using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dirac.GameServer;
using Dirac.GameServer.Types;


namespace Dirac.GameServer.Core
{
    public enum DamageType : int
    {
        Physical = 0,
        Fire = 1,
        Lightning = 2,
        Cold = 3,
        Poison = 4,
        Arcane = 5,
        Holy = 6,
        UnknownFlicker = 7
    }
}
