using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Tree.Xml;
using System.Xml.Serialization;
using Rzr.Core.Editors.Partial;
using System.Collections.ObjectModel;

namespace Rzr.Core.Xml
{
    [XmlRoot("Partial")]
    public class TreePartialContainer
    {
        [XmlElement("Root")]
        public BetTreeNodeXml Root { get; set; }

        [XmlElement("Variables")]
        public TreePartialVariableContainer Variables { get; set; }
    }
}
