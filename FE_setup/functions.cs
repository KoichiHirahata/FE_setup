using System;
using System.Windows.Forms;
using System.IO;
using Npgsql;

namespace FE_setup
{
    public class functions
    {
        #region for SetPwForm, SetDbUserPw form
        public enum strState { Proper, NotAllowed };

        public static strState checkStrings(string str)
        {
            string checkCharacters = "<>\"\'/\r\n\\";
            for (int i = 0; i < checkCharacters.Length; i++)
            {
                if (str.IndexOf(checkCharacters[i]) != -1)
                { return strState.NotAllowed; }
            }
            return strState.Proper;
        }
        #endregion

        public enum functionResult { success, failed, connectionError };

        /// <summary>
        /// Do SQL without transaction.
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="sql">SQL</param>
        /// <param name="p_str">parameter</param>
        /// <returns>Return functionResult</returns>
        public static functionResult doSQL(string connectionString, string sql, params string[] p_str)
        {
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                #region Npgsql connection
                NpgsqlConnection conn = new NpgsqlConnection();

                conn.ConnectionString = connectionString;

                try
                { conn.Open(); }
                catch (NpgsqlException nex)
                {
                    MessageBox.Show(nex.Message, "Npgsql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
                    return functionResult.connectionError;
                }
                catch (IOException ioe)
                {
                    MessageBox.Show(ioe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
                    return functionResult.connectionError;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
                    return functionResult.connectionError;
                }
                #endregion

                try
                {
                    command.CommandText = sql;
                    command.Connection = conn;

                    string p;
                    for (int i = 0; i < p_str.Length; i++)
                    {
                        p = ":p" + i.ToString();
                        //MessageBox.Show(p + " to " + p_str[i]);
                        command.Parameters.Add(new NpgsqlParameter(p, p_str[i]));
                    }

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
                    //throw ex;
                    return functionResult.failed;
                }
                conn.Close();
            }
            return functionResult.success;
        }

        public static functionResult ExeNonQuery(string srvIP, string srvPort, string pw, string sql, params string[] p_str)
        {
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                #region Npgsql connection
                NpgsqlConnection conn = new NpgsqlConnection();

                conn.ConnectionString = @"Server=" + srvIP + ";Port=" + srvPort + ";User Id=postgres;Password=" + pw + ";";

                try
                { conn.Open(); }
                catch (NpgsqlException nex)
                {
                    MessageBox.Show(nex.Message, "Npgsql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
                    return functionResult.connectionError;
                }
                catch (IOException ioe)
                {
                    MessageBox.Show(ioe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
                    return functionResult.connectionError;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
                    return functionResult.connectionError;
                }
                #endregion

                NpgsqlTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                try
                {
                    command.CommandText = sql;
                    command.Connection = conn;

                    string p;
                    for (int i = 0; i < p_str.Length; i++)
                    {
                        p = ":p" + i.ToString();
                        //MessageBox.Show(p + " to " + p_str[i]);
                        command.Parameters.Add(new NpgsqlParameter(p, p_str[i]));
                    }

                    command.Transaction = transaction;
                    command.ExecuteNonQuery();

                    //Commit transaction
                    transaction.Commit();
                }
                catch (Exception ex) // ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Roll back transaction
                    transaction.Rollback();
                    conn.Close();
                    //throw ex;
                    return functionResult.failed;
                }
                conn.Close();
            }
            return functionResult.success;
        }
    }
}
