using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using Rzr.Core.Tree.Xml;
using Rzr.Core.Editors.Table;
using Rzr.Core.Xml;
using Rzr.Core.Partials;
using Rzr.Core.Editors.Partial;
using Rzr.Core.Editors.Player;
using Rzr.Core.Editors.Variables;

namespace Rzr.Core.Tree
{
    /// <summary>
    /// Model for the BetTree user control. Most of the functions are delegated to service classes, to allow
    /// for interchangeable logic
    /// </summary>
    public class BetTreeModel : DependencyObject
    {
        #region dependency property definitions

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(BetTreeModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty HeroProperty = DependencyProperty.Register(
            "Hero", typeof(int), typeof(BetTreeModel), new PropertyMetadata(0, HeroChanged));

        public static readonly DependencyProperty TableProperty = DependencyProperty.Register(
            "Table", typeof(TableModel), typeof(BetTreeModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty HandRootProperty = DependencyProperty.Register(
            "HandRoot", typeof(HandSnapshotModel), typeof(BetTreeModel), new PropertyMetadata(null, OnHandRootChanged));

        public static readonly DependencyProperty RootNodeProperty = DependencyProperty.Register(
            "RootNode", typeof(BetTreeNodeModel), typeof(BetTreeModel), new PropertyMetadata(null, null));

        #endregion

        #region dependency events

        protected static void OnHandRootChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetTreeModel model = sender as BetTreeModel;
            model.InitialiseTree();
        }

        #endregion

        #region dependency properties

        public string Name
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        public int Hero
        {
            get { return (int)this.GetValue(HeroProperty); }
            set { this.SetValue(HeroProperty, value); }
        }

        public TableModel Table
        {
            get { return (TableModel)this.GetValue(TableProperty); }
            set { this.SetValue(TableProperty, value); }
        }

        public HandSnapshotModel HandRoot
        {
            get { return (HandSnapshotModel)this.GetValue(HandRootProperty); }
            set { this.SetValue(HandRootProperty, value); }
        }

        public BetTreeNodeModel RootNode
        {
            get { return (BetTreeNodeModel)this.GetValue(RootNodeProperty); }
            set { this.SetValue(RootNodeProperty, value); }
        }

        #endregion

        #region properties

        public BetTreeNodeService Service { get; private set; }

        #endregion

        public static void HeroChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetTreeModel model = sender as BetTreeModel;
            if (model.RootNode != null)
                model.RootNode.UpdateHero(model.Hero);
        }

        #region constructor

        public BetTreeModel(string name, BetTreeNodeService nodeService, TableModel table)
        {
            Service = nodeService;
            Name = name;
            Table = table;
            SetRoot();
        }

        #endregion

        #region functions

        public void SetRoot()
        {
            ActiveStatus[] status = Table.Seats.Select(x => x.Player.Status).ToArray();
            float[] bets = Table.Seats.Select(x => x.Player.Bet).ToArray();
            float[] stacks = Table.Seats.Select(x => x.Player.Stack).ToArray();
            bool[] active = Table.Seats.Select(x => x.Player.Active).ToArray();
            HandRoot = new HandSnapshotModel(Table.StartRound, Table.Button, active, status, bets, stacks, null);
        }

        public void InitialiseTree()
        {
            RootNode = Service.GetRootNode(this, HandRoot);
            if (TreeInitialised != null) TreeInitialised();
        }

        public void RefreshTree()
        {
            if (RootNode == null) return;

            RootNode.RefreshNode();
            RootNode.CheckWarnings();
        }

        public BetTreeNodeModel OnAddNode(BetTreeNodeModel parent)
        {
            BetTreeNodeModel child = Service.AddChild(this, parent);

            parent.InvalidateProperty(BetTreeNodeModel.ChildrenProperty);
            if (this.NodeAdded != null) NodeAdded(child);

            return child;
        }

        public void OnEditNode(BetTreeNodeModel node)
        {
            if (this.EditNode != null) EditNode(node);
        }

        public void OnDeleteNode(BetTreeNodeModel node)
        {
            node.Parent.RemoveChild(node);
            node.Parent.InvalidateProperty(BetTreeNodeModel.ChildrenProperty);
            if (this.NodeDeleted != null) NodeDeleted(node);
        }

        public void OnShowWizard(BetTreeNodeModel model)
        {
            if (ShowWizard != null) ShowWizard(model);
        }

        public void OnCalculate()
        {
            if (Calculate != null) Calculate();
        }

        public event EmptyEventHandler Calculate;

        public event AddNodeHandler NodeAdded;

        public event BetNodeHandler EditNode;

        public event BetNodeHandler NodeDeleted;

        public event BetNodeHandler ShowWizard;

        #endregion

        #region events

        public event EmptyEventHandler TreeChanged;

        public event EmptyEventHandler TreeInitialised;

        public void OnNodeChanged()
        {
            if (TreeChanged != null) TreeChanged();
        }

        #endregion

        public BetTreeXml SaveToXml()
        {
            BetTreeXml ret = new BetTreeXml();
            ret.Node = this.RootNode.SaveToXml();
            return ret;
        }

        public void LoadFromXml(BetTreeXml xml)
        {
            if (xml.Node == null) return;

            foreach (BetTreeNodeXml child in xml.Node.Children)
                LoadNode(RootNode, child, false);

            RootNode.CheckWarnings();

            if (TreeInitialised != null) TreeInitialised();
        }

        protected BetTreeNodeModel LoadNode(BetTreeNodeModel parent, BetTreeNodeXml node, bool relative)
        {
            BetTreeNodeModel ret = Service.AddChild(this, parent);
            ret.LoadFromXml(node, relative);

            if (!ret.Snapshot.IsHandEnd) 
                foreach (BetTreeNodeXml child in node.Children)
                    LoadNode(ret, child, relative);
            return ret;
        }

        public void SavePartial(TreePartialContainer container, TreePartialMeta meta)
        {
            Service.SavePartial(container, meta);
        }

        public List<TreePartialMeta> GetPartials(BetTreeNodeModel node)
        {
            return Service.GetPartialMeta(node);
        }

        public bool HasPartials(BetTreeNodeModel node)
        {
            return GetPartials(node).Count > 0;
        }

        public TreePartialContainer LoadPartial(BetTreeNodeModel parent, TreePartialMeta meta)
        {
            return Service.GetPartialTree(meta);
        }

        public PartialWizard GeneratePartialWizard(TreePartialMeta meta)
        {
            return meta.GenerateWizard(this, Service);
        }

        public PartialVariableListModel LoadVariables(BetTreeNodeModel node, TreePartialMeta meta)
        {
            TreePartialContainer container = LoadPartial(node, meta);
            PartialVariableListModel variables = container.Variables.GetVariableListModel();
            return variables;
        }

        public void LoadPlayerValues(BetTreeNodeModel node, PartialVariableListModel variables)
        {
            List<PlayerModel> activePlayers = new List<PlayerModel>();
            int nextPlayer = (int)node.Snapshot.NextPlayer;
            for (int i = 0; i < node.Snapshot.Active.Length; i++)
            {
                int playerIndex = (i + nextPlayer) % node.Snapshot.Active.Length;
                if (node.Snapshot.Active[playerIndex])
                {
                    activePlayers.Add(Table.Seats[playerIndex].Player);
                }
            }

            foreach (PartialVariableModel model in variables.Definitions)
            {
                foreach (PartialPlayerValueModel player in model.Players)
                {
                    player.Player = activePlayers[player.Index];
                    VariableGroup group = player.Player.Container.Groups.First(x => x.Name == node.Snapshot.Round.ToString());
                    player.Value = new ValueModel();
                    player.Value.Value = model.Default;
                    player.Value.Variable = group.Variables.FirstOrDefault(x => x.Name == model.Variable);
                    if (player.Value.Variable == null)
                        player.Value.Value = model.Default;
                }
            }
        }
    }
}
