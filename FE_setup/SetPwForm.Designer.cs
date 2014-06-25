namespace FE_setup
{
    partial class SetPwForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetPwForm));
            this.tbPw = new System.Windows.Forms.TextBox();
            this.btOk = new System.Windows.Forms.Button();
            this.lbPw = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbPw
            // 
            resources.ApplyResources(this.tbPw, "tbPw");
            this.tbPw.Name = "tbPw";
            // 
            // btOk
            // 
            resources.ApplyResources(this.btOk, "btOk");
            this.btOk.Name = "btOk";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // lbPw
            // 
            resources.ApplyResources(this.lbPw, "lbPw");
            this.lbPw.Name = "lbPw";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbIP
            // 
            resources.ApplyResources(this.tbIP, "tbIP");
            this.tbIP.Name = "tbIP";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbPort
            // 
            resources.ApplyResources(this.tbPort, "tbPort");
            this.tbPort.Name = "tbPort";
            // 
            // SetPwForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbIP);
            this.Controls.Add(this.lbPw);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.tbPw);
            this.Name = "SetPwForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbPw;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Label lbPw;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPort;
    }
}