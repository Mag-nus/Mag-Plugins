using System.Windows.Forms;

namespace Mag_SuitBuilder
{
	partial class EquipmentPieceControl : UserControl
	{
		public EquipmentPieceControl()
		{
			InitializeComponent();
		}

		public Constants.EquippableSlotFlags EquipableSlot { get; set; }

		public bool LockedSlot
		{
			get
			{
				return chkLocked.Checked;
			}
			set
			{
				chkLocked.Checked = value;
			}
		}

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

		public bool CanEquip(EquipmentPiece piece)
		{
			return (piece.EquipableSlots & EquipableSlot) == EquipableSlot;
	
		}

		public void SetEquipmentPiece(EquipmentPiece piece)
		{
			if (piece == null || !CanEquip(piece))
			{
				lblItemName.Text = null;

				ArmorLevel = 0;

				txtArmorSet.Text = null;

				txtSpell1.Text = null;
				txtSpell2.Text = null;
				txtSpell3.Text = null;

				return;
			}

			lblItemName.Text = piece.Name;

			ArmorLevel = piece.ArmorLevel;

			txtArmorSet.Text = piece.ArmorSet;

			if (piece.Spells.Count >= 1) txtSpell1.Text = piece.Spells[0].Name;
			if (piece.Spells.Count >= 2) txtSpell2.Text = piece.Spells[1].Name;
			if (piece.Spells.Count >= 3) txtSpell3.Text = piece.Spells[2].Name;
		}
	}
}
