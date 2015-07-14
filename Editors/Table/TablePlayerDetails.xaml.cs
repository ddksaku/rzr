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
using System.Drawing;
using Rzr.Core.Editors.Player;

namespace Rzr.Core.Editors.Table
{
    /// <summary>
    /// Interaction logic for TablePlayerDetails.xaml
    /// </summary>
    public partial class TablePlayerDetails : UserControl
    {
        #region static

        protected static float[] denominations;
        protected static Bitmap[] sources;        

        static TablePlayerDetails()
        {
            denominations = new float[] { 0.01f, 0.05f, 0.25f, 1, 5, 25, 100 };
            sources = new Bitmap[]
            {
                Properties.Resources.chips_1cent,
                Properties.Resources.chips_5cent,
                Properties.Resources.chips_25cent,
                Properties.Resources.chips_1dollar,
                Properties.Resources.chips_5dollar,
                Properties.Resources.chips_25dollar,
                Properties.Resources.chips_100dollar
            };
        }

        #endregion

        protected PlayerModel _model;
        public TableModel Table { get; set; }

        public TablePlayerDetails()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = this.DataContext as PlayerModel;
            if (_model == null) return;

            _model.BetChanged += OnPlayerBetChanged;
        }

        protected void OnPlayerBetChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetDisplay(_model.Seat == Table.Button, _model.Bet);
        }

        public void SetDisplay(bool button, float amount)
        {
            PlayerBetGrid.Children.Clear();

            float remainder = amount;
            int[] quantity = new int[7];

            for (int i = denominations.Length - 1; i >= 0; i--)
            {
                int chips = (int)(remainder / denominations[i]);
                if (chips == 0) continue;

                remainder -= (chips * denominations[i]);
                int numChips = Math.Min(4, chips);
                quantity[i] = numChips;
            }

            int numColumns = quantity.Count(x => x > 0);
            int startIndex = 3 + (numColumns - 1) / 2;
            for (int i = 0; i < quantity.Length; i++)
            {
                if (quantity[i] == 0) continue;

                BitmapSource source = Utilities.LoadBitmap(sources[i]);
                for (int j = 0; j < quantity[i]; j++)
                {
                    System.Windows.Controls.Image image = new System.Windows.Controls.Image() { Source = source };
                    PlayerBetGrid.Children.Add(image);
                    Grid.SetColumn(image, startIndex);
                    Grid.SetRow(image, 4 - j);
                    Grid.SetRowSpan(image, 2);
                }

                startIndex--;
            }

            if (amount == 0) return;

            TextBlock block = new TextBlock()
            {
                Text = amount.ToString(),
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                FontSize = 14,
                FontWeight = FontWeights.Bold
            };
            PlayerBetGrid.Children.Add(block);
            Grid.SetRow(block, 6);
            Grid.SetColumn(block, 2);
            Grid.SetColumnSpan(block, 3);
        }
    }
}
