using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Editors;

namespace Rzr.Core.Controls
{
    public interface CardSelectionModel
    {
        void SelectCard(CardModel model);
        CardModel[] Cards { get; set; }
    }
}
