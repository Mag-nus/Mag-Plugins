using System;
using System.Globalization;

using MagTools.Trackers.Corpse;

using VirindiViewService.Controls;

using Decal.Adapter.Wrappers;

namespace MagTools.Views
{
	class CorpseTrackerGUI : IDisposable
	{
		readonly CorpseTracker corpseTracker;
		readonly HudList corpseList;

		public CorpseTrackerGUI(CorpseTracker corpseTracker, HudList corpseList)
		{
			try
			{
				this.corpseTracker = corpseTracker;
				this.corpseList = corpseList;

				corpseList.ClearColumnsAndRows();

				corpseList.AddColumn(typeof(HudStaticText), 70, null);
				corpseList.AddColumn(typeof(HudStaticText), 145, null);
				corpseList.AddColumn(typeof(HudStaticText), 100, null);
				corpseList.AddColumn(typeof(HudStaticText), 0, null);

				HudList.HudListRowAccessor newRow = corpseList.AddRow();
				((HudStaticText)newRow[0]).Text = "Time";
				((HudStaticText)newRow[1]).Text = "Name";
				((HudStaticText)newRow[2]).Text = "Coords";
				((HudStaticText)newRow[3]).Text = "Id";

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

		void corpseTracker_ItemAdded(TrackedCorpse trackedCorpse)
		{
			try
			{
				try
				{
					HudList.HudListRowAccessor newRow = corpseList.InsertRow(1);

					((HudStaticText)newRow[0]).Text = trackedCorpse.TimeStamp.ToString("ddd hh:mm tt");

					((HudStaticText)newRow[1]).Text = trackedCorpse.Description;

					CoordsObject newCords = Mag.Shared.Util.GetCoords(trackedCorpse.LandBlock, trackedCorpse.LocationX, trackedCorpse.LocationY);
					((HudStaticText)newRow[2]).Text = newCords.ToString();

					((HudStaticText)newRow[3]).Text = trackedCorpse.Id.ToString(CultureInfo.InvariantCulture);
				}
				catch (Exception ex) { Debug.LogException(ex); }
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void corpseTracker_ItemRemoved(TrackedCorpse obj)
		{
			try
			{
				for (int row = 1; row <= corpseList.RowCount; row++)
				{
					if (((HudStaticText)corpseList[row - 1][3]).Text == obj.Id.ToString(CultureInfo.InvariantCulture))
					{
						corpseList.RemoveRow(row - 1);

						row--;
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
