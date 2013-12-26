using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
using Mag_SuitBuilder.Properties;
using Mag_SuitBuilder.Search;
using Mag_SuitBuilder.Spells;

using Mag.Shared.Constants;

// Bugs
// Spell compare is too slow, should be a hash compare

namespace Mag_SuitBuilder
{
	public partial class Form1 : Form
	{
		readonly EquipmentGroup boundList = new EquipmentGroup();

		public Form1()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			if ((ModifierKeys & Keys.Shift) == 0)
			{
				if (!Settings.Default.InitialSize.IsEmpty) Size = Settings.Default.InitialSize;
				if (!Settings.Default.InitialLocation.IsEmpty) Location = Settings.Default.InitialLocation;
			}

			Text += " " + Application.ProductVersion;
			txtInventoryRootPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-Tools\";

			typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, equipmentGrid, new object[] { true });
			equipmentGrid.DataSource = boundList;

			DataGridViewCellStyle style = new DataGridViewCellStyle();
			style.Format = "N3";

			foreach (DataGridViewColumn col in equipmentGrid.Columns)
			{
				if (col.HeaderText == "Variance" || col.HeaderText == "DamageBonus" || col.HeaderText == "ElementalDamageVersusMonsters" || col.HeaderText == "AttackBonus" || col.HeaderText == "MeleeDefenseBonus" || col.HeaderText == "MagicDBonus" || col.HeaderText == "MissileDBonus" || col.HeaderText == "ManaCBonus" ||
					col.HeaderText == "SalvageWorkmanship" ||
					col.HeaderText == "CalcedBuffedTinkedDoT" || col.HeaderText == "CalcedBuffedMissileDamage" || col.HeaderText == "BuffedElementalDamageVersusMonsters" || col.HeaderText == "BuffedAttackBonus" || col.HeaderText == "BuffedMeleeDefenseBonus" || col.HeaderText == "BuffedManaCBonus")
					col.DefaultCellStyle = style;
			}

			filtersControl1.FiltersChanged += () => UpdateBoundListFromTreeViewNodes(CharactersTreeView.Nodes);

			boundList.ListChanged += (s, e2) =>
			{
				foreach (var item in boundList)
					item.IsSurpassed = boundList.ItemIsSurpassed(item);
				equipmentGrid.Invalidate();
			};

			base.OnLoad(e);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			btnLoadFromDB_Click(null, null);
			
			if (String.IsNullOrEmpty(Settings.Default.ColumnWidths))
			{
				string originalText = txtInventoryRootPath.Text;
				txtInventoryRootPath.Text += "    Autosizing columns... If you have lots of inventory and a 486 this may take a while.";
				txtInventoryRootPath.Refresh();

				equipmentGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

				txtInventoryRootPath.Text = originalText;
			}
			else
			{
				var columnWidths = Settings.Default.ColumnWidths.Split(',');

				for (int i = 0 ; i < columnWidths.Length ; i++)
				{
					if (equipmentGrid.Columns.Count <= i)
						break;

					int width;
					if (int.TryParse(columnWidths[i], out width))
						equipmentGrid.Columns[i].Width = width;
				}
			}
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			if (btnStopCalculating.Enabled)
				btnStopCalculating.PerformClick();

			if ((ModifierKeys & Keys.Shift) == 0)
			{
				Settings.Default.InitialLocation = (WindowState == FormWindowState.Normal) ? Location : RestoreBounds.Location;
				Settings.Default.InitialSize = (WindowState == FormWindowState.Normal) ? Size : RestoreBounds.Size;
			}

			string columnWidths = null;
			foreach (DataGridViewColumn column in equipmentGrid.Columns)
				columnWidths += (columnWidths == null ? null : ",") + column.Width.ToString(CultureInfo.InvariantCulture);
			Settings.Default.ColumnWidths = columnWidths;
			Settings.Default.Save();

