using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Editors.Player;
using System.Windows;
using Rzr.Core.Tree.DataEditors;
using Rzr.Core.Calculator;
using Rzr.Core.Data;
using Rzr.Core.Tree.DataEvaluators;
using Rzr.Core.Tree.Xml;

namespace Rzr.Core.Tree.DataModels
{
    [Editor(typeof(PostflopRangeEditor))]
    public class PostFlopBetModel : BetTreeNodeDataModel
    {
        #region property declarations

        public HandValueRangeModel Range { get; private set; }

        public HoldemHandRound Round { get; private set; }

        public PlayerModel Player { get; private set; }

        #endregion

        #region initialise

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="table">The table upon which the players for this scenario are seated</param>
        /// <param name="round">The current round of betting</param>
        /// <param name="policy">Policy information dictating what the player can and can't do</param>
        public PostFlopBetModel(PlayerModel player, HoldemHandRound round, BetTypeModel betType) : base(betType)
        {
            Player = player;
            Round = round;
            Icon = Utilities.LoadBitmap(Properties.Resources.PreflopBetIcon);
            Range = new HandValueRangeModel(round, null);
            InfoDisplayType = typeof(BetTreeNodeInfo);
        }

        #endregion

        #region events

        public event DependencyPropertyChangedEventHandler ModelUpdated;

        /// <summary>
        /// Fired when the conditions or bet data which describe the player action are changed
        /// </summary>
        public event EventHandler ActionDetailsChanged;

        /// <summary>
        /// Fired when this model is identified as an endpoint and needs to be linked to a valid one
        /// </summary>
        public event EventHandler EndpointRequired;

        public static void OnModelChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PostFlopBetModel model = sender as PostFlopBetModel;
            if (model != null && model.ModelUpdated != null) model.ModelUpdated(model, e);
        }

        /// <summary>
        /// Handler which is fired when the bet type is changed
        /// </summary>
        public void OnBetTypeChanged(object sender, EventArgs e)
        {
            if (ActionDetailsChanged != null) ActionDetailsChanged(this, EventArgs.Empty);
            if (ModelUpdated != null) ModelUpdated(this, new DependencyPropertyChangedEventArgs());
        }

        /// <summary>
        /// Handler which is fired when the bet conditions are changed
        /// </summary>
        public void OnBetConditionsChanged(object sender, EventArgs e)
        {
            if (ActionDetailsChanged != null) ActionDetailsChanged(this, EventArgs.Empty);
            if (ModelUpdated != null) ModelUpdated(this, new DependencyPropertyChangedEventArgs());
        }

        #endregion

        public void SetRange(float range, float mask, float variation)
        {
            throw new NotImplementedException();
        }

        public override object GetEditContext()
        {
            return this;
        }

        public override void Save(object dataObject)
        {
            
        }

        public override BetTreeDataEvaluator GetEvaluator()
        {
            return new PostflopEvaluator(Player.Index, Range.Range);
        }

        public override void LoadFromXml(BetTreeNodeMetaXml xml)
        {
            if (xml.Parms.Exists(x => x.Name == "Range"))
            {
                string rangeString = xml.Parms.Find(x => x.Name == "Range").Value;
                Range.LoadFromString(rangeString);
            }
            BetModel.LoadFromXml(xml.BetXml);
        }

        public override BetTreeNodeMetaXml SaveToXml()
        {
            BetTreeNodeMetaXml ret = new BetTreeNodeMetaXml();
            BetTreeNodeParmXml rangeParm = new BetTreeNodeParmXml();
            rangeParm.Name = "Range";
            rangeParm.Value = Range.SaveToString();

            // MDS Here ConditionService needs to be saved 
            ret.Parms.Add(rangeParm);
            ret.BetXml = BetModel.SaveToXml();
            ret.Name = BetTreeNodeService.POSTFLOP_NODE_META;
            return ret;
        }

    }
}
