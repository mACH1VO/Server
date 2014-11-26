using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Dirac.GameServer.Core;

namespace Dirac.WorldVisualizer
{
    public class WorldVisualizer
    {
        private WorldVisualizer() { }

        private static WorldVisualizer instance;

        public static WorldVisualizer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WorldVisualizer();
                }
                return instance;
            }
        }
        public List<Actor> DataToDraw()
        {
            /*List<Point> ret = new List<Point>();
            if (GameServer.Game.StartingWorld != null)
            {
                foreach (var actors in GameServer.Game.StartingWorld.Actors.Values)
                {
                    int factor = 3;
                    ret.Add(new Point((int)actors.Position.x * factor, (int)actors.Position.z * factor));
                }
            }
            return ret;*/

            if (GameServer.Game.StartingMap != null)
            {
                return new List<Actor>(GameServer.Game.StartingMap.Actors.Values);
            }
            return new List<Actor>();
        }





    }
}
