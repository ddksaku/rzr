using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Editors.Conditions
{
    public interface SubConditionSectionModel
    {
        void SetMask(ulong mask);

        ulong GetMask();

        void Reset();
    }
}
