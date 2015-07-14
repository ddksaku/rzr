using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using Rzr.Core.Calculator;

namespace Rzr.Core.Data
{
    public class HandValueRangeModel : DependencyObject
    {
        public const int COLUMNS = 3;
        
        #region property declarations

        public static readonly DependencyProperty RangeItemsProperty =
            DependencyProperty.Register("RangeLevel", typeof(ObservableCollection<HandValueRangeItem>), typeof(HandValueRangeModel), new PropertyMetadata(null, OnRangeItemChanged));

        public static readonly DependencyProperty RangePercentageProperty =
            DependencyProperty.Register("RangePercentage", typeof(float), typeof(HandValueRangeModel), new PropertyMetadata(20f, OnSelectedRangeChanged));

        public static readonly DependencyProperty MaskPercentageProperty =
            DependencyProperty.Register("MaskPercentage", typeof(float), typeof(HandValueRangeModel), new PropertyMetadata(0f, OnSelectedRangeChanged));

        public static readonly DependencyProperty VariationFactorProperty =
            DependencyProperty.Register("VariationFactor", typeof(float), typeof(HandValueRangeModel), new PropertyMetadata(0f, OnSelectedRangeChanged));

        public static readonly DependencyProperty SelectedRangeRankingProperty =
            DependencyProperty.Register("SelectedRangeRanking", typeof(HandValueRangeDefinition), typeof(HandValueRangeModel), new PropertyMetadata(null, OnSelectedRangeChanged));

        public static readonly DependencyProperty SelectedConditionSetProperty =
            DependencyProperty.Register("SelectedConditionSet", typeof(ConditionService), typeof(HandValueRangeModel), new PropertyMetadata(null, OnSelectedConditionsChanged));

        #endregion

        #region dependency properties

        public ObservableCollection<HandValueRangeItem> RangeItems
        {
            get { return (ObservableCollection<HandValueRangeItem>)this.GetValue(RangeItemsProperty); }
            set { this.SetValue(RangeItemsProperty, value); }
        }

        public float RangePercentage
        {
            get { return (float)this.GetValue(RangePercentageProperty); }
            set { this.SetValue(RangePercentageProperty, value); }
        }

        public float MaskPercentage
        {
            get { return (float)this.GetValue(MaskPercentageProperty); }
            set { this.SetValue(MaskPercentageProperty, value); }
        }

        public float VariationFactor
        {
            get { return (float)this.GetValue(VariationFactorProperty); }
            set { this.SetValue(VariationFactorProperty, value); }
        }

        public HandValueRangeDefinition SelectedRangeRanking
        {
            get { return (HandValueRangeDefinition)this.GetValue(SelectedRangeRankingProperty); }
            set { this.SetValue(SelectedRangeRankingProperty, value); }
        }

        public ConditionService SelectedConditionSet
        {
            get { return (ConditionService)this.GetValue(SelectedConditionSetProperty); }
            set { this.SetValue(SelectedConditionSetProperty, value); }
        }

        #endregion

        #region other properties

        public HandRange HandRange { get; private set; }

        public HoldemHandRound Round { get; private set; }

        public HandValueRange Range { get; private set; }

        public List<HandValueRangeDefinition> SampleRanges { get; private set; }

        public List<ConditionService> ConditionSets { get; private set; }

        protected bool _loading = false;

        #endregion

        #region events

        protected static void OnRangeItemChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HandValueRangeModel model = sender as HandValueRangeModel;
            if (model != null) model.SetRange();
        }

        protected static void OnSelectedConditionsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HandValueRangeModel model = sender as HandValueRangeModel;
            if (model != null) model.SetConditions();
        }

        /// <summary>
        /// Fired when the calculation used when automatically generating the range is fired
        /// </summary>
        protected static void OnSelectedRangeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HandValueRangeModel model = sender as HandValueRangeModel;
            if (model != null) model.SetRange();
        }

        public event DependencyPropertyChangedEventHandler RangeChanged;

        public event DependencyPropertyChangedEventHandler ConditionsChanged;

        #endregion

        #region initialise

        /// <summary>
        /// Constructor
        /// </summary>
        public HandValueRangeModel(HoldemHandRound round, HandRange range)
        {
            Round = round;
            HandRange = range;

            List<ConditionService> conditions = new List<ConditionService>();
            conditions.Add(ConditionService.DefaultService);
            conditions.AddRange(ConditionService.AvailableConditionSets.Select(x => new ConditionService(x, true)));
            ConditionSets = conditions;
            SelectedConditionSet = ConditionService.DefaultService;

            SampleRanges = RzrDataService.Service.HandValueRanges.Where(x => 
                x.Round == round && x.ConditionSet == SelectedConditionSet.Name).ToList();
            if (SampleRanges.Count > 0)
                SelectedRangeRanking = SampleRanges[0];

            UpdateRange();
        }

        #endregion

        protected void SetConditions()
        {
            RangeItems = RzrDataService.Service.GetHandValueRangeItems(SelectedConditionSet, Round, HandRange, COLUMNS);
            Range = new HandValueRange(RangeItems.Select(x => x.Mask).ToArray());

            if (ConditionsChanged != null) ConditionsChanged(this, new DependencyPropertyChangedEventArgs());
        }

        protected void SetRange()
        {
            if (_loading) return;

            RzrDataService.SetRangeDefinitionAbsolute<HandValueRangeItem>(RangeItems, MaskPercentage, RangePercentage);
            RzrDataService.SetRangeVariation<HandValueRangeItem>(RangeItems, MaskPercentage, RangePercentage, VariationFactor);

            UpdateRange();

            if (RangeChanged != null) RangeChanged(this, new DependencyPropertyChangedEventArgs());
        }

        protected void UpdateRange()
        {
            if (Range == null)
                Range = new HandValueRange(RangeItems.Select(x => x.Mask).ToArray());
            Range.Probability = RangeItems.OrderBy(x => x.ID).Select(x => (int)x.Weight).ToArray();
        }

        // Add method to load relevant hand range

        public void LoadFromString(string range)
        {
            _loading = true;

            string[] parts = range.Split(';');
            RangePercentage = Convert.ToSingle(parts[0]);
            MaskPercentage = Convert.ToSingle(parts[1]);
            VariationFactor = Convert.ToSingle(parts[2]);

            string[] probs = parts[3].Split(',');
            for (int i = 0; i < Range.Probability.Length; i++)
            {
                try
                {
                    Range.Probability[i] = Convert.ToInt32(probs[i]);
                }
                catch
                {
                }
            }

            _loading = false;
        }

        public string SaveToString()
        {
            StringBuilder ret = new StringBuilder();

            ret.Append(RangePercentage + ";");
            ret.Append(MaskPercentage + ";");
            ret.Append(VariationFactor + ";");

            bool first = true;
            for (int i = 0; i < Range.Probability.Length; i++)
            {
                if (!first) ret.Append(",");
                first = false;
                ret.Append(Range.Probability[i]);
            }

            return ret.ToString();
        }
    }
}
