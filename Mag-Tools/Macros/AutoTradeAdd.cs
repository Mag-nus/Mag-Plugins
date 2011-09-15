using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Macros
{
	class AutoTradeAdd : IDisposable
	{
		public bool Enabled { private get; set; }

		System.Windows.Forms.Timer addTimer = new System.Windows.Forms.Timer();

		public AutoTradeAdd()
		{
			try
			{
				CoreManager.Current.WorldFilter.EnterTrade += new EventHandler<Decal.Adapter.Wrappers.EnterTradeEventArgs>(WorldFilter_EnterTrade);
				CoreManager.Current.WorldFilter.EndTrade += new EventHandler<Decal.Adapter.Wrappers.EndTradeEventArgs>(WorldFilter_EndTrade);
				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);

				addTimer.Tick += new EventHandler(addTimer_Tick);
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
					CoreManager.Current.WorldFilter.EnterTrade -= new EventHandler<Decal.Adapter.Wrappers.EnterTradeEventArgs>(WorldFilter_EnterTrade);
					CoreManager.Current.WorldFilter.EndTrade -= new EventHandler<Decal.Adapter.Wrappers.EndTradeEventArgs>(WorldFilter_EndTrade);
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);

					addTimer.Tick -= new EventHandler(addTimer_Tick);
					addTimer.Dispose();
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}

		private int lastTraderId = 0;

		private VTClassic.LootCore lootProfile = new VTClassic.LootCore();

		Queue<int> itemQueue = new Queue<int>();
		Collection<int> itemsWaitingForId = new Collection<int>();

		void WorldFilter_EnterTrade(object sender, Decal.Adapter.Wrappers.EnterTradeEventArgs e)
		{
			try
			{
				if (!Enabled)
					return;

				addTimer.Stop();
				itemQueue.Clear();
				itemsWaitingForId.Clear();

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

				if (lastTraderId != traderId)
				{
					if (lastTraderId != 0)
						lootProfile.UnloadProfile();

					lastTraderId = 0;

					FileInfo fileInfo = new FileInfo(PluginCore.PluginPersonalFolder + @"\" + CoreManager.Current.WorldFilter[traderId].Name + ".utl");

					if (!fileInfo.Exists)
						return;

					// Load our loot profile
					lootProfile.LoadProfile(fileInfo.FullName, false);

					lastTraderId = traderId;
				}

				ProcessInventory();

				addTimer.Interval = 200;
				addTimer.Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_EndTrade(object sender, Decal.Adapter.Wrappers.EndTradeEventArgs e)
		{
			try
			{
				addTimer.Stop();
				itemQueue.Clear();
				itemsWaitingForId.Clear();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private void ProcessInventory()
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
					Debug.WriteToChat("AutoTradeAdd.ProcessInventory(), itemInfo == null for " + item.Name);
					continue;
				}

				if (lootProfile.DoesPotentialItemNeedID(itemInfo))
				{
					itemsWaitingForId.Add(item.Id);
					CoreManager.Current.Actions.RequestId(item.Id);
				}
				else
				{
					uTank2.LootPlugins.LootAction result = lootProfile.GetLootDecision(itemInfo);

					processVTankIdentLootAction(item.Id, result);
				}

				// Can't trade more than 102 items so don't bother.
				if (itemQueue.Count >= 102)
					break;
			}
		}

		void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
		{
			try
			{
				if (itemsWaitingForId.Count == 0 || e.Change != WorldChangeType.IdentReceived || !itemsWaitingForId.Contains(e.Changed.Id))
					return;

				itemsWaitingForId.Remove(e.Changed.Id);

				// Convert the item into a VT GameItemInfo object
				uTank2.LootPlugins.GameItemInfo itemInfo = uTank2.PluginCore.PC.FWorldTracker_GetWithID(e.Changed.Id);

				if (itemInfo == null)
				{
					Debug.WriteToChat("AutoTradeAdd.ProcessInventory(), itemInfo == null for " + e.Changed.Name);
					return;
				}

				uTank2.LootPlugins.LootAction result = lootProfile.GetLootDecision(itemInfo);

				processVTankIdentLootAction(e.Changed.Id, result);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private void processVTankIdentLootAction(int itemId, uTank2.LootPlugins.LootAction result)
		{
			if (!result.IsKeep)
				return;

			itemQueue.Enqueue(itemId);
		}

		void addTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (itemQueue.Count == 0 && itemsWaitingForId.Count == 0)
				{
					addTimer.Stop();
					return;
				}

				int item = itemQueue.Dequeue();

				// Should check here to see if the trade window has >= 102 items added.

				CoreManager.Current.Actions.TradeAdd(item);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
