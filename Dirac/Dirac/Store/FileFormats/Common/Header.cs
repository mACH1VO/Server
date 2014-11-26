using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dirac.Store.FileFormats
{
    [Serializable]
    public class Header
    {
        public String FileName { get; set; }
        public int SnoType { get; set; }
        public int SNOId { get; set; }
    }
}
