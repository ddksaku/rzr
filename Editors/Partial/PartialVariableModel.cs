using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using Rzr.Core.Editors.Player;

namespace Rzr.Core.Editors.Partial
{
    public class PartialVariableModel : DependencyObject
    {
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name",
            typeof(string), typeof(PartialVariableModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty VariableProperty = DependencyProperty.Register("Variable",
            typeof(string), typeof(PartialVariableModel), new PropertyMetadata(null, null));

        public static readonly DependencyProperty FixedProperty = DependencyProperty.Register("Fixed",
            typeof(bool), typeof(PartialVariableModel), new PropertyMetadata(true, null));

        public static readonly DependencyProperty DefaultProperty = DependencyProperty.Register("Default",
            typeof(float), typeof(PartialVariableModel), new PropertyMetadata(0f, null));

        public static readonly DependencyProperty ShowPlayersProperty = DependencyProperty.Register("ShowPlayers",
            typeof(Boolean), typeof(PartialVariableModel), new PropertyMetadata(false, OnShowPlayersChanged));

        public static readonly DependencyProperty PlayersVisibilityProperty = DependencyProperty.Register("PlayersVisibility",
            typeof(Visibility), typeof(PartialVariableModel), new PropertyMetadata(Visibility.Collapsed, null));

        public static readonly DependencyProperty PlayersProperty = DependencyProperty.Register("Players",
            typeof(ObservableCollection<PartialPlayerValueModel>), typeof(PartialVariableModel), new PropertyMetadata(null, null));

        public string Name
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        public string Variable
        {
            get { return (string)this.GetValue(VariableProperty); }
            set { this.SetValue(VariableProperty, value); }
        }

        public bool Fixed
        {
            get { return (bool)this.GetValue(FixedProperty); }
            set { this.SetValue(FixedProperty, value); }
        }

        public float Default
        {
            get { return (float)this.GetValue(DefaultProperty); }
            set { this.SetValue(DefaultProperty, value); }
        }

        public ObservableCollection<PartialPlayerValueModel> Players
        {
            get { return (ObservableCollection<PartialPlayerValueModel>)this.GetValue(PlayersProperty); }
            set { this.SetValue(PlayersProperty, value); }
        }

        public Boolean ShowPlayers
        {
            get { return (Boolean)this.GetValue(ShowPlayersProperty); }
            set { this.SetValue(ShowPlayersProperty, value); }
        }

        public Visibility PlayersVisibility
        {
            get { return (Visibility)this.GetValue(PlayersVisibilityProperty); }
            set { this.SetValue(PlayersVisibilityProperty, value); }
        }

        protected static void OnShowPlayersChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PartialVariableModel model = sender as PartialVariableModel;
            model.PlayersVisibility = model.ShowPlayers ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
