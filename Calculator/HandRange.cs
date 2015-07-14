using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using Rzr.Core.Data;

namespace Rzr.Core.Calculator
{
    public class HandRange
    {
        public string Name { get; set; }

        protected int[] _probability;
        public int[] Probability
        {
            get { return _probability; }
            set { SetProbability(value); }
        }

        public HandRange()
        {
            Probability = new int[169];
        }

        public void SetProbability(IEnumerable<int> values)
        {
            _probability = values.ToArray();
            if (RangeChanged != null) RangeChanged(this, new DependencyPropertyChangedEventArgs());
        }

        public void SetProbability(int index, int value)
        {
            _probability[index] = value;
            if (RangeChanged != null) RangeChanged(this, new DependencyPropertyChangedEventArgs());
        }

        public event DependencyPropertyChangedEventHandler RangeChanged;

        public float GetProbability(ulong hand)
        {
            uint handIndex = RzrDataService.GetCardIndex(hand);
            return Probability[handIndex];
        }
    }
}