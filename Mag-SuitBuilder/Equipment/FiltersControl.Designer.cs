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
			this.cboSecondaryArmorSet = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cboPrimaryArmorSet = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.chkSalvage = new System.Windows.Forms.CheckBox();
			this.chkContainersFoci = new System.Windows.Forms.CheckBox();
			this.chkMoneyNotesKeys = new System.Windows.Forms.CheckBox();
			this.chkCompsKitsFoodManaStones = new System.Windows.Forms.CheckBox();
			this.chkMasteryBow = new System.Windows.Forms.CheckBox();
			this.chkMasteryCrossbow = new System.Windows.Forms.CheckBox();
			this.chkMasteryThrown = new System.Windows.Forms.CheckBox();
			this.chkMasteryUA = new System.Windows.Forms.CheckBox();
			this.chkMasterySword = new System.Windows.Forms.CheckBox();
			this.chkMasteryAxe = new System.Windows.Forms.CheckBox();
			this.chkMasteryMace = new System.Windows.Forms.CheckBox();
			this.chkMasterySpear = new System.Windows.Forms.CheckBox();
			this.chkMasteryDagger = new System.Windows.Forms.CheckBox();
			this.chkMasteryStaff = new System.Windows.Forms.CheckBox();
			this.chkMeleeHeavy = new System.Windows.Forms.CheckBox();
			this.chkMeleeLight = new System.Windows.Forms.CheckBox();
			this.chkMeleeFinesse = new System.Windows.Forms.CheckBox();
			this.chkMelee2H = new System.Windows.Forms.CheckBox();
			this.cantripSelectorControl1 = new Mag_SuitBuilder.Spells.CantripSelectorControl();
			this.chkWandStaffOrbWar = new System.Windows.Forms.CheckBox();
			this.chkWandStaffOrbVoid = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtMinEpics = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtMinLegendaries = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtMinTotalRating = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.txtMinDefensiveRating = new System.Windows.Forms.TextBox();
			this.txtMinOffensiveRating = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtMinimumBaseArmorLevel
			// 
			this.txtMinimumBaseArmorLevel.Location = new System.Drawing.Point(3, 49);
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
			this.label1.Location = new System.Drawing.Point(42, 52);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(198, 13);
			this.label1.TabIndex = 39;
			this.label1.Text = "Minimum base armor level for body armor";
			// 
			// checkRemoveEquipped
			// 
			this.checkRemoveEquipped.AutoSize = true;
			this.checkRemoveEquipped.Location = new System.Drawing.Point(3, 3);
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
			this.chkArmor.Location = new System.Drawing.Point(3, 75);
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
			this.chkClothing.Location = new System.Drawing.Point(3, 98);
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
			this.chkJewelry.Location = new System.Drawing.Point(3, 121);
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
			this.chkMeleeWeapon.Location = new System.Drawing.Point(3, 144);
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
			this.chkMissileWeapon.Location = new System.Drawing.Point(3, 213);
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
			this.chkWandStaffOrb.Location = new System.Drawing.Point(3, 259);
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
			this.chkAllElseObjectClasses.Location = new System.Drawing.Point(2, 397);
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
			this.chkRemoveUnequipped.Location = new System.Drawing.Point(3, 26);
			this.chkRemoveUnequipped.Name = "chkRemoveUnequipped";
			this.chkRemoveUnequipped.Size = new System.Drawing.Size(127, 17);
			this.chkRemoveUnequipped.TabIndex = 49;
			this.chkRemoveUnequipped.Text = "Remove Unequipped";
			this.chkRemoveUnequipped.UseVisualStyleBackColor = true;
			this.chkRemoveUnequipped.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// cboSecondaryArmorSet
			// 
			this.cboSecondaryArmorSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSecondaryArmorSet.Location = new System.Drawing.Point(269, 26);
			this.cboSecondaryArmorSet.Name = "cboSecondaryArmorSet";
			this.cboSecondaryArmorSet.Size = new System.Drawing.Size(144, 21);
			this.cboSecondaryArmorSet.TabIndex = 55;
			this.cboSecondaryArmorSet.SelectedIndexChanged += new System.EventHandler(this.cboFilter_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(156, 29);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(107, 13);
			this.label2.TabIndex = 54;
			this.label2.Text = "Secondary Armor Set";
			// 
			// cboPrimaryArmorSet
			// 
			this.cboPrimaryArmorSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPrimaryArmorSet.Location = new System.Drawing.Point(269, 3);
			this.cboPrimaryArmorSet.Name = "cboPrimaryArmorSet";
			this.cboPrimaryArmorSet.Size = new System.Drawing.Size(144, 21);
			this.cboPrimaryArmorSet.TabIndex = 53;
			this.cboPrimaryArmorSet.SelectedIndexChanged += new System.EventHandler(this.cboFilter_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(156, 6);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 13);
			this.label3.TabIndex = 52;
			this.label3.Text = "Primary Armor Set";
			// 
			// chkSalvage
			// 
			this.chkSalvage.AutoSize = true;
			this.chkSalvage.Location = new System.Drawing.Point(3, 305);
			this.chkSalvage.Name = "chkSalvage";
			this.chkSalvage.Size = new System.Drawing.Size(65, 17);
			this.chkSalvage.TabIndex = 56;
			this.chkSalvage.Text = "Salvage";
			this.chkSalvage.UseVisualStyleBackColor = true;
			this.chkSalvage.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkContainersFoci
			// 
			this.chkContainersFoci.AutoSize = true;
			this.chkContainersFoci.Location = new System.Drawing.Point(3, 328);
			this.chkContainersFoci.Name = "chkContainersFoci";
			this.chkContainersFoci.Size = new System.Drawing.Size(108, 17);
			this.chkContainersFoci.TabIndex = 57;
			this.chkContainersFoci.Text = "Containers && Foci";
			this.chkContainersFoci.UseVisualStyleBackColor = true;
			this.chkContainersFoci.CheckedChanged += new System.EventHandler(this.cboFilter_SelectedIndexChanged);
			// 
			// chkMoneyNotesKeys
			// 
			this.chkMoneyNotesKeys.AutoSize = true;
			this.chkMoneyNotesKeys.Location = new System.Drawing.Point(2, 351);
			this.chkMoneyNotesKeys.Name = "chkMoneyNotesKeys";
			this.chkMoneyNotesKeys.Size = new System.Drawing.Size(133, 17);
			this.chkMoneyNotesKeys.TabIndex = 58;
			this.chkMoneyNotesKeys.Text = "Money && Notes && Keys";
			this.chkMoneyNotesKeys.UseVisualStyleBackColor = true;
			this.chkMoneyNotesKeys.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkCompsKitsFoodManaStones
			// 
			this.chkCompsKitsFoodManaStones.AutoSize = true;
			this.chkCompsKitsFoodManaStones.Location = new System.Drawing.Point(2, 374);
			this.chkCompsKitsFoodManaStones.Name = "chkCompsKitsFoodManaStones";
			this.chkCompsKitsFoodManaStones.Size = new System.Drawing.Size(195, 17);
			this.chkCompsKitsFoodManaStones.TabIndex = 59;
			this.chkCompsKitsFoodManaStones.Text = "Comps && Kits && Food && ManaStones";
			this.chkCompsKitsFoodManaStones.UseVisualStyleBackColor = true;
			this.chkCompsKitsFoodManaStones.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMasteryBow
			// 
			this.chkMasteryBow.AutoSize = true;
			this.chkMasteryBow.Checked = true;
			this.chkMasteryBow.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMasteryBow.Location = new System.Drawing.Point(9, 236);
			this.chkMasteryBow.Name = "chkMasteryBow";
			this.chkMasteryBow.Size = new System.Drawing.Size(47, 17);
			this.chkMasteryBow.TabIndex = 60;
			this.chkMasteryBow.Text = "Bow";
			this.chkMasteryBow.UseVisualStyleBackColor = true;
			this.chkMasteryBow.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMasteryCrossbow
			// 
			this.chkMasteryCrossbow.AutoSize = true;
			this.chkMasteryCrossbow.Checked = true;
			this.chkMasteryCrossbow.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMasteryCrossbow.Location = new System.Drawing.Point(55, 236);
			this.chkMasteryCrossbow.Name = "chkMasteryCrossbow";
			this.chkMasteryCrossbow.Size = new System.Drawing.Size(72, 17);
			this.chkMasteryCrossbow.TabIndex = 61;
			this.chkMasteryCrossbow.Text = "Crossbow";
			this.chkMasteryCrossbow.UseVisualStyleBackColor = true;
			this.chkMasteryCrossbow.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMasteryThrown
			// 
			this.chkMasteryThrown.AutoSize = true;
			this.chkMasteryThrown.Checked = true;
			this.chkMasteryThrown.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMasteryThrown.Location = new System.Drawing.Point(133, 236);
			this.chkMasteryThrown.Name = "chkMasteryThrown";
			this.chkMasteryThrown.Size = new System.Drawing.Size(62, 17);
			this.chkMasteryThrown.TabIndex = 62;
			this.chkMasteryThrown.Text = "Thrown";
			this.chkMasteryThrown.UseVisualStyleBackColor = true;
			this.chkMasteryThrown.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMasteryUA
			// 
			this.chkMasteryUA.AutoSize = true;
			this.chkMasteryUA.Checked = true;
			this.chkMasteryUA.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMasteryUA.Location = new System.Drawing.Point(14, 190);
			this.chkMasteryUA.Name = "chkMasteryUA";
			this.chkMasteryUA.Size = new System.Drawing.Size(41, 17);
			this.chkMasteryUA.TabIndex = 63;
			this.chkMasteryUA.Text = "UA";
			this.chkMasteryUA.UseVisualStyleBackColor = true;
			this.chkMasteryUA.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMasterySword
			// 
			this.chkMasterySword.AutoSize = true;
			this.chkMasterySword.Checked = true;
			this.chkMasterySword.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMasterySword.Location = new System.Drawing.Point(61, 190);
			this.chkMasterySword.Name = "chkMasterySword";
			this.chkMasterySword.Size = new System.Drawing.Size(56, 17);
			this.chkMasterySword.TabIndex = 66;
			this.chkMasterySword.Text = "Sword";
			this.chkMasterySword.UseVisualStyleBackColor = true;
			this.chkMasterySword.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMasteryAxe
			// 
			this.chkMasteryAxe.AutoSize = true;
			this.chkMasteryAxe.Checked = true;
			this.chkMasteryAxe.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMasteryAxe.Location = new System.Drawing.Point(123, 190);
			this.chkMasteryAxe.Name = "chkMasteryAxe";
			this.chkMasteryAxe.Size = new System.Drawing.Size(44, 17);
			this.chkMasteryAxe.TabIndex = 67;
			this.chkMasteryAxe.Text = "Axe";
			this.chkMasteryAxe.UseVisualStyleBackColor = true;
			this.chkMasteryAxe.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMasteryMace
			// 
			this.chkMasteryMace.AutoSize = true;
			this.chkMasteryMace.Checked = true;
			this.chkMasteryMace.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMasteryMace.Location = new System.Drawing.Point(173, 190);
			this.chkMasteryMace.Name = "chkMasteryMace";
			this.chkMasteryMace.Size = new System.Drawing.Size(53, 17);
			this.chkMasteryMace.TabIndex = 68;
			this.chkMasteryMace.Text = "Mace";
			this.chkMasteryMace.UseVisualStyleBackColor = true;
			this.chkMasteryMace.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMasterySpear
			// 
			this.chkMasterySpear.AutoSize = true;
			this.chkMasterySpear.Checked = true;
			this.chkMasterySpear.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMasterySpear.Location = new System.Drawing.Point(232, 190);
			this.chkMasterySpear.Name = "chkMasterySpear";
			this.chkMasterySpear.Size = new System.Drawing.Size(54, 17);
			this.chkMasterySpear.TabIndex = 69;
			this.chkMasterySpear.Text = "Spear";
			this.chkMasterySpear.UseVisualStyleBackColor = true;
			this.chkMasterySpear.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMasteryDagger
			// 
			this.chkMasteryDagger.AutoSize = true;
			this.chkMasteryDagger.Checked = true;
			this.chkMasteryDagger.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMasteryDagger.Location = new System.Drawing.Point(292, 190);
			this.chkMasteryDagger.Name = "chkMasteryDagger";
			this.chkMasteryDagger.Size = new System.Drawing.Size(61, 17);
			this.chkMasteryDagger.TabIndex = 70;
			this.chkMasteryDagger.Text = "Dagger";
			this.chkMasteryDagger.UseVisualStyleBackColor = true;
			this.chkMasteryDagger.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMasteryStaff
			// 
			this.chkMasteryStaff.AutoSize = true;
			this.chkMasteryStaff.Checked = true;
			this.chkMasteryStaff.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMasteryStaff.Location = new System.Drawing.Point(359, 190);
			this.chkMasteryStaff.Name = "chkMasteryStaff";
			this.chkMasteryStaff.Size = new System.Drawing.Size(48, 17);
			this.chkMasteryStaff.TabIndex = 71;
			this.chkMasteryStaff.Text = "Staff";
			this.chkMasteryStaff.UseVisualStyleBackColor = true;
			this.chkMasteryStaff.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMeleeHeavy
			// 
			this.chkMeleeHeavy.AutoSize = true;
			this.chkMeleeHeavy.Checked = true;
			this.chkMeleeHeavy.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMeleeHeavy.Location = new System.Drawing.Point(14, 167);
			this.chkMeleeHeavy.Name = "chkMeleeHeavy";
			this.chkMeleeHeavy.Size = new System.Drawing.Size(57, 17);
			this.chkMeleeHeavy.TabIndex = 72;
			this.chkMeleeHeavy.Text = "Heavy";
			this.chkMeleeHeavy.UseVisualStyleBackColor = true;
			this.chkMeleeHeavy.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMeleeLight
			// 
			this.chkMeleeLight.AutoSize = true;
			this.chkMeleeLight.Checked = true;
			this.chkMeleeLight.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMeleeLight.Location = new System.Drawing.Point(77, 167);
			this.chkMeleeLight.Name = "chkMeleeLight";
			this.chkMeleeLight.Size = new System.Drawing.Size(49, 17);
			this.chkMeleeLight.TabIndex = 73;
			this.chkMeleeLight.Text = "Light";
			this.chkMeleeLight.UseVisualStyleBackColor = true;
			this.chkMeleeLight.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMeleeFinesse
			// 
			this.chkMeleeFinesse.AutoSize = true;
			this.chkMeleeFinesse.Checked = true;
			this.chkMeleeFinesse.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMeleeFinesse.Location = new System.Drawing.Point(132, 167);
			this.chkMeleeFinesse.Name = "chkMeleeFinesse";
			this.chkMeleeFinesse.Size = new System.Drawing.Size(62, 17);
			this.chkMeleeFinesse.TabIndex = 74;
			this.chkMeleeFinesse.Text = "Finesse";
			this.chkMeleeFinesse.UseVisualStyleBackColor = true;
			this.chkMeleeFinesse.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkMelee2H
			// 
			this.chkMelee2H.AutoSize = true;
			this.chkMelee2H.Checked = true;
			this.chkMelee2H.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMelee2H.Location = new System.Drawing.Point(200, 167);
			this.chkMelee2H.Name = "chkMelee2H";
			this.chkMelee2H.Size = new System.Drawing.Size(40, 17);
			this.chkMelee2H.TabIndex = 75;
			this.chkMelee2H.Text = "2H";
			this.chkMelee2H.UseVisualStyleBackColor = true;
			this.chkMelee2H.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// cantripSelectorControl1
			// 
			this.cantripSelectorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cantripSelectorControl1.Location = new System.Drawing.Point(0, 569);
			this.cantripSelectorControl1.Name = "cantripSelectorControl1";
			this.cantripSelectorControl1.Size = new System.Drawing.Size(528, 182);
			this.cantripSelectorControl1.TabIndex = 51;
			// 
			// chkWandStaffOrbWar
			// 
			this.chkWandStaffOrbWar.AutoSize = true;
			this.chkWandStaffOrbWar.Checked = true;
			this.chkWandStaffOrbWar.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkWandStaffOrbWar.Location = new System.Drawing.Point(9, 282);
			this.chkWandStaffOrbWar.Name = "chkWandStaffOrbWar";
			this.chkWandStaffOrbWar.Size = new System.Drawing.Size(46, 17);
			this.chkWandStaffOrbWar.TabIndex = 76;
			this.chkWandStaffOrbWar.Text = "War";
			this.chkWandStaffOrbWar.UseVisualStyleBackColor = true;
			this.chkWandStaffOrbWar.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// chkWandStaffOrbVoid
			// 
			this.chkWandStaffOrbVoid.AutoSize = true;
			this.chkWandStaffOrbVoid.Checked = true;
			this.chkWandStaffOrbVoid.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkWandStaffOrbVoid.Location = new System.Drawing.Point(55, 282);
			this.chkWandStaffOrbVoid.Name = "chkWandStaffOrbVoid";
			this.chkWandStaffOrbVoid.Size = new System.Drawing.Size(47, 17);
			this.chkWandStaffOrbVoid.TabIndex = 77;
			this.chkWandStaffOrbVoid.Text = "Void";
			this.chkWandStaffOrbVoid.UseVisualStyleBackColor = true;
			this.chkWandStaffOrbVoid.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(266, 282);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(141, 13);
			this.label4.TabIndex = 82;
			this.label4.Text = "Spells Equal Or Better Than:";
			// 
			// txtMinEpics
			// 
			this.txtMinEpics.Location = new System.Drawing.Point(329, 320);
			this.txtMinEpics.MaxLength = 1;
			this.txtMinEpics.Name = "txtMinEpics";
			this.txtMinEpics.Size = new System.Drawing.Size(18, 20);
			this.txtMinEpics.TabIndex = 81;
			this.txtMinEpics.Text = "0";
			this.txtMinEpics.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(266, 323);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(28, 13);
			this.label5.TabIndex = 80;
			this.label5.Text = "Epic";
			// 
			// txtMinLegendaries
			// 
			this.txtMinLegendaries.Location = new System.Drawing.Point(329, 298);
			this.txtMinLegendaries.MaxLength = 1;
			this.txtMinLegendaries.Name = "txtMinLegendaries";
			this.txtMinLegendaries.Size = new System.Drawing.Size(18, 20);
			this.txtMinLegendaries.TabIndex = 79;
			this.txtMinLegendaries.Text = "0";
			this.txtMinLegendaries.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(266, 301);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(57, 13);
			this.label6.TabIndex = 78;
			this.label6.Text = "Legendary";
			// 
			// txtMinTotalRating
			// 
			this.txtMinTotalRating.Location = new System.Drawing.Point(259, 119);
			this.txtMinTotalRating.MaxLength = 1;
			this.txtMinTotalRating.Name = "txtMinTotalRating";
			this.txtMinTotalRating.Size = new System.Drawing.Size(18, 20);
			this.txtMinTotalRating.TabIndex = 84;
			this.txtMinTotalRating.Text = "0";
			this.txtMinTotalRating.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(141, 121);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(112, 13);
			this.label7.TabIndex = 85;
			this.label7.Text = "Minimum Total Rating:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(117, 98);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(136, 13);
			this.label8.TabIndex = 86;
			this.label8.Text = "Minimum Defensive Rating:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(120, 76);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(133, 13);
			this.label9.TabIndex = 87;
			this.label9.Text = "Minimum Offensive Rating:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// txtMinDefensiveRating
			// 
			this.txtMinDefensiveRating.Location = new System.Drawing.Point(259, 95);
			this.txtMinDefensiveRating.MaxLength = 1;
			this.txtMinDefensiveRating.Name = "txtMinDefensiveRating";
			this.txtMinDefensiveRating.Size = new System.Drawing.Size(18, 20);
			this.txtMinDefensiveRating.TabIndex = 88;
			this.txtMinDefensiveRating.Text = "0";
			this.txtMinDefensiveRating.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
			// 
			// txtMinOffensiveRating
			// 
			this.txtMinOffensiveRating.Location = new System.Drawing.Point(259, 72);
			this.txtMinOffensiveRating.MaxLength = 1;
			this.txtMinOffensiveRating.Name = "txtMinOffensiveRating";
			this.txtMinOffensiveRating.Size = new System.Drawing.Size(18, 20);
			this.txtMinOffensiveRating.TabIndex = 89;
			this.txtMinOffensiveRating.Text = "0";
			this.txtMinOffensiveRating.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
			// 
			// FiltersControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtMinOffensiveRating);
			this.Controls.Add(this.txtMinDefensiveRating);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.txtMinTotalRating);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtMinEpics);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtMinLegendaries);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.chkWandStaffOrbVoid);
			this.Controls.Add(this.chkWandStaffOrbWar);
			this.Controls.Add(this.chkMelee2H);
			this.Controls.Add(this.chkMeleeFinesse);
			this.Controls.Add(this.chkMeleeLight);
			this.Controls.Add(this.chkMeleeHeavy);
			this.Controls.Add(this.chkMasteryStaff);
			this.Controls.Add(this.chkMasteryDagger);
			this.Controls.Add(this.chkMasterySpear);
			this.Controls.Add(this.chkMasteryMace);
			this.Controls.Add(this.chkMasteryAxe);
			this.Controls.Add(this.chkMasterySword);
			this.Controls.Add(this.chkMasteryUA);
			this.Controls.Add(this.chkMasteryThrown);
			this.Controls.Add(this.chkMasteryCrossbow);
			this.Controls.Add(this.chkMasteryBow);
			this.Controls.Add(this.chkCompsKitsFoodManaStones);
			this.Controls.Add(this.chkMoneyNotesKeys);
			this.Controls.Add(this.chkContainersFoci);
			this.Controls.Add(this.chkSalvage);
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
			this.Controls.Add(this.txtMinimumBaseArmorLevel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkRemoveEquipped);
			this.Name = "FiltersControl";
			this.Size = new System.Drawing.Size(530, 751);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

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
		private System.Windows.Forms.ComboBox cboSecondaryArmorSet;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cboPrimaryArmorSet;
		private System.Windows.Forms.Label label3;
		private Spells.CantripSelectorControl cantripSelectorControl1;
		private System.Windows.Forms.CheckBox chkSalvage;
		private System.Windows.Forms.CheckBox chkContainersFoci;
		private System.Windows.Forms.CheckBox chkMoneyNotesKeys;
		private System.Windows.Forms.CheckBox chkCompsKitsFoodManaStones;
		private System.Windows.Forms.CheckBox chkMasteryBow;
		private System.Windows.Forms.CheckBox chkMasteryCrossbow;
		private System.Windows.Forms.CheckBox chkMasteryThrown;
		private System.Windows.Forms.CheckBox chkMasteryUA;
		private System.Windows.Forms.CheckBox chkMasterySword;
		private System.Windows.Forms.CheckBox chkMasteryAxe;
		private System.Windows.Forms.CheckBox chkMasteryMace;
		private System.Windows.Forms.CheckBox chkMasterySpear;
		private System.Windows.Forms.CheckBox chkMasteryDagger;
		private System.Windows.Forms.CheckBox chkMasteryStaff;
		private System.Windows.Forms.CheckBox chkMeleeHeavy;
		private System.Windows.Forms.CheckBox chkMeleeLight;
		private System.Windows.Forms.CheckBox chkMeleeFinesse;
		private System.Windows.Forms.CheckBox chkMelee2H;
		private System.Windows.Forms.CheckBox chkWandStaffOrbWar;
		private System.Windows.Forms.CheckBox chkWandStaffOrbVoid;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtMinEpics;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtMinLegendaries;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtMinTotalRating;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtMinDefensiveRating;
		private System.Windows.Forms.TextBox txtMinOffensiveRating;
	}
}
