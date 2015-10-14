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
    public partial class ctrlUserInfo : UserControl
    {
        public ctrlUserInfo()
        {
            InitializeComponent();

        }

        public void BindLabels()
        {
            CodeITDL.User user = Context.getCurrentUser;

            lblId.Text = "#"+user.IdNumber;
            string fname = user.FirstName;
            string midname = user.MiddleName;
            string lastname = user.LastName;
            if (midname != null)
                if (midname.Length > 3)
                    midname = (midname.Substring(0, 2) + ".");
            string name = string.Empty;
            name = string.Format("{0} {1} {2}", fname, midname, lastname);
            lblName.Text = name;
            lblRole.Text = Enum.GetName(typeof(Role), user.RoleId);
        }

    }
}
