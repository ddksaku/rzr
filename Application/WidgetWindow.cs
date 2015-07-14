using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Rzr.Core.Application
{
    public interface WidgetWindow
    {
        Visibility Visibility { get; set; }

        object DataContext { get; set; }
    }
}
