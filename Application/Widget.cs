using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace Rzr.Core.Application
{
    [XmlType("Widget")]
    public class Widget
    {
        #region widget

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlElement("Gui")]
        public string GuiClass { get; set; }

        [XmlElement("Model")]
        public string ModelClass { get; set; }

        [XmlElement("Icon")]
        public string IconName { get; set; }

        #endregion

        public void Unselect()
        {
        }

        public void Select()
        {
        }
    }
}
