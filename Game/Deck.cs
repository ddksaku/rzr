using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Game
{
    /// <summary>
    /// Logical representation of a deck of cards
    /// </summary>
    public class Deck
    {
        #region static

        /// <summary>
        /// Initialiser for DeckLong var
        /// </summary>
        static Deck()
        {
            for (int i = 0; i < 52; i++)
                DeckLong |= 1ul << i;
        }

        /// <summary>
        /// ulong representation of a deck of cards, where bits 1 to 52 each represent a single card from the deck
        /// </summary>
        public static ulong DeckLong { get; private set; }

        #endregion

        #region constructor

        public Card[] Cards { get; private set; }
        
        public Deck()
        {
            Cards = new Card[52];
            for (int i = 0; i < 52; i++)
            {
                Cards[i] = new Card(i);
            }
        }

        #endregion
    }
}
