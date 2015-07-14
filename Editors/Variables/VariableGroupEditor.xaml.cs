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
    /// Interaction logic for VariableGroupEditor.xaml
    /// </summary>
    public partial class VariableGroupEditor : UserControl
    {
        protected VariableGroup _model;

        public VariableGroupEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            _model = this.DataContext as VariableGroup;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as VariableGroup;
            if (_model == null) return;
        }

        protected void AddVariable(object sender, RoutedEventArgs e)
        {
            _model.AddVariable(new Variable() { Name = "New Variable" });
        }

        protected void Hide(object sender, RoutedEventArgs e)
        {
            _model.IsSelected = false;
        }
    }
}
