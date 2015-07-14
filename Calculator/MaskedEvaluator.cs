using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Calculator;

namespace Rzr.Core.Calculator
{
    public class MaskedEvaluator
    {        
        static int[] handHash = new int[0];
        static ulong MASK_INIT = ConditionMap.DRAWTYPE;

        /// <summary>
        /// Evaluate a set of hands according to standard poker rules (Straight Flush to High Card)
        /// </summary>
        /// <param name="cards">The card mask representing the set of cards already on the table</param>
        /// <param name="numberOfCards">The number of cards in total which wil be dealt</param>
        /// <returns></returns>
        public static HandMask Evaluate(ulong board, ulong hand, int numberOfCards)
        {
            #region initialise

            //-------------------------------------------------------------------------------------
            // It's quicker to do the suit and value calculations separately, so use bitwise
            // logic to separate out the suits and the ranks.
            //-------------------------------------------------------------------------------------
            ulong masks = MASK_INIT;
            ulong cards = board | hand;
            uint retval = 0, four_mask, three_mask, two_mask;

            uint boardsc = (uint)((board >> (Values.CLUB_OFFSET)) & 0x1fffUL);
            uint boardsd = (uint)((board >> (Values.DIAMOND_OFFSET)) & 0x1fffUL);
            uint boardsh = (uint)((board >> (Values.HEART_OFFSET)) & 0x1fffUL);
            uint boardss = (uint)((board >> (Values.SPADE_OFFSET)) & 0x1fffUL);
            uint boardranks = boardsc | boardsd | boardsh | boardss;

            uint handsc = (uint)((hand >> (Values.CLUB_OFFSET)) & 0x1fffUL);
            uint handsd = (uint)((hand >> (Values.DIAMOND_OFFSET)) & 0x1fffUL);
            uint handsh = (uint)((hand >> (Values.HEART_OFFSET)) & 0x1fffUL);
            uint handss = (uint)((hand >> (Values.SPADE_OFFSET)) & 0x1fffUL);
            uint handranks = handsc | handsd | handsh | handss;

            uint sc = (uint)((cards >> (Values.CLUB_OFFSET)) & 0x1fffUL);
            uint sd = (uint)((cards >> (Values.DIAMOND_OFFSET)) & 0x1fffUL);
            uint sh = (uint)((cards >> (Values.HEART_OFFSET)) & 0x1fffUL);
            uint ss = (uint)((cards >> (Values.SPADE_OFFSET)) & 0x1fffUL);
            uint ranks = sc | sd | sh | ss;

            uint n_ranks = PreCalc.nBitsTable[ranks];
            uint n_dups = ((uint)(numberOfCards - n_ranks));

            #endregion

            #region straight/flush
            //-------------------------------------------------------------------------------------
            // If we have five cards, then we can check for a five card hand
            //-------------------------------------------------------------------------------------
            if (n_ranks >= 5)
            {
                if (CalculateFlushOrStraightFlush(numberOfCards, handsc, boardsc, sc, ref masks, ref retval))
                    return new HandMask() { Mask = masks, Result = retval };
                else if (CalculateFlushOrStraightFlush(numberOfCards, handsd, boardsd, sd, ref masks, ref retval))
                    return new HandMask() { Mask = masks, Result = retval };
                else if (CalculateFlushOrStraightFlush(numberOfCards, handsh, boardsh, sh, ref masks, ref retval))
                    return new HandMask() { Mask = masks, Result = retval };
                else if (CalculateFlushOrStraightFlush(numberOfCards, handss, boardss, ss, ref masks, ref retval))
                    return new HandMask() { Mask = masks, Result = retval };

                if ((masks & (ConditionMap.SUBDRAWVALUE_BACKDOORFLUSHDRAW | ConditionMap.SUBDRAWVALUE_BACKDOORFLUSHDRAW)) == 0)
                    masks |= ConditionMap.SUBDRAWVALUE_NOFLUSHDRAW;

                if (retval < Values.HANDTYPE_VALUE_FLUSH)
                {
                    CalculateStraight(handranks, boardranks, ranks, ref masks, ref retval);
                };

                if (retval != 0 && n_dups < 3)
                {
                    return new HandMask()
                    {
                        Result = retval,
                        Mask = masks,
                    };
                }
            }
            #endregion
         
            #region straight draw

            else if (n_ranks == 4)
            {
                masks |= PreCalc.straightDrawTable[ranks]; 
                if ((masks & ConditionMap.SUBDRAWVALUE_FLUSHDRAW) > 0)
                {
                    if ((masks & ConditionMap.SUBDRAWVALUE_OPENENDEDSTRAIGHTORDOUBLEGUTSHOT) > 0)
                    {
                        masks |= ConditionMap.SUBDRAWVALUE_FLUSHANDOPENENDEDSTRAIGHT;
                    }
                    else if ((masks & ConditionMap.SUBDRAWVALUE_SINGLEENDEDSTRAIGHTORGUTSHOT) > 0)
                    {
                        masks |= ConditionMap.SUBDRAWVALUE_FLUSHANDGUTSHOT;
                    }
                }
                else if ((masks & ConditionMap.SUBDRAWVALUE_BACKDOORFLUSHDRAW) > 0)
                {
                    if ((masks & ConditionMap.SUBDRAWVALUE_OPENENDEDSTRAIGHTORDOUBLEGUTSHOT) > 0)
                    {
                        masks |= ConditionMap.SUBDRAWVALUE_BACKDOORFLUSHANDOPENENDEDSTRAIGHT;
                    }
                    else if ((masks & ConditionMap.SUBDRAWVALUE_SINGLEENDEDSTRAIGHTORGUTSHOT) > 0)
                    {
                        masks |= ConditionMap.SUBDRAWVALUE_BACKDOORFLUSHANDGUTSHOT;
                    }
                }

            }

            //-------------------------------------------------------------------------------------
            // Overcard draw
            //-------------------------------------------------------------------------------------
            if (PreCalc.topCardTable[handranks] > PreCalc.topCardTable[boardranks])
            {
                uint handSecondCard = PreCalc.topCardTable[handranks ^ PreCalc.topCardMask[handranks]];
                if (PreCalc.topCardTable[handSecondCard] > PreCalc.topCardTable[boardranks])
                    masks |= ConditionMap.SUBDRAWVALUE_TWOOVERCARDS;
                else
                    masks |= ConditionMap.SUBDRAWVALUE_ONEOVERCARDS;
            }
            else
                masks |= ConditionMap.SUBDRAWVALUE_NOOVERCARDS;

            #endregion

            #region denominations

            //-------------------------------------------------------------------------------------
            // Regardless of whether we have 5 cards, we can still check for pairs, trips etc. 
            // However, if we already have a straight of a flush and there is no possibility of 
            // full house or quads, then the calculation will have exited by now. 
            //-------------------------------------------------------------------------------------
            switch (n_dups)
            {
                case 0:
                    {
                        return CalculateHighCard(handranks, boardranks, ranks, ref masks);
                    }
                case 1:
                    {
                        return CalculatePair(numberOfCards, handranks, boardranks, ranks, sc, sd, sh, ss, ref masks, ref retval);
                    }
                case 2:                    
                    two_mask = ranks ^ (sc ^ sd ^ sh ^ ss);
                    if (two_mask != 0)
                    {
                        return CalculateTwoPair(numberOfCards, handranks, boardranks, ranks, two_mask, ref masks, ref retval);
                    }
                    else
                    {                                                
                        three_mask = ((sc & sd) | (sh & ss)) & ((sc & sh) | (sd & ss));
                        return CalculateThreeOfAKind(numberOfCards, handranks, boardranks, ranks, three_mask, ref masks, ref retval);
                    }                        
                default:
                    four_mask = sh & sd & sc & ss;
                    if (four_mask != 0)
                    {
                        return CalculateFourOfAKind(numberOfCards, handranks, boardranks, ranks, four_mask, ref masks, ref retval);
                    };

                    two_mask = ranks ^ (sc ^ sd ^ sh ^ ss);
                    if (PreCalc.nBitsTable[two_mask] != n_dups)
                    {
                        three_mask = ((sc & sd) | (sh & ss)) & ((sc & sh) | (sd & ss));
                        return CalculateFullHouse(numberOfCards, handranks, boardranks, ranks, two_mask, three_mask, ref masks, ref retval);
                    };

                    if (retval != 0)
                        return new HandMask()
                        {
                            Result = retval,
                            Mask = masks,
                        };
                    else
                    {
                        return CalculateTwoPairTwo(numberOfCards, handranks, boardranks, ranks, two_mask, ref masks, ref retval);
                    }
            }
            #endregion
        }

