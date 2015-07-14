using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Rzr.Core.Editors.Partial;
using System.Collections.ObjectModel;

namespace Rzr.Core.Xml
{
    public class TreePartialVariableDefinition
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("Variable")]
        public string Variable { get; set; }

        [XmlElement("Default")]
        public float Default { get; set; }

        [XmlArray("Players")]
        public List<TreePartialPlayerVariable> Players { get; set; }

        public PartialVariableModel GetVariableModel()
        {
            PartialVariableModel vars = new PartialVariableModel();
            ObservableCollection<PartialPlayerValueModel> players = new ObservableCollection<PartialPlayerValueModel>();            
            foreach (TreePartialPlayerVariable player in Players)
                players.Add(player.GetPlayerModel());
            vars.Default = Default;
            vars.Variable = Variable;
            vars.Name = ID;
            vars.Players = players;
            return vars;
        }
    }
}
