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
using System.Windows.Forms;

namespace Intrensic
{
    public partial class UsbDetector : Form
    {

        private const int DBT_DEVTYP_VOLUME = 0x00000002;


        public UsbDetector()
        {

            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Load += new System.EventHandler(this.Load_Form);
            this.Activated += new EventHandler(this.Form_Activated);

            this.Show();

        }

        void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(314, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "This is invisible form. To see DriveDetector code click View Code";
            // 
            // DetectorForm
            // 
            this.ClientSize = new System.Drawing.Size(360, 80);
            this.Controls.Add(this.label1);
            this.Name = "DetectorForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void Form_Activated(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void Load_Form(object sender, EventArgs e)
        {
            // We don't really need this, just to display the label in designer ...
            InitializeComponent();

            // Create really small form, invisible anyway.
            this.Size = new System.Drawing.Size(5, 5);

            UsbNotification.RegisterUsbDeviceNotification(this.Handle);
        }

        char c;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == UsbNotification.WmDevicechange)
            {
                switch ((int)m.WParam)
                {
                    case UsbNotification.DbtDeviceremovecomplete:
                        //Console.WriteLine("removed");
                        //Usb_DeviceRemoved(); // this is where you do your magic
                        break;
                    case UsbNotification.DbtDevicearrival:
                        var devType = System.Runtime.InteropServices.Marshal.ReadInt32(m.LParam, 4);
                        if (devType == DBT_DEVTYP_VOLUME)
                        {
                            DEV_BROADCAST_VOLUME vol;
                            vol = (DEV_BROADCAST_VOLUME)
                                Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));

                            c = DriveMaskToLetter(vol.dbcv_unitmask);
                            
                            GoProDriveLetter(c.ToString() + ":\\");

                        }
                        else
                            GoProMTPDeviceDetected();
                        //Console.WriteLine("arrived");
                        //Usb_DeviceAdded(); // this is where you do your magic
                        break;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_VOLUME
        {
            public int dbcv_size;
            public int dbcv_devicetype;
            public int dbcv_reserved;
            public int dbcv_unitmask;
        }


        private void GoProMTPDeviceDetected()
        {
            Context.ProcessGoProCameraForUser();
        }

        private void GoProDriveLetter(string driveLetter)
        {
            // e.Drive is the drive letter, e.g. "E:\\"
            // If you want to be notified when drive is being removed (and be
            // able to cancel it),
            // set HookQueryRemove to true

            //String miscFolder = string.Format("{0}MISC", e.Drive);
            //String versionFile = string.Format("{0}MISC\\version.txt", e.Drive);
            String dcimFolder = string.Format("{0}DCIM", driveLetter);
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
            {
                for (int i = 0; i < System.Windows.Forms.Application.OpenForms.Count; i++)
                {
                    System.Windows.Forms.Form frm = System.Windows.Forms.Application.OpenForms[i];
                    if (frm is IFormWithGoProDetector)
                    {
                        ((IFormWithGoProDetector)frm).GoProDeviceDetected(driveLetter);
                    }
                }
            }
        }

        /// <summary>
        /// Gets drive letter from a bit mask where bit 0 = A, bit 1 = B etc.
        /// There can actually be more than one drive in the mask but we 
        /// just use the last one in this case.
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        private static char DriveMaskToLetter(int mask)
        {
            char letter;
            string drives = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // 1 = A
            // 2 = B
            // 4 = C...
            int cnt = 0;
            int pom = mask / 2;
            while (pom != 0)
            {
                // while there is any bit set in the mask
                // shift it to the righ...                
                pom = pom / 2;
                cnt++;
            }

            if (cnt < drives.Length)
                letter = drives[cnt];
            else
                letter = '?';

            return letter;
        }
    }
}
