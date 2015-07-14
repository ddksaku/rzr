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
    /// Interaction logic for BetNodeEditorHeader.xaml
    /// </summary>
    public partial class BetNodeEditorHeader : UserControl
    {
        protected BetTreeNodeModel _model;

        public BetNodeEditorHeader()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            this._model = this.DataContext as BetTreeNodeModel;
        }

        protected void OnDefaultChanged(object sender, RoutedEventArgs e)
        {
            if (DefaultChanged != null) DefaultChanged();
        }

        public event EmptyEventHandler DefaultChanged;
    }
}
