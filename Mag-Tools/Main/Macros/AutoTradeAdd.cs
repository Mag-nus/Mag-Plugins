﻿using System;
using System.IO;
using System.Collections.ObjectModel;

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
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_EndTrade(object sender, EndTradeEventArgs e)
		{
			try
			{
				Stop();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}


		bool started;

		private readonly VTClassic.LootCore lootCore = new VTClassic.LootCore();

		bool idsRequested;

		readonly Collection<int> itemIdsAdded = new Collection<int>();

		public void Start(FileInfo lootProfile)
		{
			if (started)
				return;

			if (!lootProfile.Exists)
				return;

			// Load our loot profile
			lootCore.LoadProfile(lootProfile.FullName, false);

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

			lootCore.UnloadProfile();

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
						//Debug.WriteToChat("AutoTradeAdd.Think(), itemInfo == null for " + item.Name);

						continue;
					}

					if (lootCore.DoesPotentialItemNeedID(itemInfo))
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
					//Debug.WriteToChat("AutoTradeAdd.Think(), itemInfo == null for " + item.Name);

					continue;
				}

				if (!lootCore.DoesPotentialItemNeedID(itemInfo))
				{
					uTank2.LootPlugins.LootAction result = lootCore.GetLootDecision(itemInfo);

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

			CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Auto Add To Trade - Inventory scan complete.", 5);

			Stop();
		}
	}
}