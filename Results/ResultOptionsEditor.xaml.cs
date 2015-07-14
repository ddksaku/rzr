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
    /// Interaction logic for ResultOptionsEditor.xaml
    /// </summary>
    public partial class ResultOptionsEditor : UserControl
    {
        protected ResultsOptionsModel _model;

        public ResultOptionsEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as ResultsOptionsModel;
        }

        protected void OnOptionsSelected(object sender, RoutedEventArgs e)
        {
            _model.OnOptionsSelected();
        }

    }
}
