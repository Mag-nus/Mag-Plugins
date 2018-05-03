namespace Mag_LootParser
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
            this.lblTime = new System.Windows.Forms.Label();
            this.cmdStop = new System.Windows.Forms.Button();
            this.lblResults = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdProcessAllFiles = new System.Windows.Forms.Button();
            this.cmdBrowseForDifferentSource = new System.Windows.Forms.Button();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtRawOutput1 = new System.Windows.Forms.RichTextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtRawOutput2 = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(784, 961);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblTime);
            this.tabPage1.Controls.Add(this.cmdStop);
            this.tabPage1.Controls.Add(this.lblResults);
            this.tabPage1.Controls.Add(this.progressBar1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.cmdProcessAllFiles);
            this.tabPage1.Controls.Add(this.cmdBrowseForDifferentSource);
            this.tabPage1.Controls.Add(this.txtSourcePath);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 424);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Read Log Files";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(62, 159);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(40, 13);
            this.lblTime.TabIndex = 11;
            this.lblTime.Text = "lblTime";
            // 
            // cmdStop
            // 
            this.cmdStop.Enabled = false;
            this.cmdStop.Location = new System.Drawing.Point(392, 35);
            this.cmdStop.Name = "cmdStop";
            this.cmdStop.Size = new System.Drawing.Size(111, 23);
            this.cmdStop.TabIndex = 10;
            this.cmdStop.Text = "Stop";
            this.cmdStop.UseVisualStyleBackColor = true;
            this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Location = new System.Drawing.Point(62, 137);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(52, 13);
            this.lblResults.TabIndex = 8;
            this.lblResults.Text = "lblResults";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(65, 102);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(321, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Progress";
            // 
            // cmdProcessAllFiles
            // 
            this.cmdProcessAllFiles.Location = new System.Drawing.Point(275, 35);
            this.cmdProcessAllFiles.Name = "cmdProcessAllFiles";
            this.cmdProcessAllFiles.Size = new System.Drawing.Size(111, 23);
            this.cmdProcessAllFiles.TabIndex = 3;
            this.cmdProcessAllFiles.Text = "Process All Files";
            this.cmdProcessAllFiles.UseVisualStyleBackColor = true;
            this.cmdProcessAllFiles.Click += new System.EventHandler(this.cmdProcessAllFiles_Click);
            // 
            // cmdBrowseForDifferentSource
            // 
            this.cmdBrowseForDifferentSource.Location = new System.Drawing.Point(55, 35);
            this.cmdBrowseForDifferentSource.Name = "cmdBrowseForDifferentSource";
            this.cmdBrowseForDifferentSource.Size = new System.Drawing.Size(174, 23);
            this.cmdBrowseForDifferentSource.TabIndex = 2;
            this.cmdBrowseForDifferentSource.Text = "Browse For Different Source";
            this.cmdBrowseForDifferentSource.UseVisualStyleBackColor = true;
            this.cmdBrowseForDifferentSource.Click += new System.EventHandler(this.cmdBrowseForDifferentSource_Click);
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourcePath.Location = new System.Drawing.Point(55, 9);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.Size = new System.Drawing.Size(729, 20);
            this.txtSourcePath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 424);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Containers";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(786, 418);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtRawOutput1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(792, 424);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Raw Output 1";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtRawOutput1
            // 
            this.txtRawOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRawOutput1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRawOutput1.Location = new System.Drawing.Point(3, 3);
            this.txtRawOutput1.Name = "txtRawOutput1";
            this.txtRawOutput1.Size = new System.Drawing.Size(786, 418);
            this.txtRawOutput1.TabIndex = 0;
            this.txtRawOutput1.Text = "";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtRawOutput2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(776, 935);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Raw Output 2";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtRawOutput2
            // 
            this.txtRawOutput2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRawOutput2.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRawOutput2.Location = new System.Drawing.Point(0, 0);
            this.txtRawOutput2.Name = "txtRawOutput2";
            this.txtRawOutput2.Size = new System.Drawing.Size(776, 935);
            this.txtRawOutput2.TabIndex = 0;
            this.txtRawOutput2.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 961);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Mag-LootParser";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdProcessAllFiles;
        private System.Windows.Forms.Button cmdBrowseForDifferentSource;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button cmdStop;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.RichTextBox txtRawOutput1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.RichTextBox txtRawOutput2;
    }
}

