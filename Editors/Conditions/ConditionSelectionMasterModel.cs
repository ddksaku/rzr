using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Rzr.Core.Calculator;

namespace Rzr.Core.Editors.Conditions
{
    public class ConditionSelectionMasterModel : DependencyObject
    {
        public ConditionSelectorModel Selector { get; private set; }

        public ConditionEditorModel Editor { get; private set; }

        public AvailableConditionListingModel ActiveCondition { get; set; }

        public ConditionSelectionMasterModel(ConditionService service)
        {
            Selector = new ConditionSelectorModel(service);
            Editor = new ConditionEditorModel(service);

            Selector.Edit += OnEdit;
        }

        protected void OnEdit(object sender, EventArgs e)
        {
            if (Edit != null) Edit(sender, e);
        }

        public event EventHandler Edit;
    }
}
