using CodeITBL;
using IntrensicMediaPlayer;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Threading;

namespace Intrensic
{
    public partial class frmArchiveForm : CustomForm, IChildForm
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(frmArchiveForm));

        public event ShowUploadButton ShowButtonForUpload;

        private BackgroundWorker worker = null;
        private BackgroundWorker searchWorker = null;

        public frmArchiveForm()
        {
            InitializeComponent();

            ucUserVideos.IsUpload = false;            
            dtpTo.MinDate = DateTime.Now.AddDays(-60);
            dtpFrom.MaxDate = DateTime.Now;
            dtpTo.MaxDate = DateTime.Now;
            lblName.Visible = Context.getCurrentUser.RoleId == (int)Role.Administrator;
            cbName.Visible = Context.getCurrentUser.RoleId == (int)Role.Administrator;

            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            FillUserList();

            this.ehArchiveContainer.BackgroundImage = Context.GetImageForCustomer();
            this.Resize += frmArchiveForm_Resize;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (files.Count > 0)
                {
                    ((ArchiveExpander)ehArchiveContainer.Child).BindLists(files);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            FillArchives();
        }

        private void FillUserList()
        {
            try
            {
                if (Context.getCurrentUser.RoleId == (int)Role.Administrator)
                {
                    List<CodeITDL.User> users = new List<CodeITDL.User>();
                    using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(Context.UserId))
                    {
                        users = ctx.Users.Where(x => x.CustomerId == Context.CustomerId).ToList();
                    }
                    users.Insert(0, new CodeITDL.User() { Id = -1, CustomerId = Context.CustomerId, FirstName = "Select All" });

                    cbName.DisplayMember = "FullName";
                    cbName.ValueMember = "Id";
                    cbName.DataSource = users;

                    if (cbName.Items.Count > 0)
                        cbName.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        void frmArchiveForm_Resize(object sender, EventArgs e)
        {
            //ehUserVideos.Width = pnlItemsContainer.Width;
            //ucUserVideos.Width = pnlItemsContainer.Width;
            //ucUserVideos.ControlWidht = pnlItemsContainer.Width;
            //if (ucUserVideos.itemContainer != null)
            //    ucUserVideos.itemContainer.Width = (pnlItemsContainer.Width - 21) > 0 ? pnlItemsContainer.Width - 21 : pnlItemsContainer.Width;

            ucUserVideos.InvalidateVisual();
            ehUserVideos.Invalidate();
            panel16.BackgroundImageLayout = ImageLayout.Stretch;
        }

        List<string> urlsForVideos = new List<string>();
        
        void uv_ContextMenuClick(object sender, EventArgs e, ContextAction action, List<string> videos)
        {
            if (action == ContextAction.Preview && videos.Count > 0)
            {
                List<FileFromDB> selectedFiles = new List<FileFromDB>();

                List<string> urls = new List<string>();
                foreach (CodeITBL.FileFromDB selectedFile in ucUserVideos.lvItemsContainer.SelectedItems)
                {					
					if(CodeITLicence.Licence.StorageType == CodeITLicence.StorageType.Cloud && selectedFile.IsCloudFileSystem == false)
					{
						System.Windows.MessageBox.Show(string.Format("Video {0} is stored local, please use local license", selectedFile.NewFileName), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
					}
					if (CodeITLicence.Licence.StorageType == CodeITLicence.StorageType.Local && selectedFile.IsCloudFileSystem == true)
					{
						System.Windows.MessageBox.Show(string.Format("Video {0} is stored on cloud, please use cloud license", selectedFile.NewFileName), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
					}
					else
						selectedFiles.Add(selectedFile);
                }

				if (selectedFiles.Count > 0)
				{
					VideoFromStream player = new VideoFromStream();
					player.LoadVideos(selectedFiles);
					player.Topmost = true;
					player.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
					player.ShowDialog();
				}
            }
        }
        

        private string foundDeviceWithVideos()
        {
            List<string> drives = DriveInfo.GetDrives().Where(x => x.DriveType == DriveType.Removable && x.IsReady).Select(x => x.RootDirectory.FullName).ToList();

            foreach (string drive in drives)
            {
                String dcimFolder = string.Format("{0}DCIM", drive);
                bool isGoPro = false;
                bool hasVideos = false;


                if (Directory.Exists(dcimFolder) && System.IO.Directory.GetFiles(dcimFolder, "*.mp4", SearchOption.AllDirectories).Length > 0)
                {
                    isGoPro = true;
                    hasVideos = true;
                }

                if (hasVideos && isGoPro)
                    return drive;
            }

            return string.Empty;
        }



        public void CheckIfUploadShouldBeShown()
        {
            if (ShowButtonForUpload != null)
                ShowButtonForUpload(this, null, false);
        }

        public System.Windows.Forms.Integration.ElementHost getElementHostControl { get { return this.ehUserVideos; } }
        public ucUserVideos getUserVideosConrtol { get { return this.ucUserVideos; } }

        List<CodeITBL.FileFromDB> files = new List<CodeITBL.FileFromDB>();

        private void FillArchives()
        {
            List<CodeITDL.User> UsersFromEntity = new List<CodeITDL.User>();
            List<CodeITDL.File> FilesFromEntity = new List<CodeITDL.File>();

            try
            {
                using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(Context.UserId))
                {
                    UsersFromEntity = ctx.Users.ToList();
                    FilesFromEntity = ctx.Files.ToList();
                }

                if (FilesFromEntity.Count > 0)
                {
                    DateTime limit = DateTime.Now.AddDays(6).Date;

                    List<CodeITDL.File> uploadedFiles = new List<CodeITDL.File>();
					bool isCloud = CodeITLicence.Licence.StorageType == CodeITLicence.StorageType.Cloud ? true : false;
                    if (Context.getCurrentUser.RoleId != (int)Role.Administrator)
                    {
                        uploadedFiles = FilesFromEntity.Where(x => x.UserId == Context.UserId && x.OriginalFileDate <= limit && x.IsCloudFileSystem == isCloud).OrderByDescending(x => x.CreatedOn).ToList();
                    }
                    else
                    {
                        uploadedFiles = FilesFromEntity.Where(x => x.OriginalFileDate <= limit && x.IsCloudFileSystem == isCloud && UsersFromEntity.Where(y => y.CustomerId == Context.CustomerId).Select(y => y.Id).Contains(x.UserId)).OrderByDescending(x => x.CreatedOn).ToList();
                    }

                    List<CodeITDL.User> users = UsersFromEntity.Where(x => x.CustomerId == Context.CustomerId).ToList();

                    if (uploadedFiles != null && uploadedFiles.Count > 0)
                    {
                        for (int i = 0; i < uploadedFiles.Count; i++)
                        {
                            FileFromDB file = new FileFromDB(uploadedFiles[i], i);

                            file.UserName = (file.UserId == Context.UserId ?
                                                (Context.getCurrentUser.FirstName + " " + Context.getCurrentUser.LastName) :
                                                UsersFromEntity.Where(u => u.Id == file.UserId).Select(u => (u.FirstName + " " + u.LastName)).FirstOrDefault());

                            files.Add(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void frmArchiveForm_Load(object sender, EventArgs e)
        {
            ((ArchiveExpander)ehArchiveContainer.Child).SetNewHeight(this.Height);

            worker.RunWorkerAsync();

			cbSearchBy.SelectedIndex = 0;
        }

        private void ehUserVideos_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void frmArchiveForm_Shown(object sender, EventArgs e)
        {
            int height = this.Height;
            if (this.Parent != null)
                if (this.Parent is MyPanel)
                    height = this.Parent.Height;
            //pnlItemsContainer.HorizontalScroll.Enabled = false;
            ((ArchiveExpander)ehArchiveContainer.Child).SetNewHeight(height);

        }

        public void ChangeSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            SuspendLayout();
            width = this.Width > 300 ? this.Width - 300 : this.Width;
            ehUserVideos.Dock = DockStyle.Fill;
            ucUserVideos.ControlWidht = width;

            //pnlItemsContainer.Width = width;
            //pnlItemsContainer.Height = height;

            ehArchiveContainer.Dock = DockStyle.Left;


            height = this.Height;
            if (this.Parent != null)
                if (this.Parent is MyPanel)
                    height = this.Parent.Height;

            ((ArchiveExpander)ehArchiveContainer.Child).SetNewHeight(height);

            ehArchiveContainer.Height = height;
            ((ArchiveExpander)ehArchiveContainer.Child).SetNewHeight(height);

            ResumeLayout();

            this.Refresh();
        }

        private void FillArchivesFromSearch()
        {
            List<CodeITDL.User> UsersFromEntity = new List<CodeITDL.User>();
            List<CodeITDL.File> FilesFromEntity = new List<CodeITDL.File>();

            try
            {
                ((frmUserMainScreen)this.ParentForm).performResize();
                Cursor.Current = Cursors.WaitCursor;


                List<int> users = new List<int>();
                files = new List<FileFromDB>();

                using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(Context.UserId))
                {
                    UsersFromEntity = ctx.Users.ToList();
                    FilesFromEntity = ctx.Files.ToList();
                }
				bool isCloud = CodeITLicence.Licence.StorageType == CodeITLicence.StorageType.Cloud ? true : false;
                if (Context.getCurrentUser.RoleId != (int)Role.Administrator)
                    users.Add(Context.UserId);
                else
                {
                    users = UsersFromEntity.Where(x => x.CustomerId == Context.CustomerId && (x.Id == (int)cbName.SelectedValue || (int)cbName.SelectedValue == -1)).Select(x => x.Id).ToList();
                }
                List<CodeITDL.File> tmpList = new List<CodeITDL.File>();
                DateTime to = dtpTo.Value.Date.AddHours(23).AddMinutes(59);
                if (cbSearchBy.SelectedIndex == 0)
                {
                    tmpList = FilesFromEntity.Where(x => x.OriginalFileDate >= dtpFrom.Value.Date && x.OriginalFileDate <= to && x.IsCloudFileSystem == isCloud && users.Contains(x.UserId)).OrderBy(x => x.NewFileName).ThenBy(x=>x.CreatedOn.Date).ToList();
                }
                else
                {
					tmpList = FilesFromEntity.Where(x => x.CreatedOn >= dtpFrom.Value.Date && x.CreatedOn <= to && x.IsCloudFileSystem == isCloud && users.Contains(x.UserId)).OrderBy(x => x.NewFileName).ThenBy(x => x.CreatedOn.Date).ToList();
                }

                if (tmpList != null && tmpList.Count > 0)
                {
                    for (int i = 0; i < tmpList.Count; i++)
                    {
                        FileFromDB fileDb = new FileFromDB(tmpList[i], i);
                        fileDb.UserName = (fileDb.UserId == Context.UserId ?
                                            (Context.getCurrentUser.FirstName + " " + Context.getCurrentUser.LastName) :
                                            UsersFromEntity.Where(u => u.Id == fileDb.UserId).Select(u => (u.FirstName + " " + u.LastName)).FirstOrDefault());
                        files.Add(fileDb);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void BindArchivesInSearch()
        {
            try
            {
                ucUserVideos.lvItemsContainer.ContextMenu.Visibility = System.Windows.Visibility.Hidden;
                ucUserVideos.lvItemsContainer.ContextMenu.IsOpen = false;
                ucUserVideos.IsUpload = false;
                ucUserVideos.lvItemsContainer.ItemsSource = files;

                ucUserVideos.ContextMenuClick -= uv_ContextMenuClick;
                ucUserVideos.ContextMenuClick += uv_ContextMenuClick;

                if (files.Count > 0)
                {
                    ((ArchiveExpander)ehArchiveContainer.Child).BindLists(files, cbSearchBy.SelectedIndex == 0 ? false : true);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FillArchivesFromSearch();

            BindArchivesInSearch();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            ucUserVideos.lvItemsContainer.SelectAll();            
            
        }

        private void btnClearSelection_Click(object sender, EventArgs e)
        {
            ucUserVideos.lvItemsContainer.SelectedItem = null;
        }
    }
}
