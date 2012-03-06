using System.Windows.Forms;

namespace Mag_SuitBuilder
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void calculatePossibilities_Click(object sender, System.EventArgs e)
		{
			if (txtEquipmentEntries.TextLength == 0)
			{
				MessageBox.Show("Enter some equipment in the equipment tab first.");

				return;
			}
		}
	}
}
