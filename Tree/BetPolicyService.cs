using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Tree
{
    public class BetPolicyService
    {
        public static HandSnapshotModel GetSnapshot(HandSnapshotModel lastAction, BetAction action, float amount)
        {
            if (lastAction.NextPlayer != null)
            {
                ActiveStatus[] status = lastAction.GetStatus();
                float[] bets = lastAction.GetBets();
                float[] stacks = lastAction.GetStacks();
                bool[] active = lastAction.GetActive();
                ApplyBet(action, (int)lastAction.NextPlayer, amount, status, bets, stacks);
                return new HandSnapshotModel(lastAction.Round, lastAction.Button, active, status, bets, stacks, lastAction.NextPlayer);
            }
            else
            {
                if (lastAction.IsHandEnd)
                {
                    return lastAction;
                }
                else
                {
                    HoldemHandRound round = (HoldemHandRound)(((int)lastAction.Round) + 1);
                    ActiveStatus[] status = GetNextRoundStatus(lastAction.GetStatus());
                    float[] bets = lastAction.GetBets();
                    float[] stacks = lastAction.GetStacks();
                    bool[] active = lastAction.GetActive();   
                    int start = BetPolicyService.GetRoundStart(round, lastAction.Button, status.Length);
                    start = (int)GetNextActivePlayer(status, bets, (int)round);
                    ApplyBet(action, start, amount, status, bets, stacks);
                    return new HandSnapshotModel(round, lastAction.Button, active, status, bets, stacks, start);
                }
            }
        }

        private static ActiveStatus[] GetNextRoundStatus(ActiveStatus[] activeStatus)
        {
            ActiveStatus[] status = activeStatus.Select(x =>
                (x == ActiveStatus.HasFolded || x == ActiveStatus.AllIn) ? x : ActiveStatus.StillToBet).ToArray();
            return status;
        }

        /// <summary>
        /// Applies a bet and adjusts the players status according to the bet
        /// </summary>
        private static void ApplyBet(BetAction action, int playerIndex, float amount, ActiveStatus[] status, float[] bets, float[] stacks)
        {
            switch (action)
            {
                case BetAction.Bet:
                case BetAction.Raise:
                    bets[playerIndex] += amount;
                    for (int i = 0; i < status.Length; i++)
                        if (status[i] == ActiveStatus.LastToRaise)
                            status[i] = ActiveStatus.HasBet;
                    status[playerIndex] = ActiveStatus.LastToRaise;
                    break;
                case BetAction.Call:
                    bets[playerIndex] += amount;
                    status[playerIndex] = ActiveStatus.HasBet;
                    break;
                case BetAction.Check:
                    status[playerIndex] = ActiveStatus.HasBet;
                    break;
                case BetAction.Fold:
                    status[playerIndex] = ActiveStatus.HasFolded;
                    break;
                case BetAction.AllIn:
                    bets[playerIndex] += amount;
                    for (int i = 0; i < status.Length; i++)
                        if (status[i] == ActiveStatus.LastToRaise)
                            status[i] = ActiveStatus.HasBet;
                    status[playerIndex] = ActiveStatus.AllIn;
                    break;
            }

            if (bets[playerIndex] == stacks[playerIndex])
                status[playerIndex] = ActiveStatus.AllIn;
        }

        /// <summary>
        /// Get the next active player
        /// </summary>
        /// <param name="status"></param>
        /// <param name="amounts"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static int? GetNextActivePlayer(ActiveStatus[] status, float[] amounts, int start)
        {
            int index = start;

            //-------------------------------------------------------------------------------------
            // If there is a player who has yet to play, then the first player left of the start
            // player who has yet to play is the next player to play
            //-------------------------------------------------------------------------------------
            if (status.Contains(ActiveStatus.StillToBet))
            {
                while (status[index] != ActiveStatus.StillToBet)
                {
                    if (status[index] == ActiveStatus.StillToBet)
                        return index;

                    index = (index + 1) % status.Length;
                }
                return index;
            }
            //-------------------------------------------------------------------------------------
            // Otherwise, if another player has raised, then the first player after that player
            // who is still active and has not met that value is the next to play
            //-------------------------------------------------------------------------------------
            else if (status.Contains(ActiveStatus.LastToRaise))
            {
                while (status[index] != ActiveStatus.LastToRaise)
                {
                    index = (index + 1) % status.Length;
                }

                float amount = amounts[index];
                int startIndex = index;
                while (!NeedsToBet(status, amounts, amount, index))
                {                    
                    index = (index + 1) % status.Length;
                    if (index == startIndex) break;
                }

                return status[index] == ActiveStatus.LastToRaise ? null : (int?)index;
            }
            //-------------------------------------------------------------------------------------
            // Otherwise, if there is an all in player, check that nobody else needs to call
            //-------------------------------------------------------------------------------------
            else if (status.Contains(ActiveStatus.AllIn))
            {
                int startIndex = index;
                while (status[index] != ActiveStatus.AllIn)
                {
                    index = (index + 1) % status.Length;
                    if (index == startIndex) break;
                }

                float amount = amounts[index];
                while (!NeedsToBet(status, amounts, amount, index))
                {
                    index = (index + 1) % status.Length;
                    if (index == startIndex) break;
                }
                return (status[index] == ActiveStatus.AllIn || status[index] == ActiveStatus.HasFolded) ? null : (int?)index;
            }

            return null;
        }

        /// <summary>
        /// Returns true if the player at the designated index needs to bet more chips to stay in the hand
        /// </summary>
        private static bool NeedsToBet(ActiveStatus[] status, float[] amounts, float amount, int index)
        {
            bool needsToBet = amounts[index] < amount;
            needsToBet &= status[index] != ActiveStatus.HasFolded;
            needsToBet &= status[index] != ActiveStatus.AllIn;
            return needsToBet;
        }

        public static int GetRoundStart(HoldemHandRound round, int button, int numPlayers)
        {
            return round == HoldemHandRound.PreFlop ? (button + 3) % numPlayers : (button + 1) % numPlayers;
        }
    }
}
