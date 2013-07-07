using System;

using MagTools.Trackers;
using MagTools.Trackers.Consumable;

using VirindiViewService.Controls;

namespace MagTools.Views
{
	class ConsumableTrackerGUI : IDisposable
	{
		readonly ITracker<TrackedConsumable> consumableTracker;
		readonly HudList consumableList;

		public ConsumableTrackerGUI(ITracker<TrackedConsumable> consumableTracker, HudList consumableList)
		{
			try
			{
				this.consumableTracker = consumableTracker;
				this.consumableList = consumableList;

				consumableList.ClearColumnsAndRows();

				/*consumableList.AddColumn(typeof(HudStaticText), 100, null);
				consumableList.AddColumn(typeof(HudStaticText), 115, null);
				consumableList.AddColumn(typeof(HudStaticText), 100, null);

				HudList.HudListRowAccessor newRow = consumableList.AddRow();
				((HudStaticText)newRow[0]).Text = "Time";
				((HudStaticText)newRow[1]).Text = "Name";
				((HudStaticText)newRow[2]).Text = "Coords";*/

				consumableTracker.ItemAdded += new Action<TrackedConsumable>(consumableTracker_ItemAdded);
				consumableTracker.ItemChanged += new Action<TrackedConsumable>(consumableTracker_ItemChanged);
				consumableTracker.ItemRemoved += new Action<TrackedConsumable>(consumableTracker_ItemRemoved);
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
					consumableTracker.ItemAdded -= new Action<TrackedConsumable>(consumableTracker_ItemAdded);
					consumableTracker.ItemChanged -= new Action<TrackedConsumable>(consumableTracker_ItemChanged);
					consumableTracker.ItemRemoved -= new Action<TrackedConsumable>(consumableTracker_ItemRemoved);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void consumableTracker_ItemAdded(TrackedConsumable obj)
		{
			try
			{
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void consumableTracker_ItemChanged(TrackedConsumable obj)
		{
			try
			{
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void consumableTracker_ItemRemoved(TrackedConsumable obj)
		{
			try
			{
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
