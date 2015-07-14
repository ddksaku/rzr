using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Rzr.Core.Controls
{
    public class RangeKeyControl : Canvas
    {
        public int BoxWidth { get; set; }
        public int BoxHeight { get; set; }

        protected Color[] _colors;

        public RangeKeyControl()
        {
            Initialise();
        }

        private void Initialise()
        {
            _colors = (Color[])ControlUtilities.StandardGradingColors.Clone();

            for (int i = 0; i <= 10; i++)
            {
                DataButton button = new DataButton()
                {
                    Height = BoxHeight,
                    Width = BoxWidth,
                    Margin = new Thickness(i * BoxWidth, 0, 0, 0),
                    FontSize = 10,
                    Content = (i * 10) + "%",
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    Background = new SolidColorBrush(_colors[i]),
                    Value = i
                };

                button.Click += OnLevelButtonClicked;
                this.Children.Add(button);
            }
        }

        protected void OnLevelButtonClicked(object sender, RoutedEventArgs e)
        {
            if (LevelButtonClicked != null) LevelButtonClicked(sender, e);
        }

        public event RoutedEventHandler LevelButtonClicked;
    }
}
