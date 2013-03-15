///////////////////////////////////////////////////////////////////////////////
//File: Form1.Designer.cs
//
//Description: The Virindi Tank Loot Editor.
//
//This file is Copyright (c) 2008 VirindiPlugins
//
//The original copy of this code can be obtained from http://www.virindi.net/repos/virindi_public
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////

using System.Drawing;
using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace VTClassic
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addSalvageRulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.increaseSalvageWorkmanshipsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoSortRulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addPackslotRulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.combineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportRangesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.importRangesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabLootRules = new System.Windows.Forms.TabPage();
			this.button2 = new System.Windows.Forms.Button();
			this.cmdCloneRule = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtMastery = new System.Windows.Forms.TextBox();
			this.cmbMastery = new System.Windows.Forms.ComboBox();
			this.txtSet = new System.Windows.Forms.TextBox();
			this.cmbSet = new System.Windows.Forms.ComboBox();
			this.txtSkill = new System.Windows.Forms.TextBox();
			this.cmbSkill = new System.Windows.Forms.ComboBox();
			this.txtMaterial = new System.Windows.Forms.TextBox();
			this.cmbMaterial = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.cmdDeleteRule = new System.Windows.Forms.Button();
			this.groupRule = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.txtKeepCount = new System.Windows.Forms.TextBox();
			this.cmdCloneReq = new System.Windows.Forms.Button();
			this.groupReqs = new System.Windows.Forms.GroupBox();
			this.lblValue4 = new System.Windows.Forms.Label();
			this.txtValue4 = new System.Windows.Forms.TextBox();
			this.txtValue3 = new System.Windows.Forms.TextBox();
			this.lblValue3 = new System.Windows.Forms.Label();
			this.lblValue2 = new System.Windows.Forms.Label();
			this.txtValue2 = new System.Windows.Forms.TextBox();
			this.txtValue = new System.Windows.Forms.TextBox();
			this.lblValue = new System.Windows.Forms.Label();
			this.cmbKey = new System.Windows.Forms.ComboBox();
			this.lblKey = new System.Windows.Forms.Label();
			this.cmbActsOn = new System.Windows.Forms.ComboBox();
			this.lblActsOn = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbReqType = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbAction = new System.Windows.Forms.ComboBox();
			this.txtRuleName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lstRequirements = new System.Windows.Forms.ListBox();
			this.cmdDeleteReq = new System.Windows.Forms.Button();
			this.cmdNewReq = new System.Windows.Forms.Button();
			this.cmdNewRule = new System.Windows.Forms.Button();
			this.lstRules = new System.Windows.Forms.ListBox();
			this.tabSalvageCombine = new System.Windows.Forms.TabPage();
			this.tSC_lblCombineRules = new System.Windows.Forms.Label();
			this.tSC_groupCRS = new System.Windows.Forms.GroupBox();
			this.tSC_txtCombineRange = new System.Windows.Forms.TextBox();
			this.tSC_lblCombineRange = new System.Windows.Forms.Label();
			this.tSC_cmbMaterial = new System.Windows.Forms.ComboBox();
			this.tSC_lblMaterial = new System.Windows.Forms.Label();
			this.tSC_btnDelete = new System.Windows.Forms.Button();
			this.tSC_btnNew = new System.Windows.Forms.Button();
			this.tSC_listCombine = new System.Windows.Forms.ListBox();
			this.tSC_lblDefaultCombine = new System.Windows.Forms.Label();
			this.tSC_txtDefaultCombine = new System.Windows.Forms.TextBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.menuStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabLootRules.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupRule.SuspendLayout();
			this.groupReqs.SuspendLayout();
			this.tabSalvageCombine.SuspendLayout();
			this.tSC_groupCRS.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.combineToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(652, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
			this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.newToolStripMenuItem.Text = "&New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
			this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
			this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.saveAsToolStripMenuItem.Text = "Save &As";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSalvageRulesToolStripMenuItem,
            this.increaseSalvageWorkmanshipsToolStripMenuItem,
            this.autoSortRulesToolStripMenuItem,
            this.addPackslotRulesToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
			this.editToolStripMenuItem.Text = "Rule";
			// 
			// addSalvageRulesToolStripMenuItem
			// 
			this.addSalvageRulesToolStripMenuItem.Name = "addSalvageRulesToolStripMenuItem";
			this.addSalvageRulesToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
			this.addSalvageRulesToolStripMenuItem.Text = "Add/Update Salvage Rules";
			this.addSalvageRulesToolStripMenuItem.Click += new System.EventHandler(this.addSalvageRulesToolStripMenuItem_Click);
			// 
			// increaseSalvageWorkmanshipsToolStripMenuItem
			// 
			this.increaseSalvageWorkmanshipsToolStripMenuItem.Name = "increaseSalvageWorkmanshipsToolStripMenuItem";
			this.increaseSalvageWorkmanshipsToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
			this.increaseSalvageWorkmanshipsToolStripMenuItem.Text = "Update Workmanship Requirements";
			this.increaseSalvageWorkmanshipsToolStripMenuItem.Click += new System.EventHandler(this.increaseSalvageWorkmanshipsToolStripMenuItem_Click);
			// 
			// autoSortRulesToolStripMenuItem
			// 
			this.autoSortRulesToolStripMenuItem.Name = "autoSortRulesToolStripMenuItem";
			this.autoSortRulesToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
			this.autoSortRulesToolStripMenuItem.Text = "Auto-Sort Rules";
			this.autoSortRulesToolStripMenuItem.Click += new System.EventHandler(this.autoSortRulesToolStripMenuItem_Click);
			// 
			// addPackslotRulesToolStripMenuItem
			// 
			this.addPackslotRulesToolStripMenuItem.Name = "addPackslotRulesToolStripMenuItem";
			this.addPackslotRulesToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
			this.addPackslotRulesToolStripMenuItem.Text = "Add Packslot Requirements";
			this.addPackslotRulesToolStripMenuItem.Click += new System.EventHandler(this.addPackslotRulesToolStripMenuItem_Click);
			// 
			// combineToolStripMenuItem
			// 
			this.combineToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportRangesToolStripMenuItem,
            this.importRangesToolStripMenuItem});
			this.combineToolStripMenuItem.Name = "combineToolStripMenuItem";
			this.combineToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
			this.combineToolStripMenuItem.Text = "Combine";
			// 
			// exportRangesToolStripMenuItem
			// 
			this.exportRangesToolStripMenuItem.Name = "exportRangesToolStripMenuItem";
			this.exportRangesToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.exportRangesToolStripMenuItem.Text = "Export Ranges";
			this.exportRangesToolStripMenuItem.Click += new System.EventHandler(this.exportRangesToolStripMenuItem_Click);
			// 
			// importRangesToolStripMenuItem
			// 
			this.importRangesToolStripMenuItem.Name = "importRangesToolStripMenuItem";
			this.importRangesToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.importRangesToolStripMenuItem.Text = "Import Ranges";
			this.importRangesToolStripMenuItem.Click += new System.EventHandler(this.importRangesToolStripMenuItem_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabLootRules);
			this.tabControl1.Controls.Add(this.tabSalvageCombine);
			this.tabControl1.Location = new System.Drawing.Point(0, 50);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(653, 473);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabControl1_KeyDown);
			this.tabControl1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tabControl1_KeyUp);
			// 
			// tabLootRules
			// 
			this.tabLootRules.Controls.Add(this.button2);
			this.tabLootRules.Controls.Add(this.cmdCloneRule);
			this.tabLootRules.Controls.Add(this.groupBox1);
			this.tabLootRules.Controls.Add(this.button1);
			this.tabLootRules.Controls.Add(this.cmdDeleteRule);
			this.tabLootRules.Controls.Add(this.groupRule);
			this.tabLootRules.Controls.Add(this.cmdNewRule);
			this.tabLootRules.Controls.Add(this.lstRules);
			this.tabLootRules.Location = new System.Drawing.Point(4, 22);
			this.tabLootRules.Name = "tabLootRules";
			this.tabLootRules.Padding = new System.Windows.Forms.Padding(3);
			this.tabLootRules.Size = new System.Drawing.Size(645, 447);
			this.tabLootRules.TabIndex = 0;
			this.tabLootRules.Text = "Loot Rules";
			this.tabLootRules.UseVisualStyleBackColor = true;
			this.tabLootRules.Click += new System.EventHandler(this.tabLootRules_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(92, 6);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(76, 20);
			this.button2.TabIndex = 16;
			this.button2.Text = "Move Down";
			this.toolTip1.SetToolTip(this.button2, "Hold the control key down while clicking Move Down to move a rule down by 3x.");
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// cmdCloneRule
			// 
			this.cmdCloneRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdCloneRule.Location = new System.Drawing.Point(54, 341);
			this.cmdCloneRule.Name = "cmdCloneRule";
			this.cmdCloneRule.Size = new System.Drawing.Size(54, 23);
			this.cmdCloneRule.TabIndex = 15;
			this.cmdCloneRule.Text = "Clone";
			this.toolTip1.SetToolTip(this.cmdCloneRule, "Hold the control key down while clicking Clone to insert the cloned rule after th" +
        "e current selection.");
			this.cmdCloneRule.UseVisualStyleBackColor = true;
			this.cmdCloneRule.Click += new System.EventHandler(this.cmdCloneRule_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.txtMastery);
			this.groupBox1.Controls.Add(this.cmbMastery);
			this.groupBox1.Controls.Add(this.txtSet);
			this.groupBox1.Controls.Add(this.cmbSet);
			this.groupBox1.Controls.Add(this.txtSkill);
			this.groupBox1.Controls.Add(this.cmbSkill);
			this.groupBox1.Controls.Add(this.txtMaterial);
			this.groupBox1.Controls.Add(this.cmbMaterial);
			this.groupBox1.Location = new System.Drawing.Point(9, 371);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(558, 70);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Info";
			// 
			// txtMastery
			// 
			this.txtMastery.Location = new System.Drawing.Point(334, 41);
			this.txtMastery.Name = "txtMastery";
			this.txtMastery.Size = new System.Drawing.Size(30, 20);
			this.txtMastery.TabIndex = 7;
			// 
			// cmbMastery
			// 
			this.cmbMastery.FormattingEnabled = true;
			this.cmbMastery.Location = new System.Drawing.Point(188, 40);
			this.cmbMastery.Name = "cmbMastery";
			this.cmbMastery.Size = new System.Drawing.Size(140, 21);
			this.cmbMastery.TabIndex = 6;
			// 
			// txtSet
			// 
			this.txtSet.Location = new System.Drawing.Point(517, 15);
			this.txtSet.Name = "txtSet";
			this.txtSet.Size = new System.Drawing.Size(30, 20);
			this.txtSet.TabIndex = 5;
			// 
			// cmbSet
			// 
			this.cmbSet.FormattingEnabled = true;
			this.cmbSet.Location = new System.Drawing.Point(371, 15);
			this.cmbSet.Name = "cmbSet";
			this.cmbSet.Size = new System.Drawing.Size(140, 21);
			this.cmbSet.TabIndex = 4;
			// 
			// txtSkill
			// 
			this.txtSkill.Location = new System.Drawing.Point(335, 15);
			this.txtSkill.Name = "txtSkill";
			this.txtSkill.Size = new System.Drawing.Size(30, 20);
			this.txtSkill.TabIndex = 3;
			// 
			// cmbSkill
			// 
			this.cmbSkill.FormattingEnabled = true;
			this.cmbSkill.Location = new System.Drawing.Point(189, 14);
			this.cmbSkill.Name = "cmbSkill";
			this.cmbSkill.Size = new System.Drawing.Size(140, 21);
			this.cmbSkill.TabIndex = 2;
			// 
			// txtMaterial
			// 
			this.txtMaterial.Location = new System.Drawing.Point(153, 15);
			this.txtMaterial.Name = "txtMaterial";
			this.txtMaterial.Size = new System.Drawing.Size(30, 20);
			this.txtMaterial.TabIndex = 1;
			// 
			// cmbMaterial
			// 
			this.cmbMaterial.FormattingEnabled = true;
			this.cmbMaterial.Location = new System.Drawing.Point(7, 15);
			this.cmbMaterial.Name = "cmbMaterial";
			this.cmbMaterial.Size = new System.Drawing.Size(140, 21);
			this.cmbMaterial.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
			this.button1.Location = new System.Drawing.Point(8, 6);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(78, 20);
			this.button1.TabIndex = 13;
			this.button1.Text = "Move Up";
			this.toolTip1.SetToolTip(this.button1, "Hold the control key down while clicking Move Up to move a rule up by 3x.");
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// cmdDeleteRule
			// 
			this.cmdDeleteRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdDeleteRule.Location = new System.Drawing.Point(114, 341);
			this.cmdDeleteRule.Name = "cmdDeleteRule";
			this.cmdDeleteRule.Size = new System.Drawing.Size(54, 23);
			this.cmdDeleteRule.TabIndex = 12;
			this.cmdDeleteRule.Text = "Delete";
			this.cmdDeleteRule.UseVisualStyleBackColor = true;
			this.cmdDeleteRule.Click += new System.EventHandler(this.cmdDeleteRule_Click);
			// 
			// groupRule
			// 
			this.groupRule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupRule.Controls.Add(this.label4);
			this.groupRule.Controls.Add(this.button3);
			this.groupRule.Controls.Add(this.txtKeepCount);
			this.groupRule.Controls.Add(this.cmdCloneReq);
			this.groupRule.Controls.Add(this.groupReqs);
			this.groupRule.Controls.Add(this.label2);
			this.groupRule.Controls.Add(this.cmbAction);
			this.groupRule.Controls.Add(this.txtRuleName);
			this.groupRule.Controls.Add(this.label1);
			this.groupRule.Controls.Add(this.lstRequirements);
			this.groupRule.Controls.Add(this.cmdDeleteReq);
			this.groupRule.Controls.Add(this.cmdNewReq);
			this.groupRule.Location = new System.Drawing.Point(174, 6);
			this.groupRule.Name = "groupRule";
			this.groupRule.Size = new System.Drawing.Size(463, 358);
			this.groupRule.TabIndex = 11;
			this.groupRule.TabStop = false;
			this.groupRule.Text = "Rule";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(131, 76);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 29);
			this.label4.TabIndex = 9;
			this.label4.Text = "              ";
			this.label4.Visible = false;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(9, 76);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(104, 29);
			this.button3.TabIndex = 7;
			this.button3.Text = "Toggle this rule";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// txtKeepCount
			// 
			this.txtKeepCount.Location = new System.Drawing.Point(286, 39);
			this.txtKeepCount.Name = "txtKeepCount";
			this.txtKeepCount.Size = new System.Drawing.Size(93, 20);
			this.txtKeepCount.TabIndex = 6;
			this.txtKeepCount.TextChanged += new System.EventHandler(this.txtKeepCount_TextChanged);
			// 
			// cmdCloneReq
			// 
			this.cmdCloneReq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdCloneReq.Location = new System.Drawing.Point(83, 320);
			this.cmdCloneReq.Name = "cmdCloneReq";
			this.cmdCloneReq.Size = new System.Drawing.Size(68, 23);
			this.cmdCloneReq.TabIndex = 5;
			this.cmdCloneReq.Text = "Clone";
			this.toolTip1.SetToolTip(this.cmdCloneReq, "Hold the control key down while clicking Clone to insert the cloned requirement a" +
        "fter the current selection.");
			this.cmdCloneReq.UseVisualStyleBackColor = true;
			this.cmdCloneReq.Click += new System.EventHandler(this.cmdCloneReq_Click);
			// 
			// groupReqs
			// 
			this.groupReqs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupReqs.Controls.Add(this.lblValue4);
			this.groupReqs.Controls.Add(this.txtValue4);
			this.groupReqs.Controls.Add(this.txtValue3);
			this.groupReqs.Controls.Add(this.lblValue3);
			this.groupReqs.Controls.Add(this.lblValue2);
			this.groupReqs.Controls.Add(this.txtValue2);
			this.groupReqs.Controls.Add(this.txtValue);
			this.groupReqs.Controls.Add(this.lblValue);
			this.groupReqs.Controls.Add(this.cmbKey);
			this.groupReqs.Controls.Add(this.lblKey);
			this.groupReqs.Controls.Add(this.cmbActsOn);
			this.groupReqs.Controls.Add(this.lblActsOn);
			this.groupReqs.Controls.Add(this.label3);
			this.groupReqs.Controls.Add(this.cmbReqType);
			this.groupReqs.Location = new System.Drawing.Point(261, 99);
			this.groupReqs.Name = "groupReqs";
			this.groupReqs.Size = new System.Drawing.Size(196, 253);
			this.groupReqs.TabIndex = 4;
			this.groupReqs.TabStop = false;
			this.groupReqs.Text = "Requirements";
			// 
			// lblValue4
			// 
			this.lblValue4.AutoSize = true;
			this.lblValue4.Location = new System.Drawing.Point(7, 96);
			this.lblValue4.Name = "lblValue4";
			this.lblValue4.Size = new System.Drawing.Size(43, 13);
			this.lblValue4.TabIndex = 16;
			this.lblValue4.Text = "Value 4";
			// 
			// txtValue4
			// 
			this.txtValue4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtValue4.Location = new System.Drawing.Point(6, 112);
			this.txtValue4.Name = "txtValue4";
			this.txtValue4.Size = new System.Drawing.Size(185, 20);
			this.txtValue4.TabIndex = 15;
			this.txtValue4.TextChanged += new System.EventHandler(this.txtValue4_TextChanged);
			// 
			// txtValue3
			// 
			this.txtValue3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtValue3.Location = new System.Drawing.Point(5, 230);
			this.txtValue3.Name = "txtValue3";
			this.txtValue3.Size = new System.Drawing.Size(185, 20);
			this.txtValue3.TabIndex = 14;
			this.txtValue3.TextChanged += new System.EventHandler(this.txtValue3_TextChanged);
			// 
			// lblValue3
			// 
			this.lblValue3.AutoSize = true;
			this.lblValue3.Location = new System.Drawing.Point(7, 214);
			this.lblValue3.Name = "lblValue3";
			this.lblValue3.Size = new System.Drawing.Size(43, 13);
			this.lblValue3.TabIndex = 13;
			this.lblValue3.Text = "Value 3";
			// 
			// lblValue2
			// 
			this.lblValue2.AutoSize = true;
			this.lblValue2.Location = new System.Drawing.Point(7, 175);
			this.lblValue2.Name = "lblValue2";
			this.lblValue2.Size = new System.Drawing.Size(43, 13);
			this.lblValue2.TabIndex = 12;
			this.lblValue2.Text = "Value 2";
			// 
			// txtValue2
			// 
			this.txtValue2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtValue2.Location = new System.Drawing.Point(6, 191);
			this.txtValue2.Name = "txtValue2";
			this.txtValue2.Size = new System.Drawing.Size(185, 20);
			this.txtValue2.TabIndex = 11;
			this.txtValue2.TextChanged += new System.EventHandler(this.txtValue2_TextChanged);
			// 
			// txtValue
			// 
			this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtValue.Location = new System.Drawing.Point(6, 152);
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(185, 20);
			this.txtValue.TabIndex = 10;
			this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
			// 
			// lblValue
			// 
			this.lblValue.AutoSize = true;
			this.lblValue.Location = new System.Drawing.Point(6, 136);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(34, 13);
			this.lblValue.TabIndex = 9;
			this.lblValue.Text = "Value";
			// 
			// cmbKey
			// 
			this.cmbKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbKey.FormattingEnabled = true;
			this.cmbKey.Location = new System.Drawing.Point(6, 112);
			this.cmbKey.Name = "cmbKey";
			this.cmbKey.Size = new System.Drawing.Size(142, 21);
			this.cmbKey.TabIndex = 8;
			this.cmbKey.SelectedIndexChanged += new System.EventHandler(this.cmbKey_SelectedIndexChanged);
			// 
			// lblKey
			// 
			this.lblKey.AutoSize = true;
			this.lblKey.Location = new System.Drawing.Point(6, 96);
			this.lblKey.Name = "lblKey";
			this.lblKey.Size = new System.Drawing.Size(25, 13);
			this.lblKey.TabIndex = 7;
			this.lblKey.Text = "Key";
			// 
			// cmbActsOn
			// 
			this.cmbActsOn.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cmbActsOn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbActsOn.FormattingEnabled = true;
			this.cmbActsOn.Location = new System.Drawing.Point(6, 72);
			this.cmbActsOn.Name = "cmbActsOn";
			this.cmbActsOn.Size = new System.Drawing.Size(184, 21);
			this.cmbActsOn.TabIndex = 6;
			this.cmbActsOn.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbActsOn_DrawItem);
			this.cmbActsOn.SelectedIndexChanged += new System.EventHandler(this.cmbActsOn_SelectedIndexChanged);
			// 
			// lblActsOn
			// 
			this.lblActsOn.AutoSize = true;
			this.lblActsOn.Location = new System.Drawing.Point(6, 56);
			this.lblActsOn.Name = "lblActsOn";
			this.lblActsOn.Size = new System.Drawing.Size(45, 13);
			this.lblActsOn.TabIndex = 5;
			this.lblActsOn.Text = "Acts On";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(94, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Requirement Type";
			// 
			// cmbReqType
			// 
			this.cmbReqType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbReqType.FormattingEnabled = true;
			this.cmbReqType.Location = new System.Drawing.Point(6, 32);
			this.cmbReqType.Name = "cmbReqType";
			this.cmbReqType.Size = new System.Drawing.Size(184, 21);
			this.cmbReqType.TabIndex = 3;
			this.cmbReqType.SelectedIndexChanged += new System.EventHandler(this.cmbReqType_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(37, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Action";
			// 
			// cmbAction
			// 
			this.cmbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAction.FormattingEnabled = true;
			this.cmbAction.Location = new System.Drawing.Point(159, 39);
			this.cmbAction.Name = "cmbAction";
			this.cmbAction.Size = new System.Drawing.Size(121, 21);
			this.cmbAction.TabIndex = 2;
			this.cmbAction.SelectedIndexChanged += new System.EventHandler(this.cmdAction_SelectedIndexChanged);
			// 
			// txtRuleName
			// 
			this.txtRuleName.Location = new System.Drawing.Point(159, 13);
			this.txtRuleName.Name = "txtRuleName";
			this.txtRuleName.Size = new System.Drawing.Size(228, 20);
			this.txtRuleName.TabIndex = 1;
			this.txtRuleName.TextChanged += new System.EventHandler(this.txtRuleName_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Rule Name";
			// 
			// lstRequirements
			// 
			this.lstRequirements.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lstRequirements.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstRequirements.FormattingEnabled = true;
			this.lstRequirements.Location = new System.Drawing.Point(9, 111);
			this.lstRequirements.Name = "lstRequirements";
			this.lstRequirements.Size = new System.Drawing.Size(249, 199);
			this.lstRequirements.TabIndex = 0;
			this.lstRequirements.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstRequirements_DrawItem);
			this.lstRequirements.SelectedIndexChanged += new System.EventHandler(this.lstRequirements_SelectedIndexChanged);
			// 
			// cmdDeleteReq
			// 
			this.cmdDeleteReq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdDeleteReq.Location = new System.Drawing.Point(157, 320);
			this.cmdDeleteReq.Name = "cmdDeleteReq";
			this.cmdDeleteReq.Size = new System.Drawing.Size(68, 23);
			this.cmdDeleteReq.TabIndex = 2;
			this.cmdDeleteReq.Text = "Delete";
			this.cmdDeleteReq.UseVisualStyleBackColor = true;
			this.cmdDeleteReq.Click += new System.EventHandler(this.cmdDeleteReq_Click);
			// 
			// cmdNewReq
			// 
			this.cmdNewReq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdNewReq.Location = new System.Drawing.Point(9, 320);
			this.cmdNewReq.Name = "cmdNewReq";
			this.cmdNewReq.Size = new System.Drawing.Size(68, 23);
			this.cmdNewReq.TabIndex = 1;
			this.cmdNewReq.Text = "New";
			this.toolTip1.SetToolTip(this.cmdNewReq, "Hold the control key down while clicking New to insert a new requirement after th" +
        "e current selection.");
			this.cmdNewReq.UseVisualStyleBackColor = true;
			this.cmdNewReq.Click += new System.EventHandler(this.cmdNewReq_Click);
			// 
			// cmdNewRule
			// 
			this.cmdNewRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdNewRule.Location = new System.Drawing.Point(8, 341);
			this.cmdNewRule.Name = "cmdNewRule";
			this.cmdNewRule.Size = new System.Drawing.Size(40, 23);
			this.cmdNewRule.TabIndex = 10;
			this.cmdNewRule.Text = "New";
			this.toolTip1.SetToolTip(this.cmdNewRule, "Hold the control key down while clicking New to insert a new rule after the curre" +
        "nt selection.");
			this.cmdNewRule.UseVisualStyleBackColor = true;
			this.cmdNewRule.Click += new System.EventHandler(this.cmdNewRule_Click);
			// 
			// lstRules
			// 
			this.lstRules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lstRules.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstRules.FormattingEnabled = true;
			this.lstRules.Location = new System.Drawing.Point(8, 32);
			this.lstRules.Name = "lstRules";
			this.lstRules.Size = new System.Drawing.Size(160, 303);
			this.lstRules.TabIndex = 9;
			this.lstRules.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstRules_DrawItem);
			this.lstRules.SelectedIndexChanged += new System.EventHandler(this.lstRules_SelectedIndexChanged);
			this.lstRules.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.lstRules_MouseWheel);
			// 
			// tabSalvageCombine
			// 
			this.tabSalvageCombine.Controls.Add(this.tSC_lblCombineRules);
			this.tabSalvageCombine.Controls.Add(this.tSC_groupCRS);
			this.tabSalvageCombine.Controls.Add(this.tSC_btnDelete);
			this.tabSalvageCombine.Controls.Add(this.tSC_btnNew);
			this.tabSalvageCombine.Controls.Add(this.tSC_listCombine);
			this.tabSalvageCombine.Controls.Add(this.tSC_lblDefaultCombine);
			this.tabSalvageCombine.Controls.Add(this.tSC_txtDefaultCombine);
			this.tabSalvageCombine.Location = new System.Drawing.Point(4, 22);
			this.tabSalvageCombine.Name = "tabSalvageCombine";
			this.tabSalvageCombine.Padding = new System.Windows.Forms.Padding(3);
			this.tabSalvageCombine.Size = new System.Drawing.Size(631, 447);
			this.tabSalvageCombine.TabIndex = 1;
			this.tabSalvageCombine.Text = "Salvage Combination";
			this.tabSalvageCombine.UseVisualStyleBackColor = true;
			// 
			// tSC_lblCombineRules
			// 
			this.tSC_lblCombineRules.AutoSize = true;
			this.tSC_lblCombineRules.Location = new System.Drawing.Point(8, 16);
			this.tSC_lblCombineRules.Name = "tSC_lblCombineRules";
			this.tSC_lblCombineRules.Size = new System.Drawing.Size(81, 13);
			this.tSC_lblCombineRules.TabIndex = 6;
			this.tSC_lblCombineRules.Text = "Combine Rules:";
			// 
			// tSC_groupCRS
			// 
			this.tSC_groupCRS.Controls.Add(this.tSC_txtCombineRange);
			this.tSC_groupCRS.Controls.Add(this.tSC_lblCombineRange);
			this.tSC_groupCRS.Controls.Add(this.tSC_cmbMaterial);
			this.tSC_groupCRS.Controls.Add(this.tSC_lblMaterial);
			this.tSC_groupCRS.Location = new System.Drawing.Point(306, 129);
			this.tSC_groupCRS.Name = "tSC_groupCRS";
			this.tSC_groupCRS.Size = new System.Drawing.Size(229, 154);
			this.tSC_groupCRS.TabIndex = 5;
			this.tSC_groupCRS.TabStop = false;
			this.tSC_groupCRS.Text = "Combine Range Setting";
			// 
			// tSC_txtCombineRange
			// 
			this.tSC_txtCombineRange.Location = new System.Drawing.Point(6, 72);
			this.tSC_txtCombineRange.Name = "tSC_txtCombineRange";
			this.tSC_txtCombineRange.Size = new System.Drawing.Size(201, 20);
			this.tSC_txtCombineRange.TabIndex = 3;
			this.tSC_txtCombineRange.TextChanged += new System.EventHandler(this.tSC_txtCombineRange_TextChanged);
			// 
			// tSC_lblCombineRange
			// 
			this.tSC_lblCombineRange.AutoSize = true;
			this.tSC_lblCombineRange.Location = new System.Drawing.Point(7, 56);
			this.tSC_lblCombineRange.Name = "tSC_lblCombineRange";
			this.tSC_lblCombineRange.Size = new System.Drawing.Size(83, 13);
			this.tSC_lblCombineRange.TabIndex = 2;
			this.tSC_lblCombineRange.Text = "Combine Range";
			// 
			// tSC_cmbMaterial
			// 
			this.tSC_cmbMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tSC_cmbMaterial.FormattingEnabled = true;
			this.tSC_cmbMaterial.Location = new System.Drawing.Point(6, 32);
			this.tSC_cmbMaterial.Name = "tSC_cmbMaterial";
			this.tSC_cmbMaterial.Size = new System.Drawing.Size(201, 21);
			this.tSC_cmbMaterial.TabIndex = 1;
			this.tSC_cmbMaterial.SelectedIndexChanged += new System.EventHandler(this.tSC_cmbMaterial_SelectedIndexChanged);
			// 
			// tSC_lblMaterial
			// 
			this.tSC_lblMaterial.AutoSize = true;
			this.tSC_lblMaterial.Location = new System.Drawing.Point(7, 16);
			this.tSC_lblMaterial.Name = "tSC_lblMaterial";
			this.tSC_lblMaterial.Size = new System.Drawing.Size(44, 13);
			this.tSC_lblMaterial.TabIndex = 0;
			this.tSC_lblMaterial.Text = "Material";
			// 
			// tSC_btnDelete
			// 
			this.tSC_btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.tSC_btnDelete.Location = new System.Drawing.Point(92, 393);
			this.tSC_btnDelete.Name = "tSC_btnDelete";
			this.tSC_btnDelete.Size = new System.Drawing.Size(75, 25);
			this.tSC_btnDelete.TabIndex = 4;
			this.tSC_btnDelete.Text = "Delete";
			this.tSC_btnDelete.UseVisualStyleBackColor = true;
			this.tSC_btnDelete.Click += new System.EventHandler(this.tSC_btnDelete_Click);
			// 
			// tSC_btnNew
			// 
			this.tSC_btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.tSC_btnNew.Location = new System.Drawing.Point(11, 393);
			this.tSC_btnNew.Name = "tSC_btnNew";
			this.tSC_btnNew.Size = new System.Drawing.Size(75, 25);
			this.tSC_btnNew.TabIndex = 3;
			this.tSC_btnNew.Text = "New";
			this.tSC_btnNew.UseVisualStyleBackColor = true;
			this.tSC_btnNew.Click += new System.EventHandler(this.tSC_btnNew_Click);
			// 
			// tSC_listCombine
			// 
			this.tSC_listCombine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tSC_listCombine.FormattingEnabled = true;
			this.tSC_listCombine.Location = new System.Drawing.Point(11, 32);
			this.tSC_listCombine.Name = "tSC_listCombine";
			this.tSC_listCombine.Size = new System.Drawing.Size(245, 355);
			this.tSC_listCombine.TabIndex = 2;
			this.tSC_listCombine.SelectedIndexChanged += new System.EventHandler(this.tSC_listCombine_SelectedIndexChanged);
			// 
			// tSC_lblDefaultCombine
			// 
			this.tSC_lblDefaultCombine.AutoSize = true;
			this.tSC_lblDefaultCombine.Location = new System.Drawing.Point(273, 16);
			this.tSC_lblDefaultCombine.Name = "tSC_lblDefaultCombine";
			this.tSC_lblDefaultCombine.Size = new System.Drawing.Size(123, 13);
			this.tSC_lblDefaultCombine.TabIndex = 1;
			this.tSC_lblDefaultCombine.Text = "Default Combine Range:";
			// 
			// tSC_txtDefaultCombine
			// 
			this.tSC_txtDefaultCombine.Location = new System.Drawing.Point(272, 32);
			this.tSC_txtDefaultCombine.Name = "tSC_txtDefaultCombine";
			this.tSC_txtDefaultCombine.Size = new System.Drawing.Size(294, 20);
			this.tSC_txtDefaultCombine.TabIndex = 0;
			this.tSC_txtDefaultCombine.TextChanged += new System.EventHandler(this.tSC_txtDefaultCombine_TextChanged);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator2,
            this.toolStripTextBox1,
            this.toolStripButton3});
			this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(652, 23);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			this.toolStrip1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabControl1_KeyDown);
			this.toolStrip1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tabControl1_KeyUp);
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 20);
			this.toolStripButton1.Text = "Copy rule";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 20);
			this.toolStripButton2.Text = "Paste rule";
			this.toolStripButton2.ToolTipText = "Paste rule. Hold the control key down to paste over the current selected rule.";
			this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
			// 
			// toolStripTextBox1
			// 
			this.toolStripTextBox1.AutoToolTip = true;
			this.toolStripTextBox1.Name = "toolStripTextBox1";
			this.toolStripTextBox1.Size = new System.Drawing.Size(200, 23);
			this.toolStripTextBox1.ToolTipText = "Search for rule by name";
			this.toolStripTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox1_KeyPress);
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
			this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Size = new System.Drawing.Size(23, 20);
			this.toolStripButton3.Text = "Go to next match";
			this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
			// 
			// toolTip1
			// 
			this.toolTip1.ShowAlways = true;
			// 
			// Form1
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(652, 524);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(668, 562);
			this.Name = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabLootRules.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupRule.ResumeLayout(false);
			this.groupRule.PerformLayout();
			this.groupReqs.ResumeLayout(false);
			this.groupReqs.PerformLayout();
			this.tabSalvageCombine.ResumeLayout(false);
			this.tabSalvageCombine.PerformLayout();
			this.tSC_groupCRS.ResumeLayout(false);
			this.tSC_groupCRS.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        void lstRules_MouseWheel(object sender, MouseEventArgs e)
        {
            if (CtrlPressed)
            {
                ((HandledMouseEventArgs)e).Handled = true;
                if (e.Delta > 0)
                {
                    ruleMoveUp(lstRules.SelectedIndex, true);
                }
                else
                {
                    ruleMoveDown(lstRules.SelectedIndex, true);
                }
            }
        }

        void cmbSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSet.SelectedIndex > 0)
            {
                SortedDictionary<string, int> setIds = GameInfo.getSetInfo();
                if (setIds.ContainsKey(cmbSet.Items[cmbSet.SelectedIndex].ToString()))
                {
                    txtSet.Text = setIds[cmbSet.Items[cmbSet.SelectedIndex].ToString()].ToString();
                }
            }
        }

        void cmbSkill_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSkill.SelectedIndex > 0)
            {
                SortedDictionary<string, int> skillIds = GameInfo.getSkillInfo();
                if (skillIds.ContainsKey(cmbSkill.Items[cmbSkill.SelectedIndex].ToString()))
                {
                    txtSkill.Text = skillIds[cmbSkill.Items[cmbSkill.SelectedIndex].ToString()].ToString();
                }
            }
        }

		void cmbMastery_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbMastery.SelectedIndex > 0)
			{
				SortedDictionary<string, int> masteryIds = GameInfo.getMasteryInfo();
				if (masteryIds.ContainsKey(cmbMastery.Items[cmbMastery.SelectedIndex].ToString()))
				{
					txtMastery.Text = masteryIds[cmbMastery.Items[cmbMastery.SelectedIndex].ToString()].ToString();
				}
			}
		}

        void cmbMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMaterial.SelectedIndex > 0)
            {
                SortedDictionary<string, int> matIds = GameInfo.getMaterialInfo();
                if (matIds.ContainsKey(cmbMaterial.Items[cmbMaterial.SelectedIndex].ToString()))
                {
                    txtMaterial.Text = matIds[cmbMaterial.Items[cmbMaterial.SelectedIndex].ToString()].ToString();
                }
            }
        }

        Color HighlightBGColor(Color bgc)
        {
            int r = bgc.R - 50;
            int g = bgc.G - 50;
            int b = bgc.B + 75;
            if (r < 0) r = 0;
            if (g < 0) g = 0;
            if (b > 255) b = 255;
            return Color.FromArgb(r, g, b);
        }

        void lstRules_DrawItem(object sender, DrawItemEventArgs e)
        {
            System.Windows.Forms.ListBox s = (System.Windows.Forms.ListBox)sender;

            Color bgColor = Color.White;
            Brush fgBrush = Brushes.Black;

            bool badRule = true;
            bool ruleDisabled = false;

            if (e.Index > -1)
            {
                cLootItemRule r = LootRules.Rules[e.Index];
                foreach (iLootRule ir in r.IntRules)
                {
                    if (!ir.MayRequireID())
                    {
                        badRule = false;
                        break;
                    }
                }

                foreach (iLootRule ir in r.IntRules)
                {
                    if (ir.GetRuleType() == eLootRuleType.DisabledRule && ir.UI_ActsOnCombo_Get() == 0)
                    {
                        ruleDisabled = true;
                        break;
                    }
                }

                bool hilight = (e.State & DrawItemState.Selected) > 0 || (e.State & DrawItemState.Focus) > 0;

                if (badRule)
                {
                    bgColor = Color.DarkRed;
                    if (hilight) bgColor = HighlightBGColor(bgColor);

                    fgBrush = Brushes.White;
                }
                else
                {
                    bgColor = Color.White;
                    if (hilight) bgColor = HighlightBGColor(bgColor);
                }
            }

            //e.DrawBackground();
            using (Brush bgBrush = new SolidBrush(bgColor))
            {
                e.Graphics.FillRectangle(bgBrush, e.Bounds);
            }
            if (e.Index > -1)
            {
                Font fn = e.Font;
                if (ruleDisabled) {
                    fn = new Font(fn, FontStyle.Strikeout);
                } else {
                    fn = new Font(fn, FontStyle.Regular);
                }

                e.Graphics.DrawString(s.Items[e.Index].ToString(), fn, fgBrush, e.Bounds, StringFormat.GenericDefault);
            }
            e.DrawFocusRectangle();
        }

        void cmbActsOn_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            System.Windows.Forms.ComboBox s = (System.Windows.Forms.ComboBox)sender;

            bool needsouterdraw = true;

            bool hilight = e.State == DrawItemState.Selected
                || e.State == (DrawItemState.Selected | DrawItemState.NoAccelerator | DrawItemState.NoFocusRect | DrawItemState.Focus)
                || e.State == (DrawItemState.NoAccelerator | DrawItemState.NoFocusRect | DrawItemState.ComboBoxEdit);

            try
            {
                if (CurrentReq.UI_ActsOnCombo_Uses())
                {
                    if (e.Index >= 0)
                    {
                        Color MyColor = CurrentReq.UI_ActsOnCombo_OptionColors(e.Index);

                        Color bgc = MyColor;
                        if (hilight)
                        {
                            bgc = HighlightBGColor(bgc);
                        }
                        Brush fcg = Brushes.White;
                        if (MyColor.ToArgb() == Color.White.ToArgb())
                            fcg = Brushes.Black;
                        using (Brush bg = new SolidBrush(bgc))
                        {
                            e.Graphics.FillRectangle(bg, e.Bounds);
                            e.Graphics.DrawString(s.Items[e.Index].ToString(), e.Font, fcg, e.Bounds, StringFormat.GenericDefault);
                            e.DrawFocusRectangle();
                        }
                        needsouterdraw = false;
                    }

                    /*
                    if (CurrentReq.GetRuleType() == eLootRuleType.LongValKeyLE || CurrentReq.GetRuleType() == eLootRuleType.LongValKeyGE)
                    {
                        if (GameInfo.IsIDProperty(ComboKeys.LVKFromIndex(e.Index)))
                        {
                            textBrush = hilight ? Brushes.Red : Brushes.DarkRed;
                        }
                    }
                    else if (CurrentReq.GetRuleType() == eLootRuleType.DoubleValKeyLE || CurrentReq.GetRuleType() == eLootRuleType.DoubleValKeyGE)
                    {
                        if (GameInfo.IsIDProperty(ComboKeys.DVKFromIndex(e.Index)))
                        {
                            textBrush = hilight ? Brushes.Red : Brushes.DarkRed;
                        }
                    }
                    */
                }
            }
            catch { }

            if (needsouterdraw)
            {
                Color ibgColor = Color.White;
                if (hilight) ibgColor = HighlightBGColor(ibgColor);
                using (Brush bgBrush = new SolidBrush(ibgColor))
                {
                    e.Graphics.FillRectangle(bgBrush, e.Bounds);
                }
                if (e.Index > -1)
                    e.Graphics.DrawString(s.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
        }

        void lstRequirements_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            System.Windows.Forms.ListBox s = (System.Windows.Forms.ListBox)sender;

            //Brush bgBrush = Brushes.White;
            Color bgColor = Color.White;
            Brush fgBrush = Brushes.Black;

            bool hilight = e.State == DrawItemState.Selected
                || e.State == (DrawItemState.Selected | DrawItemState.NoAccelerator | DrawItemState.NoFocusRect | DrawItemState.Focus)
                || e.State == (DrawItemState.NoAccelerator | DrawItemState.NoFocusRect | DrawItemState.ComboBoxEdit);

            if (e.Index > -1 && CurrentRule != null && CurrentRule.IntRules[e.Index].MayRequireID())
            {
                bgColor = Color.DarkRed;
                if (hilight) bgColor = HighlightBGColor(bgColor);

                fgBrush = Brushes.White;
            }
            else
            {
                bgColor = Color.White;
                if (hilight) bgColor = HighlightBGColor(bgColor);
            }

            using (Brush bgBrush = new SolidBrush(bgColor))
            {
                e.Graphics.FillRectangle(bgBrush, e.Bounds);
            }
            if (e.Index > -1)
                e.Graphics.DrawString(s.Items[e.Index].ToString(), e.Font, fgBrush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem addSalvageRulesToolStripMenuItem;
        private ToolStripMenuItem increaseSalvageWorkmanshipsToolStripMenuItem;
        private ToolStripMenuItem autoSortRulesToolStripMenuItem;
        private TabControl tabControl1;
        private TabPage tabLootRules;
        private Button button2;
        private Button cmdCloneRule;
        private GroupBox groupBox1;
        private TextBox txtSet;
        private ComboBox cmbSet;
        private TextBox txtSkill;
        private ComboBox cmbSkill;
        private TextBox txtMaterial;
        private ComboBox cmbMaterial;
        private Button button1;
        private Button cmdDeleteRule;
        private GroupBox groupRule;
        private Button cmdCloneReq;
        private GroupBox groupReqs;
        private TextBox txtValue3;
        private Label lblValue3;
        private Label lblValue2;
        private TextBox txtValue2;
        private TextBox txtValue;
        private Label lblValue;
        private ComboBox cmbKey;
        private Label lblKey;
        private ComboBox cmbActsOn;
        private Label lblActsOn;
        private Label label3;
        private ComboBox cmbReqType;
        private Label label2;
        private ComboBox cmbAction;
        private TextBox txtRuleName;
        private Label label1;
        private ListBox lstRequirements;
        private Button cmdDeleteReq;
        private Button cmdNewReq;
        private Button cmdNewRule;
        private ListBox lstRules;
        private TabPage tabSalvageCombine;
        private GroupBox tSC_groupCRS;
        private Label tSC_lblMaterial;
        private Button tSC_btnDelete;
        private Button tSC_btnNew;
        private ListBox tSC_listCombine;
        private Label tSC_lblDefaultCombine;
        private TextBox tSC_txtDefaultCombine;
        private TextBox tSC_txtCombineRange;
        private Label tSC_lblCombineRange;
        private ComboBox tSC_cmbMaterial;
        private Label tSC_lblCombineRules;
        private ToolStripMenuItem combineToolStripMenuItem;
        private ToolStripMenuItem exportRangesToolStripMenuItem;
        private ToolStripMenuItem importRangesToolStripMenuItem;
        private TextBox txtKeepCount;
        private ToolStripMenuItem addPackslotRulesToolStripMenuItem;
        private Button button3;
        private Label label4;
        private Label lblValue4;
        private TextBox txtValue4;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripButton toolStripButton3;
		private ToolTip toolTip1;
		private TextBox txtMastery;
		private ComboBox cmbMastery;


    }
}

