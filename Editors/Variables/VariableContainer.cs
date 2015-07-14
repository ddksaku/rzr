using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Xml;
using System.Windows;
using System.Collections.ObjectModel;

namespace Rzr.Core.Editors.Variables
{
    public class VariableContainer : DependencyObject
    {
        #region dependency properties

        public static readonly DependencyProperty GroupsProperty = DependencyProperty.Register("Groups", 
            typeof(ObservableCollection<VariableGroup>), typeof(VariableContainer), new PropertyMetadata(null, null));

        public ObservableCollection<VariableGroup> Groups
        {
            get { return (ObservableCollection<VariableGroup>)this.GetValue(GroupsProperty); }
            set { this.SetValue(GroupsProperty, value); }
        }

        #endregion

        #region constructor

        public VariableContainer()
        {
            ObservableCollection<VariableGroup> groups = new ObservableCollection<VariableGroup>();
            AddDefaultGroups(groups);
            Groups = groups;
        }

        private void AddDefaultGroups(ObservableCollection<VariableGroup> groups)
        {
            foreach (HoldemHandRound round in Enum.GetValues(typeof(HoldemHandRound)))
            {
                string shortName = Convert.ToString(round.ToString()[0]);
                if (groups.Count(x => x.ShortName == shortName) == 0)
                {
                    groups.Add(new VariableGroup() { ShortName = shortName, Name = round.ToString() });
                }
            }
        }

        #endregion

        #region xml

        /// <summary>
        /// Load from xml
        /// </summary>
        public void LoadFromXml(VariableContainerXml xml)
        {
            ObservableCollection<VariableGroup> groups = new ObservableCollection<VariableGroup>();
            foreach (VariableGroupXml group in xml.Groups)
            {
                VariableGroup varGroup = new VariableGroup();
                varGroup.LoadFromXml(group);
                groups.Add(varGroup);
            }
            AddDefaultGroups(groups);
            Groups = groups;
        }

        /// <summary>
        /// Save to xml
        /// </summary>
        /// <returns></returns>
        public VariableContainerXml SaveToXml()
        {
            VariableContainerXml ret = new VariableContainerXml();
            ret.Groups = this.Groups.Select(x => x.SaveToXml()).ToArray();

            return ret;
        }

        #endregion
    }

    public enum VariableType
    {
        Percentage = 0,
        Float = 1
    }
}
