namespace FE_setup
{
    partial class mainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.btCreateEndoDB = new System.Windows.Forms.Button();
            this.bt_do_sql = new System.Windows.Forms.Button();
            this.btSetDbUserPw = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.versionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCreateEndoDB
            // 
            resources.ApplyResources(this.btCreateEndoDB, "btCreateEndoDB");
            this.btCreateEndoDB.Name = "btCreateEndoDB";
            this.btCreateEndoDB.UseVisualStyleBackColor = true;
            this.btCreateEndoDB.Click += new System.EventHandler(this.btCreateEndoDB_Click);
            // 
            // bt_do_sql
            // 
            resources.ApplyResources(this.bt_do_sql, "bt_do_sql");
            this.bt_do_sql.Name = "bt_do_sql";
            this.bt_do_sql.UseVisualStyleBackColor = true;
            this.bt_do_sql.Click += new System.EventHandler(this.bt_do_sql_Click);
            // 
            // btSetDbUserPw
            // 
            resources.ApplyResources(this.btSetDbUserPw, "btSetDbUserPw");
            this.btSetDbUserPw.Name = "btSetDbUserPw";
            this.btSetDbUserPw.UseVisualStyleBackColor = true;
            this.btSetDbUserPw.Click += new System.EventHandler(this.btSetDbUserPw_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.versionToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // versionToolStripMenuItem
            // 
            this.versionToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
            resources.ApplyResources(this.versionToolStripMenuItem, "versionToolStripMenuItem");
            this.versionToolStripMenuItem.Click += new System.EventHandler(this.versionToolStripMenuItem_Click);
            // 
            // mainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btSetDbUserPw);
            this.Controls.Add(this.bt_do_sql);
            this.Controls.Add(this.btCreateEndoDB);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "mainForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCreateEndoDB;
        private System.Windows.Forms.Button bt_do_sql;
        private System.Windows.Forms.Button btSetDbUserPw;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem versionToolStripMenuItem;
    }
}

