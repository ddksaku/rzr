using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Game
{
    /// <summary>
    /// Wrapper class representing a single playing card
    /// </summary>
    public class Card
    {
        #region properties
        
        /// <summary>
        /// The raw integer value from 0 to 51 that represents this card
        /// </summary>
        public int RawInt { get; private set; }

        /// <summary>
        /// The raw long value from 2^0 to 2^51 that represents this card
        /// </summary>
        public ulong RawLong { get; private set; }

        /// <summary>
        /// The suite construct for this card
        /// </summary>
        public CardSuit Suit { get; private set; }

        /// <summary>
        /// The denomination construct for this card
        /// </summary>
        public CardDenomination Denom { get; private set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="card"></param>
        public Card(int card)
        {
            RawInt = card;
            RawLong = 1ul << card;
            Suit = CardSuit.Suits[card / 13];
            Denom = CardDenomination.Denominations[card % 13];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="card"></param>
        public Card(ulong card)
        {
            RawLong = card;
            for (int i = 0; i < 52; i++) if (1ul << i == card) RawInt = i;
            Suit = CardSuit.Suits[card / 13];
            Denom = CardDenomination.Denominations[card % 13];
        }

        public override string ToString()
        {
            return Denom.Char + Suit.Char;
        }
    }
}
