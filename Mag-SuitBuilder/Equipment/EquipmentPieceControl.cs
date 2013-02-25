using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

using Mag_SuitBuilder.Spells;

using Mag.Shared;

namespace Mag_SuitBuilder.Equipment
{
	partial class EquipmentPieceControl : UserControl
	{
		public EquipmentPieceControl()
		{
			InitializeComponent();
		}

		public EquippableSlotFlags EquippableSlots { get; set; }

		public bool CanEquip(SuitBuildableMyWorldObject piece)
		{
			return (piece.EquippableSlots & EquippableSlots) == EquippableSlots;
		}

		public void SetEquipmentPiece(SuitBuildableMyWorldObject piece)
		{
			lblCharacter.Text = null;
			lblItemName.Text = null;

			lblAL.Text = null;
			lblArmorSet.Text = null;

			lblSpell1.Text = null;
			lblSpell2.Text = null;
			lblSpell3.Text = null;
			lblSpell4.Text = null;
			lblSpell5.Text = null;
			lblSpell6.Text = null;

			if (piece == null || !CanEquip(piece))
				return;

			lblCharacter.Text = piece.Owner;
			lblItemName.Text = piece.Name;

			if (piece.CalcedStartingArmorLevel > 0)
				lblAL.Text = piece.CalcedStartingArmorLevel.ToString(CultureInfo.InvariantCulture);

			lblArmorSet.Text = piece.ItemSet;

			List<Spell> spellsInOrder = new List<Spell>();

			for (Spell.CantripLevels level = Spell.CantripLevels.Legendary; level > Spell.CantripLevels.None; level--)
			{
				foreach (Spell spell in piece.CachedSpells)
				{
					if (spellsInOrder.Contains(spell))
						continue;

					if (spell.CantripLevel >= level)
						spellsInOrder.Add(spell);
				}
			}

			for (Spell.BuffLevels level = Spell.BuffLevels.VIII; level >= Spell.BuffLevels.None; level--)
			{
				foreach (Spell spell in piece.CachedSpells)
				{
					if (spellsInOrder.Contains(spell))
						continue;

					if (spell.BuffLevel >= level)
						spellsInOrder.Add(spell);
				}
			}

			if (spellsInOrder.Count > 0) lblSpell1.Text = spellsInOrder[0].ToString();
			if (spellsInOrder.Count > 1) lblSpell2.Text = spellsInOrder[1].ToString();
			if (spellsInOrder.Count > 2) lblSpell3.Text = spellsInOrder[2].ToString();
			if (spellsInOrder.Count > 3) lblSpell4.Text = spellsInOrder[3].ToString();
			if (spellsInOrder.Count > 4) lblSpell5.Text = spellsInOrder[4].ToString();
			if (spellsInOrder.Count > 5) lblSpell6.Text = spellsInOrder[5].ToString();
		}
	}
}
