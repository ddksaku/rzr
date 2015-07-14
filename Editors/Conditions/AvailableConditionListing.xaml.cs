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
    /// Interaction logic for AvailableConditionListing.xaml
    /// </summary>
    public partial class AvailableConditionListing : UserControl
    {
        public AvailableConditionListingModel Model { get; private set; }

        public AvailableConditionListing()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            SetModel(this, new DependencyPropertyChangedEventArgs());
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = DataContext as AvailableConditionListingModel;
            if (Model == null) return;
        }

        protected void Edit(object sender, RoutedEventArgs e)
        {
            Model.OnEdit();
        }

        public void AnimateDragOver(bool hover)
        {
            InsertHere.Height = hover ? new GridLength(24) : new GridLength(0);
        }

        public void DragStatus(bool drag)
        {
            Model.Background = drag ? new SolidColorBrush(Colors.Blue) : 
                (Brush)AvailableConditionListingModel.BackgroundProperty.DefaultMetadata.DefaultValue;
        }
    }
}
