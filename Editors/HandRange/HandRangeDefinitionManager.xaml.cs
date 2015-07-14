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
using Rzr.Core.Data;

namespace Rzr.Core.Editors.HandRange
{
    /// <summary>
    /// Interaction logic for HandRangeDefinitionManager.xaml
    /// </summary>
    public partial class HandRangeDefinitionManager : UserControl
    {
        public HandRangeDefinitionManagerModel Model { get; set; }

        public HandRangeDefinitionManager()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            Editor.OnSave += Save;
            Editor.OnDelete += Delete;
            Editor.OnExit += Exit;
            SetModel(null, new DependencyPropertyChangedEventArgs());
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            Model = this.DataContext as HandRangeDefinitionManagerModel;
            if (Model == null) return;
        }

        protected void Save()
        {
            Model.Save();            
        }

        protected void Delete()
        {
            Model.Delete();            
        }

        protected void Exit()
        {
            WindowManager.ClosePopup(WindowManager.HAND_RANGE_DEFINITION_EDITOR);
        }

        protected void CreateCopy(object sender, RoutedEventArgs e)
        {
            HoleCardRangeDefinition def = Model.SelectedRange.Copy();
            Model.AddRange(def);
        }
    }
}
