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
    /// Interaction logic for ConditionEditor.xaml
    /// </summary>
    public partial class ConditionEditor : UserControl
    {
        public ConditionEditorModel Model { get; private set; }        

        public ConditionEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            ComponentEditor.OnSave += SaveComponent;
            ComponentEditor.OnCancel += CancelComponent;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = DataContext as ConditionEditorModel;
            if (Model == null) return;

            Model.OnEditComponentModel += EditComponentModel;
            Model.OnDeleteComponentModel += DeleteComponentModel;
            Model.OnConditionSelected += ConditionSelected;

            ComponentEditor.DataContext = Model.EditorModel;
            ConditionSelector.DataContext = Model.ConditionSelectorModel;
            
        }

        protected void AddAndComponent(object sender, RoutedEventArgs e)
        {
            ConditionSelector.Visibility = Visibility.Visible;
            Model.ActiveCondition = ConditionEditorModel.ANDCONDITIONS;
        }

        protected void AddAndCondition(object sender, RoutedEventArgs e)
        {
            ConditionComponentListingModel listing = Model.GetConditionAtomForAnd();
            Model.EditorModel.Atom = listing.Atom;
            ComponentsContainer.Visibility = Visibility.Hidden;
            ComponentEditor.Visibility = Visibility.Visible;
        }

        protected void DeleteSelectedAndLine(object sender, RoutedEventArgs e)
        {
            object deleteItem = AndComponents.SelectedItem;
            Model.AndAtoms.Remove((ConditionComponentListingModel)deleteItem);
            Model.InvalidateProperty(ConditionEditorModel.AndAtomsProperty);
        }

        protected void AddOrComponent(object sender, RoutedEventArgs e)
        {
            ConditionSelector.Visibility = Visibility.Visible;
            Model.ActiveCondition = ConditionEditorModel.ORCONDITIONS;
        }

        protected void AddOrCondition(object sender, RoutedEventArgs e)
        {
            ConditionComponentListingModel listing = Model.GetConditionAtomForOr();
            Model.EditorModel.Atom = listing.Atom;
            ComponentsContainer.Visibility = Visibility.Hidden;
            ComponentEditor.Visibility = Visibility.Visible;
        }

        protected void DeleteSelectedOrLine(object sender, RoutedEventArgs e)
        {
            object deleteItem = OrComponents.SelectedItem;
            Model.AndAtoms.Remove((ConditionComponentListingModel)deleteItem);
            Model.InvalidateProperty(ConditionEditorModel.AndAtomsProperty);
        }

        protected void EditComponentModel(object sender, EventArgs e)
        {
            ConditionComponentListingModel listing = sender as ConditionComponentListingModel;
            Model.EditorModel.Atom = listing.Atom;
            ComponentsContainer.Visibility = Visibility.Hidden;
            ComponentEditor.Visibility = Visibility.Visible;
        }

        protected void SaveComponent(object sender, EventArgs e)
        {
            Model.EditorModel.SaveEditor();
            Model.Refresh();
            ComponentsContainer.Visibility = Visibility.Visible;
            ComponentEditor.Visibility = Visibility.Hidden;
        }

        protected void CancelComponent(object sender, EventArgs e)
        {
            ComponentsContainer.Visibility = Visibility.Visible;
            ComponentEditor.Visibility = Visibility.Hidden;
        }

        protected void DeleteComponentModel(object sender, EventArgs e)
        {

        }

        protected void ConditionSelected(object sender, EventArgs e)
        {
            ConditionSelector.Visibility = Visibility.Hidden;            
        }


        protected void Save(object sender, RoutedEventArgs e)
        {
            if (OnSave != null) OnSave();
        }

        protected void Cancel(object sender, RoutedEventArgs e)
        {
            if (OnCancel != null) OnCancel();
        }

        public event EmptyEventHandler OnSave;

        public event EmptyEventHandler OnCancel;
    }
}
