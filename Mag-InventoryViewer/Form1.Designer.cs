namespace Mag_InventoryViewer
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
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.cmdLoadInventory = new System.Windows.Forms.Button();
			this.txtSearchDirectory = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.treeView1.CheckBoxes = true;
			this.treeView1.Location = new System.Drawing.Point(0, 41);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(178, 621);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
			// 
			// dataGridView1
			// 
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(179, 41);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.Size = new System.Drawing.Size(805, 621);
			this.dataGridView1.TabIndex = 1;
			// 
			// cmdLoadInventory
			// 
			this.cmdLoadInventory.Location = new System.Drawing.Point(12, 12);
			this.cmdLoadInventory.Name = "cmdLoadInventory";
			this.cmdLoadInventory.Size = new System.Drawing.Size(95, 23);
			this.cmdLoadInventory.TabIndex = 2;
			this.cmdLoadInventory.Text = "Load Inventory";
			this.cmdLoadInventory.UseVisualStyleBackColor = true;
			this.cmdLoadInventory.Click += new System.EventHandler(this.cmdLoadInventory_Click);
			// 
			// txtSearchDirectory
			// 
			this.txtSearchDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSearchDirectory.Location = new System.Drawing.Point(113, 15);
			this.txtSearchDirectory.Name = "txtSearchDirectory";
			this.txtSearchDirectory.Size = new System.Drawing.Size(859, 20);
			this.txtSearchDirectory.TabIndex = 3;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(984, 662);
			this.Controls.Add(this.txtSearchDirectory);
			this.Controls.Add(this.cmdLoadInventory);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.treeView1);
			this.Name = "Form1";
			this.Text = "Mag-InventoryViewer";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Button cmdLoadInventory;
		private System.Windows.Forms.TextBox txtSearchDirectory;
	}
}