        private static HandMask CalculateHighCard(uint handranks, uint boardranks, uint ranks, ref ulong masks)
        {
            masks |= ConditionMap.HANDVALUE_HIGHCARD_MASK;

            if ((ranks & Values.RankAShift) > 0) masks |= ConditionMap.SUBVALUE_ACETOP;
            else if ((ranks & Values.RankKShift) > 0) masks |= ConditionMap.SUBVALUE_KINGTOP;
            else if ((ranks & Values.RankQShift) > 0) masks |= ConditionMap.SUBVALUE_QUEENTOP;
            else if ((ranks & Values.RankJShift) > 0) masks |= ConditionMap.SUBVALUE_JACKTOP;
            else masks |= ConditionMap.SUBVALUE_TENLOWERTOP;

            if (handranks > boardranks)
            {
                handranks ^= 1u << PreCalc.topCardTable[handranks];
                if (handranks > boardranks)
                    masks |= ConditionMap.SUBVALUE_TWOOVERCARDS;
                else
                    masks |= ConditionMap.SUBVALUE_ONEOVERCARD;
            }
            else
            {
                masks |= ConditionMap.SUBVALUE_NOOVERCARDS;
            }

            return new HandMask()
            {
                Result = Values.HANDTYPE_VALUE_HIGHCARD + PreCalc.topFiveCardsTable[ranks],
                Mask = masks,
            };
        }

