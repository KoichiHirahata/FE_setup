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
    public partial class SetPwForm : Form
    {
        public Boolean pwSet = false;
        public string srvIP;
        public string srvPort;
        public string pw;

        public SetPwForm()
        { InitializeComponent(); }

        private void btOk_Click(object sender, EventArgs e)
        {
            #region check
            #region Check blank
            if (this.tbIP.Text.Length == 0)
            {
                MessageBox.Show("[IP]" + Properties.Resources.BlankNotAllowed, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (this.tbPort.Text.Length == 0)
            {
                MessageBox.Show("[Port]" + Properties.Resources.BlankNotAllowed, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (this.tbPw.Text.Length == 0)
            {
                MessageBox.Show("[Password]" + Properties.Resources.BlankNotAllowed, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion
            #region Check not allowed characters
            if (functions.checkStrings(this.tbIP.Text) == functions.strState.NotAllowed)
            {
                MessageBox.Show("[IP]" + Properties.Resources.NotAllowedCharacterIncluded, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (functions.checkStrings(this.tbPort.Text) == functions.strState.NotAllowed)
            {
                MessageBox.Show("[Port]" + Properties.Resources.NotAllowedCharacterIncluded, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (functions.checkStrings(this.tbPw.Text) == functions.strState.NotAllowed)
            {
                MessageBox.Show("[Password]" + Properties.Resources.NotAllowedCharacterIncluded, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion
            #endregion

            pwSet = true;
            srvIP = this.tbIP.Text;
            srvPort = this.tbPort.Text;
            pw = this.tbPw.Text;
            this.Close();
        }

    }
}
