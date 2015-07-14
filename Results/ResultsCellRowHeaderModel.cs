using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Calculator;
using System.Windows;

namespace Rzr.Core.Results
{
    public class ResultsCellRowHeaderModel : DependencyObject, ResultsCellModel
    {
        public static readonly DependencyProperty ShowWinLossProperty =
            DependencyProperty.Register("ShowWinLoss", typeof(bool), typeof(ResultsCellRowHeaderModel), new PropertyMetadata(false, OnShowWinLossChanged));

        public bool ShowWinLoss
        {
            get { return (bool)this.GetValue(ShowWinLossProperty); }
            set { this.SetValue(ShowWinLossProperty, value); }
        }

        protected static void OnShowWinLossChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ResultsCellRowHeaderModel model = sender as ResultsCellRowHeaderModel;
            if (model.ShowWinLossChanged != null) model.ShowWinLossChanged();
        }

        public double Height { get; private set; }

        public double Width { get; private set; }

        public ConditionContainer[] RowConditions { get; private set; }

        public void OnSelected()
        {
            
        }

        public void Refresh()
        {

        }

        public event EmptyEventHandler ShowWinLossChanged;
    }
}
