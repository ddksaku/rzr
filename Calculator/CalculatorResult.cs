using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Calculator
{
    public class CalculatorResult
    {
        public ConditionContainer[][] conditions { get; private set; }

        public int Count { get; set; }

        public int Total { get; set; }
    }
}
