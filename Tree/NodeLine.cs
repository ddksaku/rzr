using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;

namespace Rzr.Core.Tree
{
    public class NodeLine : Panel
    {
        protected Line _line1;
        protected Line _line2;
        protected Line _line3;

        public Vector Start { get; set; }

        public Vector End { get; set; }

        public NodeLine()
        {
            _line1 = new Line();
            this.Children.Add(_line1);
            _line2 = new Line();
            this.Children.Add(_line2);
            _line3 = new Line();
            this.Children.Add(_line3);
        }

        public void Redraw()
        {
            throw new NotImplementedException();
        }
    }
}
