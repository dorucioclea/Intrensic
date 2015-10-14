using System.Runtime.InteropServices;
using System.Threading;
using CodeITDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Intrensic.Util;
using System.Diagnostics;
using System.Windows;

namespace Intrensic
{
    public partial class frmLogIn : Form, IFormWithGoProDetector
    {

        

        private bool loginStartedFromGoProDevice = false;
        private string uploadPath = string.Empty;

        public frmLogIn()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            //this.Icon = Intrensic.Properties.Resources.Intrensic;
            niUser.Icon = Intrensic.Properties.Resources.shortcut;			
			this.TopMost = true;
			txtUserName.Focus();
			this.Activate();
            Context.contextMenu = this.cmNotify;            
        }


        private void PerformLogin()
        {
            DateTime started = DateTime.Now;

            btnLogin.Enabled = false;				

            System.Windows.Forms.Application.DoEvents();
            
            var loadingCancalationTokenSource = new CancellationTokenSource();
            var loadingCancalationToken = loadingCancalationTokenSource.Token;
            var loadingTask = new Task(() =>
            {
                var loadingForm = new frmLoading();
                loadingForm.TopMost = true;
                loadingForm.Show();
                while (!loadingCancalationToken.IsCancellationRequested)
                {
                    Thread.Sleep(50);
                    System.Windows.Forms.Application.DoEvents();
                }

                loadingForm.Close();

            }, loadingCancalationTokenSource.Token);
            loadingTask.Start();
            

            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text;

            bool hasUsers = false;

            CodeITDL.CodeITDbContext ctx = null;

            try
            {
                ctx = new CodeITDL.CodeITDbContext(0);
                
                if (ctx != null)
                {
					CodeITDL.License lic = null;
					string clientLicense = string.Empty;
					try
					{
						User user = ctx.Users.Where(d => d.UserName == userName && d.Password == password).FirstOrDefault();
						lic = ctx.Licenses.Where(s => s.Id == (ctx.UserLicenses.Where(c => c.UserId == (user.Id)).FirstOrDefault()).LicenseId).FirstOrDefault();
					}
					catch(Exception)
					{

					}
					if (lic != null)
						clientLicense = Encoding.UTF8.GetString(lic.LicenseBytes);

					if (!CodeITLicence.Licence.ValidateLicenceFromDB(clientLicense))
					{
						System.Windows.Forms.MessageBox.Show("You don't have license. Please contact administrator.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else
					{
						hasUsers = ctx.Users.Where(x => x.RoleId == (int)Role.Administrator && x.CustomerId == Context.CustomerId).Count() > 0;

						if (!hasUsers && userName.ToLower().Equals("administrator") && password.Equals("intrensic"))
						{
							loadingCancalationTokenSource.Cancel(true);

							System.Windows.MessageBox.Show("You are logged in as administrator, please add user with administrator role first", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

							Administration.frmUsers frmUsers = new Administration.frmUsers();
							frmUsers.ControlBox = true;
							frmUsers.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
							frmUsers.Icon = Intrensic.Properties.Resources.Intrensic;
							frmUsers.Name = "Initial User Creation Screen";
							frmUsers.isInitailUserFromLogin = true;
							frmUsers.ShowDialog();
						}
						else if (!hasUsers)
						{
							loadingCancalationTokenSource.Cancel(true);
							System.Windows.MessageBox.Show("There are no defined users and initial login information is not correct, please try again", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
							txtUserName.Clear();
							txtPassword.Clear();

							txtUserName.Focus();

							LoginAudit.WriteLoginAudit(CodeITConstants.LOGIN_INCORECT_CREDENTIALS_NO_USERS);

						}
						else if (hasUsers)
						{
							User usr = new User();
							usr = ctx.Users.FirstOrDefault(x => x.UserName.ToLower().Equals(userName.ToLower()) && x.Password.ToLower().Equals(password.ToLower()) && x.CustomerId == Context.CustomerId);
							if (usr == null)
								usr = new User();

							if (usr.Id <= 0)
							{
								loadingCancalationTokenSource.Cancel(true);
								LoginAudit.WriteLoginAudit(CodeITConstants.LOGIN_INCORECT_CREDENTIALS_HAS_USERS);
								System.Windows.MessageBox.Show("Username and/or password are not correct. Please try again", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
							}
							else
							{
								foreach (Form frm in System.Windows.Forms.Application.OpenForms)
								{
									if (frm.Name == "frmProgressStatus")
									{
										//check if user trying to login is user with current upload progress
										if (!((frmProgressStatus)frm).getOwnerOfUploadProcess.UserName.ToLower().Equals(userName.ToLower()))
										{
											loadingCancalationTokenSource.Cancel(true);
											LoginAudit.WriteLoginAudit(CodeITConstants.LOGIN_WHILE_UPLOAD_IN_PROGRESS_BY_DIFFERENT_USER, usr.Id);
											System.Windows.MessageBox.Show("There is an active upload process initiated by user: " + ((frmProgressStatus)frm).getOwnerOfUploadProcess.UserName.ToLower() + Environment.NewLine
												+ "Please wait for the upload process to complete before you are able to login", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
											return;
										}
										else
										{
											((frmProgressStatus)frm).DisableCancelButtonsOnLogout(false);
										}
									}
								}

								Context.UserId = usr.Id;
							}

							if (Context.UserId > 0)
							{
								Context.UserId = usr.Id;

								LoginAudit.WriteLoginAudit(CodeITConstants.LOGIN_SUCCESSFULL);
								ContextMenuItems(true);

								txtUserName.Clear();
								txtPassword.Clear();

								frmUserMainScreen frmMain = new frmUserMainScreen();
								frmMain.InitialGoToUpload = loginStartedFromGoProDevice;
								frmMain.InitialUploadPath = uploadPath;
								frmMain.Show();
								Context.mainForm = frmMain;
								this.uploadPath = string.Empty;
								this.loginStartedFromGoProDevice = false;
								this.Hide();
								//Thread.Sleep(5000);
								loadingCancalationTokenSource.Cancel(true);

								Context.CheckForGoProDevice();
							}
						}
					}
                }
            }
            catch (Exception ex)
            {
                // 
            }
            finally
            {
                if (ctx != null)
                {
                    ctx.Dispose();
                }

                loadingCancalationTokenSource.Cancel(true);
                btnLogin.Enabled = true;
            }
        }


        public void ContextMenuItems(bool isLogin)
        {
            tsmLogin.Visible = !isLogin;
            tsmLogout.Visible = isLogin;
            cmNotify.Refresh();
			txtUserName.Focus();
			this.Activate();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmLogin_Click(object sender, EventArgs e)
        {

            this.Show();
            txtUserName.Focus();
            this.WindowState = FormWindowState.Normal;
        }

        private void tsmLogout_Click(object sender, EventArgs e)
        {

            List<Form> openedForms = System.Windows.Forms.Application.OpenForms.Cast<Form>().ToList();
            foreach (Form frmOpened in openedForms)
            {
                if (frmOpened.Name == "DetectorForm") continue;

                if (frmOpened.Name == "frmLogIn")
                {
                    frmOpened.Hide();
                    this.ContextMenuItems(false);
                    continue;
                }
                else if (frmOpened.Name == "frmProgressStatus")
                {
                    if (((frmProgressStatus)frmOpened).hasUploadCompleted)
                        frmOpened.Close();
                    continue;
                }
                else if (frmOpened.Name == this.Name)
                    continue;
                string name = frmOpened.Name;
                frmOpened.Close();
            }


            if (Context.mainForm != null)
            {
                Context.mainForm.Close();
                Context.mainForm = null;
            }
            LoginAudit.WriteLoginAudit(CodeITConstants.LOGOUT_SUCCESSFULL);



        }

        public void GoProDeviceDetected(string driveLetter)
        {
            for (int i = 0; i < System.Windows.Forms.Application.OpenForms.Count; i++)
            {
                System.Windows.Forms.Form frm = System.Windows.Forms.Application.OpenForms[i];
                if (frm.Name == "frmUserMainScreen")
                    return;
            }
            this.loginStartedFromGoProDevice = true;
            this.uploadPath = driveLetter;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        public void DeviceDisconnected()
        {
        }

        private void tsmiShowProgress_Click(object sender, EventArgs e)
        {
            if (!Context.progressForm.hasUploadCompleted)
                Context.progressForm.Show();
            else
            {
                tsmiShowProgress.Visible = false;
                Context.progressForm.Close();
                Context.progressForm = null;
            }
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < System.Windows.Forms.Application.OpenForms.Count; i++)
            {
                System.Windows.Forms.Form frm = System.Windows.Forms.Application.OpenForms[i];
                if (frm.Name == "frmProgressStatus")
                {
                    if (((frmProgressStatus)frm).hasUploadCompleted)
                        frm.Close();
                }
            }

            PerformLogin();
        }

        private void niUser_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Context.UserId <= 0)
                tsmLogin.PerformClick();
        }


        

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
        }


        public void GoProMTPDeviceDetected()
        {
        }

        [DllImport("user32")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        private void frmLogIn_Shown(object sender, EventArgs e)
        {
            SetForegroundWindow(Handle);
            this.BringToFront();
            Activate();
        }
    }
}
