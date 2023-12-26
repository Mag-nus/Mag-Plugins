namespace Mag_SuitBuilder
{
	partial class EquipmentUpgradesForm
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			splitContainer1 = new System.Windows.Forms.SplitContainer();
			currentEquipmentGrid = new System.Windows.Forms.DataGridView();
			upgradeEquipmentGrid = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)currentEquipmentGrid).BeginInit();
			((System.ComponentModel.ISupportInitialize)upgradeEquipmentGrid).BeginInit();
			SuspendLayout();
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			splitContainer1.Location = new System.Drawing.Point(0, 0);
			splitContainer1.Name = "splitContainer1";
			splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(currentEquipmentGrid);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(upgradeEquipmentGrid);
			splitContainer1.Size = new System.Drawing.Size(889, 487);
			splitContainer1.SplitterDistance = 218;
			splitContainer1.TabIndex = 0;
			// 
			// currentEquipmentGrid
			// 
			currentEquipmentGrid.AllowUserToAddRows = false;
			currentEquipmentGrid.AllowUserToDeleteRows = false;
			currentEquipmentGrid.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(234, 234, 234);
			currentEquipmentGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			currentEquipmentGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			currentEquipmentGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			currentEquipmentGrid.Location = new System.Drawing.Point(0, 0);
			currentEquipmentGrid.Name = "currentEquipmentGrid";
			currentEquipmentGrid.Size = new System.Drawing.Size(889, 218);
			currentEquipmentGrid.TabIndex = 0;
			currentEquipmentGrid.CellPainting += currentEquipmentGrid_CellPainting;
			// 
			// upgradeEquipmentGrid
			// 
			upgradeEquipmentGrid.AllowUserToAddRows = false;
			upgradeEquipmentGrid.AllowUserToDeleteRows = false;
			upgradeEquipmentGrid.AllowUserToResizeRows = false;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(234, 234, 234);
			upgradeEquipmentGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
			upgradeEquipmentGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			upgradeEquipmentGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			upgradeEquipmentGrid.Location = new System.Drawing.Point(0, 0);
			upgradeEquipmentGrid.Name = "upgradeEquipmentGrid";
			upgradeEquipmentGrid.Size = new System.Drawing.Size(889, 265);
			upgradeEquipmentGrid.TabIndex = 0;
			upgradeEquipmentGrid.CellPainting += upgradeEquipmentGrid_CellPainting;
			// 
			// EquipmentUpgradesForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(889, 487);
			Controls.Add(splitContainer1);
			Name = "EquipmentUpgradesForm";
			Text = "Equipment Upgrades Form";
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)currentEquipmentGrid).EndInit();
			((System.ComponentModel.ISupportInitialize)upgradeEquipmentGrid).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridView currentEquipmentGrid;
		private System.Windows.Forms.DataGridView upgradeEquipmentGrid;
	}
}