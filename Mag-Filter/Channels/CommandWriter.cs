using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GenericSettingsFile;

namespace MagFilter.Channels
{
    class CommandWriter
    {
        public const string MASTER_FILE_VERSION = "1.2";
        public const string MASTER_FILE_VERSION_COMPAT = "1";

        public void WriteCommandsToFile(CommandSet cmdset, string filepath)
        {
            var contents = WriteCommandsToString(cmdset);
            WriteTextToFile(contents, filepath);
        }
        private string WriteCommandsToString(CommandSet cmdset)
        {
            using (var stream = new StringWriter())
            {
                DateTime timestampUtc = DateTime.UtcNow;
                stream.WriteLine("FileVersion:{0}", MASTER_FILE_VERSION);
                stream.WriteLine("Timestamp=TimeUtc:'{0:o}'", timestampUtc);
                stream.WriteLine("AcknowledgementUtc:{0:o}", cmdset.Acknowledgement);
                stream.WriteLine("CommandCount:{0}", cmdset.Commands.Count);
                for (int i = 0; i < cmdset.Commands.Count; ++i)
                {
                    Command cmd = cmdset.Commands[i];
                    stream.WriteLine("Command{0}=TimeStampUtc:'{1}' CommandString:'{2}'", i + 1, cmd.TimeStampUtc, cmd.CommandString);
                }
                return stream.ToString();
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
        public CommandSet ReadCommandsFromFile(string filepath)
        {
            try
            {
                if (!File.Exists(filepath))
                {
                    return null;
                }
                SettingsCollection settings = null; 
                try
                {
                    settings = (new SettingsFileLoader()).ReadSettingsFile(filepath);
                }
                catch (Exception exc)
                {
                    log.WriteError("Exception reading commands from file '{0}': {1}", filepath, exc);
                    //return new CommandSet(new List<Command>(), new DateTime());
                    return null;
                }

                string fileVersion = SettingHelpers.GetSingleStringValue(settings, "FileVersion");
                if (!fileVersion.StartsWith(MASTER_FILE_VERSION_COMPAT))
                {
                    log.WriteError(
                        "Incompatible command file, version: {0}",
                        fileVersion);
                    return null;
                }
                DateTime timestamp = settings.GetValue("Timestamp").GetUtcDateParam("TimeUtc");
                if (DateTime.UtcNow - timestamp > TimeSpan.FromHours(1))
                {
                    log.WriteInfo(
                        "Ignoring command file older than one hour, timestamp={0:O}",
                        timestamp);
                    return null;
                }
                DateTime acknowledgement = SettingHelpers.GetSingleDateTimeValue(settings, "AcknowledgementUtc");
                int ncmds = SettingHelpers.GetSingleIntValue(settings, "CommandCount");
                List<Command> cmdlist = new List<Command>(ncmds);
                for (int i = 0; i < ncmds; ++i)
                {
                    string key = string.Format("Command{0}", i + 1);
                    var cmdData = settings.GetValue(key);
                    DateTime time = cmdData.GetDateParam("TimeStampUtc");
                    string cmdString = cmdData.GetStringParam("CommandString");
                    var cmd = new Command(time, cmdString);
                    cmdlist.Add(cmd);
                }

                CommandSet cmdset = new CommandSet(cmdlist, acknowledgement);
                return cmdset;
            }
            catch (Exception exc)
            {
                log.WriteError("Exception reading command file: {0}", exc);
                return null;
            }
        }
    }
}
