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
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lblEpic = new System.Windows.Forms.Label();
			this.lblMajor = new System.Windows.Forms.Label();
			this.lblMinor = new System.Windows.Forms.Label();
			this.lblLegendary = new System.Windows.Forms.Label();
			this.defaultsComboBox = new System.Windows.Forms.ComboBox();
			this.cmdLoadDefaults = new System.Windows.Forms.Button();
			this.cmdClear = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToResizeColumns = false;
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.ColumnHeadersVisible = false;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7});
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
			this.dataGridView1.Location = new System.Drawing.Point(0, 31);
			this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
			this.dataGridView1.MultiSelect = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.RowTemplate.Height = 18;
			this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dataGridView1.Size = new System.Drawing.Size(528, 128);
			this.dataGridView1.TabIndex = 24;
			this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
			this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
			// 
			// Column1
			// 
			this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.Column1.HeaderText = "Column1";
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			this.Column1.Width = 5;
			// 
			// Column2
			// 
			this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.Column2.HeaderText = "Column2";
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			this.Column2.Width = 5;
			// 
			// Column3
			// 
			this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.Column3.HeaderText = "Column3";
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			this.Column3.Width = 5;
			// 
			// Column4
			// 
			this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.Column4.HeaderText = "Column4";
			this.Column4.Name = "Column4";
			this.Column4.ReadOnly = true;
			this.Column4.Width = 5;
			// 
			// Column5
			// 
			this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.Column5.HeaderText = "Column5";
			this.Column5.Name = "Column5";
			this.Column5.ReadOnly = true;
			this.Column5.Width = 5;
			// 
			// Column6
			// 
			this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.Column6.HeaderText = "Column6";
			this.Column6.Name = "Column6";
			this.Column6.ReadOnly = true;
			this.Column6.Width = 5;
			// 
			// Column7
			// 
			this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.Column7.HeaderText = "Column7";
			this.Column7.Name = "Column7";
			this.Column7.ReadOnly = true;
			this.Column7.Width = 5;
			// 
			// lblEpic
			// 
			this.lblEpic.AutoSize = true;
			this.lblEpic.BackColor = System.Drawing.Color.LightGreen;
			this.lblEpic.Location = new System.Drawing.Point(59, 163);
			this.lblEpic.Name = "lblEpic";
			this.lblEpic.Size = new System.Drawing.Size(46, 13);
			this.lblEpic.TabIndex = 25;
			this.lblEpic.Text = "-- Epic --";
			// 
			// lblMajor
			// 
			this.lblMajor.AutoSize = true;
			this.lblMajor.BackColor = System.Drawing.Color.Pink;
			this.lblMajor.Location = new System.Drawing.Point(111, 163);
			this.lblMajor.Name = "lblMajor";
			this.lblMajor.Size = new System.Drawing.Size(51, 13);
			this.lblMajor.TabIndex = 26;
			this.lblMajor.Text = "-- Major --";
			// 
			// lblMinor
			// 
			this.lblMinor.AutoSize = true;
			this.lblMinor.BackColor = System.Drawing.Color.LightBlue;
			this.lblMinor.Location = new System.Drawing.Point(168, 163);
			this.lblMinor.Name = "lblMinor";
			this.lblMinor.Size = new System.Drawing.Size(51, 13);
			this.lblMinor.TabIndex = 27;
			this.lblMinor.Text = "-- Minor --";
			// 
			// lblLegendary
			// 
			this.lblLegendary.AutoSize = true;
			this.lblLegendary.BackColor = System.Drawing.Color.DarkOrange;
			this.lblLegendary.Location = new System.Drawing.Point(3, 163);
			this.lblLegendary.Name = "lblLegendary";
			this.lblLegendary.Size = new System.Drawing.Size(51, 13);
			this.lblLegendary.TabIndex = 28;
			this.lblLegendary.Text = "- Lgndry -";
			// 
			// defaultsComboBox
			// 
			this.defaultsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.defaultsComboBox.FormattingEnabled = true;
			this.defaultsComboBox.Items.AddRange(new object[] {
            "War",
            "Void",
            "Heavy",
            "Light",
            "Finesse",
            "Missile",
            "Two Hand",
            "Tinker"});
			this.defaultsComboBox.Location = new System.Drawing.Point(114, 5);
			this.defaultsComboBox.Name = "defaultsComboBox";
			this.defaultsComboBox.Size = new System.Drawing.Size(154, 21);
			this.defaultsComboBox.TabIndex = 29;
			// 
			// cmdLoadDefaults
			// 
			this.cmdLoadDefaults.Location = new System.Drawing.Point(0, 3);
			this.cmdLoadDefaults.Name = "cmdLoadDefaults";
			this.cmdLoadDefaults.Size = new System.Drawing.Size(105, 23);
			this.cmdLoadDefaults.TabIndex = 30;
			this.cmdLoadDefaults.Text = "Load Defautls For:";
			this.cmdLoadDefaults.UseVisualStyleBackColor = true;
			this.cmdLoadDefaults.Click += new System.EventHandler(this.cmdLoadDefaults_Click);
			// 
			// cmdClear
			// 
			this.cmdClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdClear.Location = new System.Drawing.Point(461, 5);
			this.cmdClear.Name = "cmdClear";
			this.cmdClear.Size = new System.Drawing.Size(64, 23);
			this.cmdClear.TabIndex = 31;
			this.cmdClear.Text = "Clear";
			this.cmdClear.UseVisualStyleBackColor = true;
			this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
			// 
			// CantripSelectorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.cmdClear);
			this.Controls.Add(this.cmdLoadDefaults);
			this.Controls.Add(this.defaultsComboBox);
			this.Controls.Add(this.lblLegendary);
			this.Controls.Add(this.lblMinor);
			this.Controls.Add(this.lblMajor);
			this.Controls.Add(this.lblEpic);
			this.Controls.Add(this.dataGridView1);
			this.Name = "CantripSelectorControl";
			this.Size = new System.Drawing.Size(528, 181);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
		private System.Windows.Forms.Label lblEpic;
		private System.Windows.Forms.Label lblMajor;
		private System.Windows.Forms.Label lblMinor;
		private System.Windows.Forms.Label lblLegendary;
		private System.Windows.Forms.ComboBox defaultsComboBox;
		private System.Windows.Forms.Button cmdLoadDefaults;
		private System.Windows.Forms.Button cmdClear;
	}
}
