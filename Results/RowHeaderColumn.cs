using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Results
{
    public class RowHeaderColumn : ResultsColumnModel
    {
        public string Name { get; set; }

        public double Width { get; set; }

        public RowHeaderColumn()
        {
            Width = 100;
        }
    }
}
