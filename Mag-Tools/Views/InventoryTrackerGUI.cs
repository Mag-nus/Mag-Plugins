using System;
using System.Collections.Generic;
using System.Globalization;
using Decal.Adapter.Wrappers;

using MagTools.Trackers;
using MagTools.Trackers.Inventory;
using MagTools.Trackers.ProfitLoss;

using Mag.Shared;

using VirindiViewService.Controls;

namespace MagTools.Views
{
	class InventoryTrackerGUI : IDisposable
	{
		readonly IItemTracker<TrackedProfitLoss> profitLossTracker;
		readonly IItemTracker<TrackedInventory> inventoryTracker;
		readonly HudList hudList;

		public InventoryTrackerGUI(IItemTracker<TrackedProfitLoss> profitLossTracker, IItemTracker<TrackedInventory> inventoryTracker, HudList hudList)
		{
			try
			{
				this.profitLossTracker = profitLossTracker;
				this.inventoryTracker = inventoryTracker;
				this.hudList = hudList;

				hudList.ClearColumnsAndRows();

				hudList.AddColumn(typeof(HudPictureBox), 16, null);
				hudList.AddColumn(typeof(HudStaticText), 55, null);
				hudList.AddColumn(typeof(HudStaticText), 35, null);
				hudList.AddColumn(typeof(HudStaticText), 65, null);
				hudList.AddColumn(typeof(HudStaticText), 65, null);
				hudList.AddColumn(typeof(HudStaticText), 40, null);

				HudList.HudListRowAccessor newRow = hudList.AddRow();
				((HudStaticText)newRow[3]).Text = "MMD/h ~5m";
				((HudStaticText)newRow[3]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[4]).Text = "MMD/h ~1h";
				((HudStaticText)newRow[4]).TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = hudList.AddRow();
				((HudStaticText)newRow[1]).Text = "Peas";

				newRow = hudList.AddRow();
				((HudStaticText)newRow[1]).Text = "Comps";

				newRow = hudList.AddRow();
				((HudStaticText)newRow[1]).Text = "Salvage";

				newRow = hudList.AddRow();
				((HudStaticText)newRow[1]).Text = "Net Profit";

				hudList.AddRow();

				newRow = hudList.AddRow();
				((HudStaticText)newRow[2]).Text = "Count";
				((HudStaticText)newRow[2]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).Text = "Avg/h ~5m";
				((HudStaticText)newRow[3]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[4]).Text = "Avg/h ~1h";
				((HudStaticText)newRow[4]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[5]).Text = "(Hrs)";
				((HudStaticText)newRow[5]).TextAlignment = VirindiViewService.WriteTextFormats.Right;

				profitLossTracker.ItemsAdded += new Action<ICollection<TrackedProfitLoss>>(profitLossTracker_ItemsAdded);
				profitLossTracker.ItemChanged += new Action<TrackedProfitLoss>(profitLossTracker_ItemChanged);
				profitLossTracker.ItemRemoved += new Action<TrackedProfitLoss>(profitLossTracker_ItemRemoved);

				inventoryTracker.ItemsAdded += new Action<ICollection<TrackedInventory>>(consumableTracker_ItemsAdded);
				inventoryTracker.ItemChanged += new Action<TrackedInventory>(consumableTracker_ItemChanged);
				inventoryTracker.ItemRemoved += new Action<TrackedInventory>(consumableTracker_ItemRemoved);
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
					profitLossTracker.ItemsAdded -= new Action<ICollection<TrackedProfitLoss>>(profitLossTracker_ItemsAdded);
					profitLossTracker.ItemChanged -= new Action<TrackedProfitLoss>(profitLossTracker_ItemChanged);
					profitLossTracker.ItemRemoved -= new Action<TrackedProfitLoss>(profitLossTracker_ItemRemoved);

					inventoryTracker.ItemsAdded -= new Action<ICollection<TrackedInventory>>(consumableTracker_ItemsAdded);
					inventoryTracker.ItemChanged -= new Action<TrackedInventory>(consumableTracker_ItemChanged);
					inventoryTracker.ItemRemoved -= new Action<TrackedInventory>(consumableTracker_ItemRemoved);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void profitLossTracker_ItemsAdded(ICollection<TrackedProfitLoss> items)
		{
			try
			{
				foreach (var item in items)
					UpdateProfitLossItem(item);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void profitLossTracker_ItemChanged(TrackedProfitLoss item)
		{
			try
			{
				UpdateProfitLossItem(item);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void UpdateProfitLossItem(TrackedProfitLoss item)
		{
			int row;

			if (item.Name == "Peas") row = 1;
			else if (item.Name == "Comps") row = 2;
			else if (item.Name == "Salvage") row = 3;
			else if (item.Name == "Net Profit") row = 4;
			else return;

			double valuePerHourOverFiveMinutes = item.GetValueDifference(TimeSpan.FromMinutes(5), TimeSpan.FromHours(1));
			((HudStaticText)hudList[row][3]).Text = valuePerHourOverFiveMinutes == 0 ? String.Empty : (valuePerHourOverFiveMinutes / 250000).ToString("N1");

			double valuePerHourOverOneHour = item.GetValueDifference(TimeSpan.FromHours(1), TimeSpan.FromHours(1));
			((HudStaticText)hudList[row][4]).Text = valuePerHourOverOneHour == 0 ? String.Empty : (valuePerHourOverOneHour / 250000).ToString("N1");
		}

		void profitLossTracker_ItemRemoved(TrackedProfitLoss item)
		{
		}

		int GetZIndex(ObjectClass objectClass)
		{
			if (objectClass == ObjectClass.SpellComponent) return 0;
			if (objectClass == ObjectClass.ManaStone) return 1;
			if (objectClass == ObjectClass.HealingKit) return 2;
			if (objectClass == ObjectClass.TradeNote) return 3;
			if (objectClass == ObjectClass.Salvage) return 4;

			return int.MaxValue;
		}

		readonly List<TrackedInventory> cachedInventory = new List<TrackedInventory>();

		void consumableTracker_ItemsAdded(ICollection<TrackedInventory> items)
		{
			try
			{
				foreach (var item in items)
				{
					int zIndex = GetZIndex(item.ObjectClass);

					for (int row = 7 ; row <= hudList.RowCount ; row++)
					{
						if (row == hudList.RowCount)
						{
							HudList.HudListRowAccessor newRow = hudList.AddRow();

							((HudPictureBox)newRow[0]).Image = item.Icon + 0x6000000;
							((HudStaticText)newRow[1]).Text = item.Name;

							((HudStaticText)newRow[2]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
							((HudStaticText)newRow[3]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
							((HudStaticText)newRow[4]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
							((HudStaticText)newRow[5]).TextAlignment = VirindiViewService.WriteTextFormats.Right;

							cachedInventory.Add(item);

							break;
						}

						if ((cachedInventory[row - 7].ObjectClass == item.ObjectClass && cachedInventory[row - 7].ItemValue < item.ItemValue) || (GetZIndex(cachedInventory[row - 7].ObjectClass) > zIndex))
						{
							HudList.HudListRowAccessor newRow = hudList.InsertRow(row);

							((HudPictureBox)newRow[0]).Image = item.Icon + 0x6000000;
							((HudStaticText)newRow[1]).Text = item.Name;

							((HudStaticText)newRow[2]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
							((HudStaticText)newRow[3]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
							((HudStaticText)newRow[4]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
							((HudStaticText)newRow[5]).TextAlignment = VirindiViewService.WriteTextFormats.Right;

							cachedInventory.Insert(row - 7, item);

							break;
						}
					}

					UpdateInventoryItem(item);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void consumableTracker_ItemChanged(TrackedInventory item)
		{
			try
			{
				UpdateInventoryItem(item);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void UpdateInventoryItem(TrackedInventory item)
		{
			for (int row = 7; row <= hudList.RowCount; row++)
			{
				if (((HudStaticText)hudList[row - 1][1]).Text == item.Name)
				{
					((HudStaticText)hudList[row - 1][2]).Text = item.LastKnownValue.ToString(CultureInfo.InvariantCulture);

					if (item.LastKnownValue == 0)
					{
						((HudStaticText)hudList[row - 1][3]).Text = String.Empty;
						((HudStaticText)hudList[row - 1][4]).Text = String.Empty;
						((HudStaticText)hudList[row - 1][5]).Text = String.Empty;
					}
					else
					{
						var oneHourItemCountDifferenceOverFiveMinutes = item.GetValueDifference(TimeSpan.FromMinutes(5), TimeSpan.FromHours(1));
						((HudStaticText)hudList[row - 1][3]).Text = oneHourItemCountDifferenceOverFiveMinutes == 0 ? String.Empty : oneHourItemCountDifferenceOverFiveMinutes.ToString("N1");

						var oneHourItemCountDifferenceOverOneHour = item.GetValueDifference(TimeSpan.FromHours(1), TimeSpan.FromHours(1));
						((HudStaticText)hudList[row - 1][4]).Text = oneHourItemCountDifferenceOverOneHour == 0 ? String.Empty : oneHourItemCountDifferenceOverOneHour.ToString("N1");

						var hoursRemaining = item.GetTimeToDepletion(TimeSpan.FromHours(1));
						((HudStaticText)hudList[row - 1][5]).Text = (hoursRemaining == TimeSpan.Zero || hoursRemaining == TimeSpan.MaxValue) ? String.Empty : hoursRemaining.TotalHours.ToString("N1");
					}
				}
			}

		}

		void consumableTracker_ItemRemoved(TrackedInventory item)
		{
			try
			{
				for (int row = 7; row <= hudList.RowCount; row++)
				{
					if (((HudStaticText)hudList[row - 1][1]).Text == item.Name)
					{
						hudList.RemoveRow(row - 1);

						row--;
					}
				}

				cachedInventory.Remove(item);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
