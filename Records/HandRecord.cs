using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Records
{
    public class HandRecord
    {
        public string ID { get; set; }

        public string Winner { get; set; }

        public HoldemHandRound Round { get; set; }

        public float PotAmount { get; set; }

        public float WinAmount { get; set; }

        public int NumPlayersSeeingFlop { get; set; }

        public BetRecord[] Bets { get; set; }
    }
}
