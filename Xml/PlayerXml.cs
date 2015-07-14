using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Xml
{
    [XmlType("Player")]
    public class PlayerXml
    {
        [XmlAttribute("Stack")]
        public float Stack { get; set; }

        [XmlAttribute("Bet")]
        public float Bet { get; set; }

        [XmlAttribute("Seat")]
        public int Seat { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Active")]
        public bool Active { get; set; }

        [XmlElement("Variables")]
        public VariableContainerXml Variables { get; set; }
    }
}
