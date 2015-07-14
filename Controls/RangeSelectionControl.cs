using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using Rzr.Core.Calculator;
using Rzr.Core.Data;

namespace Rzr.Core.Controls
{
    public class RangeSelectionControl : Canvas
    {
        public int BoxHeight { get; set; }
        public int BoxWidth { get; set; }

        public int KeyHeight { get; set; }
        public int KeyOffset { get; set; }
        public int GridHeight { get; set; }
        public int GridWidth { get; set; }
        public int CurrentLevel { get; set; }
        public List<RangeDisplayItem> Items { get; set; }
        public Direction KeySide { get; set; }

        protected Button[][] _boxes;        
        protected Dictionary<Button, int> _levels;
        protected Color[] _colors;
        protected Color[] _text;
       
        /// <summary>
        /// Constructor
        /// </summary>
        public RangeSelectionControl() 
        {
            BoxHeight = 30;
            BoxWidth = 30;
            CurrentLevel = 10;
            KeySide = Direction.Right;
        }

        /// <summary>
        /// Initialise the data and the controls which make up the display
        /// </summary>
        /// <param name="height">The number of boxes hight that the range display will be</param>
        /// <param name="width">The number of boxes wide that the range display will be</param>
        public void Initialise(List<RangeDisplayItem> items, int height, int width)
        {
            this.Children.Clear();

            Items = items;
            GridHeight = height;
            GridWidth = width;

            _colors = (Color[])ControlUtilities.StandardGradingColors.Clone();
            _text = (Color[])ControlUtilities.StandardGradingText.Clone();
            _levels = new Dictionary<Button, int>();
            _boxes = new Button[GridHeight][];

            SetRangeBoxes();
            if (KeySide == Direction.Right)
                SetRangeKeyRight();
            else
                SetRangeKeyDown();
            ReColourBoxes();
        }


        /// <summary>
        /// 
        /// </summary>
        public void SetRangeBoxes()
        {            
            for (int i = 0; i < GridHeight; i++)
            {
                //---------------------------------------------------------------------------------
                // Iterate through all the available positions on the grid
                //---------------------------------------------------------------------------------
                _boxes[i] = new Button[GridWidth];

                for (int j = 0; j < GridWidth; j++)
                {
                    //-----------------------------------------------------------------------------
                    // Create a button at the given position on the grid
                    //-----------------------------------------------------------------------------
                    Button box = GetGridButton(j, i);
                    _boxes[i][j] = box;
                    this.Children.Add(box);
                    RangeDisplayItem item = Items.Find(x => x.XCord == j && x.YCord == i);
                    if (item != null)
                    {
                        box.Content = item.Description;
                        box.Click += RangeButtonClicked;
                    }                    
                }
            }
        }

        private void SetRangeKeyRight()
        {
            int left = ((BoxWidth - 1) * (GridWidth + 1)) + KeyOffset;
            int offset = 10;
            int height = KeyHeight > 0 ? KeyHeight : (((BoxHeight - 1) * GridHeight) - (offset * 2)) / 11;
            int width = (int)((float)BoxWidth * ((float)height / (float)BoxHeight));

            if (height < 0) return;

            for (int i = 0; i <= 10; i++)
            {
                Button button = new Button()
                {
                    Height = height,
                    Width = width,
                    Margin = new Thickness(left, i * height + offset, 0, 0),
                    FontSize = 10,
                    Content = (i * 10) + "%",
                    Foreground = new SolidColorBrush(_text[i]),
                    Background = new SolidColorBrush(_colors[i])
                };

                _levels[button] = i;
                button.Click += LevelButtonClicked;
                this.Children.Add(button);
            }
        }

        private void SetRangeKeyDown()
        {
            int offset = 10;
            int top = ((BoxHeight - 1) * (GridHeight + 1));
            int width = ((BoxWidth * GridWidth) - (offset * 2)) / 11;
            int height = BoxHeight * (width / BoxWidth);

            for (int i = 0; i <= 10; i++)
            {
                Button button = new Button()
                {
                    Height = height,
                    Width = width,
                    Margin = new Thickness(i * width + offset, top, 0, 0),
                    FontSize = 10,
                    Content = (i * 10) + "%",
                    Foreground = new SolidColorBrush(_text[i]),
                    Background = new SolidColorBrush(_colors[i])
                };

                _levels[button] = i;
                button.Click += LevelButtonClicked;
                this.Children.Add(button);
            }
        }

        public void ReColourBoxes()
        {
            for (int i = 0; i < GridHeight; i++)
            {
                for (int j = 0; j < GridWidth; j++)
                {
                    RangeDisplayItem item = Items.Find(x => x.YCord == i && x.XCord == j);
                    if (item != null)
                    {
                        Color color = _colors[(int)(item.Weight / 10)];
                        Color text = _text[(int)(item.Weight / 10)];
                        _boxes[i][j].Background = new SolidColorBrush(color);
                        _boxes[i][j].Foreground = new SolidColorBrush(text);
                    }
                }
            }
        }

        protected void LevelButtonClicked(object sender, RoutedEventArgs e)
        {
            CurrentLevel = _levels[(Button)sender];
            foreach (Button button in _levels.Keys)
            {
                if (button == sender)
                {
                    button.Background = new SolidColorBrush(_colors[CurrentLevel]);
                    button.Foreground = new SolidColorBrush(_text[CurrentLevel]);
                }
                else
                {
                    button.Foreground = new SolidColorBrush(_text[_levels[button]]);
                    button.Background = new SolidColorBrush(_colors[_levels[button]]);
                }
            }
        }

        protected void RangeButtonClicked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < GridHeight; i++)
            {
                for (int j = 0; j < GridWidth; j++)
                {
                    if (_boxes[i][j] == sender)
                    {
                        RangeDisplayItem item = Items.Find(x => x.XCord == j && x.YCord == i);
                        item.Weight = CurrentLevel * 10;
                        _boxes[i][j].Background = new SolidColorBrush(_colors[CurrentLevel]);
                        _boxes[i][j].Foreground = new SolidColorBrush(_text[CurrentLevel]);
                    }
                }
            }

            if (RangeChanged != null) RangeChanged();
        }

        private Button GetGridButton(int xCord, int yCord)
        {
            return new Button()
            {
                Height = BoxHeight,
                Width = BoxWidth,
                Margin = new Thickness(((GridWidth - 1) - xCord) * (BoxWidth - 1), ((GridHeight - 1) - yCord) * BoxHeight, 0, 0),
                FontSize = 10,
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Background = new SolidColorBrush(Color.FromRgb(0, 0, 0))                
            };
        }

        public event EmptyEventHandler RangeChanged;
    }
}
