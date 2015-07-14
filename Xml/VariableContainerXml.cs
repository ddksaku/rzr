using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Editors.Variables;
using System.Xml.Serialization;

namespace Rzr.Core.Xml
{
    [XmlType("Variables")]
    public class VariableContainerXml
    {
        [XmlArray("Groups")]
        public VariableGroupXml[] Groups { get; set; }

        public VariableContainerXml()
        {
            Groups = new VariableGroupXml[] {};
        }
    }

    [XmlType("Group")]
    public class VariableGroupXml
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("ShortName")]
        public string ShortName { get; set; }

        [XmlArray("Vars")]
        public VariableXml[] Variables { get; set; }

        public VariableGroupXml()
        {
            Variables = new VariableXml[] {};
        }
    }

    [XmlType("Variable")]
    public class VariableXml
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlAttribute("Type")]
        public VariableType Type { get; set; }

        [XmlElement("Value")]
        public float Value { get; set; }
    }

}
