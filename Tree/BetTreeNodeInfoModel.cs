using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using Rzr.Core.Tree.DataModels;
using Rzr.Core.Tree.Xml;

namespace Rzr.Core.Tree
{
    public class BetTreeNodeInfoModel : DependencyObject
    {
        public static readonly DependencyProperty DisplayResultsIndexProperty = DependencyProperty.Register(
            "DisplayResultsIndex", typeof(int), typeof(BetTreeNodeInfoModel), new PropertyMetadata(-2, UpdateDisplayDetails));

        public static readonly DependencyProperty RoundNameProperty = DependencyProperty.Register(
            "RoundName", typeof(string), typeof(BetTreeNodeInfoModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty PlayerNameProperty = DependencyProperty.Register(
            "PlayerName", typeof(string), typeof(BetTreeNodeInfoModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty HeroNameProperty = DependencyProperty.Register(
            "HeroName", typeof(string), typeof(BetTreeNodeInfoModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty PlayerActionProperty = DependencyProperty.Register(
            "PlayerAction", typeof(string), typeof(BetTreeNodeInfoModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty PathPercentageProperty = DependencyProperty.Register(
            "PathPercentage", typeof(string), typeof(BetTreeNodeInfoModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty TotalPathPercentageProperty = DependencyProperty.Register(
            "TotalPathPercentage", typeof(string), typeof(BetTreeNodeInfoModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty PotSummaryProperty = DependencyProperty.Register(
            "PotSummary", typeof(string), typeof(BetTreeNodeInfoModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty ExpectedValueProperty = DependencyProperty.Register(
            "ExpectedValue", typeof(string), typeof(BetTreeNodeInfoModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty WinPercentageProperty = DependencyProperty.Register(
            "WinPercentage", typeof(string), typeof(BetTreeNodeInfoModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty ResultsHeightProperty = DependencyProperty.Register(
            "ResultsHeight", typeof(GridLength), typeof(BetTreeNodeInfoModel), new PropertyMetadata(new GridLength(0), null));

        public static readonly DependencyProperty PlayerResultsProperty = DependencyProperty.Register(
            "PlayerResults", typeof(ObservableCollection<BetTreeNodePlayerInfoModel>), typeof(BetTreeNodeInfoModel), new PropertyMetadata(null, OnResultsChanged));

        public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register(
            "DisplayText", typeof(string), typeof(BetTreeNodeInfoModel), new PropertyMetadata(null, null));

        public int DisplayResultsIndex
        {
            get { return (int)this.GetValue(DisplayResultsIndexProperty); }
            set { this.SetValue(DisplayResultsIndexProperty, value); }
        }

        public string RoundName
        {
            get { return (string)this.GetValue(RoundNameProperty); }
            set { this.SetValue(RoundNameProperty, value); }
        }

        public string PlayerName
        {
            get { return (string)this.GetValue(PlayerNameProperty); }
            set { this.SetValue(PlayerNameProperty, value); }
        }

        public string HeroName
        {
            get { return (string)this.GetValue(HeroNameProperty); }
            set { this.SetValue(HeroNameProperty, value); }
        }

        public string PlayerAction
        {
            get { return (string)this.GetValue(PlayerActionProperty); }
            set { this.SetValue(PlayerActionProperty, value); }
        }

        public string PathPercentage
        {
            get { return (string)this.GetValue(PathPercentageProperty); }
            set { this.SetValue(PathPercentageProperty, value); }
        }

        public string TotalPathPercentage
        {
            get { return (string)this.GetValue(TotalPathPercentageProperty); }
            set { this.SetValue(TotalPathPercentageProperty, value); }
        }

        public string PotSummary
        {
            get { return (string)this.GetValue(PotSummaryProperty); }
            set { this.SetValue(PotSummaryProperty, value); }
        }

        public string ExpectedValue
        {
            get { return (string)this.GetValue(ExpectedValueProperty); }
            set { this.SetValue(ExpectedValueProperty, value); }
        }

        public string WinPercentage
        {
            get { return (string)this.GetValue(WinPercentageProperty); }
            set { this.SetValue(WinPercentageProperty, value); }
        }

        public GridLength ResultsHeight
        {
            get { return (GridLength)this.GetValue(ResultsHeightProperty); }
            set { this.SetValue(ResultsHeightProperty, value); }
        }

        public string DisplayText
        {
            get { return (string)this.GetValue(DisplayTextProperty); }
            set { this.SetValue(DisplayTextProperty, value); }
        }

        public ObservableCollection<BetTreeNodePlayerInfoModel> PlayerResults
        {
            get { return (ObservableCollection<BetTreeNodePlayerInfoModel>)this.GetValue(PlayerResultsProperty); }
            set { this.SetValue(PlayerResultsProperty, value); }
        }

        protected static void OnDisplayIndexChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetTreeNodeInfoModel model = sender as BetTreeNodeInfoModel;            
            model.UpdateDisplay();
        }

        protected static void OnResultsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetTreeNodeInfoModel model = sender as BetTreeNodeInfoModel;
            if (model.ResultsChanged != null) model.ResultsChanged();
        }

        protected float[] _expectedValues;
        protected float[] _winPercentages;

        public bool Manual { get; set; }

        protected static void UpdateDisplayDetails(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetTreeNodeInfoModel info = sender as BetTreeNodeInfoModel;
            info.UpdateDisplay();
        }

        public BetTreeNodeModel Parent { get; protected set; }

        public BetTreeNodeInfoModel(BetTreeNodeModel model, bool manual)
        {
            Parent = model;                        
            Parent.MetaUpdated += UpdateDisplay;
            DisplayResultsIndex = model.Tree.Hero;
            Manual = manual;
        }

        public void UpdateDisplay()
        {
            this.RoundName = GetRoundName(Parent.Snapshot.Round);

            //-------------------------------------------------------------------------------------
            // Set the player name
            //-------------------------------------------------------------------------------------
            if (Parent.Snapshot.CurrentPlayer != null)
                PlayerName = Parent.Tree.Table.Seats[(int)Parent.Snapshot.CurrentPlayer].Player.Name;
            else
                PlayerName = "n/a";

            //-------------------------------------------------------------------------------------
            // Set the player action
            //-------------------------------------------------------------------------------------
            if (Parent.Data != null && Parent.Data.BetModel != null)
            {
                switch (Parent.Data.BetModel.BetType)
                {
                    case BetAction.AllIn:
                        PlayerAction = "All In (" + Parent.Data.BetModel.BetAmount + ")";
                        break;
                    case BetAction.Bet:
                        PlayerAction = "Bet " + Parent.Data.BetModel.BetAmount;
                        break;
                    case BetAction.Call:
                        PlayerAction = "Call " + Parent.Data.BetModel.BetAmount;
                        break;
                    case BetAction.Check:
                        PlayerAction = "Check";
                        break;
                    case BetAction.Fold:
                        PlayerAction = "Fold";
                        break;
                    case BetAction.Raise:
                        PlayerAction = "Raise " + Parent.Data.BetModel.BetAmount;
                        break;
                }
            }

            //-------------------------------------------------------------------------------------
            // Set the pot summary
            //-------------------------------------------------------------------------------------
            PotSummary = Parent.Snapshot.NumPlayersActive + " in pot. Total pot " + Parent.Snapshot.Bets.Sum();

            if (DisplayResultsIndex >= 0)
                HeroName = Parent.Tree.Table.Seats[DisplayResultsIndex].Player.Name;

            DisplayText = PlayerName + " " + PlayerAction;

            UpdateResult();
        }

        private string GetRoundName(HoldemHandRound round)
        {
            switch (round)
            {
                case HoldemHandRound.PreFlop:
                    return "PF";
                case HoldemHandRound.Flop:
                    return "F";
                case HoldemHandRound.Turn:
                    return "R";
                case HoldemHandRound.River:
                    return "R";
            }
            return null;
        }

        public void SetExpectedValue(float[] values)
        {
            _expectedValues = values;
            if (DisplayResultsIndex >= 0)
                ExpectedValue = "EV: " + String.Format("{0:N2}", values[DisplayResultsIndex]);
        }

        public void SetWinPercentage(float[] winPercentages)
        {
            _winPercentages = winPercentages;
            if (DisplayResultsIndex >= 0)
                WinPercentage = "Win: " + String.Format("{0:N2}", winPercentages[DisplayResultsIndex]);
        }

        protected BetTreeNodeResults _lastResult;
        protected BetTreeNodeResults _lastParentResult;

        //BetTreeNodeResults result, BetTreeNodeResults parentResult
        public void UpdateResult(BetTreeNodeResults result, BetTreeNodeResults parentResult)
        {
            _lastResult = result;
            _lastParentResult = parentResult;
            UpdateResult();
        }

        protected void UpdateResult()
        {
            if (_lastResult != null)
            {
                ResultsHeight = new GridLength(0, GridUnitType.Star);
                if (DisplayResultsIndex >= 0)
                    ExpectedValue = String.Format("{0:N2}", _lastResult.WinAmounts[DisplayResultsIndex]);
                if (_lastParentResult != null && _lastParentResult.Count > 0)
                {                                        
                    PathPercentage = String.Format("{0:N1}", (float)(_lastResult.Count * 100) / (float)_lastParentResult.Count) + "%";
                    TotalPathPercentage = String.Format("{0:N1}", (float)(_lastResult.Count * 100) / (float)_lastResult.TotalCount) + "%";                    
                }
            }

            if (this.Parent.Meta == BetTreeNodeService.RESULT_NODE_META && (!Manual || PlayerResults == null))
            {
                ObservableCollection<BetTreeNodePlayerInfoModel> players = new ObservableCollection<BetTreeNodePlayerInfoModel>();
                for (int i = 0; i < Parent.Snapshot.Bets.Length; i++)
                {
                    if (!Parent.Tree.Table.Seats[i].Player.Active) continue;

                    float max = Parent.Snapshot.Bets.Max();
                    int maxCount = Parent.Snapshot.Bets.Count(x => x == max);

                    if (Parent.Snapshot.Bets[i] < max)
                    {
                        BetTreeNodePlayerInfoModel info = new BetTreeNodePlayerInfoModel()
                        {
                            PlayerName = Parent.Tree.Table.Seats[i].Player.Name,
                            PlayerStack = Parent.Tree.Table.Seats[i].Player.Stack,
                            PlayerBet = Parent.Snapshot.Bets[i],
                            ExpectedValue = -Parent.Snapshot.Bets[i]
                        };
                        players.Add(info);
                    }
                    else
                    {
                        BetTreeNodePlayerInfoModel info = new BetTreeNodePlayerInfoModel()
                        {
                            PlayerName = Parent.Tree.Table.Seats[i].Player.Name,
                            PlayerStack = Parent.Tree.Table.Seats[i].Player.Stack,
                            PlayerBet = Parent.Snapshot.Bets[i],
                            ExpectedValue = (Parent.Snapshot.Bets.Sum() / maxCount) - Parent.Snapshot.Bets[i]
                        };
                        players.Add(info);
                    }
                }
                PlayerResults = players;
            }
            else if (!Manual && _lastResult != null)
            {
                ObservableCollection<BetTreeNodePlayerInfoModel> players = new ObservableCollection<BetTreeNodePlayerInfoModel>();
                for (int i = 0; i < _lastResult.WinAmounts.Length; i++)
                {
                    if (!Parent.Tree.Table.Seats[i].Player.Active) continue;

                    BetTreeNodePlayerInfoModel info = new BetTreeNodePlayerInfoModel()
                    {
                        PlayerName = Parent.Tree.Table.Seats[i].Player.Name,
                        PlayerStack = Parent.Tree.Table.Seats[i].Player.Stack,
                        PlayerBet = Parent.Snapshot.Bets[i],
                        ExpectedValue = _lastResult.WinAmounts[i]
                    };
                    players.Add(info);
                }
                PlayerResults = players;
            }
        }

        public BetTreeInfoXml SaveToXml()
        {
            return new BetTreeInfoXml() 
            { 
                Players = PlayerResults == null ?
                    new BetTreePlayerInfoXml[] { } :
                    PlayerResults.Select(x => x.SaveToXml()).ToArray() 
            };
        }

        public void LoadFromXml(BetTreeInfoXml xml)
        {            
            if (xml != null && xml.Players != null)
            {
                ObservableCollection<BetTreeNodePlayerInfoModel> players = new ObservableCollection<BetTreeNodePlayerInfoModel>();
                foreach (BetTreePlayerInfoXml playerXml in xml.Players)
                {
                    BetTreeNodePlayerInfoModel model = new BetTreeNodePlayerInfoModel();
                    model.LoadFromXml(playerXml);
                    players.Add(model);
                }
                PlayerResults = players;
            }            
        }

        public event EmptyEventHandler ResultsChanged;
    }
}
