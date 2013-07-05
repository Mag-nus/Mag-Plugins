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

		System.Windows.Forms.Timer loginRetryTimer = new System.Windows.Forms.Timer();

		protected override void Startup()
		{
			ServerDispatch += new EventHandler<NetworkMessageEventArgs>(FilterCore_ServerDispatch);
			WindowMessage += new EventHandler<WindowMessageEventArgs>(FilterCore_WindowMessage);

			loginRetryTimer.Tick += new EventHandler(loginRetryTimer_Tick);
		}

		protected override void Shutdown()
		{
			ServerDispatch -= new EventHandler<NetworkMessageEventArgs>(FilterCore_ServerDispatch);
			WindowMessage -= new EventHandler<WindowMessageEventArgs>(FilterCore_WindowMessage);
		}

		void FilterCore_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				if (e.Message.Type == 0xF659) // One of your characters is still in the world. Please try again in a few minutes.
				{
					loginRetryTimer.Interval = 100;
					loginRetryTimer.Start();
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void loginRetryTimer_Tick(object sender, EventArgs e)
		{
			loginRetryTimer.Stop();

			if (loginRetryTimer.Interval == 200)
			{
				// Click the Enter button
				PostMessage(Host.Decal.Hwnd.ToInt32(), 0x0200, (IntPtr)0x00000000, (UIntPtr)0x0185015C);
				PostMessage(Host.Decal.Hwnd.ToInt32(), 0x0201, (IntPtr)0x00000001, (UIntPtr)0x0185015C);
				PostMessage(Host.Decal.Hwnd.ToInt32(), 0x0202, (IntPtr)0x00000000, (UIntPtr)0x0185015C);
			}

			if (loginRetryTimer.Interval == 100)
			{
				// Click the OK button
				PostMessage(Host.Decal.Hwnd.ToInt32(), 0x0200, (IntPtr)0x00000000, (UIntPtr)0x01460191);
				PostMessage(Host.Decal.Hwnd.ToInt32(), 0x0201, (IntPtr)0x00000001, (UIntPtr)0x01460191);
				PostMessage(Host.Decal.Hwnd.ToInt32(), 0x0202, (IntPtr)0x00000000, (UIntPtr)0x01460191);

				loginRetryTimer.Interval = 200;
				loginRetryTimer.Start();
			}
		}

		void FilterCore_WindowMessage(object sender, WindowMessageEventArgs e)
		{
			try
			{
				if (String.IsNullOrEmpty(CoreManager.Current.CharacterFilter.Name) || CoreManager.Current.CharacterFilter.Name == "LoginNotComplete")
				{
					/*
					message 0100 WM_KEYDOWN
					wParam 0000001B
					lParam 00010001
					*/
					if (e.Msg == 0x0100 && e.WParam == 0x0000001B && e.LParam == 0x00010001) // Esc Key
					{
						/*
						0200 WM_MOUSEMOVE
						wParam 00000000
						lParam 01400120

						0201 WM_LBUTTONDOWN
						wParam 00000001
						lParam 013F0120

						0202 WM_LBUTTONUP
						wParam 00000000
						lParam 013F0120
						*/

						// Click the Yes button
						PostMessage(Host.Decal.Hwnd.ToInt32(), 0x0200, (IntPtr)0x00000000, (UIntPtr)0x01400120);
						PostMessage(Host.Decal.Hwnd.ToInt32(), 0x0201, (IntPtr)0x00000001, (UIntPtr)0x01420122);
						PostMessage(Host.Decal.Hwnd.ToInt32(), 0x0202, (IntPtr)0x00000000, (UIntPtr)0x01420122);
					}

					// =======================================================================================================
					// TODO: Should also text for a left click on the Exit button and automatically click the Yes for the user
					// =======================================================================================================
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
