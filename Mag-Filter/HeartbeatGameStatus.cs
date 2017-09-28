using System;

namespace MagFilter
{
    public class HeartbeatGameStatus
    {
        public const string MASTER_FILE_VERSION = "1.4";
        public const string MASTER_FILE_VERSION_COMPAT = "1";

        public string FileVersion;
        public string ServerName;
        public string AccountName;
        public string CharacterName;
        public int UptimeSeconds;
        public int ProcessId;
        public string TeamList; // separated by commas and no spaces
        public string MagFilterVersion;
        public string MagFilterFilePath;
        public bool IsOnline;
    }
}
