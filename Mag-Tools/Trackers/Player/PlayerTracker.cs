using System;
using System.Collections.Generic;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Trackers.Player
{
	class PlayerTracker : IDisposable
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

		readonly Dictionary<string, TrackedPlayer> trackedPlayers = new Dictionary<string, TrackedPlayer>();

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
				ProcessWorldObject(e.New);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_MoveObject(object sender, Decal.Adapter.Wrappers.MoveObjectEventArgs e)
		{
			try
			{
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

			if (!trackedPlayers.ContainsKey(wo.Name))
			{
				TrackedPlayer trackedPlayer = new TrackedPlayer(wo.Name, DateTime.Now, wo.Values(LongValueKey.Landblock), wo.RawCoordinates().X, wo.RawCoordinates().Y, wo.RawCoordinates().Z, wo.Id);

				trackedPlayers.Add(trackedPlayer.Name, trackedPlayer);

				if (ItemAdded != null)
					ItemAdded(trackedPlayer);
			}
			else
			{
				TrackedPlayer trackedPlayer = trackedPlayers[wo.Name];

				trackedPlayer.LastSeen = DateTime.Now;

				trackedPlayer.LandBlock = wo.Values(LongValueKey.Landblock);

				trackedPlayer.LocationX = wo.RawCoordinates().X;
				trackedPlayer.LocationY = wo.RawCoordinates().Y;
				trackedPlayer.LocationZ = wo.RawCoordinates().Z;

				trackedPlayer.Id = wo.Id;

				if (ItemChanged != null)
					ItemChanged(trackedPlayer);
			}
		}

		public void ClearStats()
		{
			foreach (var kvp in trackedPlayers)
			{
				if (ItemRemoved != null)
					ItemRemoved(kvp.Value);
			}

			trackedPlayers.Clear();
		}

		public void ImportStats(string xmlFileName)
		{
			PlayerTrackerImporter importer = new PlayerTrackerImporter(xmlFileName);

			List<TrackedPlayer> importedList = new List<TrackedPlayer>();

			importer.Import(importedList);

			foreach (var item in importedList)
			{
				if (trackedPlayers.ContainsKey(item.Name))
					continue;

				trackedPlayers.Add(item.Name, item);

				if (ItemAdded != null)
					ItemAdded(item);
			}
		}

		public void ExportStats(string xmlFileName, bool showMessage = false)
		{
			if (trackedPlayers.Count == 0)
				return;

			List<TrackedPlayer> exportedList = new List<TrackedPlayer>();

			foreach (var kvp in trackedPlayers)
				exportedList.Add(kvp.Value);

			PlayerTrackerExporter exporter = new PlayerTrackerExporter(exportedList);

			exporter.Export(xmlFileName);

			if (showMessage)
				CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Stats exported to: " + xmlFileName, 5, Settings.SettingsManager.Misc.OutputTargetWindow.Value);
		}
	}
}
