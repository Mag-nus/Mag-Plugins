using System;

namespace MagTools.Trackers.Equipment
{
	public interface IEquipmentTracker
	{
		/// <summary>
		/// This is raised when an item has been added to the tracker.
		/// </summary>
		event Action<IEquipmentTrackedItem> ItemAdded;

		/// <summary>
		/// This is raised when we have stopped tracking an item. After this is raised the ManaTrackedItem is disposed.
		/// </summary>
		event Action<IEquipmentTrackedItem> ItemRemoved;

		int ManaNeededToRefillItems { get; }

		int NumberOfInactiveItems { get; }

		int NumberOfUnretainedItems { get; }
	}
}
