using CodeITDL;
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

namespace Intrensic.Administration
{
    public partial class frmSettings : Form
    {

        public frmSettings()
        {
            InitializeComponent();
            this.BackgroundImage = Context.GetImageForCustomer();

            this.txtTempLocation.Text = (Context.getTempFolderLocation() == System.IO.Path.GetTempPath()) ? string.Empty : Context.getTempFolderLocation();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(Context.UserId))
            {

                Guid clientId = new Guid();
                Guid.TryParse(CodeITLicence.Licence.ClientId, out clientId);


                Setting tmpLocation = ctx.Settings.Where(x => x.CustomerId == clientId && x.Name == CodeITConstants.SETTINGS_TEMP_LOCATION).FirstOrDefault();

                if (tmpLocation != null)
                {
                    if (tmpLocation.Id > 0)
                        tmpLocation.Value = txtTempLocation.Text.Trim();
                    else
                        tmpLocation = new Setting() { Value = txtTempLocation.Text.Trim(), Name = CodeITConstants.SETTINGS_TEMP_LOCATION, CustomerId = Guid.Parse(CodeITLicence.Licence.ClientId) };
                }else
                {
                    tmpLocation = new Setting() { Value = txtTempLocation.Text.Trim(), Name = CodeITConstants.SETTINGS_TEMP_LOCATION, CustomerId = clientId };
                }

                if (tmpLocation.Id <= 0)
                    ctx.Settings.Add(tmpLocation);

                ctx.SaveChanges();

                MessageBox.Show("Settings saved successfully");
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (txtTempLocation.Text.Trim() == string.Empty) return;

            if (System.IO.Directory.Exists(txtTempLocation.Text.Trim()))
            {
                try
                {
                    string filename = txtTempLocation.Text.Trim().TrimEnd('\\') + "\\testpermission.txt";
                    using (FileStream fstream = new FileStream(filename, FileMode.Create))
                    using (TextWriter writer = new StreamWriter(fstream))
                    {
                        writer.WriteLine("test");
                    }

                    if (System.IO.File.Exists(filename))
                        System.IO.File.Delete(filename);

                    MessageBox.Show("Directory is successfully accessed");
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show("Directory is not writable");
                    return;                  
                }
                
            }
            else
                MessageBox.Show("Directory is missing or not accessible");
        }
    }
}
