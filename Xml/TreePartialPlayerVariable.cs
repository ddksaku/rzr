using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Rzr.Core.Editors.Partial;
using Rzr.Core.Tree;
using Rzr.Core.Editors.Player;

namespace Rzr.Core.Xml
{
    public class TreePartialPlayerVariable
    {
        [XmlAttribute("Index")]
        public int Index { get; set; }

        [XmlAttribute("Value")]
        public float Value { get; set; }

        public PartialPlayerValueModel GetPlayerModel()
        {
            PartialPlayerValueModel player = new PartialPlayerValueModel();
            player.Index = Index;
            return player;
        }
    }
}
