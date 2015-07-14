using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Game
{
    /// <summary>
    /// Construct representing the suit of a single card
    /// </summary>
    public class CardSuit
    {
        /// <summary>
        /// An array of all possible suits
        /// </summary>
        public static CardSuit[] Suits = new CardSuit[]
        {
            new CardSuit("c", 0),
            new CardSuit("d", 1),
            new CardSuit("h", 2),
            new CardSuit("s", 3)
        };

        /// <summary>
        /// The character representing the suit
        /// </summary>
        public string Char { get; private set; }

        /// <summary>
        /// The offset value of the suit for calculation purposes
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CardSuit(string suitChar, int value)
        {
            Char = suitChar;
            Value = value;
        }
    }
}
