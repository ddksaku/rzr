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
    /// Interaction logic for BetTreeNodeTypeSelector.xaml
    /// </summary>
    public partial class BetTreeNodeTypeSelector : UserControl
    {
        public BetTreeNodeModel Model { get; private set; }

        public BetTreeNodeTypeSelector()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            SetModel(this, new DependencyPropertyChangedEventArgs());
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = this.DataContext as BetTreeNodeModel;
        }

        protected void SetType(object sender, EventArgs e)
        {
            Button button = sender as Button;
            BetTreeDataNodeMeta meta = button.DataContext as BetTreeDataNodeMeta;
            Model.Meta = meta.Name;            
            Options.Visibility = Visibility.Hidden;

            Model.Tree.RefreshTree();
        }

        protected void SelectType(object sender, EventArgs e)
        {
            Options.Visibility = Visibility.Visible;
        }
    }
}
