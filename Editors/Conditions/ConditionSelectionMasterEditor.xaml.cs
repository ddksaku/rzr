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
    /// Interaction logic for ConditionSelectionMasterEditor.xaml
    /// </summary>
    public partial class ConditionSelectionMasterEditor : UserControl
    {
        protected bool _requiresAddition;

        public ConditionSelectionMasterModel Model { get; private set; }

        public ConditionSelectionMasterEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            SetModel(this, new DependencyPropertyChangedEventArgs());

            Selector.OnAdd += OnAdd;
            Selector.OnClose += OnClose;
            Editor.OnSave += OnSave;
            Editor.OnCancel += OnCancel;           
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = DataContext as ConditionSelectionMasterModel;
            if (Model == null) return;

            Model.Edit += OnEdit;
            Selector.DataContext = Model.Selector;
            Editor.DataContext = Model.Editor;
        }

        protected void OnEdit(object sender, EventArgs e)
        {
            Model.ActiveCondition = sender as AvailableConditionListingModel;
            Model.Editor.Title = "Edit Condition";
            Model.Editor.Name = Model.ActiveCondition.Condition.Name;
            Model.Editor.Initialise(Model.ActiveCondition.Condition);

            Selector.Visibility = Visibility.Hidden;
            Editor.Visibility = Visibility.Visible;
        }

        protected void OnAdd()
        {
            Model.Editor.Title = "Add Condition";
            Model.ActiveCondition = null;
            Model.Editor.Name = null;
            Model.Editor.Initialise(null);

            Selector.Visibility = Visibility.Hidden;
            Editor.Visibility = Visibility.Visible;
        }

        protected void OnClose()
        {
            WindowManager.ClosePopup(WindowManager.CONDITIONS_EDITOR);
        }

        protected void OnSave()
        {
            if (Model.ActiveCondition == null)
            {
                string id = Model.Selector.GetNewId();
                ConditionContainer container = Model.Editor.GetCondition(id);
                Model.Selector.AddCondition(container);
            }
            else
            {
                string id = Model.ActiveCondition.Condition.ID;
                ConditionContainer container = Model.Editor.GetCondition(id);
                Model.ActiveCondition.SetCondition(container);
            }

            Selector.Visibility = Visibility.Visible;
            Editor.Visibility = Visibility.Hidden;
        }

        protected void OnCancel()
        {
            Selector.Visibility = Visibility.Visible;
            Editor.Visibility = Visibility.Hidden;
        }        
    }
}
