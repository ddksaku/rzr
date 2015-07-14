using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Game
{
    public class CardDenomination
    {
        /// <summary>
        /// Array representing all possible denominations
        /// </summary>
        public static CardDenomination[] Denominations = new CardDenomination[]
        {
            new CardDenomination("2", 0),
            new CardDenomination("3", 1),
            new CardDenomination("4", 2),
            new CardDenomination("5", 3),
            new CardDenomination("6", 4),
            new CardDenomination("7", 5),
            new CardDenomination("8", 6),
            new CardDenomination("9", 7),
            new CardDenomination("T", 8),
            new CardDenomination("J", 9),
            new CardDenomination("Q", 10),
            new CardDenomination("K", 11),
            new CardDenomination("A", 12),
        };

        /// <summary>
        /// The character representing the denomination
        /// </summary>
        public string Char { get; private set; }

        /// <summary>
        /// The offset value of the suit for denomination purposes
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="denomChar"></param>
        /// <param name="value"></param>
        public CardDenomination(string denomChar, int value)
        {
            Char = denomChar;
            Value = value;
        }
    }
}
