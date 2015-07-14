using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Rzr.Core.Controls
{
    public class ProgressBar : Canvas
    {
        public double Progress { get; private set; }
        public double Min { get; private set; }
        public double Max { get; private set; }
        public string Text { get; private set; }

        protected Canvas _bar;
        protected Label _label;

        public ProgressBar()
        {
            //http://stackoverflow.com/questions/363377/how-do-i-run-a-simple-bit-of-code-in-a-new-thread
            LinearGradientBrush background = new LinearGradientBrush();
            background.GradientStops.Add(new GradientStop(Color.FromRgb(0, 20, 0), 0));
            background.GradientStops.Add(new GradientStop(Color.FromRgb(0, 40, 70), 0.25));
            background.GradientStops.Add(new GradientStop(Color.FromRgb(0, 60, 140), 0.6));
            background.GradientStops.Add(new GradientStop(Color.FromRgb(0, 80, 210), 1));
            _bar = new Canvas() { Background = background };
            this.Children.Add(_bar);

            _label = new Label();
            this.Children.Add(_label);

            this.SizeChanged += OnSizeChanged;
        }

        public void Initialise(double min, double max, string text)
        {
            _bar.Height = this.Height;
            Min = min;
            Max = max;
            Progress = 0;
            Text = text;

            _bar.Width = 0;
            
            _label.Height = this.Height;
            _label.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            _label.Foreground = new SolidColorBrush(Colors.White);
            _label.Content = text;
            _label.VerticalAlignment = VerticalAlignment.Center;
            _label.HorizontalAlignment = HorizontalAlignment.Center;
            _label.Padding = new Thickness(0);
            _label.FontSize = 12;
            OnSizeChanged(this, null);

            InvalidateVisual();
        }

        public void Update(double progress)
        {
            if (Max - Min <= 0) return;

            Progress = progress;            
            _bar.Width = this.ActualWidth * (Progress / (Max - Min));
            InvalidateVisual();
        }

        public void FinishProgress()
        {
            Update(0);
            _label.Content = "";

        }

        protected void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _bar.Width = this.ActualWidth;
            Canvas.SetLeft(_label, (this.ActualWidth / 2) - (_label.ActualWidth / 2));
        }
    }
}
