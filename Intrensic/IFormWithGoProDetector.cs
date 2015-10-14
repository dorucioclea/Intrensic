using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intrensic
{
    interface IFormWithGoProDetector
    {
        void GoProDeviceDetected(string driveLetter);
        void GoProMTPDeviceDetected();
        void DeviceDisconnected();
    }
}
