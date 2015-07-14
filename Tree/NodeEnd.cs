using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace Rzr.Core.Tree
{
    public class NodeEnd : Control
    {
        #region property declarations

        public static readonly DependencyProperty StartProperty = DependencyProperty.Register(
            "Start", typeof(Point), typeof(NodeEnd), new PropertyMetadata(new Point(0, 0), OnPathChanged));

        public static readonly DependencyProperty EndProperty = DependencyProperty.Register(
            "End", typeof(Point), typeof(NodeEnd), new PropertyMetadata(new Point(0, 0), OnPathChanged));

        #endregion

        #region properties

        public Point Start
        {
            get { return (Point)this.GetValue(StartProperty); }
            set { this.SetValue(StartProperty, value); }
        }

        public Point End
        {
            get { return (Point)this.GetValue(EndProperty); }
            set { this.SetValue(EndProperty, value); }
        }

        public Canvas ParentCanvas { get; private set; }
        public Path NodePath { get; private set; }
        public BetTreeNodeModel Model { get; private set; }

        #endregion

        #region init

        public NodeEnd()
        {
            this.DataContextChanged += SetModel;
        }

        private void InitialiseLine()
        {
            if (NodePath == null)
            {
                NodePath = new Path();
                NodePath.Stroke = Brushes.Black;
                NodePath.StrokeThickness = 1;

                Grid grid = GetParentGrid();
                ParentCanvas = Utilities.FindChild<Canvas>(grid);
                ParentCanvas.Children.Add(NodePath);
            }
        }

        public void RemoveLine()
        {
            if (NodePath != null)
            {
                ParentCanvas.Children.Remove(NodePath);
            }
        }

        #endregion

        #region events

        protected static void OnPathChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            NodeEnd nodeEnd = sender as NodeEnd;
            if (nodeEnd != null) nodeEnd.SetPath();
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = DataContext as BetTreeNodeModel;
            if (Model == null) return;

            Model.Tree.TreeChanged += UpdateLines;            
        }

        #endregion

        #region draw

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            UpdateLines();
        }

        public void UpdateLines()
        {
            if (Model.Parent != null)
            {
                InitialiseLine();
                SetVectors();
            }
        }

        protected void SetVectors()
        {
            if (Model.Parent != null)
            {
                NodePath.Visibility = Model.Parent.IsExpanded ? Visibility.Visible : Visibility.Hidden;
            }

            Grid grid = GetParentGrid();
            if (grid == null)
            {
                NodePath.Visibility = Model.Parent.IsExpanded ? Visibility.Visible : Visibility.Hidden;
                return;
            }

            Canvas canvas = Utilities.FindChild<Canvas>(grid);
            Border border = Utilities.FindChild<Border>(grid);
            BetTreeNode parentNode = Utilities.FindChild<BetTreeNode>(border);
            Vector endVector = Utilities.GetOffset(this, grid);

            //Start = new Point(startVector.X, startVector.Y);
            //End = new Point(endVector.X, endVector.Y);
        }

        protected void SetPath()
        {
            PathGeometry geometry = new PathGeometry();
            double midX = Start.X + 20;

            // Create a figure.
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(Start.X, Start.Y);
            figure.Segments.Add(new LineSegment(new Point(midX, Start.Y), true));
            figure.Segments.Add(new LineSegment(new Point(midX, End.Y), true));
            figure.Segments.Add(new LineSegment(new Point(End.X, End.Y), true));
            geometry.Figures.Add(figure);
            NodePath.Data = geometry;
        }

        #endregion

        #region utilities

        protected Grid GetParentGrid()
        {
            Grid grid = Utilities.FindParent<Grid>(this);
            grid = Utilities.FindParent<Grid>(grid);
            grid = Utilities.FindParent<Grid>(grid);
            return grid;
        }

        #endregion
    }
}
