using Mag_SuitBuilder.Equipment;
using Mag_SuitBuilder.Spells;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.cmdResizeColumns = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.equipmentGrid = new System.Windows.Forms.DataGridView();
			this.filtersControl1 = new Mag_SuitBuilder.Equipment.FiltersControl();
			this.CharactersTreeView = new System.Windows.Forms.TreeView();
			this.CharactersTreeViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ShowEquipmentUpgradesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.chkFilters = new System.Windows.Forms.CheckBox();
			this.chkTree = new System.Windows.Forms.CheckBox();
			this.txtInventoryRootPath = new System.Windows.Forms.TextBox();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnLoadFromDB = new System.Windows.Forms.Button();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.cmdCollapseAll = new System.Windows.Forms.Button();
			this.cmdExpandAll = new System.Windows.Forms.Button();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.btnStopCalculating = new System.Windows.Forms.Button();
			this.btnCalculatePossibilities = new System.Windows.Forms.Button();
			this.cntrlSuitCantrips = new Mag_SuitBuilder.Spells.CantripSelectorControl();
			this.coveragePiece1 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece16 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece2 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece17 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece3 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece14 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece4 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece15 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece5 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece13 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece6 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece12 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece7 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece11 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece8 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece10 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.coveragePiece9 = new Mag_SuitBuilder.Equipment.EquipmentPieceControl();
			this.tabControl1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.equipmentGrid)).BeginInit();
			this.CharactersTreeViewContextMenu.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1454, 775);
			this.tabControl1.TabIndex = 17;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.cmdResizeColumns);
			this.tabPage3.Controls.Add(this.panel1);
			this.tabPage3.Controls.Add(this.chkFilters);
			this.tabPage3.Controls.Add(this.chkTree);
			this.tabPage3.Controls.Add(this.txtInventoryRootPath);
			this.tabPage3.Controls.Add(this.btnHelp);
			this.tabPage3.Controls.Add(this.btnLoadFromDB);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(1446, 749);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Step 1. Add Inventory";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// cmdResizeColumns
			// 
			this.cmdResizeColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdResizeColumns.Location = new System.Drawing.Point(1260, 7);
			this.cmdResizeColumns.Name = "cmdResizeColumns";
			this.cmdResizeColumns.Size = new System.Drawing.Size(97, 23);
			this.cmdResizeColumns.TabIndex = 38;
			this.cmdResizeColumns.Text = "Resize Columns";
			this.cmdResizeColumns.UseVisualStyleBackColor = true;
			this.cmdResizeColumns.Click += new System.EventHandler(this.cmdResizeColumns_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.equipmentGrid);
			this.panel1.Controls.Add(this.filtersControl1);
			this.panel1.Controls.Add(this.CharactersTreeView);
			this.panel1.Location = new System.Drawing.Point(1, 32);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1442, 715);
			this.panel1.TabIndex = 37;
			// 
			// equipmentGrid
			// 
			this.equipmentGrid.AllowUserToAddRows = false;
			this.equipmentGrid.AllowUserToDeleteRows = false;
			this.equipmentGrid.AllowUserToOrderColumns = true;
			this.equipmentGrid.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
			this.equipmentGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.equipmentGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.equipmentGrid.Location = new System.Drawing.Point(736, 0);
			this.equipmentGrid.Name = "equipmentGrid";
			this.equipmentGrid.Size = new System.Drawing.Size(706, 715);
			this.equipmentGrid.TabIndex = 28;
			this.equipmentGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.equipmentGrid_CellEndEdit);
			this.equipmentGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.equipmentGrid_CellFormatting);
			this.equipmentGrid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.equipmentGrid_CellPainting);
			// 
			// filtersControl1
			// 
			this.filtersControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.filtersControl1.Dock = System.Windows.Forms.DockStyle.Left;
			this.filtersControl1.Location = new System.Drawing.Point(206, 0);
			this.filtersControl1.Name = "filtersControl1";
			this.filtersControl1.Size = new System.Drawing.Size(530, 715);
			this.filtersControl1.TabIndex = 0;
			// 
			// CharactersTreeView
			// 
			this.CharactersTreeView.CheckBoxes = true;
			this.CharactersTreeView.ContextMenuStrip = this.CharactersTreeViewContextMenu;
			this.CharactersTreeView.Dock = System.Windows.Forms.DockStyle.Left;
			this.CharactersTreeView.Location = new System.Drawing.Point(0, 0);
			this.CharactersTreeView.Name = "CharactersTreeView";
			this.CharactersTreeView.Size = new System.Drawing.Size(206, 715);
			this.CharactersTreeView.TabIndex = 33;
			this.CharactersTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.inventoryTreeView_AfterCheck);
			// 
			// CharactersTreeViewContextMenu
			// 
			this.CharactersTreeViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowEquipmentUpgradesMenuItem});
			this.CharactersTreeViewContextMenu.Name = "ShowEquipmentUpgradesMenuItem";
			this.CharactersTreeViewContextMenu.Size = new System.Drawing.Size(218, 26);
			this.CharactersTreeViewContextMenu.Text = "Show Equipment Upgrades";
			this.CharactersTreeViewContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.CharactersTreeViewContextMenu_Opening);
			// 
			// ShowEquipmentUpgradesMenuItem
			// 
			this.ShowEquipmentUpgradesMenuItem.Name = "ShowEquipmentUpgradesMenuItem";
			this.ShowEquipmentUpgradesMenuItem.Size = new System.Drawing.Size(217, 22);
			this.ShowEquipmentUpgradesMenuItem.Text = "Show Equipment Upgrades";
			this.ShowEquipmentUpgradesMenuItem.Click += new System.EventHandler(this.ShowEquipmentUpgradesMenuItem_Click);
			// 
			// chkFilters
			// 
			this.chkFilters.AutoSize = true;
			this.chkFilters.Checked = true;
			this.chkFilters.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFilters.Location = new System.Drawing.Point(62, 9);
			this.chkFilters.Name = "chkFilters";
			this.chkFilters.Size = new System.Drawing.Size(53, 17);
			this.chkFilters.TabIndex = 35;
			this.chkFilters.Text = "Filters";
			this.chkFilters.UseVisualStyleBackColor = true;
			this.chkFilters.CheckedChanged += new System.EventHandler(this.chkTree_CheckedChanged);
			// 
			// chkTree
			// 
			this.chkTree.AutoSize = true;
			this.chkTree.Checked = true;
			this.chkTree.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTree.Location = new System.Drawing.Point(8, 9);
			this.chkTree.Name = "chkTree";
			this.chkTree.Size = new System.Drawing.Size(48, 17);
			this.chkTree.TabIndex = 18;
			this.chkTree.Text = "Tree";
			this.chkTree.UseVisualStyleBackColor = true;
			this.chkTree.CheckedChanged += new System.EventHandler(this.chkTree_CheckedChanged);
			// 
			// txtInventoryRootPath
			// 
			this.txtInventoryRootPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtInventoryRootPath.Location = new System.Drawing.Point(227, 9);
			this.txtInventoryRootPath.Name = "txtInventoryRootPath";
			this.txtInventoryRootPath.Size = new System.Drawing.Size(1027, 20);
			this.txtInventoryRootPath.TabIndex = 34;
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHelp.Location = new System.Drawing.Point(1363, 6);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(75, 23);
			this.btnHelp.TabIndex = 32;
			this.btnHelp.Text = "Help";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// btnLoadFromDB
			// 
			this.btnLoadFromDB.Location = new System.Drawing.Point(121, 6);
			this.btnLoadFromDB.Name = "btnLoadFromDB";
			this.btnLoadFromDB.Size = new System.Drawing.Size(100, 23);
			this.btnLoadFromDB.TabIndex = 29;
			this.btnLoadFromDB.Text = "Reload Inventory";
			this.btnLoadFromDB.UseVisualStyleBackColor = true;
			this.btnLoadFromDB.Click += new System.EventHandler(this.btnLoadFromDB_Click);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.cmdCollapseAll);
			this.tabPage1.Controls.Add(this.cmdExpandAll);
			this.tabPage1.Controls.Add(this.treeView1);
			this.tabPage1.Controls.Add(this.progressBar1);
			this.tabPage1.Controls.Add(this.btnStopCalculating);
			this.tabPage1.Controls.Add(this.btnCalculatePossibilities);
			this.tabPage1.Controls.Add(this.cntrlSuitCantrips);
			this.tabPage1.Controls.Add(this.coveragePiece1);
			this.tabPage1.Controls.Add(this.coveragePiece16);
			this.tabPage1.Controls.Add(this.coveragePiece2);
			this.tabPage1.Controls.Add(this.coveragePiece17);
			this.tabPage1.Controls.Add(this.coveragePiece3);
			this.tabPage1.Controls.Add(this.coveragePiece14);
			this.tabPage1.Controls.Add(this.coveragePiece4);
			this.tabPage1.Controls.Add(this.coveragePiece15);
			this.tabPage1.Controls.Add(this.coveragePiece5);
			this.tabPage1.Controls.Add(this.coveragePiece13);
			this.tabPage1.Controls.Add(this.coveragePiece6);
			this.tabPage1.Controls.Add(this.coveragePiece12);
			this.tabPage1.Controls.Add(this.coveragePiece7);
			this.tabPage1.Controls.Add(this.coveragePiece11);
			this.tabPage1.Controls.Add(this.coveragePiece8);
			this.tabPage1.Controls.Add(this.coveragePiece10);
			this.tabPage1.Controls.Add(this.coveragePiece9);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1446, 749);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Step 2. Generate Suits";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// cmdCollapseAll
			// 
			this.cmdCollapseAll.Location = new System.Drawing.Point(881, 147);
			this.cmdCollapseAll.Name = "cmdCollapseAll";
			this.cmdCollapseAll.Size = new System.Drawing.Size(75, 23);
			this.cmdCollapseAll.TabIndex = 37;
			this.cmdCollapseAll.Text = "Collapse All";
			this.cmdCollapseAll.UseVisualStyleBackColor = true;
			this.cmdCollapseAll.Click += new System.EventHandler(this.cmdCollapseAll_Click);
			// 
			// cmdExpandAll
			// 
			this.cmdExpandAll.Location = new System.Drawing.Point(881, 118);
			this.cmdExpandAll.Name = "cmdExpandAll";
			this.cmdExpandAll.Size = new System.Drawing.Size(75, 23);
			this.cmdExpandAll.TabIndex = 36;
			this.cmdExpandAll.Text = "Expand All";
			this.cmdExpandAll.UseVisualStyleBackColor = true;
			this.cmdExpandAll.Click += new System.EventHandler(this.cmdExpandAll_Click);
			// 
			// treeView1
			// 
			this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeView1.Location = new System.Drawing.Point(962, 8);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(478, 733);
			this.treeView1.TabIndex = 35;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(775, 8);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(181, 23);
			this.progressBar1.TabIndex = 34;
			// 
			// btnStopCalculating
			// 
			this.btnStopCalculating.Enabled = false;
			this.btnStopCalculating.Location = new System.Drawing.Point(630, 8);
			this.btnStopCalculating.Name = "btnStopCalculating";
			this.btnStopCalculating.Size = new System.Drawing.Size(139, 23);
			this.btnStopCalculating.TabIndex = 22;
			this.btnStopCalculating.Text = "Stop Calculating";
			this.btnStopCalculating.UseVisualStyleBackColor = true;
			this.btnStopCalculating.Click += new System.EventHandler(this.btnStopCalculating_Click);
			// 
			// btnCalculatePossibilities
			// 
			this.btnCalculatePossibilities.Location = new System.Drawing.Point(485, 8);
			this.btnCalculatePossibilities.Name = "btnCalculatePossibilities";
			this.btnCalculatePossibilities.Size = new System.Drawing.Size(139, 23);
			this.btnCalculatePossibilities.TabIndex = 18;
			this.btnCalculatePossibilities.Text = "Calculate Possibilities";
			this.btnCalculatePossibilities.UseVisualStyleBackColor = true;
			this.btnCalculatePossibilities.Click += new System.EventHandler(this.btnCalculatePossibilities_Click);
			// 
			// cntrlSuitCantrips
			// 
			this.cntrlSuitCantrips.Enabled = false;
			this.cntrlSuitCantrips.Location = new System.Drawing.Point(8, 564);
			this.cntrlSuitCantrips.Name = "cntrlSuitCantrips";
			this.cntrlSuitCantrips.Size = new System.Drawing.Size(528, 181);
			this.cntrlSuitCantrips.TabIndex = 33;
			// 
			// coveragePiece1
			// 
			this.coveragePiece1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece1.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.Necklace;
			this.coveragePiece1.Location = new System.Drawing.Point(8, 8);
			this.coveragePiece1.Name = "coveragePiece1";
			this.coveragePiece1.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece1.TabIndex = 0;
			// 
			// coveragePiece16
			// 
			this.coveragePiece16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece16.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.PantsUpperLegs;
			this.coveragePiece16.Location = new System.Drawing.Point(803, 425);
			this.coveragePiece16.Name = "coveragePiece16";
			this.coveragePiece16.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece16.TabIndex = 16;
			// 
			// coveragePiece2
			// 
			this.coveragePiece2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece2.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.Trinket;
			this.coveragePiece2.Location = new System.Drawing.Point(8, 147);
			this.coveragePiece2.Name = "coveragePiece2";
			this.coveragePiece2.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece2.TabIndex = 1;
			// 
			// coveragePiece17
			// 
			this.coveragePiece17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece17.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.ShirtChest;
			this.coveragePiece17.Location = new System.Drawing.Point(803, 286);
			this.coveragePiece17.Name = "coveragePiece17";
			this.coveragePiece17.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece17.TabIndex = 15;
			// 
			// coveragePiece3
			// 
			this.coveragePiece3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece3.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.LeftBracelet;
			this.coveragePiece3.Location = new System.Drawing.Point(8, 286);
			this.coveragePiece3.Name = "coveragePiece3";
			this.coveragePiece3.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece3.TabIndex = 2;
			// 
			// coveragePiece14
			// 
			this.coveragePiece14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece14.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.RightRing;
			this.coveragePiece14.Location = new System.Drawing.Point(644, 425);
			this.coveragePiece14.Name = "coveragePiece14";
			this.coveragePiece14.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece14.TabIndex = 14;
			// 
			// coveragePiece4
			// 
			this.coveragePiece4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece4.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.LeftRing;
			this.coveragePiece4.Location = new System.Drawing.Point(8, 425);
			this.coveragePiece4.Name = "coveragePiece4";
			this.coveragePiece4.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece4.TabIndex = 3;
			// 
			// coveragePiece15
			// 
			this.coveragePiece15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece15.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.RightBracelet;
			this.coveragePiece15.Location = new System.Drawing.Point(644, 286);
			this.coveragePiece15.Name = "coveragePiece15";
			this.coveragePiece15.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece15.TabIndex = 13;
			// 
			// coveragePiece5
			// 
			this.coveragePiece5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece5.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.UpperArms;
			this.coveragePiece5.Location = new System.Drawing.Point(167, 147);
			this.coveragePiece5.Name = "coveragePiece5";
			this.coveragePiece5.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece5.TabIndex = 4;
			// 
			// coveragePiece13
			// 
			this.coveragePiece13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece13.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.Feet;
			this.coveragePiece13.Location = new System.Drawing.Point(542, 564);
			this.coveragePiece13.Name = "coveragePiece13";
			this.coveragePiece13.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece13.TabIndex = 12;
			// 
			// coveragePiece6
			// 
			this.coveragePiece6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece6.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.Chest;
			this.coveragePiece6.Location = new System.Drawing.Point(326, 147);
			this.coveragePiece6.Name = "coveragePiece6";
			this.coveragePiece6.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece6.TabIndex = 5;
			// 
			// coveragePiece12
			// 
			this.coveragePiece12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece12.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.LowerLegs;
			this.coveragePiece12.Location = new System.Drawing.Point(485, 425);
			this.coveragePiece12.Name = "coveragePiece12";
			this.coveragePiece12.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece12.TabIndex = 11;
			// 
			// coveragePiece7
			// 
			this.coveragePiece7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece7.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.Head;
			this.coveragePiece7.Location = new System.Drawing.Point(326, 8);
			this.coveragePiece7.Name = "coveragePiece7";
			this.coveragePiece7.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece7.TabIndex = 6;
			// 
			// coveragePiece11
			// 
			this.coveragePiece11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece11.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.Hands;
			this.coveragePiece11.Location = new System.Drawing.Point(167, 425);
			this.coveragePiece11.Name = "coveragePiece11";
			this.coveragePiece11.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece11.TabIndex = 10;
			// 
			// coveragePiece8
			// 
			this.coveragePiece8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece8.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.LowerArms;
			this.coveragePiece8.Location = new System.Drawing.Point(167, 286);
			this.coveragePiece8.Name = "coveragePiece8";
			this.coveragePiece8.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece8.TabIndex = 7;
			// 
			// coveragePiece10
			// 
			this.coveragePiece10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece10.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.UpperLegs;
			this.coveragePiece10.Location = new System.Drawing.Point(485, 286);
			this.coveragePiece10.Name = "coveragePiece10";
			this.coveragePiece10.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece10.TabIndex = 9;
			// 
			// coveragePiece9
			// 
			this.coveragePiece9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece9.EquippableSlots = Mag.Shared.Constants.EquippableSlotFlags.Abdomen;
			this.coveragePiece9.Location = new System.Drawing.Point(326, 286);
			this.coveragePiece9.Name = "coveragePiece9";
			this.coveragePiece9.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece9.TabIndex = 8;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1454, 775);
			this.Controls.Add(this.tabControl1);
			this.DoubleBuffered = true;
			this.MinimumSize = new System.Drawing.Size(1100, 779);
			this.Name = "Form1";
			this.Text = "Mag-Suit Builder";
			this.tabControl1.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.equipmentGrid)).EndInit();
			this.CharactersTreeViewContextMenu.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private EquipmentPieceControl coveragePiece1;
		private EquipmentPieceControl coveragePiece2;
		private EquipmentPieceControl coveragePiece3;
		private EquipmentPieceControl coveragePiece4;
		private EquipmentPieceControl coveragePiece5;
		private EquipmentPieceControl coveragePiece6;
		private EquipmentPieceControl coveragePiece7;
		private EquipmentPieceControl coveragePiece8;
		private EquipmentPieceControl coveragePiece9;
		private EquipmentPieceControl coveragePiece10;
		private EquipmentPieceControl coveragePiece11;
		private EquipmentPieceControl coveragePiece12;
		private EquipmentPieceControl coveragePiece13;
		private EquipmentPieceControl coveragePiece14;
		private EquipmentPieceControl coveragePiece15;
		private EquipmentPieceControl coveragePiece16;
		private EquipmentPieceControl coveragePiece17;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Button btnCalculatePossibilities;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.DataGridView equipmentGrid;
		private System.Windows.Forms.Button btnLoadFromDB;
		private System.Windows.Forms.Button btnStopCalculating;
		private System.Windows.Forms.Button btnHelp;
		private CantripSelectorControl cntrlSuitCantrips;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Button cmdCollapseAll;
		private System.Windows.Forms.Button cmdExpandAll;
		private System.Windows.Forms.TreeView CharactersTreeView;
		private System.Windows.Forms.TextBox txtInventoryRootPath;
		private System.Windows.Forms.CheckBox chkFilters;
		private System.Windows.Forms.CheckBox chkTree;
		private System.Windows.Forms.Panel panel1;
		private FiltersControl filtersControl1;
		private System.Windows.Forms.Button cmdResizeColumns;
		private System.Windows.Forms.ContextMenuStrip CharactersTreeViewContextMenu;
		private System.Windows.Forms.ToolStripMenuItem ShowEquipmentUpgradesMenuItem;
	}
}

