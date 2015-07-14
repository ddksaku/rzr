using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Tree.Xml;
using System.Windows.Media;

namespace Rzr.Core.Tree.DataModels
{
    public class RootNodeDataModel : BetTreeNodeDataModel
    {
        public RootNodeDataModel() : base(null)
        {
            Icon = Utilities.LoadBitmap(Properties.Resources.RootNodeIcon);
            this.InfoDisplayType = typeof(RootNodeInfo);
        }

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
            return new BetTreeNodeMetaXml() { Name = BetTreeNodeService.ROOT_NODE_META };
        }

        public override void SetAppearance(BetTreeNode betTreeNode)
        {
            betTreeNode.MainBackground.Background = new SolidColorBrush(Colors.Black);
        }
    }
}
