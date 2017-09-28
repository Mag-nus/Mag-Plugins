using System;
using System.Collections.Generic;
using System.Text;

namespace MagFilter.Channels
{
    /// <summary>
    /// Read & write commands to & from disk
    /// For commands passed between ThwargLauncher application and the MagFilter dll inside the game
    /// This code is called both from MagFilter inside the game
    /// And from the ThwargLauncher application (which loads its own copy of MagFilter inside itself)
    /// </summary>
    public class ChannelWriter
    {
        public void WriteCommandsToFile(Channel channel)
        {
            channel.NeedsToWrite = false;
            string filepath = GetChannelOutboundFilepath(channel);
            var writer = new CommandWriter();
            var cmdset = new CommandSet(channel.GetOutboundCommands(), channel.LastInboundProcessedUtc);
            writer.WriteCommandsToFile(cmdset, filepath);
        }
        public void ReadCommandsFromFile(Channel channel)
        {
            string filepath = GetChannelInboundFilepath(channel);
            var writer = new CommandWriter();
            var cmdset = writer.ReadCommandsFromFile(filepath);
            if (cmdset != null)
            {
                // process their acknowledgement
                channel.ProcessAcknowledgement(cmdset.Acknowledgement);
                // Append any new commands, and figure out our new acknowledgement value
                DateTime latestUtc = channel.LastInboundProcessedUtc;
                foreach (var cmd in cmdset.Commands)
                {
                    if (cmd.TimeStampUtc > channel.LastInboundProcessedUtc)
                    {
                        if (cmd.TimeStampUtc > latestUtc)
                        {
                            latestUtc = cmd.TimeStampUtc;
                        }
                        channel.EnqueueInbound(cmd);
                    }
                }
                if (channel.LastInboundProcessedUtc < latestUtc)
                {
                    channel.LastInboundProcessedUtc = latestUtc;
                    channel.NeedsToWrite = true;
                }
            }
        }
        public string GetChannelOutboundFilepath(Channel channel)
        {
            string prefix = (channel.InGameDll ? "outcmds" : "incmds");
            string filename = string.Format("{0}_{1}.txt", prefix, channel.ProcessId);
            string filepath = System.IO.Path.Combine(FileLocations.GetRunningFolder(), filename);
            return filepath;
        }
        public string GetChannelInboundFilepath(Channel channel)
        {
            string prefix = (!channel.InGameDll ? "outcmds" : "incmds");
            string filename = string.Format("{0}_{1}.txt", prefix, channel.ProcessId);
            string filepath = System.IO.Path.Combine(FileLocations.GetRunningFolder(), filename);
            return filepath;
        }
        public void StartWatcher(Channel channel)
        {
            var cmdfilepath = GetChannelInboundFilepath(channel);

            channel.FileWatcher.Path = System.IO.Path.GetDirectoryName(cmdfilepath);
            channel.FileWatcher.Filter = System.IO.Path.GetFileName(cmdfilepath);
            channel.FileWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            channel.FileWatcher.EnableRaisingEvents = true;
        }
        public void StopWatcher(Channel channel)
        {
            if (channel != null)
            {
                channel.FileWatcher.EnableRaisingEvents = false;
            }
        }
        public bool IsWatcherEnabled(Channel channel)
        {
            return channel.FileWatcher.EnableRaisingEvents;
        }
    }
}
