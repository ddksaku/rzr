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
using Rzr.Core.Calculator;

namespace Rzr.Core.Editors.Conditions
{
    /// <summary>
    /// Interaction logic for ConditionSelector.xaml
    /// </summary>
    public partial class ConditionSelector : UserControl
    {
        protected ConditionSelectorModel Model { get; private set; }

        public ConditionSelector()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            SetModel(this, new DependencyPropertyChangedEventArgs());
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = DataContext as ConditionSelectorModel;
            if (Model == null) return;
        }

        protected void AddItem(object sender, RoutedEventArgs e)
        {
            if (OnAdd != null) OnAdd();
        }

        protected void Save(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Model.Service.Name) || 
                Model.Service.Name == ConditionService.RZR_DEFAULT_CONDITION_NAME)
            {
                CancelButton.Visibility = Visibility.Visible;
                Overlay.Visibility = Visibility.Visible;
                SaveNewButton.Visibility = Visibility.Visible;
                NewTitle.Visibility = Visibility.Visible;
            }
            else
            {
                Model.SaveService();
                if (OnClose != null) OnClose();
            }
        }

        protected void Cancel(object sender, RoutedEventArgs e)
        {
            WindowManager.ClosePopup(WindowManager.CONDITIONS_EDITOR);
        }

        protected void EditBase(object sender, RoutedEventArgs e)
        {
            CancelButton.Visibility = Visibility.Visible;
            Overlay.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Visible;            
            AvailableItems.Visibility = Visibility.Visible;
        }

        protected void SaveBase(object sender, RoutedEventArgs e)
        {
            Model.UpdateService();

            NewTitle.Visibility = Visibility.Hidden;
            CancelButton.Visibility = Visibility.Hidden;
            Overlay.Visibility = Visibility.Hidden;
            SaveButton.Visibility = Visibility.Hidden;
            AvailableItems.Visibility = Visibility.Hidden;            
        }

        protected void SaveNewBase(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Model.Service.Name) || Model.Service.Name == ConditionService.RZR_DEFAULT_CONDITION_NAME)
                return;

            Model.SaveService();

            NewTitle.Visibility = Visibility.Hidden;
            CancelButton.Visibility = Visibility.Hidden;
            Overlay.Visibility = Visibility.Hidden;
            SaveNewButton.Visibility = Visibility.Hidden;
            AvailableItems.Visibility = Visibility.Hidden;
        }

        protected void CancelBase(object sender, RoutedEventArgs e)
        {
            NewTitle.Visibility = Visibility.Hidden;
            CancelButton.Visibility = Visibility.Hidden;
            Overlay.Visibility = Visibility.Hidden;
            SaveButton.Visibility = Visibility.Hidden;
            SaveNewButton.Visibility = Visibility.Hidden;
            AvailableItems.Visibility = Visibility.Hidden;

            if (OnClose != null) OnClose();
        }

        protected void Distribute(object sender, RoutedEventArgs e)
        {
            Model.Distribute(30, 0);
        }

        protected AvailableConditionListing startDrag;
        protected AvailableConditionListing hoverListing;
        
        protected void ListMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (AvailableConditionListingModel model in Model.Conditions)
            {
                model.Background = (Brush)AvailableConditionListingModel.BackgroundProperty.DefaultMetadata.DefaultValue;
            }

            Mouse.OverrideCursor = Cursors.Hand;
            startDrag = FindAncestor<AvailableConditionListing>((DependencyObject)e.OriginalSource);
            startDrag.DragStatus(true);
        }

        protected void ListMouseMove(object sender, MouseEventArgs e)
        {
            if (startDrag == null) return;

            AvailableConditionListing currentHover = FindAncestor<AvailableConditionListing>((DependencyObject)e.OriginalSource);
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
            AvailableConditionListing newListing = FindAncestor<AvailableConditionListing>((DependencyObject)e.OriginalSource);
            if (newListing != startDrag)
            {
                int oldIndex = Model.Conditions.IndexOf(startDrag.Model);
                int newIndex = Model.Conditions.IndexOf(newListing.Model);
                Model.Conditions.Move(oldIndex, newIndex);
                Model.InvalidateProperty(ConditionSelectorModel.ConditionsProperty);
            }
            if (hoverListing != null)
                hoverListing.AnimateDragOver(false);
            hoverListing = null;
            startDrag.DragStatus(false);
            startDrag = null;
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

        public event EmptyEventHandler OnAdd;
        public event EmptyEventHandler OnClose;
    }
}
