using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Editors.Player;
using Rzr.Core.Tree.Xml;

namespace Rzr.Core.Tree.DataModels
{
    public class DefaultNodeDataModel: BetTreeNodeDataModel
    {
        #region property declarations

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
        public DefaultNodeDataModel(PlayerModel player, HoldemHandRound round, BetTypeModel betType)
            : base(betType)
        {
            Player = player;
            Round = round;
            Icon = Utilities.LoadBitmap(Properties.Resources.DefaultNodeIcon);
            InfoDisplayType = typeof(BetTreeNodeInfo);
        }

        #endregion

        public override object GetEditContext()
        {
            throw new NotImplementedException();
        }

        public override void Save(object dataObject)
        {
            throw new NotImplementedException();
        }

        public override BetTreeDataEvaluator GetEvaluator()
        {
            return null;
        }

        public override void LoadFromXml(BetTreeNodeMetaXml xml)
        {

        }

        public override BetTreeNodeMetaXml SaveToXml()
        {
            return new BetTreeNodeMetaXml() { Name = BetTreeNodeService.POSTFLOP_NODE_META };
        }
    }
}
