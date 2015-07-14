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

namespace Rzr.Core.Editors.Player
{
    /// <summary>
    /// Interaction logic for PlayerPanel.xaml
    /// </summary>
    public partial class PlayerPanel : UserControl
    {
        public PlayerModel Model { get; private set; }

        public PlayerPanel()
        {
            InitializeComponent();
            Model = DataContext as PlayerModel;
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = DataContext as PlayerModel;
            if (Model == null) return;
        }

        private void SetActivePlayer(object sender, RoutedEventArgs e)
        {
            if (PanelClicked != null) PanelClicked(this, e);
        }

        protected void OnMouseEnter(object sender, RoutedEventArgs e)
        {
            HighlightBorder.Visibility = Visibility.Visible;
        }

        protected void OnMouseLeave(object sender, RoutedEventArgs e)
        {
            HighlightBorder.Visibility = Visibility.Hidden;
        }

        public event RoutedEventHandler PanelClicked;
    }
}