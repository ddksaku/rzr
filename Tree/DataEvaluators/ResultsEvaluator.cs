using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Tree.DataEvaluators
{
    public class ResultsEvaluator: BetTreeDataEvaluator
    {
        public float[] WinAmounts { get; private set; }

        public ResultsEvaluator(float[] winAmounts)
        {
            WinAmounts = winAmounts;
        }

        public bool Evaluate(ulong[] hands, uint[] handIndex, ulong board, int numCardsDealt)
        {
            return true;
        }
    }
}
