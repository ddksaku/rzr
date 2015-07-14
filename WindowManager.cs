using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core
{
    public class WindowManager
    {
        //-----------------------------------------------------------------------------------------
        // Core
        //-----------------------------------------------------------------------------------------
        public const string HAND_RANGE_DEFINITION_EDITOR = "HandRangeDefinitionEditor";

        //-----------------------------------------------------------------------------------------
        // Rzr Range
        //-----------------------------------------------------------------------------------------
        public const string RANGE_LIST_OPTIONS = "RangeListOptions";
        public const string RANGE_TABLE_OPTIONS_X = "RangeTableOptionsX";
        public const string RANGE_TABLE_OPTIONS_Y = "RangeTableOptionsY";

        //-----------------------------------------------------------------------------------------
        // Rzr Scenario
        //-----------------------------------------------------------------------------------------
        public const string HELP_VIEW = "HelpView";
        public const string CONDITIONS_EDITOR = "ConditionsEditor";
        public const string SAVE_PARTIAL = "SavePartial";
        public const string SCENARIO_SETTINGS = "ScenarioSettings";

        public static void OpenPopup(string popupName)
        {
            if (OpenPopupRequested != null) OpenPopupRequested(popupName);
        }

        public static void ClosePopup(string popupName)
        {
            if (ClosePopupRequested != null) ClosePopupRequested(popupName);
        }

        public static event OpenPopupEventHandler OpenPopupRequested;

        public static event OpenPopupEventHandler ClosePopupRequested;
    }

    public delegate void OpenPopupEventHandler(string popupName);
}
