using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Rzr.Core.Editors.LevelGrid
{
    public class LevelGridItem : DependencyObject
    {
        #region property definitions

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(LevelGridItem), new PropertyMetadata(null, null));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description", typeof(string), typeof(LevelGridItem), new PropertyMetadata(null, null));

        public static readonly DependencyProperty EditProperty = DependencyProperty.Register(
            "Edit", typeof(bool), typeof(LevelGridItem), new PropertyMetadata(false, null));

        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register(
            "Level", typeof(int), typeof(LevelGridItem), new PropertyMetadata(0, null));

        public static readonly DependencyProperty RankProperty = DependencyProperty.Register(
            "Rank", typeof(int), typeof(LevelGridItem), new PropertyMetadata(0, null));

        #endregion

        #region dependendency properties

        public string Name
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        public string Description
        {
            get { return (string)this.GetValue(DescriptionProperty); }
            set { this.SetValue(DescriptionProperty, value); }
        }

        public bool Edit
        {
            get { return (bool)this.GetValue(EditProperty); }
            set { this.SetValue(EditProperty, value); }
        }

        public int Level
        {
            get { return (int)this.GetValue(LevelProperty); }
            set { this.SetValue(LevelProperty, value); }
        }

        public int Rank
        {
            get { return (int)this.GetValue(RankProperty); }
            set { this.SetValue(RankProperty, value); }
        }

        #endregion

        #region other properties

        public int ID { get; set; }

        public int XCord { get; set; }

        public int YCord { get; set; }

        public int GridWidth { get; set; }

        public int GridHeight { get; set; }

        #endregion

    }
}
