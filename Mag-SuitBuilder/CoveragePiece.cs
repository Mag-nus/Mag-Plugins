using System.Windows.Forms;

namespace Mag_SuitBuilder
{
	public partial class CoveragePiece : UserControl
	{
		public CoveragePiece()
		{
			InitializeComponent();
		}

		public Constants.CoverageFlags SlotFlags { get; private set; }

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

		public Constants.CoverageFlags CoverageFlags { get; set; }

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
				txtArmorLevel.Text = value.ToString();
			}
		}

		public bool CanHaveArmorSet
		{
			get
			{
				return cbArmorSet.Visible;
			}
			set
			{
				cbArmorSet.Visible = value;
			}
		}
	}
}
