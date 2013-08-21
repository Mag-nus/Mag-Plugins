using System;

using MagTools.Trackers;
using MagTools.Trackers.Consumable;

using Mag.Shared;

using VirindiViewService.Controls;

namespace MagTools.Views
{
	class ConsumableTrackerGUI : IDisposable
	{
		readonly ITracker<TrackedConsumable> tracker;
		readonly HudList hudList;

		public ConsumableTrackerGUI(ITracker<TrackedConsumable> tracker, HudList hudList)
		{
			try
			{
				this.tracker = tracker;
				this.hudList = hudList;

				hudList.ClearColumnsAndRows();

				/*consumableList.AddColumn(typeof(HudStaticText), 100, null);
				consumableList.AddColumn(typeof(HudStaticText), 115, null);
				consumableList.AddColumn(typeof(HudStaticText), 100, null);

				HudList.HudListRowAccessor newRow = consumableList.AddRow();
				((HudStaticText)newRow[0]).Text = "Time";
				((HudStaticText)newRow[1]).Text = "Name";
				((HudStaticText)newRow[2]).Text = "Coords";*/

				tracker.ItemAdded += new Action<TrackedConsumable>(consumableTracker_ItemAdded);
				tracker.ItemChanged += new Action<TrackedConsumable>(consumableTracker_ItemChanged);
				tracker.ItemRemoved += new Action<TrackedConsumable>(consumableTracker_ItemRemoved);
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
					tracker.ItemAdded -= new Action<TrackedConsumable>(consumableTracker_ItemAdded);
					tracker.ItemChanged -= new Action<TrackedConsumable>(consumableTracker_ItemChanged);
					tracker.ItemRemoved -= new Action<TrackedConsumable>(consumableTracker_ItemRemoved);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void consumableTracker_ItemAdded(TrackedConsumable item)
		{
			try
			{
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void consumableTracker_ItemChanged(TrackedConsumable item)
		{
			try
			{
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void consumableTracker_ItemRemoved(TrackedConsumable item)
		{
			try
			{
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
