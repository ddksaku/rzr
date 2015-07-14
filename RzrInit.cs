using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Application;
using System.Xml.Serialization;
using System.IO;
using Rzr.Core.Data;
using Rzr.Core.Xml;
using Rzr.Core.Calculator;

namespace Rzr.Core
{
    [XmlRoot("Root")]
    public class RzrInit
    {
        #region initialize

        public static RzrInit InitFile { get; private set; }

        public static void Initialise()
        {
            string executingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string initFile = Path.Combine(executingDirectory, RzrConfiguration.InitFile);
            XmlSerializer serializer = new XmlSerializer(typeof(RzrInit));
            using (StreamReader reader = new StreamReader(initFile))
            {
                try
                {
                    InitFile = serializer.Deserialize(reader) as RzrInit;
                }
                catch (Exception exc)
                {
                    ErrorService.Record("Problem opening initialization file", exc);
                    throw new IOException("Problem opening initialization file", exc);
                }
            }

            RzrConfiguration.Initialise();
            ConditionService.Initialise();
            RzrDataService.Initialise();
            
        }

        public static void InitialiseWeb(string webroot)
        {
            string initFile = Path.Combine(webroot, RzrConfiguration.InitFile);
            XmlSerializer serializer = new XmlSerializer(typeof(RzrInit));
            using (StreamReader reader = new StreamReader(initFile))
            {
                try
                {
                    InitFile = serializer.Deserialize(reader) as RzrInit;
                }
                catch (Exception exc)
                {
                    ErrorService.Record("Problem opening initialization file", exc);
                    throw new IOException("Problem opening initialization file", exc);
                }
            }

            RzrConfiguration.Initialise();
            ConditionService.Initialise();
            RzrDataService.Initialise();

        }

        #endregion

        [XmlArray("Widgets")]
        public List<Widget> Widgets { get; set; }

        [XmlArray("Settings")]
        public List<Parameter> Parms { get; set; }

        [XmlArray("Hooks")]
        public List<Hook> Hooks { get; set; }
    }
}
