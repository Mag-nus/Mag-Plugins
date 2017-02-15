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
        private System.Windows.Forms.Timer _timer = null;
        private string _gameToLauncherFilepath;
        private void StartBeating()
        {
            int dllProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
            _gameToLauncherFilepath = FileLocations.GetGameHeartbeatFilepath(dllProcessId);

            int intervalMilliseconds = 3000;
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = intervalMilliseconds;
            _timer.Tick += timer_Tick;
            _timer.Enabled = true;
            _timer.Start();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            lock (_locker)
            {
                if (_timer != null)
                {
                    log.WriteLogMsg("process exit");
                    _timer.Stop();
                }
            }
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (System.Threading.Monitor.TryEnter(_locker))
            {
                try
                {
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
            if (System.Threading.Monitor.TryEnter(_locker))
            {
                try
                {
                    theHeartbeat._timer.Stop();
                    theHeartbeat.SendAndReceiveCommands();
                    theHeartbeat._timer.Start();
                }
                finally
                {
                    System.Threading.Monitor.Exit(_locker);
                }
            }
            lock (_locker)
            {
            }
        }
        /// <summary>
        /// This may be called on timer thread *OR* on external caller's thread
        /// </summary>
        private void SendAndReceiveCommands()
        {
            try
            {
                _status.TeamList = _cmdParser.GetTeamList();
                LaunchControl.RecordHeartbeatStatus(_gameToLauncherFilepath, _status);
            }
            catch (Exception exc)
            {
                log.WriteLogMsg("Exception writing heartbeat status: " + exc.ToString());
            }
            try
            {
                if (_myChannel.HasOutboundCommands())
                {
                    var writer = new Channels.ChannelWriter();
                    writer.WriteCommandsToFile(_myChannel);
                }
            }
            catch (Exception exc)
            {
                log.WriteLogMsg("Exception writing command file status: " + exc.ToString());
            }
            try
            {
                ReadAndProcessInboundCommands();
            }
            catch (Exception exc)
            {
                log.WriteLogMsg("Exception reading command file status: " + exc.ToString());
            }
        }
        private void ReadAndProcessInboundCommands()
        {
            var writer = new Channels.ChannelWriter();
            writer.ReadCommandsFromFile(_myChannel);
            DateTime myack = _myChannel.LastInboundProcessedUtc;
            while (_myChannel.HasInboundCommandCount())
            {
                var cmd = _myChannel.DequeueInbound();
                if (cmd.TimeStampUtc > myack)
                {
                    myack = cmd.TimeStampUtc;
                }
                ExecuteGameCommandString(cmd.CommandString);
            }
            _myChannel.LastInboundProcessedUtc = myack;
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
