using System;
using System.IO;
using System.Collections.ObjectModel;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Macros
{
	class AutoPack : IDisposable
	{
		public bool Enabled { private get; set; }

		System.Windows.Forms.Timer packTimer = new System.Windows.Forms.Timer();

		public AutoPack()
		{
			try
			{
				CoreManager.Current.ChatBoxMessage += new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);

				CoreManager.Current.WorldFilter.CreateObject += new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<Decal.Adapter.Wrappers.ChangeObjectEventArgs>(WorldFilter_ChangeObject);

				packTimer.Tick += new EventHandler(packTimer_Tick);
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
					packTimer.Tick -= new EventHandler(packTimer_Tick);
					packTimer.Dispose();

					CoreManager.Current.ChatBoxMessage -= new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);

					CoreManager.Current.WorldFilter.CreateObject -= new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<Decal.Adapter.Wrappers.ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}

		void Current_ChatBoxMessage(object sender, ChatTextInterceptEventArgs e)
		{
			try
			{
				if (e.Text.StartsWith("You say, ") || e.Text.Contains("says, \""))
					return;

				if (e.Text.ToLower().Contains(CoreManager.Current.CharacterFilter.Name.ToLower() + " autopack"))
					startAutoPack();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_CreateObject(object sender, Decal.Adapter.Wrappers.CreateObjectEventArgs e)
		{
			try
			{
				// Catch when an object is given to us by creating it in our inventory

				// Called when purchasing an item from a vendor.
				// Called when someone hands you an item.
				// Called when a character first logs in.

				if (!Enabled)
					return;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, Decal.Adapter.Wrappers.ChangeObjectEventArgs e)
		{
			try
			{
				// Catch when we pickup an object

				// StorageChange when picking up an item.
				// StorageChange when receiving an item via trade.
				// StorageChange when you move an item in your own inventory.
				// StorageChange when you dequip an item.

				if (!Enabled)
					return;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private VTClassic.LootCore lootProfile = new VTClassic.LootCore();

		private struct ItemToProcess
		{
			public ItemToProcess(int id, int[] targetPackIds)
			{
				this.Id = id;
				this.TargetPackIds = targetPackIds;
			}

			public readonly int Id;
			public readonly int[] TargetPackIds;
		}

		private Collection<ItemToProcess> ItemsToProcess = new Collection<ItemToProcess>();

		void startAutoPack()
		{
			FileInfo fileInfo = new FileInfo(PluginCore.PluginPersonalFolder + @"\" + CoreManager.Current.CharacterFilter.Name + ".AutoPack.utl");

			if (!fileInfo.Exists)
				return;

			CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Auto Pack - Started.", 5);


			// Load our loot profile
			lootProfile.LoadProfile(fileInfo.FullName, false);


			// Get all of our side pack information and put them in the correct order
			int[] Packs = new int[CoreManager.Current.WorldFilter[CoreManager.Current.CharacterFilter.Id].Values(LongValueKey.PackSlots) + 1];

			// Main pack
			Packs[0] = CoreManager.Current.CharacterFilter.Id;

			// Load the side pack information
			foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetByContainer(CoreManager.Current.CharacterFilter.Id))
			{
				if (obj.ObjectClass != ObjectClass.Container)
					continue;

				Packs[obj.Values(LongValueKey.Slot) + 1] = obj.Id;
			}


			// Process our inventory
			ItemsToProcess.Clear();

			foreach (WorldObject item in CoreManager.Current.WorldFilter.GetInventory())
			{
				// If the item is equipped or wielded, don't process it.
				if (item.Values(LongValueKey.EquippedSlots, 0) > 0 || item.Values(LongValueKey.Slot, -1) == -1)
					continue;

				// If the item is a container or a foci, don't process it.
				if (item.ObjectClass == ObjectClass.Container || item.ObjectClass == ObjectClass.Foci)
					continue;

				// Convert the item into a VT GameItemInfo object
				uTank2.LootPlugins.GameItemInfo itemInfo = uTank2.PluginCore.PC.FWorldTracker_GetWithID(item.Id);

				uTank2.LootPlugins.LootAction result = lootProfile.GetLootDecision(itemInfo);

				if (!result.IsKeepUpTo)
					continue;

				// Separate the Data1 int result into a byte[]. Meaning, turn 123456 into { 1, 2, 3, 4, 5, 6 }
				char[] packNumbersAsChar = result.Data1.ToString().ToCharArray();
				byte[] packNumbers = new byte[packNumbersAsChar.Length];
				for (int i = 0 ; i < packNumbersAsChar.Length ; i++)
					packNumbers[i] = byte.Parse(packNumbersAsChar[i].ToString());

				// If this item is already in its primary pack, we don't need to queue it up.
				if (item.Container == Packs[packNumbers[0]])
					continue;

				int[] targetPackIds = new int[packNumbers.Length];
				for (int i = 0 ; i < packNumbers.Length ; i++)
					targetPackIds[i] = Packs[packNumbers[i]];

				ItemToProcess itemToProcess = new ItemToProcess(item.Id, targetPackIds);

				ItemsToProcess.Add(itemToProcess);
			}


			lootProfile.UnloadProfile();

			if (ItemsToProcess.Count == 0)
			{
				CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Auto Pack - All items alraedy packed.", 5);

				return;
			}

			packTimer.Interval = 200;
			packTimer.Start();
		}

		void packTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (ItemsToProcess.Count == 0)
				{
					packTimer.Stop();

					CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Auto Pack - Completed.", 5);

					return;
				}

				// Lets go through our list and see if any items are in their primary target pack
				for (int i = ItemsToProcess.Count - 1 ; i >= 0 ; i--)
				{
					ItemToProcess itemToProcess = ItemsToProcess[i];

					WorldObject item = CoreManager.Current.WorldFilter[itemToProcess.Id];

					if (item.Container == itemToProcess.TargetPackIds[0])
					{
						ItemsToProcess.RemoveAt(i);

						continue;
					}
				}

				Collection<int> itemsToSkip = new Collection<int>();

				// Lets see if we can find an item that can be moved to its target pack
				for (int packIndex = 0 ; packIndex < 10 ; packIndex++)
				{
					for (int i = ItemsToProcess.Count - 1 ; i >= 0 ; i--)
					{
						ItemToProcess itemToProcess = ItemsToProcess[i];

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

						if (Util.GetFreePackSlots(itemToProcess.TargetPackIds[packIndex]) > 0)
						{
							CoreManager.Current.Actions.MoveItem(item.Id, itemToProcess.TargetPackIds[packIndex], 0, false);

							return;
						}
					}
				}

				// If we've gotten to this point no items were moved.
				ItemsToProcess.Clear();
				packTimer.Stop();

				CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Auto Pack - Completed.", 5);

			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
