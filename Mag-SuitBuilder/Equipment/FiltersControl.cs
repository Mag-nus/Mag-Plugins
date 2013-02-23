using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Mag_SuitBuilder.Spells;

using Mag.Shared;

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

		private void btnClearAllFilters_Click(object sender, EventArgs e)
		{
			checkRemoveEquipped.Checked = false;
			txtMinimumBaseArmorLevel.Text = "0";
		}

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

		public void UpdateArmorSets(IDictionary<string, long> armorSets)
		{
			cboPrimaryArmorSet.Items.Clear();
			cboPrimaryArmorSet.Items.Add(new KeyValuePair<string, long>("No Armor Set", 0));
			cboPrimaryArmorSet.Items.Add(new KeyValuePair<string, long>("Any Armor Set", 255));
			cboPrimaryArmorSet.SelectedIndex = 1;
			foreach (var v in armorSets)
				cboPrimaryArmorSet.Items.Add(v);

			cboSecondaryArmorSet.Items.Clear();
			cboSecondaryArmorSet.Items.Add(new KeyValuePair<string, long>("No Armor Set", 0));
			cboSecondaryArmorSet.Items.Add(new KeyValuePair<string, long>("Any Armor Set", 255));
			cboSecondaryArmorSet.SelectedIndex = 1;
			foreach (var v in armorSets)
				cboSecondaryArmorSet.Items.Add(v);
		}

		public bool ItemPassesFilters(SuitBuildableMyWorldObject mwo)
		{
			if (checkRemoveEquipped.Checked && mwo.EquippedSlot != Constants.EquippableSlotFlags.None)
				return false;

			if (chkRemoveUnequipped.Checked && mwo.EquippedSlot == Constants.EquippableSlotFlags.None)
				return false;

			int minimumBaseArmorLevel;
			int.TryParse(txtMinimumBaseArmorLevel.Text, out minimumBaseArmorLevel);
			if ((mwo.ObjectClass == 2 || mwo.ObjectClass == 3) && mwo.CalcedStartingArmorLevel < minimumBaseArmorLevel && mwo.EquippableSlot.IsBodyArmor())
				return false;

			if (mwo.ObjectClass == 2) // Armor
			{
				if (!chkArmor.Checked) return false;
			}
			else if (mwo.ObjectClass == 3) // Clothing
			{
				if (!chkClothing.Checked) return false;
			}
			else if (mwo.ObjectClass == 4) // Jewelry
			{
				if (!chkJewelry.Checked) return false;
			}
			else if (mwo.ObjectClass == 1) // MeleeWeapon
			{
				if (!chkMeleeWeapon.Checked) return false;
			}
			else if (mwo.ObjectClass == 9) // MissileWeapon
			{
				if (!chkMissileWeapon.Checked) return false;
			}
			else if (mwo.ObjectClass == 31) // WandStaffOrb
			{
				if (!chkWandStaffOrb.Checked) return false;
			}
			else if (mwo.ObjectClass != 0) // All Else
			{
				if (!chkAllElseObjectClasses.Checked) return false;
			}

			if (mwo.EquippableSlot.IsBodyArmor())
			{
				KeyValuePair<string, long> primarykvp = new KeyValuePair<string, long>();
				KeyValuePair<string, long> secondarykvp = new KeyValuePair<string, long>();

				if (cboPrimaryArmorSet.SelectedItem is KeyValuePair<string, long>)
					primarykvp = (KeyValuePair<string, long>)cboPrimaryArmorSet.SelectedItem;
				if (cboSecondaryArmorSet.SelectedItem is KeyValuePair<string, long>)
					secondarykvp = (KeyValuePair<string, long>)cboSecondaryArmorSet.SelectedItem;

				// Both are No Armor Set and the item has a set
				if (primarykvp.Value == 0 && secondarykvp.Value == 0 && mwo.ItemSetId != 0)
					return false;

				if (primarykvp.Value != 255 && secondarykvp.Value != 255)
				{
					if (primarykvp.Value != mwo.ItemSetId && secondarykvp.Value != mwo.ItemSetId)
						return false;
				}
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
	}
}
