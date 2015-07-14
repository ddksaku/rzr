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
    /// Interaction logic for BetTreeNodeInfo.xaml
    /// </summary>
    public partial class BetTreeNodeInfo : UserControl, BetNodeInfoView
    {
        public BetTreeNodeInfoModel Model { get; protected set; }

        public BetTreeNodeInfo()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            
        }

        public void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.Model = DataContext as BetTreeNodeInfoModel;            
        }

        public event EmptyEventHandler ViewChanged;
    }
}
