using System;
using System.IO;
using System.Collections.ObjectModel;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Macros
{
	class InventoryPacker : IDisposable, IInventoryPacker
	{
		public InventoryPacker()
		{
			try
			{
				CoreManager.Current.ChatBoxMessage += new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
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

					CoreManager.Current.ChatBoxMessage -= new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void Current_ChatBoxMessage(object sender, ChatTextInterceptEventArgs e)
		{
			try
			{
				if (e.Text.StartsWith("You say, ") || e.Text.Contains("says, \""))
					return;

				if (e.Text.ToLower().Contains(CoreManager.Current.CharacterFilter.Name.ToLower() + " autopack"))
				{
					e.Eat = true;
					Start();
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		bool started;

		// We store our lootProfile as an object instead of a VTClassic.LootCore.
		// We do this so that if this object is instantiated before vtank's plugins are loaded, we don't throw a VTClassic.dll error.
		// By delaying the object initialization to use, we can make sure we're using the VTClassic.dll that Virindi Tank loads.
		private object lootProfile;

		bool idsRequested;

		readonly Collection<int> blackLitedItems = new Collection<int>();

		public void Start()
		{
			if (started)
				return;

			// Init our LootCore object at the very last minute (looks for VTClassic.dll if its not already loaded)
			if (lootProfile == null)
				lootProfile = new VTClassic.LootCore();

			FileInfo fileInfo = new FileInfo(PluginCore.PluginPersonalFolder + @"\" + CoreManager.Current.CharacterFilter.Name + ".AutoPack.utl");

			if (!fileInfo.Exists)
			{
				// Try to find a Default.AutoPack.utl
				fileInfo = new FileInfo(PluginCore.PluginPersonalFolder + @"\" + "Default.AutoPack.utl");

				if (!fileInfo.Exists)
					return;
			}

			CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Auto Pack - Started.", 5, Settings.SettingsManager.Misc.OutputTargetWindow.Value);

			// Load our loot profile
			((VTClassic.LootCore)lootProfile).LoadProfile(fileInfo.FullName, false);

			idsRequested = false;

			blackLitedItems.Clear();

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

			CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Auto Pack - Completed.", 5, Settings.SettingsManager.Misc.OutputTargetWindow.Value);
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

		int currentWorkingId;
		DateTime currentWorkingIdFirstAttempt;

		void Think()
		{
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
						CoreManager.Current.Actions.RequestId(item.Id);
				}

				idsRequested = true;
			}

			if (CoreManager.Current.Actions.BusyState != 0)
				return;

			if (DoAutoStack())
				return;

			if (DoAutoPack())
				return;

			// If we've gotten to this point no items were moved.
			Stop();
		}

		bool DoAutoStack()
		{
			foreach (WorldObject item in CoreManager.Current.WorldFilter.GetInventory())
			{
				if (blackLitedItems.Contains(item.Id))
					continue;

				// If the item is equipped or wielded, don't process it.
				if (item.Values(LongValueKey.EquippedSlots, 0) > 0 || item.Values(LongValueKey.Slot, -1) == -1)
					continue;

				// If the item isn't stackable, don't process it.
				if (item.Values(LongValueKey.StackMax, 0) <= 1)
					continue;

				// If the item is already max stack, don't process it
				if (item.Values(LongValueKey.StackCount, 0) == item.Values(LongValueKey.StackMax))
					continue;

				foreach (WorldObject secondItem in CoreManager.Current.WorldFilter.GetByContainer(item.Container))
				{
					if (item.Id == secondItem.Id || item.Name != secondItem.Name)
						continue;

					// If the item is already max stack, don't process it
					if (secondItem.Values(LongValueKey.StackCount, 0) == secondItem.Values(LongValueKey.StackMax))
						continue;

					if (currentWorkingId != item.Id)
					{
						currentWorkingId = item.Id;
						currentWorkingIdFirstAttempt = DateTime.Now;
					}
					else
					{
						if (DateTime.Now - currentWorkingIdFirstAttempt > TimeSpan.FromSeconds(10))
						{
							Debug.WriteToChat("Blacklisting item: " + item.Id + ", " + item.Name);
							blackLitedItems.Add(item.Id);
							return true;
						}
					}

					CoreManager.Current.Actions.MoveItem(item.Id, secondItem.Id);

					return true;
				}
			}

			return false;
		}

		private struct ItemToProcess
		{
			public ItemToProcess(int id, int[] targetPackIds)
			{
				Id = id;
				TargetPackIds = targetPackIds;
			}

			public readonly int Id;
			public readonly int[] TargetPackIds;
		}

		bool DoAutoPack()
		{
			bool waitingForIds = false;

			// Get all of our side pack information and put them in the correct order
			int[] packs = new int[CoreManager.Current.WorldFilter[CoreManager.Current.CharacterFilter.Id].Values(LongValueKey.PackSlots) + 1];

			// Main pack
			packs[0] = CoreManager.Current.CharacterFilter.Id;

			// Load the side pack information
			foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetByContainer(CoreManager.Current.CharacterFilter.Id))
			{
				if (obj.ObjectClass != ObjectClass.Container)
					continue;

				packs[obj.Values(LongValueKey.Slot) + 1] = obj.Id;
			}


			// Process our inventory
			Collection<ItemToProcess> itemsToProcess = new Collection<ItemToProcess>();

			foreach (WorldObject item in CoreManager.Current.WorldFilter.GetInventory())
			{
				if (blackLitedItems.Contains(item.Id))
					continue;

				// If the item is equipped or wielded, don't process it.
				if (item.Values(LongValueKey.EquippedSlots, 0) > 0 || item.Values(LongValueKey.Slot, -1) == -1)
					continue;

				// If the item is a container or a foci, don't process it.
				if (item.ObjectClass == ObjectClass.Container || item.ObjectClass == ObjectClass.Foci)
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
					waitingForIds = true;

					continue;
				}

				uTank2.LootPlugins.LootAction result = ((VTClassic.LootCore)lootProfile).GetLootDecision(itemInfo);

				if (!result.IsKeepUpTo)
					continue;

				// Separate the Data1 int result into a byte[]. Meaning, turn 123456 into { 1, 2, 3, 4, 5, 6 }
				char[] packNumbersAsChar = result.Data1.ToString().ToCharArray();
				byte[] packNumbers = new byte[packNumbersAsChar.Length];
				for (int i = 0 ; i < packNumbersAsChar.Length ; i++)
					packNumbers[i] = byte.Parse(packNumbersAsChar[i].ToString());

				// If this item is already in its primary pack, we don't need to queue it up.
				if (item.Container == packs[packNumbers[0]])
					continue;

				int[] targetPackIds = new int[packNumbers.Length];
				for (int i = 0 ; i < packNumbers.Length ; i++)
					targetPackIds[i] = packs[packNumbers[i]];

				ItemToProcess itemToProcess = new ItemToProcess(item.Id, targetPackIds);

				itemsToProcess.Add(itemToProcess);
			}

			// Lets go through our list and see if any items are in their primary target pack
			for (int i = itemsToProcess.Count - 1 ; i >= 0 ; i--)
			{
				ItemToProcess itemToProcess = itemsToProcess[i];

				WorldObject item = CoreManager.Current.WorldFilter[itemToProcess.Id];

				if (item == null)
				{
					itemsToProcess.RemoveAt(i);

					continue;
				}

				if (item.Container == itemToProcess.TargetPackIds[0])
					itemsToProcess.RemoveAt(i);
			}

			Collection<int> itemsToSkip = new Collection<int>();

			// Lets see if we can find an item that can be moved to its target pack
			for (int packIndex = 0 ; packIndex < 10 ; packIndex++)
			{
				for (int i = itemsToProcess.Count - 1 ; i >= 0 ; i--)
				{
					ItemToProcess itemToProcess = itemsToProcess[i];

					WorldObject item = CoreManager.Current.WorldFilter[itemToProcess.Id];

					if (itemToProcess.TargetPackIds.Length <= packIndex)
						continue;

					if (itemsToSkip.Contains(item.Id))
						continue;

					// Check to see if this item is already in the target pack
					if (item.Container == itemToProcess.TargetPackIds[packIndex])
					{
						itemsToSkip.Add(item.Id);

						continue;
					}

					// Check to see that the target is even a pack.
					WorldObject target = CoreManager.Current.WorldFilter[itemToProcess.TargetPackIds[packIndex]];

					if (target == null || (target.ObjectClass != ObjectClass.Container && target.ObjectClass != ObjectClass.Player))
						continue;

					if (Util.GetFreePackSlots(itemToProcess.TargetPackIds[packIndex]) > 0)
					{
						if (currentWorkingId != item.Id)
						{
							currentWorkingId = item.Id;
							currentWorkingIdFirstAttempt = DateTime.Now;
						}
						else
						{
							if (DateTime.Now - currentWorkingIdFirstAttempt > TimeSpan.FromSeconds(10))
							{
								Debug.WriteToChat("Blacklisting item: " + item.Id + ", " + item.Name);
								blackLitedItems.Add(item.Id);
								return true;
							}
						}

						CoreManager.Current.Actions.MoveItem(item.Id, itemToProcess.TargetPackIds[packIndex], 0, true);

						return true;
					}
				}
			}

			if (waitingForIds)
				return true;

			return false;
		}
	}
}
