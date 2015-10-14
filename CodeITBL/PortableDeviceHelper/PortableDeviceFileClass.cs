using PortableDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeITBL.PortableDeviceHelper
{
    public class PortableDeviceFileClass
    {
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Location { get; set; }
        public PortableDeviceFile PortableDeviceFileObject { get; set; }
        public string DeviceId { get; set; }
    }
}
