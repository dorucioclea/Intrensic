using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeITDL
{
    public partial class File
    {     
        [NotMapped]
        public bool isFromCard { get; set; }
        [NotMapped]
        public bool HasNote
        {
            get
            {
                return !string.IsNullOrEmpty(Note);
            }
        }

        [NotMapped]
        public string UserName { get; set; }

        [NotMapped]
        public object FileObject { get; set; }

    }
}
