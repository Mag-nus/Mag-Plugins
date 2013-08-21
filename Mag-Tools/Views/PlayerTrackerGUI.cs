using System;
using System.Globalization;

using MagTools.Trackers;
using MagTools.Trackers.Player;

using Mag.Shared;

using VirindiViewService.Controls;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Views
{
	class PlayerTrackerGUI : IDisposable
	{
		readonly ITracker<TrackedPlayer> tracker;
		readonly HudList hudList;

		public PlayerTrackerGUI(ITracker<TrackedPlayer> tracker, HudList hudList)
		{
			try
			{
				this.tracker = tracker;
				this.hudList = hudList;

				hudList.ClearColumnsAndRows();

				hudList.AddColumn(typeof(HudStaticText), 75, null);
				hudList.AddColumn(typeof(HudStaticText), 140, null);
				hudList.AddColumn(typeof(HudStaticText), 100, null);
				hudList.AddColumn(typeof(HudStaticText), 0, null);

				HudList.HudListRowAccessor newRow = hudList.AddRow();
				((HudStaticText)newRow[0]).Text = "Time";
				((HudStaticText)newRow[1]).Text = "Name";
				((HudStaticText)newRow[2]).Text = "Coords";
				((HudStaticText)newRow[3]).Text = "Id";

				tracker.ItemAdded += new Action<TrackedPlayer>(playerTracker_ItemAdded);
				tracker.ItemChanged += new Action<TrackedPlayer>(playerTracker_ItemChanged);
				tracker.ItemRemoved += new Action<TrackedPlayer>(playerTracker_ItemRemoved);

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
					tracker.ItemAdded -= new Action<TrackedPlayer>(playerTracker_ItemAdded);
					tracker.ItemChanged -= new Action<TrackedPlayer>(playerTracker_ItemChanged);
					tracker.ItemRemoved -= new Action<TrackedPlayer>(playerTracker_ItemRemoved);

					hudList.Click -= new HudList.delClickedControl(hudList_Click);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void playerTracker_ItemAdded(TrackedPlayer item)
		{
			try
			{
				HudList.HudListRowAccessor newRow = hudList.InsertRow(1);

				((HudStaticText)newRow[1]).Text = item.Name;

				playerTracker_ItemChanged(item);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void playerTracker_ItemChanged(TrackedPlayer item)
		{
			try
			{
				for (int row = 1; row <= hudList.RowCount; row++)
				{
					if (((HudStaticText)hudList[row - 1][1]).Text == item.Name)
					{
						((HudStaticText)hudList[row - 1][0]).Text = item.LastSeen.ToString("yy/MM/dd HH:mm");

						CoordsObject newCords = Mag.Shared.Util.GetCoords(item.LandBlock, item.LocationX, item.LocationY);
						((HudStaticText)hudList[row - 1][2]).Text = newCords.ToString();

						((HudStaticText)hudList[row - 1][3]).Text = item.Id.ToString(CultureInfo.InvariantCulture);

						SortList();
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void playerTracker_ItemRemoved(TrackedPlayer item)
		{
			try
			{
				for (int row = 1; row <= hudList.RowCount; row++)
				{
					if (((HudStaticText)hudList[row - 1][1]).Text == item.Name)
					{
						hudList.RemoveRow(row - 1);

						row--;
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private void SortList()
		{
			if (hudList.RowCount < 1)
				return;

			for (int row = 1; row < hudList.RowCount - 1; row++)
			{
				for (int compareRow = row + 1; compareRow < hudList.RowCount; compareRow++)
				{
					string rowName = ((HudStaticText)hudList[row][1]).Text;
					DateTime rowDateTime;

					//if (!DateTime.TryParse(((HudStaticText)playerList[row][0]).Text, out rowDateTime))
					//	break;

					if (!DateTime.TryParseExact(((HudStaticText)hudList[row][0]).Text, "yy/MM/dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out rowDateTime))
						break;

					string compareName = ((HudStaticText)hudList[compareRow][1]).Text;
					DateTime compareDateTime;

					//if (!DateTime.TryParse(((HudStaticText)playerList[compareRow][0]).Text, out compareDateTime))
					//	continue;

					if (!DateTime.TryParseExact(((HudStaticText)hudList[compareRow][0]).Text, "yy/MM/dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out compareDateTime))
						break;

					if (rowDateTime < compareDateTime || (rowDateTime == compareDateTime && String.Compare(rowName, compareName, StringComparison.Ordinal) > 0))
					{
						string obj1 = ((HudStaticText)hudList[row][0]).Text;
						((HudStaticText)hudList[row][0]).Text = ((HudStaticText)hudList[compareRow][0]).Text;
						((HudStaticText)hudList[compareRow][0]).Text = obj1;

						string obj2 = ((HudStaticText)hudList[row][1]).Text;
						((HudStaticText)hudList[row][1]).Text = ((HudStaticText)hudList[compareRow][1]).Text;
						((HudStaticText)hudList[compareRow][1]).Text = obj2;

						string obj3 = ((HudStaticText)hudList[row][2]).Text;
						((HudStaticText)hudList[row][2]).Text = ((HudStaticText)hudList[compareRow][2]).Text;
						((HudStaticText)hudList[compareRow][2]).Text = obj3;
					}
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
