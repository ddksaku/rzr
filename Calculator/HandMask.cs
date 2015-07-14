using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Calculator
{
    public struct HandMask
    {
        public uint Result { get; set; }
        public bool IsWinner { get; set; }
        public ulong Mask { get; set; }
    }
}
