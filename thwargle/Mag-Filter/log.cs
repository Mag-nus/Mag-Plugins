using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MagFilter
{
    class log
    {
        private static object _locker = new object();
        private static string _logFilepath = "";

        public static string GetLogFilepath()
        {
            return _logFilepath;
        }
        public static void WriteLogMsg(string logText)
        {
            lock (_locker)
            {
                // Use log path specified in dll.config, or default to Decal Plugin folder with hardcoded name
                if (string.IsNullOrEmpty(_logFilepath))
                {
                    AssemblySettings settings = new AssemblySettings();
                    string filepath = settings["LogFilepath"];
                    if (string.IsNullOrEmpty(filepath))
                    {
                        filepath = FileLocations.AppLogsFolder +  @"\Mag-Filter_%PID%_log.txt";
                    }
                    _logFilepath = FileLocations.ExpandFilepath(filepath);
                    // Create any needed folders
                    FileLocations.CreateAnyNeededFolders(_logFilepath);
                }

                using (StreamWriter file = new StreamWriter(_logFilepath, append: true))
                {
                    file.WriteLine(string.Format("{0:yyyy-MM-dd HH:mm:ss} {1}", DateTime.Now, logText));
                }
            }
        }
    }
}
