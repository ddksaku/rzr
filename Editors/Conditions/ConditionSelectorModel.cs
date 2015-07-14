using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Calculator;
using System.Windows;
using System.Collections.ObjectModel;
using Rzr.Core.Data;

namespace Rzr.Core.Editors.Conditions
{
    public class ConditionSelectorModel : DependencyObject
    {
        public const string ADD_NEW_STRING = "{Add New}";

        #region dependency property definitions

        public static readonly DependencyProperty ServiceProperty = DependencyProperty.Register(
            "Service", typeof(ConditionService), typeof(ConditionSelectorModel), new PropertyMetadata(null, ServiceChanged));

        public static readonly DependencyProperty ConditionsProperty = DependencyProperty.Register(
            "Conditions", typeof(ObservableCollection<AvailableConditionListingModel>), typeof(ConditionSelectorModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty AvailableRangesProperty = DependencyProperty.Register(
            "AvailableRanges", typeof(ObservableCollection<string>), typeof(ConditionSelectorModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty SelectedRangeProperty = DependencyProperty.Register(
            "SelectedRange", typeof(string), typeof(ConditionSelectorModel), new PropertyMetadata(null, null));

        #endregion

        #region property definition

        public ConditionService Service
        {
            get { return (ConditionService)this.GetValue(ServiceProperty); }
            set { this.SetValue(ServiceProperty, value); }
        }

        public ObservableCollection<AvailableConditionListingModel> Conditions
        {
            get { return (ObservableCollection<AvailableConditionListingModel>)this.GetValue(ConditionsProperty); }
            set { this.SetValue(ConditionsProperty, value); }
        }

        public string SelectedRange
        {
            get { return (string)this.GetValue(SelectedRangeProperty); }
            set { this.SetValue(SelectedRangeProperty, value); }
        }

        public ObservableCollection<string> AvailableRanges
        {
            get { return (ObservableCollection<string>)this.GetValue(AvailableRangesProperty); }
            set { this.SetValue(AvailableRangesProperty, value); }
        }


        #endregion

        protected static void ServiceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ConditionSelectorModel model = sender as ConditionSelectorModel;
            model.InitialiseConditions();
        }

        public ConditionSelectorModel(ConditionService service)
        {
            Service = service;

            ObservableCollection<string> availableRanges = new ObservableCollection<string>();
            foreach (string name in ConditionService.AvailableConditionSets)
                availableRanges.Add(name);
            availableRanges.Add(ADD_NEW_STRING);
            AvailableRanges = availableRanges;

            SelectedRange = service.Name;
        }

        protected void InitialiseConditions()
        {
            ObservableCollection<AvailableConditionListingModel> conditions = new ObservableCollection<AvailableConditionListingModel>();
            foreach (ConditionContainer condition in Service.Definition.ConfiguredConditions)
            {
                conditions.Add(GetCondition(
                    condition,
                    Service.Definition.ActiveItems.Exists(y => y.Name == condition.Name)));
            }
            Conditions = conditions;
        }

        public void SaveService()
        {
            Service.Definition.ConfiguredConditions = Conditions.Select(x => x.Condition).ToList();
            Service.Definition.ActiveItems = Conditions.Where(x => x.IsSelected).Select(
                x => new ConditionRangeItem() { Name = x.Name, Percentage = 0 }).ToList();

            Service.SaveDefinitionAs(Service.Name);                
        }

        public void Distribute(float rangeFactor, float variationFactor)
        {
            HandRangeModel range = new HandRangeModel();
            range.RangePercentage = rangeFactor;
            range.VariationFactor = variationFactor;

            List<CompiledCondition> conditions = Service.GetCompiledConditions(
                this.Conditions.Where(x => x.IsSelected).Select(x => x.Condition).ToList());
            Service.Distribute(conditions, range, 5000);

            foreach (AvailableConditionListingModel listing in this.Conditions)
                listing.Probability = String.Format("{0:0.00}%", listing.Condition.ExpectedProbability * 100);
                
        }

        public void AddCondition(ConditionContainer condition)
        {
            Conditions.Add(GetCondition(condition, true));
            InvalidateProperty(ConditionsProperty);
        }

        protected AvailableConditionListingModel GetCondition(ConditionContainer container, bool selected)
        {
            AvailableConditionListingModel model = new AvailableConditionListingModel(container, selected);
            model.Edit += OnEdit;
            model.Delete += OnDelete;
            model.MoveUp += OnMoveUp;
            model.MoveDown += OnMoveDown;
            return model;
        }

        protected void OnEdit(object sender, EventArgs e)
        {
            if (Edit != null) Edit(sender, e);
        }

        protected void OnDelete(object sender, EventArgs e)
        {
            AvailableConditionListingModel model = sender as AvailableConditionListingModel;
            Conditions.Remove(model);
            InvalidateProperty(ConditionsProperty);
        }

        protected void OnMoveUp(object sender, EventArgs e)
        {
            AvailableConditionListingModel model = sender as AvailableConditionListingModel;
            int index = Conditions.IndexOf(model);
            if (index > 0)
                Conditions.Move(index, index - 1);
            InvalidateProperty(ConditionsProperty);
        }

        protected void OnMoveDown(object sender, EventArgs e)
        {
            AvailableConditionListingModel model = sender as AvailableConditionListingModel;
            int index = Conditions.IndexOf(model);
            if (index < Conditions.Count - 1)
                Conditions.Move(index, index + 1);
            InvalidateProperty(ConditionsProperty);
        }

        public event EventHandler Edit;

        public void UpdateService()
        {
            Service = new ConditionService(SelectedRange, true);
        }

        public string GetNewId()
        {
            int id = this.Conditions.Count;
            while (this.Conditions.Count(x => x.Condition.ID == id.ToString()) > 0)
                id++;
            return id.ToString();
        }
    }
}
