using System;
using System.Globalization;
using System.Windows.Forms;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

using MagTools.Trackers.Combat;
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
		readonly CombatTracker combatTracker;

		readonly Timer hudUpdateTimer = new Timer();

		public HUD(EquipmentTracker equipmentTracker, InventoryTracker inventoryTracker, ProfitLossTracker profitLossTracker, CombatTracker combatTracker)
		{
			try
			{
				this.equipmentTracker = equipmentTracker;
				this.inventoryTracker = inventoryTracker;
				this.profitLossTracker = profitLossTracker;
				this.combatTracker = combatTracker;

				profitLossTracker.ItemChanged += new Action<TrackedProfitLoss>(profitLossTracker_ItemChanged);

				hudUpdateTimer.Tick += new EventHandler(hudUpdateTimer_Tick);
				hudUpdateTimer.Interval = 1000;
				hudUpdateTimer.Start();

				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Mana", "");
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Comps Time 1h", "");
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Net Profit 5m", "");
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Net Profit 1h", "");
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "DPS Out 1m", "");
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "DPS Out 5m", "");
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "DPS Out 1h", "");
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "DPS In 1m", "");
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "DPS In 5m", "");
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "DPS In 1h", "");
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Players", "");
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Monsters", "");
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Pack Slots", "");
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "ID Queue", "I");
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

				var nextItemToBeDepleted = inventoryTracker.NextItemToBeDepleted(TimeSpan.FromHours(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Comps Time 1h", (nextItemToBeDepleted == null) ? "" : nextItemToBeDepleted.GetTimeToDepletion(TimeSpan.FromHours(1)).TotalHours.ToString("N1") + "h");

				var freePackSlots = Util.GetFreePackSlots(CoreManager.Current.CharacterFilter.Id);
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Pack Slots", (freePackSlots == 0) ? "" : freePackSlots.ToString(CultureInfo.InvariantCulture));

				// DPS Given
				var dpsGivenOverOneMinute = combatTracker.GetDamageGivenOverTime(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "DPS Out 1m", (dpsGivenOverOneMinute == 0) ? "" : dpsGivenOverOneMinute.ToString("N0"));

				var dpsGivenOverFiveMinutes = combatTracker.GetDamageGivenOverTime(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "DPS Out 5m", (dpsGivenOverFiveMinutes == 0) ? "" : dpsGivenOverFiveMinutes.ToString("N0"));

				var dpsGivenOverOneHour = combatTracker.GetDamageGivenOverTime(TimeSpan.FromHours(1), TimeSpan.FromSeconds(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "DPS Out 1h", (dpsGivenOverOneHour == 0) ? "" : dpsGivenOverOneHour.ToString("N0"));

				// DPS Received
				var dpsReceivedOverOneMinute = combatTracker.GetDamageReceivedOverTime(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "DPS In 1m", (dpsReceivedOverOneMinute == 0) ? "" : dpsReceivedOverOneMinute.ToString("N0"));

				var dpsReceivedOverFiveMinutes = combatTracker.GetDamageReceivedOverTime(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "DPS In 5m", (dpsReceivedOverFiveMinutes == 0) ? "" : dpsReceivedOverFiveMinutes.ToString("N0"));

				var dpsReceivedOverOneHour = combatTracker.GetDamageReceivedOverTime(TimeSpan.FromHours(1), TimeSpan.FromSeconds(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "DPS In 1h", (dpsReceivedOverOneHour == 0) ? "" : dpsReceivedOverOneHour.ToString("N0"));

				// Area Items
				int playerCount = -1;
				int monsterCount = 0;
				
				foreach (var wo in CoreManager.Current.WorldFilter.GetLandscape())
				{
					if (wo.ObjectClass == ObjectClass.Player)
						playerCount++;

					if (wo.ObjectClass == ObjectClass.Monster)
						monsterCount++;
				}

				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Players", (playerCount <= 0) ? "" : playerCount.ToString(CultureInfo.InvariantCulture));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Monsters", (monsterCount == 0) ? "" : monsterCount.ToString(CultureInfo.InvariantCulture));

				// Game Info
				var itemsInIDQueue = CoreManager.Current.IDQueue.ActionCount;
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "ID Queue", (itemsInIDQueue == 0) ? "" : itemsInIDQueue.ToString(CultureInfo.InvariantCulture));

			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void profitLossTracker_ItemChanged(TrackedProfitLoss item)
		{
			try
			{
				if (item.Name == "Net Profit")
				{
					double valuePerHourOverFiveMinutes = item.GetValueDifference(TimeSpan.FromMinutes(5), TimeSpan.FromHours(1));
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Net Profit 5m", valuePerHourOverFiveMinutes == 0 ? String.Empty : (valuePerHourOverFiveMinutes / 250000).ToString("N1") + "/h");

					double valuePerHourOverOneHour = item.GetValueDifference(TimeSpan.FromHours(1), TimeSpan.FromHours(1));
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", "Net Profit 1h", valuePerHourOverOneHour == 0 ? String.Empty : (valuePerHourOverOneHour / 250000).ToString("N1") + "/h");
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
