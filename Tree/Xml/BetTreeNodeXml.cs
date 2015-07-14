using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Tree.Xml
{
    [XmlType("Node")]
    public class BetTreeNodeXml
    {
        [XmlArray("Children")]
        public List<BetTreeNodeXml> Children { get; set; }

        [XmlElement("Meta")]
        public BetTreeNodeMetaXml MetaXml { get; set; }

        [XmlElement("Info")]
        public BetTreeInfoXml InfoXml { get; set; }

        [XmlAttribute("IsDefault")]
        public bool IsDefault { get; set; }        

        public BetTreeNodeXml()
        {
            Children = new List<BetTreeNodeXml>();
        }                
    }
}
