using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Xml
{
    [XmlType("Hook")]
    public class Hook
    {
        [XmlAttribute("File")]
        public string File { get; set; }

        [XmlAttribute("Key")]
        public string Key { get; set; }

        [XmlAttribute("Type")]
        public string Type { get; set; }
    }
}
