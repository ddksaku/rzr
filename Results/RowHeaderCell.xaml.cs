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
    /// Interaction logic for RowHeaderCell.xaml
    /// </summary>
    public partial class RowHeaderCell : UserControl
    {
        protected ResultsRowModel _model;

        public RowHeaderCell()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            Initialise();
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as ResultsRowModel;
            Initialise();
        }

        private void Initialise()
        {
            if (_model == null) return;
            this.Height = _model.Height;
        }
    }
}
