using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Data
{
    public class HoleCardDefinitionList
    {
        public HoleCardDefinition[] Definitions { get; set; }
    }

    public class HoleCardDefinition
    {
        public string Definition { get; set; }

        public float Probability { get; set; }

        public int Card1;

        public int Card2;

        public bool Suited;
    }
}
