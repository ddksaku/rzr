using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Results
{
    public interface ResultsCellModel
    {
        double Height { get; }

        double Width { get; }

        bool ShowWinLoss { get; set; }

        event EmptyEventHandler ShowWinLossChanged;

        void OnSelected();

        void Refresh();
    }
}
