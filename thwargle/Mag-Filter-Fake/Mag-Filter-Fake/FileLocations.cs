using System;
using System.IO;
using System.Text;

namespace MagFilter
{
    public class FileLocations
    {
        internal static string PluginName = "Mag-Filter";

        public static string GetPluginSettingsFile()
        {
            return PluginPersonalFolder.FullName + @"\" + PluginName + ".xml";
        }
        /// <summary>
        /// Info from exe 
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLaunchFile()
        {
            return PluginPersonalFolder.FullName + @"\" + PluginName + "_launch.txt";
        }

        public static string GetCharacterFilePath()
        {
            // The folder for the roaming current user 
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string specificFolder = Path.Combine(folder, "ACAccountManager");

            string path = Path.Combine(specificFolder, "characters.txt");
            log.writeLogs("Path: " + path); // TODO - delete

            return path;
        }

        internal static DirectoryInfo PluginPersonalFolder
        {
            get
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins";
                DirectoryInfo pluginPersonalFolder = new DirectoryInfo(folder + @"\" + PluginName);

                try
                {
                    if (!pluginPersonalFolder.Exists)
                        pluginPersonalFolder.Create();
                }
                catch (Exception ex)
                {
                    // TODO: Some fall back log
                }

                return pluginPersonalFolder;
            }
        }
    }
}
