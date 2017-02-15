using System;
using System.IO;
using System.Runtime.InteropServices;

using Mag.Shared;
using Mag.Shared.Settings;

using Decal.Adapter;

namespace MagFilter
{
	[FriendlyName("MagFilter")]
	public class FilterCore : FilterBase
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern bool PostMessage(int hhwnd, uint msg, IntPtr wparam, UIntPtr lparam);


		readonly AutoRetryLogin autoRetryLogin = new AutoRetryLogin();
		readonly LoginCharacterTools loginCharacterTools = new LoginCharacterTools();
		readonly FastQuit fastQuit = new FastQuit();
		readonly LoginCompleteMessageQueueManager loginCompleteMessageQueueManager = new LoginCompleteMessageQueueManager();
		readonly AfterLoginCompleteMessageQueueManager afterLoginCompleteMessageQueueManager = new AfterLoginCompleteMessageQueueManager();

		DefaultFirstCharacterManager defaultFirstCharacterManager;
	    private LauncherChooseCharacterManager chooseCharacterManager;
        private MagFilterCommandExecutor magFilterCommandExecutor;
        private MagFilterCommandParser magFilterCommandParser;
		LoginNextCharacterManager loginNextCharacterManager;

        private string PluginName { get { return FileLocations.PluginName; } }

        public void ExternalStartup() { Startup(); } // for game emulator
		protected override void Startup()
		{
            Debug.Init(FileLocations.PluginPersonalFolder.FullName + @"\Exceptions.txt", PluginName);
            SettingsFile.Init(FileLocations.GetPluginSettingsFile(), PluginName);
		    LogStartup();

			defaultFirstCharacterManager = new DefaultFirstCharacterManager(loginCharacterTools);
            chooseCharacterManager = new LauncherChooseCharacterManager(loginCharacterTools);
            magFilterCommandExecutor = new MagFilterCommandExecutor();
            magFilterCommandParser = new MagFilterCommandParser(magFilterCommandExecutor);
            Heartbeat.SetCommandParser(magFilterCommandParser);
            loginNextCharacterManager = new LoginNextCharacterManager(loginCharacterTools);

			ClientDispatch += new EventHandler<NetworkMessageEventArgs>(FilterCore_ClientDispatch);
			ServerDispatch += new EventHandler<NetworkMessageEventArgs>(FilterCore_ServerDispatch);
			WindowMessage += new EventHandler<WindowMessageEventArgs>(FilterCore_WindowMessage);


			CommandLineText += new EventHandler<ChatParserInterceptEventArgs>(FilterCore_CommandLineText);
		}

        private void LogStartup()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            log.WriteLogMsg(string.Format(
                "MagFilter.Startup, AssemblyVer: {0}, AssemblyFileVer: {1}",
                assembly.GetName().Version,
                System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location)
                                ));
        }

        public void ExternalShutdown() { Shutdown(); } // for game emulator
        protected override void Shutdown()
		{
			ClientDispatch -= new EventHandler<NetworkMessageEventArgs>(FilterCore_ClientDispatch);
			ServerDispatch -= new EventHandler<NetworkMessageEventArgs>(FilterCore_ServerDispatch);
			WindowMessage -= new EventHandler<WindowMessageEventArgs>(FilterCore_WindowMessage);

			CommandLineText -= new EventHandler<ChatParserInterceptEventArgs>(FilterCore_CommandLineText);

            log.WriteLogMsg("FilterCore-Shutdown");
		}

        public void CallFilterCoreClientDispatch(object sender, NetworkMessageEventArgs e) // for game emulator
        {
            FilterCore_ClientDispatch(sender, e);
        }
		void FilterCore_ClientDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				autoRetryLogin.FilterCore_ClientDispatch(sender, e);
				loginCompleteMessageQueueManager.FilterCore_ClientDispatch(sender, e);
				afterLoginCompleteMessageQueueManager.FilterCore_ClientDispatch(sender, e);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void FilterCore_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				autoRetryLogin.FilterCore_ServerDispatch(sender, e);
				loginCharacterTools.FilterCore_ServerDispatch(sender, e);

				defaultFirstCharacterManager.FilterCore_ServerDispatch(sender, e);
                chooseCharacterManager.FilterCore_ServerDispatch(sender, e);
				loginNextCharacterManager.FilterCore_ServerDispatch(sender, e);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void FilterCore_WindowMessage(object sender, WindowMessageEventArgs e)
		{
			try
			{
				fastQuit.FilterCore_WindowMessage(sender, e);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void FilterCore_CommandLineText(object sender, ChatParserInterceptEventArgs e)
		{
			try
			{
				loginCompleteMessageQueueManager.FilterCore_CommandLineText(sender, e);
				afterLoginCompleteMessageQueueManager.FilterCore_CommandLineText(sender, e);

				defaultFirstCharacterManager.FilterCore_CommandLineText(sender, e);
                chooseCharacterManager.FilterCore_CommandLineText(sender, e);
				loginNextCharacterManager.FilterCore_CommandLineText(sender, e);
                magFilterCommandParser.FilterCore_CommandLineText(sender, e);
            }
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
