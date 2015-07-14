using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Rzr.Core.Xml;

namespace Rzr.Core
{
    public class RzrConfiguration
    {
        #region constants

        public const string HOOK_RANGE = "RangeHookFile";
        public const string HOOK_HANDVALUE_RANGE = "HandValueRangeHookFile";
        public const string HOOK_HANDVALUE_DEF = "HandValueDefinitionHookFile";
        public const string HOOK_RANGE_CUSTOM = "CustomRangeHookFile";

        public const string InitFile = "rzr.init";
        public const string SettingsFile = "rzr.config";
        public const string PartialsMaster = @"partials.master.xml";

        public const string ConditionsMap = "map.xml";
        public const string DefaultConditions = "conditions.xml";
        public const string RangeListConditions = "list.xml";
        public const string RangeTableXConditions = "tablex.xml";
        public const string RangeTableYConditions = "tabley.xml";

        public static Bitmap[] Cards = new Bitmap[]
        {            
            Properties.Resources.C_2,
            Properties.Resources.C_3,
            Properties.Resources.C_4,
            Properties.Resources.C_5,
            Properties.Resources.C_6,
            Properties.Resources.C_7,
            Properties.Resources.C_8,
            Properties.Resources.C_9,
            Properties.Resources.C_10,
            Properties.Resources.C_j,
            Properties.Resources.C_q,
            Properties.Resources.C_k,            
            Properties.Resources.C_1,

            Properties.Resources.B_2,
            Properties.Resources.B_3,
            Properties.Resources.B_4,
            Properties.Resources.B_5,
            Properties.Resources.B_6,
            Properties.Resources.B_7,
            Properties.Resources.B_8,
            Properties.Resources.B_9,
            Properties.Resources.B_10,
            Properties.Resources.B_j,
            Properties.Resources.B_q,
            Properties.Resources.B_k,            
            Properties.Resources.B_1,

            Properties.Resources.A_2,
            Properties.Resources.A_3,
            Properties.Resources.A_4,
            Properties.Resources.A_5,
            Properties.Resources.A_6,
            Properties.Resources.A_7,
            Properties.Resources.A_8,
            Properties.Resources.A_9,
            Properties.Resources.A_10,
            Properties.Resources.A_j,
            Properties.Resources.A_q,
            Properties.Resources.A_k,            
            Properties.Resources.A_1,

            Properties.Resources.D_2,
            Properties.Resources.D_3,
            Properties.Resources.D_4,
            Properties.Resources.D_5,
            Properties.Resources.D_6,
            Properties.Resources.D_7,
            Properties.Resources.D_8,
            Properties.Resources.D_9,
            Properties.Resources.D_10,
            Properties.Resources.D_j,
            Properties.Resources.D_q,
            Properties.Resources.D_k,            
            Properties.Resources.D_1
        };


        #endregion

        public static string DataDirectory { get; private set; }        
        public static string SaveDirectory { get; private set; }
        public static string PartialsDirectory { get; private set; }
        public static string RzrSiteUrl { get; private set; }
        public static string RzrSiteLicenseVerificationUrl { get; private set; }

        public static void Initialise()
        {
            DataDirectory = GetParm("DataDirectory");
            SaveDirectory = GetParm("SaveDirectory");
            PartialsDirectory = GetParm("PartialsDirectory");
            RzrSiteUrl = GetParm("RzrSiteUrl");
            RzrSiteLicenseVerificationUrl = GetParm("RzrSiteLicenseVerificationUrl");
        }

        public static string GetParm(string key)
        {
            Parameter parm = RzrInit.InitFile.Parms.Find(x => x.Key == key);
            return parm == null ? null : parm.Value;
        }        
    }
}
