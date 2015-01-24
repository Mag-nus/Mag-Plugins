using System;
using System.IO;
using System.Collections.ObjectModel;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Macros
{
	class AutoBuySell : IDisposable
	{
		public AutoBuySell()
		{
			try
			{
				CoreManager.Current.WorldFilter.ApproachVendor += new EventHandler<ApproachVendorEventArgs>(WorldFilter_ApproachVendor);
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
					CoreManager.Current.WorldFilter.ApproachVendor -= new EventHandler<ApproachVendorEventArgs>(WorldFilter_ApproachVendor);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		// We store our lootProfile as an object instead of a VTClassic.LootCore.
		// We do this so that if this object is instantiated before vtank's plugins are loaded, we don't throw a VTClassic.dll error.
		// By delaying the object initialization to use, we can make sure we're using the VTClassic.dll that Virindi Tank loads.
		private object lootProfile;

		/// <summary>
		/// Step 1.
		/// Approach the Vendor and see if we have a profile for him.
		/// If we do, we subscribe to the render frame event to capture the next AC frame.
		/// We do this so that all AC data from this frame is given a chance to propagate to other plugins: Virindi Item Tool
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void WorldFilter_ApproachVendor(object sender, ApproachVendorEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.AutoBuySell.Enabled.Value)
					return;

				if (VirindiItemTool.PluginCore.ActivityState != VirindiItemTool.PluginCore.ePluginActivityState.Idle)
					return;

				// Init our LootCore object at the very last minute (looks for VTClassic.dll if its not already loaded)
				if (lootProfile == null)
					lootProfile = new VTClassic.LootCore();
				else
					((VTClassic.LootCore)lootProfile).UnloadProfile();

				FileInfo fileInfo = new FileInfo(PluginCore.PluginPersonalFolder + @"\" + CoreManager.Current.WorldFilter[e.MerchantId].Name + ".utl");

				if (!fileInfo.Exists)
				{
					if (Settings.SettingsManager.AutoBuySell.TestMode.Value)
						Debug.WriteToChat("AutoBuySell.WorldFilter_ApproachVendor(), vendor profile file not found at: " + fileInfo.FullName);

					return;
				}

				// Load our loot profile
				((VTClassic.LootCore)lootProfile).LoadProfile(fileInfo.FullName, false);

				CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		/// <summary>
		/// Step 2.
		/// We've approached the vendor and waited a frame.
		/// Lets unsubscribe from the render frame and see if we can kick off a buy/sell process.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(Current_RenderFrame);

				if (Settings.SettingsManager.AutoBuySell.TestMode.Value)
					DoTestMode();
				else
					KickOffBuySell();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		/// <summary>
		/// Step 3.
		/// If the virindi item tool is an idle state, lets see if we have an open vendor.
		/// If we do, lets see if we have items to buy and sell.
		/// If we do, lets ue Virindi Item Tool to begin a buy/sell process, but we also subscribe to the item tools state change event.
		/// We will wait for the item tool to return to an idle state and will try to kick off another buy/sell process.
		/// </summary>
		private void KickOffBuySell()
		{
			if (!Settings.SettingsManager.AutoBuySell.Enabled.Value)
				return;

			if (VirindiItemTool.PluginCore.ActivityState != VirindiItemTool.PluginCore.ePluginActivityState.Idle)
				return;

			using (Vendor openVendor = CoreManager.Current.WorldFilter.OpenVendor)
			{
				if (openVendor == null || openVendor.MerchantId == 0)
					return;

				int buyAmount;
				WorldObject buyItem = GetBuyItem(((VTClassic.LootCore)lootProfile), openVendor, out buyAmount);
				WorldObject sellItem = GetSellItem(((VTClassic.LootCore)lootProfile));

				if (buyItem != null && sellItem != null && (buyItem.ObjectClass != ObjectClass.TradeNote || sellItem.ObjectClass != ObjectClass.TradeNote))
				{
					VirindiItemTool.PluginCore.ActivityStateChanged += new VirindiItemTool.PluginCore.delActivityStateChanged(PluginCore_ActivityStateChanged);
					VirindiItemTool.PluginCore.BeginSellBuy(sellItem.Name, buyItem.Name, buyAmount);
				}
			}
		}

		/// <summary>
		/// Step 4.
		/// We wait for the item tool to return to an idle state.
		/// When it does, we unsubscribe from the activity changed event and kick off another buy/sell process.
		/// </summary>
		/// <param name="newstate"></param>
		void PluginCore_ActivityStateChanged(VirindiItemTool.PluginCore.ePluginActivityState newstate)
		{
			try
			{
				if (newstate == VirindiItemTool.PluginCore.ePluginActivityState.Idle)
				{
					VirindiItemTool.PluginCore.ActivityStateChanged -= new VirindiItemTool.PluginCore.delActivityStateChanged(PluginCore_ActivityStateChanged);

					KickOffBuySell();
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		/// <summary>
		/// This will return an object to buy. Keep # rules are returned first, then Keep rules.
		/// </summary>
		/// <param name="looter"></param>
		/// <param name="openVendor"></param>
		/// <param name="buyAmount"></param>
		/// <returns></returns>
		private WorldObject GetBuyItem(VTClassic.LootCore looter, Vendor openVendor, out int buyAmount)
		{
			// See if we can find a Keep # rule first.
			foreach (WorldObject vendorObj in openVendor)
			{
				// Convert the vendor item into a VT GameItemInfo object
				uTank2.LootPlugins.GameItemInfo itemInfo = uTank2.PluginCore.PC.FWorldTracker_GetWithVendorObjectTemplateID(vendorObj.Id);

				if (itemInfo == null)
				{
					Debug.WriteToChat("AutoBuySell.GetBuyItem(), itemInfo == null for " + vendorObj.Name);
					continue;
				}

				// Get the loot profile result for this object
				// result.IsNoLoot will always be false so we must check the Keep # against items in inventory.
				// The keep # is returned as Data1
				uTank2.LootPlugins.LootAction result = looter.GetLootDecision(itemInfo);

				if (!result.IsKeepUpTo)
					continue;

				// Find out how many of this item we have in our inventory
				int currentAmountInInventory = 0;

				foreach (WorldObject playerObj in CoreManager.Current.WorldFilter.GetInventory())
				{
					if (vendorObj.Name == playerObj.Name)
					{
						if (playerObj.Values(LongValueKey.StackCount) == 0)
							currentAmountInInventory++;
						else
							currentAmountInInventory += playerObj.Values(LongValueKey.StackCount);
					}
				}

				// If we have more than our Keep #, lets continue through the rest of the vendors item
				if (currentAmountInInventory >= result.Data1)
					continue;

				// Ok, we need to buy some of this item, how many should we buy?
				buyAmount = result.Data1 - currentAmountInInventory;

				// We can't add more than 5000 of any one item to the vendor buy pane.
				if (buyAmount > 5000)
					buyAmount = 5000;

				return vendorObj;
			}

			// Ok, we didn't find a Keep # rule, lets search all Keep rules now.
			foreach (WorldObject vendorObj in openVendor)
			{
				// Convert the vendor item into a VT GameItemInfo object
				uTank2.LootPlugins.GameItemInfo itemInfo = uTank2.PluginCore.PC.FWorldTracker_GetWithVendorObjectTemplateID(vendorObj.Id);

				if (itemInfo == null)
				{
					Debug.WriteToChat("AutoBuySell.GetBuyItem(), itemInfo == null for " + vendorObj.Name);
					continue;
				}

				// Get the loot profile result for this object
				// result.IsNoLoot will always be false
				uTank2.LootPlugins.LootAction result = looter.GetLootDecision(itemInfo);

				if (!result.IsKeep)
					continue;

				// Ok, we need to buy some of this item, how many should we buy?
				buyAmount = 5000;

				return vendorObj;
			}

			buyAmount = 0;

			return null;
		}

		/// <summary>
		/// This will return an item to sell in the following Priority: Any Non SpellComponent (Pea)/TradeNote, SpellComponent (Peas), TradeNote
		/// </summary>
		/// <param name="looter"></param>
		/// <returns></returns>
		private WorldObject GetSellItem(VTClassic.LootCore looter)
		{
			Collection<WorldObject> sellObjects = new Collection<WorldObject>();

			foreach (WorldObject playerObj in CoreManager.Current.WorldFilter.GetInventory())
			{
				// Safety check to prevent equipped items from being sold.
				if (playerObj.Values(LongValueKey.EquipableSlots, 0) > 0)
					continue;

				// Convert the vendor item into a VT GameItemInfo object
				uTank2.LootPlugins.GameItemInfo itemInfo = uTank2.PluginCore.PC.FWorldTracker_GetWithID(playerObj.Id);

				if (itemInfo == null)
				{
					Debug.WriteToChat("AutoBuySell.GetSellItem(), itemInfo == null for " + playerObj.Name);
					continue;
				}

				// Get the loot profile result for this object
				// result.IsNoLoot will always be false so we must check the Keep # against items in inventory.
				uTank2.LootPlugins.LootAction result = looter.GetLootDecision(itemInfo);

				if (!result.IsSell)
					continue;

				sellObjects.Add(playerObj);
			}

			if (sellObjects.Count == 0)
				return null;

			foreach (WorldObject sellObject in sellObjects)
			{
				if (sellObject.ObjectClass != ObjectClass.SpellComponent && sellObject.ObjectClass != ObjectClass.TradeNote)
					return sellObject;
			}

			WorldObject cheapest = null;

			foreach (WorldObject sellObject in sellObjects)
			{
				if (sellObject.ObjectClass != ObjectClass.TradeNote)
				{
					if (cheapest == null || cheapest.Values(LongValueKey.Value) > sellObject.Values(LongValueKey.Value))
						cheapest = sellObject;
				}
			}

			if (cheapest != null)
				return cheapest;

			foreach (WorldObject sellObject in sellObjects)
			{
				if (cheapest == null || cheapest.Values(LongValueKey.Value) > sellObject.Values(LongValueKey.Value))
					cheapest = sellObject;
			}

			return cheapest;
		}

		private void DoTestMode()
		{
			Debug.WriteToChat("Buy Items:");

			using (Vendor openVendor = CoreManager.Current.WorldFilter.OpenVendor)
			{
				foreach (WorldObject vendorObj in openVendor)
				{
					// Convert the vendor item into a VT GameItemInfo object
					uTank2.LootPlugins.GameItemInfo itemInfo = uTank2.PluginCore.PC.FWorldTracker_GetWithVendorObjectTemplateID(vendorObj.Id);

					if (itemInfo == null)
					{
						Debug.WriteToChat("AutoBuySell.DoTestMode(), itemInfo == null for " + vendorObj.Name);
						continue;
					}

					// Get the loot profile result for this object
					// result.IsNoLoot will always be false so we must check the Keep # against items in inventory.
					// The keep # is returned as Data1
					uTank2.LootPlugins.LootAction result = ((VTClassic.LootCore)lootProfile).GetLootDecision(itemInfo);

					if (result.IsKeepUpTo || result.IsKeep)
						Debug.WriteToChat(vendorObj.Name);
				}
			}

			Debug.WriteToChat("Sell Items:");

			foreach (WorldObject playerObj in CoreManager.Current.WorldFilter.GetInventory())
			{
				// Safety check to prevent equipped items from being sold.
				if (playerObj.Values(LongValueKey.EquipableSlots, 0) > 0)
					continue;

				// Convert the vendor item into a VT GameItemInfo object
				uTank2.LootPlugins.GameItemInfo itemInfo = uTank2.PluginCore.PC.FWorldTracker_GetWithID(playerObj.Id);

				if (itemInfo == null)
				{
					Debug.WriteToChat("AutoBuySell.DoTestMode(), itemInfo == null for " + playerObj.Name);
					continue;
				}

				// Get the loot profile result for this object
				// result.IsNoLoot will always be false so we must check the Keep # against items in inventory.
				uTank2.LootPlugins.LootAction result = ((VTClassic.LootCore)lootProfile).GetLootDecision(itemInfo);

				if (result.IsSell)
					Debug.WriteToChat(playerObj.Name);
			}
		}
	}
}
