using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core;
using System.Xml.Serialization;
using Rzr.Core.Partials;
using Rzr.Core.Tree;

namespace Rzr.Core.Xml
{
    [XmlRoot("Root")]
    public class TreePartials
    {
        [XmlElement("Partials")]
        public List<TreePartialMeta> Partials { get; set; }
    }

    [XmlType("Partial")]
    public class TreePartialMeta
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("File")]
        public string File { get; set; }

        [XmlAttribute("Type")]
        public TreePartialMetaType Type { get; set; }

        [XmlAttribute("Class")]
        public string Class { get; set; }

        [XmlAttribute("StartRound")]
        public HoldemHandRound StartRound { get; set; }

        [XmlAttribute("EndRound")]
        public HoldemHandRound EndRound { get; set; }

        [XmlAttribute("Num")]
        public int NumPlayers { get; set; }

        [XmlAttribute("Relative")]
        public bool IsRelative { get; set; }

        [XmlAttribute("BaseValue")]
        public float BaseValue { get; set; }

        public PartialWizard GenerateWizard(BetTreeModel tree, BetTreeNodeService service)
        {
            Type type = System.Type.GetType(Class);
            PartialWizard wizard = Activator.CreateInstance(type) as PartialWizard;
            wizard.Tree = tree;
            wizard.Service = service;
            return wizard;
        }
    }

    public enum TreePartialMetaType
    {        
        Xml = 0,
        Computed = 1
    }
}
