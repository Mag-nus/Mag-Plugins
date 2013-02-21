using System.Windows.Forms;

namespace Mag_InventoryViewer
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			Text += " " + Application.ProductVersion;
		}

		private void cmdLoadInventory_Click(object sender, System.EventArgs e)
		{

		}
	}
}
