using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Xml;
using Rzr.Core.Tree;
using Rzr.Core.Tree.Xml;
using Rzr.Core.Editors.Partial;
using System.Windows;

namespace Rzr.Core.Partials
{
    public class PartialGenerator : DependencyObject
    {
        public static readonly DependencyProperty VariablesProperty = DependencyProperty.Register(
            "Variables", typeof(PartialVariableListModel), typeof(PartialGenerator), new PropertyMetadata(null, null));

        public TreePartialMeta Meta { get; set; }


        public PartialWizard Wizard { get; set; }

        public BetTreeModel Tree { get; set; }

        public BetTreeNodeModel Node { get; set; }

        public BetTreeNodeService Service { get; set; }

        public PartialVariableListModel Variables
        {
            get { return (PartialVariableListModel)this.GetValue(VariablesProperty); }
            set { this.SetValue(VariablesProperty, value); }
        }

        public PartialGenerator(TreePartialMeta meta, BetTreeModel tree, BetTreeNodeModel node, BetTreeNodeService service)
        {
            this.Meta = meta;
            this.Wizard = tree.GeneratePartialWizard(meta);
            this.Tree = tree;
            this.Node = node;
            this.Service = service;

            SetContainer();
        }

        private void SetContainer()
        {
            if (Meta.Type == TreePartialMetaType.Xml)
            {
                Variables = Tree.LoadVariables(Node, Meta);
            }
            else
            {
                Variables = Wizard.LoadVariables(Node);
            }

            Tree.LoadPlayerValues(Node, Variables);
        }

        public void GetPartialTree(BetTreeNodeModel parent)
        {
            if (Meta.Type == TreePartialMetaType.Xml)
            {
                TreePartialContainer container = Tree.LoadPartial(Node, Meta);

                if (container.Root == null) return;

                List<BetTreeNodeModel> ret = new List<BetTreeNodeModel>();

                foreach (BetTreeNodeXml child in container.Root.Children)
                    ret.Add(LoadNode(parent, child));
            }
            else
            {
                Wizard.GenerateTree(parent);
            }
        }

        private BetTreeNodeModel LoadNode(BetTreeNodeModel parent, BetTreeNodeXml node)
        {
            BetTreeNodeModel ret = Service.AddChild(Tree, parent);
            ret.LoadFromXml(node, true);

            if (!ret.Snapshot.IsHandEnd)
                foreach (BetTreeNodeXml child in node.Children)
                    LoadNode(ret, child);
            return ret;
        }
    }
}
