using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeITLicence;
using CodeITDL;

namespace CodeITAdminLicence
{
    public partial class LicenceForm : Form
    {
        public LicenceForm()
        {
            InitializeComponent();
        }

        private void GoPro911Licence_Load(object sender, EventArgs e)
        {

        }

        private void rad_CheckedChanged(object sender, EventArgs e)
        {
            pnlLocal.Visible = radLocal.Checked;
            pnlCloud.Visible = !radLocal.Checked;
        }

        private bool CheckInput()
        {
            bool result = true;

            try
            {
                // Client id
                if (!String.IsNullOrEmpty(txtClientId.Text))
                {
                    if (!String.IsNullOrEmpty(txtAdminUsername.Text) && !String.IsNullOrEmpty(txtAdminPassword.Text))
                    {
                        // MSSQL Credentials
                        if (!String.IsNullOrEmpty(txtSqlServerName.Text) && !String.IsNullOrEmpty(txtSqlServerDatabaseName.Text) && !String.IsNullOrEmpty(txtSqlServerUsername.Text) &&
                        !String.IsNullOrEmpty(txtSqlServerPassword.Text))
                        {
                            // Local
                            if (radLocal.Checked && (String.IsNullOrEmpty(txtLocalPath.Text) || String.IsNullOrEmpty(txtLocalUsername.Text) || String.IsNullOrEmpty(txtLocalPassword.Text)))
                            {
                                MessageBox.Show("Local input settings are incorrect!", "Incorrect input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                result = false;
                            }
                            // Cloud
                            else if (radCloud.Checked && (String.IsNullOrEmpty(txtCloudId.Text) || String.IsNullOrEmpty(txtCloudSecret.Text)))
                            {
                                MessageBox.Show("Cloud  input settings are incorrect!", "Incorrect input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                result = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("MSSQL input is incorrect!", "Incorrect input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            result = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid admin credentials!", "Incorrect input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        result = false;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid client id!", "Incorrect input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Settings are incorrect!", "Incorrect input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }

            return result;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string licenseKey = "";

            if (CheckInput())
            {
                Licence.ClientId = txtClientId.Text;

                if (dtpUntil.Checked)
                {
                    var dateValid = new DateTime();
                    if (DateTime.TryParse(dtpUntil.Text, out dateValid))
                    {
                        Licence.DateValid = dateValid;
                    }
                    else
                    {
                        Licence.DateValid = null;
                    }
                }
                else
                {
                    Licence.DateValid = null;
                }

                int usersLimit = 0;
                if (Int32.TryParse(txtNumberOfUsers.Text, out usersLimit))
                {
                    Licence.UsersLimit = usersLimit;
                }
                else
                {
                    Licence.UsersLimit = null;
                }

                Licence.SqlServerName = txtSqlServerName.Text;
                Licence.SqlServerDatabase = txtSqlServerDatabaseName.Text;
                Licence.SqlServerUsername = txtSqlServerUsername.Text;
                Licence.SqlServerPassword = txtSqlServerPassword.Text;

                if (radLocal.Checked)
                {
                    Licence.StorageType = StorageType.Local;
                    Licence.LocalPath = txtLocalPath.Text;
                    Licence.LocalUsername = txtLocalUsername.Text;
                    Licence.LocalPassword = txtLocalPassword.Text;
                }
                else if (radCloud.Checked)
                {
                    Licence.StorageType = StorageType.Cloud;
                    Licence.CloudId = txtCloudId.Text;
                    Licence.CloudSecret = txtCloudSecret.Text;
                }


                if (!Licence.IsValid())
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (string message in Licence.ValidationMessagesList)
                    {
                        sb.AppendLine(message);
                    }

                    MessageBox.Show(sb.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                licenseKey = Licence.GenerateKey();

                UpdateLicenseToDb(licenseKey);
            }
        }


        private void UpdateLicenseToDb(string licenseKey)
        {
            try
            {
                using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext())
                {
                    // Insert customer
                    Guid customerId = new Guid(txtClientId.Text);
                    Customer customer = new Customer();
                    customer.Id = customerId;
                    customer.Name = txtAdminName.Text;

                    if (ctx.Customers.FirstOrDefault(c => c.Id == customer.Id && c.Name == customer.Name) == null)
                    {
                        ctx.Customers.Add(customer);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Failed to create license!", "License failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }    

                    // Insert user
                    User currentUser = new User();
                    currentUser.FirstName = txtAdminName.Text;
                    currentUser.UserName = txtAdminUsername.Text;
                    currentUser.Password = txtAdminPassword.Text;
                    currentUser.CreatedOn = DateTime.Now;
                    currentUser.ModifiedOn = DateTime.Now;
                    currentUser.CreatedBy = 1;
                    currentUser.ModifiedBy = 1;

                    Int32 newUserId = -1;

                    if (ctx.Users.FirstOrDefault(u => u.UserName == currentUser.UserName && u.Password == currentUser.Password) == null)
                    {
                        ctx.Users.Add(currentUser);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("User already exist in the database, license creation failed!", "License failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Insert License
                    if (ctx.Users.FirstOrDefault(u => u.UserName == currentUser.UserName && u.Password == currentUser.Password) != null)
                    {
                        newUserId = ctx.Users.FirstOrDefault(u => u.UserName == currentUser.UserName && u.Password == currentUser.Password).Id;
                    }

                    if (newUserId != -1)
                    {
                        CodeITDL.License license = new CodeITDL.License();
                        license.CustomerId = customerId;
                        license.LicenseBytes = Encoding.UTF8.GetBytes(licenseKey);
                        ctx.SaveChanges();
                    }

                    Int32 licenseId = -1;
                    if (ctx.Licenses.FirstOrDefault(l => l.CustomerId == customerId) != null)
                    {
                        licenseId = ctx.Licenses.FirstOrDefault(l => l.CustomerId == customerId).Id;
                    }

                    // Inser UserLicense
                    if (licenseId != -1)
                    {
                        UserLicense userLicense = new UserLicense();
                        userLicense.LicenseId = licenseId;
                        userLicense.UserId = newUserId;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Failed to create license!", "License failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to create license!", "License failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            /*
            Licence.ParseFromString(txtLicence.Text);
            
            if (Licence.ValidationMessagesList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (string message in Licence.ValidationMessagesList)
                {
                    sb.AppendLine(message);
                }

                MessageBox.Show(sb.ToString(), "Error");
                return;
            }

            if (!Licence.IsValid())
            {
                StringBuilder sb = new StringBuilder();

                foreach (string message in Licence.ValidationMessagesList)
                {
                    sb.AppendLine(message);
                }

                MessageBox.Show(sb.ToString(), "Error");
                return;
            }


            txtClientId.Text = Licence.ClientId;
            dtpUntil.Checked = Licence.DateValid.HasValue;
            if (Licence.DateValid.HasValue)
            {
                dtpUntil.Text = Licence.DateValid.ToString();
            }
            txtNumberOfUsers.Text = Licence.UsersLimit.ToString();
            txtSqlServerName.Text = Licence.SqlServerName;
            txtSqlServerDatabaseName.Text = Licence.SqlServerDatabase;
            txtSqlServerUsername.Text = Licence.SqlServerUsername;
            txtSqlServerPassword.Text = Licence.SqlServerPassword;

            if (Licence.StorageType == StorageType.Local)
            {
                radLocal.Checked = true;
                txtLocalPath.Text = Licence.LocalPath;
                txtLocalUsername.Text = Licence.LocalUsername;
                txtLocalPassword.Text = Licence.LocalPassword;
            }
            if (Licence.StorageType == StorageType.Cloud)
            {
                radCloud.Checked = true;
                txtCloudId.Text = Licence.CloudId;
                txtCloudSecret.Text = Licence.CloudSecret;
            }
            */
        }

        private const string DEFAULT_SQL_SERVER = "intrensic.cqentjdedg6z.us-west-2.rds.amazonaws.com";
        private const string DEFAULT_SQL_DATABASE_NAME = "Intrensic";
        private const string DEFAULT_SQL_USERNAME = "Intrensic";
        private const string DEFAULT_SQL_PASSWORD = "1n7r3n51c";


        private const string DEFAULT_CLOUD_ACCESS_KEY = "AKIAJ4TQBWDKLMZAQKDA";
        private const string DEFAULT_CLOUD_SECRET_ACCESS_KEY = "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";

        private void chkDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDefault.Checked)
            {
                txtSqlServerName.Text = DEFAULT_SQL_SERVER;
                txtSqlServerDatabaseName.Text = DEFAULT_SQL_DATABASE_NAME;
                txtSqlServerUsername.Text = DEFAULT_SQL_USERNAME;
                txtSqlServerPassword.Text = DEFAULT_SQL_PASSWORD;

                txtCloudId.Text = DEFAULT_CLOUD_ACCESS_KEY;
                txtCloudSecret.Text = DEFAULT_CLOUD_SECRET_ACCESS_KEY;
            }
            else
            {
                txtSqlServerName.Text = String.Empty;
                txtSqlServerDatabaseName.Text = String.Empty;
                txtSqlServerUsername.Text = String.Empty;
                txtSqlServerPassword.Text = String.Empty;
            }
        }
    }
}
