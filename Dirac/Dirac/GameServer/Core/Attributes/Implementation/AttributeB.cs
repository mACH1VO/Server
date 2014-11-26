using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dirac.GameServer.Core
{
    public class AttributeB : Attribute
    {
        public bool DefaultValue { get { return _defaultValue.Value != 0; } }


        public AttributeB() { }
        public AttributeB(int id, int defaultValue, int u3, int u4, int u5, string scriptA, string scriptB, string name, AttributeEncoding encodingType, byte u10, int min, int max, int bitCount)
            : base(id, defaultValue, u3, u4, u5, scriptA, scriptB, name, encodingType, u10, min, max, bitCount)
        {

        }

    }
}
