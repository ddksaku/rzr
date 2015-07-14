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

namespace Rzr.Core.Editors.Conditions
{
    /// <summary>
    /// Interaction logic for ConditionSetupEditor.xaml
    /// </summary>
    public partial class ConditionSetupEditor : UserControl
    {
        public ConditionSetupModel Model { get; private set; }

        public ConditionSetupEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            SetModel(null, new DependencyPropertyChangedEventArgs());
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = this.DataContext as ConditionSetupModel;
            if (Model == null) return;

            Options.Visibility = Model.IsSelected ? Visibility.Visible : Visibility.Collapsed;
            Check.Visibility = Model.GroupName == null ? Visibility.Visible : Visibility.Hidden;
            Radio.Visibility = Model.GroupName != null ? Visibility.Visible : Visibility.Hidden;
        }

        protected void OnChecked(object sender, RoutedEventArgs e)
        {
            Options.Visibility = Model.IsSelected ? Visibility.Visible : Visibility.Collapsed;
        }

    }
}
