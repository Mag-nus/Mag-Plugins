using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Mag.Shared;
using Mag.Shared.Constants;
using Mag.Shared.Spells;

namespace Mag_SuitBuilder.Equipment
{
	public partial class FiltersControl : UserControl
	{
		public FiltersControl()
		{
			InitializeComponent();

			cantripSelectorControl1.CollectionChanged += (s, e) =>
			{
				if (FiltersChanged != null && !suspendChangedEvent)
					FiltersChanged();
			};
		}

		public Action FiltersChanged;
		private bool suspendChangedEvent;

		private void chkFilter_CheckedChanged(object sender, EventArgs e)
		{
			if (FiltersChanged != null && !suspendChangedEvent)
				FiltersChanged();
		}

		private void txtFilter_TextChanged(object sender, EventArgs e)
		{
			if (FiltersChanged != null && !suspendChangedEvent)
				FiltersChanged();
		}

		private void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (FiltersChanged != null && !suspendChangedEvent)
				FiltersChanged();
		}

		private void cmdClearAllCheckboxes_Click(object sender, EventArgs e)
		{
			suspendChangedEvent = true;

			foreach (var control in Controls)
			{
				if (control is CheckBox)
					(control as CheckBox).Checked = false;
			}

			suspendChangedEvent = false;

			if (FiltersChanged != null)
				FiltersChanged();
		}

		private void cmdCheckAllCheckboxes_Click(object sender, EventArgs e)
		{
			suspendChangedEvent = true;

			foreach (var control in Controls)
			{
				if (control == checkRemoveEquipped || control == chkRemoveUnequipped)
					continue;

				if (control is CheckBox)
					(control as CheckBox).Checked = true;
			}

			suspendChangedEvent = false;

			if (FiltersChanged != null)
				FiltersChanged();
		}

		private void cmdApplyRegexStringMatch_Click(object sender, EventArgs e)
		{
			if (FiltersChanged != null)
				FiltersChanged();
		}

		public void UpdateArmorSets(IDictionary<string, int> armorSets)
		{
			suspendChangedEvent = true;

			cboPrimaryArmorSet.Items.Clear();
			cboPrimaryArmorSet.Items.Add(new KeyValuePair<string, int>("No Armor Set", 0));
			cboPrimaryArmorSet.Items.Add(new KeyValuePair<string, int>("Any Armor Set", 255));
			cboPrimaryArmorSet.SelectedIndex = 1;
			foreach (var v in armorSets)
				cboPrimaryArmorSet.Items.Add(v);

			cboSecondaryArmorSet.Items.Clear();
			cboSecondaryArmorSet.Items.Add(new KeyValuePair<string, int>("No Armor Set", 0));
			cboSecondaryArmorSet.Items.Add(new KeyValuePair<string, int>("Any Armor Set", 255));
			cboSecondaryArmorSet.SelectedIndex = 1;
			foreach (var v in armorSets)
				cboSecondaryArmorSet.Items.Add(v);

			suspendChangedEvent = false;

			if (FiltersChanged != null)
				FiltersChanged();
		}

