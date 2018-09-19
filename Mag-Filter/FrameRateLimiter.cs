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

		private void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				if (CoreManager.Current.CharacterFilter == null || CoreManager.Current.CharacterFilter.Id == 0)
				{
					if (SettingsManager.FrameRateLimiters.CharacterSelectionScreen.Value != 0)
					{
						var lastFrameElapsedTime = stopWatch.Elapsed;

						var targetFrameSpacing = (double) 1000 / SettingsManager.FrameRateLimiters.CharacterSelectionScreen.Value;

						if (lastFrameElapsedTime.TotalMilliseconds < targetFrameSpacing)
							Thread.Sleep((int) (targetFrameSpacing - lastFrameElapsedTime.TotalMilliseconds));

						stopWatch.Reset();
					}
				}
				else
				{
					if (SettingsManager.FrameRateLimiters.InWorld.Value != 0)
					{
						var lastFrameElapsedTime = stopWatch.Elapsed;

						var targetFrameSpacing = (double)1000 / SettingsManager.FrameRateLimiters.InWorld.Value;

						if (lastFrameElapsedTime.TotalMilliseconds < targetFrameSpacing)
							Thread.Sleep((int)(targetFrameSpacing - lastFrameElapsedTime.TotalMilliseconds));

						stopWatch.Reset();
					}
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

				if (value > 0 && value < 10)
					Debug.WriteToChat("Character Selection Screen Maximum FPS cannot be less than 10. Set to zero to disable.");
				else
				{
					SettingsManager.FrameRateLimiters.CharacterSelectionScreen.Value = value;
					Debug.WriteToChat("Character Selection Screen Maximum FPS set: " + value);
				}

				e.Eat = true;
			}
			else if (e.Text.StartsWith("/mf iwmfps "))
			{
				var value = uint.Parse(e.Text.Substring(11, e.Text.Length - 11));

				if (value > 0 && value < 20)
					Debug.WriteToChat("In-World Maximum FPS cannot be less than 20. Set to zero to disable.");
				else
				{
					SettingsManager.FrameRateLimiters.InWorld.Value = value;
					Debug.WriteToChat("In-World Maximum FPS set: " + value);
				}

				e.Eat = true;
			}
		}
	}
}
