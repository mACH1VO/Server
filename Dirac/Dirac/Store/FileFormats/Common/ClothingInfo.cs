using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dirac.Store.FileFormats
{
    [Serializable]
    public class ClothingInfo
    {
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float ScaleZ { get; set; }

        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }

        public float Qx { get; set; }
        public float Qy { get; set; }
        public float Qz { get; set; }
        public float Qw { get; set; }

        public ClothingInfo() { }
    }
}
