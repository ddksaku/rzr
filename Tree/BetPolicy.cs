using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Rzr.Core.Tree
{
    public class BetPolicy
    {
        public float Required { get; set; }

        public HandSnapshotModel Parent { get; private set; }

        public float MinBet { get; private set; }

        public float MaxBet { get; private set; }

        public float MinCall { get; private set; }

        public List<BetAction> AllowedChildActions { get; set; }

        public BetPolicy(HandSnapshotModel parentModel)
        {
            Parent = parentModel;
            AllowedChildActions = new List<BetAction>();

            HoldemHandRound round = Parent.NextPlayer == null ? (HoldemHandRound)((int)Parent.Round) + 1 : Parent.Round;
            int? index = Parent.NextPlayer == null ? parentModel.GetNextRoundStart() : Parent.NextPlayer;

            if (index != null)
            {
                AddFigures((int)index);
                AddOptions((int)index);
            }
        }

        private void AddFigures(int index)
        {
            Required = Parent.Bets.Max() - Parent.Bets[index];

            MinCall = Required;
            MinBet = Required * 2;
            MaxBet = Parent.Stacks[index] - Parent.Bets[index];
        }

        private void AddOptions(int index)
        {
            if (Parent.IsHandEnd) return;

            float maxBet = Parent.Bets.Max();

            if (Parent.Stacks[index] < maxBet)
            {
                AllowedChildActions.Add(BetAction.AllIn);
            }
            else if (Parent.Bets[index] < maxBet)
            {
                AllowedChildActions.Add(BetAction.Fold);
                AllowedChildActions.Add(BetAction.Call);
                AllowedChildActions.Add(BetAction.Raise);
            }
            else
            {
                AllowedChildActions.Add(BetAction.Check);
                AllowedChildActions.Add(BetAction.Bet);
            }
        }
    }
}
