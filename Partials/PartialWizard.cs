using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Tree;
using Rzr.Core.Xml;
using Rzr.Core.Tree.Xml;
using Rzr.Core.Editors.Partial;

namespace Rzr.Core.Partials
{
    public interface PartialWizard
    {
        BetTreeNodeService Service { get; set; }
        
        BetTreeModel Tree { get; set; }
        
        void GenerateTree(BetTreeNodeModel node);

        PartialVariableListModel LoadVariables(BetTreeNodeModel Node);
    }
}
