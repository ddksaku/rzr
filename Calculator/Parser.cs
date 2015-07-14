using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Calculator
{
    public class Parser
    {
        /// <summary>
        /// Parses the string representation of a group of cards into uLong value, where each
        /// bit of the uLong represents an individual card. 
        /// </summary>
        /// <remarks>
        /// The card to which each bit corresponds is calculated using the constants in Values.cs. 
        /// For example, Values.Diamonds has a value of 1 and Values.RankTen has a value of 8, so the
        /// integer representation of the card is Values.Diamonds * 13 + Values.RankTen = 21. If 
        /// The ten of diamonds is contained within the hand string, the 21st bit of the return value
        /// be 1. If it is not contained within the hand string, the 21st bit of the return value will be 0.
        /// </remarks>
        /// <param name="hand">String representation of the group of cards to be parsed</param>
        /// <param name="cards">Reference parameter indicating the number of cards parsed</param>
        /// <returns>A long value detailing which cards were present in the hand string</returns>
        public static ulong ParseHand(string hand, ref int cards)
        {
            if (hand == null) hand = String.Empty;

            int index = 0;
            ulong handmask = 0UL;
            cards = 0;

            if (hand.Trim().Length == 0) return 0UL;

            for (int card = GetCard(hand, ref index); card >= 0; card = GetCard(hand, ref index))
            {
                handmask |= (1UL << card);
                cards++;
            }
            return handmask;
        }

        public static void ParseHole(string hand, ref ushort denoms, ref ushort suits, ref bool pair, ref bool suited, ref int[] values)
        {
            int index = 0;
            int cards = 0;
            pair = false;
            suited = false;            

            for (int card = GetCard(hand, ref index); card >= 0; card = GetCard(hand, ref index))
            {
                values[card % 13]++;
                if ((denoms & (ushort)(card % 13)) > 0) pair = true;
                denoms |= (ushort)(1UL << (card % 13));
                if ((suits & (ushort)(suits / 4)) > 0) suited = true;
                suits |= (ushort)(1UL << (card / 13));
                cards++;
            }
        }

        /// <summary>
        /// Given a starting index in a string, parses the string representation of a single card
        /// and returns the integer that corresponds to that card. See ParseHand for further 
        /// details. 
        /// </summary>
        /// <param name="cards">The string representation of a group of cards</param>
        /// <param name="index">The point in the string from which to start parsing</param>
        /// <returns></returns>
        public static int GetCard(string cards, ref int index)
        {
            int rank = 0, suit = 0;

            //-------------------------------------------------------------------------------------
            // If there is whitespace, ignore it. Start parsing at the first non-whitespace 
            // character
            //-------------------------------------------------------------------------------------
            while (index < cards.Length && cards[index] == ' ')
                index++;

            //-------------------------------------------------------------------------------------
            // If we've hit the end of the string, finish now
            //-------------------------------------------------------------------------------------
            if (index >= cards.Length)
                return -1;

            //-------------------------------------------------------------------------------------
            // The first character should be the rank. 
            //-------------------------------------------------------------------------------------
            if (index < cards.Length)
            {
                switch (cards[index++])
                {
                    case '2':
                        rank = Values.Rank2;
                        break;
                    case '3':
                        rank = Values.Rank3;
                        break;
                    case '4':
                        rank = Values.Rank4;
                        break;
                    case '5':
                        rank = Values.Rank5;
                        break;
                    case '6':
                        rank = Values.Rank6;
                        break;
                    case '7':
                        rank = Values.Rank7;
                        break;
                    case '8':
                        rank = Values.Rank8;
                        break;
                    case '9':
                        rank = Values.Rank9;
                        break;
                    case 'T':
                    case 't':
                        rank = Values.RankTen;
                        break;
                    case 'J':
                    case 'j':
                        rank = Values.RankJack;
                        break;
                    case 'Q':
                    case 'q':
                        rank = Values.RankQueen;
                        break;
                    case 'K':
                    case 'k':
                        rank = Values.RankKing;
                        break;
                    case 'A':
                    case 'a':
                        rank = Values.RankAce;
                        break;
                    default:
                        return -2;
                }
            }
            else
            {
                return -2;
            }

            //-------------------------------------------------------------------------------------
            // The second character should be the suit. 
            //-------------------------------------------------------------------------------------
            if (index < cards.Length)
            {
                switch (cards[index++])
                {
                    case 'H':
                    case 'h':
                        suit = Values.Hearts;
                        break;
                    case 'D':
                    case 'd':
                        suit = Values.Diamonds;
                        break;
                    case 'C':
                    case 'c':
                        suit = Values.Clubs;
                        break;
                    case 'S':
                    case 's':
                        suit = Values.Spades;
                        break;
                    default:
                        return -2;
                }
            }
            else
            {
                return -2;
            }

            return rank + (suit * 13);
        }

        public static int GetCard(string card)
        {
            int index = 0;
            return GetCard(card, ref index);
        }

        /// <summary>
        /// 
        /// </summary>
        public static ulong ParseHand(string pocket, string board, ref int cards)
        {
            return ParseHand(pocket + " " + board, ref cards);
        }
    }
}
