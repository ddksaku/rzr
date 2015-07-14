using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Rzr.Core.Data;

namespace Rzr.Core.Editors.HandRange
{
    public class HandDefinitionModel : DependencyObject
    {
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background",
            typeof(Brush), typeof(HandDefinitionModel), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty DefinitionProperty = DependencyProperty.Register("Definition",
            typeof(string), typeof(HandDefinitionModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(float), typeof(HandDefinitionModel), new PropertyMetadata(0f, null));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description",
            typeof(string), typeof(HandDefinitionModel), new PropertyMetadata(null, null));

        public HandDefinitionModel(HandDefinition def)
        {
            this.Definition = def.HandDef;
            this.Value = def.Value;
            this.Description = def.Description;
        }

        public Brush Background
        {
            get { return (Brush)this.GetValue(BackgroundProperty); }
            set { this.SetValue(BackgroundProperty, value); }
        }

        public string Definition
        {
            get { return (string)this.GetValue(DefinitionProperty); }
            set { this.SetValue(DefinitionProperty, value); }
        }

        public float Value
        {
            get { return (float)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        public string Description
        {
            get { return (string)this.GetValue(DescriptionProperty); }
            set { this.SetValue(DescriptionProperty, value); }
        }


        public HandDefinition GetDef()
        {
            HandDefinition ret = new HandDefinition();
            ret.HandDef = this.Definition;
            ret.Value = this.Value;
            ret.Description = this.Description;
            return ret;
        }        
        
        public delegate void RearrangeRanksHandler(HandDefinitionModel hand);
        public RearrangeRanksHandler RearrangeRanks { get; set; }
    }
}
