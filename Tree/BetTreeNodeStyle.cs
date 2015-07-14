using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace Rzr.Core.Tree
{
    /// <summary>
    /// The style of nodes for a particular round of the bet tree
    /// </summary>
    public class BetTreeNodeStyle : DependencyObject
    {
        #region dependency property definition

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background",
            typeof(Brush), typeof(BetTreeNodeStyle), new PropertyMetadata(new SolidColorBrush(Colors.Transparent), null));

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground",
            typeof(Brush), typeof(BetTreeNodeStyle), new PropertyMetadata(new SolidColorBrush(Colors.White), null));

        public static readonly DependencyProperty ResultForegroundProperty = DependencyProperty.Register("DisplayRegex",
            typeof(Brush), typeof(BetTreeNodeStyle), new PropertyMetadata(new SolidColorBrush(Colors.Red), null));

        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register("BorderColor",
            typeof(Brush), typeof(BetTreeNodeStyle), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(212, 212, 212)), null));

        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness",
            typeof(Thickness), typeof(BetTreeNodeStyle), new PropertyMetadata(new Thickness(2), null));

        public static readonly DependencyProperty CornersProperty = DependencyProperty.Register("Corners",
            typeof(CornerRadius), typeof(BetTreeNodeStyle), new PropertyMetadata(new CornerRadius(6), null));

        #endregion

        /// <summary>
        /// The color of the background
        /// </summary>
        public Brush Background
        {
            get { return (Brush)this.GetValue(BackgroundProperty); }
            set { this.SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// The color of the foreground
        /// </summary>
        public Brush Foreground
        {
            get { return (Brush)this.GetValue(ForegroundProperty); }
            set { this.SetValue(ForegroundProperty, value); }
        }

        /// <summary>
        /// The color of the result text
        /// </summary>
        public Brush ResultForeground
        {
            get { return (Brush)this.GetValue(ResultForegroundProperty); }
            set { this.SetValue(ResultForegroundProperty, value); }
        }

        /// <summary>
        /// The color of the border
        /// </summary>
        public Brush BorderColor
        {
            get { return (Brush)this.GetValue(BorderColorProperty); }
            set { this.SetValue(BorderColorProperty, value); }
        }

        /// <summary>
        /// The thickness of the border
        /// </summary>
        public Thickness BorderThickness
        {
            get { return (Thickness)this.GetValue(BorderThicknessProperty); }
            set { this.SetValue(BorderThicknessProperty, value); }
        }

        /// <summary>
        /// The radius of the corners
        /// </summary>
        public CornerRadius Corners
        {
            get { return (CornerRadius)this.GetValue(CornersProperty); }
            set { this.SetValue(CornersProperty, value); }
        }
    }
}
