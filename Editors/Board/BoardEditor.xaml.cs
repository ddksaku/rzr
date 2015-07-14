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

namespace Rzr.Core.Editors.Board
{
    /// <summary>
    /// Interaction logic for BoardEditor.xaml
    /// </summary>
    public partial class BoardEditor : UserControl
    {
        protected BoardModel _model;

        protected int _activeCard;

        protected CardEditor[] BoardCards;

        public BoardEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            Initialise();
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as BoardModel;
            Initialise();
        }

        protected void Initialise()
        {
            CardSelection.GridWidth = 30;
            CardSelection.GridHeight = 48;
            CardSelection.HorizontalSpacing = 2;
            CardSelection.VerticalSpacing = 2;

            if (_model == null) return;

            CardSelection.DataContext = _model;
            CardSelection.InitialiseVisual();

            BoardCards = new CardEditor[] { Card1, Card2, Card3, Card4, Card5 };
            for (int i = 0; i < 5; i++)
            {
                BoardCards[i].DataContext = _model.BoardCards[i];
                BoardCards[i].CardClicked += UnselectCard;
            }            
        }

        protected void UnselectCard(CardModel model)
        {
            _model.UnselectCard(model);
            BoardButton.Visibility = Visibility.Visible;
            CalculateButton.Visibility = Visibility.Hidden;
            if (CollapseExpand != null) CollapseExpand(true);
        }

        protected void FinishedSelecting(object sender, RoutedEventArgs e)
        {
            BoardButton.Visibility = Visibility.Hidden;
            CalculateButton.Visibility = Visibility.Visible;
            if (CollapseExpand != null) CollapseExpand(false);
        }

        protected void OnCalculate(object sender, RoutedEventArgs e)
        {
            if (Calculate != null) Calculate();
        }

        public event BoolEventHandler CollapseExpand;

        public event EmptyEventHandler Calculate;
    }
}
