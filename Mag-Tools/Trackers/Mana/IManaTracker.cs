using System;

namespace MagTools.Trackers.Mana
{
	public interface IManaTracker
	{
		/// <summary>
		/// This is raised when an item has been added to the tracker.
		/// </summary>
		event Action<IManaTrackedItem> ItemAdded;

		/// <summary>
		/// This is raised when we have stopped tracking an item. After this is raised the ManaTrackedItem is disposed.
		/// </summary>
		event Action<IManaTrackedItem> ItemRemoved;

		int ManaNeededToRefillItems { get; }

		int NumberOfInactiveItems { get; }
	}
}
