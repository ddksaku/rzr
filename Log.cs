using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Rzr.Core
{
    public class Log
    {
        protected const string LogPath = @"c:\rzr\RzrSuite\Log\log.txt";

        protected static string _message = String.Empty;

        public static void Write(string message)
        {
            _message += message;
        }

        public static void Flush()
        {
            using (StreamWriter writer = new StreamWriter(LogPath))
            {
                writer.Write(_message);
            }
            _message = String.Empty;
        }
    }
}
