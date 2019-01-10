using System;
using System.Threading;

using MagFilter.Settings;
using Mag.Shared;

using Decal.Adapter;

namespace MagFilter
{
	class FrameRateLimiter
	{
		readonly System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();

		public void Startup()
		{
			CoreManager.Current.RenderFrame += Current_RenderFrame;
		}

		public void Shutdown()
		{
			CoreManager.Current.RenderFrame -= Current_RenderFrame;
		}

		RateLimiter characterSelectionScreenRateLimiter;

		private void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				if (CoreManager.Current.CharacterFilter == null || CoreManager.Current.CharacterFilter.Id == 0)
				{
					if (SettingsManager.CharacterSelectionScreen.MaxFPS.Value >= 10)
					{
						if (characterSelectionScreenRateLimiter == null || characterSelectionScreenRateLimiter.MaxNumberOfEvents != SettingsManager.CharacterSelectionScreen.MaxFPS.Value)
							characterSelectionScreenRateLimiter = new RateLimiter((int)SettingsManager.CharacterSelectionScreen.MaxFPS.Value, TimeSpan.FromSeconds(1));
					}
					else
						characterSelectionScreenRateLimiter = null;
				}
				else
					characterSelectionScreenRateLimiter = null;

				if (characterSelectionScreenRateLimiter != null)
				{
					var sleepTime = (int)(characterSelectionScreenRateLimiter.GetSecondsToWaitBeforeNextEvent() * 1000);

					if (sleepTime > 0)
						Thread.Sleep(sleepTime);

					characterSelectionScreenRateLimiter.RegisterEvent();
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);

				// If we fail once, just stop.
				CoreManager.Current.RenderFrame -= Current_RenderFrame;
			}
		}

		public void FilterCore_CommandLineText(object sender, ChatParserInterceptEventArgs e)
		{
			if (e.Text.StartsWith("/mf cssmfps "))
			{
				var value = uint.Parse(e.Text.Substring(12, e.Text.Length - 12));

				if (value != 0 && value < 10)
					Debug.WriteToChat("Character Selection Screen Maximum FPS cannot be less than 10. Set to zero to disable.");
				else
				{
					SettingsManager.CharacterSelectionScreen.MaxFPS.Value = value;
					Debug.WriteToChat("Character Selection Screen Maximum FPS set: " + value);
				}

				e.Eat = true;
			}
		}
	}
}
