using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Npgsql;


namespace FE_setup
{
    public partial class mainForm : Form
    {
        private string sslString = "SSL=true;SslMode=Require;";
        public mainForm()
        { InitializeComponent(); }

        private void btCreateEndoDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\Program Files";
            ofd.Filter = "psql.exe(psql.exe)|psql.exe|All Files(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Title = Properties.Resources.WhereIsPsql;

            string psql_path;

            if (ofd.ShowDialog() == DialogResult.OK)
            { psql_path = ofd.FileName; }
            else
            { return; }

            if (File.Exists(Application.StartupPath + @"\setup.dump"))
            {
                SetPort sp = new SetPort();
                sp.ShowDialog(this);
                if (sp.portSet)
                {
                    System.Diagnostics.Process p = new System.Diagnostics.Process(); //Create process object
                    p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec"); //Get ComSpec(cmd.exe) path, and set to FileName property
                    p.StartInfo.UseShellExecute = false;
                    //p.StartInfo.RedirectStandardOutput = true; //Make output readable
                    //p.StartInfo.RedirectStandardInput = false;
                    p.StartInfo.CreateNoWindow = false; //Display window
                    p.StartInfo.WorkingDirectory = Application.StartupPath;
                    p.StartInfo.Arguments = "/c \"" + psql_path + "\" -h localhost -p " + sp.portNo + " -U postgres -f setup.dump"; //Set command line("/c" needed for close window after running)
                    sp.Dispose();
                    p.Start();
                    //string results = p.StandardOutput.ReadToEnd(); //Get output
                    p.WaitForExit(); //WaitForExit has to be after ReadToEnd
                    //p.Close();
                    //MessageBox.Show(results); //Show output
                    MessageBox.Show(Properties.Resources.ProcedureFinished, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    sp.Dispose();
                    MessageBox.Show(Properties.Resources.ProcedureCancelled, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("[setup.dump]" + Properties.Resources.FileNotExist, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void bt_do_sql_Click(object sender, EventArgs e)
        {
            SetPwForm spf = new SetPwForm();
            spf.ShowDialog(this);

            if (spf.pwSet)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "SQL(*.sql;*.txt)|*.sql;*.txt|All Files(*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.Title = Properties.Resources.SelectSQLFile;

                string sql_path;

                if (ofd.ShowDialog() == DialogResult.OK)
                { sql_path = ofd.FileName; }
                else
                { return; }

                string conn = "Server=" + spf.srvIP + ";Port=" + spf.srvPort + ";User Id=postgres;Password=" + spf.pw + ";Database=endoDB;" + sslString;
                functions.functionResult rst;

                if (File.Exists(sql_path))
                {
                    StreamReader file = new StreamReader(sql_path);
                    List<string> SQLs = new List<string>();
                    string line;
                    while (!string.IsNullOrWhiteSpace((line = file.ReadLine())))
                    { SQLs.Add(line); }

                    for (int i = 0; i < SQLs.Count; i++)
                    {
                        //MessageBox.Show(SQLs[i]);
                        rst = functions.doSQL(conn, SQLs[i]);
                        if (rst == functions.functionResult.failed)
                        { MessageBox.Show("[SQL: Line " + (i + 1).ToString() + "]" + Properties.Resources.DataBaseError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        else if (rst == functions.functionResult.connectionError)
                        { MessageBox.Show("[SQL: Line " + (i + 1).ToString() + "]" + Properties.Resources.ConnectFailed, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    MessageBox.Show("[SQL File]" + Properties.Resources.ProcedureFinished, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("[SQL File]" + Properties.Resources.FileNotExist, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            spf.Dispose();
        }

        private void btSetDbUserPw_Click(object sender, EventArgs e)
        {
            SetPwForm spf = new SetPwForm();
            spf.ShowDialog(this);

            if (spf.pwSet)
            {
                //string conn = "Server=" + spf.srvIP + ";Port=" + spf.srvPort + ";User Id=postgres;Password=" + spf.pw + ";Database=endoDB;";
                string conn = "Server=" + spf.srvIP + ";Port=" + spf.srvPort + ";User Id=postgres;Password=" + spf.pw + ";" + sslString;
                functions.functionResult rst;

                SetDbUserPw sdup = new SetDbUserPw();
                sdup.ShowDialog(this);

                if (sdup.setPw)
                {
                    rst = functions.doSQL(conn, "ALTER ROLE db_user WITH ENCRYPTED PASSWORD \'" + sdup.pw + "\';");
                    if (rst == functions.functionResult.failed)
                    {
                        MessageBox.Show(Properties.Resources.DataBaseError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (rst == functions.functionResult.connectionError)
                    {
                        MessageBox.Show(Properties.Resources.ConnectFailed, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show(Properties.Resources.UpdateDone, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    sdup.Dispose();
                    return;
                }
            }

            spf.Dispose();
        }

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Version ve = new Version();
            ve.ShowDialog(this);
        }

        private void btRestore_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\Program Files";
            ofd.Filter = "psql.exe(psql.exe)|psql.exe|All Files(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Title = Properties.Resources.WhereIsPsql;

            string psql_path;

            if (ofd.ShowDialog() == DialogResult.OK)
            { psql_path = ofd.FileName; }
            else
            { return; }

            ofd.Filter = "Back up file(*.sql)|*.sql|All Files(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Title = Properties.Resources.WhereIsBackupFile;

            string backupPath;

            if (ofd.ShowDialog() == DialogResult.OK)
            { backupPath = ofd.FileName; }
            else
            { return; }
            
            if (File.Exists(backupPath))
            {
                SetPort sp = new SetPort();
                sp.ShowDialog(this);
                if (sp.portSet)
                {
                    System.Diagnostics.Process p = new System.Diagnostics.Process(); //Create process object
                    p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec"); //Get ComSpec(cmd.exe) path, and set to FileName property
                    p.StartInfo.UseShellExecute = false;
                    //p.StartInfo.RedirectStandardOutput = true; //Make output readable
                    //p.StartInfo.RedirectStandardInput = false;
                    p.StartInfo.CreateNoWindow = false; //Display window
                    p.StartInfo.WorkingDirectory = Application.StartupPath;
                    p.StartInfo.Arguments = "/c \"" + psql_path + "\" -h localhost -p " + sp.portNo + " -U postgres -f " + backupPath; //Set command line("/c" needed for close window after running)
                    sp.Dispose();
                    p.Start();
                    //string results = p.StandardOutput.ReadToEnd(); //Get output
                    p.WaitForExit(); //WaitForExit has to be after ReadToEnd
                    //p.Close();
                    //MessageBox.Show(results); //Show output
                    MessageBox.Show(Properties.Resources.ProcedureFinished, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    sp.Dispose();
                    MessageBox.Show(Properties.Resources.ProcedureCancelled, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("[Back up file]" + Properties.Resources.FileNotExist, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
