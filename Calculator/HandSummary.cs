using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Calculator
{
    public struct HandSummary
    {
        public int Card1 { get; set; }
        public int Card2 { get; set; }
        public bool Paired { get; set; }
        public bool Suited { get; set; }

        public uint one { get; set; }

        public uint two { get; set; }

        public uint ranks { get; set; }
    }
}
