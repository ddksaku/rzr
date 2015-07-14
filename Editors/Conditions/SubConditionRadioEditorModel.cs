using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using Rzr.Core.Calculator;

namespace Rzr.Core.Editors.Conditions
{
    public class SubConditionRadioEditorModel : DependencyObject
    {
        #region dependency property definitions

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(SubConditionRadioEditorModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(SubConditionRadioEditorModel), new PropertyMetadata(false, null));

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items", typeof(ObservableCollection<SubConditionRadioItemModel>), typeof(SubConditionRadioItemModel), new PropertyMetadata(null, null));

        #endregion

        #region dependency properties

        public string Name
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }

        public ObservableCollection<SubConditionRadioItemModel> Items
        {
            get { return (ObservableCollection<SubConditionRadioItemModel>)this.GetValue(ItemsProperty); }
            set { this.SetValue(ItemsProperty, value); }
        }

        #endregion

        public SecondaryCondition Secondary { get; set; }

        public SubConditionRadioEditorModel(SecondaryCondition subCondition, string parentGroup)
        {
            this.Secondary = subCondition;

            Name = subCondition.Name;
            IsSelected = false;

            ObservableCollection<SubConditionRadioItemModel> conditions = new ObservableCollection<SubConditionRadioItemModel>();
            foreach (SecondaryConditionValue value in Secondary.Values)
                conditions.Add(new SubConditionRadioItemModel(value, parentGroup + "_" + this.Name));
            Items = conditions;
        }

        public ulong GetMask()
        {            
            ulong mask = 0;
            if (this.IsSelected)
            {
                foreach (SubConditionRadioItemModel item in Items.Where(x => x.IsSelected))
                    mask |= item.Value.MaskValue;
            }
            return mask;
        }

        public void SetMask(ConditionAtom atom)
        {
            this.IsSelected = false;
            foreach (SubConditionRadioItemModel item in Items)
            {
                ulong mask = item.Value.MaskValue;
                item.IsSelected = ((atom.PrimaryMask & mask) == mask);
                this.IsSelected |= item.IsSelected;
            }            
        }

        public void Clear()
        {
            this.IsSelected = false;
            foreach (SubConditionRadioItemModel item in Items)
                item.IsSelected = false;
        }
    }
}
