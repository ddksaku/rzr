using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Xml
{
    [XmlType("Table")]
    public class TableXml
    {
        [XmlAttribute("Button")]
        public int Button { get; set; }

        [XmlAttribute("Size")]
        public int Size { get; set; }
    }
}
