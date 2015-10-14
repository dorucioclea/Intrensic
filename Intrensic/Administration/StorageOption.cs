using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeITLicence;

namespace Intrensic.Administration
{
	public partial class StorageOption : Form
	{		
		private const string DEFAULT_CLOUD_ACCESS_KEY = "AKIAJ4TQBWDKLMZAQKDA";
		private const string DEFAULT_CLOUD_SECRET_ACCESS_KEY = "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";
		private readonly ILog _logger = LogManager.GetLogger(typeof(frmUploadMessageBox));
		public StorageOption()
		{
			InitializeComponent();
			if (Licence.StorageType == StorageType.Cloud)
			{
				rbtnCloud.Checked = true;
			}
			else
			{
				rbtnLocal.Checked = true;
			}
		}
			

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				if (rbtnCloud.Checked)
				{
					Licence.StorageType = StorageType.Cloud;
					Licence.CloudId = DEFAULT_CLOUD_ACCESS_KEY;
					Licence.CloudSecret = DEFAULT_CLOUD_SECRET_ACCESS_KEY;
					Licence.LocalPassword = string.Empty;
					Licence.LocalPath = string.Empty;
					Licence.LocalUsername = string.Empty;
				}
				else
				{
					Licence.StorageType = StorageType.Local;
					Licence.LocalPath = txtLocalPath.Text;
					Licence.LocalUsername = txtLocalUsername.Text;
					Licence.LocalPassword = txtLocalPassword.Text;
					Licence.CloudId = string.Empty;
					Licence.CloudSecret = string.Empty;
				}

				if (!Licence.IsValid())
				{
					StringBuilder sb = new StringBuilder();

					foreach (string message in Licence.ValidationMessagesList)
					{
						sb.AppendLine(message);
					}

					MessageBox.Show(sb.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

					return;
				}

				string currentLicense = Licence.GenerateKey();

				CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext();
				CodeITDL.License lic = ctx.Licenses.Where(c => c.CustomerId.ToString() == Licence.ClientId).FirstOrDefault();
				if (lic != null)
				{
					lic.LicenseBytes = Encoding.UTF8.GetBytes(currentLicense);
				}
				else
				{
					CodeITDL.License newLic = new CodeITDL.License();
					newLic.LicenseBytes = Encoding.UTF8.GetBytes(currentLicense);
					newLic.CustomerId = Guid.Parse(CodeITLicence.Licence.ClientId);
					ctx.Licenses.Add(newLic);

				}
				ctx.SaveChanges();
				MessageBox.Show("Storage settings are successfully updated.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
				MessageBox.Show("Storage settings are not updated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void rbtnCloud_CheckedChanged(object sender, EventArgs e)
		{
			if (rbtnCloud.Checked)
			{
				gbLocalSettings.Visible = false;
			}
			else
			{
				gbLocalSettings.Visible = true;
				txtLocalPassword.Text = Licence.LocalPassword;
				txtLocalPath.Text = Licence.LocalPath;
				txtLocalUsername.Text = Licence.LocalUsername;
			}
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			DialogResult result = folderBrowserDialog1.ShowDialog();
			if (result == DialogResult.OK)
			{
				txtLocalPath.Text = folderBrowserDialog1.SelectedPath;
			}
		}
	}
}
