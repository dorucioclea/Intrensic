using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CodeITLicence
{
    public partial class frmLicence : Form
    {
        public frmLicence()
        {
            InitializeComponent();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            Licence.ParseFromString(txtLicence.Text);

            if (!Licence.IsValid())
            {
                Licence.ValidationMessagesList.Clear();
                Licence.ValidationMessagesList.Add("Licence is not valid!");
                LoadMessages();
                return;
            }

            File.WriteAllText("license.txt", txtLicence.Text, Encoding.UTF8);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Application.Exit();
        }

        private void frmLicence_Load(object sender, EventArgs e)
        {
            LoadMessages();
        }

        private void LoadMessages()
        {
            lblInfo.Text = String.Empty;
            var sb = new StringBuilder();
            foreach (var message in Licence.ValidationMessagesList)
            {
                sb.AppendLine(message);
            }
            sb.AppendLine("Please provide valid licence key and click continue.");
            lblInfo.Text = sb.ToString();
        }

        private void frmLicence_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
