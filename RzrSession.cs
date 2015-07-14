using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Application;

namespace Rzr.Core
{
    public class RzrSession
    {
        #region properties

        /// <summary>
        /// The widgets that are available to Rzr Suite under the current configuration
        /// </summary>
        public static Widget[] AvailableWidgets { get; private set; }

        /// <summary>
        /// The currently active widget
        /// </summary>
        public static Widget ActiveWidget { get; private set; }

        #endregion

        #region events

        public static event WidgetChangedEventHandler WidgetChanged;

        #endregion

        #region session management

        public static void Init()
        {
            AvailableWidgets = RzrInit.InitFile.Widgets.ToArray();
        }

        public static void Reset()
        {
            //-------------------------------------------------------------------------------------
            // If the active widget specified in the user settings is null, then set it to the 
            // first available widget
            //-------------------------------------------------------------------------------------
            if (RzrUserSettings.UserSettings.ActiveWidget == null)
            {
                RzrUserSettings.UserSettings.ActiveWidget = AvailableWidgets.First().Name;
            }

            //-------------------------------------------------------------------------------------
            // Reset the active widget to the one specified in the user settings
            //-------------------------------------------------------------------------------------
            SetWidget(AvailableWidgets.First(x => x.Name == RzrUserSettings.UserSettings.ActiveWidget));
        }

        /// <summary>
        /// Set the active widget according to its name
        /// </summary>
        /// <param name="widget"></param>
        public static void SetWidget(string name)
        {
            Widget widget = AvailableWidgets.FirstOrDefault(x => x.Name == name);
            if (widget == null)
            {
                ErrorService.Record("Could not find widget " + name);
                return;
            }
            SetWidget(widget);
        }


        /// <summary>
        /// Set the active widget
        /// </summary>
        /// <param name="widget"></param>
        public static void SetWidget(Widget widget)
        {
            try
            {
                if (ActiveWidget != null)
                    ActiveWidget.Unselect();
                widget.Select();
                if (WidgetChanged != null) WidgetChanged(ActiveWidget, widget);
                ActiveWidget = widget;
            }
            catch (Exception exc)
            {               
                ErrorService.Record("Could not activate new widget " + widget.Name, exc);
                throw new InvalidOperationException("Could not activate new widget " + widget.Name, exc);
            }
        }

        #endregion
    }
}
