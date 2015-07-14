using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using Rzr.Core.Calculator;

namespace Rzr.Core.Editors.Conditions
{
    public class ConditionEditorModel : DependencyObject
    {
        #region dependency property definitions

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(ConditionEditorModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(ConditionEditorModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty AndAtomsProperty = DependencyProperty.Register(
            "AndAtoms", typeof(ObservableCollection<ConditionComponentListingModel>), typeof(ConditionEditorModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty OrAtomsProperty = DependencyProperty.Register(
            "OrAtoms", typeof(ObservableCollection<ConditionComponentListingModel>), typeof(ConditionEditorModel), new PropertyMetadata(null, null));

        #endregion

        #region property definition

        public string Title
        {
            get { return (string)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        public string Name
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        public ObservableCollection<ConditionComponentListingModel> AndAtoms
        {
            get { return (ObservableCollection<ConditionComponentListingModel>)this.GetValue(AndAtomsProperty); }
            set { this.SetValue(AndAtomsProperty, value); }
        }

        public ObservableCollection<ConditionComponentListingModel> OrAtoms
        {
            get { return (ObservableCollection<ConditionComponentListingModel>)this.GetValue(OrAtomsProperty); }
            set { this.SetValue(OrAtomsProperty, value); }
        }

        public const int ANDCONDITIONS = 1;
        public const int ORCONDITIONS = 2;

        public int ActiveCondition { get; set; }

        #endregion

        protected ConditionContainer _condition;
        
        public ComponentEditorModel EditorModel { get; private set; }

        public ExistingConditionSelectorModel ConditionSelectorModel { get; private set; }

        #region constructor

        public ConditionEditorModel(ConditionService service)
        {
            AndAtoms = new ObservableCollection<ConditionComponentListingModel>();
            OrAtoms = new ObservableCollection<ConditionComponentListingModel>();
            EditorModel = new ComponentEditorModel();
            ConditionSelectorModel = new ExistingConditionSelectorModel(service);

            ConditionSelectorModel.OnSelect += ConditionSelected;
        }

        #endregion

        public void Initialise(ConditionContainer condition)
        {
            ObservableCollection<ConditionComponentListingModel> atoms = new ObservableCollection<ConditionComponentListingModel>();
            if (condition != null && condition.AndConditions != null)
            {
                foreach (ConditionAtom atom in condition.AndConditions)
                {
                    ConditionComponentListingModel componentModel = new ConditionComponentListingModel(atom);
                    componentModel.OnEdit += EditComponentModel;
                    componentModel.OnDelete += DeleteComponentModel;
                    atoms.Add(componentModel);
                }
            }
            AndAtoms = atoms;

            atoms = new ObservableCollection<ConditionComponentListingModel>();
            if (condition != null && condition.OrConditions != null)
            {
                foreach (ConditionAtom atom in condition.OrConditions)
                {
                    ConditionComponentListingModel componentModel = new ConditionComponentListingModel(atom);
                    componentModel.OnEdit += EditComponentModel;
                    componentModel.OnDelete += DeleteComponentModel;
                    atoms.Add(componentModel);
                }
            }
            OrAtoms = atoms;

        }

        public void Refresh()
        {
            foreach (ConditionComponentListingModel atom in AndAtoms)
                atom.Refresh();
            foreach (ConditionComponentListingModel atom in OrAtoms)
                atom.Refresh();
            InvalidateProperty(AndAtomsProperty);
            InvalidateProperty(OrAtomsProperty);
        }

        public ConditionContainer GetCondition(string id)
        {
            ConditionContainer container = new ConditionContainer();
            container.ID = id;
            container.AndConditions = AndAtoms.Select(x => x.Atom).ToArray();
            container.OrConditions = OrAtoms.Select(x => x.Atom).ToArray();
            container.Name = Name;
            return container;
        }

        public ConditionComponentListingModel GetConditionAtomForAnd()
        {
            ConditionAtom atom = new ConditionAtom();
            ConditionComponentListingModel componentModel = new ConditionComponentListingModel(atom);
            componentModel.OnEdit += EditComponentModel;
            componentModel.OnDelete += DeleteComponentModel;
            AndAtoms.Add(componentModel);
            return componentModel;
        }

        public ConditionComponentListingModel GetConditionAtomForOr()
        {
            ConditionAtom atom = new ConditionAtom();
            ConditionComponentListingModel componentModel = new ConditionComponentListingModel(atom);
            componentModel.OnEdit += EditComponentModel;
            componentModel.OnDelete += DeleteComponentModel;
            OrAtoms.Add(componentModel);
            return componentModel;
        }

        protected void EditComponentModel(object sender, EventArgs e)
        {
            if (OnEditComponentModel != null) OnEditComponentModel(sender, e);
        }

        protected void DeleteComponentModel(object sender, EventArgs e)
        {
            if (OnDeleteComponentModel != null) OnDeleteComponentModel(sender, e);
        }

        public void ConditionSelected(object sender, EventArgs e)
        {
            ExistingConditionListingModel listing = sender as ExistingConditionListingModel;

            if (ActiveCondition == ANDCONDITIONS)
            {
                ConditionComponentListingModel atom = this.GetConditionAtomForAnd();
                atom.Name = listing.Name;
                atom.Atom.Type = ConditionAtomType.Nested;
                atom.Atom.LinkedContainerId = listing.ID;
            }
            else if (ActiveCondition == ORCONDITIONS)
            {
                ConditionComponentListingModel atom = this.GetConditionAtomForAnd();
                atom.Name = listing.Name;
                atom.Atom.Type = ConditionAtomType.Nested;
                atom.Atom.LinkedContainerId = listing.ID;
            }

            if (OnConditionSelected != null) OnConditionSelected(sender, e);
        }

        public event EventHandler OnConditionSelected;

        public event EventHandler OnEditComponentModel;

        public event EventHandler OnDeleteComponentModel;
    }
}
