using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Rzr.Core.Tree.DataEvaluators;
using Rzr.Core.Tree.Xml;
using System.Collections.ObjectModel;

namespace Rzr.Core.Tree.DataModels
{
    public class ManualResultsModel : BetTreeNodeDataModel
    {
        #region initialise

        public BetTreeNodeModel Parent { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="table">The table upon which the players for this scenario are seated</param>
        /// <param name="round">The current round of betting</param>
        /// <param name="policy">Policy information dictating what the player can and can't do</param>
        public ManualResultsModel(BetTreeNodeModel model)
            : base(null)
        {
            Parent = model;
            Icon = Utilities.LoadBitmap(Properties.Resources.ResultsNodeIcon);
            InfoDisplayType = typeof(ResultNodeInfo);
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
            float[] amounts = new float[Parent.Tree.Table.Seats.Count];
            if (Parent.Info.PlayerResults != null)
                amounts = Parent.Info.PlayerResults.Select(x => x.ExpectedValue).ToArray();
            return new ResultsEvaluator(amounts);
        }

        public override void LoadFromXml(BetTreeNodeMetaXml xml)
        {

        }

        public override BetTreeNodeMetaXml SaveToXml()
        {
            return new BetTreeNodeMetaXml() { Name = BetTreeNodeService.RESULT_NODE_META };
        }
    }

    public class ManualResultsItem : DependencyObject
    {
        public static DependencyProperty PlayerNameProperty = DependencyProperty.Register("PlayerName",
            typeof(string), typeof(ManualResultsItem), new PropertyMetadata(null, null));

        public static DependencyProperty PlayerBetProperty = DependencyProperty.Register("PlayerBet",
            typeof(float), typeof(ManualResultsItem), new PropertyMetadata(0f, null));

        public static DependencyProperty PlayerStackProperty = DependencyProperty.Register("PlayerStack",
            typeof(float), typeof(ManualResultsItem), new PropertyMetadata(0f, null));

        public static DependencyProperty PlayerEVProperty = DependencyProperty.Register("PlayerEV",
            typeof(float), typeof(ManualResultsItem), new PropertyMetadata(0f, null));

        public string PlayerName
        {
            get { return (string)this.GetValue(PlayerNameProperty); }
            set { this.SetValue(PlayerNameProperty, value); }
        }

        public float PlayerBet
        {
            get { return (float)this.GetValue(PlayerBetProperty); }
            set { this.SetValue(PlayerBetProperty, value); }
        }

        public float PlayerStack
        {
            get { return (float)this.GetValue(PlayerStackProperty); }
            set { this.SetValue(PlayerStackProperty, value); }
        }

        public float PlayerEV
        {
            get { return (float)this.GetValue(PlayerEVProperty); }
            set { this.SetValue(PlayerEVProperty, value); }
        }
    }
}
