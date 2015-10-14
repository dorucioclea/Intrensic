using CodeITDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Intrensic.Administration
{
    public partial class frmUsers : Form
    {
        bool isLoaded = false;
        bool hasNewImage = false;
        int editUserId = 0;

        public bool isInitailUserFromLogin { get; set; }
        public frmUsers()
        {
            InitializeComponent();
            this.BackgroundImage = Context.GetImageForCustomer();
            
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {


            if (isInitailUserFromLogin)
            {
                rbUser.Visible = rbProsecutor.Visible = rbSergeant.Visible = rbOfficeManager.Visible = rbInvestigator.Visible = false;
                rbAdministrator.Checked = true;
            }

            changeControlsState(gbUserManagement, false);
            
        }

        private void frmUsers_Shown(object sender, EventArgs e)
        {
            isLoaded = true;
            List<User> users = new List<User>();

            using (CodeITDbContext ctx = new CodeITDbContext(Context.UserId))
            {
                users = ctx.Users.Where(x => x.CustomerId == Context.CustomerId).ToList();
            }

            if (users.Count > 0)
            {
                lvUsers.Items.Clear();
                BindListView(users);
            }

        }

        private void lvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded)
                return;
            
            if (!btnCreate.Visible) return;

            if (lvUsers.SelectedItems == null)
                return;
            if (lvUsers.SelectedItems.Count == 0)
                return;
            changeControlsState(gbUserManagement, false);
            
            Image img = null;
            using (CodeITDbContext ctx = new CodeITDbContext(Context.UserId))
            {
                int selectedId = (int)lvUsers.SelectedItems[0].Tag;
                User selectedUser = ctx.Users.Where(x => x.Id == selectedId).FirstOrDefault();
                if (selectedUser == null)
                    return;

                byte[] imageArray = ctx.UserPictures.Where(x => x.UserId == selectedUser.Id).Select(x => x.Picture).FirstOrDefault();
                if (imageArray != null)
                    if (imageArray.Count() > 0)
                        img = Image.FromStream(new MemoryStream(imageArray));


                txtFirstName.Text = selectedUser.FirstName;
                txtID.Text = selectedUser.IdNumber;
                txtLastName.Text = selectedUser.LastName;
                txtMiddleName.Text = selectedUser.MiddleName;
                txtPassword.Text = selectedUser.Password;
                txtUserName.Text = selectedUser.UserName;
                txtDeviceId.Tag = selectedUser.DeviceId;
                txtDeviceId.Clear();
                if (!string.IsNullOrEmpty(selectedUser.DeviceId))
                    txtDeviceId.Text = Regex.Match(selectedUser.DeviceId,
                             @"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b",
                             RegexOptions.IgnoreCase).Value;

                rbAdministrator.Checked = ((Role)selectedUser.RoleId == Role.Administrator);
                rbUser.Checked = ((Role)selectedUser.RoleId == Role.User);
                rbInvestigator.Checked = ((Role)selectedUser.RoleId == Role.Investigator);
                rbOfficeManager.Checked = ((Role)selectedUser.RoleId == Role.PatrolOfficer);
                rbProsecutor.Checked = ((Role)selectedUser.RoleId == Role.Prosecutor);
                rbSergeant.Checked = ((Role)selectedUser.RoleId == Role.Sergeant);

            }

            
        }
                
        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in gbUserManagement.Controls)
                if (ctr.GetType() == typeof(TextBox))
                {
                    ((TextBox)ctr).Clear();
                }
                else if (ctr.GetType() == typeof(PictureBox))
                {
                    ((PictureBox)ctr).Image = ((PictureBox)ctr).InitialImage;
                }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            btnClear.PerformClick();

            changeControlsState(gbUserManagement, false);
            changeControlsState(gbUserActions, true);
            
            lvUsers_SelectedIndexChanged(sender, e);


        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            editUserId = 0;
            btnClear.PerformClick();

            changeControlsState(gbUserActions, false);
            changeControlsState(gbUserManagement, true);
           
            txtID.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvUsers.Items.Count == 0)
            {
				System.Windows.MessageBox.Show("There are no users to edit", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (lvUsers.SelectedItems.Count == 0)
            {
				System.Windows.MessageBox.Show("Please select user to edit", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            editUserId = (int)lvUsers.SelectedItems[0].Tag;

            changeControlsState(gbUserActions, false);
            changeControlsState(gbUserManagement, true);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvUsers.Items.Count == 0)
            {
                System.Windows.MessageBox.Show("There are no users to delete", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (lvUsers.SelectedItems.Count == 0)
            {
                System.Windows.MessageBox.Show("Please select user to delete", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (System.Windows.MessageBox.Show("Are you sure you want to delete selected user", "User delete confirmation", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
            {
                using (CodeITDbContext ctx = new CodeITDbContext(Context.UserId))
                {
                    int userId = (int)lvUsers.SelectedItems[0].Tag;
                    ctx.Users.Remove(ctx.Users.Where(x => x.Id == userId).FirstOrDefault());
                    ctx.SaveChanges();
                }

                ListViewItem lvi = lvUsers.SelectedItems[0];
                lvUsers.SelectedItems[0].Selected = false;
                lvUsers.Items.Remove(lvi);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<User> filteredUsers = new List<User>();

            lvUsers.Items.Clear();
            using (CodeITDbContext ctx = new CodeITDbContext(Context.UserId))
            {
                if (txtSearch.Text.Trim().Length > 0)
                    filteredUsers = ctx.Users.Where(x => (x.FirstName + " " + x.MiddleName + " " + x.LastName).ToLower().StartsWith(txtSearch.Text.ToLower()) && x.CustomerId == Context.CustomerId).ToList();
                else
                    filteredUsers = ctx.Users.Where(x => x.CustomerId == Context.CustomerId).ToList();
            }

            BindListView(filteredUsers);


        }

        private void BindListView(List<User> filteredUsers)
        {
            foreach (User user in filteredUsers)
            {
                ListViewItem lvi = new ListViewItem(string.Format("{0} {1} {2}", user.FirstName, user.MiddleName, user.LastName));
                lvi.SubItems.Add(user.UserName);
                lvi.Tag = user.Id;
                lvUsers.Items.Add(lvi);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            User usr = new User();

            if (!ValidateUserInput())
                return;

            using (CodeITDbContext ctx = new CodeITDbContext(Context.UserId))
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    if (editUserId != 0)
                        usr = ctx.Users.Where(x => x.Id == editUserId).FirstOrDefault();


                    if (editUserId == 0)
                    {
                        usr.CreatedBy = Context.UserId;
                    }

                    usr.ModifiedBy = Context.UserId;

                    usr.IdNumber = txtID.Text.Trim();
                    usr.FirstName = txtFirstName.Text.Trim();
                    usr.MiddleName = txtMiddleName.Text.Trim();
                    usr.LastName = txtLastName.Text.Trim();
                    usr.UserName = txtUserName.Text.Trim();
                    usr.Password = txtPassword.Text.Trim();
                    usr.RoleId = getSelectedRole(); 
                    usr.CustomerId = Context.CustomerId;
                    usr.DeviceId = txtDeviceId.Tag == null ? string.Empty : txtDeviceId.Tag.ToString();

                    if (editUserId <= 0)
                        ctx.Users.Add(usr);
					

                    ctx.SaveChanges();

					if (editUserId == 0)
					{
						int newUserId = ctx.Users.Where(c=>c.UserName == usr.UserName && c.Password == usr.Password && c.CustomerId == usr.CustomerId).FirstOrDefault().Id;
						UserLicense usrLic = new UserLicense();
						usrLic.LicenseId = ctx.UserLicenses.Where(c => c.UserId == Context.UserId).FirstOrDefault().LicenseId;
						usrLic.UserId = newUserId;
						ctx.UserLicenses.Add(usrLic);
						ctx.SaveChanges();
					}

                    if (hasNewImage)
                    {
                        UserPicture userPicture = new UserPicture();
                        bool isImageInDB = false;
                        if (editUserId != 0)
                            if (ctx.UserPictures.Where(x => x.UserId == editUserId).Count() > 0)
                            {
                                isImageInDB = true;
                                userPicture = ctx.UserPictures.Where(x => x.UserId == editUserId).FirstOrDefault();
                            }
                        MemoryStream ms = new MemoryStream();
                        System.Drawing.Imaging.ImageFormat imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                       
                        userPicture.Picture = ms.ToArray();

                        userPicture.UserId = usr.Id;

                        if (!isImageInDB)
                            ctx.UserPictures.Add(userPicture);

                        ctx.SaveChanges();

                        transaction.Commit();

                        if (isInitailUserFromLogin && usr.RoleId == (int)Role.Administrator)
                        {
                            System.Windows.MessageBox.Show("You have created your first user, please log in with this credentials now." + Environment.NewLine + "UserName: " + usr.UserName + "   Password: " + usr.Password, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                        }

                    }
                    else
                        transaction.Commit();

                }

            }
            if (editUserId <= 0)
                System.Windows.MessageBox.Show("User created successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                System.Windows.MessageBox.Show("User updated successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                editUserId = 0;
            }
            btnClear.PerformClick();
            btnClose.PerformClick();
            btnSearch.PerformClick();
        }

        private int getSelectedRole()
        {
            int roleId = (int)Role.Administrator;
            if (rbUser.Checked)
                roleId = (int)Role.User;
            else if (rbSergeant.Checked)
                roleId = (int)Role.Sergeant;
            else if (rbInvestigator.Checked)
                roleId = (int)Role.Investigator;
            else if (rbOfficeManager.Checked)
                roleId = (int)Role.PatrolOfficer;
            else if (rbProsecutor.Checked)
                roleId = (int)Role.Prosecutor;

            return roleId;
        }

        private bool ValidateUserInput()
        {

            if (txtFirstName.Text.Trim() == string.Empty)
            {
				System.Windows.MessageBox.Show("Please enter first name", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                txtFirstName.Focus();
                return false;
            }

            if (txtLastName.Text.Trim() == string.Empty)
            {
				System.Windows.MessageBox.Show("Please enter last name", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                txtLastName.Focus();
                return false;
            }

            if (txtUserName.Text.Trim() == string.Empty)
            {
				System.Windows.MessageBox.Show("Please enter user name", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                txtUserName.Focus();
                return false;
            }

            if (txtPassword.Text.Trim() == string.Empty)
            {
				System.Windows.MessageBox.Show("Please enter password", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                txtPassword.Focus();
                return false;
            }

            if (editUserId <= 0)
                using (CodeITDbContext ctx = new CodeITDbContext(Context.UserId))
                {
                    if (ctx.Users.Where(x => x.CustomerId == Context.CustomerId && x.UserName.ToLower() == txtUserName.Text.Trim().ToLower()).Count() > 0)
                    {
						System.Windows.MessageBox.Show("User with same UserName already exists in the system, please try another UserName", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtUserName.Focus();
                        return false;
                    }
                }



            return true;
        }

        private void btnMapDevice_Click(object sender, EventArgs e)
        {
            frmGoProDeviceSelector deviceSelector = new frmGoProDeviceSelector();
            deviceSelector.ShowDialog();

            string deviceId = deviceSelector.SelectedDevice;
            if (!string.IsNullOrEmpty(deviceId))
                txtDeviceId.Text = Regex.Match(deviceId,
                             @"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b",
                             RegexOptions.IgnoreCase).Value;
            txtDeviceId.Tag = deviceId;

        }

        private void changeControlsState(GroupBox sender, bool enable)
        {
             foreach(Control ctr in sender.Controls)
            {
                if (ctr is TextBox)
                    if ((TextBox)ctr != txtDeviceId)
                        ((TextBox)ctr).ReadOnly = !enable;
                if (ctr is Button)
                    ((Button)ctr).Visible = enable;
            }

             if (sender == gbUserActions && enable)
                 lvUsers.Focus();
        }

    }
}
