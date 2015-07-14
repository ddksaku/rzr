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
using Rzr.Core.Xml;
using Rzr.Core.Partials;

namespace Rzr.Core.Tree
{
    /// <summary>
    /// Interaction logic for WizardSettings.xaml
    /// </summary>
    public partial class WizardSettings : UserControl
    {
        protected PartialGenerator _model;

        public WizardSettings()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = this.DataContext as PartialGenerator;
            if (_model == null) return;
        }

        protected void Load(object sender, RoutedEventArgs e)
        {
            if (this.OnFinish != null) OnFinish(_model);
        }

        protected void Exit(object sender, RoutedEventArgs e)
        {
            if (this.OnClose != null) OnClose();
        }

        public event EmptyEventHandler OnClose;

        public event PartialGeneratorHandler OnFinish;
    }
}
