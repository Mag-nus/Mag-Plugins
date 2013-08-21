using System;
using System.Globalization;

using MagTools.Trackers;
using MagTools.Trackers.Corpse;

using Mag.Shared;

using VirindiViewService.Controls;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Views
{
	class CorpseTrackerGUI : IDisposable
	{
		readonly ITracker<TrackedCorpse> tracker;
		readonly HudList hudList;

		public CorpseTrackerGUI(ITracker<TrackedCorpse> tracker, HudList hudList)
		{
			try
			{
				this.tracker = tracker;
				this.hudList = hudList;

				hudList.ClearColumnsAndRows();

				hudList.AddColumn(typeof(HudStaticText), 53, null);
				hudList.AddColumn(typeof(HudStaticText), 162, null);
				hudList.AddColumn(typeof(HudStaticText), 100, null);
				hudList.AddColumn(typeof(HudStaticText), 0, null);

				HudList.HudListRowAccessor newRow = hudList.AddRow();
				((HudStaticText)newRow[0]).Text = "Time";
				((HudStaticText)newRow[1]).Text = "Name";
				((HudStaticText)newRow[2]).Text = "Coords";
				((HudStaticText)newRow[3]).Text = "Id";

				tracker.ItemAdded += new Action<TrackedCorpse>(corpseTracker_ItemAdded);
				tracker.ItemChanged += new Action<TrackedCorpse>(corpseTracker_ItemChanged);
				tracker.ItemRemoved += new Action<TrackedCorpse>(corpseTracker_ItemRemoved);

				hudList.Click += new HudList.delClickedControl(hudList_Click);
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
					tracker.ItemAdded -= new Action<TrackedCorpse>(corpseTracker_ItemAdded);
					tracker.ItemChanged -= new Action<TrackedCorpse>(corpseTracker_ItemChanged);
					tracker.ItemRemoved -= new Action<TrackedCorpse>(corpseTracker_ItemRemoved);

					hudList.Click -= new HudList.delClickedControl(hudList_Click);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void corpseTracker_ItemAdded(TrackedCorpse item)
		{
			try
			{
				if (item.Opened)
					return;

				HudList.HudListRowAccessor newRow = hudList.InsertRow(1);

				((HudStaticText)newRow[0]).Text = item.TimeStamp.ToString("ddd HH:mm");

				((HudStaticText)newRow[1]).Text = item.Description;

				CoordsObject newCords = Mag.Shared.Util.GetCoords(item.LandBlock, item.LocationX, item.LocationY);
				((HudStaticText)newRow[2]).Text = newCords.ToString();

				((HudStaticText)newRow[3]).Text = item.Id.ToString(CultureInfo.InvariantCulture);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void corpseTracker_ItemChanged(TrackedCorpse item)
		{
			try
			{
				if (item.Opened)
					RemoveCorpse(item);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void corpseTracker_ItemRemoved(TrackedCorpse item)
		{
			try
			{
				RemoveCorpse(item);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void RemoveCorpse(TrackedCorpse item)
		{
			for (int row = 1; row <= hudList.RowCount; row++)
			{
				if (((HudStaticText)hudList[row - 1][3]).Text == item.Id.ToString(CultureInfo.InvariantCulture))
				{
					hudList.RemoveRow(row - 1);

					row--;
				}
			}			
		}

		void hudList_Click(object sender, int row, int col)
		{
			try
			{
				int id;

				if (int.TryParse(((HudStaticText)hudList[row][3]).Text, out id))
					CoreManager.Current.Actions.SelectItem(id);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
