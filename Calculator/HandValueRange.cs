using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Rzr.Core.Calculator
{
    public class HandValueRange
    {
        protected int[] _probability;

        public CompiledCondition[] Mask { get; private set; }

        public int[] Probability
        {
            get { return _probability; }
            set { SetProbability(value); }
        }

        public HandValueRange(CompiledCondition[] mask)
        {
            Mask = mask;
            Probability = new int[mask.Length];
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
    }
}