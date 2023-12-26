namespace Mag_SuitBuilder
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			tabControl1 = new System.Windows.Forms.TabControl();
			tabPage1 = new System.Windows.Forms.TabPage();
			panel1 = new System.Windows.Forms.Panel();
			equipmentGrid = new System.Windows.Forms.DataGridView();
			equipmentGridContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
			copyItemsToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			filtersControl1 = new Equipment.FiltersControl();
			CharactersTreeView = new System.Windows.Forms.TreeView();
			CharactersTreeViewContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
			showEquipmentUpgradesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			cmdResizeColumns = new System.Windows.Forms.Button();
			btnHelp = new System.Windows.Forms.Button();
			txtInventoryRootPath = new System.Windows.Forms.TextBox();
			btnLoadFromDB = new System.Windows.Forms.Button();
			chkFilters = new System.Windows.Forms.CheckBox();
			chkTree = new System.Windows.Forms.CheckBox();
			tabPage2 = new System.Windows.Forms.TabPage();
			treeView1 = new System.Windows.Forms.TreeView();
			lblAccessorizerRunningThreads = new System.Windows.Forms.Label();
			lblAccessorizerQueuedThreads = new System.Windows.Forms.Label();
			lblArmorSearchThreads = new System.Windows.Forms.Label();
			progressBar1 = new System.Windows.Forms.ProgressBar();
			cmdCollapseAll = new System.Windows.Forms.Button();
			cmdExpandAll = new System.Windows.Forms.Button();
			cmdCopyToClipboard = new System.Windows.Forms.Button();
			btnStopCalculating = new System.Windows.Forms.Button();
			btnCalculatePossibilities = new System.Windows.Forms.Button();
			cntrlSuitCantrips = new Spells.CantripSelectorControl();
			equipmentPieceControl17 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl16 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl15 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl14 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl13 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl12 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl11 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl10 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl9 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl8 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl7 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl6 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl5 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl4 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl3 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl2 = new Equipment.EquipmentPieceControl();
			equipmentPieceControl1 = new Equipment.EquipmentPieceControl();
			timerCalculatorUpdator = new System.Windows.Forms.Timer(components);
			tabControl1.SuspendLayout();
			tabPage1.SuspendLayout();
			panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)equipmentGrid).BeginInit();
			equipmentGridContextMenu.SuspendLayout();
			CharactersTreeViewContextMenu.SuspendLayout();
			tabPage2.SuspendLayout();
			SuspendLayout();
			// 
			// tabControl1
			// 
			tabControl1.Controls.Add(tabPage1);
			tabControl1.Controls.Add(tabPage2);
			tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			tabControl1.Location = new System.Drawing.Point(0, 0);
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			tabControl1.Size = new System.Drawing.Size(1454, 841);
			tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			tabPage1.Controls.Add(panel1);
			tabPage1.Controls.Add(cmdResizeColumns);
			tabPage1.Controls.Add(btnHelp);
			tabPage1.Controls.Add(txtInventoryRootPath);
			tabPage1.Controls.Add(btnLoadFromDB);
			tabPage1.Controls.Add(chkFilters);
			tabPage1.Controls.Add(chkTree);
			tabPage1.Location = new System.Drawing.Point(4, 24);
			tabPage1.Name = "tabPage1";
			tabPage1.Padding = new System.Windows.Forms.Padding(3);
			tabPage1.Size = new System.Drawing.Size(1446, 813);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "Step 1. Add Inventory";
			tabPage1.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			panel1.Controls.Add(equipmentGrid);
			panel1.Controls.Add(filtersControl1);
			panel1.Controls.Add(CharactersTreeView);
			panel1.Location = new System.Drawing.Point(0, 35);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(1446, 778);
			panel1.TabIndex = 6;
			// 
			// equipmentGrid
			// 
			equipmentGrid.AllowUserToAddRows = false;
			equipmentGrid.AllowUserToDeleteRows = false;
			equipmentGrid.AllowUserToOrderColumns = true;
			equipmentGrid.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(234, 234, 234);
			equipmentGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			equipmentGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			equipmentGrid.ContextMenuStrip = equipmentGridContextMenu;
			equipmentGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			equipmentGrid.Location = new System.Drawing.Point(795, 0);
			equipmentGrid.Name = "equipmentGrid";
			equipmentGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			equipmentGrid.Size = new System.Drawing.Size(651, 778);
			equipmentGrid.TabIndex = 2;
			equipmentGrid.CellPainting += equipmentGrid_CellPainting;
			// 
			// equipmentGridContextMenu
			// 
			equipmentGridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { copyItemsToClipboardToolStripMenuItem });
			equipmentGridContextMenu.Name = "equipmentGridContextMenu";
			equipmentGridContextMenu.Size = new System.Drawing.Size(205, 26);
			// 
			// copyItemsToClipboardToolStripMenuItem
			// 
			copyItemsToClipboardToolStripMenuItem.Name = "copyItemsToClipboardToolStripMenuItem";
			copyItemsToClipboardToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
			copyItemsToClipboardToolStripMenuItem.Text = "Copy Items To Clipboard";
			copyItemsToClipboardToolStripMenuItem.Click += copyItemsToClipboardToolStripMenuItem_Click;
			// 
			// filtersControl1
			// 
			filtersControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			filtersControl1.Dock = System.Windows.Forms.DockStyle.Left;
			filtersControl1.Location = new System.Drawing.Point(206, 0);
			filtersControl1.Name = "filtersControl1";
			filtersControl1.Size = new System.Drawing.Size(589, 778);
			filtersControl1.TabIndex = 1;
			// 
			// CharactersTreeView
			// 
			CharactersTreeView.CheckBoxes = true;
			CharactersTreeView.ContextMenuStrip = CharactersTreeViewContextMenu;
			CharactersTreeView.Dock = System.Windows.Forms.DockStyle.Left;
			CharactersTreeView.Location = new System.Drawing.Point(0, 0);
			CharactersTreeView.Name = "CharactersTreeView";
			CharactersTreeView.Size = new System.Drawing.Size(206, 778);
			CharactersTreeView.TabIndex = 0;
			CharactersTreeView.AfterCheck += CharactersTreeView_AfterCheck;
			// 
			// CharactersTreeViewContextMenu
			// 
			CharactersTreeViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { showEquipmentUpgradesToolStripMenuItem });
			CharactersTreeViewContextMenu.Name = "CharactersTreeViewContextMenu";
			CharactersTreeViewContextMenu.Size = new System.Drawing.Size(218, 26);
			CharactersTreeViewContextMenu.Opening += CharactersTreeViewContextMenu_Opening;
			// 
			// showEquipmentUpgradesToolStripMenuItem
			// 
			showEquipmentUpgradesToolStripMenuItem.Name = "showEquipmentUpgradesToolStripMenuItem";
			showEquipmentUpgradesToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
			showEquipmentUpgradesToolStripMenuItem.Text = "Show Equipment Upgrades";
			showEquipmentUpgradesToolStripMenuItem.Click += showEquipmentUpgradesToolStripMenuItem_Click;
			// 
			// cmdResizeColumns
			// 
			cmdResizeColumns.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			cmdResizeColumns.Location = new System.Drawing.Point(1246, 6);
			cmdResizeColumns.Name = "cmdResizeColumns";
			cmdResizeColumns.Size = new System.Drawing.Size(113, 23);
			cmdResizeColumns.TabIndex = 5;
			cmdResizeColumns.Text = "Resize Columns";
			cmdResizeColumns.UseVisualStyleBackColor = true;
			cmdResizeColumns.Click += cmdResizeColumns_Click;
			// 
			// btnHelp
			// 
			btnHelp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnHelp.Location = new System.Drawing.Point(1365, 6);
			btnHelp.Name = "btnHelp";
			btnHelp.Size = new System.Drawing.Size(75, 23);
			btnHelp.TabIndex = 4;
			btnHelp.Text = "Help";
			btnHelp.UseVisualStyleBackColor = true;
			btnHelp.Click += btnHelp_Click;
			// 
			// txtInventoryRootPath
			// 
			txtInventoryRootPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtInventoryRootPath.Location = new System.Drawing.Point(261, 6);
			txtInventoryRootPath.Name = "txtInventoryRootPath";
			txtInventoryRootPath.Size = new System.Drawing.Size(979, 23);
			txtInventoryRootPath.TabIndex = 3;
			// 
			// btnLoadFromDB
			// 
			btnLoadFromDB.Location = new System.Drawing.Point(131, 7);
			btnLoadFromDB.Name = "btnLoadFromDB";
			btnLoadFromDB.Size = new System.Drawing.Size(124, 23);
			btnLoadFromDB.TabIndex = 2;
			btnLoadFromDB.Text = "Reload Inventory";
			btnLoadFromDB.UseVisualStyleBackColor = true;
			btnLoadFromDB.Click += btnLoadFromDB_Click;
			// 
			// chkFilters
			// 
			chkFilters.AutoSize = true;
			chkFilters.Checked = true;
			chkFilters.CheckState = System.Windows.Forms.CheckState.Checked;
			chkFilters.Location = new System.Drawing.Point(61, 10);
			chkFilters.Name = "chkFilters";
			chkFilters.Size = new System.Drawing.Size(57, 19);
			chkFilters.TabIndex = 1;
			chkFilters.Text = "Filters";
			chkFilters.UseVisualStyleBackColor = true;
			chkFilters.CheckedChanged += chkTree_CheckedChanged;
			// 
			// chkTree
			// 
			chkTree.AutoSize = true;
			chkTree.Checked = true;
			chkTree.CheckState = System.Windows.Forms.CheckState.Checked;
			chkTree.Location = new System.Drawing.Point(8, 9);
			chkTree.Name = "chkTree";
			chkTree.Size = new System.Drawing.Size(47, 19);
			chkTree.TabIndex = 0;
			chkTree.Text = "Tree";
			chkTree.UseVisualStyleBackColor = true;
			chkTree.CheckedChanged += chkTree_CheckedChanged;
			// 
			// tabPage2
			// 
			tabPage2.Controls.Add(treeView1);
			tabPage2.Controls.Add(lblAccessorizerRunningThreads);
			tabPage2.Controls.Add(lblAccessorizerQueuedThreads);
			tabPage2.Controls.Add(lblArmorSearchThreads);
			tabPage2.Controls.Add(progressBar1);
			tabPage2.Controls.Add(cmdCollapseAll);
			tabPage2.Controls.Add(cmdExpandAll);
			tabPage2.Controls.Add(cmdCopyToClipboard);
			tabPage2.Controls.Add(btnStopCalculating);
			tabPage2.Controls.Add(btnCalculatePossibilities);
			tabPage2.Controls.Add(cntrlSuitCantrips);
			tabPage2.Controls.Add(equipmentPieceControl17);
			tabPage2.Controls.Add(equipmentPieceControl16);
			tabPage2.Controls.Add(equipmentPieceControl15);
			tabPage2.Controls.Add(equipmentPieceControl14);
			tabPage2.Controls.Add(equipmentPieceControl13);
			tabPage2.Controls.Add(equipmentPieceControl12);
			tabPage2.Controls.Add(equipmentPieceControl11);
			tabPage2.Controls.Add(equipmentPieceControl10);
			tabPage2.Controls.Add(equipmentPieceControl9);
			tabPage2.Controls.Add(equipmentPieceControl8);
			tabPage2.Controls.Add(equipmentPieceControl7);
			tabPage2.Controls.Add(equipmentPieceControl6);
			tabPage2.Controls.Add(equipmentPieceControl5);
			tabPage2.Controls.Add(equipmentPieceControl4);
			tabPage2.Controls.Add(equipmentPieceControl3);
			tabPage2.Controls.Add(equipmentPieceControl2);
			tabPage2.Controls.Add(equipmentPieceControl1);
			tabPage2.Location = new System.Drawing.Point(4, 24);
			tabPage2.Name = "tabPage2";
			tabPage2.Padding = new System.Windows.Forms.Padding(3);
			tabPage2.Size = new System.Drawing.Size(1446, 813);
			tabPage2.TabIndex = 1;
			tabPage2.Text = "Step 2. Generate Suits";
			tabPage2.UseVisualStyleBackColor = true;
			// 
			// treeView1
			// 
			treeView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			treeView1.Location = new System.Drawing.Point(948, 6);
			treeView1.Name = "treeView1";
			treeView1.Size = new System.Drawing.Size(492, 801);
			treeView1.TabIndex = 27;
			treeView1.AfterSelect += treeView1_AfterSelect;
			// 
			// lblAccessorizerRunningThreads
			// 
			lblAccessorizerRunningThreads.AutoSize = true;
			lblAccessorizerRunningThreads.Location = new System.Drawing.Point(732, 103);
			lblAccessorizerRunningThreads.Name = "lblAccessorizerRunningThreads";
			lblAccessorizerRunningThreads.Size = new System.Drawing.Size(167, 15);
			lblAccessorizerRunningThreads.TabIndex = 26;
			lblAccessorizerRunningThreads.Text = "Accessorizer Running Threads:";
			// 
			// lblAccessorizerQueuedThreads
			// 
			lblAccessorizerQueuedThreads.AutoSize = true;
			lblAccessorizerQueuedThreads.Location = new System.Drawing.Point(732, 79);
			lblAccessorizerQueuedThreads.Name = "lblAccessorizerQueuedThreads";
			lblAccessorizerQueuedThreads.Size = new System.Drawing.Size(164, 15);
			lblAccessorizerQueuedThreads.TabIndex = 25;
			lblAccessorizerQueuedThreads.Text = "Accessorizer Queued Threads:";
			// 
			// lblArmorSearchThreads
			// 
			lblArmorSearchThreads.AutoSize = true;
			lblArmorSearchThreads.Location = new System.Drawing.Point(732, 55);
			lblArmorSearchThreads.Name = "lblArmorSearchThreads";
			lblArmorSearchThreads.Size = new System.Drawing.Size(126, 15);
			lblArmorSearchThreads.TabIndex = 24;
			lblArmorSearchThreads.Text = "Armor Search Threads:";
			// 
			// progressBar1
			// 
			progressBar1.Location = new System.Drawing.Point(732, 15);
			progressBar1.Name = "progressBar1";
			progressBar1.Size = new System.Drawing.Size(210, 23);
			progressBar1.TabIndex = 23;
			// 
			// cmdCollapseAll
			// 
			cmdCollapseAll.Location = new System.Drawing.Point(854, 211);
			cmdCollapseAll.Name = "cmdCollapseAll";
			cmdCollapseAll.Size = new System.Drawing.Size(79, 23);
			cmdCollapseAll.TabIndex = 22;
			cmdCollapseAll.Text = "Collapse All";
			cmdCollapseAll.UseVisualStyleBackColor = true;
			cmdCollapseAll.Click += cmdCollapseAll_Click;
			// 
			// cmdExpandAll
			// 
			cmdExpandAll.Location = new System.Drawing.Point(854, 182);
			cmdExpandAll.Name = "cmdExpandAll";
			cmdExpandAll.Size = new System.Drawing.Size(79, 23);
			cmdExpandAll.TabIndex = 21;
			cmdExpandAll.Text = "Expand All";
			cmdExpandAll.UseVisualStyleBackColor = true;
			cmdExpandAll.Click += cmdExpandAll_Click;
			// 
			// cmdCopyToClipboard
			// 
			cmdCopyToClipboard.Location = new System.Drawing.Point(486, 248);
			cmdCopyToClipboard.Name = "cmdCopyToClipboard";
			cmdCopyToClipboard.Size = new System.Drawing.Size(114, 23);
			cmdCopyToClipboard.TabIndex = 20;
			cmdCopyToClipboard.Text = "Copy to Clipboard";
			cmdCopyToClipboard.UseVisualStyleBackColor = true;
			cmdCopyToClipboard.Click += cmdCopyToClipboard_Click;
			// 
			// btnStopCalculating
			// 
			btnStopCalculating.Enabled = false;
			btnStopCalculating.Location = new System.Drawing.Point(622, 15);
			btnStopCalculating.Name = "btnStopCalculating";
			btnStopCalculating.Size = new System.Drawing.Size(104, 23);
			btnStopCalculating.TabIndex = 19;
			btnStopCalculating.Text = "Stop Calculating";
			btnStopCalculating.UseVisualStyleBackColor = true;
			btnStopCalculating.Click += btnStopCalculating_Click;
			// 
			// btnCalculatePossibilities
			// 
			btnCalculatePossibilities.Location = new System.Drawing.Point(486, 15);
			btnCalculatePossibilities.Name = "btnCalculatePossibilities";
			btnCalculatePossibilities.Size = new System.Drawing.Size(130, 23);
			btnCalculatePossibilities.TabIndex = 18;
			btnCalculatePossibilities.Text = "Calculate Possibilities";
			btnCalculatePossibilities.UseVisualStyleBackColor = true;
			btnCalculatePossibilities.Click += btnCalculatePossibilities_Click;
			// 
			// cntrlSuitCantrips
			// 
			cntrlSuitCantrips.Enabled = false;
			cntrlSuitCantrips.Location = new System.Drawing.Point(6, 610);
			cntrlSuitCantrips.Name = "cntrlSuitCantrips";
			cntrlSuitCantrips.Size = new System.Drawing.Size(585, 197);
			cntrlSuitCantrips.TabIndex = 17;
			// 
			// equipmentPieceControl17
			// 
			equipmentPieceControl17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl17.EquippableSlots = Mag.Shared.Constants.EquipMask.FootWear;
			equipmentPieceControl17.Location = new System.Drawing.Point(597, 610);
			equipmentPieceControl17.Name = "equipmentPieceControl17";
			equipmentPieceControl17.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl17.TabIndex = 16;
			// 
			// equipmentPieceControl16
			// 
			equipmentPieceControl16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl16.EquippableSlots = Mag.Shared.Constants.EquipMask.UpperLegWear;
			equipmentPieceControl16.Location = new System.Drawing.Point(791, 459);
			equipmentPieceControl16.Name = "equipmentPieceControl16";
			equipmentPieceControl16.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl16.TabIndex = 15;
			// 
			// equipmentPieceControl15
			// 
			equipmentPieceControl15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl15.EquippableSlots = Mag.Shared.Constants.EquipMask.FingerWearRight;
			equipmentPieceControl15.Location = new System.Drawing.Point(634, 459);
			equipmentPieceControl15.Name = "equipmentPieceControl15";
			equipmentPieceControl15.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl15.TabIndex = 14;
			// 
			// equipmentPieceControl14
			// 
			equipmentPieceControl14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl14.EquippableSlots = Mag.Shared.Constants.EquipMask.LowerLegArmor;
			equipmentPieceControl14.Location = new System.Drawing.Point(477, 459);
			equipmentPieceControl14.Name = "equipmentPieceControl14";
			equipmentPieceControl14.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl14.TabIndex = 13;
			// 
			// equipmentPieceControl13
			// 
			equipmentPieceControl13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl13.EquippableSlots = Mag.Shared.Constants.EquipMask.ChestWear;
			equipmentPieceControl13.Location = new System.Drawing.Point(791, 308);
			equipmentPieceControl13.Name = "equipmentPieceControl13";
			equipmentPieceControl13.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl13.TabIndex = 12;
			// 
			// equipmentPieceControl12
			// 
			equipmentPieceControl12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl12.EquippableSlots = Mag.Shared.Constants.EquipMask.WristWearRight;
			equipmentPieceControl12.Location = new System.Drawing.Point(634, 308);
			equipmentPieceControl12.Name = "equipmentPieceControl12";
			equipmentPieceControl12.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl12.TabIndex = 11;
			// 
			// equipmentPieceControl11
			// 
			equipmentPieceControl11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl11.EquippableSlots = Mag.Shared.Constants.EquipMask.UpperLegArmor;
			equipmentPieceControl11.Location = new System.Drawing.Point(477, 308);
			equipmentPieceControl11.Name = "equipmentPieceControl11";
			equipmentPieceControl11.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl11.TabIndex = 10;
			// 
			// equipmentPieceControl10
			// 
			equipmentPieceControl10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl10.EquippableSlots = Mag.Shared.Constants.EquipMask.HeadWear;
			equipmentPieceControl10.Location = new System.Drawing.Point(320, 6);
			equipmentPieceControl10.Name = "equipmentPieceControl10";
			equipmentPieceControl10.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl10.TabIndex = 9;
			// 
			// equipmentPieceControl9
			// 
			equipmentPieceControl9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl9.EquippableSlots = Mag.Shared.Constants.EquipMask.AbdomenArmor;
			equipmentPieceControl9.Location = new System.Drawing.Point(320, 308);
			equipmentPieceControl9.Name = "equipmentPieceControl9";
			equipmentPieceControl9.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl9.TabIndex = 8;
			// 
			// equipmentPieceControl8
			// 
			equipmentPieceControl8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl8.EquippableSlots = Mag.Shared.Constants.EquipMask.ChestArmor;
			equipmentPieceControl8.Location = new System.Drawing.Point(320, 157);
			equipmentPieceControl8.Name = "equipmentPieceControl8";
			equipmentPieceControl8.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl8.TabIndex = 7;
			// 
			// equipmentPieceControl7
			// 
			equipmentPieceControl7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl7.EquippableSlots = Mag.Shared.Constants.EquipMask.HandWear;
			equipmentPieceControl7.Location = new System.Drawing.Point(163, 459);
			equipmentPieceControl7.Name = "equipmentPieceControl7";
			equipmentPieceControl7.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl7.TabIndex = 6;
			// 
			// equipmentPieceControl6
			// 
			equipmentPieceControl6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl6.EquippableSlots = Mag.Shared.Constants.EquipMask.LowerArmArmor;
			equipmentPieceControl6.Location = new System.Drawing.Point(163, 308);
			equipmentPieceControl6.Name = "equipmentPieceControl6";
			equipmentPieceControl6.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl6.TabIndex = 5;
			// 
			// equipmentPieceControl5
			// 
			equipmentPieceControl5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl5.EquippableSlots = Mag.Shared.Constants.EquipMask.UpperArmArmor;
			equipmentPieceControl5.Location = new System.Drawing.Point(163, 157);
			equipmentPieceControl5.Name = "equipmentPieceControl5";
			equipmentPieceControl5.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl5.TabIndex = 4;
			// 
			// equipmentPieceControl4
			// 
			equipmentPieceControl4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl4.EquippableSlots = Mag.Shared.Constants.EquipMask.FingerWearLeft;
			equipmentPieceControl4.Location = new System.Drawing.Point(6, 459);
			equipmentPieceControl4.Name = "equipmentPieceControl4";
			equipmentPieceControl4.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl4.TabIndex = 3;
			// 
			// equipmentPieceControl3
			// 
			equipmentPieceControl3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl3.EquippableSlots = Mag.Shared.Constants.EquipMask.WristWearLeft;
			equipmentPieceControl3.Location = new System.Drawing.Point(6, 308);
			equipmentPieceControl3.Name = "equipmentPieceControl3";
			equipmentPieceControl3.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl3.TabIndex = 2;
			// 
			// equipmentPieceControl2
			// 
			equipmentPieceControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl2.EquippableSlots = Mag.Shared.Constants.EquipMask.TrinketOne;
			equipmentPieceControl2.Location = new System.Drawing.Point(6, 157);
			equipmentPieceControl2.Name = "equipmentPieceControl2";
			equipmentPieceControl2.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl2.TabIndex = 1;
			// 
			// equipmentPieceControl1
			// 
			equipmentPieceControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equipmentPieceControl1.EquippableSlots = Mag.Shared.Constants.EquipMask.NeckWear;
			equipmentPieceControl1.Location = new System.Drawing.Point(6, 6);
			equipmentPieceControl1.Name = "equipmentPieceControl1";
			equipmentPieceControl1.Size = new System.Drawing.Size(151, 145);
			equipmentPieceControl1.TabIndex = 0;
			// 
			// timerCalculatorUpdator
			// 
			timerCalculatorUpdator.Interval = 500;
			timerCalculatorUpdator.Tick += timerCalculatorUpdator_Tick;
			// 
			// Form1
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(1454, 841);
			Controls.Add(tabControl1);
			MinimumSize = new System.Drawing.Size(1200, 880);
			Name = "Form1";
			Text = "Mag-Suit Builder";
			tabControl1.ResumeLayout(false);
			tabPage1.ResumeLayout(false);
			tabPage1.PerformLayout();
			panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)equipmentGrid).EndInit();
			equipmentGridContextMenu.ResumeLayout(false);
			CharactersTreeViewContextMenu.ResumeLayout(false);
			tabPage2.ResumeLayout(false);
			tabPage2.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Panel panel1;
		private Equipment.FiltersControl filtersControl1;
		private System.Windows.Forms.DataGridView equipmentGrid;
		private System.Windows.Forms.TreeView CharactersTreeView;
		private System.Windows.Forms.Button cmdResizeColumns;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.TextBox txtInventoryRootPath;
		private System.Windows.Forms.Button btnLoadFromDB;
		private System.Windows.Forms.CheckBox chkFilters;
		private System.Windows.Forms.CheckBox chkTree;
		private Spells.CantripSelectorControl cntrlSuitCantrips;
		private Equipment.EquipmentPieceControl equipmentPieceControl17;
		private Equipment.EquipmentPieceControl equipmentPieceControl16;
		private Equipment.EquipmentPieceControl equipmentPieceControl15;
		private Equipment.EquipmentPieceControl equipmentPieceControl14;
		private Equipment.EquipmentPieceControl equipmentPieceControl13;
		private Equipment.EquipmentPieceControl equipmentPieceControl12;
		private Equipment.EquipmentPieceControl equipmentPieceControl11;
		private Equipment.EquipmentPieceControl equipmentPieceControl10;
		private Equipment.EquipmentPieceControl equipmentPieceControl9;
		private Equipment.EquipmentPieceControl equipmentPieceControl8;
		private Equipment.EquipmentPieceControl equipmentPieceControl7;
		private Equipment.EquipmentPieceControl equipmentPieceControl6;
		private Equipment.EquipmentPieceControl equipmentPieceControl5;
		private Equipment.EquipmentPieceControl equipmentPieceControl4;
		private Equipment.EquipmentPieceControl equipmentPieceControl3;
		private Equipment.EquipmentPieceControl equipmentPieceControl2;
		private Equipment.EquipmentPieceControl equipmentPieceControl1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Label lblAccessorizerRunningThreads;
		private System.Windows.Forms.Label lblAccessorizerQueuedThreads;
		private System.Windows.Forms.Label lblArmorSearchThreads;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button cmdCollapseAll;
		private System.Windows.Forms.Button cmdExpandAll;
		private System.Windows.Forms.Button cmdCopyToClipboard;
		private System.Windows.Forms.Button btnStopCalculating;
		private System.Windows.Forms.Button btnCalculatePossibilities;
		private System.Windows.Forms.ContextMenuStrip equipmentGridContextMenu;
		private System.Windows.Forms.ContextMenuStrip CharactersTreeViewContextMenu;
		private System.Windows.Forms.Timer timerCalculatorUpdator;
		private System.Windows.Forms.ToolStripMenuItem showEquipmentUpgradesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyItemsToClipboardToolStripMenuItem;
	}
}
