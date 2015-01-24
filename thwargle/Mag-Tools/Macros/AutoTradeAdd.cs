using System;
using System.IO;
using System.Collections.ObjectModel;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Macros
{
	class AutoTradeAdd : IDisposable
	{
		public AutoTradeAdd()
		{
			try
			{
				CoreManager.Current.WorldFilter.EnterTrade += new EventHandler<EnterTradeEventArgs>(WorldFilter_EnterTrade);
				CoreManager.Current.WorldFilter.EndTrade += new EventHandler<EndTradeEventArgs>(WorldFilter_EndTrade);
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
					if (started)
						Stop();

					CoreManager.Current.WorldFilter.EnterTrade -= new EventHandler<EnterTradeEventArgs>(WorldFilter_EnterTrade);
					CoreManager.Current.WorldFilter.EndTrade -= new EventHandler<EndTradeEventArgs>(WorldFilter_EndTrade);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}


		void WorldFilter_EnterTrade(object sender, EnterTradeEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.AutoTradeAdd.Enabled.Value)
					return;

				int traderId = 0;

				// This is a little trick.
				// When someone initiates the trade, they will have both the trader and tradee ID's be accurate.
				// When someone initiates a trade with you, you will have them as both ID's.
				// This will prevent our mule from auto-muling back to us.
				if (e.TradeeId == CoreManager.Current.CharacterFilter.Id)
					traderId = e.TraderId;
				else if (e.TraderId == CoreManager.Current.CharacterFilter.Id)
					traderId = e.TradeeId;

				if (traderId == 0)
					return;

				FileInfo fileInfo = new FileInfo(PluginCore.PluginPersonalFolder + @"\" + CoreManager.Current.WorldFilter[traderId].Name + ".utl");

				if (!fileInfo.Exists)
					return;

				Start(fileInfo);
			}
			catch (FileNotFoundException) { CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Unable to start Auto Add to Trade. Is Virindi Tank running?", 5); }
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_EndTrade(object sender, EndTradeEventArgs e)
		{
			try
			{
				if (started)
					Stop();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}


		bool started;

		// We store our lootProfile as an object instead of a VTClassic.LootCore.
		// We do this so that if this object is instantiated before vtank's plugins are loaded, we don't throw a VTClassic.dll error.
		// By delaying the object initialization to use, we can make sure we're using the VTClassic.dll that Virindi Tank loads.
		private object lootProfile;

		bool idsRequested;

		readonly Collection<int> itemIdsAdded = new Collection<int>();

		public void Start(FileInfo lootProfileFileInfo)
		{
			if (started)
				return;

			if (!lootProfileFileInfo.Exists)
				return;

			// Init our LootCore object at the very last minute (looks for VTClassic.dll if its not already loaded)
			if (lootProfile == null)
				lootProfile = new VTClassic.LootCore();

			// Load our loot profile
			((VTClassic.LootCore)lootProfile).LoadProfile(lootProfileFileInfo.FullName, false);

			idsRequested = false;
			itemIdsAdded.Clear();

			CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);

			started = true;
		}

		public void Stop()
		{
			if (!started)
				return;

			CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(Current_RenderFrame);

			((VTClassic.LootCore)lootProfile).UnloadProfile();

			started = false;
		}

		DateTime lastThought = DateTime.MinValue;

		void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				if (DateTime.Now - lastThought < TimeSpan.FromMilliseconds(100))
					return;

				lastThought = DateTime.Now;

				Think();

			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Think()
		{
			bool waitingForIds = false;

			if (!idsRequested)
			{
				foreach (WorldObject item in CoreManager.Current.WorldFilter.GetInventory())
				{
					// If the item is equipped or wielded, don't process it.
					if (item.Values(LongValueKey.EquippedSlots, 0) > 0 || item.Values(LongValueKey.Slot, -1) == -1)
						continue;

					// Convert the item into a VT GameItemInfo object
					uTank2.LootPlugins.GameItemInfo itemInfo = uTank2.PluginCore.PC.FWorldTracker_GetWithID(item.Id);

					if (itemInfo == null)
					{
						// This happens all the time for aetheria that has been converted
						continue;
					}

					if (((VTClassic.LootCore)lootProfile).DoesPotentialItemNeedID(itemInfo))
					{
						CoreManager.Current.Actions.RequestId(item.Id);

						waitingForIds = true;
					}
				}

				idsRequested = true;
			}

			foreach (WorldObject item in CoreManager.Current.WorldFilter.GetInventory())
			{
				// If the item is equipped or wielded, don't process it.
				if (item.Values(LongValueKey.EquippedSlots, 0) > 0 || item.Values(LongValueKey.Slot, -1) == -1)
					continue;

				if (itemIdsAdded.Contains(item.Id))
					continue;

				// Convert the item into a VT GameItemInfo object
				uTank2.LootPlugins.GameItemInfo itemInfo = uTank2.PluginCore.PC.FWorldTracker_GetWithID(item.Id);

				if (itemInfo == null)
				{
					// This happens all the time for aetheria that has been converted
					continue;
				}

				if (!((VTClassic.LootCore)lootProfile).DoesPotentialItemNeedID(itemInfo))
				{
					uTank2.LootPlugins.LootAction result = ((VTClassic.LootCore)lootProfile).GetLootDecision(itemInfo);

					if (!result.IsKeep)
						continue;

					itemIdsAdded.Add(item.Id);

					CoreManager.Current.Actions.TradeAdd(item.Id);

					return;
				}

				waitingForIds = true;
			}

			if (waitingForIds)
				return;

			CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Auto Add To Trade - Inventory scan complete.", 5, Settings.SettingsManager.Misc.OutputTargetWindow.Value);

			Stop();
		}
	}
}
