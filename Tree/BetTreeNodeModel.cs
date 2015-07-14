using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using Rzr.Core.Tree.DataModels;
using Rzr.Core.Tree.Xml;
using System.Windows.Media;
using Rzr.Core.Xml;

namespace Rzr.Core.Tree
{
    [Editor(typeof(DefaultBetTreeNodeEditor))]
    public class BetTreeNodeModel : DependencyObject
    {
        #region property declarations

        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register(
            "Children", typeof(ObservableCollection<BetTreeNodeModel>), typeof(BetTreeNodeModel), new PropertyMetadata(null, OnChanged));

        public static readonly DependencyProperty AllowedNodeTypesProperty = DependencyProperty.Register(
            "AllowedNodeTypes", typeof(ObservableCollection<BetTreeDataNodeMeta>), typeof(BetTreeNodeModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty TreeProperty = DependencyProperty.Register(
            "Tree", typeof(BetTreeModel), typeof(BetTreeNodeModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty MetaProperty = DependencyProperty.Register(
            "Meta", typeof(string), typeof(BetTreeNodeModel), new PropertyMetadata(null, OnMetaChanged));

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data", typeof(BetTreeNodeDataModel), typeof(BetTreeNodeModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty InfoProperty = DependencyProperty.Register(
            "Info", typeof(BetTreeNodeInfoModel), typeof(BetTreeNodeModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(BetTreeNodeModel), new PropertyMetadata(false, null));

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded", typeof(bool), typeof(BetTreeNodeModel), new PropertyMetadata(true, OnExpandCollapse));

        public static readonly DependencyProperty CanRemoveProperty = DependencyProperty.Register(
            "CanRemove", typeof(Visibility), typeof(BetTreeNodeModel), new PropertyMetadata(Visibility.Visible, null));

        public static readonly DependencyProperty CanAddProperty = DependencyProperty.Register(
            "CanAdd", typeof(Visibility), typeof(BetTreeNodeModel), new PropertyMetadata(Visibility.Visible, null));

        public static readonly DependencyProperty CanEditProperty = DependencyProperty.Register(
            "CanEdit", typeof(Visibility), typeof(BetTreeNodeModel), new PropertyMetadata(Visibility.Visible, null));

        public static readonly DependencyProperty CanCollapseProperty = DependencyProperty.Register(
            "CanCollapse", typeof(Visibility), typeof(BetTreeNodeModel), new PropertyMetadata(Visibility.Hidden, null));

        public static readonly DependencyProperty HasWizardsProperty = DependencyProperty.Register(
            "HasWizards", typeof(Visibility), typeof(BetTreeNodeModel), new PropertyMetadata(Visibility.Visible, null));

        public static readonly DependencyProperty IsDefaultProperty = DependencyProperty.Register(
            "IsDefault", typeof(bool), typeof(BetTreeNodeModel), new PropertyMetadata(true, OnMetaChanged));

        public static readonly DependencyProperty ShowResultProperty = DependencyProperty.Register(
            "ShowResult", typeof(bool), typeof(BetTreeNodeModel), new PropertyMetadata(false, null));

        public static readonly DependencyProperty WarningProperty = DependencyProperty.Register(
            "Warning", typeof(string), typeof(BetTreeNodeModel), new PropertyMetadata(null, OnChanged));

        public static readonly DependencyProperty WarningVisibilityProperty = DependencyProperty.Register(
            "WarningVisibility", typeof(Visibility), typeof(BetTreeNodeModel), new PropertyMetadata(Visibility.Collapsed, OnChanged));

        #endregion  

        #region property definition events

        protected static void OnExpandCollapse(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetTreeNodeModel model = sender as BetTreeNodeModel;
            if (model != null) model.OnExpandCollapse();
        }

        protected static void OnChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetTreeNodeModel model = sender as BetTreeNodeModel;
            if (model != null) model.OnChanged();
        }

        protected static void OnMetaChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetTreeNodeModel model = sender as BetTreeNodeModel;
            if (model != null) model.UpdateMeta();
        }

        protected static void OnDataChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetTreeNodeModel model = sender as BetTreeNodeModel;
            if (model != null) model.UpdateSnapshot();
        }

        #endregion

        #region dependency properties

        public ObservableCollection<BetTreeNodeModel> Children
        {
            get { return (ObservableCollection<BetTreeNodeModel>)this.GetValue(ChildrenProperty); }
            set { this.SetValue(ChildrenProperty, value); }
        }

        public ObservableCollection<BetTreeDataNodeMeta> AllowedNodeTypes
        {
            get { return (ObservableCollection<BetTreeDataNodeMeta>)this.GetValue(AllowedNodeTypesProperty); }
            set { this.SetValue(AllowedNodeTypesProperty, value); }
        }

        public BetTreeModel Tree
        {
            get { return (BetTreeModel)this.GetValue(TreeProperty); }
            set { this.SetValue(TreeProperty, value); }
        }

        public string Meta
        {
            get { return (string)this.GetValue(MetaProperty); }
            set { this.SetValue(MetaProperty, value); }
        }

        public BetTreeNodeDataModel Data
        {
            get { return (BetTreeNodeDataModel)this.GetValue(DataProperty); }
            set { this.SetValue(DataProperty, value); }
        }

        public BetTreeNodeInfoModel Info
        {
            get { return (BetTreeNodeInfoModel)this.GetValue(InfoProperty); }
            set { this.SetValue(InfoProperty, value); }
        }

        public bool IsDefault
        {
            get { return (bool)this.GetValue(IsDefaultProperty); }
            set { this.SetValue(IsDefaultProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }

        public bool IsExpanded
        {
            get { return (bool)this.GetValue(IsExpandedProperty); }
            set { this.SetValue(IsExpandedProperty, value); }
        }

        public bool ShowResult
        {
            get { return (bool)this.GetValue(ShowResultProperty); }
            set { this.SetValue(ShowResultProperty, value); }
        }

        public Visibility CanRemove
        {
            get { return (Visibility)this.GetValue(CanRemoveProperty); }
            set { this.SetValue(CanRemoveProperty, value); }
        }

        public Visibility CanAdd
        {
            get { return (Visibility)this.GetValue(CanAddProperty); }
            set { this.SetValue(CanAddProperty, value); }
        }

        public Visibility CanEdit
        {
            get { return (Visibility)this.GetValue(CanEditProperty); }
            set { this.SetValue(CanEditProperty, value); }
        }

        public Visibility CanCollapse
        {
            get { return (Visibility)this.GetValue(CanCollapseProperty); }
            set { this.SetValue(CanCollapseProperty, value); }
        }

        public Visibility HasWizards
        {
            get { return (Visibility)this.GetValue(HasWizardsProperty); }
            set { this.SetValue(HasWizardsProperty, value); }
        }

        public string Warning
        {
            get { return (string)this.GetValue(WarningProperty); }
            set { this.SetValue(WarningProperty, value); }
        }

        public Visibility WarningVisibility
        {
            get { return (Visibility)this.GetValue(WarningVisibilityProperty); }
            set { this.SetValue(WarningVisibilityProperty, value); }
        }

        #endregion

        #region other properties

        public bool IsDynamic { get; private set; }

        public BetTreeNode Interface { get; set; }

        public BetTreeNodeModel Parent { get; private set; }

        public BetTreeNodeDisplay DisplayModel { get; private set; }

        public BetTreeNodeStyle StyleModel { get; private set; }

        public BetPolicy Policy { get; private set; }

        public HandSnapshotModel Snapshot { get; private set; }

        public BetTreeNodeResults Result { get; private set; }

        protected HandSnapshotModel _parentSnapshot { get; private set; }

        #endregion

        #region construction

        public BetTreeNodeModel(BetTreeModel tree, BetTreeNodeDisplay display, BetTreeNodeStyle style, HandSnapshotModel snapshot) : this(tree, display, style, snapshot, null) { }

        public BetTreeNodeModel(BetTreeModel tree, BetTreeNodeDisplay display, BetTreeNodeStyle style, HandSnapshotModel snapshot, BetTreeNodeModel parent) : this(tree, display, style, snapshot, parent, false) { }

        public BetTreeNodeModel(BetTreeModel tree, BetTreeNodeDisplay display, BetTreeNodeStyle style, HandSnapshotModel snapshot, BetTreeNodeModel parent, bool isDynamic)
        {
            this.Tree = tree;
            this.DisplayModel = display;
            this.StyleModel = style;
            this._parentSnapshot = snapshot;
            this.Snapshot = snapshot;
            this.Parent = parent;
            this.IsDynamic = isDynamic;
            this.Result = new BetTreeNodeResults(snapshot.Stacks.Length);            

            this.Children = new ObservableCollection<BetTreeNodeModel>();
            this.CanRemove = IsFixed() ? Visibility.Collapsed : Visibility.Visible;

            this.Info = new BetTreeNodeInfoModel(this, this.Data is ManualResultsModel);
        }

        private bool IsFixed()
        {
            return (Data is ResultsModel || Data is ShowdownModel || Data is RootNodeDataModel);
        }

        #endregion

        #region event handlers

        private void UpdateSnapshot()
        {
            if (this.Parent != null)
                this.Parent.UpdatePolicy();
            
            if (Data != null && Data.BetModel != null)
                this.Snapshot = BetPolicyService.GetSnapshot(_parentSnapshot, Data.BetModel.BetType, Data.BetModel.BetAmount);

            UpdatePolicy();

            bool isEndPoint = Data is ResultsModel || Data is ShowdownModel;
            bool isStartPoint = Data is RootNodeDataModel;
            this.CanRemove = isEndPoint || IsFixed() ? Visibility.Collapsed : Visibility.Visible;
            this.CanEdit = isEndPoint | isStartPoint ? Visibility.Collapsed : Visibility.Visible;

            if (this.Info != null)
                this.Info.UpdateDisplay();

            this.HasWizards = this.Tree.HasPartials(this) ? Visibility.Visible : Visibility.Hidden;
        }

        private void UpdatePolicy()
        {
            this.Policy = new BetPolicy(this.Snapshot);
            this.CanAdd = (Policy.AllowedChildActions.Count > 0 && this.Meta != BetTreeNodeService.RESULT_NODE_META) ? Visibility.Visible : Visibility.Collapsed;
            if (PolicyChanged != null) PolicyChanged(this.Policy);            
        }

        private void UpdateMeta()
        {
            if (Data != null)
            {
                this.Data.BetModelEdited -= UpdateSnapshot;
            }

            if (Meta != null && AllowedNodeTypes != null)
            {
                BetTreeDataNodeMeta meta = AllowedNodeTypes.Where(x => x.Name == Meta).FirstOrDefault();
                if (meta != null)
                {
                    this.Data = meta.Model;
                    UpdateSnapshot();
                }
            }

            if (Data != null && Parent != null)
            {
                this.Data.ApplyPolicy(Parent.Policy);
                this.Data.BetModelEdited += UpdateSnapshot;                
            }

            this.CanEdit = ((Data is ResultsModel && !Info.Manual) || Data is ShowdownModel || Data is RootNodeDataModel) ? Visibility.Collapsed : Visibility.Visible;
            this.CanRemove = (Data is ResultsModel || Data is ShowdownModel || Data is RootNodeDataModel) ? Visibility.Collapsed : Visibility.Visible;

            if (this.MetaUpdated != null) this.MetaUpdated();

            if (this.Info != null)
                this.Info.Manual = (Data is ManualResultsModel);
        }

        public event EmptyEventHandler MetaUpdated;

        public void OnAddNode()
        {
            if (AddNode != null) AddNode(this);
        }

        public void OnEditNode()
        {
            if (EditNode != null) EditNode(this);
        }

        public void OnDeleteNode()
        {
            if (DeleteNode != null) DeleteNode(this);
        }

        public void OnShowWizard()
        {
            if (ShowWizard != null) ShowWizard(this);
        }

        public void OnChanged()
        {            
            if (Changed != null) Changed();
        }

        public object GetEditContext()
        {
            return Data.GetEditContext();
        }

        protected void OnExpandCollapse()
        {
            if (ExpandCollapse != null) ExpandCollapse(this);
        }

        #endregion

        #region events


        public event AddNodeHandler AddNode;

        public event BetNodeHandler EditNode;

        public event BetNodeHandler DeleteNode;

        public event BetNodeHandler ShowWizard;

        public event BetPolicyEvent PolicyChanged;

        public event EmptyEventHandler Changed;        

        public event BetNodeHandler ExpandCollapse;

        #endregion

        public void Save(object dataObject)
        {
            if (dataObject != null)
                Data.Save(dataObject);
            if (Data.BetModel != null)
                Data.BetModel.Save();

            UpdateSnapshot();
        }

        public void RefreshNode()
        {
            if (Parent != null)
                this.Data.ApplyPolicy(Parent.Policy);
            UpdateSnapshot();

            foreach (BetTreeNodeModel child in this.Children)
                child.RefreshNode();
        }

        public void ResetResults()
        {
            Result.Count = 0;
            Result.WinAmounts = new float[6];
            foreach (BetTreeNodeModel child in this.Children)
                child.ResetResults();
        }

        public void CompileResults(float[] winAmounts)
        {
            foreach (BetTreeNodeModel child in this.Children)
                child.CompileResults(Result.WinAmounts);

            for (int i = 0; i < winAmounts.Length; i++)
                winAmounts[i] += Result.WinAmounts[i];
        }

        public void UpdateResult()
        {
            this.Info.UpdateResult(Result, Parent == null ? null : Parent.Result);

            foreach (BetTreeNodeModel child in Children)
                child.UpdateResult();
        }

        public BetTreeNodeXml SaveToXml()
        {
            BetTreeNodeXml ret = new BetTreeNodeXml();
            ret.IsDefault = this.IsDefault;
            ret.MetaXml = this.Data.SaveToXml();
            ret.InfoXml = this.Info.SaveToXml();
            foreach (BetTreeNodeModel child in Children)
                ret.Children.Add(child.SaveToXml());
            return ret;
        }

        public void LoadFromXml(BetTreeNodeXml xml, bool relative)
        {
            if (relative)
            {
                float pot = this.Snapshot.Bets.Sum();
                float amount = xml.MetaXml.BetXml.BetAmount * pot;
                if (this.Snapshot.CurrentPlayer != null)
                {
                    float remaining = this.Snapshot.Stacks[(int)this.Snapshot.CurrentPlayer] - this.Snapshot.Bets[(int)this.Snapshot.CurrentPlayer];
                    if (amount >= remaining)
                    {
                        amount = remaining;
                    }
                }
                xml.MetaXml.BetXml.BetAmount = amount;
            }

            this.Meta = xml.MetaXml.Name;
            this.IsDefault = xml.IsDefault;
            this.Data.LoadFromXml(xml.MetaXml);
            this.Info.LoadFromXml(xml.InfoXml);
            this.Save(null);
        }

        public bool CheckWarnings()
        {
            if (this.Meta == BetTreeNodeService.RESULT_NODE_META)
            {
                SetWarnings(null, true);
                return false;
            }

            //-------------------------------------------------------------------------------------
            // Check that this is reachable
            //-------------------------------------------------------------------------------------
            if (this.Parent != null)
            {
                foreach (BetTreeNodeModel child in this.Parent.Children)
                {
                    if (child == this) break;
                    if (child.IsDefault)
                    {
                        SetWarnings("Unreachable node", false);
                        return true;
                    }
                }
            }

            //-------------------------------------------------------------------------------------
            // No default action check
            //-------------------------------------------------------------------------------------
            if (!this.Snapshot.IsHandEnd)
            {
                if (this.Children.FirstOrDefault(x => x.IsDefault) == null)
                {
                    SetWarnings("Requires default child", false);
                    return true;
                }
            }

            //-------------------------------------------------------------------------------------
            // Check that this has an endpoint
            //-------------------------------------------------------------------------------------
            if (this.Children.Count == 0 && !this.Snapshot.IsHandEnd)
            {
                SetWarnings("Endpoint required", true);
                return true;
            }

            bool hasWarnings = false;
            foreach (BetTreeNodeModel child in this.Children)
                hasWarnings |= child.CheckWarnings();
                    
            if (!hasWarnings)
                SetWarnings(null, true);

            return hasWarnings;
        }

        public void SetWarnings(string warning, bool recursive)
        {
            Warning = warning;
            WarningVisibility = warning == null ? Visibility.Collapsed : Visibility.Visible;

            if (recursive && this.Parent != null)
                this.Parent.SetWarnings(warning, recursive);
        }

        public void UpdateHero(int hero)
        {
            this.Info.DisplayResultsIndex = hero;
            foreach (BetTreeNodeModel child in Children)
                child.UpdateHero(hero);
        }

        public void AddChild(BetTreeNodeModel model)
        {
            this.IsExpanded = true;
            Children.Add(model);
            CanCollapse = this.Children.Count > 0 ? Visibility.Visible : Visibility.Hidden;            
        }

        public void RemoveChild(BetTreeNodeModel model)
        {
            this.IsExpanded = true;
            Children.Remove(model);
            CanCollapse = this.Children.Count > 0 ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
