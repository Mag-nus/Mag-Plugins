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
		/// This is raised when we have stopped tracking an item. After this is raised the ManaTrackedItem is disposed.
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
				ProcessPlayerWorldObject(e.New);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_MoveObject(object sender, Decal.Adapter.Wrappers.MoveObjectEventArgs e)
		{
			try
			{
				ProcessPlayerWorldObject(e.Moved);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void ProcessPlayerWorldObject(WorldObject playerWO)
		{
			if (playerWO.ObjectClass != ObjectClass.Player)
				return;

			if (playerWO.Name == CoreManager.Current.CharacterFilter.Name)
				return;

			if (!trackedPlayers.ContainsKey(playerWO.Name))
			{
				TrackedPlayer trackedPlayer = new TrackedPlayer(playerWO.Name, DateTime.Now, playerWO.Values(LongValueKey.Landblock), playerWO.RawCoordinates().X, playerWO.RawCoordinates().Y, playerWO.RawCoordinates().Z, playerWO.Id);

				trackedPlayers.Add(trackedPlayer.Name, trackedPlayer);

				if (ItemAdded != null)
					ItemAdded(trackedPlayer);
			}
		}

		public void ClearStats()
		{
			foreach (var kvp in trackedPlayers)
			{
				if (ItemRemoved != null)
					ItemRemoved(kvp.Value);

				kvp.Value.Dispose();
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
