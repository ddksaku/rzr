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

namespace Rzr.Core.Editors.LevelGrid
{
    /// <summary>
    /// Interaction logic for LevelGridButton.xaml
    /// </summary>
    public partial class LevelGridButton : UserControl
    {
        public LevelGridItem _model;

        public LevelGridButton()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            Initialise();
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as LevelGridItem;
            Initialise();
        }

        protected void Initialise()
        {
            if (_model == null) return;
        }

        protected void OnDelete(object sender, RoutedEventArgs e)
        {
            if (Delete != null) Delete(_model);
        }

        protected void OnEdit(object sender, RoutedEventArgs e)
        {
            if (Edit != null) Edit(_model);
        }

        public event LevelGridItemHandler Delete;
        public event LevelGridItemHandler Edit;
    }
}