		public bool ItemPassesFilters(ExtendedMyWorldObject mwo)
		{
			if (checkRemoveEquipped.Checked && mwo.EquippedSlot != EquipMask.None)
				return false;

			if (chkRemoveUnequipped.Checked && mwo.EquippedSlot == EquipMask.None)
				return false;


			int value;
			int.TryParse(txtMinimumBaseArmorLevel.Text, out value);
			if ((mwo.ObjClass == ObjectClass.Armor || mwo.ObjClass == ObjectClass.Clothing) && mwo.CalcedStartingArmorLevel < value && mwo.EquippableSlots.IsCoreBodyArmor())
				return false;

			int.TryParse(txtMaximumBaseArmorLevel.Text, out value);
			if ((mwo.ObjClass == ObjectClass.Armor || mwo.ObjClass == ObjectClass.Clothing) && mwo.CalcedStartingArmorLevel > value && mwo.EquippableSlots.IsCoreBodyArmor())
				return false;

			int.TryParse(txtMinimumExtremityArmorLevel.Text, out value);
			if ((mwo.ObjClass == ObjectClass.Armor || mwo.ObjClass == ObjectClass.Clothing) && mwo.CalcedStartingArmorLevel < value && mwo.EquippableSlots.IsExtremityBodyArmor())
				return false;

			int.TryParse(txtMaximumExtremityArmorLevel.Text, out value);
			if ((mwo.ObjClass == ObjectClass.Armor || mwo.ObjClass == ObjectClass.Clothing) && mwo.CalcedStartingArmorLevel > value && mwo.EquippableSlots.IsExtremityBodyArmor())
				return false;


			if (mwo.EquippableSlots.IsBodyArmor())
			{
				if (!chkBodyArmorClothing.Checked) return false;
			}
			else if (mwo.EquippableSlots.IsUnderwear())
			{
				if (!chkShirtPants.Checked) return false;
			}
			else if (mwo.ObjClass == ObjectClass.Jewelry)
			{
				if (!chkJewelry.Checked) return false;

				if (!chkJewelryNecklace.Checked && mwo.EquippableSlots == EquipMask.NeckWear) return false;
				if (!chkJewelryTrinket.Checked && mwo.EquippableSlots == EquipMask.TrinketOne) return false;
				if (!chkJewelryBracelet.Checked && mwo.EquippableSlots == (EquipMask.WristWearLeft | EquipMask.WristWearRight)) return false;
				if (!chkJewelryRing.Checked && mwo.EquippableSlots == (EquipMask.FingerWearLeft | EquipMask.FingerWearRight)) return false;
			}
			else if (mwo.ObjClass == ObjectClass.MeleeWeapon)
			{
				if (!chkMeleeWeapon.Checked) return false;

				if (!chkMeleeHeavy.Checked && mwo.EquipSkill == "Heavy Weapons") return false;
				if (!chkMeleeLight.Checked && mwo.EquipSkill == "Light Weapons") return false;
				if (!chkMeleeFinesse.Checked && mwo.EquipSkill == "Finesse Weapons") return false;
				if (!chkMelee2H.Checked && mwo.EquipSkill == "Two Handed Combat") return false;

				if (!chkMasteryUA.Checked && mwo.Mastery == "Unarmed Weapon") return false;
				if (!chkMasterySword.Checked && mwo.Mastery == "Sword") return false;
				if (!chkMasteryAxe.Checked && mwo.Mastery == "Axe") return false;
				if (!chkMasteryMace.Checked && mwo.Mastery == "Mace") return false;
				if (!chkMasterySpear.Checked && mwo.Mastery == "Spear") return false;
				if (!chkMasteryDagger.Checked && mwo.Mastery == "Dagger") return false;
				if (!chkMasteryStaff.Checked && mwo.Mastery == "Staff") return false;
			}
			else if (mwo.ObjClass == ObjectClass.MissileWeapon)
			{
				if (!chkMissileWeapon.Checked) return false;

				if (!chkMasteryBow.Checked && mwo.Mastery == "Bow") return false;
				if (!chkMasteryCrossbow.Checked && mwo.Mastery == "Crossbow") return false;
				if (!chkMasteryThrown.Checked && mwo.Mastery == "Thrown") return false;
			}
			else if (mwo.ObjClass == ObjectClass.WandStaffOrb)
			{
				if (!chkWandStaffOrb.Checked) return false;

				if (!chkWandStaffOrbWar.Checked && mwo.IntValues.ContainsKey(158) && mwo.IntValues[158] == 2 && mwo.IntValues.ContainsKey(159) && mwo.IntValues[159] == 0x22) return false;
				if (!chkWandStaffOrbVoid.Checked && mwo.IntValues.ContainsKey(158) && mwo.IntValues[158] == 2 && mwo.IntValues.ContainsKey(159) && mwo.IntValues[159] == 0x2B) return false;
			}
			else if (mwo.ObjClass == ObjectClass.Salvage)
			{
				if (!chkSalvage.Checked) return false;
			}
			else if (mwo.ObjClass == ObjectClass.Container || mwo.ObjClass == ObjectClass.Foci)
			{
				if (!chkContainersFoci.Checked) return false;
			}
			else if (mwo.ObjClass == ObjectClass.Money || mwo.ObjClass == ObjectClass.TradeNote || mwo.ObjClass == ObjectClass.Key)
			{
				if (!chkMoneyNotesKeys.Checked) return false;
			}
			else if (mwo.ObjClass == ObjectClass.SpellComponent || mwo.ObjClass == ObjectClass.HealingKit || mwo.ObjClass == ObjectClass.Food || mwo.ObjClass == ObjectClass.ManaStone)
			{
				if (!chkCompsKitsFoodManaStones.Checked) return false;
			}
			else if (mwo.ObjClass == ObjectClass.Misc && mwo.Name.Contains("Essence") && !mwo.Name.Contains("Corrupted") && !mwo.Name.Contains("Degenerate"))
			{
				if (!chkPets.Checked) return false;
			}
			else if (mwo.ObjClass != 0) // All Else
			{
				if (!chkAllElseObjectClasses.Checked) return false;
			}


			if (mwo.EquippableSlots.IsBodyArmor())
			{
				// Both are No Armor Set and the item has a set
				if (PrimaryArmorSetId == 0 && SecondaryArmorSetId == 0 && mwo.ItemSetId != 0)
					return false;

				if (PrimaryArmorSetId != 255 && SecondaryArmorSetId != 255)
				{
					if (PrimaryArmorSetId != mwo.ItemSetId && SecondaryArmorSetId != mwo.ItemSetId)
						return false;
				}
			}


			// Regex String Match
			if (!String.IsNullOrEmpty(txtRegexStringMatch.Text))
			{
				var regex = new Regex(txtRegexStringMatch.Text, RegexOptions.IgnoreCase);

				bool hasSpellMatch = false;

				foreach (var spellID in mwo.Spells)
				{
					Spell spell = SpellTools.GetSpell(spellID);

					if (spell == null)
						continue;

					if (regex.IsMatch(spell.Name))
					{
						hasSpellMatch = true;
						break;
					}
				}

				if (!hasSpellMatch)
				{
					var itemInfo = new ItemInfo(mwo);

					if (!regex.IsMatch(itemInfo.ToString())) return false;
				}
			}


			// Spell Quantities
			int minLegendaries;
			int.TryParse(txtMinLegendaries.Text, out minLegendaries);

			int maxLegendaries;
			int.TryParse(txtMaxLegendaries.Text, out maxLegendaries);

			int minEpics;
			int.TryParse(txtMinEpics.Text, out minEpics);

			int maxEpics;
			int.TryParse(txtMaxEpics.Text, out maxEpics);
			
			int legendaries = 0;
			int epics = 0;

			foreach (Spell spell in mwo.CachedSpells)
			{
				if (spell.CantripLevel >= Spell.CantripLevels.Legendary) legendaries++;
				if (spell.CantripLevel >= Spell.CantripLevels.Epic) epics++;
			}

			if (legendaries < minLegendaries || legendaries > maxLegendaries || epics < minEpics || epics > maxEpics)
				return false;


			// Ratings
			if (mwo.ObjClass == ObjectClass.Armor || mwo.ObjClass == ObjectClass.Clothing || mwo.ObjClass == ObjectClass.Jewelry)
			{
				if (mwo.ObjClass == ObjectClass.Armor || mwo.ObjClass == ObjectClass.Clothing)
				{
					if (int.TryParse(txtMinOffensiveRating.Text, out value))
					{
						if (Math.Max(mwo.CritDamRating, 0) + Math.Max(mwo.CritRating, 0) + Math.Max(mwo.DamRating, 0) < value)
							return false;
					}

					if (int.TryParse(txtMaxOffensiveRating.Text, out value))
					{
						if (Math.Max(mwo.CritDamRating, 0) + Math.Max(mwo.CritRating, 0) + Math.Max(mwo.DamRating, 0) > value)
							return false;
					}

					if (int.TryParse(txtMinDefensiveRating.Text, out value))
					{
						if (Math.Max(mwo.CritDamResistRating, 0) + Math.Max(mwo.CritResistRating, 0) + Math.Max(mwo.DamResistRating, 0) < value)
							return false;
					}

					if (int.TryParse(txtMaxDefensiveRating.Text, out value))
					{
						if (Math.Max(mwo.CritDamResistRating, 0) + Math.Max(mwo.CritResistRating, 0) + Math.Max(mwo.DamResistRating, 0) > value)
							return false;
					}
				}

				if (mwo.ObjClass == ObjectClass.Jewelry)
				{
					if (int.TryParse(txtMinOtherRating.Text, out value))
					{
						if (Math.Max(mwo.HealBoostRating, 0) + Math.Max(mwo.VitalityRating, 0) < value)
							return false;
					}

					if (int.TryParse(txtMaxOtherRating.Text, out value))
					{
						if (Math.Max(mwo.HealBoostRating, 0) + Math.Max(mwo.VitalityRating, 0) > value)
							return false;
					}
				}

				if (int.TryParse(txtMinTotalRating.Text, out value))
				{
					if (Math.Max(mwo.TotalRating, 0) < value)
						return false;
				}

				if (int.TryParse(txtMaxTotalRating.Text, out value))
				{
					if (Math.Max(mwo.TotalRating, 0) > value)
						return false;
				}
			}


			// Wield Requirements
			if (int.TryParse(txtWieldRequirementLevelMin.Text, out value))
			{
				if (Math.Max(mwo.WieldLevel, 0) < value)
					return false;
			}

			if (int.TryParse(txtWieldRequirementLevelMax.Text, out value))
			{
				if (Math.Max(mwo.WieldLevel, 0) > value)
					return false;
			}

			if (int.TryParse(txtWieldRequirementSkillMin.Text, out value))
			{
				if (Math.Max(mwo.SkillLevel, 0) < value)
					return false;
			}

			if (int.TryParse(txtWieldRequirementSkillMax.Text, out value))
			{
				if (Math.Max(mwo.SkillLevel, 0) > value)
					return false;
			}


			// Spell Selector
			if (cantripSelectorControl1.Count > 0)
			{
				foreach (var spell in mwo.CachedSpells)
				{
					foreach (Spell desiredSpell in cantripSelectorControl1)
					{
						if (spell.IsSameOrSurpasses(desiredSpell))
							goto end;
					}
				}
				return false;
				end: ;
			}

			return true;
		}

		public int PrimaryArmorSetId
		{
			get
			{
				if (cboPrimaryArmorSet.SelectedItem is KeyValuePair<string, int>)
					return ((KeyValuePair<string, int>)cboPrimaryArmorSet.SelectedItem).Value;

				return 255;
			}
		}

		public int SecondaryArmorSetId
		{
			get
			{
				if (cboSecondaryArmorSet.SelectedItem is KeyValuePair<string, int>)
					return ((KeyValuePair<string, int>)cboSecondaryArmorSet.SelectedItem).Value;

				return 255;
			}
		}

		public ICollection<Spell> CantripsToLookFor
		{
			get
			{
				return cantripSelectorControl1;
			}
		}
	}
}
