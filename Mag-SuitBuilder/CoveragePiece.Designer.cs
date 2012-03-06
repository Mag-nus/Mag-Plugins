namespace Mag_SuitBuilder
{
	partial class CoveragePiece
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
			this.chkLocked = new System.Windows.Forms.CheckBox();
			this.cbSpell1 = new System.Windows.Forms.ComboBox();
			this.cbSpell2 = new System.Windows.Forms.ComboBox();
			this.cbSpell3 = new System.Windows.Forms.ComboBox();
			this.txtArmorLevel = new System.Windows.Forms.TextBox();
			this.lblItemName = new System.Windows.Forms.Label();
			this.cbArmorSet = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// chkLocked
			// 
			this.chkLocked.AutoSize = true;
			this.chkLocked.Location = new System.Drawing.Point(0, 0);
			this.chkLocked.Name = "chkLocked";
			this.chkLocked.Size = new System.Drawing.Size(62, 17);
			this.chkLocked.TabIndex = 0;
			this.chkLocked.Text = "Locked";
			this.chkLocked.UseVisualStyleBackColor = true;
			// 
			// cbSpell1
			// 
			this.cbSpell1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbSpell1.FormattingEnabled = true;
			this.cbSpell1.Location = new System.Drawing.Point(0, 64);
			this.cbSpell1.Name = "cbSpell1";
			this.cbSpell1.Size = new System.Drawing.Size(154, 21);
			this.cbSpell1.TabIndex = 1;
			// 
			// cbSpell2
			// 
			this.cbSpell2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbSpell2.FormattingEnabled = true;
			this.cbSpell2.Location = new System.Drawing.Point(0, 88);
			this.cbSpell2.Name = "cbSpell2";
			this.cbSpell2.Size = new System.Drawing.Size(154, 21);
			this.cbSpell2.TabIndex = 2;
			// 
			// cbSpell3
			// 
			this.cbSpell3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbSpell3.FormattingEnabled = true;
			this.cbSpell3.Location = new System.Drawing.Point(0, 112);
			this.cbSpell3.Name = "cbSpell3";
			this.cbSpell3.Size = new System.Drawing.Size(154, 21);
			this.cbSpell3.TabIndex = 3;
			// 
			// txtArmorLevel
			// 
			this.txtArmorLevel.Location = new System.Drawing.Point(0, 40);
			this.txtArmorLevel.Name = "txtArmorLevel";
			this.txtArmorLevel.ReadOnly = true;
			this.txtArmorLevel.Size = new System.Drawing.Size(32, 20);
			this.txtArmorLevel.TabIndex = 4;
			// 
			// lblItemName
			// 
			this.lblItemName.AutoSize = true;
			this.lblItemName.Location = new System.Drawing.Point(0, 21);
			this.lblItemName.Name = "lblItemName";
			this.lblItemName.Size = new System.Drawing.Size(58, 13);
			this.lblItemName.TabIndex = 6;
			this.lblItemName.Text = "Item Name";
			// 
			// cbArmorSet
			// 
			this.cbArmorSet.FormattingEnabled = true;
			this.cbArmorSet.Location = new System.Drawing.Point(40, 40);
			this.cbArmorSet.Name = "cbArmorSet";
			this.cbArmorSet.Size = new System.Drawing.Size(114, 21);
			this.cbArmorSet.TabIndex = 7;
			// 
			// CoveragePiece
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.cbArmorSet);
			this.Controls.Add(this.lblItemName);
			this.Controls.Add(this.txtArmorLevel);
			this.Controls.Add(this.cbSpell3);
			this.Controls.Add(this.cbSpell2);
			this.Controls.Add(this.cbSpell1);
			this.Controls.Add(this.chkLocked);
			this.Name = "CoveragePiece";
			this.Size = new System.Drawing.Size(155, 135);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkLocked;
		private System.Windows.Forms.ComboBox cbSpell1;
		private System.Windows.Forms.ComboBox cbSpell2;
		private System.Windows.Forms.ComboBox cbSpell3;
		private System.Windows.Forms.TextBox txtArmorLevel;
		private System.Windows.Forms.Label lblItemName;
		private System.Windows.Forms.ComboBox cbArmorSet;
	}
}
