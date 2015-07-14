using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Calculator
{
    public class RangeCalculatorResult
    {
        public HoldemHandRound Round { get; set; }

        public CompiledCondition Condition { get; set; }

        public int Count { get; set; }

        public int Total { get; set; }

        public int PlayerIndex { get; set; }

        public int WinCount { get; set; }
    }
}
