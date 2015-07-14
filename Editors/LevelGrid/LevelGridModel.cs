using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Editors.LevelGrid
{
    public class LevelGridModel
    {
        public List<LevelGridLevel> Levels { get; set; }

        public List<LevelGridItem> Items { get; set; }

        public List<LevelGridElement> Columns { get; set; }

        public List<LevelGridElement> Rows { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }
    }
}
