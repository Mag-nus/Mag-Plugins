using System;
using System.Collections.Generic;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Trackers.Equipment
{
	class EquipmentTracker : IDisposable, IEquipmentTracker
	{
		/// <summary>
		/// This is raised when an item has been added to the tracker.
		/// </summary>
		public event Action<IEquipmentTrackedItem> ItemAdded;

		/// <summary>
		/// This is raised when we have stopped tracking an item. After this is raised the ManaTrackedItem is disposed.
		/// </summary>
		public event Action<IEquipmentTrackedItem> ItemRemoved;

		private readonly List<EquipmentTrackedItem> trackedItems = new List<EquipmentTrackedItem>();

		public EquipmentTracker()
		{
			try
			{
				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);
				CoreManager.Current.WorldFilter.CreateObject += new EventHandler<CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				CoreManager.Current.WorldFilter.ReleaseObject += new EventHandler<ReleaseObjectEventArgs>(WorldFilter_ReleaseObject);
				CoreManager.Current.CharacterFilter.Logoff += new EventHandler<LogoffEventArgs>(CharacterFilter_Logoff);
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
					CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);
					CoreManager.Current.WorldFilter.CreateObject -= new EventHandler<CreateObjectEventArgs>(WorldFilter_CreateObject);
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
					CoreManager.Current.WorldFilter.ReleaseObject -= new EventHandler<ReleaseObjectEventArgs>(WorldFilter_ReleaseObject);
					CoreManager.Current.CharacterFilter.Logoff -= new EventHandler<LogoffEventArgs>(CharacterFilter_Logoff);

					RemoveAllTrackedItems();
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				RemoveAllTrackedItems();

				// Add all of our items
				foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetInventory())
				{
					if (ShoudlWeWatchItem(obj))
						AddItem(obj);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_CreateObject(object sender, CreateObjectEventArgs e)
		{
			try
			{
				if (ShoudlWeWatchItem(e.New))
					AddItem(e.New);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
		{
			try
			{
				/*
				Storage change is a goofy process.
				if (e.Changed.Name.Contains("White E")) Util.WriteToChat(e.Changed.Name + " " + e.Change + ", Container: " + e.Changed.Container + ", Slot: " + e.Changed.Values(LongValueKey.Slot, 0) + ", EquippedSlots: " + e.Changed.Values(LongValueKey.EquippedSlots));

				Dequip:
				<{Mag-Tools}>: Enhanced White Empyrean Ring StorageChange, Container: 1343094282, Slot: 29, EquippedSlots: 262144
				<{Mag-Tools}>: AddItem
				<{Mag-Tools}>: Enhanced White Empyrean Ring StorageChange, Container: 1343094282, Slot: 29, EquippedSlots: 262144
				<{Mag-Tools}>: AddItem
				<{Mag-Tools}>: Enhanced White Empyrean Ring StorageChange, Container: 1343094282, Slot: 0, EquippedSlots: 0
				<{Mag-Tools}>: RemoveItem

				Equip:
				<{Mag-Tools}>: Enhanced White Empyrean Ring StorageChange, Container: 1343094282, Slot: 0, EquippedSlots: 262144
				<{Mag-Tools}>: AddItem7
				<{Mag-Tools}>: Enhanced White Empyrean Ring StorageChange, Container: 0, Slot: 0, EquippedSlots: 262144
				<{Mag-Tools}>: RemoveItem
				<{Mag-Tools}>: Enhanced White Empyrean Ring StorageChange, Container: 1343094282, Slot: 0, EquippedSlots: 262144
				<{Mag-Tools}>: AddItem
				Enhanced White Empyrean Ring cast Incantation of Armor Other on you, surpassing Incantation of Armor Self
				<{Mag-Tools}>: Enhanced White Empyrean Ring IdentReceived, Container: 1343094282, Slot: 0, EquippedSlots: 262144
				*/

				// This is kind of a goofy check.
				// When we equip an item StorageChange is raised three times on that item.
				// The process will call AddItem, then RemoveItem, then AddItem
				if (e.Change == WorldChangeType.StorageChange && e.Changed.Values(LongValueKey.Container) != 0)
				{
					if (ShoudlWeWatchItem(e.Changed))
						AddItem(e.Changed);
					else
						RemoveItem(e.Changed);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ReleaseObject(object sender, ReleaseObjectEventArgs e)
		{
			try
			{
				RemoveItem(e.Released);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_Logoff(object sender, LogoffEventArgs e)
		{
			try
			{
				RemoveAllTrackedItems();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		protected virtual bool ShoudlWeWatchItem(WorldObject obj)
		{
			// Only watch items equipped by me
			if (!ItemIsEquippedByMe(obj))
				return false;

			return true;
		}

		bool ItemIsEquippedByMe(WorldObject obj)
		{
			if (obj.Values(LongValueKey.EquippedSlots) <= 0)
				return false;

			// Weapons are in the -1 slot
			if (obj.Values(LongValueKey.Slot, -1) == -1)
				return (obj.Container == CoreManager.Current.CharacterFilter.Id);

			return true;
		}

		/// <summary>
		/// This will process an item for addition to our trackedItems list.
		/// It will not allow an item to be added twice.
		/// </summary>
		/// <param name="obj"></param>
		void AddItem(WorldObject obj)
		{
			foreach (EquipmentTrackedItem item in trackedItems)
			{
				if (item.Id == obj.Id)
					return;
			}

			EquipmentTrackedItem trackedItem = new EquipmentTrackedItem(obj.Id);

			trackedItems.Add(trackedItem);

			if (ItemAdded != null)
				ItemAdded(trackedItem);
		}

		void RemoveItem(WorldObject obj)
		{
			for (int i = trackedItems.Count - 1 ; i >= 0 ; i--)
			{
				if (trackedItems[i].Id == obj.Id)
				{
					EquipmentTrackedItem trackedItem = trackedItems[i];

					trackedItems.RemoveAt(i);

					if (ItemRemoved != null)
						ItemRemoved(trackedItem);

					trackedItem.Dispose();
				}
			}
		}

		void RemoveAllTrackedItems()
		{
			for (int i = trackedItems.Count - 1 ; i >= 0 ; i--)
			{
				EquipmentTrackedItem trackedItem = trackedItems[i];

				trackedItems.RemoveAt(i);

				if (ItemRemoved != null)
					ItemRemoved(trackedItem);

				trackedItem.Dispose();
			}
		}

		public TimeSpan RemainingTimeBeforeNextEmptyItem
		{
			get
			{
				TimeSpan timeSpan = TimeSpan.MaxValue;


				foreach (EquipmentTrackedItem trackedItem in trackedItems)
				{
					if (trackedItem.ManaTimeRemaining <= TimeSpan.Zero)
						continue;

					if (trackedItem.ManaTimeRemaining < timeSpan)
						timeSpan = trackedItem.ManaTimeRemaining;
				}

				return timeSpan;
			}
		}

		public int ManaNeededToRefillItems
		{
			get
			{
				int manaNeeded = 0;

				foreach (EquipmentTrackedItem trackedItem in trackedItems)
				{
					manaNeeded += trackedItem.ManaNeededToRefill;
				}

				return manaNeeded;
			}
		}

		public int NumberOfInactiveItems
		{
			get
			{
				int inactiveItems = 0;

				foreach (EquipmentTrackedItem trackedItem in trackedItems)
				{
					if (trackedItem.ItemState == EquipmentTrackedItemState.NotActive)
						inactiveItems++;
				}

				return inactiveItems;
			}
		}

		public int NumberOfUnretainedItems
		{
			get
			{
				int unretainedItems = 0;

				foreach (EquipmentTrackedItem trackedItem in trackedItems)
				{
					WorldObject wo = CoreManager.Current.WorldFilter[trackedItem.Id];

					if (wo == null)
						continue;

					// We don't show archer/missile ammo (arrows)
					if (wo.Values(LongValueKey.EquippedSlots) == 8388608)
						continue;

					if (wo.HasIdData && !wo.Values(BoolValueKey.Retained))
						unretainedItems++;
				}

				return unretainedItems;
			}
		}
	}
}
