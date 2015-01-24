using System;
using System.Collections.Generic;
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

		readonly Dictionary<string, string> paddedNames = new Dictionary<string, string>();

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

				// VHUD Minimalst Black Font: Times New Roman, 8pt, weight 1
				//Font vHUDFont = new Font("Times New Roman", 8, FontStyle.Regular);

				paddedNames.Add("Mana",				"Mana         .");
				paddedNames.Add("Comps Time 1h",	"Comps Time 1h.");
				paddedNames.Add("Net Profit 5m",	"Net Profit 5m  .");
				paddedNames.Add("Net Profit 1h",	"Net Profit 1h   ");
				paddedNames.Add("DPS Out 1m",		"DPS Out 1m  .");
				paddedNames.Add("DPS Out 5m",		"DPS Out 5m  .");
				paddedNames.Add("DPS Out 1h",		"DPS Out 1h   ");
				paddedNames.Add("DPS In 1m",		"DPS In 1m    ");
				paddedNames.Add("DPS In 5m",		"DPS In 5m    ");
				paddedNames.Add("DPS In 1h",		"DPS In 1h    .");
				paddedNames.Add("Players",			"Players        ");
				paddedNames.Add("Monsters",			"Monsters       ");
				paddedNames.Add("Pack Slots",		"Pack Slots     ");
				paddedNames.Add("ID Queue",			"ID Queue      ");

				// This code doesn't work well
				/*int longest = 0;

				foreach (var name in paddedNames)
				{
					var size = TextRenderer.MeasureText(name.Key, vHUDFont);

					if (size.Width > longest)
						longest = size.Width;
				}

				var keys = new List<string>(paddedNames.Keys);
				foreach (var key in keys)
				{
					start:

					var width = TextRenderer.MeasureText(paddedNames[key], vHUDFont).Width;
					var width1 = TextRenderer.MeasureText(paddedNames[key] + " ", vHUDFont).Width;
					var width2 = TextRenderer.MeasureText(paddedNames[key] + "  ", vHUDFont).Width;

					if (Math.Abs(longest - width) < Math.Abs(longest - width1) && Math.Abs(longest - width) < Math.Abs(longest - width2))
						continue;

					if (Math.Abs(longest - width1) < Math.Abs(longest - width2))
					{
						paddedNames[key] += " ";
						continue;
					}

					paddedNames[key] += "  ";
					goto start;
				}*/

				foreach (var value in paddedNames.Values)
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", value, "");
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
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["Mana"], "");
				else
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["Mana"], (equipmentTracker.NumberOfInactiveItems > 0 ? "*" : "") + string.Format("{0:d}h{1:d2}m", (int)equipmentTracker.RemainingTimeBeforeNextEmptyItem.TotalHours, equipmentTracker.RemainingTimeBeforeNextEmptyItem.Minutes));

				var nextItemToBeDepleted = inventoryTracker.NextItemToBeDepleted(TimeSpan.FromHours(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["Comps Time 1h"], (nextItemToBeDepleted == null) ? "" : nextItemToBeDepleted.GetTimeToDepletion(TimeSpan.FromHours(1)).TotalHours.ToString("N1") + "h");

				var freePackSlots = Util.GetFreePackSlots(CoreManager.Current.CharacterFilter.Id);
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["Pack Slots"], (freePackSlots == 0) ? "" : freePackSlots.ToString(CultureInfo.InvariantCulture));

				// DPS Given
				var dpsGivenOverOneMinute = combatTracker.GetDamageGivenOverTime(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["DPS Out 1m"], (dpsGivenOverOneMinute == 0) ? "" : dpsGivenOverOneMinute.ToString("N0"));

				var dpsGivenOverFiveMinutes = combatTracker.GetDamageGivenOverTime(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["DPS Out 5m"], (dpsGivenOverFiveMinutes == 0) ? "" : dpsGivenOverFiveMinutes.ToString("N0"));

				var dpsGivenOverOneHour = combatTracker.GetDamageGivenOverTime(TimeSpan.FromHours(1), TimeSpan.FromSeconds(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["DPS Out 1h"], (dpsGivenOverOneHour == 0) ? "" : dpsGivenOverOneHour.ToString("N0"));

				// DPS Received
				var dpsReceivedOverOneMinute = combatTracker.GetDamageReceivedOverTime(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["DPS In 1m"], (dpsReceivedOverOneMinute == 0) ? "" : dpsReceivedOverOneMinute.ToString("N0"));

				var dpsReceivedOverFiveMinutes = combatTracker.GetDamageReceivedOverTime(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["DPS In 5m"], (dpsReceivedOverFiveMinutes == 0) ? "" : dpsReceivedOverFiveMinutes.ToString("N0"));

				var dpsReceivedOverOneHour = combatTracker.GetDamageReceivedOverTime(TimeSpan.FromHours(1), TimeSpan.FromSeconds(1));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["DPS In 1h"], (dpsReceivedOverOneHour == 0) ? "" : dpsReceivedOverOneHour.ToString("N0"));

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

				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["Players"], (playerCount <= 0) ? "" : playerCount.ToString(CultureInfo.InvariantCulture));
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["Monsters"], (monsterCount == 0) ? "" : monsterCount.ToString(CultureInfo.InvariantCulture));

				// Game Info
				var itemsInIDQueue = CoreManager.Current.IDQueue.ActionCount;
				VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["ID Queue"], (itemsInIDQueue == 0) ? "" : itemsInIDQueue.ToString(CultureInfo.InvariantCulture));

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
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["Net Profit 5m"], valuePerHourOverFiveMinutes == 0 ? String.Empty : (valuePerHourOverFiveMinutes / 250000).ToString("N1") + "/h");

					double valuePerHourOverOneHour = item.GetValueDifference(TimeSpan.FromHours(1), TimeSpan.FromHours(1));
					VirindiHUDs.UIs.StatusModel.UpdateEntry("Mag-Tools", paddedNames["Net Profit 1h"], valuePerHourOverOneHour == 0 ? String.Empty : (valuePerHourOverOneHour / 250000).ToString("N1") + "/h");
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
