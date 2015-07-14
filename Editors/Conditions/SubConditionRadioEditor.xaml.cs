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
    /// Interaction logic for SubConditionRadioEditor.xaml
    /// </summary>
    public partial class SubConditionRadioEditor : UserControl
    {
        public SubConditionRadioEditorModel Model { get; private set; }

        public SubConditionRadioEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            SetModel(null, new DependencyPropertyChangedEventArgs());
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = this.DataContext as SubConditionRadioEditorModel;
            if (Model == null) return;

            OptionsBorder.Visibility = Model.IsSelected ? Visibility.Visible : Visibility.Collapsed;
        }

        protected void OnChecked(object sender, RoutedEventArgs e)
        {
            OptionsBorder.Visibility = Model.IsSelected ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
