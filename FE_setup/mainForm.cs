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
        private string sslString = "SSL Mode=Require;Trust Server Certificate=true";
        public mainForm()
        { InitializeComponent(); }

        private void btCreateEndoDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string defaultDirectory = @"C:\Program Files\PostgreSQL";
            if (Directory.Exists(defaultDirectory))
            { ofd.InitialDirectory = defaultDirectory; }
            else
            { ofd.InitialDirectory = @"C:\Program Files"; }

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
                    p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec"); //Get ComSpec(cmd.exe) path, and set to FileName property
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
                {
                    sql_path = ofd.FileName;
                    ofd.Dispose();
                }
                else
                {
                    ofd.Dispose();
                    spf.Dispose();
                    return;
                }

                string connStr = "Host=" + spf.srvIP + ";Port=" + spf.srvPort + ";Username=postgres;Password=" + spf.pw + ";Database=endoDB;" + sslString;

                if (File.Exists(sql_path))
                {
                    string SQL = "";

                    using (StreamReader file = new StreamReader(sql_path))
                    {
                        try
                        {
                            while (file.Peek() >= 0)
                            {
                                string buffer = file.ReadLine();
                                SQL += buffer + Environment.NewLine;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw;
                        }
                        finally
                        {
                            if (file != null)
                            { file.Close(); }
                        }
                    }

                    try
                    {
                        using (var conn = new NpgsqlConnection(connStr))
                        {
                            try
                            { conn.Open(); }
                            catch (NpgsqlException npe)
                            {
                                MessageBox.Show(Properties.Resources.CouldntOpenConn + "\r\n" + npe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                conn.Close();
                            }
                            catch (IOException ioe)
                            {
                                MessageBox.Show(Properties.Resources.ConnClosed + "\r\n" + ioe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                conn.Close();
                            }

                            if (conn.State == ConnectionState.Open)
                            {

                                using (var cmd = new NpgsqlCommand())
                                {
                                    try
                                    {
                                        cmd.Connection = conn;
                                        cmd.CommandText = SQL;
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (NpgsqlException nex)
                                    {
                                        MessageBox.Show("[NpgsqlException]\r\n" + nex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    finally
                                    {
                                        conn.Close();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show(Properties.Resources.CouldntOpenConn, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                conn.Close();
                            }
                        }
                    }
                    catch (ArgumentException ae)
                    {
                        MessageBox.Show(Properties.Resources.WrongConnectingString + "\r\n" + ae.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                string conn = "Host=" + spf.srvIP + ";Port=" + spf.srvPort + ";Username=postgres;Password=" + spf.pw + ";" + sslString;
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
                    p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec"); //Get ComSpec(cmd.exe) path, and set to FileName property
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
