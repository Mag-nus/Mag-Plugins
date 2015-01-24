using System;
using System.Text.RegularExpressions;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Macros
{
	class AutoTradeAccept : IDisposable
	{
		public AutoTradeAccept()
		{
			try
			{
				CoreManager.Current.WorldFilter.AcceptTrade += new EventHandler<AcceptTradeEventArgs>(WorldFilter_AcceptTrade);
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
					CoreManager.Current.WorldFilter.AcceptTrade -= new EventHandler<AcceptTradeEventArgs>(WorldFilter_AcceptTrade);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		DateTime lastTradeAcceptTime = DateTime.MinValue;

		void WorldFilter_AcceptTrade(object sender, AcceptTradeEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.AutoTradeAccept.Enabled.Value)
					return;

				// We only auto accept the trade every 2 seconds to avoid double spamming it from our own TradeAccept() action.
				// This also prevents us from respamming TradeAccept() on the char where the user accepted the trade.
				if (DateTime.Now - lastTradeAcceptTime < TimeSpan.FromSeconds(2))
					return;

				lastTradeAcceptTime = DateTime.Now;

				if (e.TargetId == CoreManager.Current.CharacterFilter.Id)
					return;

				// See if this target is in our white list
				WorldObject target = CoreManager.Current.WorldFilter[e.TargetId];

				if (target == null)
					return;

				foreach (Regex regex in Settings.SettingsManager.AutoTradeAccept.Whitelist)
				{
					if (regex.IsMatch(target.Name))
					{
						CoreManager.Current.Actions.TradeAccept();
						break;
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
