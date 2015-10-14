using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intrensic
{

    public partial class crtUserMenu : UserControl
    {
        public event StartUploadProcess Upload_Click;
        public crtUserMenu()
        {
            InitializeComponent();

            CodeITDL.User currentUser = Context.getCurrentUser;


            btnManageUsers.Visible = btnStorageOption.Visible = currentUser.RoleId == (int)Role.Administrator;
            pnlSeparatorUsers.Visible = currentUser.RoleId == (int)Role.Administrator;

            pnlSeparatorAudit.Visible = pnlSeparatorSettings.Visible = false;


			btnManageUsers.Visible = btnAuditLog.Visible = btnStorageOption.Visible = currentUser.RoleId == (int)Role.Administrator;
			pnlSeparatorAudit.Visible = pnlSeparatorSettings.Visible = pnlSeparatorUsers.Visible = currentUser.RoleId == (int)Role.Administrator;

           //btnSettings.Visible = false;

            


        }

        public event UserMenuClick UserMenuClickEvent;


        private void btnArchive_Click(object sender, EventArgs e)
        {
            if (UserMenuClickEvent != null)
                UserMenuClickEvent(sender, e, "Archive");
            ToggleUploadButton(false);
        }

        private void btnUploadVideos_Click(object sender, EventArgs e)
        {
            if (UserMenuClickEvent != null)
            {
                UserMenuClickEvent(sender, e, "Upload");                
            }
        }

        public void ToggleUploadButton(bool show)
        {
            btnStartUpload.Visible = show;
        }

        private void btnStartUpload_Click(object sender, EventArgs e)
        {
            if (Upload_Click != null)
                Upload_Click(sender, e);
            
        }
        //private void btnSettings_Click(object sender, EventArgs e)
        //{
        //    if (UserMenuClickEvent != null)
        //        UserMenuClickEvent(sender, e, "Settings");
        //    ToggleUploadButton(false);
        //}


        private void crtUserMenu_Load(object sender, EventArgs e)
        {
            ctrlUserInfo.BindLabels();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (UserMenuClickEvent != null)
                UserMenuClickEvent(sender, e, "Settings");
            ToggleUploadButton(false);
        }

        private void btnAuditLog_Click(object sender, EventArgs e)
        {
            if (UserMenuClickEvent != null)
                UserMenuClickEvent(sender, e, "Audit");
            ToggleUploadButton(false);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (UserMenuClickEvent != null)
                UserMenuClickEvent(sender, e, "Logout");
            ToggleUploadButton(false);
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            if (UserMenuClickEvent != null)
                UserMenuClickEvent(sender, e, "Users");
        }

		private void btnStorageOption_Click(object sender, EventArgs e)
		{
			if (UserMenuClickEvent != null)
				UserMenuClickEvent(sender, e, "StorageOption");			
		}


    }
}