        private static HandMask CalculatePair(int numberOfCards, uint handranks, uint boardranks, uint ranks, uint sc, uint sd, uint sh, uint ss, ref ulong masks, ref uint retval)
        {
            uint t, kickers;

            uint two_mask = ranks ^ (sc ^ sd ^ sh ^ ss);

            masks |= ConditionMap.HANDVALUE_PAIR_MASK;

            ushort pairValue = PreCalc.topCardTable[two_mask];

            retval = (uint)(Values.HANDTYPE_VALUE_PAIR + (pairValue << Values.TOP_CARD_SHIFT));
            t = ranks ^ two_mask;
            kickers = (PreCalc.topFiveCardsTable[t] >> Values.CARD_WIDTH) & ~Values.FIFTH_CARD_MASK;

            if (pairValue >= PreCalc.topCardTable[boardranks])
            {
                uint nHandRanks = PreCalc.nBitsTable[handranks];

                if (nHandRanks == 1)
                {
                    masks |= ConditionMap.SUBVALUE_PAIROVERPAIR;
                }
                else
                {
                    masks |= ConditionMap.SUBVALUE_PAIRTOPPAIR;
                }
            }
            else
            {
                uint nextRanks = ranks ^ PreCalc.topCardMask[ranks];
                if (pairValue >= PreCalc.topCardTable[nextRanks])
                {
                    masks |= ConditionMap.SUBVALUE_PAIRSECONDPAIR;
                }
                else
                {
                    masks |= ConditionMap.SUBVALUE_PAIRLOWERPAIR;
                }
            }

            if (PreCalc.nBitsTable[handranks] == 1)
                masks |= ConditionMap.SUBVALUE_POCKETPAIR;
            else
                masks |= ConditionMap.SUBVALUE_NOPOCKETPAIR;

            if (PreCalc.nBitsTable[boardranks] < (numberOfCards - 2))
                masks |= ConditionMap.SUBVALUE_PAIRONBOARD;
            else
                masks |= ConditionMap.SUBVALUE_NOPAIRONBOARD;

            int kicker = PreCalc.topCardTable[t];
            if (kicker == 12)
                masks |= ConditionMap.SUBVALUE_KICKER_A;
            else if (kicker == 11)
                masks |= ConditionMap.SUBVALUE_KICKER_K;
            else if (kicker == 10)
                masks |= ConditionMap.SUBVALUE_KICKER_Q;
            else
                masks |= ConditionMap.SUBVALUE_KICKER_J;

            retval += kickers;

            return new HandMask()
            {
                Result = retval,
                Mask = masks
            };
        }

