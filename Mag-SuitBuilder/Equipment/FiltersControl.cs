using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Mag_SuitBuilder.Spells;

using Mag.Shared;
using Mag.Shared.Constants;

namespace Mag_SuitBuilder.Equipment
{
	public partial class FiltersControl : UserControl
	{
		public FiltersControl()
		{
			InitializeComponent();

			cantripSelectorControl1.CollectionChanged += (s, e) =>
			{
				if (FiltersChanged != null)
					FiltersChanged();
			};
		}

		public Action FiltersChanged;

		private void chkFilter_CheckedChanged(object sender, EventArgs e)
		{
			if (FiltersChanged != null)
				FiltersChanged();
		}

		private void txtFilter_TextChanged(object sender, EventArgs e)
		{
			if (FiltersChanged != null)
				FiltersChanged();
		}

		private void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (FiltersChanged != null)
				FiltersChanged();
		}

		public void UpdateArmorSets(IDictionary<string, int> armorSets)
		{
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
		}

		public bool ItemPassesFilters(SuitBuildableMyWorldObject mwo)
		{
			if (checkRemoveEquipped.Checked && mwo.EquippedSlot != EquippableSlotFlags.None)
				return false;

			if (chkRemoveUnequipped.Checked && mwo.EquippedSlot == EquippableSlotFlags.None)
				return false;


			int value;
			int.TryParse(txtMinimumBaseArmorLevel.Text, out value);
			if ((mwo.ObjectClass == (int)ObjectClass.Armor || mwo.ObjectClass == (int)ObjectClass.Clothing) && mwo.CalcedStartingArmorLevel < value && mwo.EquippableSlots.IsCoreBodyArmor())
				return false;

			int.TryParse(txtMaximumBaseArmorLevel.Text, out value);
			if ((mwo.ObjectClass == (int)ObjectClass.Armor || mwo.ObjectClass == (int)ObjectClass.Clothing) && mwo.CalcedStartingArmorLevel > value && mwo.EquippableSlots.IsCoreBodyArmor())
				return false;

			int.TryParse(txtMinimumExtremityArmorLevel.Text, out value);
			if ((mwo.ObjectClass == (int)ObjectClass.Armor || mwo.ObjectClass == (int)ObjectClass.Clothing) && mwo.CalcedStartingArmorLevel < value && mwo.EquippableSlots.IsExtremityBodyArmor())
				return false;

			int.TryParse(txtMaximumExtremityArmorLevel.Text, out value);
			if ((mwo.ObjectClass == (int)ObjectClass.Armor || mwo.ObjectClass == (int)ObjectClass.Clothing) && mwo.CalcedStartingArmorLevel > value && mwo.EquippableSlots.IsExtremityBodyArmor())
				return false;


			if (mwo.EquippableSlots.IsBodyArmor())
			{
				if (!chkBodyArmorClothing.Checked) return false;
			}
			else if (mwo.EquippableSlots.IsUnderwear())
			{
				if (!chkShirtPants.Checked) return false;
			}
			else if (mwo.ObjectClass == (int)ObjectClass.Jewelry)
			{
				if (!chkJewelry.Checked) return false;

				if (!chkJewelryNecklace.Checked && mwo.EquippableSlots == EquippableSlotFlags.Necklace) return false;
				if (!chkJewelryTrinket.Checked && mwo.EquippableSlots == EquippableSlotFlags.Trinket) return false;
				if (!chkJewelryBracelet.Checked && mwo.EquippableSlots == (EquippableSlotFlags.LeftBracelet | EquippableSlotFlags.RightBracelet)) return false;
				if (!chkJewelryRing.Checked && mwo.EquippableSlots == (EquippableSlotFlags.LeftRing | EquippableSlotFlags.RightRing)) return false;
			}
			else if (mwo.ObjectClass == (int)ObjectClass.MeleeWeapon)
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
			else if (mwo.ObjectClass == (int)ObjectClass.MissileWeapon)
			{
				if (!chkMissileWeapon.Checked) return false;

				if (!chkMasteryBow.Checked && mwo.Mastery == "Bow") return false;
				if (!chkMasteryCrossbow.Checked && mwo.Mastery == "Crossbow") return false;
				if (!chkMasteryThrown.Checked && mwo.Mastery == "Thrown") return false;
			}
			else if (mwo.ObjectClass == (int)ObjectClass.WandStaffOrb)
			{
				if (!chkWandStaffOrb.Checked) return false;

				if (!chkWandStaffOrbWar.Checked && mwo.IntValues.ContainsKey(158) && mwo.IntValues[158] == 2 && mwo.IntValues.ContainsKey(159) && mwo.IntValues[159] == 0x22) return false;
				if (!chkWandStaffOrbVoid.Checked && mwo.IntValues.ContainsKey(158) && mwo.IntValues[158] == 2 && mwo.IntValues.ContainsKey(159) && mwo.IntValues[159] == 0x2B) return false;
			}
			else if (mwo.ObjectClass == (int)ObjectClass.Salvage)
			{
				if (!chkSalvage.Checked) return false;
			}
			else if (mwo.ObjectClass == (int)ObjectClass.Container || mwo.ObjectClass == (int)ObjectClass.Foci)
			{
				if (!chkContainersFoci.Checked) return false;
			}
			else if (mwo.ObjectClass == (int)ObjectClass.Money || mwo.ObjectClass == (int)ObjectClass.TradeNote || mwo.ObjectClass == (int)ObjectClass.Key)
			{
				if (!chkMoneyNotesKeys.Checked) return false;
			}
			else if (mwo.ObjectClass == (int)ObjectClass.SpellComponent || mwo.ObjectClass == (int)ObjectClass.HealingKit || mwo.ObjectClass == (int)ObjectClass.Food || mwo.ObjectClass == (int)ObjectClass.ManaStone)
			{
				if (!chkCompsKitsFoodManaStones.Checked) return false;
			}
			else if (mwo.ObjectClass != 0) // All Else
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
			if (mwo.ObjectClass == (int)ObjectClass.Armor || mwo.ObjectClass == (int)ObjectClass.Clothing || mwo.ObjectClass == (int)ObjectClass.Jewelry)
			{
				if (mwo.ObjectClass == (int)ObjectClass.Armor || mwo.ObjectClass == (int)ObjectClass.Clothing)
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

				if (mwo.ObjectClass == (int)ObjectClass.Jewelry)
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
