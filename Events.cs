using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Application;
using Rzr.Core.Editors.LevelGrid;
using Rzr.Core.Editors;
using Rzr.Core.Results;
using Rzr.Core.Tree;
using Rzr.Core.Xml;
using Rzr.Core.Partials;

namespace Rzr.Core
{
    public delegate void EmptyEventHandler();

    public delegate void WidgetChangedEventHandler(Widget oldWidget, Widget newWidget);

    public delegate void DataButtonClickedEventHandler(string dataKey, object dataValue);

    public delegate void LevelGridItemHandler(LevelGridItem dataKey);

    public delegate void CardEventHandler(CardModel card);

    public delegate void BasicCalculateEvent(int numIterations);

    public delegate void BoolEventHandler(bool arg);

    public delegate void ResultsCellEventHandler(ResultsCellModel model);

    public delegate void ResultsTableCellButtonHandler(int column, int row);

    public delegate void ProgressStartedEventHandler(double min, double max, string text);

    public delegate void ProgressEventHandler(double progress);

    public delegate void ProgressFinishedEventHandler();

    public delegate BetTreeNodeModel AddNodeHandler(BetTreeNodeModel nodeModel);

    public delegate void BetNodeHandler(BetTreeNodeModel nodeModel);

    public delegate void BetPolicyEvent(BetPolicy policy);

    public delegate void TreePartialMetaEventHandler(TreePartialMeta meta);

    public delegate void PartialGeneratorHandler(PartialGenerator container);
}