        private static HandMask CalculateTwoPair(int numberOfCards, uint handranks, uint boardranks, uint ranks, uint two_mask, ref ulong masks, ref uint retval)
        {
            uint t = ranks ^ two_mask;
            masks |= ConditionMap.HANDVALUE_TWOPAIR_MASK;
            retval = (uint)(Values.HANDTYPE_VALUE_TWOPAIR
                + (PreCalc.topFiveCardsTable[two_mask]
                & (Values.TOP_CARD_MASK | Values.SECOND_CARD_MASK))
                + (PreCalc.topCardTable[t] << Values.THIRD_CARD_SHIFT));

            ushort pairValue = PreCalc.topCardTable[two_mask];
            ushort secondPairValue = PreCalc.topCardTable[two_mask ^ (uint)(1ul << pairValue)];
            int kicker = PreCalc.topCardTable[t];

            CalculateTwoPairSub(numberOfCards, pairValue, secondPairValue, two_mask, kicker, handranks, boardranks, ranks, ref masks);

            return new HandMask()
            {
                Result = retval,
                Mask = masks
            };
        }

        private static HandMask CalculateTwoPairTwo(int numberOfCards, uint handranks, uint boardranks, uint ranks, uint two_mask, ref ulong masks, ref uint retval)
        {
            uint top, second;

            masks |= ConditionMap.HANDVALUE_TWOPAIR_MASK;
            retval = Values.HANDTYPE_VALUE_TWOPAIR;
            top = PreCalc.topCardTable[two_mask];
            retval += (top << Values.TOP_CARD_SHIFT);
            second = PreCalc.topCardTable[two_mask ^ (1 << (int)top)];
            retval += (second << Values.SECOND_CARD_SHIFT);
            int kicker = (int)(PreCalc.topCardTable[ranks ^ (1U << (int)top) ^ (1 << (int)second)]);
            retval += ((uint)kicker << Values.THIRD_CARD_SHIFT);

            ushort pairValue = PreCalc.topCardTable[two_mask];
            ushort secondPairValue = PreCalc.topCardTable[two_mask ^ (uint)(1ul << pairValue)];

            CalculateTwoPairSub(numberOfCards, pairValue, secondPairValue, two_mask, kicker, handranks, boardranks, ranks, ref masks);

            return new HandMask()
            {
                Result = retval,
                Mask = masks,
            };

        }

