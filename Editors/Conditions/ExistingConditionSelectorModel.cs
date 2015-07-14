using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using Rzr.Core.Calculator;

namespace Rzr.Core.Editors.Conditions
{
    public class ExistingConditionSelectorModel : DependencyObject
    {
        public static readonly DependencyProperty ConditionsProperty = DependencyProperty.Register("Conditions", 
            typeof(ObservableCollection<ExistingConditionListingModel>), typeof(ExistingConditionSelectorModel), new PropertyMetadata(null, null));

        public ObservableCollection<ExistingConditionListingModel> Conditions
        {
            get { return (ObservableCollection<ExistingConditionListingModel>)this.GetValue(ConditionsProperty); }
            set { this.SetValue(ConditionsProperty, value); }
        }

        public ExistingConditionSelectorModel(ConditionService service)
        {
            ObservableCollection<ExistingConditionListingModel> conditions = new ObservableCollection<ExistingConditionListingModel>();
            foreach (ConditionContainer container in service.Definition.ConfiguredConditions)
            {
                ExistingConditionListingModel containerModel = new ExistingConditionListingModel(container);
                containerModel.OnSelect += ItemSelect;
                conditions.Add(containerModel);
            }
            Conditions = conditions;
        }

        protected void ItemSelect(object sender, EventArgs e)
        {
            if (OnSelect != null) OnSelect(sender, e);
        }

        public event EventHandler OnSelect;
    }
}
