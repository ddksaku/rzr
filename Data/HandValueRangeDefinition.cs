using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Data
{
    [XmlRoot("RangeDefinition")]
    public class HandValueRangeDefinition
    {
        [XmlAttribute("ConditionSet")]
        public string ConditionSet { get; set; }

        [XmlAttribute("Round")]
        public HoldemHandRound Round { get; set; }

        [XmlElement("Hands")]
        public HoleCardHandValue[] Hands { get; set; }
    }

    [XmlType("Value")]
    public class HoleCardHandValue
    {
        [XmlAttribute("HoleCardID")]
        public int HoleCardID { get; set; }

        [XmlElement("Probability")]
        public HandValueProbability[] Probabilities { get; set; }
    }

    [XmlType("MaskProbability")]
    public class HandValueProbability
    {
        [XmlAttribute("Prob")]
        public float Probability { get; set; }

        [XmlAttribute("Mask")]
        public int ItemID { get; set; }
    }
}