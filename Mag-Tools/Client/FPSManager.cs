using System;
using System.Threading;

using Mag.Shared;

using Decal.Adapter;

namespace MagTools.Client
{
	class FPSManager : IDisposable
	{
		public FPSManager()
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
				if (Settings.SettingsManager.Misc.NoFocusFPS.Value <= Settings.SettingsManager.Misc.NoFocusFPS.DefaultValue)
				{
					ToggleNonFocusFPSBoost(false);
					return;
				}

				if (e.Msg == User32.WM_KILLFOCUS)
					ToggleNonFocusFPSBoost(true);
				else if (e.Msg == User32.WM_SETFOCUS && e.LParam == 0 && e.WParam == 0)
					ToggleNonFocusFPSBoost(false);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		RateLimiter nonFocusRateLimiter;
		RateLimiter maxFPSRateLimiter;

		void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				if (nonFocusRateLimiter != null)
				{
					var sleepTime = (int)(nonFocusRateLimiter.GetSecondsToWaitBeforeNextEvent() * 1000);

					if (sleepTime > 0)
						Thread.Sleep(sleepTime);

					nonFocusRateLimiter.RegisterEvent();
				}

				if (maxFPSRateLimiter == null)
				{
					if (Settings.SettingsManager.Misc.MaxFPS.Value >= 20)
						maxFPSRateLimiter = new RateLimiter(Settings.SettingsManager.Misc.MaxFPS.Value, TimeSpan.FromSeconds(1));
				}
				else
				{
					if (Settings.SettingsManager.Misc.MaxFPS.Value >= 20)
					{
						if (maxFPSRateLimiter.MaxNumberOfEvents != Settings.SettingsManager.Misc.MaxFPS.Value)
							maxFPSRateLimiter = new RateLimiter(Settings.SettingsManager.Misc.MaxFPS.Value, TimeSpan.FromSeconds(1));
					}
					else
						maxFPSRateLimiter = null;
				}

				if (maxFPSRateLimiter != null)
				{
					var sleepTime = (int)(maxFPSRateLimiter.GetSecondsToWaitBeforeNextEvent() * 1000);

					if (sleepTime > 0)
						Thread.Sleep(sleepTime);

					maxFPSRateLimiter.RegisterEvent();
				}

			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		int nonFocusDebugMessagesPrinted;

		void ToggleNonFocusFPSBoost(bool enabled)
		{
			if (enabled == (nonFocusRateLimiter != null))
				return;

			if (Settings.SettingsManager.Misc.NoFocusFPS.Value < 10)
				enabled = false;

			if (Settings.SettingsManager.Misc.DebuggingEnabled.Value && nonFocusDebugMessagesPrinted < 2)
			{
				if (enabled)
					Debug.WriteToChat("NoFocusFPSManager throttling set to " + enabled + " with a target FPS of " + Settings.SettingsManager.Misc.NoFocusFPS.Value);
				else
					Debug.WriteToChat("NoFocusFPSManager throttling set to " + enabled);
				nonFocusDebugMessagesPrinted++;
				if (nonFocusDebugMessagesPrinted >= 2)
					Debug.WriteToChat("NoFocusFPSManager: no more debug messages will be printed this session.");
			}

			if (enabled)
			{
				nonFocusRateLimiter = new RateLimiter(Settings.SettingsManager.Misc.NoFocusFPS.Value, TimeSpan.FromSeconds(1));

				User32.PostMessage(CoreManager.Current.Decal.Hwnd, User32.WM_ACTIVATEAPP,	(IntPtr)1, UIntPtr.Zero);
				User32.PostMessage(CoreManager.Current.Decal.Hwnd, User32.WM_ACTIVATE,		(IntPtr)2, UIntPtr.Zero);
			}
			else
			{
				nonFocusRateLimiter = null;
			}
		}
	}
}
