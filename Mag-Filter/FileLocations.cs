using System;
using System.IO;
using System.Text;
using Mag.Shared;

namespace MagFilter
{
    public class FileLocations
    {
        internal static string FilterName = "Mag-Filter";

        public static string GetFilterSettingsFilepath()
        {
            return PluginPersonalFolder.FullName + @"\" + FilterName + ".xml";
        }
        /// <summary>
        /// Launch file contains instructions (character) name from ThwargLauncher.exe for Mag-Filter.dll
        /// </summary>
        public static string GetCurrentLaunchFilePath()
        {
            string path = Path.Combine(AppFolder, FilterName + "_launch.txt");
            return path;
        }
        /// <summary>
        /// Launch response file contains pid of game, given by Mag-Filter.dll to ThwargLauncher.exe
        /// </summary>
        public static string GetCurrentLaunchResponseFilePath()
        {
            string path = Path.Combine(AppFolder, FilterName + "_launchResponse.txt");
            return path;
        }

        public static string GetCharacterFilePath()
        {
            string path = Path.Combine(AppFolder, "characters.txt");
            return path;
        }
        public static string GetOldCharacterFilePath()
        {
            string path = Path.Combine(OldAppFolder, "characters.txt");
            return path;
        }

        /// <summary>
        /// Returns path to the folder where we store profiles
        /// creates it if it doesn't yet exist
        /// </summary>
        /// <returns></returns>
        public static string GetRunningFolder()
        {
            var folder = Path.Combine(AppFolder, "Running");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        internal static string GetLoginCommandsFolder()
        {
            var folder = Path.Combine(AppFolder, "LoginCommands");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;

        }

        internal static string GetGameHeartbeatFilepath(int processId)
        {
            string filename = string.Format("game_{0}.txt", processId);
            string filepath = System.IO.Path.Combine(FileLocations.GetRunningFolder(), filename);
            return filepath;
        }
        // returns 0 for failure to parse
        public static int GetProcessIdFromGameHeartbeatFilepath(string filename)
        {
            if (filename.Length < 10) { return 0; }
            if (!filename.StartsWith("game_", StringComparison.CurrentCultureIgnoreCase)) { return 0; }
            if (!filename.EndsWith(".txt", StringComparison.CurrentCultureIgnoreCase)) { return 0; }
            int processId = 0;
            int i1 = "game_".Length;
            int len1 = filename.Length - "game_".Length - ".txt".Length;
            if (!int.TryParse(filename.Substring(i1, len1), out processId))
            {
                return 0;
            }
            return processId;
        }
        public static string GetGameToLauncherFilepath(int processId)
        {
            string filename = string.Format("gameToLauncher_{0}.txt", processId);
            string filepath = System.IO.Path.Combine(FileLocations.GetRunningFolder(), filename);
            return filepath;
        }
        public static string GetLauncherToGameFilepath(int processId)
        {
            string filename = string.Format("launcherToGame_{0}.txt", processId);
            string filepath = System.IO.Path.Combine(FileLocations.GetRunningFolder(), filename);
            return filepath;
        }

        public static string AppBaseFolder
        {
            get
            {
                // The folder for the roaming current user 
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
        }
        public static string AppFolder
        {
            get
            {
                string folderpath = System.IO.Path.Combine(AppBaseFolder, "ThwargLauncher");
                return folderpath;
            }
        }
        public static string OldAppFolder
        {
            get
            {
                string folderpath = System.IO.Path.Combine(AppBaseFolder, "ACAccountManager");
                return folderpath;
            }
        }
        public static string AppLogsFolder
        {
            get
            {
                string folderpath = System.IO.Path.Combine(AppFolder, "Logs");
                return folderpath;
            }
        }

        internal static DirectoryInfo PluginPersonalFolder
        {
            get
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins";
                DirectoryInfo pluginPersonalFolder = new DirectoryInfo(folder + @"\" + FilterName);

                try
                {
                    if (!pluginPersonalFolder.Exists)
                        pluginPersonalFolder.Create();
                }
                catch (Exception ex) { Debug.LogException(ex); }

                return pluginPersonalFolder;
            }
        }

        public static string ExpandFilepath(string filepath)
        {
            filepath = Environment.ExpandEnvironmentVariables(filepath);
            string pid = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
            filepath = filepath.Replace("%PID%", pid);
            return filepath;
        }

        public static void CreateAnyNeededFoldersOfFile(string filepath)
        {
            string folder = new FileInfo(filepath).Directory.FullName;
            Directory.CreateDirectory(folder);
        }

        public static void CreateAnyNeededFoldersOfFolder(string folderpath)
        {
            string folder = new DirectoryInfo(folderpath).FullName;
            Directory.CreateDirectory(folder);
        }
    }
}
