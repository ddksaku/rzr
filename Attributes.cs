using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core
{
    public class Editor : Attribute
    {
        public Type EditorType { get; private set; }

        public Editor(Type type)
        {
            EditorType = type;
        }
    }
}
