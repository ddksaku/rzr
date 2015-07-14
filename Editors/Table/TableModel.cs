using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Editors.Player;
using System.Windows;
using System.Collections.ObjectModel;

namespace Rzr.Core.Editors.Table
{
    public class TableModel : DependencyObject
    {
        #region dependency properties

        public static readonly DependencyProperty SeatsProperty = DependencyProperty.Register("Seats",
            typeof(ObservableCollection<SeatModel>), typeof(TableModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty ButtonProperty = DependencyProperty.Register("ButtonProperty",
            typeof(int), typeof(TableModel), new PropertyMetadata(0, OnButtonUpdated));

        public static readonly DependencyProperty SmallBlindProperty = DependencyProperty.Register("SmallBlind",
            typeof(int), typeof(TableModel), new PropertyMetadata(0, null));

        public static readonly DependencyProperty BigBlindProperty = DependencyProperty.Register("BigBlind",
            typeof(int), typeof(TableModel), new PropertyMetadata(0, null));

        public static readonly DependencyProperty UnderTheGunProperty = DependencyProperty.Register("UnderTheGun",
            typeof(int), typeof(TableModel), new PropertyMetadata(0, null));

        public static readonly DependencyProperty StartRoundProperty = DependencyProperty.Register("StartRound",
            typeof(HoldemHandRound), typeof(TableModel), new PropertyMetadata(HoldemHandRound.PreFlop, null));

        public static readonly DependencyProperty EndRoundProperty = DependencyProperty.Register("EndRound",
            typeof(HoldemHandRound), typeof(TableModel), new PropertyMetadata(HoldemHandRound.River, null));


        public ObservableCollection<SeatModel> Seats
        {
            get { return (ObservableCollection<SeatModel>)this.GetValue(SeatsProperty); }
            set { this.SetValue(SeatsProperty, value); }
        }

        public int Button
        {
            get { return (int)this.GetValue(ButtonProperty); }
            set { this.SetValue(ButtonProperty, value); }
        }

        public int BigBlind
        {
            get { return (int)this.GetValue(BigBlindProperty); }
            set { this.SetValue(BigBlindProperty, value); }
        }

        public int SmallBlind
        {
            get { return (int)this.GetValue(SmallBlindProperty); }
            set { this.SetValue(SmallBlindProperty, value); }
        }

        public int UnderTheGun
        {
            get { return (int)this.GetValue(UnderTheGunProperty); }
            set { this.SetValue(UnderTheGunProperty, value); }
        }

        public HoldemHandRound StartRound
        {
            get { return (HoldemHandRound)this.GetValue(StartRoundProperty); }
            set { this.SetValue(StartRoundProperty, value); }
        }

        public HoldemHandRound EndRound
        {
            get { return (HoldemHandRound)this.GetValue(EndRoundProperty); }
            set { this.SetValue(EndRoundProperty, value); }
        }

        public int[] AllowedButtonPositions { get; set; }

        protected static void OnButtonUpdated(object sender, DependencyPropertyChangedEventArgs e)
        {
            TableModel model = sender as TableModel;
            model.ButtonSet(model.Button);
        }

        #endregion

        public TableModel(int tableSize)
        {
            AllowedButtonPositions = new int[tableSize];
            Seats = new ObservableCollection<SeatModel>();
            for (int i = 0; i < tableSize; i++)
            {
                Seats.Add(new SeatModel(i));
                AllowedButtonPositions[i] = i + 1;
            }
            ButtonSet(0);
        }

        public void SeatPlayer(int seat, PlayerModel player)
        {
            player.Index = seat;
            Seats[seat].SeatPlayer(player);            
        }

        protected void ButtonSet(int button)
        {
            if (Seats.Count == 2)
            {
                Button = button;
                BigBlind = button ^ 1;
                SmallBlind = button;
                UnderTheGun = button ^ 1;
            }
            else
            {
                Button = button;
                BigBlind = button + 2;
                if (BigBlind >= Seats.Count) BigBlind -= Seats.Count;
                SmallBlind = button + 1;
                if (SmallBlind >= Seats.Count) SmallBlind -= Seats.Count;
                UnderTheGun = button + 3;
                if (UnderTheGun >= Seats.Count) UnderTheGun -= Seats.Count;
            }
        }        
    }
}
