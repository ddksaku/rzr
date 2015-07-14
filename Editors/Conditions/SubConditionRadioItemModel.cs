using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Rzr.Core.Calculator;

namespace Rzr.Core.Editors.Conditions
{
    /// <summary>
    /// Model for an individual subcondition radio item on the condition selection editor. 
    /// </summary>
    public class SubConditionRadioItemModel : DependencyObject
    {
        #region dependency property definitions

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(SubConditionRadioItemModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(
            "GroupName", typeof(string), typeof(SubConditionRadioItemModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(SubConditionRadioItemModel), new PropertyMetadata(false, null));

        #endregion

        #region dependency properties

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

        #endregion

        public SecondaryConditionValue Value { get; set; }

        public SubConditionRadioItemModel(SecondaryConditionValue value, string groupName)
        {
            this.Value = value;
            this.Name = value.Name;
            this.GroupName = groupName;
            
            this.IsSelected = false;
        }

    }
}
