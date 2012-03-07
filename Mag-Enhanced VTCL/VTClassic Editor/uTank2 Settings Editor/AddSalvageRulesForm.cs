using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VTClassic
{
    public partial class AddSalvageRulesForm : Form
    {
        public string grp;
        public int wrk = 10;

        public AddSalvageRulesForm()
        {
            InitializeComponent();

            SortedDictionary<string, int[]> matGroups = GameInfo.getMaterialGroups();
            this.cmdAddSRGroup.Items.Clear();
            foreach (KeyValuePair<string, int[]> kv in matGroups)
                this.cmdAddSRGroup.Items.Add(kv.Key);
            this.cmdAddSRGroup.SelectedIndex = 0;
        }

        private void cmdAddSRGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblAddSRInfo.Text = string.Empty;
            if (cmdAddSRGroup.SelectedIndex > -1)
            {
                try
                {
                    grp = cmdAddSRGroup.Items[cmdAddSRGroup.SelectedIndex].ToString();

                    int[] matIds = GameInfo.getMaterialGroups()[grp];
                    string[] sArr = new string[matIds.Length];
                    for (int i = 0; i < matIds.Length; i++)
                    {
                        sArr[i] = GameInfo.getMaterialName(matIds[i]);
                    }
                    lblAddSRInfo.Text = string.Join(", ", sArr);
                }
                catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Exception: " + ex.ToString()); }
            }
        }

        private void trbAddSRWork_Scroll(object sender, EventArgs e)
        {
            try
            {
                wrk = trbAddSRWork.Value;
                lblAddSRWorkV.Text = wrk.ToString();
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Exception: " + ex.ToString()); }
        }

    }
}
