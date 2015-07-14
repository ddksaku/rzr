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

namespace Rzr.Core.Editors.HandRange
{
    /// <summary>
    /// Interaction logic for HandRangeDefinitionListing.xaml
    /// </summary>
    public partial class HandRangeDefinitionListing : UserControl
    {
        public HandDefinitionModel Model { get; set; }

        public HandRangeDefinitionListing()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            SetModel(this, new DependencyPropertyChangedEventArgs());
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = DataContext as HandDefinitionModel;
            if (Model == null) return;
        }

        public void DragStatus(bool drag)
        {            
            Model.Background = drag ? new SolidColorBrush(Colors.Blue) :
                (Brush)HandDefinitionModel.BackgroundProperty.DefaultMetadata.DefaultValue;
        }

        public void AnimateDragOver(bool hover)
        {
            InsertHere.Height = hover ? new GridLength(24) : new GridLength(0);
        }
        
        private void txtRankValue_LostFocus(object sender, RoutedEventArgs e)
        {
            float rank;
            if (float.TryParse(txtRankValue.Text, out rank))
            {
                if (rank != Model.Value) // rank values is changed
                {
                    if (rank > 0 && rank <= 169) // rank is in a range
                    {
                        Model.Value = rank;
                        Model.RearrangeRanks(Model);
                        return;
                    }
                }
            }
            
            txtRankValue.Text = Model.Value.ToString();            
        }
    }
}
