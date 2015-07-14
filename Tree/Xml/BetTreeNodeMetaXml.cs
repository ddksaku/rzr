using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Tree.Xml
{
    [XmlType("Meta")]
    public class BetTreeNodeMetaXml
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlElement("Bet")]
        public BetTreeNodeBetXml BetXml { get; set; }

        [XmlArray("Parms")]
        public List<BetTreeNodeParmXml> Parms { get; set; }

        public BetTreeNodeMetaXml()
        {
            Parms = new List<BetTreeNodeParmXml>();
        }
    }
}
