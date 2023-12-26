namespace Mag_SuitBuilder.Spells
{
	partial class CantripSelectorControl
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			cmdLoadDefaults = new System.Windows.Forms.Button();
			defaultsComboBox = new System.Windows.Forms.ComboBox();
			cmdClear = new System.Windows.Forms.Button();
			dataGridView1 = new System.Windows.Forms.DataGridView();
			Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			lblLegendary = new System.Windows.Forms.Label();
			lblMinor = new System.Windows.Forms.Label();
			lblMajor = new System.Windows.Forms.Label();
			lblEpic = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
			SuspendLayout();
			// 
			// cmdLoadDefaults
			// 
			cmdLoadDefaults.Location = new System.Drawing.Point(3, 3);
			cmdLoadDefaults.Name = "cmdLoadDefaults";
			cmdLoadDefaults.Size = new System.Drawing.Size(112, 23);
			cmdLoadDefaults.TabIndex = 0;
			cmdLoadDefaults.Text = "Load Defaults For:";
			cmdLoadDefaults.UseVisualStyleBackColor = true;
			cmdLoadDefaults.Click += cmdLoadDefaults_Click;
			// 
			// defaultsComboBox
			// 
			defaultsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			defaultsComboBox.FormattingEnabled = true;
			defaultsComboBox.Items.AddRange(new object[] { "Finesse", "Generic", "Heavy", "Light", "Missile", "Tinker", "Two Hand", "Void", "War" });
			defaultsComboBox.Location = new System.Drawing.Point(121, 3);
			defaultsComboBox.Name = "defaultsComboBox";
			defaultsComboBox.Size = new System.Drawing.Size(154, 23);
			defaultsComboBox.TabIndex = 1;
			// 
			// cmdClear
			// 
			cmdClear.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			cmdClear.Location = new System.Drawing.Point(518, 3);
			cmdClear.Name = "cmdClear";
			cmdClear.Size = new System.Drawing.Size(64, 23);
			cmdClear.TabIndex = 2;
			cmdClear.Text = "Clear";
			cmdClear.UseVisualStyleBackColor = true;
			cmdClear.Click += cmdClear_Click;
			// 
			// dataGridView1
			// 
			dataGridView1.AllowUserToAddRows = false;
			dataGridView1.AllowUserToDeleteRows = false;
			dataGridView1.AllowUserToResizeColumns = false;
			dataGridView1.AllowUserToResizeRows = false;
			dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			dataGridView1.ColumnHeadersVisible = false;
			dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7 });
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
			dataGridView1.Location = new System.Drawing.Point(0, 33);
			dataGridView1.Margin = new System.Windows.Forms.Padding(0);
			dataGridView1.MultiSelect = false;
			dataGridView1.Name = "dataGridView1";
			dataGridView1.ReadOnly = true;
			dataGridView1.RowHeadersVisible = false;
			dataGridView1.RowTemplate.Height = 20;
			dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.None;
			dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			dataGridView1.Size = new System.Drawing.Size(585, 143);
			dataGridView1.TabIndex = 3;
			dataGridView1.CellClick += dataGridView1_CellClick;
			dataGridView1.CellDoubleClick += dataGridView1_CellClick;
			// 
			// Column1
			// 
			Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			Column1.HeaderText = "Column1";
			Column1.Name = "Column1";
			Column1.ReadOnly = true;
			Column1.Width = 5;
			// 
			// Column2
			// 
			Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			Column2.HeaderText = "Column2";
			Column2.Name = "Column2";
			Column2.ReadOnly = true;
			Column2.Width = 5;
			// 
			// Column3
			// 
			Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			Column3.HeaderText = "Column3";
			Column3.Name = "Column3";
			Column3.ReadOnly = true;
			Column3.Width = 5;
			// 
			// Column4
			// 
			Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			Column4.HeaderText = "Column4";
			Column4.Name = "Column4";
			Column4.ReadOnly = true;
			Column4.Width = 5;
			// 
			// Column5
			// 
			Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			Column5.HeaderText = "Column5";
			Column5.Name = "Column5";
			Column5.ReadOnly = true;
			Column5.Width = 5;
			// 
			// Column6
			// 
			Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			Column6.HeaderText = "Column6";
			Column6.Name = "Column6";
			Column6.ReadOnly = true;
			Column6.Width = 5;
			// 
			// Column7
			// 
			Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			Column7.HeaderText = "Column7";
			Column7.Name = "Column7";
			Column7.ReadOnly = true;
			Column7.Width = 5;
			// 
			// lblLegendary
			// 
			lblLegendary.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblLegendary.AutoSize = true;
			lblLegendary.BackColor = System.Drawing.Color.DarkOrange;
			lblLegendary.Location = new System.Drawing.Point(3, 179);
			lblLegendary.Name = "lblLegendary";
			lblLegendary.Size = new System.Drawing.Size(60, 15);
			lblLegendary.TabIndex = 4;
			lblLegendary.Text = "- Lgndry -";
			// 
			// lblMinor
			// 
			lblMinor.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblMinor.AutoSize = true;
			lblMinor.BackColor = System.Drawing.Color.LightBlue;
			lblMinor.Location = new System.Drawing.Point(200, 179);
			lblMinor.Name = "lblMinor";
			lblMinor.Size = new System.Drawing.Size(65, 15);
			lblMinor.TabIndex = 5;
			lblMinor.Text = "-- Minor --";
			// 
			// lblMajor
			// 
			lblMajor.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblMajor.AutoSize = true;
			lblMajor.BackColor = System.Drawing.Color.Pink;
			lblMajor.Location = new System.Drawing.Point(130, 179);
			lblMajor.Name = "lblMajor";
			lblMajor.Size = new System.Drawing.Size(64, 15);
			lblMajor.TabIndex = 6;
			lblMajor.Text = "-- Major --";
			// 
			// lblEpic
			// 
			lblEpic.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblEpic.AutoSize = true;
			lblEpic.BackColor = System.Drawing.Color.LightGreen;
			lblEpic.Location = new System.Drawing.Point(69, 179);
			lblEpic.Name = "lblEpic";
			lblEpic.Size = new System.Drawing.Size(55, 15);
			lblEpic.TabIndex = 7;
			lblEpic.Text = "-- Epic --";
			// 
			// CantripSelectorControl
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(lblEpic);
			Controls.Add(lblMajor);
			Controls.Add(lblMinor);
			Controls.Add(lblLegendary);
			Controls.Add(dataGridView1);
			Controls.Add(cmdClear);
			Controls.Add(defaultsComboBox);
			Controls.Add(cmdLoadDefaults);
			Name = "CantripSelectorControl";
			Size = new System.Drawing.Size(585, 197);
			((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button cmdLoadDefaults;
		private System.Windows.Forms.ComboBox defaultsComboBox;
		private System.Windows.Forms.Button cmdClear;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
		private System.Windows.Forms.Label lblLegendary;
		private System.Windows.Forms.Label lblMinor;
		private System.Windows.Forms.Label lblMajor;
		private System.Windows.Forms.Label lblEpic;
	}
}
