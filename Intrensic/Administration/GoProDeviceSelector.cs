using PodcastUtilities.PortableDevices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intrensic.Administration
{
    public partial class frmGoProDeviceSelector : Form
    {

        public string SelectedDevice { get; set; }
        public frmGoProDeviceSelector()
        {
            InitializeComponent();
            this.Icon = Intrensic.Properties.Resources.Intrensic;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            IDeviceManager manager = new DeviceManager();
            IEnumerable<IDevice> devices = manager.GetAllDevices();

            lvDevices.Items.Clear();
            foreach (var device in devices)
            {
                ListViewItem lvi = new ListViewItem(device.Name);                
                lvi.SubItems.Add(Regex.Match(device.Id,
                             @"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b",
                             RegexOptions.IgnoreCase).Value);
                lvi.SubItems.Add(device.Serial);
                lvi.Tag = device.Id + "||" + device.Serial;
                lvDevices.Items.Add(lvi);                
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (lvDevices.Items.Count == 0)
            {
				MessageBox.Show(this, "There are no devices in the list, please attach a device and refresh the list", "Info",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lvDevices.SelectedItems.Count == 0)
            {
				MessageBox.Show(this, "Please select a device from the list", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SelectedDevice = lvDevices.SelectedItems[0].Tag.ToString();

            this.Close();
        }

        private void GoProDeviceSelector_Load(object sender, EventArgs e)
        {
            btnRefresh.PerformClick();
        }
    }
}
