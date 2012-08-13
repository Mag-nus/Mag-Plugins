using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mag_SuitBuilder
{
	partial class EquipmentPieceControl : UserControl, IEquipmentPiece
	{
		public EquipmentPieceControl()
		{
			InitializeComponent();
		}

		public bool IsLocked { get { return chkLocked.Checked; } }

		string IEquipmentPiece.Name { get { return lblItemName.Text; } }

		public Constants.EquippableSlotFlags EquipableSlots { get; set; }

		public int NumberOfSlotsCovered { get { return 1; } }

		public bool CanHaveArmorLevel
		{
			get
			{
				return txtArmorLevel.Visible;
			}
			set
			{
				txtArmorLevel.Visible = value;
			}
		}

		public int ArmorLevel
		{
			get
			{
				if (!CanHaveArmorLevel)
					return 0;

				int al;

				int.TryParse(txtArmorLevel.Text, out al);

				return al;
			}
			set
			{
				if (value == 0)
					txtArmorLevel.Text = null;
				else
					txtArmorLevel.Text = value.ToString();
			}
		}

		public bool CanHaveArmorSet
		{
			get
			{
				return txtArmorSet.Visible;
			}
			set
			{
				txtArmorSet.Visible = value;
			}
		}

		public string ArmorSet { get { return txtArmorSet.Text; } }

		public ReadOnlyCollection<Spell> Spells
		{
			get
			{
				List<Spell> spells = new List<Spell>();

				if (!String.IsNullOrEmpty(txtSpell1.Text)) spells.Add(new Spell(txtSpell1.Text));
				if (!String.IsNullOrEmpty(txtSpell2.Text)) spells.Add(new Spell(txtSpell2.Text));
				if (!String.IsNullOrEmpty(txtSpell3.Text)) spells.Add(new Spell(txtSpell3.Text));
				if (!String.IsNullOrEmpty(txtSpell4.Text)) spells.Add(new Spell(txtSpell4.Text));

				return spells.AsReadOnly();
			}
		}

		public bool CanEquip(IEquipmentPiece piece)
		{
			return (piece.EquipableSlots & EquipableSlots) == EquipableSlots;
		}

		public void SetEquipmentPiece(IEquipmentPiece piece)
		{
			if (piece == null || !CanEquip(piece))
			{
				lblItemName.Text = null;

				ArmorLevel = 0;

				txtArmorSet.Text = null;

				txtSpell1.Text = null;
				txtSpell2.Text = null;
				txtSpell3.Text = null;
				txtSpell4.Text = null;

				return;
			}

			lblItemName.Text = piece.Name;

			ArmorLevel = piece.ArmorLevel;

			txtArmorSet.Text = piece.ArmorSet;

			txtSpell1.Text = (piece.Spells.Count >= 1) ? piece.Spells[0].Name : null;
			txtSpell2.Text = (piece.Spells.Count >= 2) ? piece.Spells[1].Name : null;
			txtSpell3.Text = (piece.Spells.Count >= 3) ? piece.Spells[2].Name : null;
			txtSpell4.Text = (piece.Spells.Count >= 4) ? piece.Spells[3].Name : null;
		}

		public override string ToString()
		{
			string output = lblItemName.Text;

			if (ArmorLevel > 0)
				output += " AL " + ArmorLevel;

			foreach (Spell spell in Spells)
				output += " " + spell;

			return output;
		}
	}
}
