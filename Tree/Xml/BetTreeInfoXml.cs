using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Tree.Xml
{
    [XmlType("Info")]
    public class BetTreeInfoXml
    {
        [XmlElement("Players")]
        public BetTreePlayerInfoXml[] Players { get; set; }
    }
}
