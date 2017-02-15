using System;
using System.Collections.Generic;
using System.Text;

namespace MagFilter.Channels
{
    public class Channel
    {
        private static object _locker = new object();
        public static Channel MakeLauncherChannel(int processId)
        {
            return new Channel(dll: false, processId: processId);
        }
        public static Channel MakeGameChannel()
        {
            int myProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
            return new Channel(dll: true, processId: myProcessId);
        }
        private Channel(bool dll, int processId)
        {
            this.InGameDll = dll;
            this.ProcessId = processId;
        }
        public readonly bool InGameDll;
        public readonly int ProcessId;
        private List<Command> OutboundCommands = new List<Command>();
        private List<Command> InboundCommands = new List<Command>();
        public DateTime LastInboundProcessedUtc = DateTime.MinValue;
        public bool NeedsToWrite = false;

        public bool HasOutboundCommands() { return OutboundCommands.Count > 0; }
        public IList<Command> GetOutboundCommands() { return OutboundCommands; }
        public bool HasInboundCommandCount() { return InboundCommands.Count > 0; }

        public void EnqueueOutbound(Command cmd)
        {
            lock (_locker)
            {
                OutboundCommands.Add(cmd);
                NeedsToWrite = true;
            }
        }
        public void EnqueueInbound(Command cmd)
        {
            lock (_locker)
            {
                InboundCommands.Add(cmd);
            }
        }
        public Command DequeueInbound()
        {
            if (InboundCommands.Count == 0)
            {
                return null;
            }
            else
            {
                lock (_locker)
                {
                    var cmd = InboundCommands[0];
                    InboundCommands.RemoveAt(0);
                    NeedsToWrite = true;
                    return cmd;
                }
            }
        }
        public void ProcessAcknowledgement(DateTime ackTimeUtc)
        {
            var pending = new List<Command>(OutboundCommands.Count);
            bool changed = false;
            foreach (var cmd in OutboundCommands)
            {
                if (cmd.TimeStampUtc > ackTimeUtc)
                {
                    pending.Add(cmd);
                }
                else
                {
                    changed = true;
                }
            }
            OutboundCommands = pending;
            if (changed)
            {
                NeedsToWrite = true;
            }
        }
    }
}
