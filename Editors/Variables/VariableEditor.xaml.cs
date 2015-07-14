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
using System.Globalization;

namespace Rzr.Core.Editors.Variables
{
    /// <summary>
    /// Interaction logic for VariableEditor.xaml
    /// </summary>
    public partial class VariableEditor : UserControl
    {
        protected Variable _model;

        public VariableEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as Variable;
        }

        protected void DoAction(object sender, RoutedEventArgs e)
        {
            switch (_model.State)
            {
                case VariableState.New:
                    ActionButton.Content = "Edit";
                    _model.State = VariableState.Saved;
                    break;
                case VariableState.Editing:
                    ActionButton.Content = "Edit";
                    _model.State = VariableState.Saved;
                    break;
                case VariableState.Saved:
                    ActionButton.Content = "Save";
                    _model.State = VariableState.Editing;
                    break;
            }
        }

        protected void DoDelete(object sender, RoutedEventArgs e)
        {
            _model.Delete();
        }

    }

    public class VariableStateToEditVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            VariableState state = (VariableState)value;

            switch (state)
            {
                case VariableState.Saved: return Visibility.Hidden;
                case VariableState.New: return Visibility.Visible;
                case VariableState.Editing: return Visibility.Visible;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class VariableStateToLabelVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            VariableState state = (VariableState)value;

            switch (state)
            {
                case VariableState.Saved: return Visibility.Visible;
                case VariableState.New: return Visibility.Hidden;
                case VariableState.Editing: return Visibility.Hidden;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
