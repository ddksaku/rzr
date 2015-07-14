using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Calculator
{
    [XmlType("Condition")]
    public class ConditionRecord
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Group")]
        public string Group { get; set; }

        [XmlAttribute("Class")]
        public string Class { get; set; }

        [XmlAttribute("Flag")]
        public int Flag { get; set; }
    }
}
