using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Editors.Variables;
using System.Windows;

namespace Rzr.Core.Tree
{
    public class ValueModel : DependencyObject
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(float), typeof(ValueModel), new PropertyMetadata(0f, null));

        public static readonly DependencyProperty VariableProperty = DependencyProperty.Register(
            "Variable", typeof(Variable), typeof(ValueModel), new PropertyMetadata(null, VariableChanged));

        public float Value
        {
            get { return (float)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        public Variable Variable
        {
            get { return (Variable)this.GetValue(VariableProperty); }
            set 
            {
                Variable existing = this.GetValue(VariableProperty) as Variable;
                if (value != existing)
                {
                    existing.OnValueChanged -= OnVariableChanged;
                    this.SetValue(VariableProperty, value);
                    existing.OnValueChanged += OnVariableChanged;
                }                
            }
        }

        protected static void VariableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ValueModel model = sender as ValueModel;
            model.OnVariableChanged();
        }

        protected void OnVariableChanged()
        {
            if (Variable != null)
            {
                Value = Variable.Value;
            }
        }
    }
}
