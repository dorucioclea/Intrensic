namespace Intrensic
{
    partial class frmUserMainScreen
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserMainScreen));
			this.pnlLogo = new System.Windows.Forms.Panel();
			this.pbLogo = new System.Windows.Forms.PictureBox();
			this.pnlLeft = new System.Windows.Forms.Panel();
			this.pnlRight = new System.Windows.Forms.Panel();
			this.pnlTop = new System.Windows.Forms.Panel();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.pnlPlaceHolder = new Intrensic.MyPanel();
			this.crtUserMenu = new Intrensic.crtUserMenu();
			this.pnlLogo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlLogo
			// 
			this.pnlLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(140)))), ((int)(((byte)(255)))));
			this.pnlLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.pnlLogo.Controls.Add(this.pbLogo);
			this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlLogo.Location = new System.Drawing.Point(0, 0);
			this.pnlLogo.Margin = new System.Windows.Forms.Padding(0);
			this.pnlLogo.Name = "pnlLogo";
			this.pnlLogo.Size = new System.Drawing.Size(1354, 65);
			this.pnlLogo.TabIndex = 0;
			// 
			// pbLogo
			// 
			this.pbLogo.Image = global::Intrensic.Properties.Resources.Header_logo;
			this.pbLogo.Location = new System.Drawing.Point(27, 15);
			this.pbLogo.Margin = new System.Windows.Forms.Padding(0);
			this.pbLogo.Name = "pbLogo";
			this.pbLogo.Size = new System.Drawing.Size(143, 35);
			this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbLogo.TabIndex = 0;
			this.pbLogo.TabStop = false;
			// 
			// pnlLeft
			// 
			this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlLeft.Location = new System.Drawing.Point(200, 65);
			this.pnlLeft.Margin = new System.Windows.Forms.Padding(0);
			this.pnlLeft.MaximumSize = new System.Drawing.Size(15, 0);
			this.pnlLeft.Name = "pnlLeft";
			this.pnlLeft.Size = new System.Drawing.Size(15, 668);
			this.pnlLeft.TabIndex = 4;
			// 
			// pnlRight
			// 
			this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlRight.Location = new System.Drawing.Point(1339, 65);
			this.pnlRight.Margin = new System.Windows.Forms.Padding(0);
			this.pnlRight.MaximumSize = new System.Drawing.Size(15, 0);
			this.pnlRight.Name = "pnlRight";
			this.pnlRight.Size = new System.Drawing.Size(15, 668);
			this.pnlRight.TabIndex = 5;
			// 
			// pnlTop
			// 
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(215, 65);
			this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
			this.pnlTop.MaximumSize = new System.Drawing.Size(0, 15);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(1124, 15);
			this.pnlTop.TabIndex = 6;
			// 
			// pnlBottom
			// 
			this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBottom.Location = new System.Drawing.Point(215, 718);
			this.pnlBottom.Margin = new System.Windows.Forms.Padding(0);
			this.pnlBottom.MaximumSize = new System.Drawing.Size(0, 15);
			this.pnlBottom.Name = "pnlBottom";
			this.pnlBottom.Size = new System.Drawing.Size(1124, 15);
			this.pnlBottom.TabIndex = 7;
			// 
			// pnlPlaceHolder
			// 
			this.pnlPlaceHolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.pnlPlaceHolder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlPlaceHolder.Location = new System.Drawing.Point(215, 80);
			this.pnlPlaceHolder.Margin = new System.Windows.Forms.Padding(0);
			this.pnlPlaceHolder.Name = "pnlPlaceHolder";
			this.pnlPlaceHolder.Size = new System.Drawing.Size(1124, 638);
			this.pnlPlaceHolder.TabIndex = 1;
			// 
			// crtUserMenu
			// 
			this.crtUserMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(48)))), ((int)(((byte)(86)))));
			this.crtUserMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.crtUserMenu.Dock = System.Windows.Forms.DockStyle.Left;
			this.crtUserMenu.Location = new System.Drawing.Point(0, 65);
			this.crtUserMenu.Name = "crtUserMenu";
			this.crtUserMenu.Size = new System.Drawing.Size(200, 668);
			this.crtUserMenu.TabIndex = 0;
			// 
			// frmUserMainScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.ClientSize = new System.Drawing.Size(1354, 733);
			this.Controls.Add(this.pnlPlaceHolder);
			this.Controls.Add(this.pnlBottom);
			this.Controls.Add(this.pnlTop);
			this.Controls.Add(this.pnlLeft);
			this.Controls.Add(this.pnlRight);
			this.Controls.Add(this.crtUserMenu);
			this.Controls.Add(this.pnlLogo);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmUserMainScreen";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Intrensic";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUserMainScreen_FormClosing);
			this.Load += new System.EventHandler(this.frmUserMainScreen_Load);
			this.SizeChanged += new System.EventHandler(this.frmUserMainScreen_SizeChanged);
			this.pnlLogo.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.PictureBox pbLogo;
        private crtUserMenu crtUserMenu;
        private MyPanel pnlPlaceHolder;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
    }
}