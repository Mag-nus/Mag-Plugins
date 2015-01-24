using System;
using System.Collections.Generic;

namespace MagTools.Trackers
{
	public interface IItemTracker<T>
	{
		/// <summary>
		/// This is raised when one or more items have been added to the tracker.
		/// </summary>
		event Action<ICollection<T>> ItemsAdded;

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
