using System;
using System.Globalization;

using Decal.Adapter;

using MagTools.Trackers.Equipment;
using MagTools.Trackers.Inventory;
using MagTools.Trackers.ProfitLoss;

using Mag.Shared;

namespace MagTools.Views
{
	class HUD : IDisposable
	{
		readonly EquipmentTracker equipmentTracker;
		readonly InventoryTracker inventoryTracker;
		readonly ProfitLossTracker profitLossTracker;

		readonly System.Windows.Forms.Timer hudUpdateTimer = new System.Windows.Forms.Timer();

		public HUD(EquipmentTracker equipmentTracker, InventoryTracker inventoryTracker, ProfitLossTracker profitLossTracker)
		{
			try
			{
				this.equipmentTracker = equipmentTracker;
				this.inventoryTracker = inventoryTracker;
				this.profitLossTracker = profitLossTracker;

				profitLossTracker.ItemChanged += new Action<TrackedProfitLoss>(profitLossTracker_ItemChanged);

				hudUpdateTimer.Tick += new EventHandler(hudUpdateTimer_Tick);
				hudUpdateTimer.Interval = 1000;
				hudUpdateTimer.Start();
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
					profitLossTracker.ItemChanged -= new Action<TrackedProfitLoss>(profitLossTracker_ItemChanged);

					hudUpdateTimer.Tick -= new EventHandler(hudUpdateTimer_Tick);
					hudUpdateTimer.Dispose();
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void hudUpdateTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (equipmentTracker.RemainingTimeBeforeNextEmptyItem == TimeSpan.MaxValue)
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Mana", "");
				else
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Mana", (equipmentTracker.NumberOfInactiveItems > 0 ? "*" : "") + string.Format("{0:d}h{1:d2}m", (int)equipmentTracker.RemainingTimeBeforeNextEmptyItem.TotalHours, equipmentTracker.RemainingTimeBeforeNextEmptyItem.Minutes));

				var itemsInIDQueue = CoreManager.Current.IDQueue.ActionCount;
				if (itemsInIDQueue == 0)
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "ID Queue", "");
				else
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "ID Queue", itemsInIDQueue.ToString(CultureInfo.InvariantCulture));

				var nextItemToBeDepleted = inventoryTracker.NextItemToBeDepleted(TimeSpan.FromHours(1));
				if (nextItemToBeDepleted == null)
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Comps Time", "");
				else
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Comps Time", nextItemToBeDepleted.GetTimeToDepletion(TimeSpan.FromHours(1)).TotalHours.ToString("N1") + "h");

				var freePackSlots = Util.GetFreePackSlots(CoreManager.Current.CharacterFilter.Id);
				if (freePackSlots == 0)
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Pack Slots", "");
				else
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Pack Slots", freePackSlots.ToString(CultureInfo.InvariantCulture));

			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void profitLossTracker_ItemChanged(TrackedProfitLoss item)
		{
			try
			{
				if (item.Name == "Net Profit")
				{
					double valuePerHourOverOneHour = item.GetValueDifference(TimeSpan.FromHours(1), TimeSpan.FromHours(1));
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Net Profit", valuePerHourOverOneHour == 0 ? String.Empty : (valuePerHourOverOneHour / 250000).ToString("N1") + "/h");
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
