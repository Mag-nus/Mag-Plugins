using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

using Mag.Shared;

namespace MagTools.Trackers.Inventory
{
	class InventoryTracker : IItemTracker<TrackedInventory>, IDisposable
	{
		/// <summary>
		/// TThis is raised when one or more items have been added to the tracker.
		/// </summary>
		public event Action<ICollection<TrackedInventory>> ItemsAdded;

		/// <summary>
		/// This is raised when an item we're tracking has been changed.
		/// </summary>
		public event Action<TrackedInventory> ItemChanged;

		/// <summary>
		/// This is raised when we have stopped tracking an item. After this is raised the ManaTrackedItem is disposed.
		/// </summary>
		public event Action<TrackedInventory> ItemRemoved;

		readonly List<TrackedInventory> trackedItems = new List<TrackedInventory>();
		readonly Dictionary<TrackedInventory, DateTime> lastChangeRaised = new Dictionary<TrackedInventory, DateTime>();

		readonly Timer timer = new Timer();

		public InventoryTracker()
		{
			try
			{
				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);
				CoreManager.Current.CharacterFilter.Logoff += new EventHandler<LogoffEventArgs>(CharacterFilter_Logoff);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				CoreManager.Current.WorldFilter.CreateObject += new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<Decal.Adapter.Wrappers.ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				CoreManager.Current.WorldFilter.ReleaseObject += new EventHandler<Decal.Adapter.Wrappers.ReleaseObjectEventArgs>(WorldFilter_ReleaseObject);

				var stopWatch = new System.Diagnostics.Stopwatch();

				if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
					stopWatch.Start();

				foreach (WorldObject inventoryObject in CoreManager.Current.WorldFilter.GetInventory())
					ProcessObject(inventoryObject);

				if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
					Debug.WriteToChat("Loaded Inventory Tracker: " + stopWatch.Elapsed.TotalMilliseconds.ToString("N0") + "ms");

				timer.Tick += new EventHandler(timer_Tick);
				timer.Interval = 500;
				timer.Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_Logoff(object sender, LogoffEventArgs e)
		{
			try
			{
				CoreManager.Current.WorldFilter.CreateObject -= new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<Decal.Adapter.Wrappers.ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				CoreManager.Current.WorldFilter.ReleaseObject -= new EventHandler<Decal.Adapter.Wrappers.ReleaseObjectEventArgs>(WorldFilter_ReleaseObject);

				timer.Tick -= new EventHandler(timer_Tick);
				timer.Stop();
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
					CoreManager.Current.CharacterFilter.Logoff -= new EventHandler<LogoffEventArgs>(CharacterFilter_Logoff);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void WorldFilter_CreateObject(object sender, CreateObjectEventArgs e)
		{
			try
			{
				ProcessObject(e.New);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, Decal.Adapter.Wrappers.ChangeObjectEventArgs e)
		{
			try
			{
				ProcessObject(e.Changed);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ReleaseObject(object sender, Decal.Adapter.Wrappers.ReleaseObjectEventArgs e)
		{
			try
			{
				ProcessObject(e.Released);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void ProcessObject(WorldObject obj)
		{
			if (obj == null)
				return;

			if (obj.ObjectClass == ObjectClass.SpellComponent)
			{
				if (obj.Name.Contains("Taper") || obj.Name.Contains("Scarab"))
					ProcessItemToTrack(obj);
			}

			if (obj.ObjectClass == ObjectClass.ManaStone)
				ProcessItemToTrack(obj);

			if (obj.ObjectClass == ObjectClass.HealingKit)
				ProcessItemToTrack(obj);

			if (obj.ObjectClass == ObjectClass.TradeNote)
				ProcessItemToTrack(obj, "Trade Note");

			if (obj.ObjectClass == ObjectClass.Salvage)
				ProcessItemToTrack(obj, "Salvage");

			if (obj.ObjectClass == ObjectClass.Misc)
			{
				// Dark Isle Trophies
				if (obj.Name == "Corrupted Essence" || obj.Name == "Lesser Corrupted Essence")
					ProcessItemToTrack(obj, "Corrupted Essence");
			}
		}

		void ProcessItemToTrack(WorldObject obj, string nameOverride = null)
		{
			string name = String.IsNullOrEmpty(nameOverride) ? obj.Name : nameOverride;

			int itemValue = obj.Values(LongValueKey.Value);

			if (obj.Values(LongValueKey.StackCount) != 0)
				itemValue /= obj.Values(LongValueKey.StackCount);

			int count = 0;

			foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetInventory())
			{
				if (wo.ObjectClass == obj.ObjectClass)
				{
					if (!String.IsNullOrEmpty(nameOverride) && wo.Name.Contains(name))
						count += wo.Values(LongValueKey.StackCount) == 0 ? 1 : wo.Values(LongValueKey.StackCount);
					else if (String.IsNullOrEmpty(nameOverride) && wo.Name == name)
						count += wo.Values(LongValueKey.StackCount) == 0 ? 1 : wo.Values(LongValueKey.StackCount);
				}
			}

			foreach (var trackedItem in trackedItems)
			{
				if (trackedItem.Name == name)
				{
					trackedItem.AddSnapShot(DateTime.Now, count, SnapShotGroup<int>.PruneMethod.DecreaseResolution);

					if (ItemChanged != null)
					{
						ItemChanged(trackedItem);

						if (lastChangeRaised.ContainsKey(trackedItem))
							lastChangeRaised[trackedItem] = DateTime.Now;
						else
							lastChangeRaised.Add(trackedItem, DateTime.Now);
					}

					return;
				}
			}

			var trackedInventory = new TrackedInventory(name, obj.ObjectClass, obj.Icon, itemValue);
			trackedInventory.AddSnapShot(DateTime.Now, count, SnapShotGroup<int>.PruneMethod.DecreaseResolution);

			trackedItems.Add(trackedInventory);

			if (ItemsAdded != null)
			{
				ItemsAdded(new List<TrackedInventory> { trackedInventory });

				if (lastChangeRaised.ContainsKey(trackedInventory))
					lastChangeRaised[trackedInventory] = DateTime.Now;
				else
					lastChangeRaised.Add(trackedInventory, DateTime.Now);
			}
		}

		void timer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (ItemChanged == null)
					return;

				foreach (var trackedItem in trackedItems)
				{
					if (lastChangeRaised.ContainsKey(trackedItem) && DateTime.Now - lastChangeRaised[trackedItem] < TimeSpan.FromSeconds(1))
						return;

					ItemChanged(trackedItem);

					if (lastChangeRaised.ContainsKey(trackedItem))
						lastChangeRaised[trackedItem] = DateTime.Now;
					else
						lastChangeRaised.Add(trackedItem, DateTime.Now);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		/// <summary>
		/// This will return the next item to be depleated over period, or null if no items exist or everything is depleted.
		/// </summary>
		public TrackedInventory NextItemToBeDepleted(TimeSpan period)
		{
			if (trackedItems.Count == 0)
				return null;

			TrackedInventory nextItemToBeDepleted = null;

			foreach (var item in trackedItems)
			{
				var remaining = item.GetTimeToDepletion(period);

				if (remaining <= TimeSpan.Zero || remaining == TimeSpan.MaxValue)
					continue;

				if (nextItemToBeDepleted == null || remaining < nextItemToBeDepleted.GetTimeToDepletion(period))
					nextItemToBeDepleted = item;
			}

			return nextItemToBeDepleted;
		}
	}
}
