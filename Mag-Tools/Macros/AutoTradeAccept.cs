using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Macros
{
	class AutoTradeAccept : IDisposable
	{
		public bool Enabled { private get; set; }

		public AutoTradeAccept()
		{
			try
			{
				CoreManager.Current.WorldFilter.AcceptTrade += new EventHandler<Decal.Adapter.Wrappers.AcceptTradeEventArgs>(WorldFilter_AcceptTrade);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private bool _disposed = false;

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
			if (!_disposed)
			{
				if (disposing)
				{
					CoreManager.Current.WorldFilter.AcceptTrade -= new EventHandler<Decal.Adapter.Wrappers.AcceptTradeEventArgs>(WorldFilter_AcceptTrade);
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}

		DateTime lastTradeAcceptTime = DateTime.MinValue;

		void WorldFilter_AcceptTrade(object sender, Decal.Adapter.Wrappers.AcceptTradeEventArgs e)
		{
			try
			{
				if (!Enabled)
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

				foreach (Regex regex in whitelist)
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

		Collection<Regex> whitelist = new Collection<Regex>();

		/// <summary>
		/// Adds a regex pattern used to compare against the traders name.
		/// </summary>
		/// <param name="regex"></param>
		public void AddToWhitelist(Regex regex)
		{
			whitelist.Add(regex);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="regex"></param>
		public void RemoveFromWhitelist(Regex regex)
		{
			whitelist.Remove(regex);
		}

		public void ClearWhitelist()
		{
			whitelist.Clear();
		}
	}
}
