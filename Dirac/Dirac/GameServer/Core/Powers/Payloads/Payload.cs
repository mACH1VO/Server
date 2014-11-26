using System;
using System.Linq;
using Dirac.Logging;
using Dirac.GameServer.Types;
using Dirac.GameServer;

namespace Dirac.GameServer.Core
{
    public abstract class Payload
    {
        public SkillContext Context;
        public Actor Target;

        public Payload(SkillContext context, Actor target)
        {
            this.Context = context;
            this.Target = target;
        }

        public abstract void Apply();

    }
}
