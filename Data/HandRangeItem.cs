using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Data
{
    public class HandRangeItem : RangeDataItem, RangeDisplayItem
    {
        public int XCord { get; set; }

        public int YCord { get; set; }

        public string Description { get; set; }

        public string ID { get; set; }

        public float Rank { get; set; }

        public float Weight { get; set; }

        public double Probability { get; set; }        
    }
}
