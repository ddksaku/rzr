using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Game;

namespace Rzr.Core.Calculator
{
    public class RangeCalculatorService
    {
        protected Random _rand = new Random();

        public double Count { get; protected set; }

        public HandMask[][][] Calculate(ulong[] pockets, HandRange[] ranges, Card[] boardCards, int numIterations, bool[] show)
        {
            Count = 0;
            ulong board = 0ul;
            ulong[] boardParts = new ulong[3];
            int[] numCardsSelected = new int[3];
            HandMask[][][] conditionMasks = new HandMask[numIterations][][];

            //-------------------------------------------------------------------------------------
            // 
            //-------------------------------------------------------------------------------------
            for (int i = 0; i < pockets.Length; i++)
            {
                board ^= pockets[i];
            }

            //-------------------------------------------------------------------------------------
            // Get all the possible hand ranges and their probabilities given the range data 
            //-------------------------------------------------------------------------------------
            HandProbability[][] combos = CollateHandRangeCombinations(ranges);

            //-------------------------------------------------------------------------------------
            // Initialise the board mask from the model data
            //-------------------------------------------------------------------------------------              
            int numCards = InitialiseBoardCards(boardCards, ref boardParts, ref numCardsSelected, ref board);

            //-------------------------------------------------------------------------------------
            // Iterate through a series of random sets of board cards
            //-------------------------------------------------------------------------------------            
            for (int i = 0; i < numIterations; i++)
            {
                conditionMasks[i] = GetConditionResults(pockets, ranges, combos, show, board, boardParts, numCardsSelected);
                Count++;
            }

            return conditionMasks;
        }


        public HandMask[][][] Calculate(HandRange[] ranges, Card[] boardCards, int numIterations, bool[] show)
        {
            Count = 0;
            ulong board = 0ul;
            ulong[] boardParts = new ulong[3];
            int[] numCardsSelected = new int[3];
            HandMask[][][] conditionMasks = new HandMask[numIterations][][];

            //-------------------------------------------------------------------------------------
            // Get all the possible hand ranges and their probabilities given the range data 
            //-------------------------------------------------------------------------------------
            HandProbability[][] combos = CollateHandRangeCombinations(ranges);

            //-------------------------------------------------------------------------------------
            // Initialise the board mask from the model data
            //-------------------------------------------------------------------------------------              
            int numCards = InitialiseBoardCards(boardCards, ref boardParts, ref numCardsSelected, ref board);

            //-------------------------------------------------------------------------------------
            // Iterate through a series of random sets of board cards
            //-------------------------------------------------------------------------------------            
            for (int i = 0; i < numIterations; i++)
            {
                conditionMasks[i] = GetConditionResults(new ulong[0], ranges, combos, show, board, boardParts, numCardsSelected);
                Count++;
            }

            return conditionMasks;
        }

        protected HandProbability[][] CollateHandRangeCombinations(HandRange[] ranges)
        {
            HandProbability[][] ret = new HandProbability[ranges.Length][];

            for (int i = 0; i < ranges.Length; i++)
            {
                List<HandProbability> rangeProb = new List<HandProbability>();
                for (uint handDef = 0; handDef < 169; handDef++)
                {
                    int probability = ranges[i].Probability[(int)handDef];
                    if (ranges[i].Probability[(int)handDef] == 0) continue;

                    uint card1 = handDef % 13;
                    uint card2 = handDef / 13;

                    if (card1 < card2) // Suited
                    {
                        for (int index1 = 0; index1 < 4; index1++)
                        {
                            ulong longCard1 = 1UL << (int)(card1 + (index1 * 13));
                            ulong longCard2 = 1UL << (int)(card2 + (index1 * 13));
                            ulong pocket = longCard1 | longCard2;
                            rangeProb.Add(new HandProbability() { Hand = pocket, Probability = probability });
                        }
                    }
                    else if (card1 == card2) // Pair
                    {
                        for (int index1 = 0; index1 < 4; index1++)
                        {
                            ulong longCard1 = 1UL << (int)(card1 + (index1 * 13));
                            for (int index2 = index1 + 1; index2 < 4; index2++)
                            {
                                ulong longCard2 = 1UL << (int)(card2 + (index2 * 13));
                                ulong pocket = longCard1 | longCard2;
                                rangeProb.Add(new HandProbability() { Hand = pocket, Probability = probability });
                            }
                        }
                    }
                    else if (card1 > card2) // Unsuited
                    {
                        for (int index1 = 0; index1 < 4; index1++)
                        {
                            ulong longCard1 = 1UL << (int)(card1 + (index1 * 13));
                            for (int index2 = 0; index2 < 4; index2++)
                            {
                                if (index1 == index2) continue;

                                ulong longCard2 = 1UL << (int)(card2 + (index2 * 13));
                                ulong pocket = longCard1 | longCard2;
                                rangeProb.Add(new HandProbability() { Hand = pocket, Probability = probability });
                            }
                        }
                    }
                }
                ret[i] = rangeProb.ToArray();
            }

            return ret;
        }

