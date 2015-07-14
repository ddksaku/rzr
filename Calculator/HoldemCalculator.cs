using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Rzr.Core.Calculator
{
    /// <summary>
    /// Handles the probability calculation for a single hand of Texas Hold'em.
    /// </summary>
    public class HoldemCalculator
    {
        public static void HandOdds(ulong[] masks, ulong board, long[] wins, long[] ties, long[] losses, ref long totalHands)
        {
            //-------------------------------------------------------------------------------------
            // Initialise variables
            //-------------------------------------------------------------------------------------
            ulong[] pockets = new ulong[masks.Length];
            int bestcount;
            ulong deadcards_mask = 0UL;                       

            //-------------------------------------------------------------------------------------
            // Read pocket cards
            //-------------------------------------------------------------------------------------
            for (int i = 0; i < masks.Length; i++)
            {
                deadcards_mask |= masks[i];                
            }

            //-------------------------------------------------------------------------------------
            // Iterate through every possible board, given the specified dead cards
            //-------------------------------------------------------------------------------------

            foreach (ulong boardhand in Iterator.Hands(board, deadcards_mask, 5))
            {
                //-------------------------------------------------------------------------------------
                // For each hand, evaluate and determine the best pocket
                //-------------------------------------------------------------------------------------                
                ulong bestpocket = Evaluator.Evaluate(masks[0] | boardhand, 7);
                pockets[0] = bestpocket;
                bestcount = 1;
                for (int i = 1; i < masks.Length; i++)
                {
                    pockets[i] = Evaluator.Evaluate(masks[i] | boardhand, 7);
                    if (pockets[i] > bestpocket)
                    {
                        bestpocket = pockets[i];
                        bestcount = 1;
                    }
                    else if (pockets[i] == bestpocket)
                    {
                        bestcount++;
                    }
                }

                //-------------------------------------------------------------------------------------
                // Calculate wins/ties/losses for each combination.
                //-------------------------------------------------------------------------------------
                for (int i = 0; i < masks.Length; i++)
                {
                    if (pockets[i] == bestpocket)
                    {
                        if (bestcount > 1)
                        {
                            ties[i]++;
                        }
                        else
                        {
                            wins[i]++;
                        }
                    }
                    else if (pockets[i] < bestpocket)
                    {
                        losses[i]++;
                    }
                }

                totalHands++;
            }
        }
    }
}