        private static HandMask CalculateThreeOfAKind(int numberOfCards, uint handranks, uint boardranks, uint ranks, uint three_mask, ref ulong masks, ref uint retval)
        {
            uint t, second;
            masks |= ConditionMap.HANDVALUE_TRIPS_MASK;
            retval = (uint)(Values.HANDTYPE_VALUE_TRIPS + (PreCalc.topCardTable[three_mask] << Values.TOP_CARD_SHIFT));
            t = ranks ^ three_mask;
            second = PreCalc.topCardTable[t];
            retval += (second << Values.SECOND_CARD_SHIFT);
            t ^= (1U << (int)second);
            retval += (uint)(PreCalc.topCardTable[t] << Values.THIRD_CARD_SHIFT);

            if (PreCalc.nBitsTable[handranks] == 1)
            {
                masks |= ConditionMap.SUBVALUE_THREESET;
            }
            else
            {
                if ((three_mask & handranks) > 0)
                    masks |= ConditionMap.SUBVALUE_THREETRIPS;
                else
                    masks |= ConditionMap.SUBVALUE_THREEBOARD;
            }

            uint topCard = PreCalc.topCardMask[boardranks];
            uint secondCard = PreCalc.topCardMask[boardranks ^ topCard];
            if ((handranks & topCard) == topCard)
                masks |= ConditionMap.SUBVALUE_TOP;
            else if ((handranks & secondCard) == topCard)
                masks |= ConditionMap.SUBVALUE_SECOND;
            else
                masks |= ConditionMap.SUBVALUE_LOWER;

            if (second == 12)
                masks |= ConditionMap.SUBVALUE_KICKER_A;
            else if (second == 11)
                masks |= ConditionMap.SUBVALUE_KICKER_K;
            else if (second == 10)
                masks |= ConditionMap.SUBVALUE_KICKER_Q;
            else 
                masks |= ConditionMap.SUBVALUE_KICKER_J;

            return new HandMask()
            {
                Result = retval,
                Mask = masks
            };
        }

        /// <summary>
        /// Calculate the hand value and associated masks where we have a straight
        /// </summary>
        private static void CalculateStraight(uint handranks, uint boardranks, uint ranks, ref ulong masks, ref uint retval)
        {
            //-----------------------------------------------------------------------------
            // Calculate the flush draw element of the hand mask
            //-----------------------------------------------------------------------------

            uint st = PreCalc.straightTable[ranks];

            if (st != 0)
            {
                masks |= ConditionMap.HANDVALUE_STRAIGHT_MASK;
                retval = Values.HANDTYPE_VALUE_STRAIGHT + (st << Values.TOP_CARD_SHIFT);

                uint testranks = PreCalc.nutStraightTable[boardranks];
                if ((handranks & PreCalc.nutStraightTable[boardranks]) == PreCalc.nutStraightTable[boardranks])
                    masks |= ConditionMap.SUBVALUE_TOP;
                else if ((handranks & PreCalc.secondStraightTable[boardranks]) == PreCalc.secondStraightTable[boardranks])
                    masks |= ConditionMap.SUBVALUE_SECOND;
                else
                    masks |= ConditionMap.SUBVALUE_LOWER;

                uint requiredRanks = PreCalc.straightRanks[st] ^ (boardranks & (PreCalc.straightRanks[st]));

                if (PreCalc.nBitsTable[requiredRanks] == 2)
                    masks |= ConditionMap.SUBVALUE_HOLECARDSBOTH;
                else if (PreCalc.nBitsTable[requiredRanks] == 1)
                    masks |= ConditionMap.SUBVALUE_HOLECARDSONE;
                else
                    masks |= ConditionMap.SUBVALUE_HOLECARDSNONE;
            }
            else
            {
                masks |= PreCalc.straightDrawTable[ranks];
            }
        }

