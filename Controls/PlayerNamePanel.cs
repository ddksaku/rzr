using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Rzr.Core.Controls
{
    public class PlayerNamePanel : Image
    {
        static PlayerNamePanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlayerNamePanel), new FrameworkPropertyMetadata(typeof(PlayerNamePanel)));
        }

        public PlayerNamePanel()
        {
            this.Source = Utilities.LoadBitmap(Properties.Resources.playerbackground);
        }
    }
}
