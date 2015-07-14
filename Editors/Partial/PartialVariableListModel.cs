using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;

namespace Rzr.Core.Editors.Partial
{
    public class PartialVariableListModel : DependencyObject
    {
        public static readonly DependencyProperty DefinitionsProperty = DependencyProperty.Register("Definitions",
            typeof(ObservableCollection<PartialVariableModel>), typeof(PartialVariableListModel), new PropertyMetadata(null, null));

        public ObservableCollection<PartialVariableModel> Definitions
        {
            get { return (ObservableCollection<PartialVariableModel>)this.GetValue(DefinitionsProperty); }
            set { this.SetValue(DefinitionsProperty, value); }
        }
    }
}
