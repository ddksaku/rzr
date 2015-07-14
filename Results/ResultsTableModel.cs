using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Rzr.Core.Results
{
    public class ResultsTableModel : DependencyObject
    {
        public ResultsOptionsModel OptionsModel { get; set; }

        public ResultsColumnModel[] Columns { get; set; }

        public ResultsRowModel[] Rows { get; set; }

        public List<ResultsCellModel> Cells { get; set; }        

        public ResultsTableModel()
        {
            Columns = new ResultsColumnModel[0];
            Rows = new ResultsRowModel[0];
            Cells = new List<ResultsCellModel>();
            OptionsModel = new ResultsOptionsModel();
        }

        public void SetDimensions<T, U>(List<T> columns, List<U> rows) where T : ResultsColumnModel where U : ResultsRowModel
        {
            Columns = columns.Select(x => (ResultsColumnModel)x).ToArray<ResultsColumnModel>();
            Rows = rows.Select(x => (ResultsRowModel)x).ToArray<ResultsRowModel>();
            DoCompile();
        }

        public void DoCompile()
        {
            if (Compile != null) Compile();
        }

        public void DoRefresh()
        {
            foreach (ResultsCellModel model in Cells)
                model.Refresh();
        }

        public EmptyEventHandler Compile;
        
    }
}
