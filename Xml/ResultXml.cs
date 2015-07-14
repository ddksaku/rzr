using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Xml
{
    [XmlType("Result")]
    public class ResultXml
    {
        [XmlElement("ResultClass")]
        public string ResultClass { get; set; }
    }
}
