using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

using Mag.Shared;
using Mag.Shared.Constants;
using Mag.Shared.Spells;

namespace Mag_SuitBuilder
{
	public partial class Form1 : Form
	{
		readonly SortableBindingList<ExtendedMyWorldObject> boundList = new SortableBindingList<ExtendedMyWorldObject>();

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

			XmlSerializer serializer = new XmlSerializer(typeof(List<ExtendedMyWorldObject>));

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

					List<ExtendedMyWorldObject> myWorldObjects;

					// This is pretty hacked. SuitBuildableMyWorldObject is a derived class of MyWorldObject. It extends properties for the binding list.
					// Mag-Tools serializes MyWorldObjects.
					// I don't know how to deserialize those objects out as SuitBuildableMyWorldObjects.
					var fileContents = File.ReadAllText(characterFilePaths[j]);
					fileContents = fileContents.Replace("MyWorldObject", "ExtendedMyWorldObject");

					try
					{
						using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(fileContents)))
						using (XmlReader reader = XmlReader.Create(stream))
							myWorldObjects = (List<ExtendedMyWorldObject>) serializer.Deserialize(reader);

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

			if (CharactersTreeView.Nodes.Count == 1)
				CharactersTreeView.Nodes[0].Checked = true;
			//foreach (TreeNode node in CharactersTreeView.Nodes)
			//	node.Checked = true;

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
				Cursor.Current = Cursors.WaitCursor;

