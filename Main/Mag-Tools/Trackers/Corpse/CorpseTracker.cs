using System;
using System.Collections.Generic;

using Decal.Adapter;
using Decal.Adapter.Wrappers;
using Mag.Shared;

namespace MagTools.Trackers.Corpse
{
	class CorpseTracker : ITracker<TrackedCorpse>, IDisposable
	{
		/// <summary>
		/// This is raised when an item has been added to the tracker.
		/// </summary>
		public event Action<TrackedCorpse> ItemAdded;

		/// <summary>
		/// This is raised when an item we're tracking has been changed.
		/// </summary>
		public event Action<TrackedCorpse> ItemChanged;

		/// <summary>
		/// This is raised when we have stopped tracking an item. After this is raised the ManaTrackedItem is disposed.
		/// </summary>
		public event Action<TrackedCorpse> ItemRemoved;

		readonly List<TrackedCorpse> trackedItems = new List<TrackedCorpse>();

		System.Windows.Forms.Timer maintenanceTimer = new System.Windows.Forms.Timer();

		public CorpseTracker()
		{
			try
			{
				CoreManager.Current.WorldFilter.CreateObject += new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				CoreManager.Current.WorldFilter.ReleaseObject += new EventHandler<ReleaseObjectEventArgs>(WorldFilter_ReleaseObject);
				CoreManager.Current.ContainerOpened += new EventHandler<ContainerOpenedEventArgs>(Current_ContainerOpened);

				maintenanceTimer.Interval = 60000;
				maintenanceTimer.Tick += new EventHandler(maintenanceTimer_Tick);
				maintenanceTimer.Start();
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
					maintenanceTimer.Tick -= new EventHandler(maintenanceTimer_Tick);

					CoreManager.Current.WorldFilter.CreateObject -= new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
					CoreManager.Current.WorldFilter.ReleaseObject -= new EventHandler<ReleaseObjectEventArgs>(WorldFilter_ReleaseObject);
					CoreManager.Current.ContainerOpened -= new EventHandler<ContainerOpenedEventArgs>(Current_ContainerOpened);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void WorldFilter_CreateObject(object sender, Decal.Adapter.Wrappers.CreateObjectEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.CorpseTracker.Enabled.Value)
					return;

				ProcessWorldObject(e.New);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.CorpseTracker.Enabled.Value)
					return;

				if (e.Change == WorldChangeType.IdentReceived)
					ProcessWorldObject(e.Changed);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ReleaseObject(object sender, ReleaseObjectEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.CorpseTracker.Enabled.Value)
					return;

				// Check if its a corpse decay in range of player
				if (e.Released.ObjectClass == ObjectClass.Corpse && Util.GetDistanceFromPlayer(e.Released) <= 10)
				{
					for (int i = 0; i < trackedItems.Count; i++)
					{
						if (trackedItems[i].Id == e.Released.Id)
						{
							if (ItemRemoved != null)
								ItemRemoved(trackedItems[i]);

							trackedItems.RemoveAt(i);
							break;
						}
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Current_ContainerOpened(object sender, ContainerOpenedEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.CorpseTracker.Enabled.Value)
					return;

				if (e.ItemGuid == 0)
					return;

				for (int i = 0 ; i < trackedItems.Count ; i++)
				{
					if (trackedItems[i].Id == e.ItemGuid)
					{
						TrackedCorpse trackedItem = trackedItems[i];

						if (trackedItem.Opened)
							return;

						trackedItem.Opened = true;

						if (ItemChanged != null)
							ItemChanged(trackedItem);

						break;
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		bool CorpsePassesActiveTrackingFilter(TrackedCorpse item)
		{
			if (item.Description.Contains(CoreManager.Current.CharacterFilter.Name))
				return DateTime.Now - item.TimeStamp < TimeSpan.FromDays(7);

			// Maybe keep corpses that have generated a rare longer...
			// todo

			return DateTime.Now - item.TimeStamp < TimeSpan.FromHours(2);
		}

		void ProcessWorldObject(WorldObject wo, bool opened = false)
		{
			if (wo.ObjectClass != ObjectClass.Corpse)
				return;

			// First, lets see if this world object id exists in our lists, but at different coordinates (means the id was reused)
			for (int i = 0; i < trackedItems.Count; i++)
			{
				if (trackedItems[i].Id == wo.Id)
				{
					if (trackedItems[i].LandBlock != wo.Values(LongValueKey.Landblock) || Math.Abs(trackedItems[i].LocationX - wo.RawCoordinates().X) > 1 || Math.Abs(trackedItems[i].LocationY - wo.RawCoordinates().Y) > 1)
					{
						if (ItemRemoved != null)
							ItemRemoved(trackedItems[i]);

						trackedItems.RemoveAt(i);

						break;
					}
				}
			}

			// Lets see if we're already tracking this item
			for (int i = 0 ; i < trackedItems.Count ; i++)
			{
				if (trackedItems[i].Id == wo.Id)
					return;
			}

			// This is a new item, should we track it?
			bool trackCorpse = false;

			// My own corpses
			if (wo.Name.Contains(CoreManager.Current.CharacterFilter.Name))
				trackCorpse = true;

			if (Settings.SettingsManager.CorpseTracker.TrackAllCorpses.Value)
				trackCorpse = true;

			if (!trackCorpse && Settings.SettingsManager.CorpseTracker.TrackFellowCorpses.Value)
			{
				// fix
			}

			if (!trackCorpse && Settings.SettingsManager.CorpseTracker.TrackPermittedCorpses.Value)
			{
				// fix
			}

			// Corpses killed by me
			if (!trackCorpse && wo.Values(LongValueKey.Burden) > 6000)
			{
				if (!wo.HasIdData)
					CoreManager.Current.Actions.RequestId(wo.Id);
				else if (wo.Values(StringValueKey.FullDescription).Contains(CoreManager.Current.CharacterFilter.Name))
					trackCorpse = true;
			}

			if (trackCorpse)
			{
				TrackedCorpse trackedItem = new TrackedCorpse(wo.Id, DateTime.Now, wo.Values(LongValueKey.Landblock), wo.RawCoordinates().X, wo.RawCoordinates().Y, wo.RawCoordinates().Z, wo.Name, opened);

				trackedItems.Add(trackedItem);

				if (ItemAdded != null)
					ItemAdded(trackedItem);
			}
		}

		void maintenanceTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.CorpseTracker.Enabled.Value)
					return;

				// Move items from trackedItems to expiredItems if they no longer pass the active tracking filter
				for (int i = trackedItems.Count - 1 ; i >= 0 ; i--)
				{
					TrackedCorpse trackedItem = trackedItems[i];

					if (!CorpsePassesActiveTrackingFilter(trackedItem))
					{
						if (ItemRemoved != null)
							ItemRemoved(trackedItem);

						trackedItems.RemoveAt(i);
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		public void ClearStats()
		{
			foreach (var item in trackedItems)
			{
				if (ItemRemoved != null)
					ItemRemoved(item);
			}

			trackedItems.Clear();
		}

		public void ImportStats(string xmlFileName)
		{
			List<TrackedCorpse> importedItems;

			if (CorpseTrackerImporter.Import(xmlFileName, out importedItems))
			{
				foreach (var newItem in importedItems)
				{
					if (CorpsePassesActiveTrackingFilter(newItem))
					{
						foreach (var item in trackedItems)
						{
							if (newItem.Id == item.Id)
								goto next;
						}

						trackedItems.Add(newItem);

						if (ItemAdded != null)
							ItemAdded(newItem);
					}

					next:;
				}
			}
		}

		public void ExportStats(string xmlFileName, bool showMessage = false)
		{
			if (trackedItems.Count == 0)
				return;

			List<TrackedCorpse> exportList = new List<TrackedCorpse>();

			foreach (var item in trackedItems)
			{
				if (CorpsePassesActiveTrackingFilter(item))
					exportList.Add(item);
			}

			CorpseTrackerExporter.Export(xmlFileName, exportList);

			if (showMessage)
				CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Stats exported to: " + xmlFileName, 5, Settings.SettingsManager.Misc.OutputTargetWindow.Value);
		}
	}
}
