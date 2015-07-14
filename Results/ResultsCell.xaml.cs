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

namespace Rzr.Core.Results
{
    /// <summary>
    /// Interaction logic for ResultsCell.xaml
    /// </summary>
    public partial class ResultsCell : UserControl
    {
        protected ResultsCellModel _model;

        public ResultsCell()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            Initialise();            
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as ResultsCellModel;
            Initialise();
        }

        private void Initialise()
        {
            if (_model == null) return;

            _model.ShowWinLossChanged += Refresh;
            this.Height = _model.Height;
            this.Width = _model.Width;
        }

        protected void OnSelected(object sender, RoutedEventArgs e)
        {
            _model.OnSelected();
        }

        protected void Refresh()
        {
            Visibility visibility = _model.ShowWinLoss ? Visibility.Visible : Visibility.Hidden;
            double columnWidth = _model.ShowWinLoss ? 40 : 0;

            WinText.Visibility = visibility;
            LossText.Visibility = visibility;
            WinLossColumn.Width = new GridLength(columnWidth);
            DetailColumn.Width = new GridLength(90 - columnWidth);
        }
    }
}
