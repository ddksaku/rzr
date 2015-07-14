using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Rzr.Core.Editors.Partial;
using System.Collections.ObjectModel;

namespace Rzr.Core.Xml
{
    [XmlType("Variables")]
    public class TreePartialVariableContainer
    {
        [XmlElement("Variable")]
        public TreePartialVariableDefinition[] Definitions { get; set; }

        public PartialVariableListModel GetVariableListModel()
        {
            PartialVariableListModel vars = new PartialVariableListModel();
            ObservableCollection<PartialVariableModel> defs = new ObservableCollection<PartialVariableModel>();
            foreach (TreePartialVariableDefinition def in Definitions)
                defs.Add(def.GetVariableModel());
            vars.Definitions = defs;
            return vars;
        }
    }
}
