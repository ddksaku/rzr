using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Rzr.Core.Calculator;
using System.Collections.ObjectModel;
using Rzr.Core.Data;

namespace Rzr.Core.Data
{
    public class HandRangeModel : DependencyObject
    {
        #region property declarations

        public static readonly DependencyProperty RangeItemsProperty =
            DependencyProperty.Register("RangeLevel", typeof(ObservableCollection<HandRangeItem>), typeof(HandRangeModel), new PropertyMetadata(null, OnRangeItemChanged));

        public static readonly DependencyProperty RangePercentageProperty =
            DependencyProperty.Register("RangePercentage", typeof(float), typeof(HandRangeModel), new PropertyMetadata(20f, OnSelectedRangeChanged));

        public static readonly DependencyProperty MaskPercentageProperty =
            DependencyProperty.Register("MaskPercentage", typeof(float), typeof(HandRangeModel), new PropertyMetadata(0f, OnSelectedRangeChanged));

        public static readonly DependencyProperty VariationFactorProperty =
            DependencyProperty.Register("VariationFactor", typeof(float), typeof(HandRangeModel), new PropertyMetadata(0f, OnSelectedRangeChanged));

        public static readonly DependencyProperty SelectedRangeRankingProperty =
            DependencyProperty.Register("SelectedRangeRanking", typeof(HoleCardRangeDefinition), typeof(HandRangeModel), new PropertyMetadata(null, OnSelectedRangeChanged));

        public static readonly DependencyProperty SampleRangesProperty =
            DependencyProperty.Register("SampleRanges", typeof(List<HoleCardRangeDefinition>), typeof(HandRangeModel), new PropertyMetadata(null, null));

        #endregion

        #region dependency properties

        public ObservableCollection<HandRangeItem> RangeItems
        {
            get { return (ObservableCollection<HandRangeItem>)this.GetValue(RangeItemsProperty); }
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

        public HoleCardRangeDefinition SelectedRangeRanking
        {
            get { return (HoleCardRangeDefinition)this.GetValue(SelectedRangeRankingProperty); }
            set { this.SetValue(SelectedRangeRankingProperty, value); }
        }

        public List<HoleCardRangeDefinition> SampleRanges
        {
            get { return (List<HoleCardRangeDefinition>)this.GetValue(SampleRangesProperty); }
            set { this.SetValue(SampleRangesProperty, value); }
        }

        #endregion

        #region other properties

        public Rzr.Core.Calculator.HandRange Range { get; private set; }

        protected bool _loading = false;

        #endregion

        #region events

        protected static void OnRangeItemChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HandRangeModel model = sender as HandRangeModel;
            if (model != null) model.SetRange();
        }

        /// <summary>
        /// Fired when the calculation used when automatically generating the range is fired
        /// </summary>
        protected static void OnSelectedRangeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HandRangeModel model = sender as HandRangeModel;
            if (model != null) model.SetRange();
        }

        public event DependencyPropertyChangedEventHandler RangeChanged;

        #endregion

        #region initialise

        /// <summary>
        /// Constructor
        /// </summary>
        public HandRangeModel()
        {
            SampleRanges = RzrDataService.Service.HoleCardRanges.ToList();
            RangeItems = RzrDataService.Service.GetHandRangeItems();
            Range = new HandRange();

            if (SampleRanges.Count > 0)
                SelectedRangeRanking = SampleRanges[0];

            UpdateRange();
        }

        #endregion

        protected void SetRange()
        {
            if (_loading || SelectedRangeRanking == null) return;

            RzrDataService.SetHandRangeRankings(SelectedRangeRanking, RangeItems);
            RzrDataService.SetRangeDefinitionAbsolute<HandRangeItem>(RangeItems, MaskPercentage, RangePercentage);
            RzrDataService.SetRangeVariation<HandRangeItem>(RangeItems, MaskPercentage, RangePercentage, VariationFactor);

            UpdateRange();

            if (RangeChanged != null) RangeChanged(this, new DependencyPropertyChangedEventArgs());
        }

        protected void UpdateRange()
        {
            List<HandRangeItem> items = RangeItems.OrderBy(x => Convert.ToInt32(x.ID)).ToList();
            Range.SetProbability(items.Select(x => (int)x.Weight));
        }

        public void LoadFromString(string range)
        {
            _loading = true;
            
            string[] parts = range.Split(';');
            RangePercentage = Convert.ToSingle(parts[0]);
            MaskPercentage = Convert.ToSingle(parts[1]);
            VariationFactor = Convert.ToSingle(parts[2]);

            string[] probs = parts[3].Split(',');
            for (int i = 0; i < 169; i++)
                Range.SetProbability(i, Convert.ToInt32(probs[i]));

            _loading = false;
        }

        public string SaveToString()
        {
            StringBuilder ret = new StringBuilder();

            ret.Append(RangePercentage + ";");
            ret.Append(MaskPercentage + ";");
            ret.Append(VariationFactor + ";");

            bool first = true;
            for (int i = 0; i < 169; i++)
            {
                if (!first) ret.Append(",");
                first = false;
                ret.Append(Range.Probability[i]);
            }

            return ret.ToString();
        }
    }
}
