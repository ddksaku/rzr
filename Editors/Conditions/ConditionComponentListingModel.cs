using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Calculator;
using System.Windows;

namespace Rzr.Core.Editors.Conditions
{
    public class ConditionComponentListingModel : DependencyObject
    {
        #region dependency property definitions

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(ConditionComponentListingModel), new PropertyMetadata(null, null));

        #endregion

        #region property definition

        public string Name
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        #endregion

        public ConditionAtom Atom { get; private set; }

        public ConditionComponentListingModel(ConditionAtom atom)
        {
            Atom = atom;
            Name = atom.Name;
        }

        public void Refresh()
        {
            Name = Atom.Name;
        }

        public void Delete()
        {
            if (OnDelete != null) OnDelete(this, EventArgs.Empty);
        }

        public void Edit()
        {
            if (OnEdit != null) OnEdit(this, EventArgs.Empty);
        }

        public event EventHandler OnEdit;

        public event EventHandler OnDelete;
    }
}
