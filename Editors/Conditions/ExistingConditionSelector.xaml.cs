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
    /// Interaction logic for ExistingConditionSelector.xaml
    /// </summary>
    public partial class ExistingConditionSelector : UserControl
    {
        protected ExistingConditionListingModel Model { get; private set; }

        public ExistingConditionSelector()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = DataContext as ExistingConditionListingModel;
            if (Model == null) return;
        }

        protected void ExitNoSave(object sender, RoutedEventArgs e)
        {
            if (this.Close != null) this.Close(sender, EventArgs.Empty);
        }

        public event EventHandler Close;
    }
}
