using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VTClassic
{
    public partial class UpdateWorkReqsForm : Form
    {
        public int wrk = 0;
        public eLootAction act = eLootAction.Keep;

        public UpdateWorkReqsForm()
        {
            InitializeComponent();

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(eLootActionTool.FriendlyNames().ToArray());
            comboBox1.SelectedIndex = 0;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            wrk = trackBar1.Value;
            lblUpdWRWorkV.Text = trackBar1.Value.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            act = eLootActionTool.enumValue((string)comboBox1.Items[comboBox1.SelectedIndex]);
        }

    }
}
