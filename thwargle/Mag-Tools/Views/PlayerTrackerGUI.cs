using System;
using System.Collections.Generic;
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
		readonly IItemTracker<TrackedPlayer> tracker;
		readonly HudList hudList;

		Dictionary<string, DateTime> lastSeenTimes = new Dictionary<string, DateTime>();
		DateTime lastSortTime = DateTime.MinValue;

		public PlayerTrackerGUI(IItemTracker<TrackedPlayer> tracker, HudList hudList)
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

				tracker.ItemsAdded += new Action<ICollection<TrackedPlayer>>(playerTracker_ItemsAdded);
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
					tracker.ItemsAdded -= new Action<ICollection<TrackedPlayer>>(playerTracker_ItemsAdded);
					tracker.ItemChanged -= new Action<TrackedPlayer>(playerTracker_ItemChanged);
					tracker.ItemRemoved -= new Action<TrackedPlayer>(playerTracker_ItemRemoved);

					hudList.Click -= new HudList.delClickedControl(hudList_Click);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void playerTracker_ItemsAdded(ICollection<TrackedPlayer> items)
		{
			try
			{
				foreach (var item in items)
				{
					HudList.HudListRowAccessor newRow = hudList.InsertRow(1);

					((HudStaticText) newRow[1]).Text = item.Name;

					UpdateItem(item);
				}

				SortList();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void playerTracker_ItemChanged(TrackedPlayer item)
		{
			try
			{
				UpdateItem(item);

				SortList();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void UpdateItem(TrackedPlayer item)
		{
			if (lastSeenTimes.ContainsKey(item.Name))
				lastSeenTimes[item.Name] = item.LastSeen;
			else
				lastSeenTimes.Add(item.Name, item.LastSeen);

			for (int row = 1; row <= hudList.RowCount; row++)
			{
				if (((HudStaticText)hudList[row - 1][1]).Text == item.Name)
				{
					((HudStaticText)hudList[row - 1][0]).Text = item.LastSeen.ToString("yy/MM/dd HH:mm");

					CoordsObject newCords = Mag.Shared.Util.GetCoords(item.LandBlock, item.LocationX, item.LocationY);
					((HudStaticText)hudList[row - 1][2]).Text = newCords.ToString();

					((HudStaticText)hudList[row - 1][3]).Text = item.Id.ToString(CultureInfo.InvariantCulture);
				}
			}
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

						if (lastSeenTimes.ContainsKey(item.Name))
							lastSeenTimes.Remove(item.Name);
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private void SortList()
		{
			if (hudList.RowCount < 1)
				return;

			// Some people report lag while using Mag-Tools in populated areas.
			// This will keep the list from sorting less too often
			if (DateTime.Now - lastSortTime <= TimeSpan.FromSeconds(10))
				return;

			for (int row = 1; row < hudList.RowCount - 1; row++)
			{
				for (int compareRow = row + 1; compareRow < hudList.RowCount; compareRow++)
				{
					string rowName = ((HudStaticText)hudList[row][1]).Text;
					DateTime rowDateTime = lastSeenTimes[rowName];

					string compareName = ((HudStaticText)hudList[compareRow][1]).Text;
					DateTime compareDateTime = lastSeenTimes[compareName];

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

			lastSortTime = DateTime.Now;
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
