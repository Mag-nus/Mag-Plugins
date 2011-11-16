using System;
using System.Runtime.InteropServices;

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

				ShowWindowFrame(false);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		internal struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
		static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

		//Gets window attributes
		[DllImport("USER32.DLL")]
		static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		//Sets window attributes
		[DllImport("USER32.DLL")]
		static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		/// <summary>
		/// The MoveWindow function changes the position and dimensions of the specified window. For a top-level window, the position and dimensions are relative to the upper-left corner of the screen. For a child window, they are relative to the upper-left corner of the parent window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="x">Specifies the new position of the left side of the window.</param>
		/// <param name="y">Specifies the new position of the top of the window.</param>
		/// <param name="nWidth">Specifies the new width of the window.</param>
		/// <param name="nHeight">Specifies the new height of the window.</param>
		/// <param name="bRepaint">Specifies whether the window is to be repainted. If this parameter is TRUE, the window receives a message. If the parameter is FALSE, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of moving a child window.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para></returns>
		[DllImport("user32.dll", SetLastError = true)]
		static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

		/// <summary>
		/// Right now this just removes the the window frame. visible param has no function.
		/// </summary>
		/// <param name="visible"></param>
		static void ShowWindowFrame(bool visible)
		{
			RECT rect = new RECT();

			GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

			// 1686 1078 -> 1680 1050
			//Debug.WriteToChat((rect.Right - rect.Left) + " " + (rect.Bottom - rect.Top));

			const int GWL_STYLE = -16;
			const int WS_BORDER = 0x00800000; //window with border
			const int WS_DLGFRAME = 0x00400000; //window with double border but no title
			const int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //window with a title bar

			int style = GetWindowLong(CoreManager.Current.Decal.Hwnd, GWL_STYLE);

			SetWindowLong(CoreManager.Current.Decal.Hwnd, GWL_STYLE, (style & ~WS_CAPTION));

			MoveWindow(CoreManager.Current.Decal.Hwnd, rect.Left, rect.Top, (rect.Right - rect.Left) - TotalWindowFrameWidth, (rect.Bottom - rect.Top) - TotalWindowFrameHeight, true);
		}

		static int TotalWindowFrameWidth
		{
			get
			{
				// This is a hack because I don't know how to get the windows current theme border information
				// So, I just compare the current form size to the known AC client window sizes

				RECT rect = new RECT();

				GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

				int width = rect.Right - rect.Left;

				// Widths: 800 1024 1152 1280 1360 1400 1440 1600 1680 1792 1800 1856 1920 2048 2560
				if (width >= 2560) return width - 2560;
				if (width >= 2048) return width - 2048;
				if (width >= 1920) return width - 1920;
				if (width >= 1856) return width - 1856;
				if (width >= 1800) return width - 1800;
				if (width >= 1792) return width - 1792; // This would be broken for styles that have a border with >= 8, as it would be 1800+ then.
				if (width >= 1680) return width - 1680;
				if (width >= 1600) return width - 1600;
				if (width >= 1440) return width - 1440;
				if (width >= 1400) return width - 1400;
				if (width >= 1360) return width - 1360;
				if (width >= 1280) return width - 1280;
				if (width >= 1152) return width - 1152;
				if (width >= 1024) return width - 1024;
				if (width >= 800) return width - 800;

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

				RECT rect = new RECT();

				GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

				int height = rect.Bottom - rect.Top;

				// Workarounds go here:
				if (height == (1024 + 28)) return 28;

				// Heights: 600 720 768 800 864 900 960 1024 1050 1080 1200 1344 1392 1440 1536 1600
				if (height >= 1600) return height - 1600;
				if (height >= 1536) return height - 1536;
				if (height >= 1440) return height - 1440;
				if (height >= 1392) return height - 1392;
				if (height >= 1344) return height - 1344;
				if (height >= 1200) return height - 1200;
				if (height >= 1080) return height - 1080;
				if (height >= 1050) return height - 1050;
				if (height >= 1024) return height - 1024;
				if (height >= 960) return height - 960;
				if (height >= 900) return height - 900;
				if (height >= 864) return height - 864;
				if (height >= 800) return height - 800;
				if (height >= 768) return height - 768;
				if (height >= 720) return height - 720;
				if (height >= 600) return height - 600;

				// Windows 7 default: 28
				return 0;
			}
		}
	}
}
