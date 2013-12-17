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


			int minimumBaseArmorLevel;
			int.TryParse(txtMinimumBaseArmorLevel.Text, out minimumBaseArmorLevel);
			if ((mwo.ObjectClass == (int)ObjectClass.Armor || mwo.ObjectClass == (int)ObjectClass.Clothing) && mwo.CalcedStartingArmorLevel < minimumBaseArmorLevel && mwo.EquippableSlots.IsBodyArmor())
				return false;

			if (mwo.ObjectClass == (int)ObjectClass.Armor || mwo.ObjectClass == (int)ObjectClass.Clothing || mwo.ObjectClass == (int)ObjectClass.Jewelry)
			{
				int rating;

				if (mwo.ObjectClass == (int)ObjectClass.Armor || mwo.ObjectClass == (int)ObjectClass.Clothing)
				{
					int.TryParse(txtMinOffensiveRating.Text, out rating);
					if (rating > 0 && mwo.CritDamRating + mwo.CritRating + mwo.DamRating < rating)
						return false;

					int.TryParse(txtMinDefensiveRating.Text, out rating);
					if (rating > 0 && mwo.CritDamResistRating + mwo.CritResistRating + mwo.DamResistRating < rating)
						return false;
				}

				int.TryParse(txtMinTotalRating.Text, out rating);
				if (rating > 0 && mwo.TotalRating < rating)
					return false;
			}


			if (mwo.ObjectClass == (int)ObjectClass.Armor)
			{
				if (!chkArmor.Checked) return false;
			}
			else if (mwo.ObjectClass == (int)ObjectClass.Clothing)
			{
				if (!chkClothing.Checked) return false;
			}
			else if (mwo.ObjectClass == (int)ObjectClass.Jewelry)
			{
				if (!chkJewelry.Checked) return false;
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


			int minLegendaries;
			int.TryParse(txtMinLegendaries.Text, out minLegendaries);

			int minEpics;
			int.TryParse(txtMinEpics.Text, out minEpics);
			
			if (minLegendaries > 0 || minEpics > 0)
			{
				int legendaries = 0;
				int epics = 0;

				foreach (Spell spell in mwo.CachedSpells)
				{
					if (spell.CantripLevel >= Spell.CantripLevels.Legendary) legendaries++;
					if (spell.CantripLevel >= Spell.CantripLevels.Epic) epics++;
				}

				if ((minLegendaries > 0 && legendaries < minLegendaries) || (minEpics > 0 && epics < minEpics))
					return false;
			}


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
