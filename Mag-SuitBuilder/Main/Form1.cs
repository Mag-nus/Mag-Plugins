using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using Mag_SuitBuilder.Equipment;
using Mag_SuitBuilder.Search;
using Mag_SuitBuilder.Spells;

namespace Mag_SuitBuilder
{
	public partial class Form1 : Form
	{
		readonly EquipmentGroup equipmentGroup = new EquipmentGroup();

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

			cboPrimaryArmorSet.Items.Add(ArmorSet.NoArmorSet);
			cboSecondaryArmorSet.Items.Add(ArmorSet.NoArmorSet);

			foreach (EquipmentPiece piece in equipmentGroup)
			{
				if (piece.ArmorSet == null || piece.ArmorSet == ArmorSet.NoArmorSet || (piece.EquipableSlots & Constants.EquippableSlotFlags.AllBodyArmor) == 0)
					continue;

				if (!cboPrimaryArmorSet.Items.Contains(piece.ArmorSet))
				{
					cboPrimaryArmorSet.Items.Add(piece.ArmorSet);
					cboSecondaryArmorSet.Items.Add(piece.ArmorSet);
				}
			}

			cboPrimaryArmorSet.Items.Add(ArmorSet.AnyArmorSet);
			cboSecondaryArmorSet.Items.Add(ArmorSet.AnyArmorSet);

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
				"Method 2 loads information from the windows clipboard (cntrl+c cntrl+v) that you may have generated using Mag-Tools->Misc->Tools->Clipboard Inventory Info." + Environment.NewLine + Environment.NewLine +
				"If items are showing up in your results that you do not want, simply select the row and hit the delete key to remove them from new searches.");
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

		ArmorSearcher armorSearcher;

		private void btnCalculatePossibilities_Click(object sender, System.EventArgs e)
		{
			btnCalculatePossibilities.Enabled = false;

			listBox1.Items.Clear();
			PopulateFromEquipmentGroup(null);

			if (armorSearcher != null)
			{
				armorSearcher.SuitCreated -= new Action<CompletedSuit>(suitBuilder_SuitCreated);
				armorSearcher.SearchCompleted -= new Action(suitBuilder_SearchCompleted);
			}

			SearcherConfiguration config = new SearcherConfiguration();
			config.MinimumArmorLevelPerPiece = int.Parse(txtMinimumBaseArmorLevel.Text);
			config.CantripsToLookFor = cntrlCantripFilters;
			config.PrimaryArmorSet = cboPrimaryArmorSet.SelectedItem as ArmorSet;
			config.SecondaryArmorSet = cboSecondaryArmorSet.SelectedItem as ArmorSet;
			config.OnlyAddPiecesWithArmor = true;

			// Build our base suit from locked in pieces
			/* todo hack fix
			for (int i = Equipment.Count - 1; i >= 0; i--)
			{
				if (Equipment[i].Locked)
				{
					SuitBuilder.Push(Equipment[i], Equipment[i].EquipableSlots);
					Equipment.RemoveAt(i);
				}
			}
			*/

			armorSearcher = new ArmorSearcher(config, equipmentGroup);

			armorSearcher.SuitCreated += new Action<CompletedSuit>(suitBuilder_SuitCreated);
			armorSearcher.SearchCompleted += new Action(suitBuilder_SearchCompleted);

			new Thread(() =>
			{
				DateTime starTime = DateTime.Now;

				// Do the actual search here
				armorSearcher.Start();

				DateTime endTime = DateTime.Now;

				//MessageBox.Show((endTime - starTime).TotalSeconds.ToString());
			}).Start();

			btnStopCalculating.Enabled = true;
			progressBar1.Style = ProgressBarStyle.Marquee;
		}

		void suitBuilder_SuitCreated(CompletedSuit obj)
		{
			BeginInvoke((MethodInvoker)(() =>
			{
				for (int i = 0 ; i < listBox1.Items.Count ; i++)
				{
					CompletedSuit suit = listBox1.Items[i] as CompletedSuit;

					if (suit != null && (suit.Count < obj.Count || (suit.Count == obj.Count && suit.TotalBaseArmorLevel < obj.TotalBaseArmorLevel)))
					{
						listBox1.Items.Insert(i, obj);
						return;
					}
				}

				listBox1.Items.Add(obj);
			}));
		}

		[DllImport("user32.dll")]
		static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

		void suitBuilder_SearchCompleted()
		{
			BeginInvoke((MethodInvoker)(() =>
			{
				progressBar1.Style = ProgressBarStyle.Blocks;
				btnStopCalculating.Enabled = false;
				btnCalculatePossibilities.Enabled = true;
				FlashWindow(this.Handle, true);
			}));
		}

		private void btnStopCalculating_Click(object sender, EventArgs e)
		{
			progressBar1.Style = ProgressBarStyle.Blocks;
			btnStopCalculating.Enabled = false;

			armorSearcher.Stop();

			btnCalculatePossibilities.Enabled = true;
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			CompletedSuit suit = ((ListBox)sender).SelectedItem as CompletedSuit;

			PopulateFromEquipmentGroup(suit);
		}

		private void PopulateFromEquipmentGroup(CompletedSuit suit)
		{
			if (suit == null)
				return;

			foreach (Control cntrl in tabPage1.Controls)
			{
				if (cntrl is EquipmentPieceControl)
				{
					EquipmentPieceControl coveragePiece = (cntrl as EquipmentPieceControl);

					coveragePiece.SetEquipmentPiece(suit[coveragePiece.EquipableSlots]);
					/*
					if (suit.ContainsKey(coveragePiece.EquipableSlots))
						coveragePiece.SetEquipmentPiece(suit[coveragePiece.EquipableSlots]);
					else
						coveragePiece.SetEquipmentPiece(null);
					*/
					cntrl.Refresh();
				}
			}

			cntrlSuitCantrips.Clear();

			foreach (Spell spell in suit.EffectiveSpells)
			{
				cntrlSuitCantrips.Add(spell);
			}

			cntrlSuitCantrips.Refresh();
		}
	}
}
