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
using Rzr.Core.Data;

namespace Rzr.Core.Editors.HandValue
{
    /// <summary>
    /// Interaction logic for HandValueEditor.xaml
    /// </summary>
    public partial class HandValueEditor : UserControl
    {
        protected HandValueRangeModel _model;

        public HandValueEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            Initialise();
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as HandValueRangeModel;
            Initialise();
        }

        protected void Initialise()
        {
            if (_model == null) return;

            _model.RangeChanged += OnRangeChanged;
            _model.ConditionsChanged += OnConditionsChanged;
            RangeSelection.KeyHeight = 20;
            RangeSelection.RangeChanged += OnRangeCustomised;
            RangeSelection.BoxWidth = 100;
            RangeSelection.BoxHeight = 24;
            RangeSelection.KeyOffset = -80;
            RangeSelection.Initialise(_model.RangeItems.ToList<RangeDisplayItem>(), (_model.RangeItems.Count / HandValueRangeModel.COLUMNS) + 1, HandValueRangeModel.COLUMNS);
        }

        protected void OnConditionsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RangeSelection.Initialise(_model.RangeItems.ToList<RangeDisplayItem>(), _model.RangeItems.Count, 1);
        }

        protected void OnRangeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RangeSelection.ReColourBoxes();
        }

        protected void OnRangeCustomised()
        {
            for (int i = 0; i < _model.RangeItems.Count; i++)
            {
                _model.Range.Probability[i] = (int)_model.RangeItems[i].Weight;
            }
        }
    }
}
