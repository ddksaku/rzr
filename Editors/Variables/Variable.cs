using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Xml;
using System.Windows;

namespace Rzr.Core.Editors.Variables
{
    public class Variable : DependencyObject
    {
        #region dependency properties

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(Variable), new PropertyMetadata(null, null));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description", typeof(string), typeof(Variable), new PropertyMetadata(null, null));

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            "Type", typeof(VariableType), typeof(Variable), new PropertyMetadata(VariableType.Percentage, null));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(float), typeof(Variable), new PropertyMetadata(0f, ValueChanged));

        public static readonly DependencyProperty IsPrimaryProperty = DependencyProperty.Register(
            "Primary", typeof(bool), typeof(Variable), new PropertyMetadata(true, null));

        public static readonly DependencyProperty ItemVisibilityProperty = DependencyProperty.Register(
            "ItemVisibility", typeof(Visibility), typeof(Variable), new PropertyMetadata(Visibility.Collapsed, null));

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
            "State", typeof(VariableState), typeof(Variable), new PropertyMetadata(VariableState.Saved, null));

        public string Name
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        public string Description
        {
            get { return (string)this.GetValue(DescriptionProperty); }
            set { this.SetValue(DescriptionProperty, value); }
        }

        public VariableType Type
        {
            get { return (VariableType)this.GetValue(TypeProperty); }
            set { this.SetValue(TypeProperty, value); }
        }

        public float Value
        {
            get { return (float)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        public bool IsPrimary
        {
            get { return (bool)this.GetValue(IsPrimaryProperty); }
            set { this.SetValue(IsPrimaryProperty, value); }
        }

        public Visibility ItemVisibility
        {
            get { return (Visibility)this.GetValue(ItemVisibilityProperty); }
            set { this.SetValue(ItemVisibilityProperty, value); }
        }

        public VariableState State
        {
            get { return (VariableState)this.GetValue(StateProperty); }
            set { this.SetValue(StateProperty, value); }
        }

        protected static void ValueChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Variable var = sender as Variable;
            if (var.OnValueChanged != null) var.OnValueChanged();
        }

        public event EmptyEventHandler OnValueChanged;

        #endregion

        #region xml

        /// <summary>
        /// Load variable from xml
        /// </summary>
        public void LoadFromXml(VariableXml xml)
        {
            this.Name = xml.Name;
            this.Description = xml.Description;
            this.Type = xml.Type;
            this.Value = xml.Value;
        }

        /// <summary>
        /// Save variable to xml
        /// </summary>
        public VariableXml SaveToFile()
        {
            VariableXml ret = new VariableXml();
            ret.Name = this.Name;
            ret.Description = this.Description;
            ret.Type = this.Type;
            ret.Value = this.Value;
            return ret;
        }

        public void Delete()
        {
            if (OnDelete != null) OnDelete(this);
        }

        public event VariableEvent OnDelete;

        #endregion
    }

    public enum VariableState
    {
        Saved = 0,
        New = 1,
        Editing = 2
    }

    public delegate void VariableEvent(Variable var);
}
