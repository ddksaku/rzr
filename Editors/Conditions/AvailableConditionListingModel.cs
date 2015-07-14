using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Calculator;
using System.Windows;
using System.Windows.Media;

namespace Rzr.Core.Editors.Conditions
{
    public class AvailableConditionListingModel : DependencyObject
    {
        #region dependency property definitions

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(AvailableConditionListingModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(AvailableConditionListingModel), new PropertyMetadata(false, null));

        public static readonly DependencyProperty ProbabilityProperty = DependencyProperty.Register(
            "Probability", typeof(string), typeof(AvailableConditionListingModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background", typeof(Brush), typeof(AvailableConditionListingModel), new PropertyMetadata(new SolidColorBrush(Colors.Black), null));

        #endregion

        #region property definition

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

        public string Probability
        {
            get { return (string)this.GetValue(ProbabilityProperty); }
            set { this.SetValue(ProbabilityProperty, value); }
        }

        public Brush Background
        {
            get { return (Brush)this.GetValue(BackgroundProperty); }
            set { this.SetValue(BackgroundProperty, value); }
        }

        #endregion

        public ConditionContainer Condition { get; private set; }

        public AvailableConditionListingModel(ConditionContainer condition, bool isSelected)
        {
            Condition = condition;
            Name = condition.Name;
            Probability = String.Format("{0:0.00}%", condition.ExpectedProbability * 100);
            IsSelected = isSelected;            
        }

        public void SetCondition(ConditionContainer condition)
        {
            Condition = condition;
            Name = condition.Name;
            Probability = String.Format("{0:0.00}%", condition.ExpectedProbability * 100);
        }

        public void OnEdit()
        {
            if (Edit != null) Edit(this, EventArgs.Empty);
        }

        public event EventHandler Edit;

        public event EventHandler Delete;

        public event EventHandler MoveUp;

        public event EventHandler MoveDown;
    }
}