				boundList.RaiseListChangedEvents = false;
				boundList.Clear();
			}

			foreach (TreeNode node in nodes)
			{
				if (node.Tag is List<ExtendedMyWorldObject>)
				{
					foreach (ExtendedMyWorldObject piece in (node.Tag as List<ExtendedMyWorldObject>))
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

				Cursor.Current = Cursors.Arrow;
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
				"[Load Inventory] loads all Charname.Inventory.xml files from MyDocuments\\Decal Plugins\\Mag-Tools\\ServerName(s) from all servers." + Environment.NewLine +
				"Enable inventory logging in Mag-Tools under Misc->Options->Inventory Logger Enabled" + Environment.NewLine + Environment.NewLine +
				"If items are showing up in your results that you do not want, simply check the [Exclude] column.");
		}

		private void copyItemsToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var sb = new StringBuilder();

			if (equipmentGrid.SelectedRows.Count == 0)
				return;

			foreach (DataGridViewRow row in equipmentGrid.SelectedRows)
			{
				var rowAsMyWorldObject = row.DataBoundItem as MyWorldObject;

				if (rowAsMyWorldObject != null)
				{
					var itemInfo = new ItemInfo(rowAsMyWorldObject);
					sb.AppendLine(itemInfo.ToString());
				}
			}

			try
			{
				Clipboard.SetText(sb.ToString());
			}
			catch { }
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

			var characterEquipment = new List<ExtendedMyWorldObject>();
			var muleEquipment = new List<ExtendedMyWorldObject>();

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










		List<LeanMyWorldObject> searchItems;

		DateTime startTime;

		ArmorSearcher armorSearcher;
		long armorThreadCounter;
		int armorSearcherHighestItemCount;

		readonly ConcurrentDictionary<Searcher, int> accessorySearchers = new ConcurrentDictionary<Searcher, int>(); // We use this list to stop accessory searchers when the user stops the build.
		long accessoryThreadQueueCounter;
		long accessoryThreadRunningCounter;

		bool abortedSearch;

		private void btnCalculatePossibilities_Click(object sender, System.EventArgs e)
		{
			if (filtersControl1.CantripsToLookFor.Count == 0)
			{
				if (MessageBox.Show("You have no spells selected. Your search results won't be very useful. Would you like to go ahead anyway?" + Environment.NewLine + Environment.NewLine + "To select spells, load defsults or click the spells you want on the bottom of the filters group on Tab labeled 'Step 1. Add Inventory'", "No Spells Selected", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
					return;
			}

			btnCalculatePossibilities.Enabled = false;

			treeView1.Nodes.Clear();
			PopulateFromEquipmentGroup(null);

			if (armorSearcher != null)
			{
				armorSearcher.SuitCreated -= new Action<CompletedSuit>(armorSearcher_SuitCreated);
				armorSearcher.SearchCompleted -= new Action(ThreadFinished);
			}

			accessorySearchers.Clear();

			abortedSearch = false;

			SearcherConfiguration config = new SearcherConfiguration();
			config.CantripsToLookFor = filtersControl1.CantripsToLookFor;
			config.PrimaryArmorSet = filtersControl1.PrimaryArmorSetId;
			config.SecondaryArmorSet = filtersControl1.SecondaryArmorSetId;

			// Build the list of items we're going to use in our search
			searchItems = new List<LeanMyWorldObject>();

			// Only add items that meet our minimum requirements
			foreach (var piece in boundList)
			{
				if (piece.Locked || (!piece.Exclude && config.ItemPassesRules(piece)))
					searchItems.Add(new LeanMyWorldObject(piece));
			}

			var possibleSpells = new List<Spell>();

			// Go through our Equipment and remove/disable any extra spells that we're not looking for
			foreach (var piece in searchItems)
			{
				piece.SpellsToUseInSearch.Clear();

				foreach (Spell spell in piece.ExtendedMyWorldObject.CachedSpells)
				{
					if (config.SpellPassesRules(spell) && !spell.IsOfSameFamilyAndGroup(SpellTools.GetSpell(4667))) // Epic Impenetrability
					{
						piece.SpellsToUseInSearch.Add(spell);

						if (!possibleSpells.Contains(spell))
							possibleSpells.Add(spell);
					}
				}
			}

			// Go through our possible spells and make sure they are all unique. This will also keep the lowest level spell that passed our rules
			for (int i = possibleSpells.Count - 1; i >= 0 ; i--)
			{
				for (int j = 0; j < i ; j++)
				{
					if (possibleSpells[j].IsOfSameFamilyAndGroup(possibleSpells[i]))
					{
						if (possibleSpells[j].Surpasses(possibleSpells[i]))
							possibleSpells.RemoveAt(j);
						else
							possibleSpells.RemoveAt(j);

						goto next;
					}
				}

				next:;
			}

			// Now, we create our bitmapped spell map
			if (possibleSpells.Count > 32)
			{
				MessageBox.Show("Too many spells.");
				btnCalculatePossibilities.Enabled = true;
				return;
			}

			Dictionary<Spell, int> spellMap = new Dictionary<Spell, int>();

			for (int i = 0; i < possibleSpells.Count; i++)
				spellMap.Add(possibleSpells[i], 1 << i);

			// Now, we update each item with the new spell map
			foreach (var piece in searchItems)
			{
				piece.SpellBitmap = 0;

				foreach (var spell in piece.SpellsToUseInSearch)
				{
					foreach (var kvp in spellMap)
					{
						if (spell.IsOfSameFamilyAndGroup(kvp.Key))
							piece.SpellBitmap |= kvp.Value;
					}
				}
			}

			// Build our base suit from locked in pieces
			CompletedSuit baseSuit = new CompletedSuit();

			// Add locked pieces in order of slots covered, starting with the fewest
			for (int slotCount = 1; slotCount <= 5; slotCount++)
			{
				foreach (var item in searchItems)
				{
					// Don't add items that we don't care about
					if (item.EquippableSlots == EquippableSlotFlags.None || item.EquippableSlots == EquippableSlotFlags.MeleeWeapon || item.EquippableSlots == EquippableSlotFlags.MissileWeapon || item.EquippableSlots == EquippableSlotFlags.TwoHandWeapon || item.EquippableSlots == EquippableSlotFlags.Wand || item.EquippableSlots == EquippableSlotFlags.MissileAmmo)
						continue;
					if (item.EquippableSlots == EquippableSlotFlags.Cloak || item.EquippableSlots == EquippableSlotFlags.BlueAetheria || item.EquippableSlots == EquippableSlotFlags.YellowAetheria || item.EquippableSlots == EquippableSlotFlags.RedAetheria)
						continue;

					if (item.ExtendedMyWorldObject.Locked && item.EquippableSlots.GetTotalBitsSet() == slotCount)
					{
						try
						{
							if (item.EquippableSlots.GetTotalBitsSet() > 1 && item.EquippableSlots.IsBodyArmor() && MessageBox.Show(item.ExtendedMyWorldObject.Name + " covers multiple slots. Would you like to reduce it?", "Add Item", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
									if (option == CoverageFlags.Feet && baseSuit[EquippableSlotFlags.Feet] == null)				{ slotFlag = EquippableSlotFlags.Feet; break; }
								}

								if (slotFlag == EquippableSlotFlags.None)
									MessageBox.Show("Unable to reduce " + item + " into an open single slot." + Environment.NewLine + "Reduction coverage option not expected or not open.");
								else
									baseSuit.AddItem(slotFlag, item);
							}
							else if (!baseSuit.AddItem(item))
								MessageBox.Show("Failed to add " + item.ExtendedMyWorldObject.Name + " to base suit of armor.");
						}
						catch (ArgumentException) // Item failed to add
						{
							MessageBox.Show("Failed to add " + item.ExtendedMyWorldObject.Name + " to base suit of armor. It overlaps another piece");
						}
					}
				}
			}

			if (baseSuit.Count > 0)
				AddCompletedSuitToTreeView(baseSuit);

			armorSearcher = new ArmorSearcher(config, searchItems, baseSuit);
			armorSearcherHighestItemCount = 0;

			armorSearcher.SuitCreated += new Action<CompletedSuit>(armorSearcher_SuitCreated);
			armorSearcher.SearchCompleted += new Action(ThreadFinished);

			startTime = DateTime.Now;

			armorThreadCounter = 1;
			accessoryThreadQueueCounter = 0;
			accessoryThreadRunningCounter = 0;

			timerCalculatorUpdator.Start();

			new Thread(() =>
			{
				// Do the actual search here
				armorSearcher.Start();

				Interlocked.Decrement(ref armorThreadCounter);
				ThreadFinished();
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

		readonly Object lockObject = new Object();

		void armorSearcher_SuitCreated(CompletedSuit obj)
		{
			BeginInvoke((MethodInvoker)(() => AddCompletedSuitToTreeView(obj)));

			lock (lockObject)
			{
				// We don't accessorise suits that have less items than our best suit
				if (obj.Count < armorSearcherHighestItemCount)
					return;

				if (obj.Count > armorSearcherHighestItemCount)
				{
					armorSearcherHighestItemCount = obj.Count;

					// Disable previous accessory searchers that have a lower initial item count
					foreach (var kvp in accessorySearchers)
					{
						if (kvp.Value < armorSearcherHighestItemCount && kvp.Key != null && kvp.Key.Running)
							kvp.Key.Stop();
					}
				}
			}

			Interlocked.Increment(ref accessoryThreadQueueCounter);

			ThreadPool.QueueUserWorkItem(delegate
			{
				AccessorySearcher accSearcher;

				lock (lockObject)
				{
					if (abortedSearch || obj.Count < armorSearcherHighestItemCount)
					{
						Interlocked.Decrement(ref accessoryThreadQueueCounter);
						ThreadFinished();
						return;
					}

					Interlocked.Increment(ref accessoryThreadRunningCounter);

					accSearcher = new AccessorySearcher(new SearcherConfiguration(), searchItems, obj);
					accessorySearchers.TryAdd(accSearcher, obj.Count);
				}

				accSearcher.SuitCreated += new Action<CompletedSuit>(accSearcher_SuitCreated);
				accSearcher.Start();
				accSearcher.SuitCreated -= new Action<CompletedSuit>(accSearcher_SuitCreated);

				Interlocked.Decrement(ref accessoryThreadRunningCounter);
				Interlocked.Decrement(ref accessoryThreadQueueCounter);
				ThreadFinished();
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
					if (nodeAsSuit.Suit.Count < suit.Count)
					{
						nodes.Insert(i, newNode);
						break;
					}

					if (nodeAsSuit.Suit.Count == suit.Count)
					{
						if (nodeAsSuit.Suit.TotalBaseArmorLevel < suit.TotalBaseArmorLevel)
						{
							nodes.Insert(i, newNode);
							break;
						}

						if (nodeAsSuit.Suit.TotalBaseArmorLevel == suit.TotalBaseArmorLevel)
						{
							if ((nodeAsSuit.Suit.TotalEffectiveLegendaries < suit.TotalEffectiveLegendaries) || (nodeAsSuit.Suit.TotalEffectiveLegendaries == suit.TotalEffectiveLegendaries && nodeAsSuit.Suit.TotalEffectiveEpics < suit.TotalEffectiveEpics))
							{
								nodes.Insert(i, newNode);
								break;
							}
						}
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

		void ThreadFinished()
		{
			if (Interlocked.Read(ref armorThreadCounter) == 0 && Interlocked.Read(ref accessoryThreadQueueCounter) == 0)
			{
				BeginInvoke((MethodInvoker)(() =>
				{
					progressBar1.Style = ProgressBarStyle.Blocks;
					btnStopCalculating.Enabled = false;
					btnCalculatePossibilities.Enabled = true;
					FlashWindow(this.Handle, true);

					#if DEBUG
					//MessageBox.Show("Search finished in: " + (DateTime.Now - startTime).TotalSeconds + " seconds.");
					#endif
				}));
			}
		}

		private void btnStopCalculating_Click(object sender, EventArgs e)
		{
			progressBar1.Style = ProgressBarStyle.Blocks;
			btnStopCalculating.Enabled = false;

			abortedSearch = true;

			armorSearcher.Stop();

			foreach (Searcher searcher in accessorySearchers.Keys)
			{
				if (searcher != null && searcher.Running)
					searcher.Stop();
			}

			accessorySearchers.Clear();

			Thread.Sleep(500); // Give the threads time to stop

			timerCalculatorUpdator_Tick(null, null);
			timerCalculatorUpdator.Stop();

			btnCalculatePossibilities.Enabled = true;
		}

		private void timerCalculatorUpdator_Tick(object sender, EventArgs e)
		{
			lblArmorSearchThreads.Text = "Armor Search Threads: " + Interlocked.Read(ref armorThreadCounter);
			lblAccessorizerQueuedThreads.Text = "Accessorizer Queued Threads: " + (Interlocked.Read(ref accessoryThreadQueueCounter) - Interlocked.Read(ref accessoryThreadRunningCounter));
			lblAccessorizerRunningThreads.Text = "Accessorizer Running Threads: " + Interlocked.Read(ref accessoryThreadRunningCounter);
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

					coveragePiece.SetEquipmentPiece(suit[coveragePiece.EquippableSlots] == null ? null : suit[coveragePiece.EquippableSlots].ExtendedMyWorldObject);

					cntrl.Refresh();
				}
			}

			cntrlSuitCantrips.Clear();

			// This method adds every spell for all the items of the suit
			foreach (var kvp in suit)
			{
				foreach (var spell in kvp.Value.ExtendedMyWorldObject.CachedSpells)
					cntrlSuitCantrips.Add(spell);
			}

			// This method only adds the spells that met our spell filter criteria
			//foreach (Spell spell in suit.EffectiveSpells)
			//	cntrlSuitCantrips.Add(spell);

			cntrlSuitCantrips.Refresh();
		}

		private void cmdCopyToClipboard_Click(object sender, EventArgs e)
		{
			var sb = new StringBuilder();

			CompletedSuitTreeNode node = treeView1.SelectedNode as CompletedSuitTreeNode;

			if (node == null)
				return;

			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.Head] == null ? null : node.Suit[EquippableSlotFlags.Head].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.Chest] == null ? null : node.Suit[EquippableSlotFlags.Chest].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.Abdomen] == null ? null : node.Suit[EquippableSlotFlags.Abdomen].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.UpperArms] == null ? null : node.Suit[EquippableSlotFlags.UpperArms].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.LowerArms] == null ? null : node.Suit[EquippableSlotFlags.LowerArms].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.Hands] == null ? null : node.Suit[EquippableSlotFlags.Hands].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.UpperLegs] == null ? null : node.Suit[EquippableSlotFlags.UpperLegs].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.LowerLegs] == null ? null : node.Suit[EquippableSlotFlags.LowerLegs].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.Feet] == null ? null : node.Suit[EquippableSlotFlags.Feet].ExtendedMyWorldObject, sb);
			sb.AppendLine();
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.ShirtChest] == null ? null : node.Suit[EquippableSlotFlags.ShirtChest].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.PantsUpperLegs] == null ? null : node.Suit[EquippableSlotFlags.PantsUpperLegs].ExtendedMyWorldObject, sb);
			sb.AppendLine();
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.Necklace] == null ? null : node.Suit[EquippableSlotFlags.Necklace].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.Trinket] == null ? null : node.Suit[EquippableSlotFlags.Trinket].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.LeftBracelet] == null ? null : node.Suit[EquippableSlotFlags.LeftBracelet].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.RightBracelet] == null ? null : node.Suit[EquippableSlotFlags.RightBracelet].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.LeftRing] == null ? null : node.Suit[EquippableSlotFlags.LeftRing].ExtendedMyWorldObject, sb);
			AddEquipmentPieceToClipboard(node.Suit[EquippableSlotFlags.RightRing] == null ? null : node.Suit[EquippableSlotFlags.RightRing].ExtendedMyWorldObject, sb);
			sb.AppendLine();
			sb.AppendLine("Total Effective Legendaries: ".PadRight(30) + node.Suit.TotalEffectiveLegendaries);
			sb.AppendLine("Total Effective Epics: ".PadRight(30) + node.Suit.TotalEffectiveEpics);
			sb.AppendLine("Total Effective Majors: ".PadRight(30) + node.Suit.TotalEffectiveMajors);
			sb.AppendLine("Total Base Armor Level: ".PadRight(30) + node.Suit.TotalBaseArmorLevel);
			sb.AppendLine();

			var spells = new List<Spell>();

			foreach (var kvp in node.Suit)
			{
				foreach (var spell in kvp.Value.ExtendedMyWorldObject.CachedSpells)
					spells.Add(spell);
			}

			spells.Sort((a, b) =>
				            {
								if (a.CantripLevel == Spell.CantripLevels.None && b.CantripLevel != Spell.CantripLevels.None)
									return 1;
								if (a.CantripLevel != Spell.CantripLevels.None && b.CantripLevel == Spell.CantripLevels.None)
									return -1;
								if (a.CantripLevel != Spell.CantripLevels.None && b.CantripLevel != Spell.CantripLevels.None)
								{
									if (a.CantripLevel == b.CantripLevel)
										return String.Compare(b.Name, a.Name, StringComparison.OrdinalIgnoreCase);

									return b.CantripLevel.CompareTo(a.CantripLevel);
								}

								if (a.BuffLevel == b.BuffLevel)
									return String.Compare(b.Name, a.Name, StringComparison.OrdinalIgnoreCase);

					            return b.BuffLevel.CompareTo(a.BuffLevel);
				            });

			foreach (var spell in spells)
				sb.AppendLine(spell.ToString());

			try
			{
				Clipboard.SetText(sb.ToString());
			} catch {};
		}

		void AddEquipmentPieceToClipboard(ExtendedMyWorldObject mwo, StringBuilder sb)
		{
			if (mwo == null)
			{
				sb.AppendLine();
				return;
			}

			var itemInfo = new ItemInfo(mwo);

			sb.AppendLine(mwo.Owner.PadRight(20) + ", " + itemInfo);
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