        protected int InitialiseBoardCards(Card[] boardCards, ref ulong[] boardParts, ref int[] numCardsSelected, ref ulong board)
        {
            int numCards = 0;
            for (int i = 0; i < 5; i++)
            {
                if (boardCards[i] != null)
                {
                    if (i <= 2)
                    {
                        boardParts[0] |= boardCards[i].RawLong;
                        numCardsSelected[0]++;
                    }
                    else if (i <= 3)
                    {
                        boardParts[1] |= boardCards[i].RawLong;
                        numCardsSelected[1]++;
                    }
                    else
                    {
                        boardParts[2] |= boardCards[i].RawLong;
                        numCardsSelected[2]++;
                    }
                    board |= boardCards[i].RawLong;
                    numCards++;
                }
            }
            return numCards;
        }

        protected HandMask[][] GetConditionResults(ulong[] pockets, HandRange[] ranges, HandProbability[][] combos,
            bool[] show, ulong fullBoard, ulong[] boardPart, int[] numCardsSelected)
        {
            ulong[] hands = GetHands(pockets, ranges, combos, fullBoard);
            HandMask[][] handMasks = new HandMask[][] {
                new HandMask[hands.Length],
                new HandMask[hands.Length],
                new HandMask[hands.Length],
            };
            ulong dead = 0;

            foreach (ulong hand in hands)
                dead |= hand;

            ulong board = boardPart[0] | GetRandomCards(dead, 3 - numCardsSelected[0]);
            if (show[0])
                handMasks[0] = SetResults(3, hands, board);

            board |= boardPart[1] | GetRandomCards(dead | board, 1 - numCardsSelected[1]);
            if (show[1])
                handMasks[1] = SetResults(4, hands, board);

            board |= boardPart[2] | GetRandomCards(dead | board, 1 - numCardsSelected[2]);
            if (show[2])
                handMasks[2] = SetResults(5, hands, board);

            return handMasks;
        }

        protected ulong[] GetHands(ulong[] pockets, HandRange[] ranges, HandProbability[][] combos, ulong board)
        {
            ulong[] ret = new ulong[pockets.Length + ranges.Length];
            ulong deck = Deck.DeckLong ^ board;

            for (int j = 0; j < pockets.Length; j++)
                ret[j] = pockets[j];

            for (int j = 0; j < combos.Length; j++)
            {
                if (combos[j].Length == 0) continue;

                //---------------------------------------------------------------------------------
                // Select a hand at random
                //---------------------------------------------------------------------------------
                int index = _rand.Next(combos[j].Length);
                int prob = _rand.Next(100);

                HandProbability combo = combos[j][index];
                //---------------------------------------------------------------------------------
                // Select another hand if any of the cards required have already been dealt, or if
                // the probability of the player using the hand fails the random probability test
                //---------------------------------------------------------------------------------
                while ((deck & combo.Hand) != combo.Hand || prob > combo.Probability)
                {
                    index = _rand.Next(combos[j].Length);
                    prob = _rand.Next(100);
                    combo = combos[j][index];
                }

                deck = deck ^ combo.Hand;
                ret[pockets.Length + j] = combo.Hand;
            }
            return ret;
        }

        protected ulong GetRandomCards(ulong dead, int numCards)
        {
            ulong ret = 0;
            while (numCards > 0)
            {
                ulong card = 1ul << _rand.Next(52);
                while ((dead & card) > 1)
                    card = 1ul << _rand.Next(52);
                ret |= card;
                dead |= card;
                numCards--;
            }
            return ret;
        }

        /// <summary>
        /// Get the set of results and conditions relating to the given board and set of hands
        /// </summary>
        /// <param name="round"></param>
        /// <param name="numCards"></param>
        /// <param name="hands"></param>
        /// <param name="board"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected HandMask[] SetResults(int numCards, ulong[] hands, ulong board)
        {
            HandMask[] values = new HandMask[hands.Length];
            uint winningValue = 0;

            for (int i = 0; i < hands.Length; i++)
            {
                values[i] = MaskedEvaluator.Evaluate(board, hands[i], numCards + 2);
                if (values[i].Result > winningValue)
                    winningValue = values[i].Result;
            }

            for (int i = 0; i < hands.Length; i++)
                if (values[i].Result == winningValue)
                    values[i].IsWinner = true;
            return values;
        }

        protected struct HandProbability
        {
            public ulong Hand { get; set; }
            public int Probability { get; set; }
        }
    }
}
