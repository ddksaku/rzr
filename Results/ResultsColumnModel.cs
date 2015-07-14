using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Results
{
    [Editor(typeof(ColumnHeaderCell))]
    public interface ResultsColumnModel
    {
        string Name { get; set; }

        double Width { get; set; }        
    }
}
