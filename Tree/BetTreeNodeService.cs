using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Tree.DataModels;
using Rzr.Core.Editors.Player;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Rzr.Core.Xml;
using System.Xml.Serialization;
using System.IO;
using Rzr.Core.Partials;

namespace Rzr.Core.Tree
{
    /// <summary>
    /// Service for getting new bet nodes
    /// </summary>
    public class BetTreeNodeService
    {
        public const string ROOT_NODE_META = "Root";
        public const string DEFAULT_NODE_META = "Default";
        public const string PREFLOP_NODE_META = "Preflop";
        public const string POSTFLOP_NODE_META = "Postflop";
        public const string RESULT_NODE_META = "Result";
        public const string SHOWDOWN_NODE_META = "Showdown";

        /// <summary>
        /// The default node style
        /// </summary>
        protected Dictionary<Type, BetTreeNodeDisplay> _nodeStyles;
        protected Dictionary<HoldemHandRound, BetTreeNodeStyle> _roundStyles;

        protected BetTreeNodeStyle _resultsStyle = new BetTreeNodeStyle() {
            BorderColor = new SolidColorBrush(Color.FromRgb(255, 100, 100)),
            Background = new SolidColorBrush(Color.FromRgb(100, 20, 20))
        };

        protected TreePartialMeta[] _computed = new TreePartialMeta[]
        {
            new TreePartialMeta() 
            { 
                Name = "Computed Preflop", StartRound = HoldemHandRound.PreFlop, 
                EndRound = HoldemHandRound.PreFlop, Class = typeof(PreflopPartialWizard).ToString(),
                Type = TreePartialMetaType.Computed
            }
        };

        protected PlayerModel[] _players;

        protected TreePartials _partials;

        public TreePartialMeta ActiveMeta { get; set; }

        public BetTreeNodeService(PlayerModel[] players)
        {
            _players = players;
            InitialiseNodeDisplays();
            InitialiseNodeStyles();
            LoadPartials();
        }

        protected void InitialiseNodeDisplays()
        {
            _nodeStyles = new Dictionary<Type, BetTreeNodeDisplay>();
            _nodeStyles[typeof(RootNodeDataModel)] = new BetTreeNodeDisplay() 
                { DisplayRegex = "Root", Width = new GridLength(80) };
            _nodeStyles[typeof(PreflopBetModel)] = new BetTreeNodeDisplay();
            _nodeStyles[typeof(PostFlopBetModel)] = new BetTreeNodeDisplay();
            _nodeStyles[typeof(RandomBetModel)] = new BetTreeNodeDisplay() 
                { DisplayRegex = "{Round} - {CurrentPlayer} ({CurrentPlayerStake}): {BetAction} {BetAmount} ({TotalPot})\n{Percentage}" };
            _nodeStyles[typeof(ShowdownModel)] = new BetTreeNodeDisplay()
                { DisplayRegex = "Showdown" };
            _nodeStyles[typeof(ResultsModel)] = new BetTreeNodeDisplay() 
                { DisplayRegex = "Result 1: {1}, 2: {2}, 3: {3}, 4: {4}, 5: {5}, 6: {6}" };            
        }

        protected void InitialiseNodeStyles()
        {
            _roundStyles = new Dictionary<HoldemHandRound, BetTreeNodeStyle>();
            _roundStyles[HoldemHandRound.PreFlop] = new BetTreeNodeStyle() {
                BorderColor = new SolidColorBrush(Color.FromRgb(0, 0, 200))
            };
            _roundStyles[HoldemHandRound.Flop] = new BetTreeNodeStyle() {
                BorderColor = new SolidColorBrush(Color.FromRgb(15, 80, 150))
            };
            _roundStyles[HoldemHandRound.Turn] = new BetTreeNodeStyle() {
                BorderColor = new SolidColorBrush(Color.FromRgb(30, 160, 100))
            };
            _roundStyles[HoldemHandRound.River] = new BetTreeNodeStyle() {
                BorderColor = new SolidColorBrush(Color.FromRgb(50, 240, 50))
            };
        }

