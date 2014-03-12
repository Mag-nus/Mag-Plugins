using System;
using System.Collections.Generic;

using MagTools.Trackers.Combat;
using MagTools.Trackers.Combat.Standard;

using Mag.Shared;

using VirindiViewService.Controls;

using Decal.Adapter;

namespace MagTools.Views
{
	class CombatTrackerGUI : IDisposable
	{
		readonly CombatTracker combatTracker;
		readonly HudList monsterList;

		readonly CombatTrackerGUIInfo combatTrackerGUIInfo;

		int selectedRow;

		public CombatTrackerGUI(CombatTracker combatTracker, HudList monsterList, HudList damageInfoList)
		{
			try
			{
				if (combatTracker == null)
					return;

				this.combatTracker = combatTracker;
				this.monsterList = monsterList;

				combatTrackerGUIInfo = new CombatTrackerGUIInfo(damageInfoList);

				monsterList.ClearColumnsAndRows();

				// Each character is a max of 6 pixels wide
				monsterList.AddColumn(typeof(HudStaticText), 5, null);
				monsterList.AddColumn(typeof(HudStaticText), 111, null);
				monsterList.AddColumn(typeof(HudStaticText), 37, null);
				monsterList.AddColumn(typeof(HudStaticText), 55, null); // This cannot go any smaller without pruning text
				monsterList.AddColumn(typeof(HudStaticText), 77, null);

				HudList.HudListRowAccessor newRow = monsterList.AddRow();
				((HudStaticText)newRow[2]).Text = "KB's";
				((HudStaticText)newRow[2]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).Text = "Dmg Rcvd";
				((HudStaticText)newRow[3]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[4]).Text = "Dmg Givn";
				((HudStaticText)newRow[4]).TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = monsterList.AddRow();
				((HudStaticText)newRow[0]).Text = "*";
				((HudStaticText)newRow[1]).Text = "All";
				((HudStaticText)newRow[2]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[4]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				
				selectedRow = 1;

				monsterList.Click += new HudList.delClickedControl(monsterList_Click);

				combatTracker.InfoCleared += new Action<bool>(combatTracker_InfoCleared);
				combatTracker.StatsLoaded += new Action<List<CombatInfo>>(combatTracker_StatsLoaded);
				combatTracker.CombatInfoUpdated += new Action<CombatInfo>(combatTracker_CombatInfoUpdated);
				combatTracker.AetheriaInfoUpdated += new Action<AetheriaInfo>(combatTracker_AetheriaInfoUpdated);
				combatTracker.CloakInfoUpdated += new Action<CloakInfo>(combatTracker_CloakInfoUpdated);
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
					monsterList.Click -= new HudList.delClickedControl(monsterList_Click);

					combatTracker.InfoCleared -= new Action<bool>(combatTracker_InfoCleared);
					combatTracker.StatsLoaded -= new Action<List<CombatInfo>>(combatTracker_StatsLoaded);
					combatTracker.CombatInfoUpdated -= new Action<CombatInfo>(combatTracker_CombatInfoUpdated);
					combatTracker.AetheriaInfoUpdated -= new Action<AetheriaInfo>(combatTracker_AetheriaInfoUpdated);
					combatTracker.CloakInfoUpdated -= new Action<CloakInfo>(combatTracker_CloakInfoUpdated);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
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

				combatTrackerGUIInfo.Clear();

				if (selectedRow == 0 || selectedRow == 1)
					loadInfoForName("All");
				else
					loadInfoForName(((HudStaticText)monsterList[selectedRow][1]).Text);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void combatTracker_InfoCleared(bool obj)
		{
			combatTrackerGUIInfo.Clear();

			for (int row = monsterList.RowCount - 1 ; row >= 2 ; row--)
			{
				monsterList.RemoveRow(row);
			}

			selectedRow = 1;

			((HudStaticText)monsterList[selectedRow][0]).Text = "*";

			((HudStaticText)monsterList[selectedRow][2]).Text = "";
			((HudStaticText)monsterList[selectedRow][3]).Text = "";
			((HudStaticText)monsterList[selectedRow][4]).Text = "";
		}

		void combatTracker_StatsLoaded(List<CombatInfo> obj)
		{
			foreach (CombatInfo combatInfo in obj)
			{
				combatTracker_CombatInfoUpdated(combatInfo);
			}
		}

		void combatTracker_CombatInfoUpdated(CombatInfo obj)
		{
			AddNamesToList(obj.SourceName, obj.TargetName);

			if (!String.IsNullOrEmpty(obj.SourceName) && obj.SourceName != CoreManager.Current.CharacterFilter.Name)
				loadInfoForName(obj.SourceName);

			if (!String.IsNullOrEmpty(obj.TargetName) && obj.TargetName != CoreManager.Current.CharacterFilter.Name)
				loadInfoForName(obj.TargetName);
		}

		void combatTracker_AetheriaInfoUpdated(AetheriaInfo obj)
		{
			AddNamesToList(obj.SourceName, obj.TargetName);

			if (!String.IsNullOrEmpty(obj.SourceName) && obj.SourceName != CoreManager.Current.CharacterFilter.Name)
				loadInfoForName(obj.SourceName);

			if (!String.IsNullOrEmpty(obj.TargetName) && obj.TargetName != CoreManager.Current.CharacterFilter.Name)
				loadInfoForName(obj.TargetName);
		}

		void combatTracker_CloakInfoUpdated(CloakInfo obj)
		{
			AddNamesToList(obj.SourceName, obj.TargetName);

			if (!String.IsNullOrEmpty(obj.SourceName) && obj.SourceName != CoreManager.Current.CharacterFilter.Name)
				loadInfoForName(obj.SourceName);

			if (!String.IsNullOrEmpty(obj.TargetName) && obj.TargetName != CoreManager.Current.CharacterFilter.Name)
				loadInfoForName(obj.TargetName);
		}

		void AddNamesToList(string sourceName, string targetName)
		{
			if (String.IsNullOrEmpty(sourceName) && String.IsNullOrEmpty(targetName))
				return;

			// We don't add ourself to the list.
			if ((sourceName == CoreManager.Current.CharacterFilter.Name && sourceName == targetName) || 
				(sourceName == CoreManager.Current.CharacterFilter.Name && String.IsNullOrEmpty(targetName)) ||
				(String.IsNullOrEmpty(sourceName) && targetName == CoreManager.Current.CharacterFilter.Name))
				return;

			for (int row = 2 ; row <= monsterList.RowCount ; row++)
			{
				// Have we reached the end of the row list? If so, we haven't found this name in the list
				if (row == monsterList.RowCount)
				{
					HudList.HudListRowAccessor newRow = monsterList.AddRow();

					if (String.IsNullOrEmpty(sourceName))
						((HudStaticText)newRow[1]).Text = targetName;
					else if (String.IsNullOrEmpty(targetName))
						((HudStaticText)newRow[1]).Text = sourceName;
					else if (sourceName == CoreManager.Current.CharacterFilter.Name)
						((HudStaticText)newRow[1]).Text = targetName;
					else
						((HudStaticText)newRow[1]).Text = sourceName;

					((HudStaticText)newRow[2]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
					((HudStaticText)newRow[3]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
					((HudStaticText)newRow[4]).TextAlignment = VirindiViewService.WriteTextFormats.Right;

					// Sort the list
					if (Settings.SettingsManager.CombatTracker.SortAlphabetically.Value)
						SortListAlphabetically();

					break;
				}

				if (!String.IsNullOrEmpty(sourceName) && ((HudStaticText)monsterList[row][1]).Text == sourceName)
					break;

				if (!String.IsNullOrEmpty(targetName) && ((HudStaticText)monsterList[row][1]).Text == targetName)
					break;
			}
		}

		void SortListAlphabetically()
		{
			for (int row = 2 ; row < monsterList.RowCount - 1 ; row++)
			{
				for (int compareRow = row + 1 ; compareRow < monsterList.RowCount ; compareRow++)
				{
					if (String.CompareOrdinal(((HudStaticText)monsterList[row][1]).Text, ((HudStaticText)monsterList[compareRow][1]).Text) == 1)
					{
						string obj0 = ((HudStaticText)monsterList[row][0]).Text;
						((HudStaticText)monsterList[row][0]).Text = ((HudStaticText)monsterList[compareRow][0]).Text;
						((HudStaticText)monsterList[compareRow][0]).Text = obj0;

						string obj1 = ((HudStaticText)monsterList[row][1]).Text;
						((HudStaticText)monsterList[row][1]).Text = ((HudStaticText)monsterList[compareRow][1]).Text;
						((HudStaticText)monsterList[compareRow][1]).Text = obj1;

						string obj2 = ((HudStaticText)monsterList[row][2]).Text;
						((HudStaticText)monsterList[row][2]).Text = ((HudStaticText)monsterList[compareRow][2]).Text;
						((HudStaticText)monsterList[compareRow][2]).Text = obj2;

						string obj3 = ((HudStaticText)monsterList[row][3]).Text;
						((HudStaticText)monsterList[row][3]).Text = ((HudStaticText)monsterList[compareRow][3]).Text;
						((HudStaticText)monsterList[compareRow][3]).Text = obj3;

						string obj4 = ((HudStaticText)monsterList[row][4]).Text;
						((HudStaticText)monsterList[row][4]).Text = ((HudStaticText)monsterList[compareRow][4]).Text;
						((HudStaticText)monsterList[compareRow][4]).Text = obj4;
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name">name should be a target name, not the current players name.</param>
		void loadInfoForName(string name)
		{
			for (int i = 1 ; i <= 2 ; i++)
			{
				List<CombatInfo> combatInfos;
				List<AetheriaInfo> aetheriaInfos;
				List<CloakInfo> cloakInfos;

				if (i == 1)
				{
					combatInfos = combatTracker.GetCombatInfos(CoreManager.Current.CharacterFilter.Name);
					aetheriaInfos = combatTracker.GetAetheriaInfos(CoreManager.Current.CharacterFilter.Name);
					cloakInfos = combatTracker.GetCloakInfos(CoreManager.Current.CharacterFilter.Name);
				}
				else
				{
					combatInfos = combatTracker.GetCombatInfos(name);
					aetheriaInfos = combatTracker.GetAetheriaInfos(name);
					cloakInfos = combatTracker.GetCloakInfos(name);
				}

				int killingBlows = 0;
				int damageReceived = 0;
				long damageGiven = 0;

				foreach (CombatInfo combatInfo in combatInfos)
				{
					if (combatInfo.SourceName == CoreManager.Current.CharacterFilter.Name)
						killingBlows += combatInfo.KillingBlows;
					if (combatInfo.TargetName == CoreManager.Current.CharacterFilter.Name)
						damageReceived += combatInfo.Damage;
					if (combatInfo.SourceName == CoreManager.Current.CharacterFilter.Name)
						damageGiven += combatInfo.Damage;
				}

				int targetRow = 0;

				if (i == 1)
					targetRow = 1;
				else
				{
					for (int row = 2 ; row < monsterList.RowCount ; row++)
					{
						if (((HudStaticText)monsterList[row][1]).Text == name)
						{
							targetRow = row;
							break;
						}
					}
				}

				if (targetRow != 0)
				{
					if (killingBlows != 0)
						((HudStaticText)monsterList[targetRow][2]).Text = Util.NumberFormatter(killingBlows, ("#,##0"), 99999);
					if (damageReceived != 0)
						((HudStaticText)monsterList[targetRow][3]).Text = Util.NumberFormatter(damageReceived, ("#,##0"), 9999999);
					if (damageGiven != 0)
						((HudStaticText)monsterList[targetRow][4]).Text = Util.NumberFormatter(damageGiven, ("#,##0"), 99999999);

					if (targetRow == selectedRow)
					{
						// Add up our damage received totals for each type vs element
						// This is ugly but I just wanted to get it done quickly
						int typelessMeleeMissile = 0;
						int typelessMagic = 0;
						int slashMeleeMissile = 0;
						int slashMagic = 0;
						int pierceMeleeMissile = 0;
						int pierceMagic = 0;
						int bludgeMeleeMissile = 0;
						int bludgeMagic = 0;
						int fireMeleeMissile = 0;
						int fireMagic = 0;
						int coldMeleeMissile = 0;
						int coldMagic = 0;
						int acidMeleeMissile = 0;
						int acidMagic = 0;
						int electricMeleeMissile = 0;
						int electricMagic = 0;

						int totalMeleeMissile = 0;
						int totalMagic = 0;

						foreach (CombatInfo combatInfo in combatInfos)
						{
							// We only add up damage received here, which means the player was the target
							if (combatInfo.TargetName != CoreManager.Current.CharacterFilter.Name)
								continue;

							foreach (KeyValuePair<AttackType, CombatInfo.DamageByAttackType> damageByAttackType in combatInfo.DamageByAttackTypes)
							{
								foreach (KeyValuePair<DamageElement, CombatInfo.DamageByAttackType.DamageByElement> damageByElement in damageByAttackType.Value.DamageByElements)
								{
									if (damageByAttackType.Key == AttackType.MeleeMissle)
									{
										if (damageByElement.Key == DamageElement.Typeless) typelessMeleeMissile += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Slash) slashMeleeMissile += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Pierce) pierceMeleeMissile += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Bludge) bludgeMeleeMissile += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Fire) fireMeleeMissile += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Cold) coldMeleeMissile += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Acid) acidMeleeMissile += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Electric) electricMeleeMissile += damageByElement.Value.Damage;

										totalMeleeMissile += damageByElement.Value.Damage;
									}

									if (damageByAttackType.Key == AttackType.Magic)
									{
										if (damageByElement.Key == DamageElement.Typeless) typelessMagic += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Slash) slashMagic += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Pierce) pierceMagic += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Bludge) bludgeMagic += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Fire) fireMagic += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Cold) coldMagic += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Acid) acidMagic += damageByElement.Value.Damage;
										if (damageByElement.Key == DamageElement.Electric) electricMagic += damageByElement.Value.Damage;

										totalMagic += damageByElement.Value.Damage;
									}
								}
							}
						}

