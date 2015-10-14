using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intrensic
{
    public class ChildForm : Form
    {
        public ChildForm() : base()
        {
            this.TopLevel = false;
            this.Dock = DockStyle.Fill;            
            this.ControlBox = false;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ChildForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChildForm";
            this.ResumeLayout(false);

        }
    }
}