        public BetTreeNodeModel GetRootNode(BetTreeModel tree, HandSnapshotModel snapshot)
        {
            BetTreeNodeStyle style = _roundStyles[snapshot.Round];  
            BetTreeNodeModel model = new BetTreeNodeModel(tree, _nodeStyles[typeof(RootNodeDataModel)], style, snapshot);

            ObservableCollection<BetTreeDataNodeMeta> metaList = new ObservableCollection<BetTreeDataNodeMeta>();
            metaList.Add(new BetTreeDataNodeMeta(ROOT_NODE_META, new RootNodeDataModel()));
            model.AllowedNodeTypes = metaList;
            model.Meta = ROOT_NODE_META;

            model.AddNode += tree.OnAddNode;
            model.EditNode += tree.OnEditNode;
            model.DeleteNode += tree.OnDeleteNode;
            model.ShowWizard += tree.OnShowWizard;

            return model;
        }

        public BetTreeNodeModel AddChild(BetTreeModel tree, BetTreeNodeModel parent)
        {
            HoldemHandRound round = parent.Snapshot.NextPlayer == null ? 
                (HoldemHandRound)((int)parent.Snapshot.Round) + 1 : parent.Snapshot.Round;
            int? player = parent.Snapshot.NextPlayer == null ?
                parent.Snapshot.GetNextRoundStart() : parent.Snapshot.NextPlayer;                

            BetTreeNodeModel model;
            switch (round)
            {
                case HoldemHandRound.PreFlop:
                    model = AddPreflopStandard(parent, round, (int)player);
                    break;
                case HoldemHandRound.Flop:
                case HoldemHandRound.Turn:
                case HoldemHandRound.River:
                    model = AddPostflopStandard(parent, round, (int)player);
                    break;
                default:
                    model = AddShowdownStandard(parent);
                    break;
            }
            
            parent.AddChild(model);
            model.AddNode += tree.OnAddNode;
            model.EditNode += tree.OnEditNode;
            model.DeleteNode += tree.OnDeleteNode;
            model.ShowWizard += tree.OnShowWizard;
            return model;
        }

        public BetTreeNodeModel AddPreflopStandard(BetTreeNodeModel parent, HoldemHandRound round, int player)
        {
            BetTypeModel betModel = new BetTypeModel(parent.Policy);
            ObservableCollection<BetTreeDataNodeMeta> metaList = new ObservableCollection<BetTreeDataNodeMeta>();            
            BetTreeNodeStyle style = _roundStyles[round]; 
            BetTreeNodeModel model = new BetTreeNodeModel(parent.Tree, _nodeStyles[typeof(PreflopBetModel)], style, parent.Snapshot, parent);

            betModel.AssociatePolicy(model);
            metaList.Add(new BetTreeDataNodeMeta(PREFLOP_NODE_META, new PreflopBetModel(_players[player], round, betModel)));
            metaList.Add(new BetTreeDataNodeMeta(RESULT_NODE_META, new ManualResultsModel(model)));
            metaList.Add(new BetTreeDataNodeMeta(SHOWDOWN_NODE_META, new ShowdownModel(parent.Snapshot)));
            
            model.AllowedNodeTypes = metaList;
            model.Meta = PREFLOP_NODE_META;
            
            return model;
        }

        protected BetTreeNodeModel AddPostflopStandard(BetTreeNodeModel parent, HoldemHandRound round, int player)
        {
            BetTypeModel betModel = new BetTypeModel(parent.Policy);
            BetTreeNodeStyle style = _roundStyles[round]; 
            BetTreeNodeModel model = new BetTreeNodeModel(parent.Tree, _nodeStyles[typeof(PreflopBetModel)], style, parent.Snapshot, parent);
            betModel.AssociatePolicy(model);

            ObservableCollection<BetTreeDataNodeMeta> metaList = new ObservableCollection<BetTreeDataNodeMeta>();
            metaList.Add(new BetTreeDataNodeMeta(POSTFLOP_NODE_META, new PostFlopBetModel(_players[player], round, betModel)));
            metaList.Add(new BetTreeDataNodeMeta(RESULT_NODE_META, new ManualResultsModel(model)));
            metaList.Add(new BetTreeDataNodeMeta(SHOWDOWN_NODE_META, new ShowdownModel(parent.Snapshot)));
            
            model.AllowedNodeTypes = metaList;
            model.Meta = POSTFLOP_NODE_META;

            return model;
        }

