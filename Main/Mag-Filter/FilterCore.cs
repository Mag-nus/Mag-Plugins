using System;
using System.IO;
using System.Runtime.InteropServices;

using Decal.Adapter;

namespace MagFilter
{
	[FriendlyName("Mag-Filter")]
	public class FilterCore : FilterBase
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern bool PostMessage(int hhwnd, uint msg, IntPtr wparam, UIntPtr lparam);

		internal static string PluginName = "Mag-Filter";

		internal static DirectoryInfo PluginPersonalFolder
		{
			get
			{
				DirectoryInfo pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\" + PluginName);

				try
				{
					if (!pluginPersonalFolder.Exists)
						pluginPersonalFolder.Create();
				}
				catch (Exception ex) { Debug.LogException(ex); }

				return pluginPersonalFolder;
			}
		}


		readonly AutoRetryLogin autoRetryLogin = new AutoRetryLogin();
		readonly LoginCharacterTools loginCharacterTools = new LoginCharacterTools();
		readonly FastQuit fastQuit = new FastQuit();


		protected override void Startup()
		{
			ClientDispatch += new EventHandler<NetworkMessageEventArgs>(FilterCore_ClientDispatch);
			ServerDispatch += new EventHandler<NetworkMessageEventArgs>(FilterCore_ServerDispatch);
			WindowMessage += new EventHandler<WindowMessageEventArgs>(FilterCore_WindowMessage);
		}

		protected override void Shutdown()
		{
			ClientDispatch -= new EventHandler<NetworkMessageEventArgs>(FilterCore_ClientDispatch);
			ServerDispatch -= new EventHandler<NetworkMessageEventArgs>(FilterCore_ServerDispatch);
			WindowMessage -= new EventHandler<WindowMessageEventArgs>(FilterCore_WindowMessage);
		}

		void FilterCore_ClientDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				autoRetryLogin.FilterCore_ClientDispatch(sender, e);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void FilterCore_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				autoRetryLogin.FilterCore_ServerDispatch(sender, e);

				loginCharacterTools.FilterCore_ServerDispatch(sender, e);
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
	}
}
