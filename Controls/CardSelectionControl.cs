using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Rzr.Core.Editors;
using System.Windows;

namespace Rzr.Core.Controls
{
    public class CardSelectionControl: Grid
    {
        #region properties

        protected CardSelectionModel _model;
        protected CardEditor[] _editors = new CardEditor[52];

        public int GridWidth { get; set; }
        public int GridHeight { get; set; }
        public int HorizontalSpacing { get; set; }
        public int VerticalSpacing { get; set; }

        #endregion

        #region initialise

        /// <summary>
        /// Constructor
        /// </summary>
        public CardSelectionControl()
        {
            this.DataContextChanged += SetModel;
            this.Loaded += SetModel;
        }

        /// <summary>
        /// Initialise the model
        /// </summary>
        protected void SetModel(object sender, EventArgs e)
        {
            _model = DataContext as CardSelectionModel;
            if (_model == null) return;

            InitialiseVisual();
            InitialiseData();
        }

        /// <summary>
        /// Initialise the model
        /// </summary>
        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = DataContext as CardSelectionModel;
            if (_model == null) return;

            InitialiseVisual();
            InitialiseData();            
        }

        /// <summary>
        /// Initialise controls
        /// </summary>
        public void InitialiseVisual()
        {
            if (_model == null) return;

            CreateGrid();

            // Add and bind the controls
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    AddCardEditor(j, i);
                }
            }
        }

        /// <summary>
        /// Creates the grid for the card controls
        /// </summary>
        protected void CreateGrid()
        {
            this.RowDefinitions.Clear();
            this.ColumnDefinitions.Clear();

            for (int i = 0; i < 4; i++)
                this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(GridHeight + VerticalSpacing) });
            for (int i = 0; i < 13; i++)
                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(GridWidth + HorizontalSpacing) });
        }

        /// <summary>
        /// Adds the rekevant card editor control at the given location in the grid
        /// </summary>
        protected void AddCardEditor(int xCord, int yCord)
        {
            //-------------------------------------------------------------------------------------
            // Initialise the editor...
            //-------------------------------------------------------------------------------------
            int card = (yCord * 13) + xCord;
            CardEditor editor = new CardEditor() { BackStyle = "Empty" };
            editor.DataContext = _model.Cards[card];
            _editors[card] = editor;
            editor.CardClicked += OnCardSelected;

            //-------------------------------------------------------------------------------------
            // ...and add it to the control at the correct co-ordinatess
            //-------------------------------------------------------------------------------------
            this.Children.Add(editor);
            editor.Height = GridHeight;
            editor.Width = GridWidth;
            editor.HorizontalAlignment = HorizontalAlignment.Center;
            editor.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(editor, yCord);
            Grid.SetColumn(editor, xCord);            
        }

        private void InitialiseData()
        {
            if (_model != null) return;
        }

        #endregion

        #region events

        /// <summary>
        /// Notifies the model that the given card has been selected by the user
        /// </summary>
        /// <param name="model"></param>
        protected void OnCardSelected(CardModel model)
        {
            _model.SelectCard(model);
            if (CardSelected != null) CardSelected(model);
        }

        public event CardEventHandler CardSelected;

        #endregion

        #region display

        #endregion
    }
}
