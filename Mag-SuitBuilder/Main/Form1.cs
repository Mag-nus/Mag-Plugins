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

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (btnStopCalculating.Enabled)
				btnStopCalculating.PerformClick();
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

			treeView1.Nodes.Clear();
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
			CompletedSuit baseSuit = new CompletedSuit();

			// Add pieces in order of slots covered, starting with the fewest
			for (int slotCount = 1; slotCount <= 5; slotCount++)
			{
				for (int i = 0; i < equipmentGroup.Count; i++)
				{
					if (equipmentGroup[i].Locked && equipmentGroup[i].EquipableSlots.GetTotalBitsSet() == slotCount)
					{
						if (slotCount == 1 || equipmentGroup[i].EquipableSlots.IsBodyArmor())
							// For body armor, we add it to whatever slots its marked as filling. We don't assume reduction.
							baseSuit.AddItem(equipmentGroup[i].EquipableSlots, equipmentGroup[i]);
						else
						{
							// This is broken for suits with multiple bracelets, rings, etc..
							// Iterate through the set slots and see if they are available
							baseSuit.AddItem(equipmentGroup[i].EquipableSlots, equipmentGroup[i]);
						}
					}
				}
			}

			armorSearcher = new ArmorSearcher(config, equipmentGroup, baseSuit);

			armorSearcher.SuitCreated += new Action<CompletedSuit>(suitBuilder_SuitCreated);
			armorSearcher.SearchCompleted += new Action(suitBuilder_SearchCompleted);

			new Thread(() =>
			{
				DateTime startTime = DateTime.Now;

				// Do the actual search here
				armorSearcher.Start();

				DateTime endTime = DateTime.Now;

				//MessageBox.Show((endTime - startTime).TotalSeconds.ToString());
			}).Start();

			btnStopCalculating.Enabled = true;
			progressBar1.Style = ProgressBarStyle.Marquee;
		}

		private class CompletedSuitTreeNode : TreeNode
		{
			public readonly CompletedSuit Suit;

			public CompletedSuitTreeNode(CompletedSuit suit)
			{
				Suit = suit;
				Text = suit.ToString();
			}
		}

		void suitBuilder_SuitCreated(CompletedSuit obj)
		{
			BeginInvoke((MethodInvoker)(() =>
			{
				CompletedSuitTreeNode newNode = new CompletedSuitTreeNode(obj);

				TreeNodeCollection nodes = FindDeepestNode(treeView1.Nodes, obj);

				for (int i = 0 ; i <= nodes.Count ; i++)
				{
					if (i == nodes.Count)
					{
						nodes.Add(newNode);
						break;
					}

					CompletedSuitTreeNode nodeAsSuit = (nodes[i] as CompletedSuitTreeNode);

					if (nodeAsSuit != null && (nodeAsSuit.Suit.Count < obj.Count || (nodeAsSuit.Suit.Count == obj.Count && nodeAsSuit.Suit.TotalBaseArmorLevel < obj.TotalBaseArmorLevel)))
					{
						nodes.Insert(i, newNode);
						break;
					}
				}

				treeView1.ExpandAll();
			}));
		}

		TreeNodeCollection FindDeepestNode(TreeNodeCollection nodes, CompletedSuit suit)
		{
			foreach (TreeNode node in nodes)
			{
				CompletedSuitTreeNode nodeAsSuit = (node as CompletedSuitTreeNode);

				if (nodeAsSuit != null && suit.IsProperSubsetOf(nodeAsSuit.Suit))
					return FindDeepestNode(node.Nodes, suit);
			}

			return nodes;
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

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			CompletedSuitTreeNode node = (e.Node as CompletedSuitTreeNode);

			if (node != null)
				PopulateFromEquipmentGroup(node.Suit);
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

					cntrl.Refresh();
				}
			}

			cntrlSuitCantrips.Clear();

			foreach (Spell spell in suit.EffectiveSpells)
				cntrlSuitCantrips.Add(spell);

			cntrlSuitCantrips.Refresh();
		}
	}
}
