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
using Rzr.Core.Tree.DataModels;

namespace Rzr.Core.Tree.DataEditors
{
    /// <summary>
    /// Interaction logic for PostflopRangeEditor.xaml
    /// </summary>
    public partial class PostflopRangeEditor : UserControl, BetTreeEditor
    {
        protected PostFlopBetModel _model;

        public PostflopRangeEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            this._model = this.DataContext as PostFlopBetModel;
            if (_model == null) return;

            RangeEditor.DataContext = _model.Range;
        }

        protected void ShowSettings(object sender, RoutedEventArgs e)
        {
            WindowManager.OpenPopup(WindowManager.CONDITIONS_EDITOR);
        }
    }
}
