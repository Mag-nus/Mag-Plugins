using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using Mag_SuitBuilder.Equipment;
using Mag_SuitBuilder.Search;
using Mag_SuitBuilder.Spells;

using Mag.Shared;

// Bugs
// Spell compare is too slow, should be a hash compare
// AccessorySearcher thread pool keeps going even after user clicks stop.

namespace Mag_SuitBuilder
{
	public partial class Form1 : Form
	{
		readonly EquipmentGroup boundList = new EquipmentGroup();

		public Form1()
		{
			InitializeComponent();

			Text += " " + Application.ProductVersion;
			txtInventoryRootPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-Tools\";

			typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, equipmentGrid, new object[] { true });
			equipmentGrid.DataSource = boundList;

			filtersControl1.FiltersChanged += () => UpdateBoundListFromTreeViewNodes(inventoryTreeView.Nodes);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (btnStopCalculating.Enabled)
				btnStopCalculating.PerformClick();
		}

		private void chkTree_CheckedChanged(object sender, EventArgs e)
		{
			inventoryTreeView.Visible = chkTree.Checked;
			filtersControl1.Visible = chkFilters.Checked;

		}
		private void btnLoadFromDB_Click(object sender, EventArgs e)
		{
			this.Enabled = false;

			XmlSerializer serializer = new XmlSerializer(typeof(List<SuitBuildableMyWorldObject>));

			inventoryTreeView.Nodes.Clear();
			boundList.Clear();

			Dictionary<string, long> armorSets = new Dictionary<string, long>();

			string txtInventoryRootPathOrig = txtInventoryRootPath.Text;

			string[] serverFolderPaths = Directory.GetDirectories(txtInventoryRootPath.Text);

			for (int i = 0; i < serverFolderPaths.Length; i++)
			{
				string serverName = serverFolderPaths[i].Substring(serverFolderPaths[i].LastIndexOf(Path.DirectorySeparatorChar) + 1, serverFolderPaths[i].Length - serverFolderPaths[i].LastIndexOf(Path.DirectorySeparatorChar) - 1);

				TreeNode serverNode = inventoryTreeView.Nodes.Add(serverName);

				string[] characterFilePaths = Directory.GetFiles(serverFolderPaths[i], "*.Inventory.xml", SearchOption.AllDirectories);

				for (int j = 0; j < characterFilePaths.Length; j++)
				{
					txtInventoryRootPath.Text = txtInventoryRootPathOrig + "   Server " + (i + 1) + " of " + serverFolderPaths.Length + ", Character " + (j + 1) + " of " + characterFilePaths.Length;
					txtInventoryRootPath.Refresh();

					string characterName = characterFilePaths[j].Substring(characterFilePaths[j].LastIndexOf(Path.DirectorySeparatorChar) + 1, characterFilePaths[j].Length - characterFilePaths[j].LastIndexOf(Path.DirectorySeparatorChar) - 1);
					characterName = characterName.Substring(0, characterName.IndexOf("."));

					TreeNode characterNode = serverNode.Nodes.Add(characterName);

					List<SuitBuildableMyWorldObject> myWorldObjects = new List<SuitBuildableMyWorldObject>();

					// This is pretty hacked. SuitBuildableMyWorldObject is a derived class of MyWorldObject. It extends properties for the binding list.
					// Mag-Tools serializes MyWorldObjects.
					// I don't know how to deserialize those objects out as SuitBuildableMyWorldObjects.
					var fileContents = File.ReadAllText(characterFilePaths[j]);
					fileContents = fileContents.Replace("MyWorldObject", "SuitBuildableMyWorldObject");

					using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(fileContents)))
					using (XmlReader reader = XmlReader.Create(stream))
							myWorldObjects = (List<SuitBuildableMyWorldObject>)serializer.Deserialize(reader);

					foreach (var mwo in myWorldObjects)
					{
						mwo.Owner = characterName;
						mwo.BuildSpellCache();
						if (mwo.ItemSetId != -1 && !armorSets.ContainsKey(mwo.ItemSet) && mwo.EquippableSlot.IsBodyArmor())
							armorSets.Add(mwo.ItemSet, mwo.ItemSetId);
					}

					characterNode.Tag = myWorldObjects;
				}
			}

			armorSets = (from entry in armorSets orderby entry.Key ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
			filtersControl1.UpdateArmorSets(armorSets);

			txtInventoryRootPath.Text = txtInventoryRootPathOrig + "    Selecting all nodes...";
			txtInventoryRootPath.Refresh();

			inventoryTreeView.ExpandAll();

			if (inventoryTreeView.Nodes.Count > 0)
				inventoryTreeView.Nodes[0].Checked = true;

			txtInventoryRootPath.Text = txtInventoryRootPathOrig + "    Autosizing columns... If you have lots of inventory and a 486 this may take a while.";
			txtInventoryRootPath.Refresh();

			//equipmentGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

			txtInventoryRootPath.Text = txtInventoryRootPathOrig;

			this.Enabled = true;
		}

		int count;
		private void inventoryTreeView_AfterCheck(object sender, TreeViewEventArgs e)
		{
			count++;

			CheckAllChildNodes(e.Node.Nodes, e.Node.Checked);

			if ((--count) == 0)
				UpdateBoundListFromTreeViewNodes(inventoryTreeView.Nodes);
		}

		void CheckAllChildNodes(TreeNodeCollection nodes, bool checkedState)
		{
			foreach (TreeNode node in nodes)
			{
				node.Checked = checkedState;
				CheckAllChildNodes(node.Nodes, checkedState);
			}
		}
	
		void UpdateBoundListFromTreeViewNodes(TreeNodeCollection nodes, bool clear = true)
		{
			if (clear)
			{
				boundList.RaiseListChangedEvents = false;
				boundList.Clear();
			}

			foreach (TreeNode node in nodes)
			{
				if (node.Tag is List<SuitBuildableMyWorldObject>)
				{
					foreach (SuitBuildableMyWorldObject piece in (node.Tag as List<SuitBuildableMyWorldObject>))
					{
						if (node.Checked && filtersControl1.ItemPassesFilters(piece))
							boundList.Add(piece);
					}
				}

				UpdateBoundListFromTreeViewNodes(node.Nodes, false);
			}

			if (clear)
			{
				boundList.RaiseListChangedEvents = true;
				boundList.ResetBindings();
			}
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			MessageBox.Show("All columns can be sorted. Some column cells can be edited." + Environment.NewLine + Environment.NewLine +
				"Use the [Locked] checkbox for pieces you want your suit built around." + Environment.NewLine + Environment.NewLine +
				"Rows in dark gray are equipment pieces that will be removed from the search as they are surpassed by another item." + Environment.NewLine + Environment.NewLine +
				"[Load Inventory] loads all Charname.Inventory.xml files from MyDocuments\\Decal Plugins\\Mag-Tools\\ServerName(s) from all servers." + Environment.NewLine +
				"Enable inventory logging in Mag-Tools under Misc->Options->Inventory Logger Enabled" + Environment.NewLine + Environment.NewLine +
				"If items are showing up in your results that you do not want, simply check the [Exclude] column.");
		}

		private void equipmentGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			equipmentGrid.InvalidateRow(e.RowIndex);
		}

		private void equipmentGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			// This needs to be done differently
			//if (boundList.ItemIsSurpassed(boundList[e.RowIndex]))
			//	e.CellStyle.BackColor = Color.DarkGray;
		}

		private void equipmentGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			// This just hides numeric fields that aren't supported, they return -1
			if ((e.Value is int && (int)e.Value == -1) ||
				(e.Value is double && (double)e.Value == -1) ||
				(e.Value is EquippableSlotFlags && (EquippableSlotFlags)e.Value == EquippableSlotFlags.None) ||
				(e.Value is CoverageFlags && (CoverageFlags)e.Value == CoverageFlags.None))
			{
				e.PaintBackground(e.ClipBounds, true);
				e.Handled = true;
			}
		}















		ArmorSearcher armorSearcher;

		private void btnCalculatePossibilities_Click(object sender, System.EventArgs e)
		{
			btnCalculatePossibilities.Enabled = false;

			treeView1.Nodes.Clear();
			PopulateFromEquipmentGroup(null);

			if (armorSearcher != null)
			{
				armorSearcher.SuitCreated -= new Action<CompletedSuit>(armorSearcher_SuitCreated);
				armorSearcher.SearchCompleted -= new Action(armorSearcher_SearchCompleted);
			}

			SearcherConfiguration config = new SearcherConfiguration();
			//config.MinimumArmorLevelPerPiece = int.Parse(txtMinimumBaseArmorLevel.Text);
			//config.CantripsToLookFor = cntrlCantripFilters;
			//config.PrimaryArmorSet = cboPrimaryArmorSet.SelectedItem as ArmorSet;
			//config.SecondaryArmorSet = cboSecondaryArmorSet.SelectedItem as ArmorSet;
			config.OnlyAddPiecesWithArmor = true;
			/* todo hack fix
			// Go through our Equipment and remove/disable any extra spells that we're not looking for
			foreach (var piece in equipmentGroup)
			{
				piece.SpellsToUseInSearch.Clear();

				foreach (Spell spell in piece.Spells)
				{
					if (config.SpellPassesRules(spell))
						piece.SpellsToUseInSearch.Add(spell);
				}
			}*/

			// Build our base suit from locked in pieces
			CompletedSuit baseSuit = new CompletedSuit();

			// Add pieces in order of slots covered, starting with the fewest
			for (int slotCount = 1; slotCount <= 5; slotCount++)
			{/* todo hack fix
				for (int i = 0; i < equipmentGroup.Count; i++)
				{
					if (equipmentGroup[i].Locked && equipmentGroup[i].EquipableSlots.GetTotalBitsSet() == slotCount)
					{
						try
						{
							if (slotCount == 1 || equipmentGroup[i].EquipableSlots.IsBodyArmor())
								// For body armor, we add it to whatever slots its marked as filling. We don't assume reduction.
								baseSuit.AddItem(equipmentGroup[i].EquipableSlots, equipmentGroup[i]);
							else
							{
								if (equipmentGroup[i].EquipableSlots == EquippableSlotFlags.Bracelet &&
								    baseSuit[EquippableSlotFlags.LeftBracelet] == null)
									baseSuit.AddItem(EquippableSlotFlags.LeftBracelet, equipmentGroup[i]);
								else if (equipmentGroup[i].EquipableSlots == EquippableSlotFlags.Bracelet &&
								         baseSuit[EquippableSlotFlags.RightBracelet] == null)
									baseSuit.AddItem(EquippableSlotFlags.RightBracelet, equipmentGroup[i]);
								else if (equipmentGroup[i].EquipableSlots == EquippableSlotFlags.Ring &&
								         baseSuit[EquippableSlotFlags.LeftRing] == null)
									baseSuit.AddItem(EquippableSlotFlags.LeftRing, equipmentGroup[i]);
								else if (equipmentGroup[i].EquipableSlots == EquippableSlotFlags.Ring &&
								         baseSuit[EquippableSlotFlags.RightRing] == null)
									baseSuit.AddItem(EquippableSlotFlags.RightRing, equipmentGroup[i]);
								else
									baseSuit.AddItem(equipmentGroup[i].EquipableSlots, equipmentGroup[i]);
							}
						}
						catch (ArgumentException)
						{
							MessageBox.Show("Failed to add " + equipmentGroup[i].Name + " to base suit of armor.");
						}
					}
				}*/
			}

			if (baseSuit.Count > 0)
				AddCompletedSuitToTreeView(baseSuit);

			//armorSearcher = new ArmorSearcher(config, equipmentGroup, baseSuit);

			armorSearcher.SuitCreated += new Action<CompletedSuit>(armorSearcher_SuitCreated);
			armorSearcher.SearchCompleted += new Action(armorSearcher_SearchCompleted);

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

		void armorSearcher_SuitCreated(CompletedSuit obj)
		{
			BeginInvoke((MethodInvoker)(() => AddCompletedSuitToTreeView(obj)));

			ThreadPool.QueueUserWorkItem(delegate
			{
				/*
				AccessorySearcher accSearcher = new AccessorySearcher(new SearcherConfiguration(), equipmentGroup, obj);
				accSearcher.SuitCreated += new Action<CompletedSuit>(accSearcher_SuitCreated);
				accSearcher.Start();
				accSearcher.SuitCreated -= new Action<CompletedSuit>(accSearcher_SuitCreated);
				*/
			});
		}

		void accSearcher_SuitCreated(CompletedSuit obj)
		{
			BeginInvoke((MethodInvoker)(() => AddCompletedSuitToTreeView(obj)));
		}

		void AddCompletedSuitToTreeView(CompletedSuit suit)
		{
			CompletedSuitTreeNode newNode = new CompletedSuitTreeNode(suit);

			TreeNodeCollection nodes = FindDeepestNode(treeView1.Nodes, suit);

			for (int i = 0; i <= nodes.Count; i++)
			{
				if (i == nodes.Count)
				{
					nodes.Add(newNode);
					break;
				}

				CompletedSuitTreeNode nodeAsSuit = (nodes[i] as CompletedSuitTreeNode);

				//if (nodeAsSuit != null && (nodeAsSuit.Suit.Count < suit.Count || (nodeAsSuit.Suit.Count == suit.Count && nodeAsSuit.Suit.TotalBaseArmorLevel < suit.TotalBaseArmorLevel)))
				if (nodeAsSuit != null)
				{
					if (nodeAsSuit.Suit.TotalBaseArmorLevel < suit.TotalBaseArmorLevel)
					{
						nodes.Insert(i, newNode);
						break;
					}
					
					if ((nodeAsSuit.Suit.TotalBaseArmorLevel == suit.TotalBaseArmorLevel) && ((nodeAsSuit.Suit.TotalEffectiveEpics < suit.TotalEffectiveEpics) || (nodeAsSuit.Suit.TotalEffectiveEpics == suit.TotalEffectiveEpics && nodeAsSuit.Suit.TotalEffectiveMajors < suit.TotalEffectiveMajors)))
					{
						nodes.Insert(i, newNode);
						break;
					}
				}
			}
		}

		TreeNodeCollection FindDeepestNode(TreeNodeCollection nodes, CompletedSuit suit)
		{
			foreach (TreeNode node in nodes)
			{
				CompletedSuitTreeNode nodeAsSuit = (node as CompletedSuitTreeNode);

				if (nodeAsSuit != null && suit.IsProperSupersetOf(nodeAsSuit.Suit))
					return FindDeepestNode(node.Nodes, suit);
			}

			return nodes;
		}

		[DllImport("user32.dll")]
		static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

		void armorSearcher_SearchCompleted()
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

		private void cmdExpandAll_Click(object sender, EventArgs e)
		{
			treeView1.ExpandAll();
		}

		private void cmdCollapseAll_Click(object sender, EventArgs e)
		{
			treeView1.CollapseAll();
		}
	}
}
