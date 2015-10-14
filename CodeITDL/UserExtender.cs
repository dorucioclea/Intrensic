using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeITDL
{
    public partial class User
    {
        [NotMapped]
        public string FullName
        {
			get
			{
				if (FirstName == "Select All")
					return string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);
				else
					return string.Format("{0} {1} {2} ({3})", FirstName, MiddleName, LastName, UserName);
			}
        }
    }
}
