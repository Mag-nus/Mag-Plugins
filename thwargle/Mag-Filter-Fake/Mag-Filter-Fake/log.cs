using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MagFilter
{
    class log
    {
        public static void writeLogs(string logText)
        {
            // Write the string to a file.
            string path = FileLocations.PluginPersonalFolder.FullName + @"\MagFilter_Log.txt";

            using (StreamWriter file = new StreamWriter(path, append: true))
            {
                file.WriteLine(string.Format("{0:yyyy-MM-dd HH:mm:ss} {1}", DateTime.Now, logText));
            }
        }
    }
}
