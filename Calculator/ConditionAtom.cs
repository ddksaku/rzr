using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Calculator
{
    [XmlType("Atom")]
    public class ConditionAtom
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Primary")]
        public ulong PrimaryMask { get; set; }

        [XmlAttribute("Type")]
        public ConditionAtomType Type { get; set; }

        [XmlAttribute("LinkId")]
        public string LinkedContainerId { get; set; }

        [XmlIgnore()]
        public ConditionContainer LinkedContainer { get; set; }
    }

    public enum ConditionAtomType
    {
        Standard,
        Nested
    }
}
