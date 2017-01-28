namespace Mag_WorldObjectParser
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
			this.lblWorkingFile = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.lblProgress = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmdReadAllFiles = new System.Windows.Forms.Button();
			this.cmdBrowseForDifferentSource = new System.Windows.Forms.Button();
			this.txtSourcePath = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.lblResults = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(675, 551);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.lblResults);
			this.tabPage1.Controls.Add(this.lblWorkingFile);
			this.tabPage1.Controls.Add(this.progressBar1);
			this.tabPage1.Controls.Add(this.lblProgress);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.cmdReadAllFiles);
			this.tabPage1.Controls.Add(this.cmdBrowseForDifferentSource);
			this.tabPage1.Controls.Add(this.txtSourcePath);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(667, 525);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Read Log Files";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// lblWorkingFile
			// 
			this.lblWorkingFile.AutoSize = true;
			this.lblWorkingFile.Location = new System.Drawing.Point(62, 124);
			this.lblWorkingFile.Name = "lblWorkingFile";
			this.lblWorkingFile.Size = new System.Drawing.Size(73, 13);
			this.lblWorkingFile.TabIndex = 7;
			this.lblWorkingFile.Text = "lblWorkingFile";
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(65, 149);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(321, 23);
			this.progressBar1.Step = 1;
			this.progressBar1.TabIndex = 6;
			// 
			// lblProgress
			// 
			this.lblProgress.AutoSize = true;
			this.lblProgress.Location = new System.Drawing.Point(62, 102);
			this.lblProgress.Name = "lblProgress";
			this.lblProgress.Size = new System.Drawing.Size(58, 13);
			this.lblProgress.TabIndex = 5;
			this.lblProgress.Text = "lblProgress";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 102);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Progress";
			// 
			// cmdReadAllFiles
			// 
			this.cmdReadAllFiles.Location = new System.Drawing.Point(275, 35);
			this.cmdReadAllFiles.Name = "cmdReadAllFiles";
			this.cmdReadAllFiles.Size = new System.Drawing.Size(111, 23);
			this.cmdReadAllFiles.TabIndex = 3;
			this.cmdReadAllFiles.Text = "Read All Files";
			this.cmdReadAllFiles.UseVisualStyleBackColor = true;
			this.cmdReadAllFiles.Click += new System.EventHandler(this.cmdReadAllFiles_Click);
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
			this.txtSourcePath.Size = new System.Drawing.Size(604, 20);
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
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(667, 525);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// lblResults
			// 
			this.lblResults.AutoSize = true;
			this.lblResults.Location = new System.Drawing.Point(62, 184);
			this.lblResults.Name = "lblResults";
			this.lblResults.Size = new System.Drawing.Size(52, 13);
			this.lblResults.TabIndex = 8;
			this.lblResults.Text = "lblResults";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(675, 551);
			this.Controls.Add(this.tabControl1);
			this.Name = "Form1";
			this.Text = "Mag-WordObjectParser";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label lblProgress;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button cmdReadAllFiles;
		private System.Windows.Forms.Button cmdBrowseForDifferentSource;
		private System.Windows.Forms.TextBox txtSourcePath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Label lblWorkingFile;
		private System.Windows.Forms.Label lblResults;
	}
}

