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
						LimitFPS(SettingsManager.FrameRateLimiters.CharacterSelectionScreen.Value);
				}
				else
				{
					if (SettingsManager.FrameRateLimiters.InWorld.Value != 0)
						LimitFPS(SettingsManager.FrameRateLimiters.InWorld.Value);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);

				// If we fail once, just stop.
				CoreManager.Current.RenderFrame -= Current_RenderFrame;
			}
		}

		private uint lastMaxFPS;
		private double targetFrameSpacing;
		private int frameCount;

		private void LimitFPS(uint maxFPS)
		{
			if (lastMaxFPS != maxFPS)
			{
				lastMaxFPS = maxFPS;
				targetFrameSpacing = (double)1000 / maxFPS;
				frameCount = 1;

				stopWatch.Reset();
				stopWatch.Start();

				return;
			}

			var elapsedMilliseconds = stopWatch.ElapsedMilliseconds;

			var sleepTime = (int)((targetFrameSpacing * frameCount) - elapsedMilliseconds);

			if (sleepTime > 0)
				Thread.Sleep(sleepTime);

			if (++frameCount > maxFPS || elapsedMilliseconds > 1000)
			{
				frameCount = 1;

				stopWatch.Reset();
				stopWatch.Start();
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
