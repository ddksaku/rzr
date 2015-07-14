using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Xml;
using System.Windows;
using System.Collections.ObjectModel;

namespace Rzr.Core.Editors.Variables
{
    public class VariableGroup : DependencyObject
    {
        #region dependency properties

        public static readonly DependencyProperty VariablesProperty = DependencyProperty.Register("Variables",
            typeof(ObservableCollection<Variable>), typeof(VariableGroup), new PropertyMetadata(null, null));

        public static readonly DependencyProperty GroupVisibilityProperty = DependencyProperty.Register("GroupVisibility",
            typeof(Visibility), typeof(VariableGroup), new PropertyMetadata(Visibility.Collapsed, null));

        public static readonly DependencyProperty ShortNameProperty = DependencyProperty.Register("ShortName",
            typeof(string), typeof(VariableGroup), new PropertyMetadata(null, null));   
        
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name",
            typeof(string), typeof(VariableGroup), new PropertyMetadata(null, null));

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected",
            typeof(bool), typeof(VariableGroup), new PropertyMetadata(false, OnSelectedChanged));

        public static readonly DependencyProperty ShowPrimaryProperty = DependencyProperty.Register("ShowPrimary",
            typeof(bool), typeof(VariableGroup), new PropertyMetadata(false, OnShowPrimaryChanged));

        public static readonly DependencyProperty ShowAllProperty = DependencyProperty.Register("ShowAll",
            typeof(bool), typeof(VariableGroup), new PropertyMetadata(false, OnShowAllChanged));

        public ObservableCollection<Variable> Variables
        {
            get { return (ObservableCollection<Variable>)this.GetValue(VariablesProperty); }
            set { this.SetValue(VariablesProperty, value); }
        }

        public Visibility GroupVisibility
        {
            get { return (Visibility)this.GetValue(GroupVisibilityProperty); }
            set { this.SetValue(GroupVisibilityProperty, value); }
        }

        public string Name
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        public string ShortName
        {
            get { return (string)this.GetValue(ShortNameProperty); }
            set { this.SetValue(ShortNameProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }

        public bool ShowPrimary
        {
            get { return (bool)this.GetValue(ShowPrimaryProperty); }
            set { this.SetValue(ShowPrimaryProperty, value); }
        }

        public bool ShowAll
        {
            get { return (bool)this.GetValue(ShowAllProperty); }
            set { this.SetValue(ShowAllProperty, value); }
        }
        
        protected static void OnSelectedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            VariableGroup group = sender as VariableGroup;
            group.GroupVisibility = group.IsSelected ? Visibility.Visible : Visibility.Collapsed;
        }

        protected static void OnShowPrimaryChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            VariableGroup group = sender as VariableGroup;
            foreach (Variable var in group.Variables.Where(x => x.IsPrimary))
                var.ItemVisibility = group.ShowPrimary ? Visibility.Visible : Visibility.Collapsed;
        }

        protected static void OnShowAllChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            VariableGroup group = sender as VariableGroup;
            foreach (Variable var in group.Variables.Where(x => !x.IsPrimary))
                var.ItemVisibility = group.ShowAll ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region constructor

        public VariableGroup()
        {
            Variables = new ObservableCollection<Variable>();
        }

        #endregion

        #region xml

        public void LoadFromXml(VariableGroupXml xml)
        {
            this.Name = xml.Name;
            this.ShortName = xml.ShortName;
            Variables.Clear();
            foreach (VariableXml variable in xml.Variables)
            {
                Variable var = new Variable();
                var.LoadFromXml(variable);
                var.OnDelete += this.DeleteVariable;
                this.Variables.Add(var);
            }
        }

        public VariableGroupXml SaveToXml()
        {
            VariableGroupXml xml = new VariableGroupXml();
            xml.Name = Name;
            xml.ShortName = ShortName;
            xml.Variables = this.Variables.Select(x => x.SaveToFile()).ToArray();
            return xml;
        }

        #endregion

        public void DeleteVariable(Variable var)
        {
            var.OnDelete -= this.DeleteVariable;
            this.Variables.Remove(var);
        }

        public void AddVariable(Variable var)
        {
            var.OnDelete += this.DeleteVariable;
            this.Variables.Add(var);
        }
    }    
}
