using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dirac.GameServer.Core.AI.Actions.States
{
    public interface IStrategy
    {
        AIAction Strategy { get; set; }
    }
}
