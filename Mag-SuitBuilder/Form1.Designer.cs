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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.calculatePossibilities = new System.Windows.Forms.Button();
			this.listPossibilities = new System.Windows.Forms.ListView();
			this.coveragePiece1 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece16 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece2 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece17 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece3 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece14 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece4 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece15 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece5 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece13 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece6 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece12 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece7 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece11 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece8 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece10 = new Mag_SuitBuilder.CoveragePiece();
			this.coveragePiece9 = new Mag_SuitBuilder.CoveragePiece();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.txtEquipmentEntries = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1050, 750);
			this.tabControl1.TabIndex = 17;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.calculatePossibilities);
			this.tabPage1.Controls.Add(this.listPossibilities);
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
			this.tabPage1.Size = new System.Drawing.Size(1042, 724);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Slots";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// calculatePossibilities
			// 
			this.calculatePossibilities.Location = new System.Drawing.Point(560, 16);
			this.calculatePossibilities.Name = "calculatePossibilities";
			this.calculatePossibilities.Size = new System.Drawing.Size(168, 23);
			this.calculatePossibilities.TabIndex = 18;
			this.calculatePossibilities.Text = "Calculate Possibilities";
			this.calculatePossibilities.UseVisualStyleBackColor = true;
			this.calculatePossibilities.Click += new System.EventHandler(this.calculatePossibilities_Click);
			// 
			// listPossibilities
			// 
			this.listPossibilities.Location = new System.Drawing.Point(560, 48);
			this.listPossibilities.Name = "listPossibilities";
			this.listPossibilities.Size = new System.Drawing.Size(216, 240);
			this.listPossibilities.TabIndex = 17;
			this.listPossibilities.UseCompatibleStateImageBehavior = false;
			// 
			// coveragePiece1
			// 
			this.coveragePiece1.ArmorLevel = 0;
			this.coveragePiece1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece1.CanHaveArmorLevel = false;
			this.coveragePiece1.CanHaveArmorSet = false;
			this.coveragePiece1.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.Necklace;
			this.coveragePiece1.Location = new System.Drawing.Point(8, 8);
			this.coveragePiece1.LockedSlot = false;
			this.coveragePiece1.Name = "coveragePiece1";
			this.coveragePiece1.Size = new System.Drawing.Size(150, 136);
			this.coveragePiece1.TabIndex = 0;
			this.coveragePiece1.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece16
			// 
			this.coveragePiece16.ArmorLevel = 0;
			this.coveragePiece16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece16.CanHaveArmorLevel = true;
			this.coveragePiece16.CanHaveArmorSet = false;
			this.coveragePiece16.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.Pants;
			this.coveragePiece16.Location = new System.Drawing.Point(880, 440);
			this.coveragePiece16.LockedSlot = false;
			this.coveragePiece16.Name = "coveragePiece16";
			this.coveragePiece16.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece16.TabIndex = 16;
			this.coveragePiece16.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece2
			// 
			this.coveragePiece2.ArmorLevel = 0;
			this.coveragePiece2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece2.CanHaveArmorLevel = false;
			this.coveragePiece2.CanHaveArmorSet = false;
			this.coveragePiece2.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.Trinket;
			this.coveragePiece2.Location = new System.Drawing.Point(8, 152);
			this.coveragePiece2.LockedSlot = false;
			this.coveragePiece2.Name = "coveragePiece2";
			this.coveragePiece2.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece2.TabIndex = 1;
			this.coveragePiece2.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece17
			// 
			this.coveragePiece17.ArmorLevel = 0;
			this.coveragePiece17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece17.CanHaveArmorLevel = true;
			this.coveragePiece17.CanHaveArmorSet = false;
			this.coveragePiece17.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.Shirt;
			this.coveragePiece17.Location = new System.Drawing.Point(880, 296);
			this.coveragePiece17.LockedSlot = false;
			this.coveragePiece17.Name = "coveragePiece17";
			this.coveragePiece17.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece17.TabIndex = 15;
			this.coveragePiece17.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece3
			// 
			this.coveragePiece3.ArmorLevel = 0;
			this.coveragePiece3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece3.CanHaveArmorLevel = false;
			this.coveragePiece3.CanHaveArmorSet = false;
			this.coveragePiece3.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.LeftBracelet;
			this.coveragePiece3.Location = new System.Drawing.Point(8, 296);
			this.coveragePiece3.LockedSlot = false;
			this.coveragePiece3.Name = "coveragePiece3";
			this.coveragePiece3.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece3.TabIndex = 2;
			this.coveragePiece3.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece14
			// 
			this.coveragePiece14.ArmorLevel = 0;
			this.coveragePiece14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece14.CanHaveArmorLevel = false;
			this.coveragePiece14.CanHaveArmorSet = false;
			this.coveragePiece14.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.RightRing;
			this.coveragePiece14.Location = new System.Drawing.Point(712, 440);
			this.coveragePiece14.LockedSlot = false;
			this.coveragePiece14.Name = "coveragePiece14";
			this.coveragePiece14.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece14.TabIndex = 14;
			this.coveragePiece14.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece4
			// 
			this.coveragePiece4.ArmorLevel = 0;
			this.coveragePiece4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece4.CanHaveArmorLevel = false;
			this.coveragePiece4.CanHaveArmorSet = false;
			this.coveragePiece4.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.LeftRing;
			this.coveragePiece4.Location = new System.Drawing.Point(8, 440);
			this.coveragePiece4.LockedSlot = false;
			this.coveragePiece4.Name = "coveragePiece4";
			this.coveragePiece4.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece4.TabIndex = 3;
			this.coveragePiece4.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece15
			// 
			this.coveragePiece15.ArmorLevel = 0;
			this.coveragePiece15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece15.CanHaveArmorLevel = false;
			this.coveragePiece15.CanHaveArmorSet = false;
			this.coveragePiece15.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.RightBracelet;
			this.coveragePiece15.Location = new System.Drawing.Point(712, 296);
			this.coveragePiece15.LockedSlot = false;
			this.coveragePiece15.Name = "coveragePiece15";
			this.coveragePiece15.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece15.TabIndex = 13;
			this.coveragePiece15.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece5
			// 
			this.coveragePiece5.ArmorLevel = 0;
			this.coveragePiece5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece5.CanHaveArmorLevel = true;
			this.coveragePiece5.CanHaveArmorSet = true;
			this.coveragePiece5.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.UpperArms;
			this.coveragePiece5.Location = new System.Drawing.Point(192, 152);
			this.coveragePiece5.LockedSlot = false;
			this.coveragePiece5.Name = "coveragePiece5";
			this.coveragePiece5.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece5.TabIndex = 4;
			this.coveragePiece5.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece13
			// 
			this.coveragePiece13.ArmorLevel = 0;
			this.coveragePiece13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece13.CanHaveArmorLevel = true;
			this.coveragePiece13.CanHaveArmorSet = true;
			this.coveragePiece13.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.Feet;
			this.coveragePiece13.Location = new System.Drawing.Point(528, 584);
			this.coveragePiece13.LockedSlot = false;
			this.coveragePiece13.Name = "coveragePiece13";
			this.coveragePiece13.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece13.TabIndex = 12;
			this.coveragePiece13.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece6
			// 
			this.coveragePiece6.ArmorLevel = 0;
			this.coveragePiece6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece6.CanHaveArmorLevel = true;
			this.coveragePiece6.CanHaveArmorSet = true;
			this.coveragePiece6.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.Chest;
			this.coveragePiece6.Location = new System.Drawing.Point(360, 152);
			this.coveragePiece6.LockedSlot = false;
			this.coveragePiece6.Name = "coveragePiece6";
			this.coveragePiece6.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece6.TabIndex = 5;
			this.coveragePiece6.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece12
			// 
			this.coveragePiece12.ArmorLevel = 0;
			this.coveragePiece12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece12.CanHaveArmorLevel = true;
			this.coveragePiece12.CanHaveArmorSet = true;
			this.coveragePiece12.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.LowerLegs;
			this.coveragePiece12.Location = new System.Drawing.Point(528, 440);
			this.coveragePiece12.LockedSlot = false;
			this.coveragePiece12.Name = "coveragePiece12";
			this.coveragePiece12.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece12.TabIndex = 11;
			this.coveragePiece12.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece7
			// 
			this.coveragePiece7.ArmorLevel = 0;
			this.coveragePiece7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece7.CanHaveArmorLevel = true;
			this.coveragePiece7.CanHaveArmorSet = true;
			this.coveragePiece7.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.Head;
			this.coveragePiece7.Location = new System.Drawing.Point(360, 8);
			this.coveragePiece7.LockedSlot = false;
			this.coveragePiece7.Name = "coveragePiece7";
			this.coveragePiece7.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece7.TabIndex = 6;
			this.coveragePiece7.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece11
			// 
			this.coveragePiece11.ArmorLevel = 0;
			this.coveragePiece11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece11.CanHaveArmorLevel = true;
			this.coveragePiece11.CanHaveArmorSet = true;
			this.coveragePiece11.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.Hands;
			this.coveragePiece11.Location = new System.Drawing.Point(192, 440);
			this.coveragePiece11.LockedSlot = false;
			this.coveragePiece11.Name = "coveragePiece11";
			this.coveragePiece11.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece11.TabIndex = 10;
			this.coveragePiece11.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece8
			// 
			this.coveragePiece8.ArmorLevel = 0;
			this.coveragePiece8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece8.CanHaveArmorLevel = true;
			this.coveragePiece8.CanHaveArmorSet = true;
			this.coveragePiece8.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.LowerArms;
			this.coveragePiece8.Location = new System.Drawing.Point(192, 296);
			this.coveragePiece8.LockedSlot = false;
			this.coveragePiece8.Name = "coveragePiece8";
			this.coveragePiece8.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece8.TabIndex = 7;
			this.coveragePiece8.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece10
			// 
			this.coveragePiece10.ArmorLevel = 0;
			this.coveragePiece10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece10.CanHaveArmorLevel = true;
			this.coveragePiece10.CanHaveArmorSet = true;
			this.coveragePiece10.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.UpperLegs;
			this.coveragePiece10.Location = new System.Drawing.Point(528, 296);
			this.coveragePiece10.LockedSlot = false;
			this.coveragePiece10.Name = "coveragePiece10";
			this.coveragePiece10.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece10.TabIndex = 9;
			this.coveragePiece10.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// coveragePiece9
			// 
			this.coveragePiece9.ArmorLevel = 0;
			this.coveragePiece9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.coveragePiece9.CanHaveArmorLevel = true;
			this.coveragePiece9.CanHaveArmorSet = true;
			this.coveragePiece9.EquipableSlot = Mag_SuitBuilder.Constants.EquippableSlotFlags.Abdomen;
			this.coveragePiece9.Location = new System.Drawing.Point(360, 296);
			this.coveragePiece9.LockedSlot = false;
			this.coveragePiece9.Name = "coveragePiece9";
			this.coveragePiece9.Size = new System.Drawing.Size(155, 135);
			this.coveragePiece9.TabIndex = 8;
			this.coveragePiece9.UnderwearCoverage = Mag_SuitBuilder.Constants.UnderwearCoverage.None;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.txtEquipmentEntries);
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(1042, 724);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Equipment Text Entries";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// txtEquipmentEntries
			// 
			this.txtEquipmentEntries.Location = new System.Drawing.Point(8, 32);
			this.txtEquipmentEntries.Multiline = true;
			this.txtEquipmentEntries.Name = "txtEquipmentEntries";
			this.txtEquipmentEntries.Size = new System.Drawing.Size(1024, 688);
			this.txtEquipmentEntries.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(325, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Paste the Mag-Tools item info identification strings in the box below.";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1050, 750);
			this.Controls.Add(this.tabControl1);
			this.Name = "Form1";
			this.Text = "Mag-Suit Builder";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private CoveragePiece coveragePiece1;
		private CoveragePiece coveragePiece2;
		private CoveragePiece coveragePiece3;
		private CoveragePiece coveragePiece4;
		private CoveragePiece coveragePiece5;
		private CoveragePiece coveragePiece6;
		private CoveragePiece coveragePiece7;
		private CoveragePiece coveragePiece8;
		private CoveragePiece coveragePiece9;
		private CoveragePiece coveragePiece10;
		private CoveragePiece coveragePiece11;
		private CoveragePiece coveragePiece12;
		private CoveragePiece coveragePiece13;
		private CoveragePiece coveragePiece14;
		private CoveragePiece coveragePiece15;
		private CoveragePiece coveragePiece16;
		private CoveragePiece coveragePiece17;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TextBox txtEquipmentEntries;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button calculatePossibilities;
		private System.Windows.Forms.ListView listPossibilities;
	}
}

