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
    /// Interaction logic for ComponentEditor.xaml
    /// </summary>
    public partial class ComponentEditor : UserControl
    {
        public ComponentEditorModel Model { get; private set; }        

        public ComponentEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = DataContext as ComponentEditorModel;
            if (Model == null) return;            
        }

        protected void Save(object sender, RoutedEventArgs e)
        {
            if (OnSave != null) OnSave(this, EventArgs.Empty);
        }

        protected void Cancel(object sender, RoutedEventArgs e)
        {
            if (OnCancel != null) OnCancel(this, EventArgs.Empty);
        }

        public event EventHandler OnSave;

        public event EventHandler OnCancel;
    }
}
