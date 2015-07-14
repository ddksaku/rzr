using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Game;
using System.Windows;

namespace Rzr.Core.Editors
{
    public class CardModel : DependencyObject
    {
        public static readonly DependencyProperty AvailableProperty = DependencyProperty.Register(
            "Available", typeof(bool), typeof(CardModel), new PropertyMetadata(true, OnCardSelectionChanged));

        public static readonly DependencyProperty ModelCardProperty = DependencyProperty.Register(
            "ModelCard", typeof(Card), typeof(CardModel), new PropertyMetadata(null, OnCardSelectionChanged));

        public bool Available 
        {
            get { return (bool)this.GetValue(AvailableProperty); }
            set { this.SetValue(AvailableProperty, value); }
        }

        public Card ModelCard
        {
            get { return (Card)this.GetValue(ModelCardProperty); }
            set { this.SetValue(ModelCardProperty, value); }
        }

        protected static void OnCardSelectionChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CardModel model = sender as CardModel;
            if (model.CardSelectionChanged != null) model.CardSelectionChanged();
        }

        public CardModel()
        {
            ModelCard = null;
        }

        public CardModel(int card)
        {
            ModelCard = new Card(card);
        }

        public event EmptyEventHandler CardSelectionChanged;
    }
}
