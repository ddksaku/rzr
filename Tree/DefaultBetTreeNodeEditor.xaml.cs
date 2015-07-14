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
    /// Interaction logic for DefaultBetTreeNodeEditor.xaml
    /// </summary>
    public partial class DefaultBetTreeNodeEditor : UserControl, BetTreeEditor
    {
        public DefaultBetTreeNodeEditor()
        {
            InitializeComponent();
        }

        public event BetNodeHandler EditNode;
    }
}
