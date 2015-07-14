using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Data
{
    [XmlType("Mask")]
    public class HandValueDefinition
    {
        [XmlAttribute("Value")]
        public ulong Mask { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }

        [XmlAttribute("Rank")]
        public int Rank { get; set; }
    }

    [XmlRoot("Root")]
    public class HandValueDefinitionList
    {
        [XmlElement("Mask")]
        public HandValueDefinition[] Definitions { get; set; }
    }
}
