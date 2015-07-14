using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Rzr.Core.Tree.DataEvaluators;
using Rzr.Core.Tree.Xml;

namespace Rzr.Core.Tree.DataModels
{
    public class ResultsModel : BetTreeNodeDataModel
    {
        #region property declarations

        public HandSnapshotModel Snapshot { get; private set; }

        #endregion

        #region initialise

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="table">The table upon which the players for this scenario are seated</param>
        /// <param name="round">The current round of betting</param>
        /// <param name="policy">Policy information dictating what the player can and can't do</param>
        public ResultsModel(HandSnapshotModel model) : base(null)
        {
            Snapshot = model;
            Icon = Utilities.LoadBitmap(Properties.Resources.ResultsNodeIcon);
            InfoDisplayType = typeof(ResultNodeInfo);
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
            ResultsModel model = sender as ResultsModel;
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

        #endregion

        public override object GetEditContext()
        {
            return this;
        }

        public override void Save(object dataObject)
        {
            
        }

        public override BetTreeDataEvaluator GetEvaluator()
        {
            float[] winAmounts = new float[Snapshot.Stacks.Length];
            for (int i = 0; i < winAmounts.Length; i++)
            {
                winAmounts[i] = -Snapshot.Bets[i];
                if (Snapshot.Status[i] != ActiveStatus.HasFolded)
                    winAmounts[i] += Snapshot.Bets.Sum();
            }
            return new ResultsEvaluator(winAmounts);
        }

        public override void LoadFromXml(BetTreeNodeMetaXml xml)
        {

        }

        public override BetTreeNodeMetaXml SaveToXml()
        {
            return new BetTreeNodeMetaXml() { Name = BetTreeNodeService.RESULT_NODE_META };
        }
    }
}
