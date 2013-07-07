using System;
using System.Globalization;

using MagTools.Trackers;
using MagTools.Trackers.Corpse;

using VirindiViewService.Controls;

using Decal.Adapter.Wrappers;

namespace MagTools.Views
{
	class CorpseTrackerGUI : IDisposable
	{
		readonly ITracker<TrackedCorpse> corpseTracker;
		readonly HudList corpseList;

		public CorpseTrackerGUI(ITracker<TrackedCorpse> corpseTracker, HudList corpseList)
		{
			try
			{
				this.corpseTracker = corpseTracker;
				this.corpseList = corpseList;

				corpseList.ClearColumnsAndRows();

				corpseList.AddColumn(typeof(HudStaticText), 73, null);
				corpseList.AddColumn(typeof(HudStaticText), 142, null);
				corpseList.AddColumn(typeof(HudStaticText), 100, null);
				corpseList.AddColumn(typeof(HudStaticText), 0, null);

				HudList.HudListRowAccessor newRow = corpseList.AddRow();
				((HudStaticText)newRow[0]).Text = "Time";
				((HudStaticText)newRow[1]).Text = "Name";
				((HudStaticText)newRow[2]).Text = "Coords";
				((HudStaticText)newRow[3]).Text = "Id";

				corpseTracker.ItemAdded += new Action<TrackedCorpse>(corpseTracker_ItemAdded);
				corpseTracker.ItemChanged += new Action<TrackedCorpse>(corpseTracker_ItemChanged);
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
					corpseTracker.ItemChanged -= new Action<TrackedCorpse>(corpseTracker_ItemChanged);
					corpseTracker.ItemRemoved -= new Action<TrackedCorpse>(corpseTracker_ItemRemoved);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void corpseTracker_ItemAdded(TrackedCorpse trackedCorpse)
		{
			try
			{
				if (trackedCorpse.Opened)
					return;

				HudList.HudListRowAccessor newRow = corpseList.InsertRow(1);

				((HudStaticText)newRow[0]).Text = trackedCorpse.TimeStamp.ToString("ddd hh:mm tt");

				((HudStaticText)newRow[1]).Text = trackedCorpse.Description;

				CoordsObject newCords = Mag.Shared.Util.GetCoords(trackedCorpse.LandBlock, trackedCorpse.LocationX, trackedCorpse.LocationY);
				((HudStaticText)newRow[2]).Text = newCords.ToString();

				((HudStaticText)newRow[3]).Text = trackedCorpse.Id.ToString(CultureInfo.InvariantCulture);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void corpseTracker_ItemChanged(TrackedCorpse trackedCorpse)
		{
			try
			{
				if (trackedCorpse.Opened)
					RemoveCorpse(trackedCorpse);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void corpseTracker_ItemRemoved(TrackedCorpse trackedCorpse)
		{
			try
			{
				RemoveCorpse(trackedCorpse);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void RemoveCorpse(TrackedCorpse trackedCorpse)
		{
			for (int row = 1; row <= corpseList.RowCount; row++)
			{
				if (((HudStaticText)corpseList[row - 1][3]).Text == trackedCorpse.Id.ToString(CultureInfo.InvariantCulture))
				{
					corpseList.RemoveRow(row - 1);

					row--;
				}
			}			
		}
	}
}
