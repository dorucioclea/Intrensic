using CodeITBL;
using IntrensicMediaPlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Intrensic
{
    public partial class frmUploadForm : CustomForm
    {
        public event ShowUploadButton ShowButtonForUpload;

        public frmUploadForm()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            ucUserVideos.IsUpload = true;
            this.BackgroundImage = Context.GetImageForCustomer();
        }

        public void CheckForMediaOnGoProDevice()
        {
            Context.ProcessGoProCameraForUser();
        }

        public void SearchForVideos()
        {

            string path = foundDeviceWithVideos();

            if (path == string.Empty)
            {
                MessageBox.Show("No device with videos found, please insert the device and try again");

                if (ShowButtonForUpload != null)
                    ShowButtonForUpload(this, null, false);
                return;
            }


            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] allFiles = dirInfo.GetFiles("*.MP4", SearchOption.AllDirectories);

            List<CodeITDL.File> list = new List<CodeITDL.File>();


            foreach (FileInfo fi in allFiles)
            {
                list.Add(new CodeITDL.File() { OriginalFileName = fi.Name, OriginalFileLocation = fi.FullName, NewFileName = Guid.NewGuid().ToString().Replace("-", string.Empty), OriginalFileDate = fi.CreationTime, isFromCard = true, UserName = fi.Name });
            }

            ucUserVideos.lvItemsContainer.ContextMenu.Visibility = System.Windows.Visibility.Hidden;
            ucUserVideos.lvItemsContainer.ContextMenu.IsOpen = false;
            ucUserVideos.IsUpload = true;
            ucUserVideos.lvItemsContainer.ItemsSource = list;
            //if (ucUserVideos.ContextMenuClick == null)

            ucUserVideos.ContextMenuClick -= uv_ContextMenuClick;
            ucUserVideos.ContextMenuClick += uv_ContextMenuClick;

            if (ShowButtonForUpload != null)
                ShowButtonForUpload(this, null, list.Count > 0);

        }

        void uv_ContextMenuClick(object sender, EventArgs e, ContextAction action, List<string> videos)
        {
            if (action == ContextAction.Preview && videos.Count > 0)
            {
                /*MainWindow player = new MainWindow(videos, false);
                player.Topmost = true;
                player.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                player.ShowDialog();*/
                // TODO: Replace with AxPlayer

                VideoFromStream player = new VideoFromStream(true);
                player.LoadVideos(videos);
                player.Topmost = true;
                player.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                player.ShowDialog();

            }
        }

        private string foundDeviceWithVideos()
        {
            List<string> drives = DriveInfo.GetDrives().Where(x => x.DriveType == DriveType.Removable && x.IsReady).Select(x => x.RootDirectory.FullName).ToList();

            foreach (string drive in drives)
            {
                //String miscFolder = string.Format("{0}MISC", drive);
                //String versionFile = string.Format("{0}MISC\\version.txt", drive);
                String dcimFolder = string.Format("{0}DCIM", drive);
                bool isGoPro = false;
                bool hasVideos = false;

                //if (Directory.Exists(miscFolder) && File.Exists(versionFile))
                //    isGoPro = true;

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
                ShowButtonForUpload(this, null, ucUserVideos.lvItemsContainer.Items.Count > 0);
        }

        public System.Windows.Forms.Integration.ElementHost getElementHostControl { get { return this.ehUserVideos; } }
        public ucUserVideos getUserVideosConrtol { get { return this.ucUserVideos; } }

        private void frmUploadForm_SizeChanged(object sender, EventArgs e)
        {

            ehUserVideos.Dock = DockStyle.Fill;
            ehUserVideos.Width = this.Width;
            ehUserVideos.Height = this.Height;
            ucUserVideos.Width = this.Width;
            ucUserVideos.Height = this.Height;
            ucUserVideos.ControlWidht = this.Width;

            if (ucUserVideos.itemContainer != null)
            {
                ucUserVideos.itemContainer.Width = this.Width - 21 > 0 ? this.Width - 21 : this.Width;
            }
            
        }

        private void btnClearSelection_Click(object sender, EventArgs e)
        {
            ucUserVideos.lvItemsContainer.SelectedItem = null;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            ucUserVideos.lvItemsContainer.SelectAll();
        }
    }
}
