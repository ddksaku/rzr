using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Xml
{
    [XmlType("OptionSet")]
    public class PlayerOptionSetXml
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlArray("Set")]
        public ParmXml[] Parms { get; set; }
    }
}
