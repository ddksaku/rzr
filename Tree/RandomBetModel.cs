using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Editors.Player;
using Rzr.Core.Tree.DataModels;
using System.Windows;
using Rzr.Core.Tree.Xml;
using System.Windows.Media;

namespace Rzr.Core.Tree
{
    public class RandomBetModel: BetTreeNodeDataModel
    {
        #region property declarations

        public int Random { get; private set; }

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
        public RandomBetModel(PlayerModel player, HoldemHandRound round, BetTypeModel betType)
            : base(betType)
        {
            Player = player;
            Round = round;
            Random = 100;
            Icon = Utilities.LoadBitmap(Properties.Resources.RandomBetIcon);
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
            RandomBetModel model = sender as RandomBetModel;
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
            string randomString = xml.Parms.Find(x => x.Name == "Random").Value;
            Random = Convert.ToInt32(randomString);
            BetModel.LoadFromXml(xml.BetXml);            
        }

        public override BetTreeNodeMetaXml SaveToXml()
        {
            BetTreeNodeMetaXml ret = new BetTreeNodeMetaXml();
            BetTreeNodeParmXml randomParm = new BetTreeNodeParmXml();
            randomParm.Name = "Random";
            randomParm.Value = Random.ToString();
            ret.Parms.Add(randomParm);
            ret.BetXml = BetModel.SaveToXml();
            return ret;
            
        }
    }
}
