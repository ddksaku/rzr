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

namespace Rzr.Core.Editors.LevelGrid
{
    /// <summary>
    /// Interaction logic for MoveableButtonEditor.xaml
    /// </summary>
    public partial class MoveableButtonEditor : UserControl
    {
        protected List<LevelGridButton> _buttons = new List<LevelGridButton>();
        protected LevelGridModel _model;

        public MoveableButtonEditor()
        {
            InitializeComponent();
            this.DataContextChanged += SetModel;
        }

        protected void SetModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            _model = this.DataContext as LevelGridModel;
            Initiliase();
        }

        protected void Initiliase()
        {
            if (_model == null) return;

            this.Height = _model.Height + 30;
            this.Width = _model.Width + 30;

            ClearGrid();
            SetGridDimensions();
            SetGridButtons();
        }

        protected void ClearGrid()
        {
            ButtonGrid.Children.Clear();
            ButtonGrid.ColumnDefinitions.Clear();
            ButtonGrid.RowDefinitions.Clear();
        }

        protected void SetGridDimensions()
        {
            for (int i = 0; i < _model.Columns.Count; i++)
            {
                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(_model.Columns[i].Dimension)
                });
            }

            for (int j = 0; j < _model.Rows.Count; j++)
            {
                ButtonGrid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(_model.Rows[j].Dimension)
                });
            }
        }

        protected void SetGridButtons()
        {
            for (int i = 0; i < _model.Columns.Count; i++)
            {
                for (int j = 0; j < _model.Rows.Count; j++)
                {
                    LevelGridItem item = _model.Items.Find(x => x.XCord == i && x.YCord == j);
                    if (item == null) continue;

                    LevelGridButton button = AddItem(item);
                    SetGrid(button, item);
                }
            }
        }

        protected LevelGridButton AddItem(LevelGridItem item)
        {
            LevelGridButton button = new LevelGridButton() { DataContext = item };
            _buttons.Add(button);
            button.Delete += OnItemDelete;
            button.Edit += OnItemEdit;
            ButtonGrid.Children.Add(button);
            return button;
        }

        protected void SetGrid(LevelGridButton button, LevelGridItem item)
        {
            Grid.SetRow(button, item.YCord);
            Grid.SetColumn(button, item.XCord);
            Grid.SetRowSpan(button, item.GridHeight);
            Grid.SetColumnSpan(button, item.GridWidth);
        }

        protected void OnItemDelete(LevelGridItem button)
        {
            if (ItemDelete != null) ItemDelete(button as LevelGridItem);
        }

        protected void OnItemEdit(LevelGridItem button)
        {
            if (ItemEdit != null) ItemEdit(button as LevelGridItem);
        }


        protected void DeleteItem(LevelGridItem item)
        {
            LevelGridButton button = _buttons.Find(x => x.DataContext == item);
            ButtonGrid.Children.Remove(button);
            _buttons.Remove(button);
        }

        public event LevelGridItemHandler ItemDelete;

        public event LevelGridItemHandler ItemEdit;
    }
}
