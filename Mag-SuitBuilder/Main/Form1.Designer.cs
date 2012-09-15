using Mag_SuitBuilder.Equipment;
using Mag_SuitBuilder.Spells;

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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnLoadFromClipboard = new System.Windows.Forms.Button();
			this.btnLoadFromDB = new System.Windows.Forms.Button();
			this.equipmentGrid = new System.Windows.Forms.DataGridView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.button9 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.button8 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.cboSecondaryArmorSet = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cboPrimaryArmorSet = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtMinimumBaseArmorLevel = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cntrlCantripFilters = new Mag_SuitBuilder.Spells.CantripSelectorControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.cntrlSuitCantrips = new Mag_SuitBuilder.Spells.CantripSelectorControl();
			this.btnStopCalculating = new System.Windows.Forms.Button();
			this.btnCalculatePossibilities = new System.Windows.Forms.Button();
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
			this.button2 = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.equipmentGrid)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(972, 741);
			this.tabControl1.TabIndex = 17;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.btnHelp);
			this.tabPage3.Controls.Add(this.btnClear);
			this.tabPage3.Controls.Add(this.btnLoadFromClipboard);
			this.tabPage3.Controls.Add(this.btnLoadFromDB);
			this.tabPage3.Controls.Add(this.equipmentGrid);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(964, 715);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Step 1. Add Inventory";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// btnHelp
			// 
			this.btnHelp.Location = new System.Drawing.Point(881, 6);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(75, 23);
			this.btnHelp.TabIndex = 32;
			this.btnHelp.Text = "Help";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(773, 6);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(75, 23);
			this.btnClear.TabIndex = 31;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnLoadFromClipboard
			// 
			this.btnLoadFromClipboard.Location = new System.Drawing.Point(379, 6);
			this.btnLoadFromClipboard.Name = "btnLoadFromClipboard";
			this.btnLoadFromClipboard.Size = new System.Drawing.Size(365, 23);
			this.btnLoadFromClipboard.TabIndex = 30;
			this.btnLoadFromClipboard.Text = "Method 2. Load Inventory Info from Mag-Tools Clipboard Output";
			this.btnLoadFromClipboard.UseVisualStyleBackColor = true;
			this.btnLoadFromClipboard.Click += new System.EventHandler(this.btnLoadFromClipboard_Click);
			// 
			// btnLoadFromDB
			// 
			this.btnLoadFromDB.Location = new System.Drawing.Point(8, 6);
			this.btnLoadFromDB.Name = "btnLoadFromDB";
			this.btnLoadFromDB.Size = new System.Drawing.Size(365, 23);
			this.btnLoadFromDB.TabIndex = 29;
			this.btnLoadFromDB.Text = "Method 1. Load Inventory Database from Mag-Tools Export on Disk";
			this.btnLoadFromDB.UseVisualStyleBackColor = true;
			this.btnLoadFromDB.Click += new System.EventHandler(this.btnLoadFromDB_Click);
			// 
			// equipmentGrid
			// 
			this.equipmentGrid.AllowUserToOrderColumns = true;
			this.equipmentGrid.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
			this.equipmentGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.equipmentGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.equipmentGrid.Location = new System.Drawing.Point(3, 35);
			this.equipmentGrid.Name = "equipmentGrid";
			this.equipmentGrid.Size = new System.Drawing.Size(955, 677);
			this.equipmentGrid.TabIndex = 28;
			this.equipmentGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.equipmentGrid_CellEndEdit);
			this.equipmentGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.equipmentGrid_CellFormatting);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.button2);
			this.tabPage2.Controls.Add(this.button9);
			this.tabPage2.Controls.Add(this.button7);
			this.tabPage2.Controls.Add(this.button8);
			this.tabPage2.Controls.Add(this.button6);
			this.tabPage2.Controls.Add(this.button5);
			this.tabPage2.Controls.Add(this.button4);
			this.tabPage2.Controls.Add(this.button3);
			this.tabPage2.Controls.Add(this.button1);
			this.tabPage2.Controls.Add(this.label6);
			this.tabPage2.Controls.Add(this.cboSecondaryArmorSet);
			this.tabPage2.Controls.Add(this.label2);
			this.tabPage2.Controls.Add(this.cboPrimaryArmorSet);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Controls.Add(this.txtMinimumBaseArmorLevel);
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Controls.Add(this.cntrlCantripFilters);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(964, 715);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Step 2. Setup Filters";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// button9
			// 
			this.button9.Location = new System.Drawing.Point(721, 41);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(97, 23);
			this.button9.TabIndex = 52;
			this.button9.Text = "Tinker";
			this.button9.UseVisualStyleBackColor = true;
			this.button9.Click += new System.EventHandler(this.loadDefaultSpells_Click);
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(309, 70);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(97, 23);
			this.button7.TabIndex = 51;
			this.button7.Text = "Void";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new System.EventHandler(this.loadDefaultSpells_Click);
			// 
			// button8
			// 
			this.button8.Location = new System.Drawing.Point(309, 41);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(97, 23);
			this.button8.TabIndex = 50;
			this.button8.Text = "War";
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += new System.EventHandler(this.loadDefaultSpells_Click);
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(515, 41);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(97, 23);
			this.button6.TabIndex = 49;
			this.button6.Text = "Two Hand";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.loadDefaultSpells_Click);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(412, 128);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(97, 23);
			this.button5.TabIndex = 48;
			this.button5.Text = "Finesse";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.loadDefaultSpells_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(412, 99);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(97, 23);
			this.button4.TabIndex = 47;
			this.button4.Text = "Light";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.loadDefaultSpells_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(412, 70);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(97, 23);
			this.button3.TabIndex = 46;
			this.button3.Text = "Heavy";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.loadDefaultSpells_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(412, 41);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(97, 23);
			this.button1.TabIndex = 44;
			this.button1.Text = "Missile";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.loadDefaultSpells_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(306, 12);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(554, 26);
			this.label6.TabIndex = 43;
			this.label6.Text = resources.GetString("label6.Text");
			// 
			// cboSecondaryArmorSet
			// 
			this.cboSecondaryArmorSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSecondaryArmorSet.Location = new System.Drawing.Point(121, 70);
			this.cboSecondaryArmorSet.Name = "cboSecondaryArmorSet";
			this.cboSecondaryArmorSet.Size = new System.Drawing.Size(144, 21);
			this.cboSecondaryArmorSet.TabIndex = 38;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 73);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(107, 13);
			this.label2.TabIndex = 37;
			this.label2.Text = "Secondary Armor Set";
			// 
			// cboPrimaryArmorSet
			// 
			this.cboPrimaryArmorSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPrimaryArmorSet.Location = new System.Drawing.Point(121, 38);
			this.cboPrimaryArmorSet.Name = "cboPrimaryArmorSet";
			this.cboPrimaryArmorSet.Size = new System.Drawing.Size(144, 21);
			this.cboPrimaryArmorSet.TabIndex = 36;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 41);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 13);
			this.label3.TabIndex = 35;
			this.label3.Text = "Primary Armor Set";
			// 
			// txtMinimumBaseArmorLevel
			// 
			this.txtMinimumBaseArmorLevel.Location = new System.Drawing.Point(215, 12);
			this.txtMinimumBaseArmorLevel.MaxLength = 3;
			this.txtMinimumBaseArmorLevel.Name = "txtMinimumBaseArmorLevel";
			this.txtMinimumBaseArmorLevel.Size = new System.Drawing.Size(33, 20);
			this.txtMinimumBaseArmorLevel.TabIndex = 34;
			this.txtMinimumBaseArmorLevel.Text = "200";
			this.txtMinimumBaseArmorLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(201, 13);
			this.label1.TabIndex = 33;
			this.label1.Text = "Minimum base armor level for body armor:";
			// 
			// cntrlCantripFilters
			// 
			this.cntrlCantripFilters.Location = new System.Drawing.Point(309, 157);
			this.cntrlCantripFilters.Name = "cntrlCantripFilters";
			this.cntrlCantripFilters.Size = new System.Drawing.Size(528, 150);
			this.cntrlCantripFilters.TabIndex = 32;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.treeView1);
			this.tabPage1.Controls.Add(this.progressBar1);
			this.tabPage1.Controls.Add(this.cntrlSuitCantrips);
			this.tabPage1.Controls.Add(this.btnStopCalculating);
			this.tabPage1.Controls.Add(this.btnCalculatePossibilities);
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
			this.tabPage1.Size = new System.Drawing.Size(964, 715);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Step 3. Generate Suits";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// treeView1
			// 
			this.treeView1.Location = new System.Drawing.Point(485, 37);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(471, 243);
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
			// cntrlSuitCantrips
			// 
			this.cntrlSuitCantrips.Enabled = false;
			this.cntrlSuitCantrips.Location = new System.Drawing.Point(8, 564);
			this.cntrlSuitCantrips.Name = "cntrlSuitCantrips";
			this.cntrlSuitCantrips.Size = new System.Drawing.Size(528, 150);
			this.cntrlSuitCantrips.TabIndex = 33;
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
			// coveragePiece1
			// 
			this.coveragePiece1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece1.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.Necklace;
			this.coveragePiece1.Location = new System.Drawing.Point(8, 8);
			this.coveragePiece1.Name = "coveragePiece1";
			this.coveragePiece1.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece1.TabIndex = 0;
			// 
			// coveragePiece16
			// 
			this.coveragePiece16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece16.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.Pants;
			this.coveragePiece16.Location = new System.Drawing.Point(803, 425);
			this.coveragePiece16.Name = "coveragePiece16";
			this.coveragePiece16.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece16.TabIndex = 16;
			// 
			// coveragePiece2
			// 
			this.coveragePiece2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece2.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.Trinket;
			this.coveragePiece2.Location = new System.Drawing.Point(8, 147);
			this.coveragePiece2.Name = "coveragePiece2";
			this.coveragePiece2.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece2.TabIndex = 1;
			// 
			// coveragePiece17
			// 
			this.coveragePiece17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece17.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.Shirt;
			this.coveragePiece17.Location = new System.Drawing.Point(803, 286);
			this.coveragePiece17.Name = "coveragePiece17";
			this.coveragePiece17.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece17.TabIndex = 15;
			// 
			// coveragePiece3
			// 
			this.coveragePiece3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece3.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.LeftBracelet;
			this.coveragePiece3.Location = new System.Drawing.Point(8, 286);
			this.coveragePiece3.Name = "coveragePiece3";
			this.coveragePiece3.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece3.TabIndex = 2;
			// 
			// coveragePiece14
			// 
			this.coveragePiece14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece14.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.RightRing;
			this.coveragePiece14.Location = new System.Drawing.Point(644, 425);
			this.coveragePiece14.Name = "coveragePiece14";
			this.coveragePiece14.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece14.TabIndex = 14;
			// 
			// coveragePiece4
			// 
			this.coveragePiece4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece4.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.LeftRing;
			this.coveragePiece4.Location = new System.Drawing.Point(8, 425);
			this.coveragePiece4.Name = "coveragePiece4";
			this.coveragePiece4.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece4.TabIndex = 3;
			// 
			// coveragePiece15
			// 
			this.coveragePiece15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece15.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.RightBracelet;
			this.coveragePiece15.Location = new System.Drawing.Point(644, 286);
			this.coveragePiece15.Name = "coveragePiece15";
			this.coveragePiece15.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece15.TabIndex = 13;
			// 
			// coveragePiece5
			// 
			this.coveragePiece5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece5.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.UpperArms;
			this.coveragePiece5.Location = new System.Drawing.Point(167, 147);
			this.coveragePiece5.Name = "coveragePiece5";
			this.coveragePiece5.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece5.TabIndex = 4;
			// 
			// coveragePiece13
			// 
			this.coveragePiece13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece13.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.Feet;
			this.coveragePiece13.Location = new System.Drawing.Point(542, 564);
			this.coveragePiece13.Name = "coveragePiece13";
			this.coveragePiece13.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece13.TabIndex = 12;
			// 
			// coveragePiece6
			// 
			this.coveragePiece6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece6.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.Chest;
			this.coveragePiece6.Location = new System.Drawing.Point(326, 147);
			this.coveragePiece6.Name = "coveragePiece6";
			this.coveragePiece6.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece6.TabIndex = 5;
			// 
			// coveragePiece12
			// 
			this.coveragePiece12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece12.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.LowerLegs;
			this.coveragePiece12.Location = new System.Drawing.Point(485, 425);
			this.coveragePiece12.Name = "coveragePiece12";
			this.coveragePiece12.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece12.TabIndex = 11;
			// 
			// coveragePiece7
			// 
			this.coveragePiece7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece7.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.Head;
			this.coveragePiece7.Location = new System.Drawing.Point(326, 8);
			this.coveragePiece7.Name = "coveragePiece7";
			this.coveragePiece7.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece7.TabIndex = 6;
			// 
			// coveragePiece11
			// 
			this.coveragePiece11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece11.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.Hands;
			this.coveragePiece11.Location = new System.Drawing.Point(167, 425);
			this.coveragePiece11.Name = "coveragePiece11";
			this.coveragePiece11.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece11.TabIndex = 10;
			// 
			// coveragePiece8
			// 
			this.coveragePiece8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece8.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.LowerArms;
			this.coveragePiece8.Location = new System.Drawing.Point(167, 286);
			this.coveragePiece8.Name = "coveragePiece8";
			this.coveragePiece8.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece8.TabIndex = 7;
			// 
			// coveragePiece10
			// 
			this.coveragePiece10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece10.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.UpperLegs;
			this.coveragePiece10.Location = new System.Drawing.Point(485, 286);
			this.coveragePiece10.Name = "coveragePiece10";
			this.coveragePiece10.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece10.TabIndex = 9;
			// 
			// coveragePiece9
			// 
			this.coveragePiece9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece9.EquipableSlots = Mag_SuitBuilder.Constants.EquippableSlotFlags.Abdomen;
			this.coveragePiece9.Location = new System.Drawing.Point(326, 286);
			this.coveragePiece9.Name = "coveragePiece9";
			this.coveragePiece9.Size = new System.Drawing.Size(153, 133);
			this.coveragePiece9.TabIndex = 8;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(721, 128);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(97, 23);
			this.button2.TabIndex = 53;
			this.button2.Text = "Clear";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.loadDefaultSpells_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(972, 741);
			this.Controls.Add(this.tabControl1);
			this.DoubleBuffered = true;
			this.MinimumSize = new System.Drawing.Size(988, 779);
			this.Name = "Form1";
			this.Text = "Mag-Suit Builder";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.tabControl1.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.equipmentGrid)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
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
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Button btnCalculatePossibilities;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.DataGridView equipmentGrid;
		private System.Windows.Forms.Button btnLoadFromClipboard;
		private System.Windows.Forms.Button btnLoadFromDB;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Button btnStopCalculating;
		private CantripSelectorControl cntrlCantripFilters;
		private System.Windows.Forms.TextBox txtMinimumBaseArmorLevel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboSecondaryArmorSet;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cboPrimaryArmorSet;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label6;
		private CantripSelectorControl cntrlSuitCantrips;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Button button2;
	}
}

