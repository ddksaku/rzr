using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Calculator
{
    [XmlRoot("Root")]
    public class ConditionsDefinition
    {
        [XmlArray("Master")]
        public List<ConditionContainer> ConfiguredConditions { get; set; }

        [XmlArray("Selected")]
        public List<ConditionRangeItem> ActiveItems { get; set; }
    }

    [XmlType("Item")]
    public class ConditionRangeItem
    {
        [XmlAttribute("Percentage")]
        public int Percentage { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }
    }

    [XmlType("Meta")]
    public class ConditionMeta
    {
        [XmlElement("RangePercentage")]
        public float SelectedRange { get; set; }

        [XmlElement("MaskPercentage")]
        public float SelectedRangeMask { get; set; }

        [XmlElement("VariationPercentage")]
        public float SelectedVariation { get; set; }
    }
}
