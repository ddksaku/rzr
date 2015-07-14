using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Rzr.Core.Tree
{
    /// <summary>
    /// Model which contains parameters dictating the the display of different node TYPES. The node type
    /// relates to the type of BetTreeNodeDataModel which is referenced by the BetTreeNode, NOT the round.
    /// </summary>
    public class BetTreeNodeDisplay : DependencyObject
    {
        #region dependency property definition

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", 
            typeof(GridLength), typeof(BetTreeNodeDisplay), new PropertyMetadata(new GridLength(180), null));

        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height",
            typeof(GridLength), typeof(BetTreeNodeDisplay), new PropertyMetadata(new GridLength(180), null));

        public static readonly DependencyProperty DisplayRegexProperty = DependencyProperty.Register("DisplayRegex",
            typeof(string), typeof(BetTreeNodeDisplay), new PropertyMetadata(
                "{Round} - {CurrentPlayer} ({CurrentPlayerStake}): {BetAction} {BetAmount} ({TotalPot})", 
            null));

        #endregion

        #region dependency properties

        /// <summary>
        /// The width of the box which contains the node display
        /// </summary>
        public GridLength Width
        {
            get { return (GridLength)this.GetValue(WidthProperty); }
            set { this.SetValue(WidthProperty, value); }
        }

        /// <summary>
        /// The height of the box which contains the node display
        /// </summary>
        public GridLength Height
        {
            get { return (GridLength)this.GetValue(HeightProperty); }
            set { this.SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// The string used as the base for the node display
        /// </summary>
        public string DisplayRegex
        {
            get { return (string)this.GetValue(DisplayRegexProperty); }
            set { this.SetValue(DisplayRegexProperty, value); }
        }

        #endregion
    }
}
