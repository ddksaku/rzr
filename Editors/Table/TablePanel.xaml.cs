using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rzr.Core.Editors.Table
{
    /// <summary>
    /// Interaction logic for TablePanel.xaml
    /// </summary>
    public partial class TablePanel : UserControl
    {
        protected TableModel _model;
        protected TablePlayerDetails[] _summaries;

        public TablePanel()
        {
            InitializeComponent();
            ImageBrush brush = new ImageBrush(Utilities.LoadBitmap(Properties.Resources.table_clear));
            brush.Stretch = Stretch.None;
            this.Background = brush;
            this.DataContextChanged += OnDataContextChanged;
        }

        protected void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as TableModel;
            if (_model == null) return;
        }
    }
}
