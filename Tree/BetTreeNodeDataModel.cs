using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Rzr.Core.Tree.DataModels;
using Rzr.Core.Tree.Xml;
using System.Windows.Media;

namespace Rzr.Core.Tree
{
    abstract public class BetTreeNodeDataModel : DependencyObject
    {
        public static DependencyProperty BetModelProperty = DependencyProperty.Register("BetModel",
            typeof(BetTypeModel), typeof(BetTreeNodeDataModel), new PropertyMetadata(null, OnBetModelEdited));

        public BetTypeModel BetModel
        {
            get { return (BetTypeModel)this.GetValue(BetModelProperty); }
            set 
            {
                if (value != null)
                    value.BetTypeChanged += OnBetModelEdited;
                this.SetValue(BetModelProperty, value); 
            }
        }

        protected static void OnBetModelEdited(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetTreeNodeDataModel model = sender as BetTreeNodeDataModel;
            if (model != null && model.BetModelEdited != null) model.BetModelEdited();
        }

        protected void OnBetModelEdited()
        {
            if (BetModelEdited != null) BetModelEdited();
        }

        public event EmptyEventHandler BetModelEdited;

        public BetTreeNodeDataModel(BetTypeModel betModel)
        {
            BetModel = betModel;
        }

        public void ApplyPolicy(BetPolicy policy)
        {
            if (this.BetModel == null) return;

            if (!policy.AllowedChildActions.Contains(this.BetModel.BetType))
            {
                switch (this.BetModel.BetType)
                {
                    case BetAction.Fold:
                        if (policy.Required == 0)
                            this.BetModel.BetType = BetAction.Check;
                        break;
                    case BetAction.Check:
                        if (policy.Required > 0)
                            this.BetModel.BetType = BetAction.Fold;
                        break;
                    case BetAction.Call:
                        if (policy.Required == 0)
                            this.BetModel.BetType = BetAction.Check;
                        else if (this.BetModel.BetAmount != policy.Required)
                            this.BetModel.BetAmount = policy.Required;
                        break;
                    case BetAction.Bet:
                        if (policy.Required > 0)
                            this.BetModel.BetType = BetAction.Raise;
                        break;
                    case BetAction.Raise:
                        if (policy.Required == 0)
                            this.BetModel.BetType = BetAction.Bet;
                        break;
                }
            }
        }

        abstract public object GetEditContext();

        abstract public void Save(object dataObject);

        abstract public BetTreeDataEvaluator GetEvaluator();

        abstract public void LoadFromXml(BetTreeNodeMetaXml xml);

        abstract public Xml.BetTreeNodeMetaXml SaveToXml();

        public ImageSource Icon { get; protected set; }

        public Type InfoDisplayType { get; set; }

        public virtual void SetAppearance(BetTreeNode betTreeNode)
        {

        }
    }
}
