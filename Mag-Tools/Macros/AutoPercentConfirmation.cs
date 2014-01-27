using System;
using System.Text.RegularExpressions;

using Decal.Adapter;

using Mag.Shared;

namespace MagTools.Macros
{
	class AutoPercentConfirmation : IDisposable
	{
		public AutoPercentConfirmation()
		{
			try
			{
				CoreManager.Current.EchoFilter.ServerDispatch += new EventHandler<NetworkMessageEventArgs>(EchoFilter_ServerDispatch);
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
					CoreManager.Current.EchoFilter.ServerDispatch -= new EventHandler<NetworkMessageEventArgs>(EchoFilter_ServerDispatch);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		private static readonly byte MinimumYesPercent = 100;

		// You determine that you have a 100 percent chance to succeed.
		private static readonly Regex PercentConfirmation = new Regex("^You determine that you have a (?<percent>.+) percent chance to succeed.$");

		void EchoFilter_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.Tinkering.AutoClickYes.Value)
					return;

				if (e.Message.Type == 0xF7B0 && (int)e.Message["event"] == 0x0274 && e.Message.Value<int>("type") == 5)
				{
					Match match = PercentConfirmation.Match(e.Message.Value<string>("text"));

					if (match.Success)
					{
						int percent;

						if (int.TryParse(match.Groups["percent"].Value, out percent) && percent >= MinimumYesPercent)
							PostMessageTools.ClickYes();
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