						combatTrackerGUIInfo.TypelessMeleeMissile = typelessMeleeMissile;
						combatTrackerGUIInfo.TypelessMeleeMissile = typelessMagic;
						combatTrackerGUIInfo.SlashMeleeMissile = slashMeleeMissile;
						combatTrackerGUIInfo.SlashMagic = slashMagic;
						combatTrackerGUIInfo.PierceMeleeMissile = pierceMeleeMissile;
						combatTrackerGUIInfo.PierceMagic = pierceMagic;
						combatTrackerGUIInfo.BludgeMeleeMissile = bludgeMeleeMissile;
						combatTrackerGUIInfo.BludgeMagic = bludgeMagic;
						combatTrackerGUIInfo.FireMeleeMissile = fireMeleeMissile;
						combatTrackerGUIInfo.FireMagic = fireMagic;
						combatTrackerGUIInfo.ColdMeleeMissile = coldMeleeMissile;
						combatTrackerGUIInfo.ColdMagic = coldMagic;
						combatTrackerGUIInfo.AcidMeleeMissile = acidMeleeMissile;
						combatTrackerGUIInfo.AcidMagic = acidMagic;
						combatTrackerGUIInfo.ElectricMeleeMissile = electricMeleeMissile;
						combatTrackerGUIInfo.ElectricMagic = electricMagic;

