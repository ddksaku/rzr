using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Calculator;

namespace Rzr.Core.Tree.DataEvaluators
{
    public class PreflopEvaluator : BetTreeDataEvaluator
    {
        protected static Random _rand = new Random();

        public int PlayerIndex { get; private set; }

        public HandRange Range { get; private set; }

        public PreflopEvaluator(int playerIndex, HandRange range)
        {
            PlayerIndex = playerIndex;
            Range = range;
        }

        public bool Evaluate(ulong[] hands, uint[] handIndex, ulong board, int numCardsDealt)
        {
            float prob = Range.Probability[handIndex[PlayerIndex]];
            if (prob > 0)
            {
                return (_rand.Next(100) < prob);
            }
            return false;
        }
    }
}
