namespace Intrensic.Administration
{
    partial class frmUsers
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
			this.ofdPicture = new System.Windows.Forms.OpenFileDialog();
			this.gbUserManagement = new System.Windows.Forms.GroupBox();
			this.rbSergeant = new System.Windows.Forms.RadioButton();
			this.rbProsecutor = new System.Windows.Forms.RadioButton();
			this.rbOfficeManager = new System.Windows.Forms.RadioButton();
			this.rbInvestigator = new System.Windows.Forms.RadioButton();
			this.btnMapDevice = new System.Windows.Forms.Button();
			this.txtDeviceId = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.txtID = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.rbAdministrator = new System.Windows.Forms.RadioButton();
			this.rbUser = new System.Windows.Forms.RadioButton();
			this.label5 = new System.Windows.Forms.Label();
			this.txtLastName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtMiddleName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtFirstName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.gbUserActions = new System.Windows.Forms.GroupBox();
			this.btnDelete = new System.Windows.Forms.Button();
			this.lvUsers = new System.Windows.Forms.ListView();
			this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colUserName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnEdit = new System.Windows.Forms.Button();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.btnCreate = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.btnSearch = new System.Windows.Forms.Button();
			this.gbUserManagement.SuspendLayout();
			this.gbUserActions.SuspendLayout();
			this.SuspendLayout();
			// 
			// ofdPicture
			// 
			this.ofdPicture.Filter = "\"JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF F" +
    "iles (*.gif)|*.gif|BITMAP Files (*.bmp)|*.bmp\"";
			this.ofdPicture.Title = "Please Choose User Image";
			// 
			// gbUserManagement
			// 
			this.gbUserManagement.BackColor = System.Drawing.Color.Transparent;
			this.gbUserManagement.Controls.Add(this.rbSergeant);
			this.gbUserManagement.Controls.Add(this.rbProsecutor);
			this.gbUserManagement.Controls.Add(this.rbOfficeManager);
			this.gbUserManagement.Controls.Add(this.rbInvestigator);
			this.gbUserManagement.Controls.Add(this.btnMapDevice);
			this.gbUserManagement.Controls.Add(this.txtDeviceId);
			this.gbUserManagement.Controls.Add(this.label10);
			this.gbUserManagement.Controls.Add(this.txtID);
			this.gbUserManagement.Controls.Add(this.label9);
			this.gbUserManagement.Controls.Add(this.label8);
			this.gbUserManagement.Controls.Add(this.btnClose);
			this.gbUserManagement.Controls.Add(this.btnClear);
			this.gbUserManagement.Controls.Add(this.btnSave);
			this.gbUserManagement.Controls.Add(this.txtPassword);
			this.gbUserManagement.Controls.Add(this.label7);
			this.gbUserManagement.Controls.Add(this.txtUserName);
			this.gbUserManagement.Controls.Add(this.label6);
			this.gbUserManagement.Controls.Add(this.rbAdministrator);
			this.gbUserManagement.Controls.Add(this.rbUser);
			this.gbUserManagement.Controls.Add(this.label5);
			this.gbUserManagement.Controls.Add(this.txtLastName);
			this.gbUserManagement.Controls.Add(this.label4);
			this.gbUserManagement.Controls.Add(this.txtMiddleName);
			this.gbUserManagement.Controls.Add(this.label3);
			this.gbUserManagement.Controls.Add(this.txtFirstName);
			this.gbUserManagement.Controls.Add(this.label2);
			this.gbUserManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.gbUserManagement.ForeColor = System.Drawing.Color.White;
			this.gbUserManagement.Location = new System.Drawing.Point(303, 1);
			this.gbUserManagement.Name = "gbUserManagement";
			this.gbUserManagement.Size = new System.Drawing.Size(513, 588);
			this.gbUserManagement.TabIndex = 8;
			this.gbUserManagement.TabStop = false;
			this.gbUserManagement.Text = "User Management";
			this.gbUserManagement.UseCompatibleTextRendering = true;
			// 
			// rbSergeant
			// 
			this.rbSergeant.AutoSize = true;
			this.rbSergeant.ForeColor = System.Drawing.Color.White;
			this.rbSergeant.Location = new System.Drawing.Point(94, 246);
			this.rbSergeant.Name = "rbSergeant";
			this.rbSergeant.Size = new System.Drawing.Size(68, 17);
			this.rbSergeant.TabIndex = 30;
			this.rbSergeant.Text = "Sergeant";
			this.rbSergeant.UseVisualStyleBackColor = true;
			// 
			// rbProsecutor
			// 
			this.rbProsecutor.AutoSize = true;
			this.rbProsecutor.ForeColor = System.Drawing.Color.White;
			this.rbProsecutor.Location = new System.Drawing.Point(94, 223);
			this.rbProsecutor.Name = "rbProsecutor";
			this.rbProsecutor.Size = new System.Drawing.Size(76, 17);
			this.rbProsecutor.TabIndex = 29;
			this.rbProsecutor.Text = "Prosecutor";
			this.rbProsecutor.UseVisualStyleBackColor = true;
			// 
			// rbOfficeManager
			// 
			this.rbOfficeManager.AutoSize = true;
			this.rbOfficeManager.ForeColor = System.Drawing.Color.White;
			this.rbOfficeManager.Location = new System.Drawing.Point(94, 200);
			this.rbOfficeManager.Name = "rbOfficeManager";
			this.rbOfficeManager.Size = new System.Drawing.Size(86, 17);
			this.rbOfficeManager.TabIndex = 28;
			this.rbOfficeManager.Text = "Patrol Officer";
			this.rbOfficeManager.UseVisualStyleBackColor = true;
			// 
			// rbInvestigator
			// 
			this.rbInvestigator.AutoSize = true;
			this.rbInvestigator.ForeColor = System.Drawing.Color.White;
			this.rbInvestigator.Location = new System.Drawing.Point(94, 177);
			this.rbInvestigator.Name = "rbInvestigator";
			this.rbInvestigator.Size = new System.Drawing.Size(80, 17);
			this.rbInvestigator.TabIndex = 27;
			this.rbInvestigator.Text = "Investigator";
			this.rbInvestigator.UseVisualStyleBackColor = true;
			// 
			// btnMapDevice
			// 
			this.btnMapDevice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.btnMapDevice.FlatAppearance.BorderSize = 0;
			this.btnMapDevice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnMapDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnMapDevice.ForeColor = System.Drawing.Color.White;
			this.btnMapDevice.Location = new System.Drawing.Point(270, 391);
			this.btnMapDevice.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.btnMapDevice.Name = "btnMapDevice";
			this.btnMapDevice.Size = new System.Drawing.Size(209, 20);
			this.btnMapDevice.TabIndex = 26;
			this.btnMapDevice.Text = "Map Attached Device To User";
			this.btnMapDevice.UseVisualStyleBackColor = false;
			this.btnMapDevice.Click += new System.EventHandler(this.btnMapDevice_Click);
			// 
			// txtDeviceId
			// 
			this.txtDeviceId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.txtDeviceId.ForeColor = System.Drawing.Color.White;
			this.txtDeviceId.Location = new System.Drawing.Point(94, 391);
			this.txtDeviceId.Name = "txtDeviceId";
			this.txtDeviceId.ReadOnly = true;
			this.txtDeviceId.Size = new System.Drawing.Size(170, 20);
			this.txtDeviceId.TabIndex = 24;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.ForeColor = System.Drawing.Color.White;
			this.label10.Location = new System.Drawing.Point(16, 393);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(58, 13);
			this.label10.TabIndex = 25;
			this.label10.Text = "Device ID:";
			// 
			// txtID
			// 
			this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.txtID.ForeColor = System.Drawing.Color.White;
			this.txtID.Location = new System.Drawing.Point(94, 17);
			this.txtID.Name = "txtID";
			this.txtID.Size = new System.Drawing.Size(170, 20);
			this.txtID.TabIndex = 0;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.ForeColor = System.Drawing.Color.White;
			this.label9.Location = new System.Drawing.Point(16, 19);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(18, 13);
			this.label9.TabIndex = 23;
			this.label9.Text = "ID";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.ForeColor = System.Drawing.Color.White;
			this.label8.Location = new System.Drawing.Point(16, 406);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(65, 13);
			this.label8.TabIndex = 20;
			this.label8.Text = "User Picture";
			this.label8.Visible = false;
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.btnClose.FlatAppearance.BorderSize = 0;
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnClose.ForeColor = System.Drawing.Color.White;
			this.btnClose.Location = new System.Drawing.Point(445, 557);
			this.btnClose.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(61, 20);
			this.btnClose.TabIndex = 10;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnClear
			// 
			this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.btnClear.FlatAppearance.BorderSize = 0;
			this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnClear.ForeColor = System.Drawing.Color.White;
			this.btnClear.Location = new System.Drawing.Point(344, 557);
			this.btnClear.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(61, 20);
			this.btnClear.TabIndex = 9;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = false;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.btnSave.FlatAppearance.BorderSize = 0;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnSave.ForeColor = System.Drawing.Color.White;
			this.btnSave.Location = new System.Drawing.Point(241, 557);
			this.btnSave.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(61, 20);
			this.btnSave.TabIndex = 8;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = false;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// txtPassword
			// 
			this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.txtPassword.ForeColor = System.Drawing.Color.White;
			this.txtPassword.Location = new System.Drawing.Point(94, 304);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(170, 20);
			this.txtPassword.TabIndex = 7;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.ForeColor = System.Drawing.Color.White;
			this.label7.Location = new System.Drawing.Point(16, 306);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 13);
			this.label7.TabIndex = 15;
			this.label7.Text = "Password";
			// 
			// txtUserName
			// 
			this.txtUserName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.txtUserName.ForeColor = System.Drawing.Color.White;
			this.txtUserName.Location = new System.Drawing.Point(94, 278);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(170, 20);
			this.txtUserName.TabIndex = 6;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.ForeColor = System.Drawing.Color.White;
			this.label6.Location = new System.Drawing.Point(16, 280);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(60, 13);
			this.label6.TabIndex = 13;
			this.label6.Text = "User Name";
			// 
			// rbAdministrator
			// 
			this.rbAdministrator.AutoSize = true;
			this.rbAdministrator.ForeColor = System.Drawing.Color.White;
			this.rbAdministrator.Location = new System.Drawing.Point(94, 133);
			this.rbAdministrator.Name = "rbAdministrator";
			this.rbAdministrator.Size = new System.Drawing.Size(85, 17);
			this.rbAdministrator.TabIndex = 5;
			this.rbAdministrator.Text = "Administrator";
			this.rbAdministrator.UseVisualStyleBackColor = true;
			// 
			// rbUser
			// 
			this.rbUser.AutoSize = true;
			this.rbUser.Checked = true;
			this.rbUser.ForeColor = System.Drawing.Color.White;
			this.rbUser.Location = new System.Drawing.Point(94, 154);
			this.rbUser.Name = "rbUser";
			this.rbUser.Size = new System.Drawing.Size(47, 17);
			this.rbUser.TabIndex = 4;
			this.rbUser.TabStop = true;
			this.rbUser.Text = "User";
			this.rbUser.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.ForeColor = System.Drawing.Color.White;
			this.label5.Location = new System.Drawing.Point(16, 136);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(29, 13);
			this.label5.TabIndex = 9;
			this.label5.Text = "Role";
			// 
			// txtLastName
			// 
			this.txtLastName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.txtLastName.ForeColor = System.Drawing.Color.White;
			this.txtLastName.Location = new System.Drawing.Point(94, 95);
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.Size = new System.Drawing.Size(170, 20);
			this.txtLastName.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(16, 97);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(58, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Last Name";
			// 
			// txtMiddleName
			// 
			this.txtMiddleName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.txtMiddleName.ForeColor = System.Drawing.Color.White;
			this.txtMiddleName.Location = new System.Drawing.Point(94, 69);
			this.txtMiddleName.Name = "txtMiddleName";
			this.txtMiddleName.Size = new System.Drawing.Size(170, 20);
			this.txtMiddleName.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(16, 71);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(69, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Middle Name";
			// 
			// txtFirstName
			// 
			this.txtFirstName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.txtFirstName.ForeColor = System.Drawing.Color.White;
			this.txtFirstName.Location = new System.Drawing.Point(94, 43);
			this.txtFirstName.Name = "txtFirstName";
			this.txtFirstName.Size = new System.Drawing.Size(170, 20);
			this.txtFirstName.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(16, 45);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "First Name";
			// 
			// gbUserActions
			// 
			this.gbUserActions.BackColor = System.Drawing.Color.Transparent;
			this.gbUserActions.Controls.Add(this.btnDelete);
			this.gbUserActions.Controls.Add(this.lvUsers);
			this.gbUserActions.Controls.Add(this.btnEdit);
			this.gbUserActions.Controls.Add(this.txtSearch);
			this.gbUserActions.Controls.Add(this.btnCreate);
			this.gbUserActions.Controls.Add(this.label1);
			this.gbUserActions.Controls.Add(this.btnSearch);
			this.gbUserActions.ForeColor = System.Drawing.Color.White;
			this.gbUserActions.Location = new System.Drawing.Point(6, 1);
			this.gbUserActions.Name = "gbUserActions";
			this.gbUserActions.Size = new System.Drawing.Size(291, 588);
			this.gbUserActions.TabIndex = 7;
			this.gbUserActions.TabStop = false;
			this.gbUserActions.Text = "User Search";
			// 
			// btnDelete
			// 
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.btnDelete.FlatAppearance.BorderSize = 0;
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnDelete.ForeColor = System.Drawing.Color.White;
			this.btnDelete.Location = new System.Drawing.Point(218, 557);
			this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(61, 20);
			this.btnDelete.TabIndex = 5;
			this.btnDelete.Text = "Delete";
			this.btnDelete.UseVisualStyleBackColor = false;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// lvUsers
			// 
			this.lvUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.lvUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colUserName});
			this.lvUsers.ForeColor = System.Drawing.Color.White;
			this.lvUsers.FullRowSelect = true;
			this.lvUsers.Location = new System.Drawing.Point(9, 44);
			this.lvUsers.Name = "lvUsers";
			this.lvUsers.Size = new System.Drawing.Size(270, 510);
			this.lvUsers.TabIndex = 2;
			this.lvUsers.UseCompatibleStateImageBehavior = false;
			this.lvUsers.View = System.Windows.Forms.View.Details;
			this.lvUsers.SelectedIndexChanged += new System.EventHandler(this.lvUsers_SelectedIndexChanged);
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 150;
			// 
			// colUserName
			// 
			this.colUserName.Text = "UserName";
			this.colUserName.Width = 100;
			// 
			// btnEdit
			// 
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.btnEdit.FlatAppearance.BorderSize = 0;
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnEdit.ForeColor = System.Drawing.Color.White;
			this.btnEdit.Location = new System.Drawing.Point(112, 557);
			this.btnEdit.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(61, 20);
			this.btnEdit.TabIndex = 4;
			this.btnEdit.Text = "Edit";
			this.btnEdit.UseVisualStyleBackColor = false;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// txtSearch
			// 
			this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.txtSearch.ForeColor = System.Drawing.Color.White;
			this.txtSearch.Location = new System.Drawing.Point(42, 18);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(170, 20);
			this.txtSearch.TabIndex = 0;
			// 
			// btnCreate
			// 
			this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.btnCreate.FlatAppearance.BorderSize = 0;
			this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnCreate.ForeColor = System.Drawing.Color.White;
			this.btnCreate.Location = new System.Drawing.Point(9, 557);
			this.btnCreate.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.Size = new System.Drawing.Size(61, 20);
			this.btnCreate.TabIndex = 3;
			this.btnCreate.Text = "Create";
			this.btnCreate.UseVisualStyleBackColor = false;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Name";
			// 
			// btnSearch
			// 
			this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.btnSearch.FlatAppearance.BorderSize = 0;
			this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnSearch.ForeColor = System.Drawing.Color.White;
			this.btnSearch.Location = new System.Drawing.Point(218, 18);
			this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(61, 20);
			this.btnSearch.TabIndex = 1;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = false;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// frmUsers
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(140)))), ((int)(((byte)(255)))));
			this.BackgroundImage = global::Intrensic.Properties.Resources.default_inapp;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.ClientSize = new System.Drawing.Size(825, 595);
			this.Controls.Add(this.gbUserManagement);
			this.Controls.Add(this.gbUserActions);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmUsers";
			this.Text = "Users";
			this.Load += new System.EventHandler(this.frmUsers_Load);
			this.Shown += new System.EventHandler(this.frmUsers_Shown);
			this.gbUserManagement.ResumeLayout(false);
			this.gbUserManagement.PerformLayout();
			this.gbUserActions.ResumeLayout(false);
			this.gbUserActions.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvUsers;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colUserName;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox gbUserActions;
        private System.Windows.Forms.GroupBox gbUserManagement;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMiddleName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rbAdministrator;
        private System.Windows.Forms.RadioButton rbUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.OpenFileDialog ofdPicture;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtDeviceId;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnMapDevice;
        private System.Windows.Forms.RadioButton rbSergeant;
        private System.Windows.Forms.RadioButton rbProsecutor;
        private System.Windows.Forms.RadioButton rbOfficeManager;
        private System.Windows.Forms.RadioButton rbInvestigator;

    }
}