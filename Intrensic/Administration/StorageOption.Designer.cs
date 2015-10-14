namespace Intrensic.Administration
{
	partial class StorageOption
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.gbLocalSettings = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.lblLocalUsername = new System.Windows.Forms.Label();
			this.lblLocalPath = new System.Windows.Forms.Label();
			this.lblLocalPassword = new System.Windows.Forms.Label();
			this.txtLocalPath = new System.Windows.Forms.TextBox();
			this.txtLocalUsername = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtLocalPassword = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.gbStorageOptions = new System.Windows.Forms.GroupBox();
			this.rbtnLocal = new System.Windows.Forms.RadioButton();
			this.rbtnCloud = new System.Windows.Forms.RadioButton();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.btnSave = new System.Windows.Forms.Button();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.gbLocalSettings.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.gbStorageOptions.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(673, 350);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// panel2
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.panel2, 2);
			this.panel2.Controls.Add(this.gbLocalSettings);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(203, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(467, 120);
			this.panel2.TabIndex = 1;
			// 
			// gbLocalSettings
			// 
			this.gbLocalSettings.BackColor = System.Drawing.Color.Transparent;
			this.gbLocalSettings.Controls.Add(this.tableLayoutPanel3);
			this.gbLocalSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbLocalSettings.ForeColor = System.Drawing.Color.White;
			this.gbLocalSettings.Location = new System.Drawing.Point(0, 0);
			this.gbLocalSettings.Name = "gbLocalSettings";
			this.gbLocalSettings.Size = new System.Drawing.Size(467, 120);
			this.gbLocalSettings.TabIndex = 2;
			this.gbLocalSettings.TabStop = false;
			this.gbLocalSettings.Text = "Local Storage System Settings";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 4;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
			this.tableLayoutPanel3.Controls.Add(this.btnBrowse, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.lblLocalUsername, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.lblLocalPath, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.lblLocalPassword, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.txtLocalPath, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.txtLocalUsername, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.label1, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.txtLocalPassword, 1, 2);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 4;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(461, 101);
			this.tableLayoutPanel3.TabIndex = 0;
			// 
			// btnBrowse
			// 
			this.btnBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.btnBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnBrowse.FlatAppearance.BorderSize = 0;
			this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnBrowse.ForeColor = System.Drawing.Color.White;
			this.btnBrowse.Location = new System.Drawing.Point(385, 3);
			this.btnBrowse.Margin = new System.Windows.Forms.Padding(5, 3, 5, 12);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(71, 20);
			this.btnBrowse.TabIndex = 20;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = false;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// lblLocalUsername
			// 
			this.lblLocalUsername.AutoSize = true;
			this.lblLocalUsername.Dock = System.Windows.Forms.DockStyle.Right;
			this.lblLocalUsername.ForeColor = System.Drawing.Color.White;
			this.lblLocalUsername.Location = new System.Drawing.Point(112, 40);
			this.lblLocalUsername.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblLocalUsername.Name = "lblLocalUsername";
			this.lblLocalUsername.Size = new System.Drawing.Size(55, 30);
			this.lblLocalUsername.TabIndex = 14;
			this.lblLocalUsername.Text = "Username";
			// 
			// lblLocalPath
			// 
			this.lblLocalPath.AutoSize = true;
			this.lblLocalPath.Dock = System.Windows.Forms.DockStyle.Right;
			this.lblLocalPath.ForeColor = System.Drawing.Color.White;
			this.lblLocalPath.Location = new System.Drawing.Point(7, 5);
			this.lblLocalPath.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblLocalPath.Name = "lblLocalPath";
			this.lblLocalPath.Size = new System.Drawing.Size(160, 30);
			this.lblLocalPath.TabIndex = 12;
			this.lblLocalPath.Text = "Path to network storage location";
			// 
			// lblLocalPassword
			// 
			this.lblLocalPassword.AutoSize = true;
			this.lblLocalPassword.Dock = System.Windows.Forms.DockStyle.Right;
			this.lblLocalPassword.ForeColor = System.Drawing.Color.White;
			this.lblLocalPassword.Location = new System.Drawing.Point(114, 75);
			this.lblLocalPassword.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblLocalPassword.Name = "lblLocalPassword";
			this.lblLocalPassword.Size = new System.Drawing.Size(53, 30);
			this.lblLocalPassword.TabIndex = 16;
			this.lblLocalPassword.Text = "Password";
			// 
			// txtLocalPath
			// 
			this.tableLayoutPanel3.SetColumnSpan(this.txtLocalPath, 2);
			this.txtLocalPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLocalPath.Location = new System.Drawing.Point(173, 3);
			this.txtLocalPath.Name = "txtLocalPath";
			this.txtLocalPath.Size = new System.Drawing.Size(204, 20);
			this.txtLocalPath.TabIndex = 17;
			// 
			// txtLocalUsername
			// 
			this.txtLocalUsername.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLocalUsername.Location = new System.Drawing.Point(173, 38);
			this.txtLocalUsername.Name = "txtLocalUsername";
			this.txtLocalUsername.Size = new System.Drawing.Size(57, 20);
			this.txtLocalUsername.TabIndex = 18;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 105);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 1);
			this.label1.TabIndex = 0;
			// 
			// txtLocalPassword
			// 
			this.txtLocalPassword.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLocalPassword.Location = new System.Drawing.Point(173, 73);
			this.txtLocalPassword.Name = "txtLocalPassword";
			this.txtLocalPassword.PasswordChar = '*';
			this.txtLocalPassword.Size = new System.Drawing.Size(57, 20);
			this.txtLocalPassword.TabIndex = 19;
			this.txtLocalPassword.UseSystemPasswordChar = true;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.Add(this.gbStorageOptions);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(194, 120);
			this.panel1.TabIndex = 0;
			// 
			// gbStorageOptions
			// 
			this.gbStorageOptions.BackColor = System.Drawing.Color.Transparent;
			this.gbStorageOptions.Controls.Add(this.rbtnLocal);
			this.gbStorageOptions.Controls.Add(this.rbtnCloud);
			this.gbStorageOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbStorageOptions.ForeColor = System.Drawing.Color.White;
			this.gbStorageOptions.Location = new System.Drawing.Point(0, 0);
			this.gbStorageOptions.Name = "gbStorageOptions";
			this.gbStorageOptions.Size = new System.Drawing.Size(194, 120);
			this.gbStorageOptions.TabIndex = 1;
			this.gbStorageOptions.TabStop = false;
			this.gbStorageOptions.Text = "Storage Options";
			// 
			// rbtnLocal
			// 
			this.rbtnLocal.AutoSize = true;
			this.rbtnLocal.Location = new System.Drawing.Point(27, 56);
			this.rbtnLocal.Name = "rbtnLocal";
			this.rbtnLocal.Size = new System.Drawing.Size(51, 17);
			this.rbtnLocal.TabIndex = 1;
			this.rbtnLocal.TabStop = true;
			this.rbtnLocal.Text = "Local";
			this.rbtnLocal.UseVisualStyleBackColor = true;
			this.rbtnLocal.CheckedChanged += new System.EventHandler(this.rbtnCloud_CheckedChanged);
			// 
			// rbtnCloud
			// 
			this.rbtnCloud.AutoSize = true;
			this.rbtnCloud.Location = new System.Drawing.Point(27, 20);
			this.rbtnCloud.Name = "rbtnCloud";
			this.rbtnCloud.Size = new System.Drawing.Size(52, 17);
			this.rbtnCloud.TabIndex = 0;
			this.rbtnCloud.TabStop = true;
			this.rbtnCloud.Text = "Cloud";
			this.rbtnCloud.UseVisualStyleBackColor = true;
			this.rbtnCloud.CheckedChanged += new System.EventHandler(this.rbtnCloud_CheckedChanged);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel2.Controls.Add(this.btnSave, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 129);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(194, 29);
			this.tableLayoutPanel2.TabIndex = 4;
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(58)))));
			this.tableLayoutPanel2.SetColumnSpan(this.btnSave, 2);
			this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSave.FlatAppearance.BorderSize = 0;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnSave.ForeColor = System.Drawing.Color.White;
			this.btnSave.Location = new System.Drawing.Point(49, 0);
			this.btnSave.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(145, 29);
			this.btnSave.TabIndex = 10;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = false;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// StorageOption
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(140)))), ((int)(((byte)(255)))));
			this.BackgroundImage = global::Intrensic.Properties.Resources.default_inapp;
			this.ClientSize = new System.Drawing.Size(673, 350);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "StorageOption";
			this.Text = "StorageOption";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.gbLocalSettings.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.gbStorageOptions.ResumeLayout(false);
			this.gbStorageOptions.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.GroupBox gbLocalSettings;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox gbStorageOptions;
		private System.Windows.Forms.RadioButton rbtnLocal;
		private System.Windows.Forms.RadioButton rbtnCloud;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label lblLocalPath;
		private System.Windows.Forms.Label lblLocalUsername;
		private System.Windows.Forms.Label lblLocalPassword;
		private System.Windows.Forms.TextBox txtLocalPath;
		private System.Windows.Forms.TextBox txtLocalUsername;
		private System.Windows.Forms.TextBox txtLocalPassword;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Button btnBrowse;

	}
}