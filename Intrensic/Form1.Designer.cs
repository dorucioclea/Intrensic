namespace Intrensic
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lblInfo = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.niUser = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.ctrlUserInfo1 = new Intrensic.ctrlUserInfo();
            this.ehVideos = new System.Windows.Forms.Integration.ElementHost();
            this.button2 = new System.Windows.Forms.Button();
            this.cmNotify.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(39, 25);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(35, 13);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "label1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 330);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // niUser
            // 
            this.niUser.ContextMenuStrip = this.cmNotify;
            this.niUser.Icon = ((System.Drawing.Icon)(resources.GetObject("niUser.Icon")));
            this.niUser.Text = "Intrensic";
            this.niUser.Visible = true;
            // 
            // cmNotify
            // 
            this.cmNotify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(16)))), ((int)(((byte)(59)))));
            this.cmNotify.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cmNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmLogin});
            this.cmNotify.Name = "cmNotify";
            this.cmNotify.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.cmNotify.ShowImageMargin = false;
            this.cmNotify.Size = new System.Drawing.Size(83, 26);
            // 
            // tsmLogin
            // 
            this.tsmLogin.ForeColor = System.Drawing.Color.White;
            this.tsmLogin.Name = "tsmLogin";
            this.tsmLogin.Size = new System.Drawing.Size(82, 22);
            this.tsmLogin.Text = "Log In";
            this.tsmLogin.Click += new System.EventHandler(this.tsmLogin_Click);
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(12, 41);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(319, 283);
            this.axWindowsMediaPlayer1.TabIndex = 4;
            // 
            // ctrlUserInfo1
            // 
            this.ctrlUserInfo1.BackColor = System.Drawing.Color.Transparent;
            this.ctrlUserInfo1.Location = new System.Drawing.Point(155, 343);
            this.ctrlUserInfo1.Name = "ctrlUserInfo1";
            this.ctrlUserInfo1.Size = new System.Drawing.Size(130, 50);
            this.ctrlUserInfo1.TabIndex = 6;
            // 
            // ehVideos
            // 
            this.ehVideos.Location = new System.Drawing.Point(354, 41);
            this.ehVideos.Name = "ehVideos";
            this.ehVideos.Size = new System.Drawing.Size(494, 352);
            this.ehVideos.TabIndex = 7;
            this.ehVideos.Text = "elementHost1";
            this.ehVideos.Child = null;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(294, 330);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(37, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 405);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ehVideos);
            this.Controls.Add(this.ctrlUserInfo1);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.cmNotify.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        //private AxAXVLC.AxVLCPlugin2 axVLCPlugin21;
        private System.Windows.Forms.Button button1;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.NotifyIcon niUser;
        private System.Windows.Forms.ContextMenuStrip cmNotify;
        private System.Windows.Forms.ToolStripMenuItem tsmLogin;
        private ctrlUserInfo ctrlUserInfo1;
        private System.Windows.Forms.Integration.ElementHost ehVideos;
        private System.Windows.Forms.Button button2;
    }
}

