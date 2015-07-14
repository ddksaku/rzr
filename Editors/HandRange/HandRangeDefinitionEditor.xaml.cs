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
using Rzr.Core.Data;

namespace Rzr.Core.Editors.HandRange
{
    /// <summary>
    /// Interaction logic for HandRangeDefinitionEditor.xaml
    /// </summary>
    public partial class HandRangeDefinitionEditor : UserControl
    {
        protected HandRangeDefinitionListing startDrag;
        protected HandRangeDefinitionListing hoverListing;

        protected HandRangeDefinitionModel Model { get; private set; }

        public HandRangeDefinitionEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            SetModel(this, new DependencyPropertyChangedEventArgs());
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = DataContext as HandRangeDefinitionModel;
            if (Model == null) return;
        }

        protected void ListMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (HandDefinitionModel model in Model.Hands)
            {
                model.Background = (Brush)HandDefinitionModel.BackgroundProperty.DefaultMetadata.DefaultValue;
            }

            Mouse.OverrideCursor = Cursors.Hand;
            startDrag = FindAncestor<HandRangeDefinitionListing>((DependencyObject)e.OriginalSource);
            startDrag.DragStatus(true);
        }

        protected void ListMouseMove(object sender, MouseEventArgs e)
        {
            if (startDrag == null) return;

            HandRangeDefinitionListing currentHover = FindAncestor<HandRangeDefinitionListing>((DependencyObject)e.OriginalSource);
            if (currentHover != hoverListing && currentHover != startDrag)
            {
                if (hoverListing != null)
                    hoverListing.AnimateDragOver(false);

                hoverListing = currentHover;
                if (hoverListing != null)
                    hoverListing.AnimateDragOver(true);
            }
        }

        protected void ListMouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = null;
            HandRangeDefinitionListing newListing = FindAncestor<HandRangeDefinitionListing>((DependencyObject)e.OriginalSource);
            if (startDrag != null && newListing != startDrag)
            {
                Model.RearrangeRanks(startDrag.Model, newListing.Model); // re-arrange ranks from start drag to new listing                                                
            }
            if (hoverListing != null)
                hoverListing.AnimateDragOver(false);
            hoverListing = null;
            if (startDrag != null)
            {
                startDrag.DragStatus(false);
            }
            startDrag = null;
        }

        protected void Save(object sender, RoutedEventArgs e)
        {
            if (OnSave != null) OnSave();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (OnDelete != null) OnDelete();
        }

        protected void Exit(object sender, RoutedEventArgs e)
        {
            if (OnExit != null) OnExit();
        }

        private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        public event EmptyEventHandler OnSave;
        public event EmptyEventHandler OnDelete;
        public event EmptyEventHandler OnExit;                
    }
}
