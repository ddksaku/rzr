using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Rzr.Core.Controls
{
    public class DataButton : Button
    {
        public string Key { get; set; }

        public object Value { get; set; }

        public int HorizontalIndex { get; set; }

        public int VerticalIndex { get; set; }
    }
}