						combatTrackerGUIInfo.TotalMeleeMissile = totalMeleeMissile;
						combatTrackerGUIInfo.TotalMagic = totalMagic;


						// Attacks
						int totalAttacks = 0;
						int failedAttacks = 0;
						foreach (CombatInfo combatInfo in combatInfos)
						{
							if (combatInfo.SourceName == CoreManager.Current.CharacterFilter.Name)
							{
								totalAttacks += combatInfo.TotalAttacks;
								failedAttacks += combatInfo.FailedAttacks;
							}
						}
						combatTrackerGUIInfo.SetAttacks(totalAttacks, ((totalAttacks - failedAttacks) / (float)totalAttacks) * 100);

						// Evades
						int totalMeleeDefends = 0;
						int totalEvades = 0;
						foreach (CombatInfo combatInfo in combatInfos)
						{
							if (combatInfo.TargetName == CoreManager.Current.CharacterFilter.Name)
							{
								foreach (KeyValuePair<AttackType, CombatInfo.DamageByAttackType> damageByAttackType in combatInfo.DamageByAttackTypes)
								{
									if (damageByAttackType.Key == AttackType.MeleeMissle)
									{
										totalMeleeDefends += damageByAttackType.Value.TotalAttacks;
										totalEvades += damageByAttackType.Value.FailedAttacks;
									}
								}
							}
						}
						combatTrackerGUIInfo.SetEvades(totalMeleeDefends, (totalEvades / (float)totalMeleeDefends) * 100);

