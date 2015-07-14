using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Rzr.Core.Data;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Rzr.Core.Editors.HandRange
{
    public class HandRangeDefinitionModel : DependencyObject
    {
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name",
            typeof(string), typeof(HandRangeDefinitionModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description",
            typeof(string), typeof(HandRangeDefinitionModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty DefaultRangeProperty = DependencyProperty.Register("DefaultRange",
            typeof(float), typeof(HandRangeDefinitionModel), new PropertyMetadata(20f, null));

        public static readonly DependencyProperty DefaultVariationProperty = DependencyProperty.Register("DefaultVariation",
            typeof(float), typeof(HandRangeDefinitionModel), new PropertyMetadata(0f, null));

        public static readonly DependencyProperty HandsProperty = DependencyProperty.Register("Hands",
            typeof(ObservableCollection<HandDefinitionModel>), typeof(HandRangeDefinitionModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background",
            typeof(Brush), typeof(HandRangeDefinitionModel), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(230, 230, 230)), null));

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

        public float DefaultRange
        {
            get { return (float)this.GetValue(DefaultRangeProperty); }
            set { this.SetValue(DefaultRangeProperty, value); }
        }

        public float DefaultVariation
        {
            get { return (float)this.GetValue(DefaultVariationProperty); }
            set { this.SetValue(DefaultVariationProperty, value); }
        }

        public ObservableCollection<HandDefinitionModel> Hands
        {
            get { return (ObservableCollection<HandDefinitionModel>)this.GetValue(HandsProperty); }
            set { this.SetValue(HandsProperty, value); }
        }

        public Brush Background
        {
            get { return (Brush)this.GetValue(BackgroundProperty); }
            set { this.SetValue(BackgroundProperty, value); }
        }

        public void SaveTo(HoleCardRangeDefinition def)
        {
            def.Name = this.Name;
            def.Description = this.Description;
            def.DefaultRange = this.DefaultRange;
            def.DefaultVariation = this.DefaultVariation;
            def.Hands = this.Hands.Select(x => x.GetDef()).ToList();
        }

        public void LoadFrom(HoleCardRangeDefinition def)
        {
            if (def == null)
            {
                this.Name = string.Empty;
                this.Description = string.Empty;
                this.DefaultRange = float.NaN;
                this.DefaultVariation = float.NaN;
                this.Hands = null;                
            }
            else
            {
                this.Name = def.Name;
                this.Description = def.Description;
                this.DefaultRange = def.DefaultRange;
                this.DefaultVariation = def.DefaultVariation;
                ObservableCollection<HandDefinitionModel> defs = new ObservableCollection<HandDefinitionModel>();
                foreach (HandDefinition handDef in def.Hands.OrderByDescending(h => h.Value))
                {
                    HandDefinitionModel hand = new HandDefinitionModel(handDef);                    
                    hand.RearrangeRanks = new HandDefinitionModel.RearrangeRanksHandler(RearrangeRanks);
                    defs.Add(hand);
                }
                this.Hands = defs;
            }
        }        
        
        /// <summary>
        /// re-arrange ranks with current hand's rank changed
        /// </summary>
        /// <param name="hand"></param>
        protected void RearrangeRanks(HandDefinitionModel hand)
        {
            HandDefinitionModel startDragHand = hand;
            HandDefinitionModel newListingHand = Hands[169 - (int)(hand.Value)];

            RearrangeRanks(startDragHand, newListingHand);
        }

        /// <summary>
        /// re-arrange ranks from start drag to new listing
        /// </summary>
        /// <param name="startDragHand"></param>
        /// <param name="newListingHand"></param>
        public void RearrangeRanks(HandDefinitionModel startDragHand, HandDefinitionModel newListingHand)
        {
            int oldIndex = Hands.IndexOf(startDragHand);
            int newIndex = Hands.IndexOf(newListingHand);

            startDragHand.Value = newListingHand.Value; // replace start rank value to the new rank value              
            int indexOffset = (newIndex > oldIndex) ? 1 : -1;
            int currentIndex = oldIndex;
            // update from next of the start rank value to the new rank value
            while (currentIndex != newIndex) 
            {
                currentIndex = currentIndex + indexOffset;
                Hands[currentIndex].Value = Hands[currentIndex].Value + indexOffset;
            } 
            
            Hands.Move(oldIndex, newIndex);
            InvalidateProperty(HandRangeDefinitionModel.HandsProperty);
        }
    }
}
