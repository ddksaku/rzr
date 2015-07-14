using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rzr.Core.Editors.Conditions
{
    /// <summary>
    /// Interaction logic for SubConditionRadioItem.xaml
    /// </summary>
    public partial class SubConditionRadioItem : UserControl
    {
        public SubConditionRadioItemModel Model { get; private set; }

        public SubConditionRadioItem()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            SetModel(null, new DependencyPropertyChangedEventArgs());
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = this.DataContext as SubConditionRadioItemModel;
            if (Model == null) return;
        }
    }
}
