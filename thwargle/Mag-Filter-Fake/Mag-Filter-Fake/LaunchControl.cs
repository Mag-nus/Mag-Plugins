using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace MagFilter
{
    public class LaunchControl
    {
        public class LaunchInfo
        {
            public string ServerName;
            public string AccountName;
            public string CharacterName;
            public DateTime Time;
        }
        public LaunchInfo GetLaunchInfo()
        {
            var info = new LaunchInfo();
            string launchFilepath = FileLocations.GetCurrentLaunchFile();

            if (File.Exists(launchFilepath))
            {
                using (var file = new StreamReader(launchFilepath))
                {
                    string contents = file.ReadToEnd();
                    string[] stringSeps = new string[] { "\r\n" };
                    string[] lines = contents.Split(stringSeps, StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Length == 4)
                    {
                        if (lines[0].StartsWith("ServerName:")
                            && lines[1].StartsWith("AccountName:")
                            && lines[2].StartsWith("CharacterName:")
                            && lines[3].StartsWith("Date:")
                            )
                        {
                            info.ServerName = lines[0].Substring("ServerName:".Length);
                            info.AccountName = lines[1].Substring("AccountName:".Length);
                            info.CharacterName = lines[2].Substring("CharacterName:".Length);
                            info.Time = DateTime.Parse(lines[2].Substring("Date:".Length));
                        }
                    }
                }
            }
            
            return info;
        }
        public void RecordLaunchInfo(string serverName, string accountName, string characterName, DateTime timestamp)
        {
            string launchInfoPath = FileLocations.GetCurrentLaunchFile();
            using (var file = new StreamWriter(launchInfoPath, append: false))
            {
                file.WriteLine("ServerName:" + serverName);
                file.WriteLine("AccountName:" + accountName);
                file.WriteLine("CharacterName:" + characterName);
                file.WriteLine("Date:" + timestamp.ToString());
            }
        }
    }
}
