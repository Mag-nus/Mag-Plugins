using System;

namespace MagTools.Trackers
{
	public interface ITracker<T>
	{
		/// <summary>
		/// This is raised when an item has been added to the tracker.
		/// </summary>
		event Action<T> ItemAdded;

		/// <summary>
		/// This is raised when an item we're tracking has been changed.
		/// </summary>
		event Action<T> ItemChanged;

		/// <summary>
		/// This is raised when we have stopped tracking an item.
		/// </summary>
		event Action<T> ItemRemoved;
	}
}