        /// <summary>
        /// Calculate the hand value and associated masks where we have a flush or a straight flush
        /// </summary>
        private static bool CalculateFlushOrStraightFlush(int numberOfCards, uint handsuit, uint boardsuit, uint allsuit, ref ulong mask, ref uint ret)
        {
            if (PreCalc.nBitsTable[allsuit] >= 5)
            {
                //---------------------------------------------------------------------------------
                // If we can make a straight with the cards of the suit, we have a straight flush
                //---------------------------------------------------------------------------------
                if (PreCalc.straightTable[allsuit] != 0)
                {
                    mask |= ConditionMap.HANDVALUE_STRAIGHTFLUSH_MASK;
                    ret = Values.HANDTYPE_VALUE_STRAIGHTFLUSH + (uint)(PreCalc.straightTable[allsuit] << Values.TOP_CARD_SHIFT);

                    //-----------------------------------------------------------------------------
                    // How many hole cards are used for the straight flush?
                    //-----------------------------------------------------------------------------
                    int numHoleCards = PreCalc.nBitsTable[(PreCalc.topFiveCardsMask[allsuit] & handsuit)];

                    if (numHoleCards == 2)
                        mask |= ConditionMap.SUBVALUE_HOLECARDSBOTH;
                    else if (numHoleCards == 1)
                        mask |= ConditionMap.SUBVALUE_HOLECARDSONE;
                    else
                        mask |= ConditionMap.SUBVALUE_HOLECARDSNONE;                    
                    return true;
                }
                //---------------------------------------------------------------------------------
                // If no straight, then we have a plain flush
                //---------------------------------------------------------------------------------
                else
                {
                    mask |= ConditionMap.HANDVALUE_FLUSH_MASK;
                    ret = Values.HANDTYPE_VALUE_FLUSH + PreCalc.topFiveCardsTable[allsuit];

                    //-----------------------------------------------------------------------------
                    // Is this the best possible flush given the cards on the board?
                    //-----------------------------------------------------------------------------
                    if ((allsuit & PreCalc.nutFlushTable[boardsuit]) == PreCalc.nutFlushTable[boardsuit])
                        mask |= ConditionMap.SUBVALUE_TOP;
                    else if ((allsuit & PreCalc.secondFlushTable[boardsuit]) == PreCalc.secondFlushTable[boardsuit])
                        mask |= ConditionMap.SUBVALUE_SECOND;
                    else
                        mask |= ConditionMap.SUBVALUE_LOWER;

                    //-----------------------------------------------------------------------------
                    // How many hole cards are used for the flush?
                    //-----------------------------------------------------------------------------
                    int numHoleCards = PreCalc.nBitsTable[(PreCalc.topFiveCardsMask[allsuit] & handsuit)];

                    if (numHoleCards == 2)
                        mask |= ConditionMap.SUBVALUE_HOLECARDSBOTH;
                    else if (numHoleCards == 1)
                        mask |= ConditionMap.SUBVALUE_HOLECARDSONE;
                    else
                        mask |= ConditionMap.SUBVALUE_HOLECARDSNONE;
                    return true;
                }
            }
            //-------------------------------------------------------------------------------------
            // If we have no flush or straight flush we can still have a draw. If there are 6 cards
            // we can have a standard draw only
            //-------------------------------------------------------------------------------------
            else if (numberOfCards == 6)
            {
                if (PreCalc.nBitsTable[allsuit] == 4)
                {
                    mask |= ConditionMap.SUBDRAWVALUE_FLUSHDRAW;

                    //-----------------------------------------------------------------------------
                    // How many hole cards are used for the draw?
                    //-----------------------------------------------------------------------------
                    if (PreCalc.nBitsTable[handsuit] == 2)
                        mask |= ConditionMap.SUBDRAWVALUE_BOTHHOLECARDS;
                    else if (PreCalc.nBitsTable[handsuit] == 1)
                        mask |= ConditionMap.SUBDRAWVALUE_ONEHOLECARD;
                    else
                        mask |= ConditionMap.SUBDRAWVALUE_NOHOLECARDS;
                }
            }
            //-------------------------------------------------------------------------------------
            // If we have no flush or straight flush we can still have a draw. If there are 5 cards 
            // test for backdoor flush draw as well
            //-------------------------------------------------------------------------------------
            else if (numberOfCards == 5)
            {
                if (PreCalc.nBitsTable[allsuit] == 4)
                {
                    mask |= ConditionMap.SUBDRAWVALUE_FLUSHDRAW;

                    //-----------------------------------------------------------------------------
                    // How many hole cards are used for the draw?
                    //-----------------------------------------------------------------------------
                    if (PreCalc.nBitsTable[handsuit] == 2)
                        mask |= ConditionMap.SUBDRAWVALUE_BOTHHOLECARDS;
                    else if (PreCalc.nBitsTable[handsuit] == 1)
                        mask |= ConditionMap.SUBDRAWVALUE_ONEHOLECARD;
                    else
                        mask |= ConditionMap.SUBDRAWVALUE_NOHOLECARDS;

                }
                else if (PreCalc.nBitsTable[allsuit] == 3)
                {
                    mask |= ConditionMap.SUBDRAWVALUE_BACKDOORFLUSHDRAW;

                    //-----------------------------------------------------------------------------
                    // How many hole cards are used for the draw?
                    //-----------------------------------------------------------------------------
                    if (PreCalc.nBitsTable[handsuit] == 2)
                        mask |= ConditionMap.SUBDRAWVALUE_BOTHHOLECARDS;
                    else if (PreCalc.nBitsTable[handsuit] == 1)
                        mask |= ConditionMap.SUBDRAWVALUE_ONEHOLECARD;
                    else
                        mask |= ConditionMap.SUBDRAWVALUE_NOHOLECARDS;
                }
            }
            return false;
        }

