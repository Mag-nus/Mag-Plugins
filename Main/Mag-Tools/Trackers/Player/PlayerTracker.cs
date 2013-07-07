using System;
using System.Collections.Generic;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Trackers.Player
{
	class PlayerTracker : ITracker<TrackedPlayer>, IDisposable
	{
		/// <summary>
		/// This is raised when an item has been added to the tracker.
		/// </summary>
		public event Action<TrackedPlayer> ItemAdded;

		/// <summary>
		/// This is raised when an item we're tracking has been changed.
		/// </summary>
		public event Action<TrackedPlayer> ItemChanged;

		/// <summary>
		/// This is raised when we have stopped tracking an item.
		/// </summary>
		public event Action<TrackedPlayer> ItemRemoved;

		readonly Dictionary<string, TrackedPlayer> trackedItems = new Dictionary<string, TrackedPlayer>();

		public PlayerTracker()
		{
			try
			{
				CoreManager.Current.WorldFilter.CreateObject += new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.MoveObject += new EventHandler<Decal.Adapter.Wrappers.MoveObjectEventArgs>(WorldFilter_MoveObject);
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
					CoreManager.Current.WorldFilter.MoveObject -= new EventHandler<Decal.Adapter.Wrappers.MoveObjectEventArgs>(WorldFilter_MoveObject);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void WorldFilter_CreateObject(object sender, Decal.Adapter.Wrappers.CreateObjectEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.PlayerTracker.Enabled.Value)
					return;

				ProcessWorldObject(e.New);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_MoveObject(object sender, Decal.Adapter.Wrappers.MoveObjectEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.PlayerTracker.Enabled.Value)
					return;

				ProcessWorldObject(e.Moved);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void ProcessWorldObject(WorldObject wo)
		{
			if (wo.ObjectClass != ObjectClass.Player)
				return;

			if (wo.Name == CoreManager.Current.CharacterFilter.Name)
				return;

			if (!trackedItems.ContainsKey(wo.Name))
			{
				TrackedPlayer trackedItem = new TrackedPlayer(wo.Name, DateTime.Now, wo.Values(LongValueKey.Landblock), wo.RawCoordinates().X, wo.RawCoordinates().Y, wo.RawCoordinates().Z, wo.Id);

				trackedItems.Add(trackedItem.Name, trackedItem);

				if (ItemAdded != null)
					ItemAdded(trackedItem);
			}
			else
			{
				TrackedPlayer trackedItem = trackedItems[wo.Name];

				trackedItem.LastSeen = DateTime.Now;

				trackedItem.LandBlock = wo.Values(LongValueKey.Landblock);

				trackedItem.LocationX = wo.RawCoordinates().X;
				trackedItem.LocationY = wo.RawCoordinates().Y;
				trackedItem.LocationZ = wo.RawCoordinates().Z;

				trackedItem.Id = wo.Id;

				if (ItemChanged != null)
					ItemChanged(trackedItem);
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
			PlayerTrackerImporter importer = new PlayerTrackerImporter(xmlFileName);

			List<TrackedPlayer> importedList = new List<TrackedPlayer>();

			importer.Import(importedList);

			foreach (var item in importedList)
			{
				if (trackedItems.ContainsKey(item.Name))
					continue;

				trackedItems.Add(item.Name, item);

				if (ItemAdded != null)
					ItemAdded(item);
			}
		}

		public void ExportStats(string xmlFileName, bool showMessage = false)
		{
			if (trackedItems.Count == 0)
				return;

			List<TrackedPlayer> exportedList = new List<TrackedPlayer>();

			foreach (var kvp in trackedItems)
				exportedList.Add(kvp.Value);

			PlayerTrackerExporter exporter = new PlayerTrackerExporter(exportedList);

			exporter.Export(xmlFileName);

			if (showMessage)
				CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Stats exported to: " + xmlFileName, 5, Settings.SettingsManager.Misc.OutputTargetWindow.Value);
		}
	}
}
