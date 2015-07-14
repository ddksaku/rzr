using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Tree.Xml
{
    [XmlType("PlayerInfo")]
    public class BetTreePlayerInfoXml
    {
        [XmlAttribute("Index")]
        public int Index { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Stack")]
        public float Stack { get; set; }

        [XmlAttribute("Bet")]
        public float Bet { get; set; }

        [XmlAttribute("Ev")]
        public float ExpectedValue { get; set; }
    }
}
