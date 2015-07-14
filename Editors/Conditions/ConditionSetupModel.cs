using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Calculator;
using System.Windows;
using System.Collections.ObjectModel;

namespace Rzr.Core.Editors.Conditions
{
    public class ConditionSetupModel : DependencyObject
    {
        #region dependency property definitions

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(ConditionSetupModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(
            "GroupName", typeof(string), typeof(ConditionSetupModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(ConditionSetupModel), new PropertyMetadata(false, null));

        public static readonly DependencyProperty ConditionsProperty = DependencyProperty.Register(
            "Conditions", typeof(ObservableCollection<SubConditionRadioEditorModel>), typeof(ConditionSetupModel), new PropertyMetadata(null, null));

        #endregion

        #region property definition

        public string Name
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        public string GroupName
        {
            get { return (string)this.GetValue(GroupNameProperty); }
            set { this.SetValue(GroupNameProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }

        public ObservableCollection<SubConditionRadioEditorModel> SubConditions
        {
            get { return (ObservableCollection<SubConditionRadioEditorModel>)this.GetValue(ConditionsProperty); }
            set { this.SetValue(ConditionsProperty, value); }
        }

        #endregion

        public PrimaryCondition Primary { get; set; }

        #region constructor

        public ConditionSetupModel(PrimaryCondition condition)
        {
            Primary = condition;
            Name = condition.Name;
            GroupName = condition.GroupName;

            ObservableCollection<SubConditionRadioEditorModel> conditions = new ObservableCollection<SubConditionRadioEditorModel>();
            foreach (SecondaryCondition subCondition in Primary.SubConditions)
                conditions.Add(new SubConditionRadioEditorModel(subCondition, GroupName));
            SubConditions = conditions;
        }

        #endregion

        public ulong GetMask()
        {
            ulong mask = Primary.MaskValue;
            foreach (SubConditionRadioEditorModel subCondition in this.SubConditions)
                mask |= subCondition.GetMask();

            return mask;
        }

        public void Initialise(ConditionAtom atom)
        {   
            ulong primaryMask = Primary.MaskValue;
            this.IsSelected = ((atom.PrimaryMask & primaryMask) == primaryMask);

            foreach (SubConditionRadioEditorModel subCondition in this.SubConditions)
            {
                if (this.IsSelected)
                    subCondition.SetMask(atom);
                else
                    subCondition.Clear();
            }
        }

        public void Clear()
        {
            this.IsSelected = false;
            foreach (SubConditionRadioEditorModel subCondition in this.SubConditions)
                subCondition.Clear();
        }
    }
}
