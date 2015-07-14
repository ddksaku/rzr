using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Calculator
{
    public class Values
    {
        #region Consts

        public static readonly int Hearts = 2;
        public static readonly int Diamonds = 1;
        public static readonly int Clubs = 0;
        public static readonly int Spades = 3;

        public static readonly int Rank2 = 0;
        public static readonly int Rank3 = 1;
        public static readonly int Rank4 = 2;
        public static readonly int Rank5 = 3;
        public static readonly int Rank6 = 4;
        public static readonly int Rank7 = 5;
        public static readonly int Rank8 = 6;
        public static readonly int Rank9 = 7;
        public static readonly int RankTen = 8;
        public static readonly int RankJack = 9;
        public static readonly int RankQueen = 10;
        public static readonly int RankKing = 11;
        public static readonly int RankAce = 12;

        public static readonly uint Rank2Shift = 1u << Rank2;
        public static readonly uint Rank3Shift = 1u << Rank3;
        public static readonly uint Rank4Shift = 1u << Rank4;
        public static readonly uint Rank5Shift = 1u << Rank5;
        public static readonly uint Rank6Shift = 1u << Rank6;
        public static readonly uint Rank7Shift = 1u << Rank7;
        public static readonly uint Rank8Shift = 1u << Rank8;
        public static readonly uint Rank9Shift = 1u << Rank9;
        public static readonly uint RankTShift = 1u << RankTen;
        public static readonly uint RankJShift = 1u << RankJack;
        public static readonly uint RankQShift = 1u << RankQueen;
        public static readonly uint RankKShift = 1u << RankKing;
        public static readonly uint RankAShift = 1u << RankAce;

        public static readonly int CardJoker = 52;
        public static readonly int NumberOfCards = 52;
        public static readonly int NCardsWJoker = 53;
        public static readonly int HANDTYPE_SHIFT = 24;
        public static readonly int TOP_CARD_SHIFT = 16;
        public static readonly System.UInt32 TOP_CARD_MASK = 0x000F0000;
        public static readonly int SECOND_CARD_SHIFT = 12;
        public static readonly System.UInt32 SECOND_CARD_MASK = 0x0000F000;
        public static readonly int THIRD_CARD_SHIFT = 8;
        public static readonly int FOURTH_CARD_SHIFT = 4;
        public static readonly int FIFTH_CARD_SHIFT = 0;
        public static readonly System.UInt32 FIFTH_CARD_MASK = 0x0000000F;
        public static readonly int CARD_WIDTH = 4;
        public static readonly System.UInt32 CARD_MASK = 0x0F;

        public static readonly uint HANDTYPE_VALUE_STRAIGHTFLUSH = (((uint)HandTypes.StraightFlush) << HANDTYPE_SHIFT);
        public static readonly uint HANDTYPE_VALUE_STRAIGHT = (((uint)HandTypes.Straight) << HANDTYPE_SHIFT);
        public static readonly uint HANDTYPE_VALUE_FLUSH = (((uint)HandTypes.Flush) << HANDTYPE_SHIFT);
        public static readonly uint HANDTYPE_VALUE_FULLHOUSE = (((uint)HandTypes.FullHouse) << HANDTYPE_SHIFT);
        public static readonly uint HANDTYPE_VALUE_FOUR_OF_A_KIND = (((uint)HandTypes.FourOfAKind) << HANDTYPE_SHIFT);
        public static readonly uint HANDTYPE_VALUE_TRIPS = (((uint)HandTypes.Trips) << HANDTYPE_SHIFT);
        public static readonly uint HANDTYPE_VALUE_TWOPAIR = (((uint)HandTypes.TwoPair) << HANDTYPE_SHIFT);
        public static readonly uint HANDTYPE_VALUE_PAIR = (((uint)HandTypes.Pair) << HANDTYPE_SHIFT);
        public static readonly uint HANDTYPE_VALUE_HIGHCARD = (((uint)HandTypes.HighCard) << HANDTYPE_SHIFT);
        public static readonly int SPADE_OFFSET = 13 * Spades;
        public static readonly int CLUB_OFFSET = 13 * Clubs;
        public static readonly int DIAMOND_OFFSET = 13 * Diamonds;
        public static readonly int HEART_OFFSET = 13 * Hearts;

        #endregion

        public enum HandTypes
        {
            HighCard = 0,
            Pair = 1,
            TwoPair = 2,
            Trips = 3,
            Straight = 4,
            Flush = 5,
            FullHouse = 6,
            FourOfAKind = 7,
            StraightFlush = 8
        }
    }
}
