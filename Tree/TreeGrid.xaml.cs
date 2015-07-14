using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rzr.Core.Tree
{
    /// <summary>
    /// Interaction logic for TreeGrid.xaml
    /// </summary>
    public partial class TreeGrid : UserControl
    {
        #region properties

        protected BetTreeModel _model;
        protected List<TreeGridItem> _items;
        protected Dictionary<HoldemHandRound, Canvas> _canvases;
        protected Dictionary<TreeGridItem, TreeGridStart> _grids;
        protected Dictionary<Canvas, ColumnDefinition> _columns;
        protected TreeGridStart StartGrid;
        protected BetTreeNodeModel _sourceNode;
        protected List<TreeGridLine> _lines;

        #endregion

        #region constructor

        /// <summary>
        /// Initialise
        /// </summary>
        public TreeGrid()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            SetModel(null, new DependencyPropertyChangedEventArgs());

            _lines = new List<TreeGridLine>();
            _items = new List<TreeGridItem>();
            _grids = new Dictionary<TreeGridItem, TreeGridStart>();
            _canvases = new Dictionary<HoldemHandRound, Canvas>();
            _canvases[HoldemHandRound.PreFlop] = PreflopCanvas;
            _canvases[HoldemHandRound.Flop] = FlopCanvas;
            _canvases[HoldemHandRound.Turn] = TurnCanvas;
            _canvases[HoldemHandRound.River] = RiverCanvas;            

            _columns = new Dictionary<Canvas, ColumnDefinition>();
            _columns[PreflopCanvas] = Preflop;
            _columns[FlopCanvas] = Flop;
            _columns[TurnCanvas] = Turn;
            _columns[RiverCanvas] = River;

            StartGrid = new TreeGridStart();
            PreflopCanvas.Children.Add(StartGrid);
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = this.DataContext as BetTreeModel;
            if (_model == null) return;

            _model.NodeAdded += NodeAdded;
            _model.NodeDeleted += NodeDeleted;
        }

        public void SetRootNode(BetTreeNodeModel node)
        {
            foreach (HoldemHandRound round in Enum.GetValues(typeof(HoldemHandRound)))
                ClearGrid(round);
            
            _sourceNode = node;
            SetGridStartForRound(node.Snapshot.Round);

            RefreshNodes(_sourceNode);
        }

        public void RefreshNodes(BetTreeNodeModel node)
        {
            NodeAdded(node);
            InitialiseNode(node);
        }

        private void SetGridStartForRound(HoldemHandRound round)
        {
            ((Canvas)StartGrid.Parent).Children.Remove(StartGrid);
            
            StartGrid = new TreeGridStart();
            _canvases[round].Children.Add(StartGrid);
        }

        private void ClearGrid(HoldemHandRound round)
        {
            foreach (UIElement el in _canvases[round].Children)
            {
                if (el is TreeGridStart)
                {
                    TreeGridStart start = el as TreeGridStart;
                    List<TreeGridItem> remove = new List<TreeGridItem>();
                    foreach (TreeGridItem item in start.MainGrid.Children)
                        remove.Add(item);

                    foreach (TreeGridItem item in remove)
                        RemoveItem(start.MainGrid, item);   
                }
            }
        }

        protected void InitialiseNode(BetTreeNodeModel node)
        {            
            foreach (BetTreeNodeModel child in node.Children)
            {
                NodeAdded(child);
                InitialiseNode(child);                
            }
        }

        #endregion

        #region add node

        /// <summary>
        /// Updates the visual layout of the tree after a node has been added to the tree model
        /// </summary>
        /// <param name="node">The node which has been added</param>
        /// <returns>The node which has been added</returns>
        protected BetTreeNodeModel NodeAdded(BetTreeNodeModel node)
        {
            //-------------------------------------------------------------------------------------
            // If there is already a display for this node, then no need to continue here
            //-------------------------------------------------------------------------------------
            TreeGridItem nodeDisplay = GetDisplay(node);
            if (nodeDisplay != null) return node;

            TreeGridItem parentDisplay = GetDisplay(node.Parent);
            
            //-------------------------------------------------------------------------------------
            // If there is no parent node or the parent node is a different round, then we need
            // to add the visual component to a round root. Otherwise, we just add the visual 
            // component to it's parent
            //-------------------------------------------------------------------------------------
            if (parentDisplay == null || node.Parent.Snapshot.Round != node.Snapshot.Round)
            {
                AddRootNode(node, parentDisplay);
            }
            else
            {
                AddStandardNode(node, parentDisplay);
            }                

            return node;
        }

        /// <summary>
        /// Adds a node to the root of a round section
        /// </summary>
        /// <param name="node">The node to be added to the root</param>
        /// <param name="parentDisplay">The node parent from the previous round, or null of this is the first round</param>
        protected void AddRootNode(BetTreeNodeModel node, TreeGridItem parentDisplay)
        {
            TreeGridStart start = (parentDisplay == null) ? StartGrid : GetGrid(parentDisplay, null);
            if (start == null)
            {                
                start = GetNewGridStart(node, parentDisplay);
            }

            TreeGridItem item = GetNewRootItem(node);
            AppendItemToGrid(start.MainGrid, item);
            UpdateGridDimensions(node, item, parentDisplay);
            UpdateCanvas(node.Snapshot.Round);
        }

        /// <summary>
        /// Adds a node to the child grid of its parent
        /// </summary>
        /// <param name="node">The node to be added</param>
        /// <param name="parentDisplay">The parent grid to which the node is added</param>
        protected void AddStandardNode(BetTreeNodeModel node, TreeGridItem parentDisplay)
        {
            Grid parentGrid = parentDisplay.Children;
            TreeGridItem item = GetNewItem(node);
            AppendItemToGrid(parentGrid, item);
        }

        /// <summary>
        /// Get a new grid item and add the appropriate event handlers
        /// </summary>
        /// <param name="node">The node model from which the visual component should be derived</param>
        /// <returns>Visual representation of a node</returns>
        public TreeGridItem GetNewItem(BetTreeNodeModel node)
        {
            TreeGridItem item = new TreeGridItem();
            item.DoFixLayout += this.UpdateDimensions;
            item.ItemLayoutUpdated += this.ItemLayoutUpdated;
            _items.Add(item);

            item.DataContext = node;
            node.ExpandCollapse += DoExpandCollapse;

            return item;
        }

        /// <summary>
        /// Get a new round root item and add the appropriate event handlers
        /// </summary>
        /// <param name="node">The node model from which the visual component should be derived</param>
        /// <returns>Visual representation of a node</returns>
        public TreeGridItem GetNewRootItem(BetTreeNodeModel node)
        {
            TreeGridItem item = new TreeGridItem();
            item.DoFixLayout += this.UpdateDimensions;
            item.ItemLayoutUpdated += this.RootItemLayoutUpdated;
            _items.Add(item);
            item.LayoutUpdated += this.RefreshCanvas;

            item.DataContext = node;
            node.ExpandCollapse += DoExpandCollapse;
            return item;
        }

        /// <summary>
        /// Append an item to it's parent and update the layout
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="item"></param>
        public void AppendItemToGrid(Grid grid, TreeGridItem item)
        {
            RowDefinition row = new RowDefinition();
            grid.RowDefinitions.Add(row);
            grid.Children.Add(item);
            Grid.SetColumn(item, 0);
            Grid.SetRow(item, grid.RowDefinitions.Count - 1);
            item.HorizontalAlignment = HorizontalAlignment.Left;
            item.VerticalAlignment = VerticalAlignment.Top;


            TreeGridItem parent = GetDisplay(item.Model.Parent);

            if (parent != null)
            {
                Brush stroke = new SolidColorBrush(Color.FromArgb((byte)100, (byte)0, (byte)0, (byte)0.5));

                TreeGridLine line = new TreeGridLine()
                {
                    Line1 = new Line() { Stroke = stroke, StrokeThickness = 2 },
                    Line2 = new Line() { Stroke = stroke, StrokeThickness = 2 },
                    Line3 = new Line() { Stroke = stroke, StrokeThickness = 2 },
                    Start = parent,
                    End = item
                };

                UnderlayCanvas.Children.Add(line.Line1);
                UnderlayCanvas.Children.Add(line.Line2);
                UnderlayCanvas.Children.Add(line.Line3);
                _lines.Add(line);
            }

            UpdateDimensions(item, true);

            row.Height = new GridLength(item.Height);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parentDisplay"></param>
        /// <returns></returns>
        protected TreeGridStart GetNewGridStart(BetTreeNodeModel node, TreeGridItem parentDisplay)
        {
            TreeGridStart start = new TreeGridStart();
            start.LinkedItem = parentDisplay;
            start.LayoutUpdated += this.RefreshCanvas;
            Canvas canvas = _canvases[node.Snapshot.Round];
            canvas.Children.Add(start);
            _grids[parentDisplay] = start;
            return start;
        }

        #endregion

        #region delete node

        protected void NodeDeleted(BetTreeNodeModel node)
        {
            TreeGridItem childDisplay = _items.Find(x => x.DataContext == node);
            TreeGridItem parentDisplay = _items.Find(x => x.DataContext == node.Parent);

            bool found = (parentDisplay == null) ? false : RemoveItem(parentDisplay.Children, childDisplay);

            if (!found)
            {
                TreeGridStart start = GetGrid(parentDisplay, StartGrid);
                RemoveItem(start.MainGrid, childDisplay);
            }

            foreach (BetTreeNodeModel child in node.Children)
                RemoveFromChildCanvases(child, childDisplay);

            UpdateDimensions(parentDisplay, true);
            UpdateCanvas(childDisplay.Model.Snapshot.Round);            
        }

        public void RefreshCanvas(object sender, EventArgs e)
        {
            foreach (HoldemHandRound round in Enum.GetValues(typeof(HoldemHandRound)))
                UpdateCanvas(round);
        }

        private void RemoveFromChildCanvases(BetTreeNodeModel node, TreeGridItem parentDisplay)
        {
            TreeGridItem nodeDisplay = _items.Find(x => x.DataContext == node);

            foreach (BetTreeNodeModel child in node.Children)
                RemoveFromChildCanvases(child, nodeDisplay);
            
            TreeGridStart start = GetGrid(parentDisplay, null);
            if (start != null)
                RemoveItem(start.MainGrid, nodeDisplay);
        }

        protected bool RemoveItem(Grid parent, TreeGridItem child)
        {
            bool found = false;
            for (int i = 0; i < parent.Children.Count; i++)
            {
                if (parent.Children[i] == child)
                {
                    parent.Children.RemoveAt(i);
                    parent.RowDefinitions.RemoveAt(i);
                    found = true;

                    if (i < parent.Children.Count)
                        Grid.SetRow(parent.Children[i], i);
                }
                else if (found)
                {
                    Grid.SetRow(parent.Children[i], i);
                }
            }

            List<TreeGridLine> lines = _lines.Where(x => x.Start == child || x.End == child).ToList();
            foreach (TreeGridLine line in lines)
            {
                UnderlayCanvas.Children.Remove(line.Line1);
                UnderlayCanvas.Children.Remove(line.Line2);
                UnderlayCanvas.Children.Remove(line.Line3);
                _lines.Remove(line);                
            }

            return found;
        }

        #endregion

        #region expand and collapse

        protected void DoExpandCollapse(BetTreeNodeModel model)
        {
            foreach (BetTreeNodeModel child in model.Children)
                SetExpanded(child, model.IsExpanded);

            UpdateCanvas(model.Snapshot.Round);            
        }

        protected void SetExpanded(BetTreeNodeModel model, bool parentExpanded)
        {
            TreeGridItem display = GetDisplay(model);
            Visibility visibility = parentExpanded ? Visibility.Visible : Visibility.Collapsed  ;
            if (visibility != display.Visibility)
            {
                display.Visibility = visibility;
                UpdateDimensions(display, true);
            }

            foreach (BetTreeNodeModel child in model.Children)
                SetExpanded(child, parentExpanded && model.IsExpanded);            
        }        

        #endregion

        #region update layout

        /// <summary>
        /// Event handler for when an item's layout is updated; updates the parent
        /// </summary>
        /// <param name="item">The item which was updated</param>
        protected void ItemLayoutUpdated(TreeGridItem item)
        {
            BetTreeNodeModel node = item.DataContext as BetTreeNodeModel;
            TreeGridItem parentDisplay = _items.Find(x => x.DataContext == node.Parent);
            
            if (parentDisplay != null)
            {
                UpdateDimensions(parentDisplay, true);                
            }

            UpdateCanvas(item.Model.Snapshot.Round);
        }

        protected void RootItemLayoutUpdated(TreeGridItem item)
        {
            BetTreeNodeModel node = item.DataContext as BetTreeNodeModel;
            TreeGridItem parentDisplay = _items.Find(x => x.DataContext == node.Parent);

            UpdateGridDimensions(node, item, parentDisplay);

            if (parentDisplay != null)
            {
                UpdateDimensions(parentDisplay, true);
            }

            UpdateCanvas(item.Model.Snapshot.Round);
        }

        private void UpdateGridDimensions(BetTreeNodeModel node, TreeGridItem itemDisplay, TreeGridItem parentDisplay)
        {
            TreeGridStart grid = GetGrid(parentDisplay, StartGrid);
            double height = 0, width = 0;

            for (int i = 0; i < grid.MainGrid.Children.Count; i++)
            {
                TreeGridItem child = grid.MainGrid.Children[i] as TreeGridItem;
                if (child.Visibility == Visibility.Visible)
                {
                    if (child.Width > width) width = child.Width;
                    height += child.Height;
                }
                if (child == itemDisplay)
                {
                    grid.MainGrid.RowDefinitions[i].Height = new GridLength(child.Height);
                }
            }

            grid.MainGrid.ColumnDefinitions[0].Width = new GridLength(width);
            grid.Width = width;
            grid.Height = height;
        }

        /// <summary>
        /// Update the dimensions of a single grid item
        /// </summary>
        /// <param name="item">The item to be updated</param>
        /// <param name="expanded">Whether or not the item is expanded</param>
        public void UpdateDimensions(TreeGridItem item, bool expanded)
        {
            double childGridWidth = 0, childGridHeight = 0;

            for (int i = 0; expanded && i < item.Model.Children.Count; i++)
            {
                TreeGridItem child = GetDisplay(item.Model.Children[i]);                

                if (child != null)
                {
                    if (child.Visibility != Visibility.Visible) continue;

                    if (child.Width > childGridWidth && child.Model.Snapshot.Round == item.Model.Snapshot.Round)
                        childGridWidth = child.Width;

                    if (i < item.Children.RowDefinitions.Count)
                        item.Children.RowDefinitions[i].Height = new GridLength(child.Height);

                    childGridHeight += child.Height;
                }
            }

            item.FixLayout(childGridWidth, childGridHeight);
        }        

        protected void UpdateCanvas(HoldemHandRound round)
        {
            UpdateNodePositions(round);
            UpdateNodeLines(round);
        }

        protected void UpdateNodePositions(HoldemHandRound round)
        {
            Canvas canvas = _canvases[round];

            double width = 0;
            double height = 0;
            foreach (TreeGridStart child in canvas.Children)
            {
                if (child.Width > width) width = child.Width;
                double itemTop = child.LinkedItem == null ? height : GetGridStartTop(child.LinkedItem);
                Canvas.SetTop(child, itemTop);
                Canvas.SetLeft(child, 0);
                height += child.Height;
            }
            canvas.Width = width;
            canvas.Height = height;

            double colWidth = canvas.Width < 50 ? 50 : canvas.Width;
            _columns[canvas].Width = new GridLength(colWidth);
        }

        protected void UpdateNodeLines(HoldemHandRound round)
        {
            foreach (TreeGridLine line in _lines)
            {
                UIElement startElement = line.Start.GetLineStart();
                UIElement endElement = line.End.GetLineEnd();

                Point start = startElement.TranslatePoint(new Point(0, 0), UnderlayCanvas);
                Point end = endElement.TranslatePoint(new Point(0, 0), UnderlayCanvas);

                double startX = start.X + startElement.RenderSize.Width / 2;
                double startY = start.Y + startElement.RenderSize.Height / 2;
                double endX = end.X + endElement.RenderSize.Width / 2;
                double endY = end.Y + endElement.RenderSize.Height / 2;
                double offset = 20;

                line.Line1.X1 = startX;
                line.Line1.Y1 = startY;
                line.Line1.X2 = endX - offset;
                line.Line1.Y2 = startY;
                line.Line1.Visibility = (line.End.Visibility == Visibility.Visible && line.Start.Model.IsExpanded) ? Visibility.Visible : Visibility.Hidden;

                line.Line2.X1 = endX - offset;
                line.Line2.Y1 = startY;
                line.Line2.X2 = endX - offset;
                line.Line2.Y2 = endY;
                line.Line2.Visibility = (line.End.Visibility == Visibility.Visible && line.Start.Model.IsExpanded) ? Visibility.Visible : Visibility.Hidden;

                line.Line3.X1 = endX - offset;
                line.Line3.Y1 = endY;
                line.Line3.X2 = endX;
                line.Line3.Y2 = endY;
                line.Line3.Visibility = (line.End.Visibility == Visibility.Visible &&line.Start.Model.IsExpanded) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        #endregion

        #region utilities

        protected double GetGridStartTop(TreeGridItem item)
        {
            Canvas parent = _canvases[item.Model.Snapshot.Round];
            Point relativeLocation = item.TranslatePoint(new Point(0, 0), parent);
            return relativeLocation.Y;
        }


        protected TreeGridStart GetGrid(TreeGridItem item, TreeGridStart defaultGrid)
        {
            if (item != null && _grids.ContainsKey(item))
                return _grids[item];
            else
                return defaultGrid;
        }

        protected TreeGridItem GetDisplay(BetTreeNodeModel node)
        {
            return _items.Find(x => x.DataContext == node);
        }

        #endregion

        protected struct TreeGridLine
        {
            public TreeGridItem Start { get; set; }
            public TreeGridItem End { get; set; }
            public Line Line1 { get; set; }
            public Line Line2 { get; set; }
            public Line Line3 { get; set; }
        }
    }
}
