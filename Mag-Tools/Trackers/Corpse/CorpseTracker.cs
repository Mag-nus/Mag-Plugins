using System;
using System.Collections.Generic;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

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

		readonly Dictionary<int, TrackedCorpse> trackedItems = new Dictionary<int, TrackedCorpse>();

		readonly bool trackOnlyPersistentStats;

		public CorpseTracker(bool trackOnlyPersistentStats = false)
		{
			try
			{
				this.trackOnlyPersistentStats = trackOnlyPersistentStats;

				CoreManager.Current.WorldFilter.CreateObject += new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				CoreManager.Current.ContainerOpened += new EventHandler<ContainerOpenedEventArgs>(Current_ContainerOpened);
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
					CoreManager.Current.WorldFilter.CreateObject -= new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
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

		void Current_ContainerOpened(object sender, ContainerOpenedEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.CorpseTracker.Enabled.Value)
					return;

				if (trackedItems.ContainsKey(e.ItemGuid))
				{
					TrackedCorpse trackedItem = trackedItems[e.ItemGuid];

					if (trackedItem.Opened)
						return;

					trackedItem.Opened = true;

					if (ItemChanged != null)
						ItemChanged(trackedItem);
				}
				else
				{
					WorldObject wo = CoreManager.Current.WorldFilter[e.ItemGuid];

					if (wo == null)
						return;

					ProcessWorldObject(wo, true);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void ProcessWorldObject(WorldObject wo, bool opened = false)
		{
			if (wo.ObjectClass != ObjectClass.Corpse)
				return;

			if (trackedItems.ContainsKey(wo.Id))
			{
				TrackedCorpse trackedItem = trackedItems[wo.Id];

				// Is this the same corpse as one we've already opened?
				if (trackedItem.LandBlock == wo.Values(LongValueKey.Landblock) && Math.Abs(trackedItem.LocationX - wo.RawCoordinates().X) < 1 && Math.Abs(trackedItem.LocationY - wo.RawCoordinates().Y) < 1)
					return;

				// New corpse with same id
				if (ItemRemoved != null)
					ItemRemoved(trackedItem);

				trackedItems.Remove(trackedItem.Id);
			}

			bool trackCorpse = false;

			// My own corpses
			if (wo.Name.Contains(CoreManager.Current.CharacterFilter.Name))
				trackCorpse = true;

			if (Settings.SettingsManager.CorpseTracker.TrackAllCorpses.Value && !trackOnlyPersistentStats)
				trackCorpse = true;

			if (Settings.SettingsManager.CorpseTracker.TrackFellowCorpses.Value && !trackOnlyPersistentStats)
			{
				// fix
			}

			if (Settings.SettingsManager.CorpseTracker.TrackPermittedCorpses.Value && !trackOnlyPersistentStats)
			{
				// fix
			}

			// Corpses killed by me
			if (!trackCorpse && wo.Values(LongValueKey.Burden) > 6000 && !trackOnlyPersistentStats)
			{
				if (!wo.HasIdData)
					CoreManager.Current.Actions.RequestId(wo.Id);
				else if (wo.Values(StringValueKey.FullDescription).Contains(CoreManager.Current.CharacterFilter.Name))
					trackCorpse = true;
			}

			if (trackCorpse)
			{
				TrackedCorpse trackedItem = new TrackedCorpse(wo.Id, DateTime.Now, wo.Values(LongValueKey.Landblock), wo.RawCoordinates().X, wo.RawCoordinates().Y, wo.RawCoordinates().Z, wo.Name, opened);

				trackedItems.Add(trackedItem.Id, trackedItem);

				if (ItemAdded != null)
					ItemAdded(trackedItem);
			}
		}

		public void ClearStats()
		{
			foreach (var kvp in trackedItems)
			{
				if (ItemRemoved != null)
					ItemRemoved(kvp.Value);
			}

			trackedItems.Clear();
		}

		public void ImportStats(string xmlFileName)
		{
			CorpseTrackerImporter importer = new CorpseTrackerImporter(xmlFileName);

			List<TrackedCorpse> importedList = new List<TrackedCorpse>();

			importer.Import(importedList);

			foreach (var item in importedList)
			{
				if (trackedItems.ContainsKey(item.Id))
					continue;

				trackedItems.Add(item.Id, item);

				if (ItemAdded != null)
					ItemAdded(item);
			}
		}

		public void ExportStats(string xmlFileName, bool showMessage = false)
		{
			if (trackedItems.Count == 0)
				return;

			List<TrackedCorpse> exportedList = new List<TrackedCorpse>();

			foreach (var kvp in trackedItems)
				exportedList.Add(kvp.Value);

			CorpseTrackerExporter exporter = new CorpseTrackerExporter(exportedList);

			exporter.Export(xmlFileName);

			if (showMessage)
				CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Stats exported to: " + xmlFileName, 5, Settings.SettingsManager.Misc.OutputTargetWindow.Value);
		}
	}
}
