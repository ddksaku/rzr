using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core
{
    public enum HoldemHandRound
    {
        PreFlop = 0,
        Flop = 1,
        Turn = 2,
        River = 3
    }

    public enum Direction
    {
        Up, Right, Down, Left
    }

    public enum LogicalEnum
    {
        OR = 0,
        AND = 1        
    }

    public enum BetAction
    {
        Fold,
        Check,
        Call,
        Bet,
        Raise,
        AllIn
    }

    public enum ActiveStatus
    {
        HasFolded,
        StillToBet,
        HasBet,        
        LastToRaise,
        AllIn
    }
}
