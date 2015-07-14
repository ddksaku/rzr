using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Xml
{
    [XmlType("Attr")]
    public class ParmXml
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Key")]
        public string Key { get; set; }

        [XmlAttribute("ParentKey")]
        public string ParentKey { get; set; }

        [XmlAttribute("Type")]
        public string Type { get; set; }

        [XmlText()]
        public string Value { get; set; }
    }
}
