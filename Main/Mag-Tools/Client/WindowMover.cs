using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

using Mag.Shared;

using Decal.Adapter;

namespace MagTools.Client
{
	class WindowMover : IDisposable
	{
		public WindowMover()
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

		struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
		static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

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

		void CharacterFilter_Login(object sender, Decal.Adapter.Wrappers.LoginEventArgs e)
		{
			try
			{
				WindowPosition windowPosition;

				if (GetWindowPositionForThisClient(out windowPosition))
				{
					RECT rect = new RECT();

					GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

					MoveWindow(CoreManager.Current.Decal.Hwnd, windowPosition.X, windowPosition.Y, rect.Right - rect.Left, rect.Bottom - rect.Top, true);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		public static bool GetWindowPositionForThisClient(out WindowPosition windowPosition)
		{
			Collection<WindowPosition> windowPositions = Settings.SettingsManager.Misc.WindowPositions;

			foreach (WindowPosition position in windowPositions)
			{
				if (position.Server == CoreManager.Current.CharacterFilter.Server && position.AccountName == CoreManager.Current.CharacterFilter.AccountName)
				{
					windowPosition = position;
					return true;
				}
			}

			windowPosition = new WindowPosition();
			return false;
		}

		public static void SetWindowPosition()
		{
			RECT rect = new RECT();

			GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

			WindowPosition windowPosition = new WindowPosition(CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.AccountName, rect.Left, rect.Top);

			Settings.SettingsManager.Misc.SetWindowPosition(windowPosition);
		}

		public static void DeleteWindowPosition()
		{
			Settings.SettingsManager.Misc.DeleteWindowPosition(CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.AccountName);
		}
	}
}