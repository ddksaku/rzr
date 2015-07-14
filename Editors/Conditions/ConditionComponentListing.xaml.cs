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
    /// Interaction logic for ConditionComponentListing.xaml
    /// </summary>
    public partial class ConditionComponentListing : UserControl
    {
        public ConditionComponentListingModel Model { get; private set; }        

        public ConditionComponentListing()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = DataContext as ConditionComponentListingModel;
            if (Model == null) return;

            EditButton.Visibility = Model.Atom.Type == Calculator.ConditionAtomType.Nested ?
                Visibility.Hidden : Visibility.Visible;
        }

        protected void EditComponent(object sender, RoutedEventArgs e)
        {
            Model.Edit();
        }

        protected void DeleteComponent(object sender, RoutedEventArgs e)
        {
            Model.Delete();
        }
    }
}
