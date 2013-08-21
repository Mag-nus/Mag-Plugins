using System;
using System.Collections.Generic;

using Mag.Shared;

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

		readonly List<TrackedPlayer> trackedItems = new List<TrackedPlayer>();

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

			for (int i = 0 ; i < trackedItems.Count ; i++)
			{
				if (trackedItems[i].Name == wo.Name)
				{
					TrackedPlayer trackedItem = trackedItems[i];

					trackedItem.LastSeen = DateTime.Now;

					trackedItem.LandBlock = wo.Values(LongValueKey.Landblock);

					trackedItem.LocationX = wo.RawCoordinates().X;
					trackedItem.LocationY = wo.RawCoordinates().Y;
					trackedItem.LocationZ = wo.RawCoordinates().Z;

					trackedItem.Id = wo.Id;

					if (ItemChanged != null)
						ItemChanged(trackedItem);

					return;
				}
			}

			TrackedPlayer newItem = new TrackedPlayer(wo.Name, DateTime.Now, wo.Values(LongValueKey.Landblock), wo.RawCoordinates().X, wo.RawCoordinates().Y, wo.RawCoordinates().Z, wo.Id);

			trackedItems.Add(newItem);

			if (ItemAdded != null)
				ItemAdded(newItem);
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
			List<TrackedPlayer> importedItems;

			if (PlayerTrackerImporter.Import(xmlFileName, out importedItems))
			{
				foreach (var newItem in importedItems)
				{
					foreach (var item in trackedItems)
					{
						if (newItem.Name == item.Name)
							goto next;
					}

					trackedItems.Add(newItem);

					if (ItemAdded != null)
						ItemAdded(newItem);

					next:;
				}
			}
		}

		public void ExportStats(string xmlFileName, bool showMessage = false)
		{
			if (trackedItems.Count == 0)
				return;

			PlayerTrackerExporter.Export(xmlFileName, trackedItems);

			if (showMessage)
				CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Stats exported to: " + xmlFileName, 5, Settings.SettingsManager.Misc.OutputTargetWindow.Value);
		}
	}
}
