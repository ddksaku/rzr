using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Rzr.Core.Calculator;

namespace Rzr.Core.Editors.Conditions
{
    public class ExistingConditionListingModel : DependencyObject
    {
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(ExistingConditionListingModel), new PropertyMetadata(null, null));

        public string Name
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        protected ConditionContainer _container;

        public string ID { get { return _container.ID; } }
        
        public ExistingConditionListingModel(ConditionContainer container)
        {
            _container = container;
            Name = _container.Name;
        }

        public void Select()
        {
            if (OnSelect != null) OnSelect(this, EventArgs.Empty);
        }

        public event EventHandler OnSelect;
    }
}
