using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using Rzr.Core.Controls;

namespace Rzr.Core.Results
{
    /// <summary>
    /// Control for printing a set of results based on a set of conditions
    /// </summary>
    public class ResultsTable : Canvas
    {
        #region properties

        /// <summary>
        /// The model containing the row and column data that backs this table
        /// </summary>
        protected ResultsTableModel _model;

        /// <summary>
        /// Row groups
        /// </summary>
        protected ResultsTableGroup[] _groups;

        /// <summary>
        /// The height of the header row
        /// </summary>
        public double ColumnHeaderHeight { get; set; }

        /// <summary>
        /// The width of the header column
        /// </summary>
        public double RowHeaderWidth { get; set; }

        /// <summary>
        /// The height of a divider between groups
        /// </summary>
        public double DividerHeight { get; set; }

        /// <summary>
        /// The standard horizontal space between cells
        /// </summary>
        public double HorizontalSpacing { get; set; }

        /// <summary>
        /// The standard vertical space between cells
        /// </summary>
        public double VerticalSpacing { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ResultsTable()
        {
            DividerHeight = 10;
            HorizontalSpacing = -1;
            VerticalSpacing = 0;
            ColumnHeaderHeight = 30;
            RowHeaderWidth = 150;
            _groups = new ResultsTableGroup[0];

            this.DataContextChanged += SetModel;
            Initialise();
        }

        /// <summary>
        /// Fired when the data context is changed
        /// </summary>
        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as ResultsTableModel;
            if (_model == null) return;

            Initialise();            
        }

        #endregion

        #region build table

        /// <summary>
        /// Create the table when the data context is initialised
        /// </summary>
        private void Initialise()
        {
            if (_model == null) return;

            //-------------------------------------------------------------------------------------
            // 
            //-------------------------------------------------------------------------------------            
            SetDimensions();
            SetGroups();
            BuildHeader();

            //-------------------------------------------------------------------------------------
            // 
            //-------------------------------------------------------------------------------------            
            double top = ColumnHeaderHeight;
            foreach (ResultsTableGroup group in _groups.OrderBy(x => x.DisplayOrder))
                BuildGroup(group, ref top);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetDimensions()
        {
            this.Height = _model.Rows.Sum(x => x.Height) + (_model.Rows.Length * VerticalSpacing) + ColumnHeaderHeight;
            this.Width = _model.Columns.Sum(x => x.Width) + (_model.Columns.Length * HorizontalSpacing) + RowHeaderWidth;
        }

        /// <summary>
        /// Add the header row
        /// </summary>
        protected void BuildHeader()
        {
            Control editor = GetEditorForObject(_model.OptionsModel);
            editor.Height = ColumnHeaderHeight;
            SetChild(editor, 0, 0);
            double widthSoFar = RowHeaderWidth + HorizontalSpacing;

            for (int i = 0; i < _model.Columns.Length; i++)            
            {
                editor = GetEditorForObject(_model.Columns[i]);
                editor.Height = ColumnHeaderHeight;
                SetChild(editor, 0, widthSoFar);
                widthSoFar += _model.Columns[i].Width + HorizontalSpacing;
            }

            this.Width = widthSoFar;
        }

        private void SetGroups()
        {
            _groups = _model.Rows.Select(x => x.Group).Distinct().
                Select(x => new ResultsTableGroup() { Name = x }).ToArray();
        }

        /// <summary>
        /// Add a group of rows
        /// </summary>
        /// <param name="group"></param>
        protected void BuildGroup(ResultsTableGroup group, ref double top)
        {
            double rowWidth = _model.Columns.Sum(x => x.Width) + ((_model.Columns.Length - 1 ) * HorizontalSpacing);
            BuildDivider(ref top);
            Color[] background = new Color[] { Color.FromRgb(60, 60, 60), Color.FromRgb(0, 0, 0) };

            int count = 0;
            foreach (ResultsRowModel row in _model.Rows.Where(x => x.Group == group.Name).OrderBy(x => x.DisplayOrder))
            {
                count++;
                BuildRow(row, ref top, rowWidth, background[count % 2]);
            }
        }

        /// <summary>
        /// Create a dividing space
        /// </summary>
        /// <param name="top"></param>
        private void BuildDivider(ref double top)
        {
            top += DividerHeight;
        }

        /// <summary>
        /// Add a standard data row
        /// </summary>
        /// <param name="rowNumber"></param>
        protected virtual void BuildRow(ResultsRowModel row, ref double top, double rowWidth, Color backgroundColor)
        {
            double widthSoFar = 0;
            Control editor = GetEditorForObject(row);
            editor.Width = RowHeaderWidth;
            SetChild(editor, top, widthSoFar);
            widthSoFar += RowHeaderWidth + HorizontalSpacing;
   
            Canvas background = new Canvas() { Width = rowWidth, Height = row.Height, Background = new SolidColorBrush(backgroundColor) };
            SetChild(background, top, widthSoFar);

            for (int i = 0; i < _model.Columns.Length; i++)
            {
                ResultsCellModel cellModel = row.GetCellForColumn(i, _model.Columns[i]);
                _model.Cells.Add(cellModel);
                editor = GetEditorForObject(cellModel);
                SetChild(editor, top, widthSoFar);
                widthSoFar += _model.Columns[i].Width + HorizontalSpacing;
            }
            top += row.Height + VerticalSpacing;
        }

        #endregion

        #region utilities

        protected void SetChild(UIElement element, double top, double left)
        {
            this.Children.Add(element);
            Canvas.SetTop(element, top);
            Canvas.SetLeft(element, left);
        }

        protected Control GetEditorForObject(object obj)
        {
            Type type = obj.GetType();
            object[] attributes = type.GetCustomAttributes(typeof(Editor), true);
            if (attributes.Length > 0)
            {
                Type editorType = ((Editor)attributes[0]).EditorType;
                Control ret = Activator.CreateInstance(editorType) as Control;
                ret.DataContext = obj;
                return ret;
            }
            return null;
        }

        #endregion

        #region events

        public void CompileGrid()
        {
            Children.Clear();
            Initialise();
            this.InvalidateVisual();
        }

        #endregion

        
    }    
}