			base.OnClosing(e);
		}

		private void chkTree_CheckedChanged(object sender, EventArgs e)
		{
			CharactersTreeView.Visible = chkTree.Checked;
			filtersControl1.Visible = chkFilters.Checked;

		}

		private void btnLoadFromDB_Click(object sender, EventArgs e)
		{
			this.Enabled = false;

			XmlSerializer serializer = new XmlSerializer(typeof(List<SuitBuildableMyWorldObject>));

			CharactersTreeView.Nodes.Clear();
			boundList.Clear();

			Dictionary<string, int> armorSets = new Dictionary<string, int>();

			string txtInventoryRootPathOrig = txtInventoryRootPath.Text;

			string[] serverFolderPaths = Directory.GetDirectories(txtInventoryRootPath.Text);

			for (int i = 0; i < serverFolderPaths.Length; i++)
			{
				string serverName = serverFolderPaths[i].Substring(serverFolderPaths[i].LastIndexOf(Path.DirectorySeparatorChar) + 1, serverFolderPaths[i].Length - serverFolderPaths[i].LastIndexOf(Path.DirectorySeparatorChar) - 1);

				TreeNode serverNode = CharactersTreeView.Nodes.Add(serverName);

				string[] characterFilePaths = Directory.GetFiles(serverFolderPaths[i], "*.Inventory.xml", SearchOption.AllDirectories);

				for (int j = 0; j < characterFilePaths.Length; j++)
				{
					txtInventoryRootPath.Text = txtInventoryRootPathOrig + "   Server " + (i + 1) + " of " + serverFolderPaths.Length + ", Character " + (j + 1) + " of " + characterFilePaths.Length;
					txtInventoryRootPath.Refresh();

					string characterName = characterFilePaths[j].Substring(characterFilePaths[j].LastIndexOf(Path.DirectorySeparatorChar) + 1, characterFilePaths[j].Length - characterFilePaths[j].LastIndexOf(Path.DirectorySeparatorChar) - 1);
					characterName = characterName.Substring(0, characterName.IndexOf(".", StringComparison.Ordinal));

					TreeNode characterNode = serverNode.Nodes.Add(characterName);

					List<SuitBuildableMyWorldObject> myWorldObjects;

					// This is pretty hacked. SuitBuildableMyWorldObject is a derived class of MyWorldObject. It extends properties for the binding list.
					// Mag-Tools serializes MyWorldObjects.
					// I don't know how to deserialize those objects out as SuitBuildableMyWorldObjects.
					var fileContents = File.ReadAllText(characterFilePaths[j]);
					fileContents = fileContents.Replace("MyWorldObject", "SuitBuildableMyWorldObject");

					try
					{
						using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(fileContents)))
						using (XmlReader reader = XmlReader.Create(stream))
							myWorldObjects = (List<SuitBuildableMyWorldObject>) serializer.Deserialize(reader);

						foreach (var mwo in myWorldObjects)
						{
							mwo.Owner = characterName;
							mwo.BuiltItemSearchCache();
							if (mwo.ItemSetId != 0 && !armorSets.ContainsKey(mwo.ItemSet) && mwo.EquippableSlots.IsBodyArmor())
								armorSets.Add(mwo.ItemSet, mwo.ItemSetId);
						}

						characterNode.Tag = myWorldObjects;
					}
					catch (Exception ex)
					{
						MessageBox.Show("Error parsing file: " + characterFilePaths[j] + Environment.NewLine + "Try deleting the characters Name.Inventory.xml file and relog him." + Environment.NewLine + Environment.NewLine + ex);
					}
				}
			}

			armorSets = (from entry in armorSets orderby entry.Key ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
			filtersControl1.UpdateArmorSets(armorSets);

			txtInventoryRootPath.Text = txtInventoryRootPathOrig + "    Selecting all nodes...";
			txtInventoryRootPath.Refresh();

			CharactersTreeView.ExpandAll();

			foreach (TreeNode node in CharactersTreeView.Nodes)
				node.Checked = true;

			txtInventoryRootPath.Text = txtInventoryRootPathOrig;

			this.Enabled = true;
		}

		int count;
		private void inventoryTreeView_AfterCheck(object sender, TreeViewEventArgs e)
		{
			count++;

			CheckAllChildNodes(e.Node.Nodes, e.Node.Checked);

			if ((--count) == 0)
				UpdateBoundListFromTreeViewNodes(CharactersTreeView.Nodes);
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
						if (node.Checked && (piece.Locked || filtersControl1.ItemPassesFilters(piece)))
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

		private void cmdResizeColumns_Click(object sender, EventArgs e)
		{
			this.Enabled = false;
			equipmentGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
			this.Enabled = true;
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
			foreach (var item in boundList)
				item.IsSurpassed = boundList.ItemIsSurpassed(item);

			equipmentGrid.Invalidate();
		}

		private void equipmentGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			//if (boundList.ItemIsSurpassed(boundList[e.RowIndex])) // This method is real time but slow
			if (boundList[e.RowIndex].IsSurpassed)
				e.CellStyle.BackColor = Color.DarkGray;
		}

		private void equipmentGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			// This just hides numeric fields that aren't supported, they return -1
			if ((e.Value is int && (int)e.Value == -1) ||
				(e.Value is double && Math.Abs((double)e.Value + 1) < Double.Epsilon) ||
				(e.Value is EquippableSlotFlags && (EquippableSlotFlags)e.Value == EquippableSlotFlags.None) ||
				(e.Value is CoverageFlags && (CoverageFlags)e.Value == CoverageFlags.None))
			{
				e.PaintBackground(e.ClipBounds, true);
				e.Handled = true;
			}
		}

		private void CharactersTreeViewContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			bool characterSelected = CharactersTreeView.SelectedNode != null && CharactersTreeView.SelectedNode.Level >= 1;

			ShowEquipmentUpgradesMenuItem.Enabled = characterSelected;
		}

		private void ShowEquipmentUpgradesMenuItem_Click(object sender, EventArgs e)
		{
			if (CharactersTreeView.SelectedNode == null)
				return;

			EquipmentGroup characterEquipment = new EquipmentGroup();
			EquipmentGroup muleEquipment = new EquipmentGroup();

			foreach (var item in boundList)
			{
				if (item.Owner == CharactersTreeView.SelectedNode.Text && item.EquippedSlot != 0)
					characterEquipment.Add(item);

				if (item.Owner != CharactersTreeView.SelectedNode.Text && item.EquippedSlot == 0)
					muleEquipment.Add(item);
			}

			var upgrades = characterEquipment.GetUpgradeOptions(muleEquipment);

			EquipmentUpgradesForm equipmentUpgradesForm = new EquipmentUpgradesForm();
			equipmentUpgradesForm.Owner = this;

			equipmentUpgradesForm.Show();

			equipmentUpgradesForm.Update(upgrades);
		}












		ArmorSearcher armorSearcher;
		List<Searcher> accessorySearchers = new List<Searcher>(); // We use this list to stop accessory searchers when the user stops the build.

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

			accessorySearchers.Clear();

			SearcherConfiguration config = new SearcherConfiguration();
			config.CantripsToLookFor = filtersControl1.CantripsToLookFor;
			config.PrimaryArmorSet = filtersControl1.PrimaryArmorSetId;
			config.SecondaryArmorSet = filtersControl1.SecondaryArmorSetId;

			// Go through our Equipment and remove/disable any extra spells that we're not looking for
			foreach (var piece in boundList)
			{
				piece.SpellsToUseInSearch.Clear();

				foreach (Spell spell in piece.CachedSpells)
				{
					if (config.SpellPassesRules(spell) && !spell.IsOfSameFamilyAndGroup(Spell.GetSpell("Epic Impenetrability")))
						piece.SpellsToUseInSearch.Add(spell);
				}
			}

			// Build our base suit from locked in pieces
			CompletedSuit baseSuit = new CompletedSuit();

			// Add locked pieces in order of slots covered, starting with the fewest
			for (int slotCount = 1; slotCount <= 5; slotCount++)
			{
				foreach (SuitBuildableMyWorldObject item in boundList)
				{
					// Don't add items that we don't care about
					if (item.EquippableSlots == EquippableSlotFlags.None || item.EquippableSlots == EquippableSlotFlags.MeleeWeapon || item.EquippableSlots == EquippableSlotFlags.MissileWeapon || item.EquippableSlots == EquippableSlotFlags.TwoHandWeapon || item.EquippableSlots == EquippableSlotFlags.Wand || item.EquippableSlots == EquippableSlotFlags.MissileAmmo)
						continue;
					if (item.EquippableSlots == EquippableSlotFlags.Cloak || item.EquippableSlots == EquippableSlotFlags.BlueAetheria || item.EquippableSlots == EquippableSlotFlags.YellowAetheria || item.EquippableSlots == EquippableSlotFlags.RedAetheria)
						continue;

					if (item.Locked && item.EquippableSlots.GetTotalBitsSet() == slotCount)
					{
						try
						{
							if (item.EquippableSlots.GetTotalBitsSet() > 1 && item.EquippableSlots.IsBodyArmor() && MessageBox.Show(item.Name + " covers multiple slots. Would you like to reduce it?", "Add Item", MessageBoxButtons.YesNo) == DialogResult.Yes)
							{
								var reductionOptions = item.Coverage.ReductionOptions();

								EquippableSlotFlags slotFlag = EquippableSlotFlags.None;

								foreach (var option in reductionOptions)
								{
									if (option == CoverageFlags.Chest && baseSuit[EquippableSlotFlags.Chest] == null)			{ slotFlag = EquippableSlotFlags.Chest; break; }
									if (option == CoverageFlags.UpperArms && baseSuit[EquippableSlotFlags.UpperArms] == null)	{ slotFlag = EquippableSlotFlags.UpperArms; break; }
									if (option == CoverageFlags.LowerArms && baseSuit[EquippableSlotFlags.LowerArms] == null)	{ slotFlag = EquippableSlotFlags.LowerArms; break; }
									if (option == CoverageFlags.Abdomen && baseSuit[EquippableSlotFlags.Abdomen] == null)		{ slotFlag = EquippableSlotFlags.Abdomen; break; }
									if (option == CoverageFlags.UpperLegs && baseSuit[EquippableSlotFlags.UpperLegs] == null)	{ slotFlag = EquippableSlotFlags.UpperLegs; break; }
									if (option == CoverageFlags.LowerLegs && baseSuit[EquippableSlotFlags.LowerLegs] == null)	{ slotFlag = EquippableSlotFlags.LowerLegs; break; }
								}

								if (slotFlag == EquippableSlotFlags.None)
									MessageBox.Show("Unable to reduce " + item + " into an open single slot." + Environment.NewLine + "Reduction coverage option not expected or not open.");
								else
									baseSuit.AddItem(slotFlag, item);
							}
							else if (!baseSuit.AddItem(item))
								MessageBox.Show("Failed to add " + item.Name + " to base suit of armor.");
						}
						catch (ArgumentException) // Item failed to add
						{
							MessageBox.Show("Failed to add " + item.Name + " to base suit of armor. It overlaps another piece");
						}
					}
				}
			}

			if (baseSuit.Count > 0)
				AddCompletedSuitToTreeView(baseSuit);

			armorSearcher = new ArmorSearcher(config, boundList, baseSuit);

			armorSearcher.SuitCreated += new Action<CompletedSuit>(armorSearcher_SuitCreated);
			armorSearcher.SearchCompleted += new Action(armorSearcher_SearchCompleted);

			new Thread(() =>
			{
				//DateTime startTime = DateTime.Now;

				// Do the actual search here
				armorSearcher.Start();

				//DateTime endTime = DateTime.Now;

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
				AccessorySearcher accSearcher = new AccessorySearcher(new SearcherConfiguration(), boundList, obj);
				accessorySearchers.Add(accSearcher);
				accSearcher.SuitCreated += new Action<CompletedSuit>(accSearcher_SuitCreated);
				accSearcher.Start();
				accSearcher.SuitCreated -= new Action<CompletedSuit>(accSearcher_SuitCreated);
			});
		}

		void accSearcher_SuitCreated(CompletedSuit obj)
		{
			if (!IsDisposed)
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

			// Accessory searchers could still be running...
		}

		private void btnStopCalculating_Click(object sender, EventArgs e)
		{
			progressBar1.Style = ProgressBarStyle.Blocks;
			btnStopCalculating.Enabled = false;

			armorSearcher.Stop();

			foreach (Searcher searcher in accessorySearchers)
			{
				if (searcher != null && searcher.Running)
					searcher.Stop();
			}
			accessorySearchers.Clear();

			btnCalculatePossibilities.Enabled = true;
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			CompletedSuitTreeNode node = e.Node as CompletedSuitTreeNode;

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
					EquipmentPieceControl coveragePiece = cntrl as EquipmentPieceControl;

					coveragePiece.SetEquipmentPiece(suit[coveragePiece.EquippableSlots]);

					cntrl.Refresh();
				}
			}

			cntrlSuitCantrips.Clear();

			// This method adds every spell for all the items of the suit
			foreach (var kvp in suit)
			{
				foreach (var spell in kvp.Value.CachedSpells)
					cntrlSuitCantrips.Add(spell);
			}

			// This method only adds the spells that met our spell filter criteria
			//foreach (Spell spell in suit.EffectiveSpells)
			//	cntrlSuitCantrips.Add(spell);

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