        /// <summary>
        /// Calculate the hand value and associated masks where we have a full house
        /// </summary>
        private static HandMask CalculateFullHouse(int numberOfCards, uint handranks, uint boardranks, uint ranks, uint two_mask, uint three_mask, ref ulong masks, ref uint retval)
        {
            uint tc, t;

            masks |= ConditionMap.HANDVALUE_FULLHOUSE_MASK;
            retval = Values.HANDTYPE_VALUE_FULLHOUSE;
            tc = PreCalc.topCardTable[three_mask];
            retval += (tc << Values.TOP_CARD_SHIFT);
            t = (two_mask | three_mask) ^ (1U << (int)tc);
            retval += (uint)(PreCalc.topCardTable[t] << Values.SECOND_CARD_SHIFT);

            if (PreCalc.nBitsTable[handranks] == 1)
            {
                uint topThree = PreCalc.topCardMask[three_mask];
                if ((handranks & topThree) == handranks)
                    masks |= ConditionMap.SUBVALUE_HOLECARDSBOTH;
                else if ((handranks & PreCalc.topCardMask[t]) == handranks)
                {
                    if ((handranks & boardranks) > 0)
                        masks |= ConditionMap.SUBVALUE_HOLECARDSONE;
                    else
                        masks |= ConditionMap.SUBVALUE_HOLECARDSBOTH;
                }
                else
                    masks |= ConditionMap.SUBVALUE_HOLECARDSNONE;
            }
            else
            {
                if ((PreCalc.nBitsTable[three_mask] == 1) && (handranks & (three_mask | (1u << PreCalc.topCardTable[t]))) == handranks)
                    masks |= ConditionMap.SUBVALUE_HOLECARDSBOTH;
                else if ((handranks & (three_mask | PreCalc.topCardMask[t])) > 0)
                    masks |= ConditionMap.SUBVALUE_HOLECARDSONE;
                else
                    masks |= ConditionMap.SUBVALUE_HOLECARDSNONE;
            }


            if ((three_mask & handranks) > 0)
            {
                uint topCard =PreCalc.topCardMask[ranks];
                uint secondCard = PreCalc.topCardMask[ranks ^ topCard];

                if ((three_mask & topCard) > 0)
                    masks |= ConditionMap.SUBVALUE_TOP;
                else if ((three_mask & secondCard) > 0)
                    masks |= ConditionMap.SUBVALUE_SECOND;
                else
                    masks |= ConditionMap.SUBVALUE_LOWER;
            }
            else
            {
                uint topThree = PreCalc.topCardMask[three_mask];
                uint topCard = PreCalc.topCardMask[ranks ^ topThree];
                uint secondCard = PreCalc.topCardMask[ranks ^ (topThree | topCard)];

                if ((two_mask & topCard) > 0)
                    masks |= ConditionMap.SUBVALUE_TOP;
                else if ((two_mask & secondCard) > 0)
                    masks |= ConditionMap.SUBVALUE_SECOND;
                else
                    masks |= ConditionMap.SUBVALUE_LOWER;
            }

            return new HandMask()
            {
                Result = retval,
                Mask = masks
            };
        }

