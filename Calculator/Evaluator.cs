using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Calculator
{
    public class Evaluator
    {
        /// <summary>
        /// Evaluate a set of hands according to standard poker rules (Straight Flush to High Card)
        /// </summary>
        /// <param name="cards">The card mask representing the set of cards already on the table</param>
        /// <param name="numberOfCards">The number of cards in total which wil be dealt</param>
        /// <returns></returns>
        public static uint Evaluate(ulong cards, int numberOfCards)
        {
            uint retval = 0, four_mask, three_mask, two_mask;

            //-------------------------------------------------------------------------------------
            // It's quicker to do the suit and value calculations separately, so use bitwise
            // logic to separate out the suits and the ranks.
            //-------------------------------------------------------------------------------------
            uint sc = (uint)((cards >> (Values.CLUB_OFFSET)) & 0x1fffUL);
            uint sd = (uint)((cards >> (Values.DIAMOND_OFFSET)) & 0x1fffUL);
            uint sh = (uint)((cards >> (Values.HEART_OFFSET)) & 0x1fffUL);
            uint ss = (uint)((cards >> (Values.SPADE_OFFSET)) & 0x1fffUL);

            uint ranks = sc | sd | sh | ss;
            uint n_ranks = PreCalc.nBitsTable[ranks];
            uint n_dups = ((uint)(numberOfCards - n_ranks));

            //-------------------------------------------------------------------------------------
            // If we have five cards, then we can check for a five card hand
            //-------------------------------------------------------------------------------------
            if (n_ranks >= 5)
            {
                //---------------------------------------------------------------------------------
                // Start by calculating straight, flush or straight flush. If the hand is made and 
                // there's no possibility of a full house or quads, then this is the only bit of 
                // the calculation that we need to perform.
                //---------------------------------------------------------------------------------
                if (PreCalc.nBitsTable[ss] >= 5)
                {
                    if (PreCalc.straightTable[ss] != 0)
                        return Values.HANDTYPE_VALUE_STRAIGHTFLUSH + (uint)(PreCalc.straightTable[ss] << Values.TOP_CARD_SHIFT);
                    else
                        retval = Values.HANDTYPE_VALUE_FLUSH + PreCalc.topFiveCardsTable[ss];
                }
                else if (PreCalc.nBitsTable[sc] >= 5)
                {
                    if (PreCalc.straightTable[sc] != 0)
                        return Values.HANDTYPE_VALUE_STRAIGHTFLUSH + (uint)(PreCalc.straightTable[sc] << Values.TOP_CARD_SHIFT);
                    else
                        retval = Values.HANDTYPE_VALUE_FLUSH + PreCalc.topFiveCardsTable[sc];
                }
                else if (PreCalc.nBitsTable[sd] >= 5)
                {
                    if (PreCalc.straightTable[sd] != 0)
                        return Values.HANDTYPE_VALUE_STRAIGHTFLUSH + (uint)(PreCalc.straightTable[sd] << Values.TOP_CARD_SHIFT);
                    else
                        retval = Values.HANDTYPE_VALUE_FLUSH + PreCalc.topFiveCardsTable[sd];
                }
                else if (PreCalc.nBitsTable[sh] >= 5)
                {
                    if (PreCalc.straightTable[sh] != 0)
                        return Values.HANDTYPE_VALUE_STRAIGHTFLUSH + (uint)(PreCalc.straightTable[sh] << Values.TOP_CARD_SHIFT);
                    else
                        retval = Values.HANDTYPE_VALUE_FLUSH + PreCalc.topFiveCardsTable[sh];
                }
                else
                {
                    uint st = PreCalc.straightTable[ranks];
                    if (st != 0)
                        retval = Values.HANDTYPE_VALUE_STRAIGHT + (st << Values.TOP_CARD_SHIFT);
                };

                if (retval != 0 && n_dups < 3)
                    return retval;
            }

            //-------------------------------------------------------------------------------------
            // Regardless of whether we have 5 cards, we can still check for pairs, trips etc. 
            // However, if we already have a straight of a flush and there is no possibility of 
            // full house or quads, then the calculation will have exited by now. 
            //-------------------------------------------------------------------------------------
            switch (n_dups)
            {
                case 0:
                    return Values.HANDTYPE_VALUE_HIGHCARD + PreCalc.topFiveCardsTable[ranks];
                case 1:
                    {
                        uint t, kickers;

                        two_mask = ranks ^ (sc ^ sd ^ sh ^ ss);

                        retval = (uint) (Values.HANDTYPE_VALUE_PAIR + (PreCalc.topCardTable[two_mask] << Values.TOP_CARD_SHIFT));
                        t = ranks ^ two_mask;      
                        kickers = (PreCalc.topFiveCardsTable[t] >> Values.CARD_WIDTH) & ~Values.FIFTH_CARD_MASK;
                        retval += kickers;
                        return retval;
                    }

                case 2:
                    two_mask = ranks ^ (sc ^ sd ^ sh ^ ss);
                    if (two_mask != 0)
                    {
                        uint t = ranks ^ two_mask;
                        retval = (uint) (Values.HANDTYPE_VALUE_TWOPAIR
                            + (PreCalc.topFiveCardsTable[two_mask]
                            & (Values.TOP_CARD_MASK | Values.SECOND_CARD_MASK))
                            + (PreCalc.topCardTable[t] << Values.THIRD_CARD_SHIFT));

                        return retval;
                    }
                    else
                    {
                        uint t, second;
                        three_mask = ((sc & sd) | (sh & ss)) & ((sc & sh) | (sd & ss));
                        retval = (uint) (Values.HANDTYPE_VALUE_TRIPS + (PreCalc.topCardTable[three_mask] << Values.TOP_CARD_SHIFT));
                        t = ranks ^ three_mask;
                        second = PreCalc.topCardTable[t];
                        retval += (second << Values.SECOND_CARD_SHIFT);
                        t ^= (1U << (int)second);
                        retval += (uint) (PreCalc.topCardTable[t] << Values.THIRD_CARD_SHIFT);
                        return retval;
                    }

                default:
                    four_mask = sh & sd & sc & ss;
                    if (four_mask != 0)
                    {
                        uint tc = PreCalc.topCardTable[four_mask];
                        retval = (uint) (Values.HANDTYPE_VALUE_FOUR_OF_A_KIND
                            + (tc << Values.TOP_CARD_SHIFT)
                            + ((PreCalc.topCardTable[ranks ^ (1U << (int)tc)]) << Values.SECOND_CARD_SHIFT));
                        return retval;
                    };

                    two_mask = ranks ^ (sc ^ sd ^ sh ^ ss);
                    if (PreCalc.nBitsTable[two_mask] != n_dups)
                    {
                        uint tc, t;
                        three_mask = ((sc & sd) | (sh & ss)) & ((sc & sh) | (sd & ss));
                        retval = Values.HANDTYPE_VALUE_FULLHOUSE;
                        tc = PreCalc.topCardTable[three_mask];
                        retval += (tc << Values.TOP_CARD_SHIFT);
                        t = (two_mask | three_mask) ^ (1U << (int)tc);
                        retval += (uint) (PreCalc.topCardTable[t] << Values.SECOND_CARD_SHIFT);
                        return retval;
                    };

                    if (retval != 0) 
                        return retval;
                    else
                    {
                        uint top, second;

                        retval = Values.HANDTYPE_VALUE_TWOPAIR;
                        top = PreCalc.topCardTable[two_mask];
                        retval += (top << Values.TOP_CARD_SHIFT);
                        second = PreCalc.topCardTable[two_mask ^ (1 << (int)top)];
                        retval += (second << Values.SECOND_CARD_SHIFT);
                        retval += (uint) ((PreCalc.topCardTable[ranks ^ (1U << (int)top) ^ (1 << (int)second)]) << Values.THIRD_CARD_SHIFT);
                        return retval;
                    }
            }
        }
    }
}
