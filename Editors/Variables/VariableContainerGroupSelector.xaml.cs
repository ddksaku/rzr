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

namespace Rzr.Core.Editors.Variables
{
    /// <summary>
    /// Interaction logic for VariableContainerGroupSelector.xaml
    /// </summary>
    public partial class VariableContainerGroupSelector : UserControl
    {
        protected VariableContainer _model;

        public VariableContainerGroupSelector()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = this.DataContext as VariableContainer;
        }
    }
}
