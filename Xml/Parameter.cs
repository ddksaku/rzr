using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Xml
{
    [XmlType("Var")]
    public class Parameter
    {
        [XmlAttribute("Key")]
        public string Key { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }
    }
}
