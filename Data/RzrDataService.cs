using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Rzr.Core.Editors.HandRange;
using Rzr.Core.Calculator;
using Rzr.Core.Game;
using Rzr.Core.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Rzr.Core.Data
{
    /// <summary>
    /// Service class for handling data that can be modified by the user.
    /// 
    /// For example, hand range and flop range definitions can be updated by the user as the ranks can 
    /// be modified up or down. However, the master list of available conditions cannot be modified by the
    /// user and so these are handled by a separate service, in this case the ConditionService
    /// </summary>
    public class RzrDataService
    {
        #region singleton

        public static RzrDataService Service { get; private set; }

        protected RzrDataService() 
        {
            List<HoleCardRangeDefinition> ranges = new List<HoleCardRangeDefinition>();
            HoleCardRanges = ranges.AsReadOnly();
        }

        public static void Initialise()
        {
            Service = new RzrDataService();            
            CalculateProbabilities();
            Service.HoleCardRanges = GetHoleCardRanges();
            Service.HandValueDefs = GetHandValueDefinitions();
            Service.HandValueRanges = GetHandValueRanges();
        }

        private static void CalculateProbabilities()
        {
            Service.PROB_OFFSUIT_HOLECARDS = ((float)(8 * 3) / (float)(52 * 51)) * 100f;
            Service.PROB_PAIR_HOLECARDS = ((float)(4 * 3) / (float)(52 * 51)) * 100f;
            Service.PROB_SUITED_HOLECARDS = ((float)(8 * 1) / (float)(52 * 51)) * 100f;
        }

        private static ReadOnlyCollection<HoleCardRangeDefinition> GetHoleCardRanges()
        {
            List<Hook> hooks = RzrInit.InitFile.Hooks.FindAll(x => x.Type == RzrConfiguration.HOOK_RANGE);
            List<HoleCardRangeDefinition> defs = new List<HoleCardRangeDefinition>();
            foreach (Hook hook in hooks)
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(HoleCardRangesFile));
                string path = Path.Combine(RzrConfiguration.DataDirectory, hook.File);
                using (StreamReader reader = new StreamReader(path))
                {
                    HoleCardRangesFile file = serialiser.Deserialize(reader) as HoleCardRangesFile;
                    defs.AddRange(file.Ranges);
                }
            }
            return defs.AsReadOnly();
        }

        public void SaveHoleCardRange(HoleCardRangeDefinition def)
        {
            Hook hook = RzrInit.InitFile.Hooks.Find(x => x.Type == RzrConfiguration.HOOK_RANGE);

            HoleCardRangesFile file = null;

            XmlSerializer serialiser = new XmlSerializer(typeof(HoleCardRangesFile));
            string path = Path.Combine(RzrConfiguration.DataDirectory, hook.File);
            using (StreamReader reader = new StreamReader(path))
            {
                file = serialiser.Deserialize(reader) as HoleCardRangesFile;
            }

            if (def.ID == 0) def.ID = file.Ranges.Max(x => x.ID) + 1;

            file.Ranges.RemoveAll(x => x.ID == def.ID);
            file.Ranges.Add(def);
            file.Ranges = file.Ranges.OrderBy(x => x.Name).ToList();

            using (StreamWriter writer = new StreamWriter(path))
            {
                serialiser.Serialize(writer, file);
            }

            HoleCardRanges = file.Ranges.AsReadOnly();
        }

        public void DeleteHoleCardRange(string name)
        {
            Hook hook = RzrInit.InitFile.Hooks.Find(x => x.Type == RzrConfiguration.HOOK_RANGE);

            HoleCardRangesFile file = null;

            XmlSerializer serialiser = new XmlSerializer(typeof(HoleCardRangesFile));
            string path = Path.Combine(RzrConfiguration.DataDirectory, hook.File);
            using (StreamReader reader = new StreamReader(path))
            {
                file = serialiser.Deserialize(reader) as HoleCardRangesFile;
            }

            file.Ranges.RemoveAll(x => x.Name == name);

            using (StreamWriter writer = new StreamWriter(path))
            {
                serialiser.Serialize(writer, file);
            }

            HoleCardRanges = file.Ranges.AsReadOnly();
        }

        private static HandValueDefinitionList GetHandValueDefinitions()
        {
            List<Hook> hooks = RzrInit.InitFile.Hooks.FindAll(x => x.Type == RzrConfiguration.HOOK_HANDVALUE_DEF);

            HandValueDefinitionList ret = null;

            foreach (Hook hook in hooks)
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(HandValueDefinitionList));
                string path = Path.Combine(RzrConfiguration.DataDirectory, hook.File);
                using (StreamReader reader = new StreamReader(path))
                {
                    ret = serialiser.Deserialize(reader) as HandValueDefinitionList;                    
                }
            }

            return ret;
        }


        private static ReadOnlyCollection<HandValueRangeDefinition> GetHandValueRanges()
        {
            List<Hook> hooks = RzrInit.InitFile.Hooks.FindAll(x => x.Type == RzrConfiguration.HOOK_HANDVALUE_RANGE);
            List<HandValueRangeDefinition> defs = new List<HandValueRangeDefinition>();

            foreach (Hook hook in hooks)
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(HandValueRangeDefinition));
                string path = Path.Combine(RzrConfiguration.DataDirectory, hook.File);
                using (StreamReader reader = new StreamReader(path))
                {
                    HandValueRangeDefinition file = serialiser.Deserialize(reader) as HandValueRangeDefinition;
                    defs.Add(file);
                }
            }
            return defs.AsReadOnly();
        }

        #endregion

        #region properties

        public double PROB_OFFSUIT_HOLECARDS { get; private set; }
        public double PROB_PAIR_HOLECARDS { get; private set; }
        public double PROB_SUITED_HOLECARDS { get; private set; }
        public HandValueDefinitionList HandValueDefs { get; private set; }
        public ReadOnlyCollection<HoleCardRangeDefinition> HoleCardRanges { get; private set; }
        public ReadOnlyCollection<HandValueRangeDefinition> HandValueRanges { get; private set; }

        #endregion

        #region hand ranges

        public ObservableCollection<HandRangeItem> GetHandRangeItems()
        {
            ObservableCollection<HandRangeItem> items = new ObservableCollection<HandRangeItem>();
            foreach (CardDenomination i in CardDenomination.Denominations)
            {
                foreach (CardDenomination j in CardDenomination.Denominations)
                {
                    string description = GetDescription(i, j);
                    double probability = GetProbability(i.Value, j.Value);
                    items.Add(new HandRangeItem()
                    {
                        ID = Convert.ToString(i.Value + (j.Value * 13)),
                        Description = description,
                        Probability = probability,
                        XCord = i.Value,
                        YCord = j.Value
                    });
                }
            }
            return items;
        }

        public static bool IsSuited(int value1, int value2)
        {
            return (value1 > value2);
        }

        public static bool IsPaired(int value1, int value2)
        {
            return (value1 == value2);
        }

        private string GetDescription(CardDenomination card1, CardDenomination card2)
        {
            if (IsSuited(card1.Value, card2.Value))
                return card1.Char + card2.Char + "s";
            else if (IsPaired(card1.Value, card2.Value))
                return card1.Char + card2.Char;
            else 
                return card1.Char + card2.Char + "o";            
        }

        public static uint GetCardIndex(ulong hand)
        {
            uint sc = (uint)((hand >> (Values.CLUB_OFFSET)) & 0x1fffUL);
            uint sd = (uint)((hand >> (Values.DIAMOND_OFFSET)) & 0x1fffUL);
            uint sh = (uint)((hand >> (Values.HEART_OFFSET)) & 0x1fffUL);
            uint ss = (uint)((hand >> (Values.SPADE_OFFSET)) & 0x1fffUL);

            bool flush = (PreCalc.nBitsTable[sd] >= 2) ||
                         (PreCalc.nBitsTable[sd] >= 2) ||
                         (PreCalc.nBitsTable[sd] >= 2) ||
                         (PreCalc.nBitsTable[sd] >= 2);

            uint ranks = sc | sd | sh | ss;

            ushort card1 = PreCalc.topCardTable[ranks];
            ushort card2 = PreCalc.topCardTable[ranks ^ PreCalc.topCardMask[ranks]];

            return GetCardIndex(flush, card1, card2);
        }

        public static uint GetCardIndex(int card1, int card2)
        {
            int card1Denom = card1 % 13;
            int card2Denom = card2 % 13;

            int card1Suit = card1 / 13;
            int card2Suit = card2 / 13;

            return GetCardIndex(card1Suit == card2Suit, card1Denom, card2Denom);
        }

        public static uint GetCardIndex(bool suited, int card1Denom, int card2Denom)
        {
            if (card1Denom == card2Denom)
            {
                return (uint)(card1Denom + (card2Denom * 13));
            }
            else
            {
                if (suited)
                {
                    if (card1Denom > card2Denom)
                        return (uint)(card1Denom + card2Denom * 13);
                    else
                        return (uint)(card2Denom + card1Denom * 13);
                }
                else
                {
                    if (card1Denom < card2Denom)
                        return (uint)(card1Denom + card2Denom * 13);
                    else
                        return (uint)(card2Denom + card1Denom * 13);
                }
            }
        }

        private double GetProbability(int denom1, int denom2)
        {
            if (IsSuited(denom1, denom2))
                return PROB_SUITED_HOLECARDS;
            else if (IsPaired(denom1, denom2))
                return PROB_PAIR_HOLECARDS;
            else 
                return PROB_OFFSUIT_HOLECARDS;
            
        }

        public HandRangeModel GetHandRangeModel(HoleCardRangeDefinition definition)
        {
            HandRangeModel ret = new HandRangeModel();
            if (definition == null)
                definition = ret.SampleRanges.FirstOrDefault();

            ret.SelectedRangeRanking = definition;
            ret.VariationFactor = definition.DefaultVariation;
            ret.RangePercentage = definition.DefaultRange;
            ret.MaskPercentage = 0;

            SetHandRangeRankings(definition, ret.RangeItems);
            SetRangeDefinitionAbsolute<HandRangeItem>(ret.RangeItems, ret.MaskPercentage, ret.RangePercentage);
            SetRangeVariation<HandRangeItem>(ret.RangeItems, ret.MaskPercentage, ret.RangePercentage, ret.VariationFactor);

            return ret;

        }

        #endregion

        #region hand value range

        public ObservableCollection<HandValueRangeItem> GetHandValueRangeItems(ConditionService service, HoldemHandRound round, HandRange range, int columns)
        {
            ObservableCollection<HandValueRangeItem> items = new ObservableCollection<HandValueRangeItem>();

            List<CompiledCondition> conditions = new List<CompiledCondition>();
            foreach (CompiledCondition container in service.GetCompiledConditions(false))
                conditions.Add(container);

            int rows = (conditions.Count / columns);

            for (int i = 0; i < conditions.Count; i++)
            {
                items.Add(new HandValueRangeItem()
                {
                    ID = conditions[i].ID,
                    Mask = conditions[i],
                    Description = conditions[i].Name,
                    Probability = conditions[i].Probability * 100,
                    Rank = conditions.Count - i,
                    XCord = columns - 1 - (i % columns),
                    YCord = rows - (i / columns)
                });
            }

            return items;
        }

        private float[] GetHandValueProbabilities(List<CompiledCondition> conditions, HandRange range, HoldemHandRound round, float numIterations)
        {
            int numCards = ((int)round) + 2;
            float[] probabilities = new float[conditions.Count];

            for (int i = 0; i < numIterations; i++)
            {
                ulong deck = Deck.DeckLong;
                ulong hand = GetRandomHole(deck);                

                deck |= hand;
                ulong board = GetRandomBoard(deck, numCards);

                HandMask mask = MaskedEvaluator.Evaluate(board, hand, numCards + 2);

                for (int j = 0; j < conditions.Count; j++)
                {
                    if (conditions[j].Matches(mask))
                    {
                        probabilities[j]++;
                        break;
                    }
                }
            }

            return probabilities;
        }

        private ulong GetRandomBoard(ulong deck, int numCards)
        {
            ulong cards = 0;
            for (int i = 0; i < numCards; i++)
            {
                int cardValue = _rand.Next(52);
                while ((deck & 1UL << cardValue) == 0)
                    cardValue = _rand.Next(52);
                cards |= 1ul << cardValue;
                deck ^= cards;
            }
            return cards;
        }

        Random _rand = new Random();

        private ulong GetRandomHole(ulong deck)
        {
            int card1Value = _rand.Next(52);
            while ((deck & 1UL << card1Value) == 0)
                card1Value = _rand.Next(52);
            ulong cards = 1ul << card1Value;
            deck ^= cards;

            int card2Value = _rand.Next(52);
            while ((deck & 1UL << card2Value) == 0)
                card2Value = _rand.Next(52);
            cards |= 1ul << card2Value;

            return cards;
        }


        #endregion

        #region calc

        public static void SetHandRangeRankings(HoleCardRangeDefinition definition, IEnumerable<HandRangeItem> items)
        {
            foreach (HandRangeItem item in items)
            {
                HandDefinition def = definition.Hands.Find(x => x.HandDef == item.ID);
                if (def != null)                    
                    item.Rank = def.Value;
            }
        }

        public static void SetRangeDefinitionAbsolute<T>(IEnumerable<T> items, float startRange, float endRange) where T : RangeDataItem
        {
            if (startRange > endRange) startRange = endRange;

            double totalRange = 0;
            foreach (T item in items.OrderByDescending(x => x.Rank))
            {
                if (totalRange >= endRange || totalRange < startRange)
                {
                    item.Weight = 0;
                }
                else
                {
                    item.Weight = 100;
                }
                totalRange += item.Probability;
            }
        }

        /// <summary>
        /// Modifies the range to add variation, according to a sliding variation factor
        /// </summary>
        public static void SetRangeVariation<T>(IEnumerable<T> items, float startRange, float endRange, float variation) where T : RangeDataItem
        {
            if (startRange > endRange) startRange = endRange;

            //-------------------------------------------------------------------------------------
            // Calculate where to start and end the range adjustments
            //-------------------------------------------------------------------------------------
            float midPoint = (endRange + startRange) / 2;

            float startPoint = endRange - ((endRange - midPoint) * (variation / 100));
            float endPoint = endRange + ((100 - endRange) * (variation / 100));
            startPoint = Math.Max(startPoint, endRange - (100 - endRange));
            List<T> endRangeItems = GetItemsBetween<T>(items, startPoint, endPoint);

            startPoint = startRange - (startRange * (variation / 100));
            endPoint = startRange + ((midPoint - startRange) * (variation / 100));
            endPoint = Math.Min(endPoint, startRange * 2);
            List<T> startRangeItems = GetItemsBetween<T>(items, startPoint, endPoint);
                        
            ApplyWeighting<T>(endRangeItems);
            ApplyWeighting<T>(startRangeItems);

            Rebalance<T>(items.OrderBy(x => x.Rank).ToList(), endRange - startRange);
        }

        private static List<T> GetItemsBetween<T>(IEnumerable<T> items, float startPoint, float endPoint) where T : RangeDataItem
        {
            List<T> rangeItems = new List<T>();
            double rangeSum = 0;
            foreach (T item in items.OrderByDescending(x => x.Rank))
            {
                if (rangeSum >= startPoint && rangeSum < endPoint)
                    rangeItems.Add(item);
                rangeSum += item.Probability;
            }
            return rangeItems;
        }

        private static void ApplyWeighting<T>(List<T> rangeItems) where T : RangeDataItem
        {
            for (int i = 0; i < rangeItems.Count; i++)
            {
                double factorRoot = (double)((i * 2) - rangeItems.Count) / (double)rangeItems.Count;
                double weightingFactor = Math.Pow(factorRoot, 3d);
                double multiplier = 1d - ((weightingFactor + 1) / 2d);
                rangeItems[i].Weight = (float)(100 * multiplier);
            }
        }

        private static void Rebalance<T>(List<T> items, float requiredRange) where T : RangeDataItem
        {
            double rangeSum = items.Sum(x => x.Probability * x.Weight);
            int index = 0;
            requiredRange = requiredRange * 100;

            if (rangeSum > requiredRange)
            {
                while (rangeSum > requiredRange && index < items.Count)
                {
                    T item = items[index];

                    if (item.Weight == 0)
                    {
                        index++;
                        continue;
                    }

                    double weight = item.Weight * item.Probability;
                    if (rangeSum - weight > requiredRange)
                    {
                        rangeSum -= weight;
                        item.Weight = 0;
                    }
                    else
                    {
                        double reduction = (rangeSum - requiredRange) / items[index].Probability;
                        item.Weight -= (float)reduction;
                        if (item.Weight < 0) item.Weight = 0;
                        break;
                    }
                    index++;
                }
            }
            else
            {

            }
        }

        public static void CheckRangeValid<T>(IEnumerable<T> definitions) where T : RangeDataItem
        {
            //-------------------------------------------------------------------------------------
            // Check that the data is valid
            //-------------------------------------------------------------------------------------
            double totalProbability = Math.Round(definitions.Sum(x => x.Probability), 2);
            if (totalProbability != 100)
                throw new InvalidOperationException("Total probability of " + totalProbability + "% is not valid");

            double minWeight = definitions.Min(x => x.Weight);
            if (minWeight < 0)
                throw new InvalidOperationException("Cannot have negative weight: " + minWeight);

            double maxWeight = definitions.Max(x => x.Weight);
            if (maxWeight > 100)
                throw new InvalidOperationException("Cannot have weight greater than 100: " + maxWeight);
        }

        #endregion
    }
}
