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
    /// Interaction logic for ExistingConditionListing.xaml
    /// </summary>
    public partial class ExistingConditionListing : UserControl
    {
        public ExistingConditionListingModel Model { get; private set; }

        public ExistingConditionListing()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = DataContext as ExistingConditionListingModel;
        }

        protected void Select(object sender, RoutedEventArgs e)
        {
            Model.Select();
        }
    }
}
