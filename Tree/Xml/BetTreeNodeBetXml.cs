using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Rzr.Core.Tree.Xml
{
    [XmlType("Bet")]
    public class BetTreeNodeBetXml
    {
        [XmlAttribute("Amount")]
        public float BetAmount { get; set; }

        [XmlAttribute("Type")]
        public BetAction BetType { get; set; }
    }
}
