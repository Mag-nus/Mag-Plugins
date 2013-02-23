namespace Mag_SuitBuilder.Equipment
{
	partial class FiltersControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnClearAllFilters = new System.Windows.Forms.Button();
			this.txtMinimumBaseArmorLevel = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.checkRemoveEquipped = new System.Windows.Forms.CheckBox();
			this.chkArmor = new System.Windows.Forms.CheckBox();
			this.chkClothing = new System.Windows.Forms.CheckBox();
			this.chkJewelry = new System.Windows.Forms.CheckBox();
			this.chkMeleeWeapon = new System.Windows.Forms.CheckBox();
			this.chkMissileWeapon = new System.Windows.Forms.CheckBox();
			this.chkWandStaffOrb = new System.Windows.Forms.CheckBox();
			this.chkAllElseObjectClasses = new System.Windows.Forms.CheckBox();
			this.chkRemoveUnequipped = new System.Windows.Forms.CheckBox();
			this.cantripSelectorControl1 = new Mag_SuitBuilder.Spells.CantripSelectorControl();
			this.cboSecondaryArmorSet = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cboPrimaryArmorSet = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnClearAllFilters
			// 
			this.btnClearAllFilters.Location = new System.Drawing.Point(3, 3);
			this.btnClearAllFilters.Name = "btnClearAllFilters";
			this.btnClearAllFilters.Size = new System.Drawing.Size(75, 23);
			this.btnClearAllFilters.TabIndex = 41;
			this.btnClearAllFilters.Text = "Clear Filters";
			this.btnClearAllFilters.UseVisualStyleBackColor = true;
			this.btnClearAllFilters.Click += new System.EventHandler(this.btnClearAllFilters_Click);
			// 
			// txtMinimumBaseArmorLevel
			// 
			this.txtMinimumBaseArmorLevel.Location = new System.Drawing.Point(6, 78);
			this.txtMinimumBaseArmorLevel.MaxLength = 3;
			this.txtMinimumBaseArmorLevel.Name = "txtMinimumBaseArmorLevel";
			this.txtMinimumBaseArmorLevel.Size = new System.Drawing.Size(33, 20);
			this.txtMinimumBaseArmorLevel.TabIndex = 40;
			this.txtMinimumBaseArmorLevel.Text = "200";
			this.txtMinimumBaseArmorLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtMinimumBaseArmorLevel.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(45, 81);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(198, 13);
			this.label1.TabIndex = 39;
			this.label1.Text = "Minimum base armor level for body armor";
			// 
			// checkRemoveEquipped
			// 
			this.checkRemoveEquipped.AutoSize = true;
			this.checkRemoveEquipped.Location = new System.Drawing.Point(6, 32);
			this.checkRemoveEquipped.Name = "checkRemoveEquipped";
			this.checkRemoveEquipped.Size = new System.Drawing.Size(114, 17);
			this.checkRemoveEquipped.TabIndex = 38;
			this.checkRemoveEquipped.Text = "Remove Equipped";
			this.checkRemoveEquipped.UseVisualStyleBackColor = true;
			this.checkRemoveEquipped.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkArmor
			// 
			this.chkArmor.AutoSize = true;
			this.chkArmor.Checked = true;
			this.chkArmor.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkArmor.Location = new System.Drawing.Point(6, 104);
			this.chkArmor.Name = "chkArmor";
			this.chkArmor.Size = new System.Drawing.Size(53, 17);
			this.chkArmor.TabIndex = 42;
			this.chkArmor.Text = "Armor";
			this.chkArmor.UseVisualStyleBackColor = true;
			this.chkArmor.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkClothing
			// 
			this.chkClothing.AutoSize = true;
			this.chkClothing.Checked = true;
			this.chkClothing.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkClothing.Location = new System.Drawing.Point(6, 127);
			this.chkClothing.Name = "chkClothing";
			this.chkClothing.Size = new System.Drawing.Size(64, 17);
			this.chkClothing.TabIndex = 43;
			this.chkClothing.Text = "Clothing";
			this.chkClothing.UseVisualStyleBackColor = true;
			this.chkClothing.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkJewelry
			// 
			this.chkJewelry.AutoSize = true;
			this.chkJewelry.Checked = true;
			this.chkJewelry.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkJewelry.Location = new System.Drawing.Point(6, 150);
			this.chkJewelry.Name = "chkJewelry";
			this.chkJewelry.Size = new System.Drawing.Size(61, 17);
			this.chkJewelry.TabIndex = 44;
			this.chkJewelry.Text = "Jewelry";
			this.chkJewelry.UseVisualStyleBackColor = true;
			this.chkJewelry.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMeleeWeapon
			// 
			this.chkMeleeWeapon.AutoSize = true;
			this.chkMeleeWeapon.Checked = true;
			this.chkMeleeWeapon.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMeleeWeapon.Location = new System.Drawing.Point(6, 173);
			this.chkMeleeWeapon.Name = "chkMeleeWeapon";
			this.chkMeleeWeapon.Size = new System.Drawing.Size(99, 17);
			this.chkMeleeWeapon.TabIndex = 45;
			this.chkMeleeWeapon.Text = "Melee Weapon";
			this.chkMeleeWeapon.UseVisualStyleBackColor = true;
			this.chkMeleeWeapon.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMissileWeapon
			// 
			this.chkMissileWeapon.AutoSize = true;
			this.chkMissileWeapon.Checked = true;
			this.chkMissileWeapon.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMissileWeapon.Location = new System.Drawing.Point(6, 196);
			this.chkMissileWeapon.Name = "chkMissileWeapon";
			this.chkMissileWeapon.Size = new System.Drawing.Size(101, 17);
			this.chkMissileWeapon.TabIndex = 46;
			this.chkMissileWeapon.Text = "Missile Weapon";
			this.chkMissileWeapon.UseVisualStyleBackColor = true;
			this.chkMissileWeapon.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkWandStaffOrb
			// 
			this.chkWandStaffOrb.AutoSize = true;
			this.chkWandStaffOrb.Checked = true;
			this.chkWandStaffOrb.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkWandStaffOrb.Location = new System.Drawing.Point(6, 219);
			this.chkWandStaffOrb.Name = "chkWandStaffOrb";
			this.chkWandStaffOrb.Size = new System.Drawing.Size(100, 17);
			this.chkWandStaffOrb.TabIndex = 47;
			this.chkWandStaffOrb.Text = "Wand Staff Orb";
			this.chkWandStaffOrb.UseVisualStyleBackColor = true;
			this.chkWandStaffOrb.CheckStateChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkAllElseObjectClasses
			// 
			this.chkAllElseObjectClasses.AutoSize = true;
			this.chkAllElseObjectClasses.Checked = true;
			this.chkAllElseObjectClasses.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAllElseObjectClasses.Location = new System.Drawing.Point(6, 242);
			this.chkAllElseObjectClasses.Name = "chkAllElseObjectClasses";
			this.chkAllElseObjectClasses.Size = new System.Drawing.Size(60, 17);
			this.chkAllElseObjectClasses.TabIndex = 48;
			this.chkAllElseObjectClasses.Text = "All Else";
			this.chkAllElseObjectClasses.UseVisualStyleBackColor = true;
			this.chkAllElseObjectClasses.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkRemoveUnequipped
			// 
			this.chkRemoveUnequipped.AutoSize = true;
			this.chkRemoveUnequipped.Location = new System.Drawing.Point(6, 55);
			this.chkRemoveUnequipped.Name = "chkRemoveUnequipped";
			this.chkRemoveUnequipped.Size = new System.Drawing.Size(127, 17);
			this.chkRemoveUnequipped.TabIndex = 49;
			this.chkRemoveUnequipped.Text = "Remove Unequipped";
			this.chkRemoveUnequipped.UseVisualStyleBackColor = true;
			this.chkRemoveUnequipped.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// cantripSelectorControl1
			// 
			this.cantripSelectorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cantripSelectorControl1.Location = new System.Drawing.Point(0, 263);
			this.cantripSelectorControl1.Name = "cantripSelectorControl1";
			this.cantripSelectorControl1.Size = new System.Drawing.Size(528, 182);
			this.cantripSelectorControl1.TabIndex = 51;
			// 
			// cboSecondaryArmorSet
			// 
			this.cboSecondaryArmorSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSecondaryArmorSet.Location = new System.Drawing.Point(237, 26);
			this.cboSecondaryArmorSet.Name = "cboSecondaryArmorSet";
			this.cboSecondaryArmorSet.Size = new System.Drawing.Size(144, 21);
			this.cboSecondaryArmorSet.TabIndex = 55;
			this.cboSecondaryArmorSet.SelectedIndexChanged += new System.EventHandler(this.cboFilter_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(124, 29);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(107, 13);
			this.label2.TabIndex = 54;
			this.label2.Text = "Secondary Armor Set";
			// 
			// cboPrimaryArmorSet
			// 
			this.cboPrimaryArmorSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPrimaryArmorSet.Location = new System.Drawing.Point(237, 3);
			this.cboPrimaryArmorSet.Name = "cboPrimaryArmorSet";
			this.cboPrimaryArmorSet.Size = new System.Drawing.Size(144, 21);
			this.cboPrimaryArmorSet.TabIndex = 53;
			this.cboPrimaryArmorSet.SelectedIndexChanged += new System.EventHandler(this.cboFilter_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(124, 6);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 13);
			this.label3.TabIndex = 52;
			this.label3.Text = "Primary Armor Set";
			// 
			// FiltersControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.cboSecondaryArmorSet);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cboPrimaryArmorSet);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cantripSelectorControl1);
			this.Controls.Add(this.chkRemoveUnequipped);
			this.Controls.Add(this.chkAllElseObjectClasses);
			this.Controls.Add(this.chkWandStaffOrb);
			this.Controls.Add(this.chkMissileWeapon);
			this.Controls.Add(this.chkMeleeWeapon);
			this.Controls.Add(this.chkJewelry);
			this.Controls.Add(this.chkClothing);
			this.Controls.Add(this.chkArmor);
			this.Controls.Add(this.btnClearAllFilters);
			this.Controls.Add(this.txtMinimumBaseArmorLevel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkRemoveEquipped);
			this.Name = "FiltersControl";
			this.Size = new System.Drawing.Size(530, 445);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnClearAllFilters;
		private System.Windows.Forms.TextBox txtMinimumBaseArmorLevel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkRemoveEquipped;
		private System.Windows.Forms.CheckBox chkArmor;
		private System.Windows.Forms.CheckBox chkClothing;
		private System.Windows.Forms.CheckBox chkJewelry;
		private System.Windows.Forms.CheckBox chkMeleeWeapon;
		private System.Windows.Forms.CheckBox chkMissileWeapon;
		private System.Windows.Forms.CheckBox chkWandStaffOrb;
		private System.Windows.Forms.CheckBox chkAllElseObjectClasses;
		private System.Windows.Forms.CheckBox chkRemoveUnequipped;
		private Spells.CantripSelectorControl cantripSelectorControl1;
		private System.Windows.Forms.ComboBox cboSecondaryArmorSet;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cboPrimaryArmorSet;
		private System.Windows.Forms.Label label3;
	}
}
