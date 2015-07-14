using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Results
{
    [Editor(typeof(RowHeaderCell))]
    public interface ResultsRowModel
    {
        string Group { get; }

        string Name { get; }

        int DisplayOrder { get; }

        int Height { get; set; }

        ResultsCellModel GetCellForColumn(int columnIndex, ResultsColumnModel column);        
    }
}