        /// <summary>
        /// Calculate the hand value and associated masks where we have four of a kind
        /// </summary>
        private static HandMask CalculateFourOfAKind(int numberOfCards, uint handranks, uint boardranks, uint ranks, uint four_mask, ref ulong masks, ref uint retval)
        {
            uint tc = PreCalc.topCardTable[four_mask];
            masks |= ConditionMap.HANDVALUE_QUADS_MASK;
            retval = (uint)(Values.HANDTYPE_VALUE_FOUR_OF_A_KIND
                + (tc << Values.TOP_CARD_SHIFT)
                + ((PreCalc.topCardTable[ranks ^ (1U << (int)tc)]) << Values.SECOND_CARD_SHIFT));

            if (PreCalc.nBitsTable[handranks] == 1)
            {
                if ((handranks & four_mask) == handranks)
                    masks |= ConditionMap.SUBVALUE_HOLECARDSBOTH;
                else
                    masks |= ConditionMap.SUBVALUE_HOLECARDSNONE;
            }
            else
            {
                if ((handranks & four_mask) > 0)
                    masks |= ConditionMap.SUBVALUE_HOLECARDSONE;
                else
                    masks |= ConditionMap.SUBVALUE_HOLECARDSNONE;
            }

            return new HandMask()
            {
                Result = retval,
                Mask = masks
            };
        }

        private static void CalculateTwoPairSub(int numberOfCards, ushort pairValue, ushort secondPairValue, uint two_mask,
            int kicker, uint handranks, uint boardranks, uint ranks, ref ulong masks)
        {
            if (PreCalc.nBitsTable[handranks] == 1)
                masks |= ConditionMap.SUBVALUE_POCKETPAIR;
            else
                masks |= ConditionMap.SUBVALUE_NOPOCKETPAIR;

            if (PreCalc.nBitsTable[boardranks] < (numberOfCards - 2))
                masks |= ConditionMap.SUBVALUE_PAIRONBOARD;
            else
                masks |= ConditionMap.SUBVALUE_NOPAIRONBOARD;

            if (pairValue >= PreCalc.topCardTable[boardranks])
            {
                uint nHandRanks = PreCalc.nBitsTable[handranks];

                if (nHandRanks == 1 && (PreCalc.topCardTable[handranks] == pairValue))
                {
                    masks |= ConditionMap.SUBVALUE_PAIROVERPAIR;
                }
                else
                {
                    masks |= ConditionMap.SUBVALUE_PAIRTOPPAIR;
                }
            }
            else
            {
                uint nextRanks = ranks ^ PreCalc.topCardMask[ranks];
                if (two_mask >= PreCalc.topCardMask[nextRanks])
                {
                    masks |= ConditionMap.SUBVALUE_PAIRSECONDPAIR;
                }
                else
                {
                    masks |= ConditionMap.SUBVALUE_PAIRLOWERPAIR;
                }
            }

            if (secondPairValue >= PreCalc.topCardTable[boardranks])
            {
                masks |= ConditionMap.SUBVALUE_SECONDPAIRTOPPAIR;
            }
            else
            {
                uint nextRanks = ranks ^ PreCalc.topCardMask[ranks];
                uint secondPairMask = two_mask ^ PreCalc.topCardMask[two_mask];
                if (secondPairMask >= PreCalc.topCardMask[nextRanks])
                {
                    masks |= ConditionMap.SUBVALUE_SECONDPAIRSECONDPAIR;
                }
                else
                {
                    masks |= ConditionMap.SUBVALUE_SECONDPAIRLOWERPAIR;
                }
            }

            if (kicker == 12)
                masks |= ConditionMap.SUBVALUE_KICKER_A;
            else if (kicker == 11)
                masks |= ConditionMap.SUBVALUE_KICKER_K;
            else if (kicker == 10)
                masks |= ConditionMap.SUBVALUE_KICKER_Q;
            else
                masks |= ConditionMap.SUBVALUE_KICKER_J;

        }
    }
}
