using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Rzr.Core.Tree.Xml;

namespace Rzr.Core.Tree.DataModels
{
    public class BetTypeModel : DependencyObject
    {
        #region property definitions

        public static DependencyProperty BetAmountProperty = DependencyProperty.Register("BetAmount",
            typeof(float), typeof(BetTypeModel), new PropertyMetadata(0f, OnBetTypeEdited));
        
        public static DependencyProperty BetTypeProperty = DependencyProperty.Register("BetType",
            typeof(BetAction), typeof(BetTypeModel), new PropertyMetadata(BetAction.Fold, OnBetTypeEdited));

        #endregion

        #region properties

        private float _originalBetAmount;
        private BetAction _originalBetType;

        public BetPolicy Policy { get; private set; }

        public float BetAmount 
        {
            get { return (float)this.GetValue(BetAmountProperty); }
            set { this.SetValue(BetAmountProperty, value); }
        }

        public BetAction BetType
        {
            get { return (BetAction)this.GetValue(BetTypeProperty); }
            set { this.SetValue(BetTypeProperty, value); }
        }                

        #endregion

        #region events

        public BetTypeModel(BetPolicy policy)
        {
            Policy = policy;
            if (policy.AllowedChildActions.Contains(BetAction.Check))
                BetType = BetAction.Check;
            else
                BetType = BetAction.Fold;
        }

        public event EventHandler BetTypeEdited;
        public event EmptyEventHandler BetTypeChanged;

        protected static void OnBetTypeEdited(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetTypeModel model = sender as BetTypeModel;
            model.OnBetChanged();
        }

        public void OnBetChanged()
        {
            if (Policy != null)
            {
                if (BetAmount == 0)
                {
                    BetType = Policy.MinBet == 0 ? BetAction.Check : BetAction.Fold;
                }
                else if (BetAmount < Policy.MinCall)
                {
                    BetType = BetAction.Fold;
                    BetAmount = 0;
                }
                else if (BetAmount == Policy.MinCall)
                {
                    BetType = BetAction.Call;
                }
                else if (BetAmount < Policy.MinBet)
                {
                    BetAmount = Policy.MinBet;
                }

                if (BetAmount >= Policy.MaxBet)
                {
                    BetType = BetAction.AllIn;
                    BetAmount = Policy.MaxBet;
                }
                else if (BetAmount >= Policy.MinBet && BetAmount > Policy.MinCall)
                {
                    BetType = Policy.MinCall > 0 ? BetAction.Raise : BetAction.Bet;
                }
            }

            if (BetTypeEdited != null) BetTypeEdited(this, EventArgs.Empty);
        }

        public void StartEdit()
        {
            _originalBetAmount = BetAmount;
            _originalBetType = BetType;            
        }

        public void Cancel()
        {
            BetAmount = _originalBetAmount;
            BetType = _originalBetType;
        }

        public void Save()
        {
            _originalBetAmount = BetAmount;
            _originalBetType = BetType;                        
            if (BetTypeChanged != null) BetTypeChanged();
        }

        #endregion

        public override string ToString()
        {
            if (this.BetType == BetAction.Bet || this.BetType == BetAction.Call)
                return this.BetType + " " + this.BetAmount;
            else if (this.BetType == BetAction.Raise)
                return this.BetType + " " + this.BetAmount;
            else
                return this.BetType.ToString();
        }

        public void AssociatePolicy(BetTreeNodeModel model)
        {
            if (model.Parent != null)
            model.Parent.PolicyChanged += UpdatePolicy;
            this.Policy = model.Parent.Policy;
        }

        protected void UpdatePolicy(BetPolicy policy)
        {
            this.Policy = policy;
        }

        public void LoadFromXml(BetTreeNodeBetXml xml)
        {
            if (xml != null)
            {
                this.BetType = xml.BetType;
                this.BetAmount = xml.BetAmount;                
            }
        }

        public BetTreeNodeBetXml SaveToXml()
        {
            BetTreeNodeBetXml ret = new BetTreeNodeBetXml();
            ret.BetType = this.BetType;
            ret.BetAmount = this.BetAmount;
            return ret;
        }
    }
}
