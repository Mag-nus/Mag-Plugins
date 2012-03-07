using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VTClassic
{
    public partial class SelectMinPackSlotsForm : Form
    {

        public int slots = 1;
        public eLootAction act = eLootAction.Keep;

        public SelectMinPackSlotsForm()
        {
            InitializeComponent();

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(eLootActionTool.FriendlyNames().ToArray());
            comboBox1.SelectedIndex = 0;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            slots = Decimal.ToInt32(numericUpDown1.Value);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            act = eLootActionTool.enumValue((string)comboBox1.Items[comboBox1.SelectedIndex]);
        }

    }
}
