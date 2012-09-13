using System.Windows.Forms;

namespace Mag_SuitBuilder.Equipment
{
	partial class EquipmentPieceControl : UserControl
	{
		public EquipmentPieceControl()
		{
			InitializeComponent();
		}

		public Constants.EquippableSlotFlags EquipableSlots { get; set; }

		public bool CanEquip(EquipmentPiece piece)
		{
			return (piece.EquipableSlots & EquipableSlots) == EquipableSlots;
		}

		public void SetEquipmentPiece(EquipmentPiece piece)
		{
			if (piece == null || !CanEquip(piece))
			{
				lblItemName.Text = null;

				txtArmorLevel.Text = null;

				txtArmorSet.Text = null;

				txtSpell1.Text = null;
				txtSpell2.Text = null;
				txtSpell3.Text = null;
				txtSpell4.Text = null;

				return;
			}

			lblItemName.Text = piece.Name;

			if (piece.BaseArmorLevel == 0)
				txtArmorLevel.Text = null;
			else
				txtArmorLevel.Text = piece.BaseArmorLevel.ToString();

			txtArmorSet.Text = piece.ArmorSet.ToString();

			txtSpell1.Text = (piece.Spells.Count >= 1) ? piece.Spells[0].Name : null;
			txtSpell2.Text = (piece.Spells.Count >= 2) ? piece.Spells[1].Name : null;
			txtSpell3.Text = (piece.Spells.Count >= 3) ? piece.Spells[2].Name : null;
			txtSpell4.Text = (piece.Spells.Count >= 4) ? piece.Spells[3].Name : null;
		}
	}
}
