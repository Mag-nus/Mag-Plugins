using System;
using System.Threading;

using Mag.Shared;

using Decal.Adapter;

namespace MagTools.Client
{
	class NoFocusFPSManager : IDisposable
	{
		public NoFocusFPSManager()
		{
			try
			{
				CoreManager.Current.WindowMessage += new EventHandler<WindowMessageEventArgs>(Current_WindowMessage);
				CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);
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
					CoreManager.Current.WindowMessage -= new EventHandler<WindowMessageEventArgs>(Current_WindowMessage);
					CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(Current_RenderFrame);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void Current_WindowMessage(object sender, WindowMessageEventArgs e)
		{
			try
			{
				if (Settings.SettingsManager.Misc.NoFocusFPS.Value == Settings.SettingsManager.Misc.NoFocusFPS.DefaultValue)
				{
					Throttling = false;
					return;
				}

				if (e.Msg == User32.WM_KILLFOCUS)
					Throttling = true;

				if (e.Msg == 7 && e.LParam == 0 && e.WParam == 0)
					Throttling = false;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		readonly System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

		void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				if (!Throttling)
					return;

				if (!stopWatch.IsRunning)
					return;

				if (stopWatch.ElapsedMilliseconds < 1000 / Settings.SettingsManager.Misc.NoFocusFPS.Value)
				{
					int msToSleep = (int)((1000 / Settings.SettingsManager.Misc.NoFocusFPS.Value) - stopWatch.ElapsedMilliseconds);
					if (msToSleep > 100) 
						msToSleep = 100;
					if (msToSleep > 0)
						Thread.Sleep(msToSleep);
				}

				stopWatch.Reset();
				stopWatch.Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		int debugMessagesPrinted;

		bool _throttling;
		bool Throttling
		{
			get
			{
				return _throttling;
			}
			set
			{
				if (_throttling == value)
					return;

				_throttling = value;

				if (Settings.SettingsManager.Misc.DebuggingEnabled.Value && debugMessagesPrinted < 2)
				{
					if (_throttling)
						Debug.WriteToChat("NoFocusFPSManager throttling set to " + _throttling + " with a target FPS of " + Settings.SettingsManager.Misc.NoFocusFPS.Value);
					else
						Debug.WriteToChat("NoFocusFPSManager throttling set to " + _throttling);
					debugMessagesPrinted++;
					if (debugMessagesPrinted >= 2)
						Debug.WriteToChat("NoFocusFPSManager: no more debug messages will be printed this session.");
				}

				if (_throttling)
				{
					User32.PostMessage(CoreManager.Current.Decal.Hwnd, User32.WM_ACTIVATEAPP, (IntPtr)1, UIntPtr.Zero);
					User32.PostMessage(CoreManager.Current.Decal.Hwnd, User32.WM_ACTIVATE, (IntPtr)2, UIntPtr.Zero);

					stopWatch.Start();
				}
				else
				{
					stopWatch.Stop();
				}
			}
		}
	}
}
