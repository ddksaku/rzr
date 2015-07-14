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

namespace Rzr.Core.Tree
{
    /// <summary>
    /// Interaction logic for BetControls.xaml
    /// </summary>
    public partial class BetControls : UserControl
    {
        protected BetTypeModel _model;

        protected int _playerIndex;

        public BetControls()
        {
            InitializeComponent();
            this.DataContextChanged += OnDataContextChanged;
        }

        public void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_model != null)
                _model.BetTypeEdited -= OnBetTypeChanged;

            _model = DataContext as BetTypeModel;
            if (_model == null) return;            

            _model.BetTypeEdited += OnBetTypeChanged;
            Initialise();
        }

        public void Initialise()
        {
            _initialising = true;
            SetAvailableBetControls();
            RecordPlayerBetAmounts();
            SetLabels();
            SetBetLevels();
            OnBetTypeChanged(this, EventArgs.Empty);
            _initialising = false;
        }

        public void SetAvailableBetControls()
        {
            _playerIndex = _model.Policy.Parent.NextPlayer == null ?
                BetPolicyService.GetRoundStart(_model.Policy.Parent.Round, _model.Policy.Parent.Button, _model.Policy.Parent.Bets.Length) :
                (int)_model.Policy.Parent.NextPlayer;


            float betMade = _model.Policy.Parent.Bets[_playerIndex];
            float requiredBet = _model.Policy.Parent.Bets.Max();
            float maxBet = _model.Policy.MaxBet;

            if (betMade < requiredBet)
            {
                if (requiredBet >= _model.Policy.MaxBet)
                {
                    FoldButton.Visibility = Visibility.Visible;
                    CheckButton.Visibility = Visibility.Hidden;
                    CallButton.Visibility = Visibility.Hidden;
                    BetButton.Visibility = Visibility.Hidden;
                    RaiseButton.Visibility = Visibility.Hidden;
                    AllinButton.Visibility = Visibility.Visible;
                }
                else if (_model.Policy.MinBet == _model.Policy.MaxBet)
                {
                    FoldButton.Visibility = Visibility.Visible;
                    CheckButton.Visibility = Visibility.Hidden;
                    CallButton.Visibility = Visibility.Visible;
                    BetButton.Visibility = Visibility.Hidden;
                    RaiseButton.Visibility = Visibility.Hidden;
                    AllinButton.Visibility = Visibility.Visible;
                }
                else
                {
                    FoldButton.Visibility = Visibility.Visible;
                    CheckButton.Visibility = Visibility.Hidden;
                    CallButton.Visibility = Visibility.Visible;
                    BetButton.Visibility = Visibility.Hidden;
                    RaiseButton.Visibility = Visibility.Visible;
                    AllinButton.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                FoldButton.Visibility = Visibility.Hidden;
                CheckButton.Visibility = Visibility.Visible;
                CallButton.Visibility = Visibility.Hidden;
                BetButton.Visibility = Visibility.Visible;
                RaiseButton.Visibility = Visibility.Hidden;
            }
        }

        public void RecordPlayerBetAmounts()
        {
            ReportGrid.Children.Clear();
            AddBorders(_model.Policy.Parent.Bets.Length);
            for (int i = 0; i < 6; i++)
            {
                if (i < _model.Policy.Parent.Bets.Length)
                {
                    AddStackDetails(i * 2 + 1, "Player " + i, _model.Policy.Parent.Bets[i]);
                }
            }
        }

        private void AddBorders(int numPlayers)
        {
            Border headerBorder = new Border()
            {
                Background = new SolidColorBrush(Color.FromRgb(150, 150, 150)),
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80))
            };
            ReportGrid.Children.Add(headerBorder);
            Grid.SetRow(headerBorder, 1);
            Grid.SetColumn(headerBorder, 1);
            Grid.SetColumnSpan(headerBorder, (numPlayers * 2) - 1);                

            for (int i = 0; i < numPlayers * 2; i += 2)
            {
                Border border = new Border()
                {
                    Background = new SolidColorBrush(Colors.Transparent),
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush(Colors.White)
                };
                ReportGrid.Children.Add(border);
                Grid.SetRow(border, 1);
                Grid.SetColumn(border, i + 1);
                Grid.SetRowSpan(border, 5);
            }

            Border highlight = new Border()
            {
                Background = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(3),
                BorderBrush = new SolidColorBrush(Colors.Red)
            };
            ReportGrid.Children.Add(highlight);
            Grid.SetRow(highlight, 1);
            Grid.SetColumn(highlight, _playerIndex * 2 + 1);
            Grid.SetRowSpan(highlight, 5);
        }

        private void AddStackDetails(int i, string playerName, float bet)
        {
            AddStackCell(playerName, 1, i, true, Colors.Black);
            AddStackCell("(" + bet + ")", 3, i, true, Colors.Blue);
            AddStackCell(Convert.ToString(bet), 5, i, true, Colors.Black);            
        }

        private void AddStackCell(string text, int row, int column, bool highlight, Color textColor)
        {
            TextBlock header = new TextBlock() 
            { 
                Text = text, 
                Foreground = new SolidColorBrush(textColor),
                VerticalAlignment = VerticalAlignment.Center, 
                HorizontalAlignment = HorizontalAlignment.Center,
                FontWeight = highlight ? FontWeights.Bold : FontWeights.Normal,
                Padding = new Thickness(2)
            };
            ReportGrid.Children.Add(header);
            Grid.SetRow(header, row);
            Grid.SetColumn(header, column);
        }

        private void SetLabels()
        {
            PotLabel.Text = "Pot: " + _model.Policy.Parent.Bets.Sum();
        }

        protected const float slidercurve = 3;

        protected bool _initialising = false;

        private void SetBetLevels()
        {
            BetSlider.Minimum = Math.Pow(_model.Policy.MinBet, 1 / slidercurve);
            float stack = _model.Policy.Parent.Stacks[_playerIndex];
            float alreadyBet = _model.Policy.Parent.Bets[_playerIndex];
            BetSlider.Maximum = Math.Pow((stack - alreadyBet), 1 / slidercurve);
            BetSlider.Value = _model.BetAmount == 0 ? _model.Policy.MinBet : _model.BetAmount;
            BetInput.Text = Convert.ToString(_model.Policy.MinBet);
        }

        protected void BetInputChanged(object sender, RoutedEventArgs e)
        {
            float value;
            try
            {
                value = Convert.ToSingle(BetInput.Text);
            }
            catch
            {
                value = _model.Policy.MinBet;
            }
            float sliderValue = (float)Math.Pow(value, 1 / slidercurve);
            if (!BetInput.IsFocused)
                BetInput.Text = Math.Round(value, 2).ToString();

            BetSlider.Value = sliderValue;
            if (_model.Policy.MinCall == 0)
                Bet(sender, e);
            else 
                Raise(sender, e);
        }

        protected void BetSliderChanged(object sender, RoutedEventArgs e)
        {
            float inputValue = (float)Math.Pow(BetSlider.Value, slidercurve);
            if (!BetInput.IsFocused)
                BetInput.Text = Math.Round(inputValue, 2).ToString();
        }

        protected void Fold(object sender, RoutedEventArgs e)
        {
            if (_initialising) return;
            _model.BetAmount = 0;
            _model.BetType = BetAction.Fold;
        }

        protected void Check(object sender, RoutedEventArgs e)
        {
            if (_initialising) return;
            _model.BetAmount = 0;
            _model.BetType = BetAction.Check;
        }

        protected void Call(object sender, RoutedEventArgs e)
        {
            if (_initialising) return;
            _model.BetAmount = _model.Policy.Required;
            _model.BetType = BetAction.Call;
        }

        protected void Bet(object sender, RoutedEventArgs e)
        {
            if (_initialising) return;
            _model.BetAmount = Convert.ToSingle(BetInput.Text);            
            _model.BetType = BetAction.Bet;
        }

        protected void Raise(object sender, RoutedEventArgs e)
        {
            if (_initialising) return;
            try
            {
                _model.BetAmount = Convert.ToSingle(BetInput.Text);
                _model.BetType = BetAction.Raise;
            }
            catch
            {
                
            }
        }

        protected void Allin(object sender, RoutedEventArgs e)
        {
            if (_initialising) return;
            _model.BetAmount = _model.Policy.MaxBet;
            _model.BetType = BetAction.AllIn;
        }

        protected void BetInputLostFocus(object sender, RoutedEventArgs e)
        {
            BetInput.Text = String.Format("{0:N2}", _model.BetAmount);
            OnBetTypeChanged(sender, e);
        }

        protected void SetBet(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            float percentage = 0f;
            if (senderButton == FiftyButton)
                percentage = 0.5f;
            else if (senderButton == SixtyFiveButton)
                percentage = 0.65f;
            else if (senderButton == EightyButton)
                percentage = 0.8f;

            float betAmount = _model.Policy.MinCall + ((_model.Policy.Parent.Bets.Sum() + _model.Policy.MinCall) * percentage);
            betAmount = Math.Max(_model.Policy.MinBet, betAmount);
            betAmount = (float)Math.Round(betAmount, 2);
            if (!BetInput.IsFocused)
                BetInput.Text = Convert.ToString(betAmount);
            _model.BetAmount = betAmount;

            if (_model.Policy.MinCall == 0)
            {
                _model.BetType = BetAction.Bet;
            }
            else
            {                
                _model.BetType = BetAction.Raise;
            }
        }

        const string SelectedBetButtonStyle = "SelectedBetButton";
        const string BetButtonStyle = "BetButton";

        protected void OnBetTypeChanged(object sender, EventArgs e)
        {
            this.CallButton.Content = "Call: " + _model.Policy.Required;
            this.BetButton.Content = "Bet:  " + BetInput.Text;
            this.RaiseButton.Content = "Raise:  " + BetInput.Text;
            FoldButton.Style = this.FindResource(_model.BetType == BetAction.Fold ? SelectedBetButtonStyle : BetButtonStyle) as Style;
            CheckButton.Style = this.FindResource(_model.BetType == BetAction.Check ? SelectedBetButtonStyle : BetButtonStyle) as Style;
            CallButton.Style = this.FindResource(_model.BetType == BetAction.Call ? SelectedBetButtonStyle : BetButtonStyle) as Style;
            BetButton.Style = this.FindResource(_model.BetType == BetAction.Bet ? SelectedBetButtonStyle : BetButtonStyle) as Style;
            RaiseButton.Style = this.FindResource(_model.BetType == BetAction.Raise ? SelectedBetButtonStyle : BetButtonStyle) as Style;
            AllinButton.Style = this.FindResource(_model.BetType == BetAction.AllIn ? SelectedBetButtonStyle : BetButtonStyle) as Style;
        }

    }
}
