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
    /// Interaction logic for PreflopRangeEditor.xaml
    /// </summary>
    public partial class PreflopRangeEditor : UserControl, BetTreeEditor
    {
        protected PreflopBetModel _model;

        public PreflopRangeEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            this._model = this.DataContext as PreflopBetModel;
            if (_model == null) return;

            RangeEditor.DataContext = _model.Range;
            RangeEditor.SetTextBoxesVisible(true);
        }
    }
}
