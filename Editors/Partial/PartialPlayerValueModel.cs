using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using Rzr.Core.Editors.Player;
using Rzr.Core.Tree;

namespace Rzr.Core.Editors.Partial
{
    public class PartialPlayerValueModel : DependencyObject
    {
        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index",
            typeof(int), typeof(PartialPlayerValueModel), new PropertyMetadata(0, null));

        public static readonly DependencyProperty PlayerProperty = DependencyProperty.Register("Player",
            typeof(PlayerModel), typeof(PartialPlayerValueModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(ValueModel), typeof(PartialPlayerValueModel), new PropertyMetadata(null, null));

        public int Index
        {
            get { return (int)this.GetValue(IndexProperty); }
            set { this.SetValue(IndexProperty, value); }
        }

        public PlayerModel Player
        {
            get { return (PlayerModel)this.GetValue(PlayerProperty); }
            set { this.SetValue(PlayerProperty, value); }
        }

        public ValueModel Value
        {
            get { return (ValueModel)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
    }
}
