using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeITBL
{
    public class TaskProgressReport
    {
        public string Name { get; set; }
        public int PercentComplete { get; set; }
        public long CurrentStep { get; set; }
        public long TotalSteps { get; set; }
    }
}