						// Resists
						int totalMagicDefends = 0;
						int totalResists = 0;
						foreach (CombatInfo combatInfo in combatInfos)
						{
							if (combatInfo.TargetName == CoreManager.Current.CharacterFilter.Name)
							{
								foreach (KeyValuePair<AttackType, CombatInfo.DamageByAttackType> damageByAttackType in combatInfo.DamageByAttackTypes)
								{
									if (damageByAttackType.Key == AttackType.Magic)
									{
										totalMagicDefends += damageByAttackType.Value.TotalAttacks;
										totalResists += damageByAttackType.Value.FailedAttacks;
									}
								}
							}
						}
						combatTrackerGUIInfo.SetResists(totalMagicDefends, (totalResists / (float)totalMagicDefends) * 100);

						// Aetheria Surges
						int aetheriaSurges = 0;
						foreach (AetheriaInfo aetheriaInfo in aetheriaInfos)
						{
							if (aetheriaInfo.SourceName == CoreManager.Current.CharacterFilter.Name)
								aetheriaSurges += aetheriaInfo.TotalSurges;
						}
						// Aetheria proc % is calc'd against total offensive attacks (hit or miss).
						combatTrackerGUIInfo.SetAetheriaSurges(aetheriaSurges, (aetheriaSurges / (float)totalAttacks) * 100);

