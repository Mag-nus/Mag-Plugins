using System;
using System.Collections.ObjectModel;

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

		void CharacterFilter_Login(object sender, Decal.Adapter.Wrappers.LoginEventArgs e)
		{
			try
			{
				WindowPosition windowPosition;

				if (GetWindowPositionForThisClient(out windowPosition))
				{
					User32.RECT rect = new User32.RECT();

					User32.GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

					User32.MoveWindow(CoreManager.Current.Decal.Hwnd, windowPosition.X, windowPosition.Y, rect.Right - rect.Left, rect.Bottom - rect.Top, true);
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
			User32.RECT rect = new User32.RECT();

			User32.GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

			WindowPosition windowPosition = new WindowPosition(CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.AccountName, rect.Left, rect.Top);

			Settings.SettingsManager.Misc.SetWindowPosition(windowPosition);
		}

		public static void DeleteWindowPosition()
		{
			Settings.SettingsManager.Misc.DeleteWindowPosition(CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.AccountName);
		}
	}
}