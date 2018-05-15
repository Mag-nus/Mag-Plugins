using System;

using Mag.Shared;

using Decal.Adapter;

namespace MagTools.Client
{
	class WindowFrameRemover : IDisposable
	{
		public WindowFrameRemover()
		{
			try
			{
				CoreManager.Current.CharacterFilter.Login += new EventHandler<Decal.Adapter.Wrappers.LoginEventArgs>(CharacterFilter_Login);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private bool disposed;

		public void Dispose()
		{
			Dispose(true);

			// Use SupressFinalize in case a subclass
			// of this type implements a finalizer.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// If you need thread safety, use a lock around these 
			// operations, as well as in your methods that use the resource.
			if (!disposed)
			{
				if (disposing)
				{
					CoreManager.Current.CharacterFilter.Login -= new EventHandler<Decal.Adapter.Wrappers.LoginEventArgs>(CharacterFilter_Login);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void CharacterFilter_Login(object sender, Decal.Adapter.Wrappers.LoginEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.Misc.RemoveWindowFrame.Value)
					return;

				RemoveWindowFrame();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		static void RemoveWindowFrame()
		{
			User32.RECT rect = new User32.RECT();

			User32.GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

			// 1686 1078 -> 1680 1050
			//Debug.WriteToChat((rect.Right - rect.Left) + " " + (rect.Bottom - rect.Top));

			const int GWL_STYLE = -16;
			const int WS_BORDER = 0x00800000; //window with border
			const int WS_DLGFRAME = 0x00400000; //window with double border but no title
			const int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //window with a title bar

			int style = User32.GetWindowLong(CoreManager.Current.Decal.Hwnd, GWL_STYLE);

			User32.SetWindowLong(CoreManager.Current.Decal.Hwnd, GWL_STYLE, (style & ~WS_CAPTION));

			User32.MoveWindow(CoreManager.Current.Decal.Hwnd, rect.Left, rect.Top, (rect.Right - rect.Left) - TotalWindowFrameWidth, (rect.Bottom - rect.Top) - TotalWindowFrameHeight, true);
		}

		static int TotalWindowFrameWidth
		{
			get
			{
				// This is a hack because I don't know how to get the windows current theme border information
				// So, I just compare the current form size to the known AC client window sizes

				User32.RECT rect = new User32.RECT();

				User32.GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

				// Widths: 800 1024 1152 1280 1360 1400 1440 1600 1680 1792 1800 1856 1920 2048 2560 3840 4096 5040 7680
				if (rect.Width >= 7680) return rect.Width - 7680;
				if (rect.Width >= 5040) return rect.Width - 5040;
				if (rect.Width >= 4096) return rect.Width - 4096;
				if (rect.Width >= 3840) return rect.Width - 3840;
				if (rect.Width >= 2560) return rect.Width - 2560;
				if (rect.Width >= 2048) return rect.Width - 2048;
				if (rect.Width >= 1920) return rect.Width - 1920;
				if (rect.Width >= 1856) return rect.Width - 1856;
				if (rect.Width >= 1800) return rect.Width - 1800;
				if (rect.Width >= 1792) return rect.Width - 1792; // This would be broken for styles that have a border with >= 8, as it would be 1800+ then.
				if (rect.Width >= 1680) return rect.Width - 1680;
				if (rect.Width >= 1600) return rect.Width - 1600;
				if (rect.Width >= 1440) return rect.Width - 1440;
				if (rect.Width >= 1400) return rect.Width - 1400;
				if (rect.Width >= 1360) return rect.Width - 1360;
				if (rect.Width >= 1280) return rect.Width - 1280;
				if (rect.Width >= 1152) return rect.Width - 1152;
				if (rect.Width >= 1024) return rect.Width - 1024;
				if (rect.Width >= 800) return rect.Width - 800;

				// Windows 7 default: 6
				return 0;
			}
		}

		static int TotalWindowFrameHeight
		{
			get
			{
				// This is a hack because I don't know how to get the windows current theme border information
				// So, I just compare the current form size to the known AC client window sizes

				User32.RECT rect = new User32.RECT();

				User32.GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

				// Workarounds go here:
				if (rect.Height == (1024 + 28)) return 28;

				// Heights: 600 720 768 800 864 900 960 1024 1050 1080 1200 1344 1392 1440 1536 1600 2160 4320
				if (rect.Height >= 4320) return rect.Height - 4320;
				if (rect.Height >= 2160) return rect.Height - 2160;
				if (rect.Height >= 1600) return rect.Height - 1600;
				if (rect.Height >= 1536) return rect.Height - 1536;
				if (rect.Height >= 1440) return rect.Height - 1440;
				if (rect.Height >= 1392) return rect.Height - 1392;
				if (rect.Height >= 1344) return rect.Height - 1344;
				if (rect.Height >= 1200) return rect.Height - 1200;
				if (rect.Height >= 1080) return rect.Height - 1080;
				if (rect.Height >= 1050) return rect.Height - 1050;
				if (rect.Height >= 1024) return rect.Height - 1024;
				if (rect.Height >= 960) return rect.Height - 960;
				if (rect.Height >= 900) return rect.Height - 900;
				if (rect.Height >= 864) return rect.Height - 864;
				if (rect.Height >= 800) return rect.Height - 800;
				if (rect.Height >= 768) return rect.Height - 768;
				if (rect.Height >= 720) return rect.Height - 720;
				if (rect.Height >= 600) return rect.Height - 600;

				// Windows 7 default: 28
				return 0;
			}
		}
	}
}
