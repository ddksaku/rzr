using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Rzr.Core.Tree.Xml;

namespace Rzr.Core.Xml
{
    [XmlRoot("Scenario")]
    public class ContainerXml
    {
        [XmlElement("DisplayName")]
        public string DisplayName { get; set; }

        [XmlArray("Players")]
        public PlayerXml[] Players { get; set; }

        [XmlElement("Table")]
        public TableXml Table { get; set; }

        [XmlElement("BetTree")]
        public BetTreeXml BetTree { get; set; }
    }
}
