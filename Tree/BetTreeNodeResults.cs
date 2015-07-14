using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Tree
{
    public class BetTreeNodeResults
    {
        public int TotalCount { get; set; }

        public int Count { get; set; }

        public float[] WinAmounts { get; set; }

        public BetTreeNodeResults(int numPlayers)
        {
            WinAmounts = new float[numPlayers];
        }

        public void RecordWinAmounts(float[] amounts)
        {
            for (int i = 0; i < amounts.Length; i++)
            {
                WinAmounts[i] += amounts[i];
            }
        }

        public void AggregateWinAmounts()
        {
            for (int i = 0; i < WinAmounts.Length; i++)
            {
                if (Count == 0) 
                    WinAmounts[i] = 0;
                else
                    WinAmounts[i] = WinAmounts[i] / (float)Count;
            }
        }

    }
}
