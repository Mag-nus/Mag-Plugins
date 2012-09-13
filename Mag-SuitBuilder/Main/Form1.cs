using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Mag_SuitBuilder
{
	public partial class Form1 : Form
	{
		EquipmentGroup equipmentGroup = new EquipmentGroup();

		public Form1()
		{
			InitializeComponent();

			Text = "Mag-SuitBuilder " + Application.ProductVersion;
	
			typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, equipmentGrid, new object[] { true });
			equipmentGrid.DataSource = equipmentGroup;
		}

		private void btnLoadFromDB_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Not implemented.");
		}

		private void btnLoadFromClipboard_Click(object sender, EventArgs e)
		{
			if (!Clipboard.ContainsText(TextDataFormat.Text))
			{
				MessageBox.Show("Clipboard does not contain text." + Environment.NewLine + "I'm expecting multi-line input (each line is an item) that resembles the following: " + Environment.NewLine + Environment.NewLine +
					"Copper Chainmail Leggings, AL 607, Tinks 10, Epic Invulnerability, Wield Lvl 150, Melee Defense 390 to Activate, Diff 262" + Environment.NewLine +
					"Gold Top, Tinks 2, Augmented Health III, Augmented Damage II, Major Storm Ward, Wield Lvl 150, Diff 410, Craft 9" + Environment.NewLine +
					"Iron Amuli Coat, Defender's Set, AL 618, Tinks 10, Epic Strength, Wield Lvl 180, Melee Defense 300 to Activate, Diff 160");
				return;
			}

			string text = Clipboard.GetText();

			string[] lines = Regex.Split(text, "\r\n");

			foreach (string line in lines)
			{
				if (String.IsNullOrEmpty(line.Trim()))
					continue;

				EquipmentPiece piece = new EquipmentPiece(line);
				equipmentGroup.Add(piece);
			}

			equipmentGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

			cboPrimaryArmorSet.Items.Clear();
			cboSecondaryArmorSet.Items.Clear();

			cboPrimaryArmorSet.Items.Add("No Armor Set");
			cboSecondaryArmorSet.Items.Add("No Armor Set");

			foreach (EquipmentPiece piece in equipmentGroup)
			{
				if (String.IsNullOrEmpty(piece.ArmorSet) || (piece.EquipableSlots & Constants.EquippableSlotFlags.AllBodyArmor) == 0)
					continue;

				if (!cboPrimaryArmorSet.Items.Contains(piece.ArmorSet))
				{
					cboPrimaryArmorSet.Items.Add(piece.ArmorSet);
					cboSecondaryArmorSet.Items.Add(piece.ArmorSet);
				}
			}

			cboPrimaryArmorSet.Items.Add("Any Armor Set");
			cboSecondaryArmorSet.Items.Add("Any Armor Set");

			cboPrimaryArmorSet.SelectedIndex = cboPrimaryArmorSet.Items.Count - 1;
			cboSecondaryArmorSet.SelectedIndex = cboSecondaryArmorSet.Items.Count - 1;
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			equipmentGroup.Clear();
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			MessageBox.Show("All columns can be sorted. Some column cells can be edited." + Environment.NewLine + Environment.NewLine +
				"Check the first checkbox (Locked) for pieces you want your suit built around." + Environment.NewLine + Environment.NewLine +
				"Rows in dark gray are equipment pieces that will be removed from the search as they are surpassed by another item." + Environment.NewLine + Environment.NewLine +
				"Method 1 loads all Charname.Inventory.xml files from MyDocuments\\Decal Plugins\\Mag-Tools\\ServerName(s) from all servers" + Environment.NewLine + Environment.NewLine +
				"Method 2 loads information from the windows clipboard (cntrl+c cntrl+v) that you may have generated using Mag-Tools->Misc->Tools->Clipboard Inventory Info.");
		}

		private void equipmentGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			equipmentGrid.InvalidateRow(e.RowIndex);
		}

		private void equipmentGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (equipmentGroup.ItemIsSurpassed(equipmentGroup[e.RowIndex]))
				e.CellStyle.BackColor = Color.DarkGray;
		}

		private void loadDefaultSpells_Click(object sender, EventArgs e)
		{
			string buttonText = (sender as Button).Text;

			cntrlCantripFilters.LoadDefaults(buttonText);
		}

		SuitSearcher suitBuilder;

		private void btnCalculatePossibilities_Click(object sender, System.EventArgs e)
		{
			btnCalculatePossibilities.Enabled = false;

			listBox1.Items.Clear();
			PopulateFromEquipmentGroup(null);

			if (suitBuilder != null)
			{
				suitBuilder.SuitCreated -= new Action<SuitBuilder>(suitBuilder_SuitCreated);
				suitBuilder.SearchCompleted -= new Action(suitBuilder_SearchCompleted);
			}

			SuitSearcherConfiguration config = new SuitSearcherConfiguration();
			config.MinimumArmorLevelPerPiece = int.Parse(txtMinimumBaseArmorLevel.Text);
			config.CantripsToLookFor = cntrlCantripFilters;
			config.PrimaryArmorSet = cboPrimaryArmorSet.Text;
			config.SecondaryArmorSet = cboSecondaryArmorSet.Text;
			config.OnlyAddPiecesWithArmor = true;

			suitBuilder = new SuitSearcher(config, equipmentGroup);

			suitBuilder.SuitCreated += new Action<SuitBuilder>(suitBuilder_SuitCreated);
			suitBuilder.SearchCompleted += new Action(suitBuilder_SearchCompleted);

			suitBuilder.Start();

			btnStopCalculating.Enabled = true;
		}

		void suitBuilder_SuitCreated(SuitBuilder obj)
		{
			// This is just a hack for now for testing
			BeginInvoke((MethodInvoker)(() => listBox1.Items.Add(obj)));
		}

		[DllImport("user32.dll")]
		static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

		void suitBuilder_SearchCompleted()
		{
			BeginInvoke((MethodInvoker)(() =>
				                            {
					                            btnStopCalculating.Enabled = false;
					                            btnCalculatePossibilities.Enabled = true;
				                            }
			                           ));

			FlashWindow(this.Handle, true);
		}

		private void btnStopCalculating_Click(object sender, EventArgs e)
		{
			btnStopCalculating.Enabled = false;

			suitBuilder.Stop();

			btnCalculatePossibilities.Enabled = true;
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			SuitBuilder suit = ((ListBox)sender).SelectedItem as SuitBuilder;

			PopulateFromEquipmentGroup(suit);
		}

		private void PopulateFromEquipmentGroup(SuitBuilder suit)
		{
			if (suit == null)
				return;

			Dictionary<Constants.EquippableSlotFlags, EquipmentPiece> suitEquipment = suit.GetEquipment();

			foreach (Control cntrl in tabPage1.Controls)
			{
				if (cntrl is EquipmentPieceControl)
				{
					EquipmentPieceControl coveragePiece = (cntrl as EquipmentPieceControl);

					if (suitEquipment.ContainsKey(coveragePiece.EquipableSlots))
						coveragePiece.SetEquipmentPiece(suitEquipment[coveragePiece.EquipableSlots]);
					else
						coveragePiece.SetEquipmentPiece(null);

					cntrl.Refresh();
				}
			}

			cntrlSuitCantrips.Clear();

			foreach (EquipmentPiece piece in suitEquipment.Values)
			{
				foreach (Spell spell in piece.Spells)
					cntrlSuitCantrips.Add(spell);
			}
		}
	}
}
