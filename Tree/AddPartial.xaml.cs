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
using Rzr.Core.Xml;

namespace Rzr.Core.Tree
{
    /// <summary>
    /// Interaction logic for AddPartial.xaml
    /// </summary>
    public partial class AddPartial : UserControl
    {
        protected BetTreeModel _tree;
        protected BetTreeNodeModel _model;

        public AddPartial()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = this.DataContext as BetTreeNodeModel;
            if (_model == null) return;

            MetaList.DataContext = _model.Tree.GetPartials(_model);
        }

        protected void LoadItem(object sender, RoutedEventArgs e)
        {
            TreePartialMeta meta = ((Button)sender).DataContext as TreePartialMeta;
            if (OnLoad != null) OnLoad(meta);
        }

        protected void Exit(object sender, RoutedEventArgs e)
        {
            if (OnClose != null) OnClose();
        }

        public event EmptyEventHandler OnClose;

        public event TreePartialMetaEventHandler OnLoad;
    }
}
