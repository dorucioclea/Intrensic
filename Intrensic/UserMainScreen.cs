using CodeITBL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;

namespace Intrensic
{
    public partial class frmUserMainScreen : Form, IFormWithGoProDetector
    {
        frmUploadForm uploadForm;
        frmArchiveForm frmArchive;
        public bool InitialGoToUpload { get; set; }
        public string InitialUploadPath { get; set; }


        public frmUserMainScreen()
        {
            InitializeComponent();

			this.WindowState = FormWindowState.Maximized;
			
            //this.Icon = Intrensic.Properties.Resources.Intrensic;
            //this.pnlPlaceHolder.BackgroundImage = Context.GetImageForCustomer();
            //Intrensic.Properties.Resources.icon;
            //this.Icon = Intrensic.Properties.Resources.
        }

        Dictionary<int, IFileTransfer> taskList = new Dictionary<int, IFileTransfer>();

        void crtUserMenu_Upload_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            ucUserVideos uv = uploadForm.getUserVideosConrtol;
            if (uv.lvItemsContainer.Items.Count == 0)
            {
				MessageBox.Show(this, "There are no videos to upload, please insert removable media and try again", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (uv.lvItemsContainer.SelectedItems.Count == 0)
            {
				MessageBox.Show(this, "Please select one or more videos to upload", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                uv.lvItemsContainer.Focus();
                return;
            }

            bool shouldDeleteCompletedFiles = (MessageBox.Show("Would you like to delete successfully uploaded files?", "Delete Confirmation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes);
            //List<CodeITDL.File> filePaths = new List<CodeITDL.File>();
            List<FileFromDB> filePaths = new List<FileFromDB>();
            foreach (FileFromDB item in uv.lvItemsContainer.SelectedItems)
            {
                filePaths.Add(item);

               
            }

            Context.progressForm.Show();
            Context.progressForm.BeginUploadProcess(filePaths, shouldDeleteCompletedFiles);
        }
                
        private void BindUserControl(string driveLetter)
        {

            DirectoryInfo dirInfo = new DirectoryInfo(driveLetter);
            FileInfo[] allFiles = dirInfo.GetFiles("*.MP4", SearchOption.AllDirectories);

            List<CodeITDL.File> list = new List<CodeITDL.File>();


            foreach (FileInfo fi in allFiles)
            {
                list.Add(new CodeITDL.File() { OriginalFileName = fi.Name, OriginalFileLocation = fi.FullName, NewFileName = Guid.NewGuid().ToString().Replace("-", string.Empty), OriginalFileDate = fi.CreationTime, isFromCard = false, UserName = fi.Name });
                //list.Add(new demo() { Date = fi.CreationTime.ToString(), Name = fi.Name, UniqueId = string.Format("{0}_{1}", fi.Name, new Random().Next(1, 1000)), FullPath = fi.FullName, HasNote = false, isFromCard = false, Note = string.Empty });
            }
            if (uploadForm == null)
                uploadForm = new frmUploadForm();

            ucUserVideos uv = uploadForm.getUserVideosConrtol;

            uv.ContextMenuClick -= uv_ContextMenuClick;
            uv.ContextMenuClick += uv_ContextMenuClick;
            uv.lvItemsContainer.ContextMenu.Visibility = System.Windows.Visibility.Hidden;
            uv.lvItemsContainer.ContextMenu.IsOpen = false;
            uv.IsUpload = true;
            uv.lvItemsContainer.ItemsSource = list;

        }

        void uv_ContextMenuClick(object sender, EventArgs e, ContextAction action, List<string> videos)
        {
            if (action == ContextAction.Preview && videos.Count > 0)
            {
                frmMediaPlayerPopup mediaPlayerPopup = new frmMediaPlayerPopup();
                mediaPlayerPopup.LoadVideos(videos);
                mediaPlayerPopup.StartPosition = FormStartPosition.CenterParent;
                mediaPlayerPopup.ShowDialog(this);
            }
        }
        
        bool isClosingFromLogout = false;
        
        void crtUserMenu_UserMenuClickEvent(object sender, EventArgs e, string menuItem)
        {

            //while (pnlPlaceHolder.Controls.Count > 0)
            //    pnlPlaceHolder.Controls.RemoveAt(0);

            pnlPlaceHolder.Controls.Clear();

            switch (menuItem)
            {
                case "Archive":
                    frmArchive = new frmArchiveForm();
                    frmArchive.Dock = DockStyle.Fill;
                    frmArchive.TopLevel = false;
                    //frmArchive.BackColor = Color.Transparent;
                    pnlPlaceHolder.Controls.Add(frmArchive);
                    frmArchive.Show();
                    //frmArchive.Width = pnlPlaceHolder.Width;
                    //frmArchive.WindowState = FormWindowState.Maximized;
                    //frmArchive.Refresh();
                    //frmArchive.PerformAutoScale();
                    //frmArchive.
                    //pnlPlaceHolder.Refresh();
                    break;
                case "Upload":
                    uploadForm = new frmUploadForm();
                    uploadForm.Dock = DockStyle.Fill;
                    uploadForm.ShowButtonForUpload += uploadForm_ShowButtonForUpload;
                    uploadForm.TopLevel = false;
                    //uploadForm.BackColor = Color.Transparent;
                    uploadForm.Width = pnlPlaceHolder.Width;
                    pnlPlaceHolder.Controls.Add(uploadForm);
                    //uploadForm.SearchForVideos();
                    //uploadForm.CheckForMediaOnGoProDevice();
                    if(!Context.CheckForGoProDevice())
                    {
                        MessageBox.Show(this, "GoPro camera not detected. Please connect your camera and try again.","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    uploadForm.Show();
                    break;
                case "Users":
                    Administration.frmUsers frmUsers = new Administration.frmUsers();
                    frmUsers.Dock = DockStyle.Fill;
                    frmUsers.TopLevel = false;                    
                    pnlPlaceHolder.Controls.Add(frmUsers);
                    frmUsers.Show();
                    break;
                case "Settings":
                    Administration.frmSettings frmSettings = new Administration.frmSettings();
                    frmSettings.Dock = DockStyle.Fill;
                    frmSettings.TopLevel = false;
                    pnlPlaceHolder.Controls.Add(frmSettings);
                    frmSettings.Show();
                    break;
                case "Audit":
                    Administration.frmAuditLog frmAudit = new Administration.frmAuditLog();
                    frmAudit.Dock = DockStyle.Fill;
                    frmAudit.TopLevel = false;
                    pnlPlaceHolder.Controls.Add(frmAudit);
                    frmAudit.Show();                    
                    break;
				case "StorageOption":
					Administration.StorageOption frmStorgeOption = new Administration.StorageOption();
					frmStorgeOption.Dock = DockStyle.Fill;
					frmStorgeOption.TopLevel = false;
					pnlPlaceHolder.Controls.Add(frmStorgeOption);
					frmStorgeOption.Show();
					break;
                default: // LOGOUT
                    isClosingFromLogout = true;
                    List<Form> openedForms = Application.OpenForms.Cast<Form>().ToList();
                    foreach (Form frmOpened in openedForms)
                    {
                        if (frmOpened.Name == "DetectorForm") continue;
                        if (frmOpened.Name == "frmLogIn")
                        {
                            frmOpened.Show();
                            ((frmLogIn)frmOpened).ContextMenuItems(false);
                            continue;
                        }
						if (frmOpened.Name == "frmProgressStatus")
						{
							if (((frmProgressStatus)frmOpened).hasUploadCompleted)
							{
								frmOpened.Close();
							}
							else
							{
								((frmProgressStatus)frmOpened).DisableCancelButtonsOnLogout(true);
							}
							continue;
						}
						else if (frmOpened.Name == this.Name)
						{
							frmOpened.Close();
							continue;
						}
                        //frmOpened.Close();
                    }

                    LoginAudit.WriteLoginAudit(CodeITConstants.LOGOUT_SUCCESSFULL);
                    
					//this.Close();
					//this.Dispose();
					//Environment.Exit(0);              
                    break;

            }
        }

        void uploadForm_ShowButtonForUpload(object sender, EventArgs e, bool show)
        {
            crtUserMenu.ToggleUploadButton(show);
        }

        public void GoProDeviceDetected(string driveLetter)
        {
            BindUserControl(driveLetter);
        }

        public void DeviceDisconnected()
        {
            if (uploadForm == null)
                uploadForm = new frmUploadForm();

            ucUserVideos uv = uploadForm.getUserVideosConrtol;
                        
            uv.lvItemsContainer.ItemsSource = new List<string>();

            //throw new NotImplementedException();
        }

        private void frmUserMainScreen_Load(object sender, EventArgs e)
        {
            crtUserMenu.UserMenuClickEvent -= crtUserMenu_UserMenuClickEvent;
            crtUserMenu.Upload_Click -= crtUserMenu_Upload_Click;

            crtUserMenu.UserMenuClickEvent += crtUserMenu_UserMenuClickEvent;
            crtUserMenu.Upload_Click += crtUserMenu_Upload_Click;
            if (this.InitialGoToUpload)
            {

                uploadForm = new frmUploadForm();
                uploadForm.Dock = DockStyle.Fill;
                uploadForm.ShowButtonForUpload -= uploadForm_ShowButtonForUpload;
                uploadForm.ShowButtonForUpload += uploadForm_ShowButtonForUpload;
                uploadForm.TopLevel = false;
                uploadForm_ShowButtonForUpload(sender, e, true);
                pnlPlaceHolder.Controls.Add(uploadForm);

                BindUserControl(InitialUploadPath);
                //uploadForm.SearchForVideos();
                uploadForm.Show();

            }
            else
            {
                frmArchive = new frmArchiveForm();
                frmArchive.TopLevel = false;
                pnlPlaceHolder.Controls.Add(frmArchive);
                frmArchive.Width = pnlPlaceHolder.Width;
                //frmArchive.WindowState = FormWindowState.Maximized;
                //frmArchive.Refresh();
                //frmArchive.PerformAutoScale();
                //frmArchive.
                //pnlPlaceHolder.Refresh();
                frmArchive.Show();
            }
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
        }

        private void frmUserMainScreen_SizeChanged(object sender, EventArgs e)
        {
            performResize();
        }


        public void performResize()
        {
            //MessageBox.Show(pnlPlaceHolder.Width.ToString() + " x " + pnlPlaceHolder.Height.ToString());
            foreach (Control ctr in pnlPlaceHolder.Controls)
            {
                if (ctr is IChildForm)
                {
                    SuspendLayout();
                    ((IChildForm)ctr).ChangeSize(pnlPlaceHolder.Width, pnlPlaceHolder.Height);
                    ResumeLayout();
                }
            }
        }

        private void frmUserMainScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isClosingFromLogout)
                return;

            List<Form> openedForms = Application.OpenForms.Cast<Form>().ToList();
            foreach (Form frmOpened in openedForms)
            {
                if (frmOpened.Name == "DetectorForm") continue;
                if (frmOpened.Name == "frmLogIn")
                {
                    frmOpened.Hide();
                    ((frmLogIn)frmOpened).ContextMenuItems(false);
                    continue;
                }
                if (frmOpened.Name == "frmProgressStatus")
                {
                    if (((frmProgressStatus)frmOpened).hasUploadCompleted)
                    {
                        frmOpened.Close();
                    }
                    else
                    {
                        ((frmProgressStatus)frmOpened).DisableCancelButtonsOnLogout(true);
                    }
                    continue;
                }
                else if (frmOpened.Name == this.Name)
                    continue;

                frmOpened.Close();
            }

            LoginAudit.WriteLoginAudit(CodeITConstants.LOGOUT_SUCCESSFULL);
            this.Dispose();
            Environment.Exit(0);
        }


        public void GoProMTPDeviceDetected()
        {
            Context.ProcessGoProCameraForUser();
        }
    }
}
