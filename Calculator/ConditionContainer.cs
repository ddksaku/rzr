using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Calculator
{
    [XmlType("Condition")]
    public class ConditionContainer
    {
        // debug only
        [XmlIgnore()]
        public PrimaryCondition Primary { get; set; }
        
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("ExpectedProbability")]
        public float ExpectedProbability { get; set; }

        [XmlAttribute("Group")]
        public string Group { get; set; }

        [XmlElement("AndCondition")]
        public ConditionAtom[] AndConditions { get; set; }

        [XmlElement("OrCondition")]
        public ConditionAtom[] OrConditions { get; set; }
    }
}