						// Cloak Surges
						int cloakSurges = 0;
						foreach (CloakInfo cloakInfo in cloakInfos)
						{
							if (cloakInfo.SourceName == CoreManager.Current.CharacterFilter.Name)
								cloakSurges += cloakInfo.TotalSurges;
						}
						// Cloaks proc % is calc'd against all received attacks (only hits).
						combatTrackerGUIInfo.SetCloakSurges(cloakSurges, cloakSurges / (float)((totalMeleeDefends - totalEvades) + (totalMagicDefends - totalResists)) * 100);


						// Normal Avg/Max
						// Crits/%
						// Crit Avg/Max
						long totalNormalDamage = 0;
						int maxNormalDamage = 0;
						int crits = 0;
						long totalCritDamage = 0;
						int maxCritDamage = 0;
						foreach (CombatInfo combatInfo in combatInfos)
						{
							if (combatInfo.SourceName == CoreManager.Current.CharacterFilter.Name)
							{
								totalNormalDamage += combatInfo.TotalNormalDamage;
								if (maxNormalDamage < combatInfo.MaxNormalDamage)
									maxNormalDamage = combatInfo.MaxNormalDamage;

								crits += combatInfo.Crits;

								totalCritDamage += combatInfo.TotalCritDamage;
								if (maxCritDamage < combatInfo.MaxCritDamage)
									maxCritDamage = combatInfo.MaxCritDamage;
							}
						}

						if (totalAttacks - failedAttacks - crits - killingBlows != 0)
							combatTrackerGUIInfo.SetAvgMax((int)(totalNormalDamage / (totalAttacks - failedAttacks - crits - killingBlows)), maxNormalDamage);
						if (totalAttacks - failedAttacks - killingBlows != 0)
							combatTrackerGUIInfo.SetCrits(crits, (crits / (float)(totalAttacks - failedAttacks - killingBlows)) * 100);
						if (crits != 0)
							combatTrackerGUIInfo.SetCritsAvgMax((int)(totalCritDamage / crits), maxCritDamage);


						// Damage Total
						combatTrackerGUIInfo.TotalDamage = damageGiven;
					}
				}
			}
		}
	}
}
