namespace Mag_SuitBuilder
{
	partial class EquipmentPieceControl
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
            this.txtArmorLevel = new System.Windows.Forms.TextBox();
            this.lblItemName = new System.Windows.Forms.Label();
            this.txtSpell1 = new System.Windows.Forms.TextBox();
            this.txtSpell2 = new System.Windows.Forms.TextBox();
            this.txtSpell3 = new System.Windows.Forms.TextBox();
            this.txtArmorSet = new System.Windows.Forms.TextBox();
            this.txtSpell4 = new System.Windows.Forms.TextBox();
            this.chkLocked = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtArmorLevel
            // 
            this.txtArmorLevel.Location = new System.Drawing.Point(0, 16);
            this.txtArmorLevel.Name = "txtArmorLevel";
            this.txtArmorLevel.ReadOnly = true;
            this.txtArmorLevel.Size = new System.Drawing.Size(32, 20);
            this.txtArmorLevel.TabIndex = 4;
            // 
            // lblItemName
            // 
            this.lblItemName.AutoSize = true;
            this.lblItemName.Location = new System.Drawing.Point(16, 0);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(58, 13);
            this.lblItemName.TabIndex = 6;
            this.lblItemName.Text = "Item Name";
            // 
            // txtSpell1
            // 
            this.txtSpell1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpell1.Location = new System.Drawing.Point(0, 39);
            this.txtSpell1.Name = "txtSpell1";
            this.txtSpell1.Size = new System.Drawing.Size(152, 20);
            this.txtSpell1.TabIndex = 8;
            // 
            // txtSpell2
            // 
            this.txtSpell2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpell2.Location = new System.Drawing.Point(0, 63);
            this.txtSpell2.Name = "txtSpell2";
            this.txtSpell2.Size = new System.Drawing.Size(152, 20);
            this.txtSpell2.TabIndex = 9;
            // 
            // txtSpell3
            // 
            this.txtSpell3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpell3.Location = new System.Drawing.Point(0, 87);
            this.txtSpell3.Name = "txtSpell3";
            this.txtSpell3.Size = new System.Drawing.Size(152, 20);
            this.txtSpell3.TabIndex = 10;
            // 
            // txtArmorSet
            // 
            this.txtArmorSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtArmorSet.Location = new System.Drawing.Point(40, 16);
            this.txtArmorSet.Name = "txtArmorSet";
            this.txtArmorSet.Size = new System.Drawing.Size(112, 20);
            this.txtArmorSet.TabIndex = 11;
            // 
            // txtSpell4
            // 
            this.txtSpell4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpell4.Location = new System.Drawing.Point(0, 111);
            this.txtSpell4.Name = "txtSpell4";
            this.txtSpell4.Size = new System.Drawing.Size(152, 20);
            this.txtSpell4.TabIndex = 12;
            // 
            // chkLocked
            // 
            this.chkLocked.AutoSize = true;
            this.chkLocked.Location = new System.Drawing.Point(0, 0);
            this.chkLocked.Name = "chkLocked";
            this.chkLocked.Size = new System.Drawing.Size(15, 14);
            this.chkLocked.TabIndex = 13;
            this.chkLocked.UseVisualStyleBackColor = true;
            // 
            // EquipmentPieceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.chkLocked);
            this.Controls.Add(this.txtSpell4);
            this.Controls.Add(this.txtArmorSet);
            this.Controls.Add(this.txtSpell3);
            this.Controls.Add(this.txtSpell2);
            this.Controls.Add(this.txtSpell1);
            this.Controls.Add(this.lblItemName);
            this.Controls.Add(this.txtArmorLevel);
            this.Name = "EquipmentPieceControl";
            this.Size = new System.Drawing.Size(153, 133);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtArmorLevel;
		private System.Windows.Forms.Label lblItemName;
		private System.Windows.Forms.TextBox txtSpell1;
		private System.Windows.Forms.TextBox txtSpell2;
		private System.Windows.Forms.TextBox txtSpell3;
		private System.Windows.Forms.TextBox txtArmorSet;
		private System.Windows.Forms.TextBox txtSpell4;
		private System.Windows.Forms.CheckBox chkLocked;
	}
}
