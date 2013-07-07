using System;
using System.Globalization;

using Decal.Adapter.Wrappers;

using MagTools.Trackers;
using MagTools.Trackers.Player;

using VirindiViewService.Controls;

namespace MagTools.Views
{
	class PlayerTrackerGUI : IDisposable
	{
		readonly ITracker<TrackedPlayer> playerTracker;
		readonly HudList playerList;

		public PlayerTrackerGUI(ITracker<TrackedPlayer> playerTracker, HudList playerList)
		{
			try
			{
				this.playerTracker = playerTracker;
				this.playerList = playerList;

				playerList.ClearColumnsAndRows();

				playerList.AddColumn(typeof(HudStaticText), 100, null);
				playerList.AddColumn(typeof(HudStaticText), 115, null);
				playerList.AddColumn(typeof(HudStaticText), 100, null);

				HudList.HudListRowAccessor newRow = playerList.AddRow();
				((HudStaticText)newRow[0]).Text = "Time";
				((HudStaticText)newRow[1]).Text = "Name";
				((HudStaticText)newRow[2]).Text = "Coords";

				playerTracker.ItemAdded += new Action<TrackedPlayer>(playerTracker_ItemAdded);
				playerTracker.ItemChanged += new Action<TrackedPlayer>(playerTracker_ItemChanged);
				playerTracker.ItemRemoved += new Action<TrackedPlayer>(playerTracker_ItemRemoved);
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
					playerTracker.ItemAdded -= new Action<TrackedPlayer>(playerTracker_ItemAdded);
					playerTracker.ItemChanged -= new Action<TrackedPlayer>(playerTracker_ItemChanged);
					playerTracker.ItemRemoved -= new Action<TrackedPlayer>(playerTracker_ItemRemoved);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void playerTracker_ItemAdded(TrackedPlayer trackedPlayer)
		{
			try
			{
				HudList.HudListRowAccessor newRow = playerList.InsertRow(1);

				((HudStaticText)newRow[1]).Text = trackedPlayer.Name;

				playerTracker_ItemChanged(trackedPlayer);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void playerTracker_ItemChanged(TrackedPlayer trackedPlayer)
		{
			try
			{
				for (int row = 1; row <= playerList.RowCount; row++)
				{
					if (((HudStaticText)playerList[row - 1][1]).Text == trackedPlayer.Name)
					{
						((HudStaticText)playerList[row - 1][0]).Text = trackedPlayer.LastSeen.ToString("MM/dd/yy hh:mm tt");

						CoordsObject newCords = Mag.Shared.Util.GetCoords(trackedPlayer.LandBlock, trackedPlayer.LocationX, trackedPlayer.LocationY);
						((HudStaticText)playerList[row - 1][2]).Text = newCords.ToString();

						SortList();
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void playerTracker_ItemRemoved(TrackedPlayer trackedPlayer)
		{
			try
			{
				for (int row = 1; row <= playerList.RowCount; row++)
				{
					if (((HudStaticText)playerList[row - 1][1]).Text == trackedPlayer.Name)
					{
						playerList.RemoveRow(row - 1);

						row--;
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private void SortList()
		{
			if (playerList.RowCount < 1)
				return;

			for (int row = 1; row < playerList.RowCount - 1; row++)
			{
				for (int compareRow = row + 1; compareRow < playerList.RowCount; compareRow++)
				{
					string rowName = ((HudStaticText)playerList[row][1]).Text;
					DateTime rowDateTime;

					//if (!DateTime.TryParse(((HudStaticText)playerList[row][0]).Text, out rowDateTime))
					//	break;

					if (!DateTime.TryParseExact(((HudStaticText)playerList[row][0]).Text, "MM/dd/yy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out rowDateTime))
						break;

					string compareName = ((HudStaticText)playerList[compareRow][1]).Text;
					DateTime compareDateTime;

					//if (!DateTime.TryParse(((HudStaticText)playerList[compareRow][0]).Text, out compareDateTime))
					//	continue;

					if (!DateTime.TryParseExact(((HudStaticText)playerList[compareRow][0]).Text, "MM/dd/yy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out compareDateTime))
						break;

					if (rowDateTime < compareDateTime || (rowDateTime == compareDateTime && String.Compare(rowName, compareName, StringComparison.Ordinal) > 0))
					{
						string obj1 = ((HudStaticText)playerList[row][0]).Text;
						((HudStaticText)playerList[row][0]).Text = ((HudStaticText)playerList[compareRow][0]).Text;
						((HudStaticText)playerList[compareRow][0]).Text = obj1;

						string obj2 = ((HudStaticText)playerList[row][1]).Text;
						((HudStaticText)playerList[row][1]).Text = ((HudStaticText)playerList[compareRow][1]).Text;
						((HudStaticText)playerList[compareRow][1]).Text = obj2;

						string obj3 = ((HudStaticText)playerList[row][2]).Text;
						((HudStaticText)playerList[row][2]).Text = ((HudStaticText)playerList[compareRow][2]).Text;
						((HudStaticText)playerList[compareRow][2]).Text = obj3;
					}
				}
			}
		}
	}
}
