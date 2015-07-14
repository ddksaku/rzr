using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Calculator;

namespace Rzr.Core.Tree.DataEvaluators
{
    public class PostflopEvaluator : BetTreeDataEvaluator
    {
        protected static Random _rand = new Random();

        public int PlayerIndex { get; private set; }

        public HandValueRange Range { get; private set; }

        public PostflopEvaluator(int playerIndex, HandValueRange range)
        {
            PlayerIndex = playerIndex;
            Range = range;
        }

        public bool Evaluate(ulong[] hands, uint[] handIndex, ulong board, int numCardsDealt)
        {
            HandMask mask = MaskedEvaluator.Evaluate(board, hands[PlayerIndex], numCardsDealt + 2);

            for (int i = 0; i < Range.Mask.Length; i++)
            {
                double prob = Range.Probability[i];
                if (prob > 0)
                {
                    if (Range.Mask[i].Matches(mask))
                    {
                        return (_rand.Next(100) < prob);
                    }
                }
            }

            return false;
        }
    }
}
