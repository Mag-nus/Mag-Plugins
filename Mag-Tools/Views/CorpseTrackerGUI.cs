using System;
using System.Collections.Generic;

using MagTools.Trackers.Corpse;

using VirindiViewService.Controls;

namespace MagTools.Views
{
	class CorpseTrackerGUI : IDisposable
	{
		readonly CorpseTracker corpseTracker;
		readonly List<TrackedCorpse> trackedCorpses = new List<TrackedCorpse>();

		public CorpseTrackerGUI(CorpseTracker corpseTracker, HudList corpseList)
		{
			try
			{
				this.corpseTracker = corpseTracker;

				corpseTracker.ItemAdded += new Action<TrackedCorpse>(corpseTracker_ItemAdded);
				corpseTracker.ItemRemoved += new Action<TrackedCorpse>(corpseTracker_ItemRemoved);
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
					corpseTracker.ItemAdded -= new Action<TrackedCorpse>(corpseTracker_ItemAdded);
					corpseTracker.ItemRemoved -= new Action<TrackedCorpse>(corpseTracker_ItemRemoved);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void corpseTracker_ItemAdded(TrackedCorpse obj)
		{
			try
			{
				trackedCorpses.Add(obj);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void corpseTracker_ItemRemoved(TrackedCorpse obj)
		{
			try
			{
				trackedCorpses.Remove(obj);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
