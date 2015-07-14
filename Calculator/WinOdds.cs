using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Calculator
{
    public class WinOdds
    {
        public string WinPercent { get; private set; }
        public string DrawPercent { get; private set; }
        public string LossPercent { get; private set; }

        public WinOdds(string win, string draw, string loss)
        {
            WinPercent = win;
            DrawPercent = draw;
            LossPercent = loss;
        }
    }
}
