using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dirac;
using Dirac.GameServer.Types;
using Dirac.Logging;
using Dirac.Extensions;
using Dirac.Math;


namespace Dirac.GameServer.Core
{
    public static class MonsterFactory
    {
        private static Dictionary<String, int> MonstersSNObyName = new Dictionary<string, int>();
        public static void Initialize()
        {

        }

        public static Monster Create(int snoId)
        {
            if (Actor.SNOToFile.ContainsKey(snoId))
            {
                if (Actor.SNOToFile[snoId].ToLower().Contains("monster")) //si es monster
                {
                    Monster monster = new Monster(snoId);
                    return monster;
                }
            }
            return null;
        }

        /*public static Monster Create(World world, float x, float y, float z, int snoId)
        {
            Monster monster = new Monster(world, snoId);
            //monster.SetFacingRotation((float)(RandomHelper.NextDouble() * 2.0f * Math.PI));
            Vector3 pos = new Vector3(x, y, z);
            monster.EnterWorld(pos);
            return monster;
        }

        public static Monster Create(World world, Vector3 pos, String name)
        {
            Monster monster = new Monster(world, MonstersSNObyName[name]);
            //monster.SetFacingRotation((float)(RandomHelper.NextDouble() * 2.0f * Math.PI));
            monster.EnterWorld(pos);
            return monster;
        }

        public static Monster putmonster(World world, float x, float y, float z, int snoId)
        {
            Monster monster = new Monster(world, snoId);
            //monster.SetFacingRotation((float)(RandomHelper.NextDouble() * 2.0f * Math.PI));
            Vector3 pos = new Vector3(x, y, z);
            monster.EnterWorld(pos);
            return monster;
        }*/
    }
}
