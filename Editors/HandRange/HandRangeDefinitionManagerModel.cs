using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using Rzr.Core.Data;

namespace Rzr.Core.Editors.HandRange
{
    public class HandRangeDefinitionManagerModel : DependencyObject
    {
        public static readonly DependencyProperty RangesProperty = DependencyProperty.Register("Ranges",
            typeof(ObservableCollection<HoleCardRangeDefinition>), typeof(HandRangeDefinitionManagerModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty SelectedRangeProperty = DependencyProperty.Register("SelectedRange",
            typeof(HoleCardRangeDefinition), typeof(HandRangeDefinitionManagerModel), new PropertyMetadata(null, OnRangeChanged));

        public static readonly DependencyProperty ActiveRangeModelProperty = DependencyProperty.Register("ActiveRangeModel",
            typeof(HandRangeDefinitionModel), typeof(HandRangeDefinitionManagerModel), new PropertyMetadata(null, null));

        public ObservableCollection<HoleCardRangeDefinition> Ranges
        {
            get { return (ObservableCollection<HoleCardRangeDefinition>)this.GetValue(RangesProperty); }
            set { this.SetValue(RangesProperty, value); }
        }

        public HoleCardRangeDefinition SelectedRange
        {
            get { return (HoleCardRangeDefinition)this.GetValue(SelectedRangeProperty); }
            set { this.SetValue(SelectedRangeProperty, value); }
        }

        public HandRangeDefinitionModel ActiveRangeModel
        {
            get { return (HandRangeDefinitionModel)this.GetValue(ActiveRangeModelProperty); }
            set { this.SetValue(ActiveRangeModelProperty, value); }
        }

        protected static void OnRangeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HandRangeDefinitionManagerModel model = sender as HandRangeDefinitionManagerModel;
            model.SetActiveRange();
        }

        protected RzrDataService _service;

        public HandRangeDefinitionManagerModel(RzrDataService service)
        {
            _service = service;
            ActiveRangeModel = new HandRangeDefinitionModel();

            SetRanges();            
            SelectedRange = Ranges.FirstOrDefault();            
        }

        protected void SetRanges()
        {
            ObservableCollection<HoleCardRangeDefinition> ranges = new ObservableCollection<HoleCardRangeDefinition>();
            foreach (HoleCardRangeDefinition range in _service.HoleCardRanges)
                ranges.Add(range);            
            Ranges = ranges;            
        }

        protected void SetActiveRange()
        {             
            ActiveRangeModel.LoadFrom(SelectedRange);
        }

        public void AddRange(HoleCardRangeDefinition def)
        {
            Ranges.Add(def);
            this.InvalidateProperty(RangesProperty);
            SelectedRange = def;
        }

        public void Save()
        {
            ActiveRangeModel.SaveTo(SelectedRange);
            RzrDataService.Service.SaveHoleCardRange(SelectedRange);            
            string selectedName = ActiveRangeModel.Name;
            SetRanges();
            SelectedRange = Ranges.First(x => x.Name == selectedName);             
        }

        public void Delete()
        {
            if (SelectedRange != null)
            {
                RzrDataService.Service.DeleteHoleCardRange(SelectedRange.Name);
                SetRanges();
                SelectedRange = Ranges.FirstOrDefault();                
            }
        }
    }
}
