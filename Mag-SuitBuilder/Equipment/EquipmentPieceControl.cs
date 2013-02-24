using System.Globalization;
using System.Windows.Forms;

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
			lblItemName.Text = null;

			txtArmorLevel.Text = null;

			txtArmorSet.Text = null;

			txtSpell1.Text = null;
			txtSpell2.Text = null;
			txtSpell3.Text = null;
			txtSpell4.Text = null;

			if (piece == null || !CanEquip(piece))
				return;

			lblItemName.Text = piece.Name;

			if (piece.CalcedStartingArmorLevel > 0)
				txtArmorLevel.Text = piece.CalcedStartingArmorLevel.ToString(CultureInfo.InvariantCulture);

			txtArmorSet.Text = piece.ItemSet;

			if (piece.CachedSpells.Count > 0) txtSpell1.Text = piece.CachedSpells[0].ToString();
			if (piece.CachedSpells.Count > 1) txtSpell2.Text = piece.CachedSpells[1].ToString();
			if (piece.CachedSpells.Count > 2) txtSpell3.Text = piece.CachedSpells[2].ToString();
			if (piece.CachedSpells.Count > 3) txtSpell4.Text = piece.CachedSpells[3].ToString();
		}
	}
}
