using System;
using System.Collections.Generic;
using System.Text;

using Mag.Shared;

namespace MagFilter
{
    class Heartbeat
    {
        private static object _locker = new object();
        private static Heartbeat theHeartbeat = new Heartbeat();
        private Channels.Channel _myChannel = Channels.Channel.MakeGameChannel();
        private MagFilterCommandParser _cmdParser = null;
        private DateTime LastSendAndReceive;
        private int _offlineTimeoutSeconds = 30;
        private const int TIMER_SECONDS = 3;
        private const int TIMER_SKIPSEC = 1; // Skip timer if send & received this recent

        private HeartbeatGameStatus _status = new HeartbeatGameStatus();

        public static void RecordServer(string ServerName)
        {
            theHeartbeat._status.ServerName = ServerName;
        }
        public static void RecordAccount(string AccountName)
        {
            theHeartbeat._status.AccountName = AccountName;
        }
        public static void RecordCharacterName(string CharacterName)
        {
            theHeartbeat._status.CharacterName = CharacterName;
        }
        public static void SendCommand(string commandString)
        {
            theHeartbeat._myChannel.EnqueueOutbound(
                new Channels.Command(DateTime.UtcNow, commandString)
                );
        }
        public static void SetCommandParser(MagFilterCommandParser parser) { theHeartbeat._cmdParser = parser; }
        public static void LaunchHeartbeat()
        {
            theHeartbeat.StartBeating();
        }
        private System.Timers.Timer _timer = null;
        private string _gameToLauncherFilepath;
        private int StringToInt(string text, int defval)
        {
            int value = defval;
            int.TryParse(text, out value);
            return value;
        }
        private void StartBeating()
        {
            if (_timer != null)
            {
                // already started
                return;
            }
            AssemblySettings settings = new AssemblySettings();
            _offlineTimeoutSeconds = StringToInt(settings.GetValue("ServerOnlineTimeoutSeconds", "30"), 30);

            int dllProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
            _gameToLauncherFilepath = FileLocations.GetGameHeartbeatFilepath(dllProcessId);

            int intervalMilliseconds = 1000 * TIMER_SECONDS;
            _timer = new System.Timers.Timer();
            _timer.Interval = intervalMilliseconds;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();
            StartChannelFileWatcher();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }
        private void StartChannelFileWatcher()
        {
            var writer = new Channels.ChannelWriter();
            if (!writer.IsWatcherEnabled(_myChannel))
            {
                writer.StartWatcher(_myChannel);
                _myChannel.FileWatcher.Changed += OnChannelFileWatcherChanged;
            }
        }
        void OnChannelFileWatcherChanged(object sender, System.IO.FileSystemEventArgs e)
        {
            log.WriteInfo("Channel File Watcher Fired");
            Heartbeat.SendAndReceiveImmediately();
        }
        void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            lock (_locker)
            {
                if (_myChannel != null)
                {
                    var writer = new Channels.ChannelWriter();
                    if (writer.IsWatcherEnabled(_myChannel))
                    {
                        writer.StopWatcher(_myChannel);
                    }
                }
                if (_timer != null)
                {
                    _timer.Stop();
                }
                log.WriteInfo("process exit");
            }
        }
        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (System.Threading.Monitor.TryEnter(_locker, 500)) // half second timeout
            {
                try
                {
                    if ((DateTime.UtcNow - LastSendAndReceive).TotalMilliseconds < 1000 * TIMER_SKIPSEC)
                    {
                        return;
                    }
                    SendAndReceiveCommands();
                }
                finally
                {
                    System.Threading.Monitor.Exit(_locker);
                }
            }
        }
        public static void SendAndReceiveImmediately()
        {
            if (System.Threading.Monitor.TryEnter(_locker, 1000)) // one second timeout
            {
                try
                {
                    try
                    {
                        theHeartbeat._timer.Stop();
                        theHeartbeat.SendAndReceiveCommands();
                    }
                    finally
                    {
                        theHeartbeat._timer.Start();
                    }
                }
                finally
                {
                    System.Threading.Monitor.Exit(_locker);
                }
            }
        }

        public bool IsOnline()
        {
            try
            {
                var gap = DateTime.UtcNow - FilterCore.GetLastServerDispatchUtc();
                bool isOnline = gap.TotalSeconds < _offlineTimeoutSeconds;
                return isOnline;
                // No, not this stuff
                /*
                var worldFilter = Decal.Adapter.CoreManager.Current.WorldFilter;
                if (worldFilter == null) { log.WriteError("worldFilter null"); return false; }
                var inventory = worldFilter.GetInventory();
                if (inventory == null) { log.WriteError("inventory null"); return false; }
                var firstObject = inventory.First;
                if (firstObject == null) { log.WriteError("firstObject null"); return false; }
                return Decal.Adapter.CoreManager.Current.Actions.IsValidObject(firstObject.Id);
                */
            }
            catch (Exception exc)
            {
                log.WriteError("IsOnline exception: " + exc.ToString());
                return false;
            }
        }

        /// <summary>
        /// This may be called on timer thread *OR* on external caller's thread
        /// </summary>
        private void SendAndReceiveCommands()
        {
            bool success = true;
            try
            {
                _status.TeamList = _cmdParser.GetTeamList();
                _status.IsOnline = IsOnline();
                LaunchControl.RecordHeartbeatStatus(_gameToLauncherFilepath, _status);
            }
            catch (Exception exc)
            {
                success = false;
                log.WriteError("Exception writing heartbeat status: " + exc.ToString());
            }
            try
            {
                if (_myChannel.NeedsToWrite)
                {
                    var writer = new Channels.ChannelWriter();
                    writer.WriteCommandsToFile(_myChannel);
                }
            }
            catch (Exception exc)
            {
                success = false;
                log.WriteError("Exception writing command file status: " + exc.ToString());
            }
            try
            {
                ReadAndProcessInboundCommands();
            }
            catch (Exception exc)
            {
                success = false;
                log.WriteError("Exception reading command file status: " + exc + " - "  + exc.StackTrace);
            }
            if (success)
            {
                LastSendAndReceive = DateTime.UtcNow;
            }
        }
        private void ReadAndProcessInboundCommands()
        {
            string status = "Creating ChannelWriter";
            try
            {
                var writer = new Channels.ChannelWriter();
                status = "writer.ReadCommandsFromFile";
                writer.ReadCommandsFromFile(_myChannel);
                status = "while HasInboundCommandCount";
                while (_myChannel.HasInboundCommandCount())
                {
                    status = "DequeueInbound";
                    var cmd = _myChannel.DequeueInbound();
                    status = "ExecuteGameCommandString";
                    ExecuteGameCommandString(cmd.CommandString);
                    status = "time stamp handling";
                    if (cmd.TimeStampUtc > _myChannel.LastInboundProcessedUtc)
                    {
                        _myChannel.LastInboundProcessedUtc = cmd.TimeStampUtc;
                        _myChannel.NeedsToWrite = true;
                    }
                    status = "finishing loop";
                }
            }
            catch (Exception exc)
            {
                throw new Exception("ReadAndProcessInboundCommands: " + status, exc);
            }
        }
        private void ExecuteGameCommandString(string commandString)
        {
            // Note: "Mag.Shared.PostMessageTools.SendMsg" does not work
            // DecalProxy.DispatchChatToBoxWithPluginIntercept(commandString) works
            _cmdParser.ExecuteCommandFromLauncher(commandString); // cross-thread
        }
        private static string EncodeString(string text)
        {
            return LaunchControl.EncodeString(text);
        }
    }
}
