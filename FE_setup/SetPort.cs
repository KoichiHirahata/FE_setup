using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FE_setup
{
    public partial class SetPort : Form
    {
        public Boolean portSet = false;
        public string portNo;

        public SetPort()
        { InitializeComponent(); }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (functions.checkStrings(tbPortNo.Text) == functions.strState.NotAllowed)
            {
                MessageBox.Show(Properties.Resources.NotAllowedCharacterIncluded, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                portSet = true;
                portNo = tbPortNo.Text;
                this.Close();
            }
        }
    }
}
