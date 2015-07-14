using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Editors.Player;
using Rzr.Core.Tree.DataModels;

namespace Rzr.Core.Tree
{
    public class BetTreeDataNodeMeta
    {
        public string Name { get; private set; }

        public Type Type { get; private set; }

        public BetTreeNodeDataModel Model { get; private set; }

        public BetTreeDataNodeMeta(string name, BetTreeNodeDataModel model)
        {
            Name = name;
            Type = model.GetType();
            Model = model;
        }
    }
}
