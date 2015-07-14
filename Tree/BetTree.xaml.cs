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
using Rzr.Core.Tree.DataModels;
using Rzr.Core.Xml;
using Rzr.Core.Partials;

namespace Rzr.Core.Tree
{
    /// <summary>
    /// Interaction logic for BetTree.xaml
    /// </summary>
    public partial class BetTree : UserControl
    {
        #region properties

        public BetTreeModel Controller { get; set; }
        
        protected BetTreeNodeModel _rootNode;

        protected BetTreeNodeModel _activeModel;

        protected BetTreeEditor _activeEditor;

        #endregion

        #region constructor

        public BetTree()
        {
            InitializeComponent();

            this.Background = new ImageBrush(Utilities.LoadBitmap(Properties.Resources.background));

            this.DataContextChanged += OnDataContextChanged;
            EditorHeader.DefaultChanged += SetRangeControlsVisibility;
            ManualResults.OnSaveAndExit += OnManualResultsExit;
            AddPartialDialogue.OnLoad += LoadPartial;
            AddPartialDialogue.OnClose += ClosePartialDialog;
            WizardDialogue.OnFinish += LoadWizard;
            WizardDialogue.OnClose += CloseWizardDialog;
            
            this.Loaded += OnTreeLoaded;
        }

        protected void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Controller = this.DataContext as BetTreeModel;
            if (Controller == null) return;

            Controller.ShowWizard += ShowPartialDialog;
            Controller.EditNode += EditNode;
            Controller.TreeInitialised += TreeInitialised;
            Controller.Calculate += OnCalculate;

            TreeInitialised();
            Controller.RefreshTree();
        }

        protected void OnTreeLoaded(object sender, EventArgs e)
        {
            Controller.OnNodeChanged();
        }

        protected void OnCalculate()
        {
            if (Calculate != null) Calculate();
            MainTree.RefreshNodes(Controller.RootNode);
        }

        public event EmptyEventHandler Calculate;
        

        #endregion

        #region event handlers

        public void TreeInitialised()
        {
            if (Controller.RootNode == null) return;

            _rootNode = Controller.RootNode;
            MainTree.SetRootNode(_rootNode);
        }

        public void ShowPartialDialog(BetTreeNodeModel model)
        {
            _activeModel = model;
            AddPartialDialogue.DataContext = model;
            AddPartialEditor.Visibility = Visibility.Visible;            
        }

        public void ClosePartialDialog()
        {
            AddPartialEditor.Visibility = Visibility.Hidden;
        }

        public void ShowWizardDialog(TreePartialMeta meta)
        {
            PartialGenerator generator = new PartialGenerator(meta, Controller, _activeModel, Controller.Service);
            WizardDialogue.DataContext = generator;
            WizardEditor.Visibility = Visibility.Visible;
        }

        public void CloseWizardDialog()
        {
            WizardEditor.Visibility = Visibility.Hidden;
        }

        public void LoadPartial(TreePartialMeta meta)
        {
            ClosePartialDialog();
            ShowWizardDialog(meta);
        }

        public void LoadWizard(PartialGenerator generator)
        {
            generator.GetPartialTree(_activeModel);
            MainTree.RefreshNodes(_activeModel);
            CloseWizardDialog();
        }

        public void EditNode(BetTreeNodeModel model)
        {
            _activeModel = model;
            if (_activeModel == null) return;

            switch (_activeModel.Meta)
            {
                case BetTreeNodeService.PREFLOP_NODE_META:
                case BetTreeNodeService.POSTFLOP_NODE_META:
                    ShowBetRangeEditor();
                    break;
                case BetTreeNodeService.RESULT_NODE_META:
                    ShowResultEditor();
                    break;
                case BetTreeNodeService.SHOWDOWN_NODE_META:
                    break;
            }
        }

        protected void ShowBetRangeEditor()
        {
            Editor.Visibility = Visibility.Visible;

            _activeEditor = (BetTreeEditor)Utilities.GetEditorControl(_activeModel.Data);
            if (_activeEditor == null) return;

            Editor.Children.Add((Control)_activeEditor);
            Grid.SetColumn((Control)_activeEditor, 2);
            Grid.SetRow((Control)_activeEditor, 3);
            ((Control)_activeEditor).HorizontalAlignment = HorizontalAlignment.Center;

            EditorHeader.DataContext = _activeModel;

            BetControls.DataContext = _activeModel.Data.BetModel;
            BetControlsMask.Visibility = _activeModel.Children.Count(
                x => !(x.Data is ResultsModel || x.Data is ShowdownModel)) > 0
                ? Visibility.Visible : Visibility.Hidden;

            _activeEditor.DataContext = _activeModel.GetEditContext();

            SetRangeControlsVisibility();
        }

        protected void ShowResultEditor()
        {
            ResultEditor.Visibility = Visibility.Visible;
            ManualResults.DataContext = _activeModel.Info;
        }

        public void SetRangeControlsVisibility()
        {
            if (_activeModel == null) return;

            if (_activeModel.IsDefault)
                RangeRow.Height = new GridLength(0);
            else
                RangeRow.Height = new GridLength(370);
        }

        protected void CancelEdit(object sender, RoutedEventArgs e)
        {
            Editor.Children.Remove((Control)_activeEditor);
            _activeEditor = null;
            Editor.Visibility = Visibility.Hidden;
        }

        protected void SaveEdit(object sender, RoutedEventArgs e)
        {
            _activeModel.Save(_activeEditor.DataContext);

            Editor.Children.Remove((Control)_activeEditor);
            _activeEditor = null;
            Editor.Visibility = Visibility.Hidden;

            Controller.RefreshTree();            
        }

        protected void OnManualResultsExit()
        {
            ResultEditor.Visibility = Visibility.Hidden;
        }

        #endregion
    }

    class TreeViewLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TreeViewItem item = (TreeViewItem)value;
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
            return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
