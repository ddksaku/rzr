using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Editors.HandRange;
using Rzr.Core.Data;
using System.Windows;
using Rzr.Core.Editors.Variables;

namespace Rzr.Core.Editors.Player
{
    public class PlayerModel : DependencyObject
    {
        #region properties

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name",
                typeof(string), typeof(PlayerModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index",
                typeof(int), typeof(PlayerModel), new PropertyMetadata(-1, null));

        public static readonly DependencyProperty SeatProperty = DependencyProperty.Register("Seat", 
            typeof(int), typeof(PlayerModel), new PropertyMetadata(-1, null));

        public static readonly DependencyProperty HandRangeProperty = DependencyProperty.Register("HandRange",
            typeof(HandRangeModel), typeof(PlayerModel), new PropertyMetadata(null, HandRangeChanged));

        public static readonly DependencyProperty HeadlineResultProperty = DependencyProperty.Register("HeadlineResult",
            typeof(string), typeof(PlayerModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty ActiveProperty = DependencyProperty.Register(
            "Active", typeof(bool), typeof(PlayerModel), new PropertyMetadata(true, ActivePropertyChanged));

        public static readonly DependencyProperty StackProperty = DependencyProperty.Register(
            "Stack", typeof(float), typeof(PlayerModel), new PropertyMetadata(100f, StackPropertyChanged));

        public static readonly DependencyProperty BetProperty = DependencyProperty.Register(
            "Bet", typeof(float), typeof(PlayerModel), new PropertyMetadata(0f, BetPropertyChanged));

        public static readonly DependencyProperty ExpectedValueProperty = DependencyProperty.Register(
            "ExpectedValue", typeof(string), typeof(PlayerModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            "Status", typeof(ActiveStatus), typeof(PlayerModel), new PropertyMetadata(ActiveStatus.StillToBet, ActivePropertyChanged));

        public static readonly DependencyProperty PlayerVisibilityProperty = DependencyProperty.Register(
            "PlayerVisibility", typeof(Visibility), typeof(PlayerModel), new PropertyMetadata(Visibility.Visible, null));

        public static readonly DependencyProperty ContainerProperty = DependencyProperty.Register("Container",
                typeof(VariableContainer), typeof(PlayerModel), new PropertyMetadata(null, null));

        public string Name
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        public int Index
        {
            get { return (int)this.GetValue(IndexProperty); }
            set { this.SetValue(IndexProperty, value); }
        }

        public int Seat
        {
            get { return (int)this.GetValue(SeatProperty); }
            set { this.SetValue(SeatProperty, value); }
        }

        public bool Active
        {
            get { return (bool)this.GetValue(ActiveProperty); }
            set { this.SetValue(ActiveProperty, value); }
        }

        public Visibility PlayerVisibility
        {
            get { return (Visibility)this.GetValue(PlayerVisibilityProperty); }
            set { this.SetValue(PlayerVisibilityProperty, value); }
        }

        public HandRangeModel HandRange
        {
            get { return (HandRangeModel)this.GetValue(HandRangeProperty); }
            set { this.SetValue(HandRangeProperty, value); }
        }

        public ActiveStatus Status
        {
            get { return (ActiveStatus)this.GetValue(StatusProperty); }
            set { this.SetValue(StatusProperty, value); }
        }

        public string HeadlineResult
        {
            get { return (string)this.GetValue(HeadlineResultProperty); }
            set { this.SetValue(HeadlineResultProperty, value); }
        }

        public float Stack
        {
            get { return (float)this.GetValue(StackProperty); }
            set { this.SetValue(StackProperty, value); }
        }

        public float Bet
        {
            get { return (float)this.GetValue(BetProperty); }
            set { this.SetValue(BetProperty, value); }
        }

        public string ExpectedValue
        {
            get { return (string)this.GetValue(ExpectedValueProperty); }
            set { this.SetValue(ExpectedValueProperty, value); }
        }

        public VariableContainer Container
        {
            get { return (VariableContainer)this.GetValue(ContainerProperty); }
            set { this.SetValue(ContainerProperty, value); }
        }

        public static void HandRangeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PlayerModel model = sender as PlayerModel;
            if (model.PocketChanged != null) model.PocketChanged(model, e);
        }

        public static void ActivePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PlayerModel model = sender as PlayerModel;
            model.PlayerVisibility = model.Active ? Visibility.Visible : Visibility.Collapsed;
            if (model.ActiveChanged != null) model.ActiveChanged();
        }

        public static void StackPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PlayerModel model = sender as PlayerModel;
            if (model.StackChanged != null) model.StackChanged(model, e);
        }

        public static void BetPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PlayerModel model = sender as PlayerModel;
            if (model.BetChanged != null) model.BetChanged(model, e);
        }
        
        public Array AllowedStatus
        {
            get
            {
                return Enum.GetValues(typeof(ActiveStatus));
            }
        }

        #endregion

        public PlayerModel()
        {
            Container = new VariableContainer();
        }

        public event DependencyPropertyChangedEventHandler PocketChanged;
        public event DependencyPropertyChangedEventHandler StackChanged;
        public event EmptyEventHandler ActiveChanged;
        public event DependencyPropertyChangedEventHandler BetChanged;

        public PlayerModel Clone()
        {
            PlayerModel ret = new PlayerModel();
            PlayerModel.Copy(this, ret);
            return ret;
        }

        protected static void Copy(PlayerModel from, PlayerModel to)
        {
            to.Name = from.Name;
            to.Index = from.Index;
            to.Seat = from.Seat;
            to.Active = from.Active;
            to.HandRange = from.HandRange;
            to.Status = from.Status;
            to.Stack = from.Stack;
            to.Bet = from.Bet;            
        }
    }
}
