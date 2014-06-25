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
    public partial class SetDbUserPw : Form
    {
        public Boolean setPw = false;
        public string pw;
        public SetDbUserPw()
        { InitializeComponent(); }

        private void btOk_Click(object sender, EventArgs e)
        {
            #region Check
            #region Check blank
            if (tbPw.Text.Length == 0)
            {
                MessageBox.Show(Properties.Resources.BlankNotAllowed, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion
            #region Check not allowed characters
            if (functions.checkStrings(this.tbPw.Text) == functions.strState.NotAllowed)
            {
                MessageBox.Show(Properties.Resources.NotAllowedCharacterIncluded, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion
            #endregion

            setPw = true;
            pw = tbPw.Text;
            this.Close();
        }
    }
}
