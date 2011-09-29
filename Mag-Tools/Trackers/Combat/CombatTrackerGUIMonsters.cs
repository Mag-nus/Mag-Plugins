using System;
using System.Collections.Generic;

using VirindiViewService.Controls;

namespace MagTools.Trackers.Combat
{
	class CombatTrackerGUIMonsters
	{
		ICombatTracker combatTracker;
		HudList monsterList;
		CombatTrackerGUIInfo combatTrackerGUIInfo;

		Dictionary<string, TrackedCombat> combatInfoByMonster = new Dictionary<string, TrackedCombat>();
		int selectedRow = 0;

		public CombatTrackerGUIMonsters(ICombatTracker combatTracker, HudList monsterList, CombatTrackerGUIInfo combatTrackerGUIInfo)
		{
			try
			{
				this.combatTracker = combatTracker;
				this.monsterList = monsterList;
				this.combatTrackerGUIInfo = combatTrackerGUIInfo;

				monsterList.ClearColumnsAndRows();

				monsterList.AddColumn(typeof(HudStaticText), 5, null);
				monsterList.AddColumn(typeof(HudStaticText), 153, null);
				monsterList.AddColumn(typeof(HudStaticText), 55, null); // This cannot go any smaller without pruning text
				monsterList.AddColumn(typeof(HudStaticText), 72, null);

				HudList.HudListRowAccessor newRow = monsterList.AddRow();
				((HudStaticText)newRow[2]).Text = "Dmg Rcvd";
				((HudStaticText)newRow[2]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).Text = "Dmg Givn";
				((HudStaticText)newRow[3]).TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = monsterList.AddRow();
				((HudStaticText)newRow[0]).Text = "*";
				((HudStaticText)newRow[1]).Text = "All";
				((HudStaticText)newRow[2]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).TextAlignment = VirindiViewService.WriteTextFormats.Right;

				combatInfoByMonster.Clear();
				combatInfoByMonster.Add("All", new TrackedCombat());

				selectedRow = 1;

				monsterList.Click += new HudList.delClickedControl(monsterList_Click);

				combatTracker.CombatEvent += new Action<CombatEventArgs>(combatTracker_CombatEvent);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private bool _disposed = false;

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
			if (!_disposed)
			{
				if (disposing)
				{
					monsterList.Click -= new HudList.delClickedControl(monsterList_Click);

					combatTracker.CombatEvent -= new Action<CombatEventArgs>(combatTracker_CombatEvent);
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}

		void monsterList_Click(object sender, int row, int col)
		{
			try
			{
				if (selectedRow == row)
					return;

				if (row == 0)
					row = 1;

				((HudStaticText)monsterList[selectedRow][0]).Text = "";

				selectedRow = row;

				((HudStaticText)monsterList[selectedRow][0]).Text = "*";

				if (selectedRow == 0 || selectedRow == 1)
					combatTrackerGUIInfo.LoadFromTrackedCombat(combatInfoByMonster["All"]);
				else if (combatInfoByMonster.ContainsKey(((HudStaticText)monsterList[selectedRow][1]).Text))
					combatTrackerGUIInfo.LoadFromTrackedCombat(combatInfoByMonster[((HudStaticText)monsterList[selectedRow][1]).Text]);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void combatTracker_CombatEvent(CombatEventArgs e)
		{
			try
			{
				if (!combatInfoByMonster.ContainsKey(e.MonsterName))
				{
					combatInfoByMonster.Add(e.MonsterName, new TrackedCombat());

					HudList.HudListRowAccessor newRow = monsterList.AddRow();
					((HudStaticText)newRow[1]).Text = e.MonsterName;
					((HudStaticText)newRow[2]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
					((HudStaticText)newRow[3]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				}

				TrackedCombat all = combatInfoByMonster["All"];
				all.AddFromCombatEventArgs(e);

				TrackedCombat monster = combatInfoByMonster[e.MonsterName];
				monster.AddFromCombatEventArgs(e);

				if (selectedRow == 0 || selectedRow == 1)
					combatTrackerGUIInfo.LoadFromTrackedCombat(all);
				else if (((HudStaticText)monsterList[selectedRow][1]).Text == e.MonsterName)
					combatTrackerGUIInfo.LoadFromTrackedCombat(monster);

				if (all[AttackDirection.PlayerReceived].TotalDamage != 0)
					((HudStaticText)monsterList[1][2]).Text = all[AttackDirection.PlayerReceived].TotalDamage.ToString(("#,##0"));
				if (all[AttackDirection.PlayerInitiated].TotalDamage != 0)
					((HudStaticText)monsterList[1][3]).Text = all[AttackDirection.PlayerInitiated].TotalDamage.ToString(("#,##0"));

				for (int row = 2 ; row < monsterList.RowCount ; row++)
				{
					if (((HudStaticText)monsterList[row][1]).Text == e.MonsterName)
					{
						if (monster[AttackDirection.PlayerReceived].TotalDamage != 0)
							((HudStaticText)monsterList[row][2]).Text = monster[AttackDirection.PlayerReceived].TotalDamage.ToString(("#,##0"));
						if (monster[AttackDirection.PlayerInitiated].TotalDamage != 0)
							((HudStaticText)monsterList[row][3]).Text = monster[AttackDirection.PlayerInitiated].TotalDamage.ToString(("#,##0"));

						break;
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
