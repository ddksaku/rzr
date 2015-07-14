using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using Rzr.Core.Game;

namespace Rzr.Core.Calculator
{
    public class WebCalculatorService
    {
        public static bool[] GetPockets(string[] keys, List<ulong> pockets, NameValueCollection form)
        {
            bool[] active = new bool[keys.Length];

            for (int i = 0; i < keys.Length; i++)
            {
                string range = form[keys[i]];
                ulong? pocket = GetPocket(range);
                if (pocket != null)
                    pockets.Add((ulong)pocket);
                active[i] = pocket != null;

            }
            return active;
        }

        protected static ulong? GetPocket(string range)
        {
            JavaScriptSerializer serialiser = new JavaScriptSerializer();            
            if (String.IsNullOrEmpty(range)) return null;

            int[] json = (int[])serialiser.Deserialize(range, typeof(int[]));
            if (json.Length != 2) return null;

            ulong ret = 0ul;
            foreach (int val in json)
                ret |= 1ul << val;
            return ret;
        }

        public static bool[] GetRanges(string[] keys, List<HandRange> ranges, NameValueCollection form)
        {
            bool[] active = new bool[keys.Length];

            for (int i = 0; i < keys.Length; i++)
            {
                string rangeKey = form[keys[i]];
                HandRange range = GetRange(rangeKey);
                if (range != null)
                {
                    active[i] = true;
                    ranges.Add(range);
                }
            }

            return active;
        }

        protected static HandRange GetRange(string range)
        {
            JavaScriptSerializer serialiser = new JavaScriptSerializer();
            if (String.IsNullOrEmpty(range)) return null;

            RangeJson json = (RangeJson)serialiser.Deserialize(range, typeof(RangeJson));
            HandRange ret = new HandRange();
            int[] vals = new int[169];
            foreach (RangeJsonItem item in json.RangeData)
                vals[item.val] = (int)(item.selected * 100);
            ret.SetProbability(vals);
            return ret;
        }

        public static Card[] GetBoard(string key, NameValueCollection form)
        {
            string range = form[key];
            JavaScriptSerializer serialiser = new JavaScriptSerializer();

            int[] json = (int[])serialiser.Deserialize(range, typeof(int[]));
            Card[] ret = new Card[5];
            for (int i = 0; i < 5 && json != null; i++)
                if (json.Length > i)
                    ret[i] = new Card(json[i]);
            return ret;
        }

        public static List<RangeCalculatorResult> GetResults(int numPlayers, List<CompiledCondition> conditions)
        {
            List<RangeCalculatorResult> results = new List<RangeCalculatorResult>();
            foreach (CompiledCondition condition in conditions)
            {
                for (int player = 0; player < numPlayers; player++)
                {
                    results.Add(new RangeCalculatorResult() { Condition = condition, PlayerIndex = player, Round = HoldemHandRound.Flop });
                    results.Add(new RangeCalculatorResult() { Condition = condition, PlayerIndex = player, Round = HoldemHandRound.Turn });
                    results.Add(new RangeCalculatorResult() { Condition = condition, PlayerIndex = player, Round = HoldemHandRound.River });
                }
            }
            return results;
        }

        public static void CompileResults(HandMask[][][] masks, int numHands, List<RangeCalculatorResult> results)
        {
            for (int hand = 0; hand < numHands; hand++)
            {
                foreach (RangeCalculatorResult result in results)
                {
                    int round = ((int)result.Round) - 1;
                    HandMask mask = masks[hand][round][result.PlayerIndex];
                    if (result.Condition.Matches(mask))
                    {
                        result.Count++;
                        /*
                        if ((mask.Mask & MaskedEvaluator.HANDCONDITION_ISWINNER) > 0)
                            result.WinCount++;
                         */
                    }
                    result.Total++;
                }
            }
        }
    }

    [Serializable()]
    public class RangeJson
    {
        public RangeJsonItem[] RangeData { get; set; }
    }

    [Serializable()]
    public class RangeJsonItem
    {
        public int val { get; set; }

        public float selected { get; set; }
    }

    public struct WebCalculatorResult
    {
        public string Condition { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string Player3 { get; set; }
        public string Player4 { get; set; }
    }
}
