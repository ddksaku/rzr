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
    /// Interaction logic for RootNodeInfo.xaml
    /// </summary>
    public partial class RootNodeInfo : UserControl, BetNodeInfoView
    {
        public BetTreeNodeInfoModel Model { get; protected set; }

        public RootNodeInfo()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        public void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = this.DataContext as BetTreeNodeInfoModel;
            if (Model == null) return;

            Model.ResultsChanged += UpdateResults;
        }

        protected void Calculate(object sender, RoutedEventArgs e)
        {
            Model.Parent.Tree.OnCalculate();
        }

        public void UpdateResults()
        {
            float resultsRowHeight = 60;
            ResultsRow.Height = new GridLength(resultsRowHeight);
            this.Height = resultsRowHeight + 48;
            this.Width = 120;
            if (ViewChanged != null) ViewChanged();
        }

        public event EmptyEventHandler ViewChanged;
    }
}
