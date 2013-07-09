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

		readonly List<TrackedCorpse> trackedItems = new List<TrackedCorpse>();

		public CorpseTracker()
		{
			try
			{
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

				for (int i = 0 ; i <= trackedItems.Count ; i++)
				{
					if (i == trackedItems.Count)
					{
						WorldObject wo = CoreManager.Current.WorldFilter[e.ItemGuid];

						if (wo == null)
							return;

						ProcessWorldObject(wo, true);

						break;
					}

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

		void ProcessWorldObject(WorldObject wo, bool opened = false)
		{
			if (wo.ObjectClass != ObjectClass.Corpse)
				return;

			for (int i = 0 ; i < trackedItems.Count ; i++)
			{
				if (trackedItems[i].Id == wo.Id)
				{
					TrackedCorpse trackedItem = trackedItems[wo.Id];

					// Is this the same corpse as one we've already opened?
					if (trackedItem.LandBlock == wo.Values(LongValueKey.Landblock) && Math.Abs(trackedItem.LocationX - wo.RawCoordinates().X) < 1 && Math.Abs(trackedItem.LocationY - wo.RawCoordinates().Y) < 1)
						return;

					// New corpse with same id
					if (ItemRemoved != null)
						ItemRemoved(trackedItem);

					trackedItems.RemoveAt(i);

					break;
				}
			}

			bool trackCorpse = false;

			// My own corpses
			if (wo.Name.Contains(CoreManager.Current.CharacterFilter.Name))
				trackCorpse = true;

			if (Settings.SettingsManager.CorpseTracker.TrackAllCorpses.Value)
				trackCorpse = true;

			if (Settings.SettingsManager.CorpseTracker.TrackFellowCorpses.Value)
			{
				// fix
			}

			if (Settings.SettingsManager.CorpseTracker.TrackPermittedCorpses.Value)
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

				DoAddItem(trackedItem);
			}
		}

		void DoAddItem(TrackedCorpse item)
		{
			// Limit the tracker to only the 1000 most recent items
			if (trackedItems.Count > 1000)
				trackedItems.RemoveRange(0, trackedItems.Count - 1000);

			trackedItems.Add(item);

			if (ItemAdded != null)
				ItemAdded(item);
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
			CorpseTrackerImporter importer = new CorpseTrackerImporter(xmlFileName);

			List<TrackedCorpse> importedList = new List<TrackedCorpse>();

			importer.Import(importedList);

			foreach (var newItem in importedList)
			{
				foreach (var item in trackedItems)
				{
					if (newItem.Id == item.Id)
						goto next;
				}

				DoAddItem(newItem);

				next:;
			}
		}

		public void ExportStats(string xmlFileName, bool showMessage = false)
		{
			if (trackedItems.Count == 0)
				return;

			List<TrackedCorpse> exportList = new List<TrackedCorpse>();

			foreach (var item in trackedItems)
			{
				if (item.Description.Contains(CoreManager.Current.CharacterFilter.Name))
					exportList.Add(item);
			}

			CorpseTrackerExporter exporter = new CorpseTrackerExporter(exportList);

			exporter.Export(xmlFileName);

			if (showMessage)
				CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Stats exported to: " + xmlFileName, 5, Settings.SettingsManager.Misc.OutputTargetWindow.Value);
		}
	}
}
