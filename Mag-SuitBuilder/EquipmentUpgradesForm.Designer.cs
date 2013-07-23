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
			this.currentEquipmentGrid = new System.Windows.Forms.DataGridView();
			this.upgradeEquipmentGrid = new System.Windows.Forms.DataGridView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			((System.ComponentModel.ISupportInitialize)(this.currentEquipmentGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.upgradeEquipmentGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// currentEquipmentGrid
			// 
			this.currentEquipmentGrid.AllowUserToAddRows = false;
			this.currentEquipmentGrid.AllowUserToDeleteRows = false;
			this.currentEquipmentGrid.AllowUserToOrderColumns = true;
			this.currentEquipmentGrid.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
			this.currentEquipmentGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.currentEquipmentGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.currentEquipmentGrid.Location = new System.Drawing.Point(0, 0);
			this.currentEquipmentGrid.Name = "currentEquipmentGrid";
			this.currentEquipmentGrid.Size = new System.Drawing.Size(889, 218);
			this.currentEquipmentGrid.TabIndex = 29;
			this.currentEquipmentGrid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.currentEquipmentGrid_CellPainting);
			// 
			// upgradeEquipmentGrid
			// 
			this.upgradeEquipmentGrid.AllowUserToAddRows = false;
			this.upgradeEquipmentGrid.AllowUserToDeleteRows = false;
			this.upgradeEquipmentGrid.AllowUserToOrderColumns = true;
			this.upgradeEquipmentGrid.AllowUserToResizeRows = false;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
			this.upgradeEquipmentGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
			this.upgradeEquipmentGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.upgradeEquipmentGrid.Location = new System.Drawing.Point(0, 0);
			this.upgradeEquipmentGrid.Name = "upgradeEquipmentGrid";
			this.upgradeEquipmentGrid.Size = new System.Drawing.Size(889, 264);
			this.upgradeEquipmentGrid.TabIndex = 30;
			this.upgradeEquipmentGrid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.upgradeEquipmentGrid_CellPainting);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.currentEquipmentGrid);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.upgradeEquipmentGrid);
			this.splitContainer1.Size = new System.Drawing.Size(889, 487);
			this.splitContainer1.SplitterDistance = 218;
			this.splitContainer1.SplitterWidth = 5;
			this.splitContainer1.TabIndex = 31;
			// 
			// EquipmentUpgradesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(889, 487);
			this.Controls.Add(this.splitContainer1);
			this.Name = "EquipmentUpgradesForm";
			this.Text = "Equipment Upgrades Form";
			((System.ComponentModel.ISupportInitialize)(this.currentEquipmentGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.upgradeEquipmentGrid)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView currentEquipmentGrid;
		private System.Windows.Forms.DataGridView upgradeEquipmentGrid;
		private System.Windows.Forms.SplitContainer splitContainer1;
	}
}