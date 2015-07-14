using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Calculator
{
    public class BoardSummary
    {
        public uint zero;
        public uint one;
        public uint two;
        public uint three;
        public uint four;
        public uint ranks;

        public uint[] total;
        public uint[] variable;

        public override string ToString()
        {
            string ret = "";
            for (int i = 0; i < 13; i++)
            {
                ret += total[i] + ",";
            }
            return ret;
        }
    }
}
