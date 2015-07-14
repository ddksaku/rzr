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
using Rzr.Core.Calculator;
using Rzr.Core.Data;

namespace Rzr.Core.Editors.HandRange
{
    /// <summary>
    /// Interaction logic for HandRangeEditor.xaml
    /// </summary>
    public partial class HandRangeEditor : UserControl
    {
        protected HandRangeModel _model;

        public HandRangeEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            Initialise();
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as HandRangeModel;
            Initialise();
        }

        protected void Initialise()
        {
            if (_model == null) return;

            _model.RangeChanged += OnRangeChanged;
            RangeSelection.RangeChanged += OnRangeCustomised;
            RangeSelection.BoxWidth = 28;
            RangeSelection.BoxHeight = 20;
            RangeSelection.Initialise(_model.RangeItems.ToList<RangeDisplayItem>(), 13, 13);
        }

        protected void OnRangeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RangeSelection.ReColourBoxes();
        }

        protected void OnRangeCustomised()
        {
            for (int i = 0; i < _model.RangeItems.Count; i++)
            {
                _model.Range.SetProbability(i, (int)_model.RangeItems[i].Weight);
            }
        }

        public void SetTextBoxesVisible(bool visible)
        {
            TextBoxColumn1.Width = visible ? new GridLength(50) : new GridLength(0);
            TextBoxColumn2.Width = visible ? new GridLength(50) : new GridLength(0);
        }

        protected void OpenHandRangeDefinitionEditor(object sender, RoutedEventArgs e)
        {
            WindowManager.OpenPopup(WindowManager.HAND_RANGE_DEFINITION_EDITOR);
        }
    }
}
