namespace Intrensic
{
    partial class frmLogIn
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogIn));
			this.btnCancel = new System.Windows.Forms.Button();
			this.cmNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiShowProgress = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmLogin = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmLogout = new System.Windows.Forms.ToolStripMenuItem();
			this.niUser = new System.Windows.Forms.NotifyIcon(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.btnLogin = new System.Windows.Forms.Button();
			this.cmNotify.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.Transparent;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatAppearance.BorderSize = 0;
			this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Location = new System.Drawing.Point(976, 446);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(0, 0);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.TabStop = false;
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// cmNotify
			// 
			this.cmNotify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(16)))), ((int)(((byte)(59)))));
			this.cmNotify.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.cmNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiShowProgress,
            this.tsmLogin,
            this.tsmLogout});
			this.cmNotify.Name = "cmNotify";
			this.cmNotify.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.cmNotify.ShowImageMargin = false;
			this.cmNotify.Size = new System.Drawing.Size(127, 70);
			// 
			// tsmiShowProgress
			// 
			this.tsmiShowProgress.ForeColor = System.Drawing.Color.White;
			this.tsmiShowProgress.Name = "tsmiShowProgress";
			this.tsmiShowProgress.Size = new System.Drawing.Size(126, 22);
			this.tsmiShowProgress.Text = "Show Progress";
			this.tsmiShowProgress.Visible = false;
			this.tsmiShowProgress.Click += new System.EventHandler(this.tsmiShowProgress_Click);
			// 
			// tsmLogin
			// 
			this.tsmLogin.ForeColor = System.Drawing.Color.White;
			this.tsmLogin.Name = "tsmLogin";
			this.tsmLogin.Size = new System.Drawing.Size(126, 22);
			this.tsmLogin.Text = "Log In";
			this.tsmLogin.Click += new System.EventHandler(this.tsmLogin_Click);
			// 
			// tsmLogout
			// 
			this.tsmLogout.ForeColor = System.Drawing.Color.White;
			this.tsmLogout.Name = "tsmLogout";
			this.tsmLogout.Size = new System.Drawing.Size(126, 22);
			this.tsmLogout.Text = "Log Out";
			this.tsmLogout.Visible = false;
			this.tsmLogout.Click += new System.EventHandler(this.tsmLogout_Click);
			// 
			// niUser
			// 
			this.niUser.ContextMenuStrip = this.cmNotify;
			this.niUser.Icon = ((System.Drawing.Icon)(resources.GetObject("niUser.Icon")));
			this.niUser.Text = "Intrensic";
			this.niUser.Visible = true;
			this.niUser.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.niUser_MouseDoubleClick);
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(421, 275);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 20);
			this.label1.TabIndex = 3;
			this.label1.Text = "Username:";
			// 
			// txtUserName
			// 
			this.txtUserName.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.txtUserName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.txtUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtUserName.ForeColor = System.Drawing.Color.White;
			this.txtUserName.Location = new System.Drawing.Point(530, 272);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(149, 26);
			this.txtUserName.TabIndex = 0;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Image = global::Intrensic.Properties.Resources.login_logo;
			this.pictureBox1.Location = new System.Drawing.Point(534, 148);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(145, 73);
			this.pictureBox1.TabIndex = 11;
			this.pictureBox1.TabStop = false;
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(421, 318);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(91, 20);
			this.label2.TabIndex = 4;
			this.label2.Text = "Password:";
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtPassword.ForeColor = System.Drawing.Color.White;
			this.txtPassword.Location = new System.Drawing.Point(530, 315);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '○';
			this.txtPassword.Size = new System.Drawing.Size(149, 26);
			this.txtPassword.TabIndex = 1;
			this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
			// 
			// btnLogin
			// 
			this.btnLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnLogin.BackColor = System.Drawing.Color.White;
			this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.btnLogin.Location = new System.Drawing.Point(530, 365);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(149, 32);
			this.btnLogin.TabIndex = 2;
			this.btnLogin.Text = "Log In";
			this.btnLogin.UseVisualStyleBackColor = false;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click_1);
			// 
			// frmLogIn
			// 
			this.AcceptButton = this.btnLogin;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::Intrensic.Properties.Resources.login_background2;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(1163, 562);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.txtUserName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmLogIn";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Login";
			this.Shown += new System.EventHandler(this.frmLogIn_Shown);
			this.cmNotify.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ContextMenuStrip cmNotify;
        private System.Windows.Forms.ToolStripMenuItem tsmLogin;
        private System.Windows.Forms.NotifyIcon niUser;
        private System.Windows.Forms.ToolStripMenuItem tsmLogout;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowProgress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
    }
}