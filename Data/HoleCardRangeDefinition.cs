using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
    using System.Xml.Serialization;

namespace Rzr.Core.Data
{
    [XmlType("RangeDef")]
    public class HoleCardRangeDefinition
    {
        public const string Key = "RangeDefinition";

        [XmlElement("ID")]
        public int ID { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlAttribute("DefaultRange")]
        public float DefaultRange { get; set; }

        [XmlAttribute("DefaultVariation")]
        public float DefaultVariation { get; set; }

        [XmlArray("Hands")]
        public List<HandDefinition> Hands { get; set; }

        public HoleCardRangeDefinition Copy()
        {
            HoleCardRangeDefinition copy = new HoleCardRangeDefinition();
            copy.Name = "Copy of " + Name;
            copy.DefaultRange = DefaultRange;
            copy.DefaultVariation = DefaultVariation;
            copy.Hands = Hands.ToList();
            return copy;
        }
    }

    [XmlType("Hand")]
    public class HandDefinition
    {
        [XmlAttribute("Definition")]
        public string HandDef { get; set; }

        [XmlAttribute("Value")]
        public float Value { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }
    }
}
