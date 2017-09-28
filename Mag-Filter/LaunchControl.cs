using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GenericSettingsFile;

namespace MagFilter
{
    public class LaunchControl
    {
        public class LaunchInfo
        {
            public const string MASTER_FILE_VERSION = "1.2";
            public const string MASTER_FILE_VERSION_COMPAT = "1";

            public bool IsValid;
            public string FileVersion;
            public DateTime LaunchTime;
            public string ServerName;
            public string AccountName;
            public string CharacterName;
        }
        public class LaunchResponse
        {
            public const string MASTER_FILE_VERSION = "1.2";
            public const string MASTER_FILE_VERSION_COMPAT = "1";

            public bool IsValid;
            public string FileVersion;
            public DateTime ResponseTime;
            public int ProcessId;
            public string MagFilterVersion;
        }
        public class HeartbeatResponse
        {
            public bool IsValid;
            public HeartbeatGameStatus Status = new HeartbeatGameStatus();
            public string LogFilepath;
        }
        public class MagFilterInfo
        {
            public string MagFilterPath;
            public string MagFilterVersion;
        }
        /// <summary>
        /// Called by ThwargLauncher
        /// </summary>
        /// <returns></returns>
        public static MagFilterInfo GetMagFilterInfo()
        {
            var info = new MagFilterInfo();
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            info.MagFilterVersion = assembly.GetName().Version.ToString();
            info.MagFilterPath = assembly.Location;
            return info;
        }
        /// <summary>
        /// Called by ThwargLauncher
        /// </summary>
        public static void RecordLaunchInfo(string serverName, string accountName, string characterName, DateTime timestampUtc)
        {
            string filepath = FileLocations.GetCurrentLaunchFilePath();
            using (var file = new StreamWriter(filepath, append: false))
            {
                file.WriteLine("FileVersion:{0}", LaunchInfo.MASTER_FILE_VERSION);
                file.WriteLine("Timestamp=TimeUtc:'{0}'", timestampUtc);
                file.WriteLine("ServerName:{0}", serverName);
                file.WriteLine("AccountName:{0}", accountName);
                file.WriteLine("CharacterName:{0}", characterName);
            }
        }
        public static LaunchInfo DebugGetLaunchInfo()
        {
            return GetLaunchInfo();
        }
        /// <summary>
        /// Called by Mag-Filter
        /// </summary>
        internal static LaunchInfo GetLaunchInfo()
        {
            var info = new LaunchInfo();
            try
            {
                string filepath = FileLocations.GetCurrentLaunchFilePath();

                if (!File.Exists(filepath))
                {
                    log.WriteError("No launch file found: '{0}'", filepath);
                    return info;
                }
                var settings = (new SettingsFileLoader()).ReadSettingsFile(filepath);

                info.FileVersion = SettingHelpers.GetSingleStringValue(settings, "FileVersion");
                if (!info.FileVersion.StartsWith(LaunchInfo.MASTER_FILE_VERSION_COMPAT))
                {
                    throw new Exception(string.Format(
                        "Incompatible launch info file version: {0}",
                        info.FileVersion));
                }

                info.LaunchTime = settings.GetValue("Timestamp").GetDateParam("TimeUtc");
                TimeSpan maxLatency = new TimeSpan(0, 0, 0, 45); // 30 seconds max latency from exe call to game launch
                if (DateTime.UtcNow - info.LaunchTime >= maxLatency)
                {
                    log.WriteInfo("DateTime.UtcNow-'{0}', info.LaunchTime='{1}', maxLatency='{2}'", DateTime.UtcNow, info.LaunchTime, maxLatency);
                    log.WriteInfo("Launch file TimeUtc too old");
                    return info;
                }

                info.ServerName = SettingHelpers.GetSingleStringValue(settings, "ServerName");
                info.AccountName = SettingHelpers.GetSingleStringValue(settings, "AccountName");
                info.CharacterName = SettingHelpers.GetSingleStringValue(settings, "CharacterName");

                info.IsValid = true;
            }
            catch (Exception exc)
            {
                log.WriteError("GetLaunchInfo exception: {0}", exc);
            }
            return info;
        }
        /// <summary>
        /// Called by Mag-Filter
        /// </summary>
        internal static void RecordLaunchResponse(DateTime timestampUtc)
        {
            string filepath = FileLocations.GetCurrentLaunchResponseFilePath();
            using (var file = new StreamWriter(filepath, append: false))
            {
                int pid = System.Diagnostics.Process.GetCurrentProcess().Id;
                file.WriteLine("FileVersion:{0}", LaunchResponse.MASTER_FILE_VERSION);
                file.WriteLine("TimeUtc:" + timestampUtc);
                file.WriteLine("ProcessId:{0}", pid);
                file.WriteLine("MagFilterVersion:{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            }
        }
        /// <summary>
        /// Called by ThwargLauncher
        /// </summary>
        public static LaunchResponse GetLaunchResponse(TimeSpan maxLatency)
        {
            var info = new LaunchResponse();
            try
            {
                string filepath = FileLocations.GetCurrentLaunchResponseFilePath();
                if (string.IsNullOrEmpty(filepath)) { return info; }
                if (!File.Exists(filepath)) { return info; }

                var settings = (new SettingsFileLoader()).ReadSettingsFile(filepath);

                info.FileVersion = SettingHelpers.GetSingleStringValue(settings, "FileVersion");
                if (!info.FileVersion.StartsWith(LaunchResponse.MASTER_FILE_VERSION_COMPAT))
                {
                    throw new Exception(string.Format(
                        "Incompatible launch response file version: {0}",
                        info.FileVersion));
                }

                info.ResponseTime = SettingHelpers.GetSingleDateTimeValue(settings, "TimeUtc");
                if (DateTime.UtcNow - info.ResponseTime >= maxLatency)
                {
                    return info;
                }
                info.ProcessId = SettingHelpers.GetSingleIntValue(settings, "ProcessId");
                info.MagFilterVersion = SettingHelpers.GetSingleStringValue(settings, "MagFilterVersion");

                info.IsValid = true;
            }
            catch (Exception exc)
            {
                log.WriteError("GetLaunchResponse exception: {0}", exc);
            }
            return info;
        }
        internal static void RecordHeartbeatStatus(string filepath, HeartbeatGameStatus status)
        {
            string contents = RecordHeartbeatStatusToString(status);
            WriteTextToFile(contents, filepath);
        }
        private static string RecordHeartbeatStatusToString(HeartbeatGameStatus status)
        {
            using (var stream = new StringWriter())
            {
                TimeSpan span = DateTime.Now - System.Diagnostics.Process.GetCurrentProcess().StartTime;
                stream.WriteLine("FileVersion:{0}", HeartbeatGameStatus.MASTER_FILE_VERSION);
                stream.WriteLine("UptimeSeconds:{0}", (int)span.TotalSeconds);
                stream.WriteLine("ServerName:{0}", status.ServerName);
                stream.WriteLine("AccountName:{0}", status.AccountName);
                stream.WriteLine("CharacterName:{0}", status.CharacterName);
                stream.WriteLine("LogFilepath:{0}", log.GetLogFilepath());
                stream.WriteLine("ProcessId:{0}", System.Diagnostics.Process.GetCurrentProcess().Id);
                stream.WriteLine("TeamList:{0}", status.TeamList);
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                stream.WriteLine("MagFilterVersion:{0}", assembly.GetName().Version);
                stream.WriteLine("MagFilterFilePath:{0}", assembly.Location);
                stream.WriteLine("IsOnline:{0}", status.IsOnline);
                var text = stream.ToString();
                return text;
            }
        }
        private static void WriteTextToFile(string contents, string filepath)
        {
            using (var file = File.Open(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var outstr = new StreamWriter(file, Encoding.UTF8))
                {
                    outstr.Write(contents);
                }
            }
        }
        /// <summary>
        /// Called by ThwargLauncher
        /// </summary>
        public static string GetHeartbeatStatusFileVersion() { return HeartbeatGameStatus.MASTER_FILE_VERSION; }
        public static HeartbeatResponse GetHeartbeatStatus(string filepath)
        {
            var info = new HeartbeatResponse();
            try
            {
                if (string.IsNullOrEmpty(filepath)) { return info; }
                if (!File.Exists(filepath)) { return info; }

                var settings = (new SettingsFileLoader()).ReadSettingsFile(filepath);

                info.Status.FileVersion = SettingHelpers.GetSingleStringValue(settings, "FileVersion");
                if (!info.Status.FileVersion.StartsWith(HeartbeatGameStatus.MASTER_FILE_VERSION_COMPAT))
                {
                    throw new Exception(string.Format(
                        "Incompatible heartbeat status file version: {0}",
                        info.Status.FileVersion));
                }
                info.Status.UptimeSeconds = SettingHelpers.GetSingleIntValue(settings, "UptimeSeconds");
                info.Status.ServerName = SettingHelpers.GetSingleStringValue(settings, "ServerName");
                info.Status.AccountName = SettingHelpers.GetSingleStringValue(settings, "AccountName");
                info.Status.CharacterName = SettingHelpers.GetSingleStringValue(settings, "CharacterName");
                info.LogFilepath = SettingHelpers.GetSingleStringValue(settings, "LogFilepath");
                info.Status.ProcessId = SettingHelpers.GetSingleIntValue(settings, "ProcessId");
                info.Status.TeamList = SettingHelpers.GetSingleStringValue(settings, "TeamList");
                info.Status.MagFilterVersion = SettingHelpers.GetSingleStringValue(settings, "MagFilterVersion");
                info.Status.MagFilterFilePath = SettingHelpers.GetSingleStringValue(settings, "MagFilterFilePath");
                info.Status.IsOnline = SettingHelpers.GetSingleBoolValue(settings, "IsOnline", false);

                info.IsValid = true;
            }
            catch (Exception exc)
            {
                log.WriteError("GetHeartbeatStatus exception: {0}", exc);
            }
            return info;
        }
        private class StringSetting { public bool IsValid; public string Value; }
        private static StringSetting ParseStringSetting(string line, string prefix)
        {
            var result = new StringSetting();
            if (BeginsWith(line, prefix))
            {
                string text = line.Substring(prefix.Length);
                result.IsValid = true;
                result.Value = text;
            }
            return result;
        }
        private class IntSetting { public bool IsValid; public int Value; }
        private static IntSetting ParseIntSetting(string line, string prefix)
        {
            var result = new IntSetting();
            var strret = ParseStringSetting(line, prefix);
            if (strret.IsValid)
            {
                string text = strret.Value;
                int value = 0;
                if (int.TryParse(text, out value))
                {
                    result.IsValid = true;
                    result.Value = value;
                }
            }
            return result;
        }
        private class DateTimeSetting { public bool IsValid; public DateTime Value; }
        private static DateTimeSetting ParseDateTimeSetting(string line, string prefix)
        {
            var result = new DateTimeSetting();
            var strret = ParseStringSetting(line, prefix);
            if (strret.IsValid)
            {
                string text = strret.Value;
                DateTime value = DateTime.MinValue;
                if (DateTime.TryParse(text, out value))
                {
                    result.IsValid = true;
                    result.Value = value;
                }
            }
            return result;
        }
        /// <summary>
        /// Line starts with specified prefix and has at least one character beyond it
        ///  (primarily used to Substring(prefix.Length) will not fail
        /// </summary>
        private static bool BeginsWith(string line, string prefix)
        {
            return line != null && line.StartsWith(prefix) && line.Length > prefix.Length;
        }
        public static string EncodeString(string text)
        {
            if (text == null) { return ""; }
            text = text.Replace("|", "|V");
            text = text.Replace("\"", "|Q");
            text = text.Replace(",", "|C");
            text = text.Replace("'", "|q");
            return text;
        }
        public static string DecodeString(string text)
        {
            if (text == null) { return ""; }
            text = text.Replace("|q", "'");
            text = text.Replace("|C", ",");
            text = text.Replace("|Q", "\"");
            text = text.Replace("|V", "|");
            return text;
        }
    }
}
