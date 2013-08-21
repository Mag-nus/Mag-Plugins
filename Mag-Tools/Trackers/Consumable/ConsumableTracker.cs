using System;
using System.Collections.Generic;

using Mag.Shared;

namespace MagTools.Trackers.Consumable
{
	class ConsumableTracker : ITracker<TrackedConsumable>, IDisposable
	{
		/// <summary>
		/// This is raised when an item has been added to the tracker.
		/// </summary>
		public event Action<TrackedConsumable> ItemAdded;

		/// <summary>
		/// This is raised when an item we're tracking has been changed.
		/// </summary>
		public event Action<TrackedConsumable> ItemChanged;

		/// <summary>
		/// This is raised when we have stopped tracking an item. After this is raised the ManaTrackedItem is disposed.
		/// </summary>
		public event Action<TrackedConsumable> ItemRemoved;

		readonly Dictionary<int, TrackedConsumable> trackedItems = new Dictionary<int, TrackedConsumable>();

		public ConsumableTracker()
		{
			try
			{
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
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}
	}
}
