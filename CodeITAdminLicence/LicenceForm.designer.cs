namespace CodeITAdminLicence
{
    partial class LicenceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenceForm));
            this.btnGenerate = new System.Windows.Forms.Button();
            this.dtpUntil = new System.Windows.Forms.DateTimePicker();
            this.txtNumberOfUsers = new System.Windows.Forms.TextBox();
            this.lblNumberInfo = new System.Windows.Forms.Label();
            this.grpSqlServer = new System.Windows.Forms.GroupBox();
            this.chkDefault = new System.Windows.Forms.CheckBox();
            this.lblSqlServerPassword = new System.Windows.Forms.Label();
            this.txtSqlServerPassword = new System.Windows.Forms.TextBox();
            this.lblSqlServerUsername = new System.Windows.Forms.Label();
            this.txtSqlServerUsername = new System.Windows.Forms.TextBox();
            this.lblSqlServerDatabaseName = new System.Windows.Forms.Label();
            this.txtSqlServerDatabaseName = new System.Windows.Forms.TextBox();
            this.lblSqlServerName = new System.Windows.Forms.Label();
            this.txtSqlServerName = new System.Windows.Forms.TextBox();
            this.grpStorage = new System.Windows.Forms.GroupBox();
            this.radCloud = new System.Windows.Forms.RadioButton();
            this.radLocal = new System.Windows.Forms.RadioButton();
            this.pnlLocal = new System.Windows.Forms.Panel();
            this.lblLocalPassword = new System.Windows.Forms.Label();
            this.txtLocalPassword = new System.Windows.Forms.TextBox();
            this.lblLocalUsername = new System.Windows.Forms.Label();
            this.txtLocalUsername = new System.Windows.Forms.TextBox();
            this.lblLocalPath = new System.Windows.Forms.Label();
            this.txtLocalPath = new System.Windows.Forms.TextBox();
            this.pnlCloud = new System.Windows.Forms.Panel();
            this.lblCloudSecret = new System.Windows.Forms.Label();
            this.lblCloudId = new System.Windows.Forms.Label();
            this.txtCloudSecret = new System.Windows.Forms.TextBox();
            this.txtCloudId = new System.Windows.Forms.TextBox();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtAdminName = new System.Windows.Forms.TextBox();
            this.txtAdminUsername = new System.Windows.Forms.TextBox();
            this.txtAdminPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grpSqlServer.SuspendLayout();
            this.grpStorage.SuspendLayout();
            this.pnlLocal.SuspendLayout();
            this.pnlCloud.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(370, 263);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(171, 29);
            this.btnGenerate.TabIndex = 6;
            this.btnGenerate.Text = "Create license";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dtpUntil
            // 
            this.dtpUntil.Checked = false;
            this.dtpUntil.CustomFormat = "MM/dd/yyyy";
            this.dtpUntil.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpUntil.Location = new System.Drawing.Point(7, 19);
            this.dtpUntil.Name = "dtpUntil";
            this.dtpUntil.ShowCheckBox = true;
            this.dtpUntil.Size = new System.Drawing.Size(145, 20);
            this.dtpUntil.TabIndex = 2;
            // 
            // txtNumberOfUsers
            // 
            this.txtNumberOfUsers.Location = new System.Drawing.Point(17, 19);
            this.txtNumberOfUsers.Name = "txtNumberOfUsers";
            this.txtNumberOfUsers.Size = new System.Drawing.Size(45, 20);
            this.txtNumberOfUsers.TabIndex = 3;
            this.txtNumberOfUsers.Text = "1";
            this.txtNumberOfUsers.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblNumberInfo
            // 
            this.lblNumberInfo.AutoSize = true;
            this.lblNumberInfo.Location = new System.Drawing.Point(68, 22);
            this.lblNumberInfo.Name = "lblNumberInfo";
            this.lblNumberInfo.Size = new System.Drawing.Size(32, 13);
            this.lblNumberInfo.TabIndex = 12;
            this.lblNumberInfo.Text = "users";
            // 
            // grpSqlServer
            // 
            this.grpSqlServer.Controls.Add(this.chkDefault);
            this.grpSqlServer.Controls.Add(this.lblSqlServerPassword);
            this.grpSqlServer.Controls.Add(this.txtSqlServerPassword);
            this.grpSqlServer.Controls.Add(this.lblSqlServerUsername);
            this.grpSqlServer.Controls.Add(this.txtSqlServerUsername);
            this.grpSqlServer.Controls.Add(this.lblSqlServerDatabaseName);
            this.grpSqlServer.Controls.Add(this.txtSqlServerDatabaseName);
            this.grpSqlServer.Controls.Add(this.lblSqlServerName);
            this.grpSqlServer.Controls.Add(this.txtSqlServerName);
            this.grpSqlServer.Location = new System.Drawing.Point(295, 12);
            this.grpSqlServer.Name = "grpSqlServer";
            this.grpSqlServer.Size = new System.Drawing.Size(302, 238);
            this.grpSqlServer.TabIndex = 4;
            this.grpSqlServer.TabStop = false;
            this.grpSqlServer.Text = "MS SQL Server Configuration";
            // 
            // chkDefault
            // 
            this.chkDefault.AutoSize = true;
            this.chkDefault.Location = new System.Drawing.Point(207, 21);
            this.chkDefault.Name = "chkDefault";
            this.chkDefault.Size = new System.Drawing.Size(80, 17);
            this.chkDefault.TabIndex = 16;
            this.chkDefault.Text = "Use default";
            this.chkDefault.UseVisualStyleBackColor = true;
            this.chkDefault.CheckedChanged += new System.EventHandler(this.chkDefault_CheckedChanged);
            // 
            // lblSqlServerPassword
            // 
            this.lblSqlServerPassword.AutoSize = true;
            this.lblSqlServerPassword.Location = new System.Drawing.Point(6, 179);
            this.lblSqlServerPassword.Name = "lblSqlServerPassword";
            this.lblSqlServerPassword.Size = new System.Drawing.Size(127, 13);
            this.lblSqlServerPassword.TabIndex = 15;
            this.lblSqlServerPassword.Text = "MSSQL Server Password";
            // 
            // txtSqlServerPassword
            // 
            this.txtSqlServerPassword.Location = new System.Drawing.Point(9, 195);
            this.txtSqlServerPassword.Name = "txtSqlServerPassword";
            this.txtSqlServerPassword.PasswordChar = '*';
            this.txtSqlServerPassword.Size = new System.Drawing.Size(269, 20);
            this.txtSqlServerPassword.TabIndex = 4;
            this.txtSqlServerPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSqlServerUsername
            // 
            this.lblSqlServerUsername.AutoSize = true;
            this.lblSqlServerUsername.Location = new System.Drawing.Point(6, 139);
            this.lblSqlServerUsername.Name = "lblSqlServerUsername";
            this.lblSqlServerUsername.Size = new System.Drawing.Size(129, 13);
            this.lblSqlServerUsername.TabIndex = 13;
            this.lblSqlServerUsername.Text = "MSSQL Server Username";
            // 
            // txtSqlServerUsername
            // 
            this.txtSqlServerUsername.Location = new System.Drawing.Point(9, 156);
            this.txtSqlServerUsername.Name = "txtSqlServerUsername";
            this.txtSqlServerUsername.Size = new System.Drawing.Size(269, 20);
            this.txtSqlServerUsername.TabIndex = 3;
            this.txtSqlServerUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSqlServerDatabaseName
            // 
            this.lblSqlServerDatabaseName.AutoSize = true;
            this.lblSqlServerDatabaseName.Location = new System.Drawing.Point(6, 98);
            this.lblSqlServerDatabaseName.Name = "lblSqlServerDatabaseName";
            this.lblSqlServerDatabaseName.Size = new System.Drawing.Size(158, 13);
            this.lblSqlServerDatabaseName.TabIndex = 11;
            this.lblSqlServerDatabaseName.Text = "MSSQL Server Database Name";
            // 
            // txtSqlServerDatabaseName
            // 
            this.txtSqlServerDatabaseName.Location = new System.Drawing.Point(9, 114);
            this.txtSqlServerDatabaseName.Name = "txtSqlServerDatabaseName";
            this.txtSqlServerDatabaseName.Size = new System.Drawing.Size(269, 20);
            this.txtSqlServerDatabaseName.TabIndex = 1;
            this.txtSqlServerDatabaseName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSqlServerName
            // 
            this.lblSqlServerName.AutoSize = true;
            this.lblSqlServerName.Location = new System.Drawing.Point(6, 57);
            this.lblSqlServerName.Name = "lblSqlServerName";
            this.lblSqlServerName.Size = new System.Drawing.Size(169, 13);
            this.lblSqlServerName.TabIndex = 9;
            this.lblSqlServerName.Text = "MSSQL Server Name (IP Address)";
            // 
            // txtSqlServerName
            // 
            this.txtSqlServerName.Location = new System.Drawing.Point(9, 73);
            this.txtSqlServerName.Name = "txtSqlServerName";
            this.txtSqlServerName.Size = new System.Drawing.Size(269, 20);
            this.txtSqlServerName.TabIndex = 0;
            // 
            // grpStorage
            // 
            this.grpStorage.Controls.Add(this.radCloud);
            this.grpStorage.Controls.Add(this.radLocal);
            this.grpStorage.Controls.Add(this.pnlLocal);
            this.grpStorage.Controls.Add(this.pnlCloud);
            this.grpStorage.Location = new System.Drawing.Point(603, 63);
            this.grpStorage.Name = "grpStorage";
            this.grpStorage.Size = new System.Drawing.Size(341, 187);
            this.grpStorage.TabIndex = 5;
            this.grpStorage.TabStop = false;
            this.grpStorage.Text = "Storage Configuration";
            // 
            // radCloud
            // 
            this.radCloud.AutoSize = true;
            this.radCloud.Location = new System.Drawing.Point(104, 20);
            this.radCloud.Name = "radCloud";
            this.radCloud.Size = new System.Drawing.Size(92, 17);
            this.radCloud.TabIndex = 1;
            this.radCloud.Text = "Cloud Storage";
            this.radCloud.UseVisualStyleBackColor = true;
            // 
            // radLocal
            // 
            this.radLocal.AutoSize = true;
            this.radLocal.Checked = true;
            this.radLocal.Location = new System.Drawing.Point(7, 20);
            this.radLocal.Name = "radLocal";
            this.radLocal.Size = new System.Drawing.Size(91, 17);
            this.radLocal.TabIndex = 0;
            this.radLocal.TabStop = true;
            this.radLocal.Text = "Local Storage";
            this.radLocal.UseVisualStyleBackColor = true;
            this.radLocal.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);
            // 
            // pnlLocal
            // 
            this.pnlLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLocal.Controls.Add(this.lblLocalPassword);
            this.pnlLocal.Controls.Add(this.txtLocalPassword);
            this.pnlLocal.Controls.Add(this.lblLocalUsername);
            this.pnlLocal.Controls.Add(this.txtLocalUsername);
            this.pnlLocal.Controls.Add(this.lblLocalPath);
            this.pnlLocal.Controls.Add(this.txtLocalPath);
            this.pnlLocal.Location = new System.Drawing.Point(7, 40);
            this.pnlLocal.Name = "pnlLocal";
            this.pnlLocal.Size = new System.Drawing.Size(328, 141);
            this.pnlLocal.TabIndex = 2;
            // 
            // lblLocalPassword
            // 
            this.lblLocalPassword.AutoSize = true;
            this.lblLocalPassword.Location = new System.Drawing.Point(3, 90);
            this.lblLocalPassword.Name = "lblLocalPassword";
            this.lblLocalPassword.Size = new System.Drawing.Size(53, 13);
            this.lblLocalPassword.TabIndex = 15;
            this.lblLocalPassword.Text = "Password";
            // 
            // txtLocalPassword
            // 
            this.txtLocalPassword.Location = new System.Drawing.Point(6, 106);
            this.txtLocalPassword.Name = "txtLocalPassword";
            this.txtLocalPassword.PasswordChar = '*';
            this.txtLocalPassword.Size = new System.Drawing.Size(317, 20);
            this.txtLocalPassword.TabIndex = 14;
            this.txtLocalPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblLocalUsername
            // 
            this.lblLocalUsername.AutoSize = true;
            this.lblLocalUsername.Location = new System.Drawing.Point(3, 48);
            this.lblLocalUsername.Name = "lblLocalUsername";
            this.lblLocalUsername.Size = new System.Drawing.Size(55, 13);
            this.lblLocalUsername.TabIndex = 13;
            this.lblLocalUsername.Text = "Username";
            // 
            // txtLocalUsername
            // 
            this.txtLocalUsername.Location = new System.Drawing.Point(6, 64);
            this.txtLocalUsername.Name = "txtLocalUsername";
            this.txtLocalUsername.Size = new System.Drawing.Size(317, 20);
            this.txtLocalUsername.TabIndex = 12;
            this.txtLocalUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblLocalPath
            // 
            this.lblLocalPath.AutoSize = true;
            this.lblLocalPath.Location = new System.Drawing.Point(3, 7);
            this.lblLocalPath.Name = "lblLocalPath";
            this.lblLocalPath.Size = new System.Drawing.Size(160, 13);
            this.lblLocalPath.TabIndex = 11;
            this.lblLocalPath.Text = "Path to network storage location";
            // 
            // txtLocalPath
            // 
            this.txtLocalPath.Location = new System.Drawing.Point(6, 23);
            this.txtLocalPath.Name = "txtLocalPath";
            this.txtLocalPath.Size = new System.Drawing.Size(317, 20);
            this.txtLocalPath.TabIndex = 0;
            // 
            // pnlCloud
            // 
            this.pnlCloud.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCloud.Controls.Add(this.lblCloudSecret);
            this.pnlCloud.Controls.Add(this.lblCloudId);
            this.pnlCloud.Controls.Add(this.txtCloudSecret);
            this.pnlCloud.Controls.Add(this.txtCloudId);
            this.pnlCloud.Location = new System.Drawing.Point(7, 43);
            this.pnlCloud.Name = "pnlCloud";
            this.pnlCloud.Size = new System.Drawing.Size(328, 107);
            this.pnlCloud.TabIndex = 3;
            this.pnlCloud.Visible = false;
            // 
            // lblCloudSecret
            // 
            this.lblCloudSecret.AutoSize = true;
            this.lblCloudSecret.Location = new System.Drawing.Point(6, 48);
            this.lblCloudSecret.Name = "lblCloudSecret";
            this.lblCloudSecret.Size = new System.Drawing.Size(97, 13);
            this.lblCloudSecret.TabIndex = 11;
            this.lblCloudSecret.Text = "Secret Access Key";
            // 
            // lblCloudId
            // 
            this.lblCloudId.AutoSize = true;
            this.lblCloudId.Location = new System.Drawing.Point(6, 7);
            this.lblCloudId.Name = "lblCloudId";
            this.lblCloudId.Size = new System.Drawing.Size(75, 13);
            this.lblCloudId.TabIndex = 11;
            this.lblCloudId.Text = "Access Key Id";
            // 
            // txtCloudSecret
            // 
            this.txtCloudSecret.Location = new System.Drawing.Point(6, 64);
            this.txtCloudSecret.Name = "txtCloudSecret";
            this.txtCloudSecret.Size = new System.Drawing.Size(317, 20);
            this.txtCloudSecret.TabIndex = 1;
            this.txtCloudSecret.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCloudId
            // 
            this.txtCloudId.Location = new System.Drawing.Point(6, 23);
            this.txtCloudId.Name = "txtCloudId";
            this.txtCloudId.Size = new System.Drawing.Size(317, 20);
            this.txtCloudId.TabIndex = 0;
            this.txtCloudId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtClientId
            // 
            this.txtClientId.Location = new System.Drawing.Point(18, 19);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Size = new System.Drawing.Size(253, 20);
            this.txtClientId.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtClientId);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(277, 67);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Licence for Client ID";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dtpUntil);
            this.groupBox2.Location = new System.Drawing.Point(603, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(170, 50);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Valid until";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtNumberOfUsers);
            this.groupBox3.Controls.Add(this.lblNumberInfo);
            this.groupBox3.Location = new System.Drawing.Point(793, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(151, 50);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Valid for";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.txtAdminPassword);
            this.groupBox4.Controls.Add(this.txtAdminUsername);
            this.groupBox4.Controls.Add(this.txtAdminName);
            this.groupBox4.Location = new System.Drawing.Point(12, 85);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(277, 165);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Admin credentials";
            // 
            // txtAdminName
            // 
            this.txtAdminName.Location = new System.Drawing.Point(18, 39);
            this.txtAdminName.Name = "txtAdminName";
            this.txtAdminName.Size = new System.Drawing.Size(253, 20);
            this.txtAdminName.TabIndex = 1;
            this.txtAdminName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtAdminUsername
            // 
            this.txtAdminUsername.Location = new System.Drawing.Point(18, 80);
            this.txtAdminUsername.Name = "txtAdminUsername";
            this.txtAdminUsername.Size = new System.Drawing.Size(253, 20);
            this.txtAdminUsername.TabIndex = 2;
            this.txtAdminUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtAdminPassword
            // 
            this.txtAdminPassword.Location = new System.Drawing.Point(18, 122);
            this.txtAdminPassword.Name = "txtAdminPassword";
            this.txtAdminPassword.PasswordChar = '*';
            this.txtAdminPassword.Size = new System.Drawing.Size(253, 20);
            this.txtAdminPassword.TabIndex = 3;
            this.txtAdminPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Password";
            // 
            // LicenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 304);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpStorage);
            this.Controls.Add(this.grpSqlServer);
            this.Controls.Add(this.btnGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LicenceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Intrensic License Generator";
            this.Load += new System.EventHandler(this.GoPro911Licence_Load);
            this.grpSqlServer.ResumeLayout(false);
            this.grpSqlServer.PerformLayout();
            this.grpStorage.ResumeLayout(false);
            this.grpStorage.PerformLayout();
            this.pnlLocal.ResumeLayout(false);
            this.pnlLocal.PerformLayout();
            this.pnlCloud.ResumeLayout(false);
            this.pnlCloud.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.DateTimePicker dtpUntil;
        private System.Windows.Forms.TextBox txtNumberOfUsers;
        private System.Windows.Forms.Label lblNumberInfo;
        private System.Windows.Forms.GroupBox grpSqlServer;
        private System.Windows.Forms.Label lblSqlServerPassword;
        private System.Windows.Forms.TextBox txtSqlServerPassword;
        private System.Windows.Forms.Label lblSqlServerUsername;
        private System.Windows.Forms.TextBox txtSqlServerUsername;
        private System.Windows.Forms.Label lblSqlServerDatabaseName;
        private System.Windows.Forms.TextBox txtSqlServerDatabaseName;
        private System.Windows.Forms.Label lblSqlServerName;
        private System.Windows.Forms.TextBox txtSqlServerName;
        private System.Windows.Forms.GroupBox grpStorage;
        private System.Windows.Forms.RadioButton radLocal;
        private System.Windows.Forms.RadioButton radCloud;
        private System.Windows.Forms.Label lblLocalPath;
        private System.Windows.Forms.TextBox txtLocalPath;
        private System.Windows.Forms.Panel pnlCloud;
        private System.Windows.Forms.Label lblCloudId;
        private System.Windows.Forms.TextBox txtCloudId;
        private System.Windows.Forms.Label lblCloudSecret;
        private System.Windows.Forms.TextBox txtCloudSecret;
        private System.Windows.Forms.TextBox txtClientId;
        private System.Windows.Forms.Label lblLocalPassword;
        private System.Windows.Forms.TextBox txtLocalPassword;
        private System.Windows.Forms.Label lblLocalUsername;
        private System.Windows.Forms.TextBox txtLocalUsername;
        private System.Windows.Forms.Panel pnlLocal;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkDefault;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtAdminUsername;
        private System.Windows.Forms.TextBox txtAdminName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAdminPassword;
    }
}

