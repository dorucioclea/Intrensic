using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intrensic
{
    public partial class frmLoading : Form
    {
        public frmLoading()
        {
            InitializeComponent();
        }

        [DllImport("user32")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        private void frmLoading_Shown(object sender, EventArgs e)
        {
            SetForegroundWindow(Handle);
            this.BringToFront();
            Activate();
        }
    }
}
