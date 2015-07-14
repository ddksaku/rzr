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

namespace Rzr.Core.Results
{
    /// <summary>
    /// Interaction logic for ColumnHeaderCell.xaml
    /// </summary>
    public partial class ColumnHeaderCell : UserControl
    {
        protected ResultsColumnModel _model;

        public ColumnHeaderCell()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            Initialise();
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as ResultsColumnModel;
            Initialise();
        }

        private void Initialise()
        {
            if (_model == null) return;
            this.Width = _model.Width;
        }
    }
}
