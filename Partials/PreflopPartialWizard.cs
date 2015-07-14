using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Tree;
using Rzr.Core.Xml;
using Rzr.Core.Tree.DataModels;
using Rzr.Core.Editors.Partial;
using System.Collections.ObjectModel;

namespace Rzr.Core.Partials
{
    public class PreflopPartialWizard : PartialWizard
    {
        public const int BETRANGE = 0;
        public const int CALLRANGE = 1;
        public const int BETAMOUNT = 2;

        public TreePartialVariableDefinition[][] _variables = new TreePartialVariableDefinition[][]
        {
            new TreePartialVariableDefinition[] 
            { 
                new TreePartialVariableDefinition() { ID = "Open Raise", Variable = "OpenRaise%" },
                new TreePartialVariableDefinition() { ID = "Cold Call Open", Variable = "ColdCall%" },
                new TreePartialVariableDefinition() { ID = "Open Raise Amount", Variable = "OpenRaise$", Default = 1 }
            },
            new TreePartialVariableDefinition[] 
            { 
                new TreePartialVariableDefinition() { ID = "3Bet", Variable = "3Bet%" },
                new TreePartialVariableDefinition() { ID = "Call 3Bet", Variable = "Call3Bet%" },
                new TreePartialVariableDefinition() { ID = "3Bet Amount", Variable = "3Bet$", Default = 1 }
            },
            new TreePartialVariableDefinition[] 
            {
                new TreePartialVariableDefinition() { ID = "4Bet", Variable = "4Bet%" },
                new TreePartialVariableDefinition() { ID = "Call 4Bet", Variable = "Call4Bet%" },
                new TreePartialVariableDefinition() { ID = "4Bet Amount", Variable = "4Bet$", Default = 1 }
            },
            new TreePartialVariableDefinition[] 
            { 
                new TreePartialVariableDefinition() { ID = "5Bet", Variable = "5Bet%" },
                new TreePartialVariableDefinition() { ID = "Call 5Bet", Variable = "Call5Bet%" },
                new TreePartialVariableDefinition() { ID = "5Bet Amount", Variable = "5Bet$", Default = 1 }
            },
            new TreePartialVariableDefinition[] 
            { 
                new TreePartialVariableDefinition() { ID = "All In", Variable = "AllIn%" }
            },
        };

        public BetTreeNodeService Service { get; set; }
        public BetTreeModel Tree { get; set; }
        public PartialVariableListModel Variables { get; set; }

        public PartialVariableListModel LoadVariables(BetTreeNodeModel Node)
        {
            int numPlayers = Node.Snapshot.Active.Count(x => x);
            Variables = new PartialVariableListModel();
            ObservableCollection<PartialVariableModel> defs = new ObservableCollection<PartialVariableModel>();
            foreach (TreePartialVariableDefinition[] defSet in _variables)
            {
                foreach (TreePartialVariableDefinition defSingle in defSet)
                {
                    PartialVariableModel var = new PartialVariableModel() { Name = defSingle.ID, Variable = defSingle.Variable, Default = defSingle.Default };                    
                    ObservableCollection<PartialPlayerValueModel> players = new ObservableCollection<PartialPlayerValueModel>();
                    for (int i = 0; i < numPlayers; i++)
                        players.Add(new PartialPlayerValueModel() { Index = i });
                    var.Players = players;
                    defs.Add(var);
                }
            }

            Variables.Definitions = defs;
            return Variables;
        }

        public void GenerateTree(BetTreeNodeModel parent)
        {
            int level = 0;
            AddChildNodes(parent, level, 3);
        }

        private void AddChildNodes(BetTreeNodeModel node, int level, int maxLevel)
        {
            if (node.Snapshot.IsRoundEnd || node.Snapshot.IsHandEnd) return;

            int activePlayer = 0;
            float potAmount = node.Snapshot.Bets.Sum();
            float callAmount = node.Policy.MinCall;
            float basePot = potAmount + callAmount;

            if (level < maxLevel && node.Policy.AllowedChildActions.Contains(BetAction.Bet))
            {                
                BetTreeNodeModel child = Service.AddChild(Tree, node);

                PartialVariableModel var = Variables.Definitions.First(x => x.Name == _variables[level][BETAMOUNT].ID);
                child.Data.BetModel.BetAmount = callAmount + (basePot * var.Players[activePlayer].Value.Value);
                child.Data.BetModel.Save();
                AddChildNodes(child, level + 1, maxLevel);
            }
            else if (level < maxLevel && node.Policy.AllowedChildActions.Contains(BetAction.Raise))
            {
                BetTreeNodeModel child = Service.AddChild(Tree, node);

                PartialVariableModel var = Variables.Definitions.First(x => x.Name == _variables[level][BETAMOUNT].ID);
                child.Data.BetModel.BetAmount = callAmount + (basePot * var.Players[activePlayer].Value.Value);
                child.Data.BetModel.Save();
                AddChildNodes(child, level + 1, maxLevel);
            }

            if (node.Policy.AllowedChildActions.Contains(BetAction.Call))
            {
                BetTreeNodeModel child = Service.AddChild(Tree, node);

                child.Data.BetModel.BetAmount = callAmount;
                child.Data.BetModel.Save();
                AddChildNodes(child, level, maxLevel);
            }
            else if (node.Policy.AllowedChildActions.Contains(BetAction.Check))
            {
                BetTreeNodeModel child = Service.AddChild(Tree, node);
                child.Data.BetModel.BetAmount = 0;
                child.Data.BetModel.Save();
                AddChildNodes(child, level, maxLevel);
            }

            if (node.Policy.AllowedChildActions.Contains(BetAction.Fold))
            {
                BetTreeNodeModel child = Service.AddChild(Tree, node);
                child.Data.BetModel.BetAmount = 0;
                child.Data.BetModel.Save();
                AddChildNodes(child, level, maxLevel);
            }
        }
    }
}
