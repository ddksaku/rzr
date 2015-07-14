using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using Rzr.Core.Calculator;
using System.Windows.Controls;

namespace Rzr.Core.Editors.Conditions
{
    public class ComponentEditorModel : DependencyObject
    {
        #region dependency property definitions

        public static readonly DependencyProperty AtomProperty = DependencyProperty.Register(
            "Atom", typeof(ConditionAtom), typeof(ComponentEditorModel), new PropertyMetadata(null, OnAtomChanged));

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(ComponentEditorModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty ValueSubconditionsProperty = DependencyProperty.Register(
            "ValueSubconditions", typeof(ObservableCollection<SubConditionRadioEditorModel>), typeof(ComponentEditorModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty DrawSubconditionsProperty = DependencyProperty.Register(
            "DrawSubconditions", typeof(ObservableCollection<SubConditionRadioEditorModel>), typeof(ComponentEditorModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty ValueConditionsProperty = DependencyProperty.Register(
            "ValueConditions", typeof(ObservableCollection<PrimaryCondition>), typeof(ComponentEditorModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty SelectedConditionProperty = DependencyProperty.Register(
            "SelectedCondition", typeof(string), typeof(ComponentEditorModel), new PropertyMetadata(null, SelectedConditionChanged));

        #endregion

        public ConditionAtom Atom
        {
            get { return (ConditionAtom)this.GetValue(AtomProperty); }
            set { this.SetValue(AtomProperty, value); }
        }

        public string Name
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        public ObservableCollection<SubConditionRadioEditorModel> ValueSubconditions
        {
            get { return (ObservableCollection<SubConditionRadioEditorModel>)this.GetValue(ValueSubconditionsProperty); }
            set { this.SetValue(ValueSubconditionsProperty, value); }
        }

        public ObservableCollection<SubConditionRadioEditorModel> DrawSubconditions
        {
            get { return (ObservableCollection<SubConditionRadioEditorModel>)this.GetValue(DrawSubconditionsProperty); }
            set { this.SetValue(DrawSubconditionsProperty, value); }
        }

        public ObservableCollection<PrimaryCondition> ValueConditions
        {
            get { return (ObservableCollection<PrimaryCondition>)this.GetValue(ValueConditionsProperty); }
            set { this.SetValue(ValueConditionsProperty, value); }
        }

        public string SelectedCondition 
        {
            get { return (string)this.GetValue(SelectedConditionProperty); }
            set { this.SetValue(SelectedConditionProperty, value); }
        }

        protected static void OnAtomChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ComponentEditorModel model = sender as ComponentEditorModel;
            model.InitialiseEditor();
        }

        protected static void SelectedConditionChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ComponentEditorModel model = sender as ComponentEditorModel;
            model.ValueChanged(sender, e);
        }

        public ComponentEditorModel()
        {
            ObservableCollection<PrimaryCondition> conditions = new ObservableCollection<PrimaryCondition>();
            foreach (PrimaryCondition condition in ConditionService.ConditionMap.Conditions.Where(x => x.Name != "Draw Type"))
                conditions.Add(condition);
            ValueConditions = conditions;

            PrimaryCondition draw = ConditionService.ConditionMap.Conditions.First(x => x.Name == "Draw Type");
            ObservableCollection<SubConditionRadioEditorModel> drawConditions = new ObservableCollection<SubConditionRadioEditorModel>();
            foreach (SecondaryCondition subCondition in draw.SubConditions)
                drawConditions.Add(new SubConditionRadioEditorModel(subCondition, draw.GroupName));
            DrawSubconditions = drawConditions;
        }

        protected void ValueChanged(object sender, DependencyPropertyChangedEventArgs e)
        {            
            PrimaryCondition condition = ValueConditions.FirstOrDefault(x => x.Name == SelectedCondition);
            if (condition != null)
            {
                ObservableCollection<SubConditionRadioEditorModel> conditions = new ObservableCollection<SubConditionRadioEditorModel>();
                foreach (SecondaryCondition subCondition in condition.SubConditions)
                    conditions.Add(new SubConditionRadioEditorModel(subCondition, condition.GroupName));
                ValueSubconditions = conditions;
            }
            else
            {
                ValueSubconditions = new ObservableCollection<SubConditionRadioEditorModel>();
            }
        }

        protected void InitialiseEditor()
        {
            Name = Atom.Name;

            if (Atom == null)
            {
                SelectedCondition = null;
            }
            else
            {
                PrimaryCondition selected = ConditionService.ConditionMap.Conditions.FirstOrDefault(x => (x.MaskValue & Atom.PrimaryMask) == x.MaskValue);
                if (selected != null)
                {
                    SelectedCondition = selected.Name;
                    foreach (SubConditionRadioEditorModel subCondition in ValueSubconditions)
                        subCondition.SetMask(Atom);
                    foreach (SubConditionRadioEditorModel subCondition in DrawSubconditions)
                        subCondition.SetMask(Atom);
                }
            }
        }

        public void SaveEditor()
        {
            if (Atom == null)
                Atom = new ConditionAtom();

            PrimaryCondition condition = ValueConditions.FirstOrDefault(x => x.Name == SelectedCondition);
            ulong mask = (condition == null) ? 0 : condition.MaskValue;

            if (this.ValueSubconditions != null)
            {
                foreach (SubConditionRadioEditorModel subCondition in this.ValueSubconditions)
                    mask |= subCondition.GetMask();
            }
            foreach (SubConditionRadioEditorModel subCondition in this.DrawSubconditions)
                mask |= subCondition.GetMask();

            Atom.PrimaryMask = mask;
            Atom.Name = Name;
        }
    }
}
