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

namespace Rzr.Core.Tree
{
    /// <summary>
    /// Interaction logic for TreeGridItem.xaml
    /// </summary>
    public partial class TreeGridItem : UserControl
    {
        public BetTreeNodeModel Model;
        protected List<TreeGridItem> _children;

        public TreeGridItem()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            Model = DataContext as BetTreeNodeModel;

            _children = new List<TreeGridItem>();
            NodeDisplay.OnLayoutUpdated += OnFixLayout;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = DataContext as BetTreeNodeModel;
            if (Model == null) return;
        }



        public void FixLayout(double childGridWidth, double childGridHeight)
        {
            if (this.Visibility == Visibility.Visible)
            {
                this.MainGrid.ColumnDefinitions[0].Width = new GridLength(NodeDisplay.Width);
                this.MainGrid.RowDefinitions[0].Height = new GridLength(NodeDisplay.Height);
                this.MainGrid.ColumnDefinitions[1].Width = new GridLength(childGridWidth);
                this.Width = NodeDisplay.Width + childGridWidth;
                this.Height = Math.Max(childGridHeight, NodeDisplay.Height);
                this.MainGrid.RowDefinitions[1].Height = new GridLength(this.Height - NodeDisplay.Height);
                this.Children.Width = childGridWidth;
                this.Children.Height = childGridHeight;
            }
            else
            {
                this.Width = 0;
                this.Height = 0;
            }

            if (this.ItemLayoutUpdated != null) this.ItemLayoutUpdated(this);
        }

        protected void OnFixLayout()
        {
            if (this.DoFixLayout != null) DoFixLayout(this, true);
        }

        public UIElement GetLineStart()
        {
            return NodeDisplay.GetLineStart();
        }

        public UIElement GetLineEnd()
        {
            return NodeDisplay.GetLineEnd();
        }

        public event TreeGridNodeItemEventHandler DoFixLayout;

        public event TreeGridItemEventHandler ItemLayoutUpdated;
    }

    public delegate void TreeGridItemEventHandler(TreeGridItem item);

    public delegate void TreeGridNodeItemEventHandler(TreeGridItem item, bool expanded);
}
