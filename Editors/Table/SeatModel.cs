using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Editors.Player;
using System.Windows;

namespace Rzr.Core.Editors.Table
{
    public class SeatModel : DependencyObject
    {
        public static readonly DependencyProperty PlayerProperty = DependencyProperty.Register(
            "Player", typeof(PlayerModel), typeof(SeatModel), new PropertyMetadata(null, null));

        public PlayerModel Player
        {
            get { return (PlayerModel)this.GetValue(PlayerProperty); }
            set { this.SetValue(PlayerProperty, value); }
        }

        public int SeatNumber { get; private set; }

        public SeatModel(int seatNumber)
        {
            SeatNumber = seatNumber;
        }

        public void SeatPlayer(PlayerModel player)
        {
            Player = player;
        }
    }
}
