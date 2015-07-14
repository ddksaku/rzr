using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Rzr.Core.Tree
{
    public class HandSnapshotModel
    {
        #region properties

        public HoldemHandRound Round { get; private set; }

        public int Button { get; private set; }

        public int NumPlayersInHand { get; private set; }

        public int NumPlayersActive { get; private set; }

        public bool IsHandEnd { get; private set; }

        public bool IsRoundEnd { get; private set; }

        public int? CurrentPlayer { get; private set; }

        public int? NextPlayer { get; private set; }

        public bool[] Active { get; private set; }

        public ActiveStatus[] Status { get; private set; }

        public float[] Stacks { get; private set; }

        public float[] Bets { get; private set; }

        #endregion

        public HandSnapshotModel(HoldemHandRound round, int button, bool[] active, ActiveStatus[] status, float[] bets, float[] stacks, int? currentPlayer)
        {
            Round = round;
            Button = button;
            Active = active;
            Stacks = stacks;
            Bets = bets;
            Status = status;
            CurrentPlayer = currentPlayer;

            for (int i = 0; i < active.Length; i++)
            {
                if (!active[i])
                {
                    Status[i] = ActiveStatus.HasFolded;
                    Bets[i] = 0;
                }
            }

            int start = CurrentPlayer != null ? (int)CurrentPlayer : BetPolicyService.GetRoundStart(round, button, stacks.Length);
            NextPlayer = BetPolicyService.GetNextActivePlayer(status, bets, start);

            NumPlayersInHand = status.Count(x => x != ActiveStatus.HasFolded);
            NumPlayersActive = status.Count(x => x != ActiveStatus.HasFolded && x != ActiveStatus.AllIn);
            IsRoundEnd = NextPlayer == null;
            IsHandEnd = NumPlayersActive == 0 || NumPlayersInHand == 1 || IsRoundEnd && round == HoldemHandRound.River;
        }

        public ActiveStatus[] GetStatus()
        {
            return (ActiveStatus[])Status.Clone();
        }

        public float[] GetBets()
        {
            return (float[])Bets.Clone();
        }

        public float[] GetStacks()
        {
            return (float[])Stacks.Clone();
        }

        public bool[] GetActive()
        {
            return (bool[])Active.Clone();
        }

        public int? GetNextRoundStart()
        {
            int round = BetPolicyService.GetRoundStart(((HoldemHandRound)((int)Round) + 1), Button, Status.Length);
            ActiveStatus[] newStatus = Status.Select(x => GetStatusForNextRound(x)).ToArray();
            return BetPolicyService.GetNextActivePlayer(newStatus, Bets, round);
        }

        private ActiveStatus GetStatusForNextRound(ActiveStatus previousStatus)
        {
            if (previousStatus == ActiveStatus.AllIn) return ActiveStatus.AllIn;
            else if (previousStatus == ActiveStatus.HasFolded) return ActiveStatus.HasFolded;
            else return ActiveStatus.StillToBet;
        }
    }
}
