using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Rzr.Core.Tree.Xml;

namespace Rzr.Core.Tree
{
    public class BetTreeNodePlayerInfoModel : DependencyObject
    {
        public static readonly DependencyProperty PlayerNameProperty = DependencyProperty.Register(
            "PlayerName", typeof(string), typeof(BetTreeNodePlayerInfoModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty PlayerStackProperty = DependencyProperty.Register(
            "PlayerStack", typeof(float), typeof(BetTreeNodePlayerInfoModel), new PropertyMetadata(0f, null));

        public static readonly DependencyProperty PlayerBetProperty = DependencyProperty.Register(
            "PlayerBet", typeof(float), typeof(BetTreeNodePlayerInfoModel), new PropertyMetadata(0f, null));

        public static readonly DependencyProperty ExpectedValueProperty = DependencyProperty.Register(
            "ExpectedValue", typeof(float), typeof(BetTreeNodePlayerInfoModel), new PropertyMetadata(0f, null));

        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(
            "Index", typeof(int), typeof(BetTreeNodePlayerInfoModel), new PropertyMetadata(0, null));

        public string PlayerName
        {
            get { return (string)this.GetValue(PlayerNameProperty); }
            set { this.SetValue(PlayerNameProperty, value); }
        }

        public float PlayerStack
        {
            get { return (float)this.GetValue(ExpectedValueProperty); }
            set { this.SetValue(ExpectedValueProperty, value); }
        }

        public float PlayerBet
        {
            get { return (float)this.GetValue(ExpectedValueProperty); }
            set { this.SetValue(ExpectedValueProperty, value); }
        }

        public float ExpectedValue
        {
            get { return (float)this.GetValue(ExpectedValueProperty); }
            set { this.SetValue(ExpectedValueProperty, value); }
        }

        public int Index
        {
            get { return (int)this.GetValue(IndexProperty); }
            set { this.SetValue(IndexProperty, value); }
        }

        public BetTreePlayerInfoXml SaveToXml()
        {
            return new BetTreePlayerInfoXml()
            {
                Name = PlayerName,
                Bet = PlayerBet,
                Stack = PlayerStack,
                ExpectedValue = ExpectedValue,
                Index = Index
            };
        }

        public void LoadFromXml(BetTreePlayerInfoXml xml)
        {
            PlayerName = xml.Name;
            PlayerBet = xml.Bet;
            PlayerStack = xml.Stack;
            ExpectedValue = xml.ExpectedValue;
            Index = xml.Index;
        }
    }
}
