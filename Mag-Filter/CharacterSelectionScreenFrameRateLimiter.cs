using System;
using System.Threading;

using MagFilter.Settings;

using Decal.Adapter;
using Mag.Shared;

namespace MagFilter
{
	class CharacterSelectionScreenFrameRateLimiter
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
				if (SettingsManager.CharacterSelectionScreen.MaximumFPS.Value != 0 && (CoreManager.Current.CharacterFilter == null || CoreManager.Current.CharacterFilter.Id == 0))
				{
					var lastFrameElapsedTime = stopWatch.Elapsed;

					var targetFrameSpacing = (double)1000 / SettingsManager.CharacterSelectionScreen.MaximumFPS.Value;

					if (lastFrameElapsedTime.TotalMilliseconds < targetFrameSpacing)
						Thread.Sleep((int)(targetFrameSpacing - lastFrameElapsedTime.TotalMilliseconds));

					stopWatch.Reset();
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
					SettingsManager.CharacterSelectionScreen.MaximumFPS.Value = value;
					Debug.WriteToChat("Character Selection Screen Maximum FPS set: " + e.Text);
				}

				e.Eat = true;
			}
		}
	}
}
