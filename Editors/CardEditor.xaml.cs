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

namespace Rzr.Core.Editors
{
    /// <summary>
    /// Interaction logic for CardEditor.xaml
    /// </summary>
    public partial class CardEditor : UserControl
    {
        protected CardModel _model;

        public string BackStyle 
        {
            get { return CardImage.BackStyle; }
            set { CardImage.BackStyle = value; }
        }

        public CardEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
            Initialise();
            CardImage.UpdateImage();
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as CardModel;
            Initialise();
        }

        protected void Initialise()
        {
            if (_model == null) return;
            _model.CardSelectionChanged += UpdateEditor;
            UpdateEditor();
        }

        protected void OnCardClicked(object sender, RoutedEventArgs e)
        {
            if (CardClicked != null) CardClicked(_model);
        }

        public void UpdateEditor()
        {
            CardImage.Value = _model.ModelCard == null || !_model.Available ? null : (int?)_model.ModelCard.RawInt;
        }

        public event CardEventHandler CardClicked;        
    }
}