        protected BetTreeNodeModel AddShowdownStandard(BetTreeNodeModel parent)
        {
            BetTypeModel betModel = new BetTypeModel(parent.Policy);
            BetTreeNodeStyle style = _roundStyles[parent.Snapshot.Round]; 
            BetTreeNodeModel model = new BetTreeNodeModel(parent.Tree, _nodeStyles[typeof(PreflopBetModel)], style, parent.Snapshot, parent);

            ObservableCollection<BetTreeDataNodeMeta> metaList = new ObservableCollection<BetTreeDataNodeMeta>();
            metaList.Add(new BetTreeDataNodeMeta(SHOWDOWN_NODE_META, new ShowdownModel(parent.Snapshot)));
            model.AllowedNodeTypes = metaList;
            model.Meta = SHOWDOWN_NODE_META;
            
            betModel.AssociatePolicy(model);
            
            return model;
        }

        protected void AddEndPoint(BetTreeNodeModel parent)
        {
            
            BetTreeNodeDisplay display = null;
            ObservableCollection<BetTreeDataNodeMeta> metaList = new ObservableCollection<BetTreeDataNodeMeta>();
            if (parent.Snapshot.NumPlayersInHand <= 1)
            {
                metaList.Add(new BetTreeDataNodeMeta(RESULT_NODE_META, new ResultsModel(parent.Snapshot)));
                display = _nodeStyles[typeof(ResultsModel)];
            }
            else
            {
                metaList.Add(new BetTreeDataNodeMeta("Showdown", new ShowdownModel(parent.Snapshot)));
                display = _nodeStyles[typeof(ShowdownModel)];
            }

            BetTreeNodeModel model = new BetTreeNodeModel(parent.Tree, display, this._resultsStyle, parent.Snapshot, parent, true);

            model.AllowedNodeTypes = metaList;
            model.Meta = metaList.First().Name;

            parent.AddChild(model);
        }

        #region partials

        protected void LoadPartials()
        {
            string fileName = Path.Combine(RzrConfiguration.PartialsDirectory, RzrConfiguration.PartialsMaster);

            if (new FileInfo(fileName).Exists)
            {
                using (StreamReader source = new StreamReader(fileName))
                {
                    XmlSerializer serialiser = new XmlSerializer(typeof(TreePartials));
                    _partials = (TreePartials)serialiser.Deserialize(source);
                }
            }
            else
            {
                _partials = new TreePartials();
                _partials.Partials = new List<TreePartialMeta>();
            }
        }

        public void SavePartial(TreePartialContainer tree, TreePartialMeta meta)
        {
            _partials.Partials.RemoveAll(x => x.Name == meta.Name);
            _partials.Partials.Add(meta);

            SavePartials();
            SavePartialTree(tree, meta);
        }

        protected void SavePartials()
        {
            string fileName = Path.Combine(RzrConfiguration.PartialsDirectory, RzrConfiguration.PartialsMaster);

            using (StreamWriter target = new StreamWriter(fileName))
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(TreePartials));
                serialiser.Serialize(target, _partials);
            }
        }

        protected void SavePartialTree(TreePartialContainer tree, TreePartialMeta meta)
        {
            string fileName = Path.Combine(RzrConfiguration.PartialsDirectory, meta.File);

            using (StreamWriter target = new StreamWriter(fileName))
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(TreePartialContainer));
                serialiser.Serialize(target, tree);
            }
        }

        public List<TreePartialMeta> GetPartialMeta(BetTreeNodeModel model)
        {
            List<TreePartialMeta> meta = new List<TreePartialMeta>();

            HoldemHandRound currentRound = model.Snapshot.Round;
            int numPlayers = model.Snapshot.Active.Count(x => x);

            meta.AddRange(GetComputedPartials(numPlayers, currentRound));

            if (model.Snapshot.IsRoundEnd)
            {
                int? nextRound = model.Snapshot.GetNextRoundStart();
                if (nextRound != null)
                {
                    HoldemHandRound startRound = (HoldemHandRound)nextRound;
                    meta.AddRange(_partials.Partials.FindAll(x => x.StartRound == startRound && x.NumPlayers == numPlayers));
                }
            }            

            return meta;
        }

        private IEnumerable<TreePartialMeta> GetComputedPartials(int activePlayers, HoldemHandRound currentRound)
        {
            return _computed.Where(x => x.StartRound == currentRound);
        }

        #endregion

        public TreePartialContainer GetPartialTree(TreePartialMeta meta)
        {
            string fileName = Path.Combine(RzrConfiguration.PartialsDirectory, meta.File);

            using (StreamReader target = new StreamReader(fileName))
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(TreePartialContainer));
                return (TreePartialContainer)serialiser.Deserialize(target);
            }            
        }
    }
}
