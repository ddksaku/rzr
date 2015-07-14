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
using System.ComponentModel;
using Rzr.Core.Tree.DataModels;

namespace Rzr.Core.Tree
{
    /// <summary>
    /// Interaction logic for BetTreeNode.xaml
    /// </summary>
    public partial class BetTreeNode : UserControl
    {
        protected BetTreeNodeModel _model;

        protected Control _info;

        protected Line _linkLine;

        public BetTreeNode()
        {            
            InitializeComponent();
            EditButton.Source = Utilities.LoadBitmap(Properties.Resources.icon_settings);
            QuestionButton.Source = Utilities.LoadBitmap(Properties.Resources.icon_question);
            AddButton.Source = Utilities.LoadBitmap(Properties.Resources.addicon);
            RemoveButton.Source = Utilities.LoadBitmap(Properties.Resources.deleteicon);
            WizardButton.Source = Utilities.LoadBitmap(Properties.Resources.wizard);
            this.DataContextChanged += SetModel;                       
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as BetTreeNodeModel;
            if (_model == null) return;

            _model.Changed += UpdateVisual;
            _model.Interface = this;
            _model.MetaUpdated += UpdateInfo;

            Control editor = Utilities.GetEditorControl(_model);
            MainGrid.Children.Add(editor);
            Grid.SetRow(editor, 2);
            Grid.SetColumn(editor, 1);
            Grid.SetColumnSpan(editor, 3);

            UpdateInfo();
            UpdateVisual();

            _model.Info.ResultsChanged += UpdateVisual;
        }

        protected void UpdateVisual()
        {
            double horizontalPadding = NodePadding.Padding.Left + NodePadding.Padding.Right;
            double verticalPadding = NodePadding.Padding.Top + NodePadding.Padding.Bottom;

            double width = SetColumnWidth(0, _model.CanRemove, 24);
            width += SetColumnWidth(1, Visibility.Visible, _info.Width);
            width += SetColumnWidth(2, Visibility.Visible, 24);
            width += SetColumnWidth(3, _model.CanEdit, 24);
            width += SetColumnWidth(4, Visibility.Visible, 24);
            width += SetColumnWidth(5, _model.CanAdd, 24);
            width += 15;

            this.ContentRow.Height = new GridLength(_info.Height);
            this.Width = width + horizontalPadding;
            this.Height = 34 + _info.Height + verticalPadding;

            if (this.OnLayoutUpdated != null) this.OnLayoutUpdated();
        }

        protected double SetColumnWidth(int columnIndex, Visibility visible, double visibleWidth)
        {
            double width = visible == Visibility.Visible ? visibleWidth : 0;
            MainGrid.ColumnDefinitions[columnIndex].Width = new GridLength(width);
            return width;
        }

        protected void UpdateInfo()
        {
            if (_info != null)
                this.MainGrid.Children.Remove(_info);

            if (_model.Data != null)
            {
                _info = (Control)Activator.CreateInstance(_model.Data.InfoDisplayType);
                _info.DataContext = _model.Info;
                ((BetNodeInfoView)_info).ViewChanged += OnViewChanged;

                this.MainGrid.Children.Add(_info);
                _info.Padding = new Thickness(7, 5, 0, 0);
                Grid.SetColumn(_info, 1);
                Grid.SetRow(_info, 1);

                _model.Info.Manual = _model.Data is ManualResultsModel;

                _model.Data.SetAppearance(this);
            }
        }

        protected void EditNode(object sender, RoutedEventArgs e)
        {
            this._model.OnEditNode();
        }

        protected void DeleteNode(object sender, RoutedEventArgs e)
        {
            this._model.OnDeleteNode();
        }

        protected void AddNode(object sender, RoutedEventArgs e)
        {
            this._model.OnAddNode();
        }

        protected void ShowWizard(object sender, RoutedEventArgs e)
        {
            this._model.OnShowWizard();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            _model.Tree.OnNodeChanged();
        }

        public event EmptyEventHandler OnLayoutUpdated;

        public UIElement GetLineStart()
        {
            return ExpandCollapseButton;
        }

        public UIElement GetLineEnd()
        {
            return RemoveButton;
        }

        protected void OnViewChanged()
        {
            this.UpdateVisual();
        }
    }
}
