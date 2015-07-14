using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Rzr.Core
{
    [XmlRoot("Root")]
    public class RzrUserSettings
    {
        #region initialize

        public static RzrUserSettings UserSettings { get; private set; }

        public static void Initialise()
        {
            string executingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string settingsFile = Path.Combine(executingDirectory, RzrConfiguration.SettingsFile);
            XmlSerializer serializer = new XmlSerializer(typeof(RzrUserSettings));
            using (StreamReader reader = new StreamReader(settingsFile))
            {
                try
                {
                    UserSettings = serializer.Deserialize(reader) as RzrUserSettings;
                }
                catch (Exception exc)
                {
                    ErrorService.Record("Problem opening user settings file", exc);
                    throw new IOException("Problem opening user settings file");
                }
            }
        }

        #endregion

        # region usersettings

        [XmlElement("ActiveWidget")]
        public string ActiveWidget { get; set; }

        [XmlElement("MaxSeats")]
        public int MaxSeats { get; set; }

        #endregion
    }
}
