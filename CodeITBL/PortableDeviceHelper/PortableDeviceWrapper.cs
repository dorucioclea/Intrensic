using PortableDevices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeITBL.PortableDeviceHelper
{
    public static class PortableDeviceWrapper
    {
        static List<PortableDeviceFile> files = new List<PortableDeviceFile>();

        static string currentDeviceId = string.Empty;

        static PortableDevice gopro;

        public static void ConnectDevice()
        {
            var devices = new PortableDeviceCollection();
            devices.Refresh();

            gopro = devices.First();
            gopro.Connect();
            currentDeviceId = gopro.DeviceId;
        }

        public static bool IsDeviceConnected()
        {
            return !string.IsNullOrEmpty(currentDeviceId);
        }

        public static List<PortableDeviceFileClass> getFiles()
        {
            List<PortableDeviceFileClass> result = new List<PortableDeviceFileClass>();

            if (gopro == null)
                return result;

            //ne e toj device
            if (gopro.DeviceId != currentDeviceId)
                return result;


            var root = gopro.GetContents();


            var filesOnDevice = from f in root.Files select f;


            foreach (var file in filesOnDevice)
            {
                if (file is PortableDeviceFile)
                    files.Add(file as PortableDeviceFile);
                else if (file is PortableDeviceFolder)
                    GetAllFilesFromFolder(file as PortableDeviceFolder);
            }


            PortableDeviceApiLib.IPortableDeviceContent pContent;
            gopro.PortableDeviceClass.Content(out pContent);

            PortableDeviceApiLib.IPortableDeviceProperties pProperties;
            pContent.Properties(out pProperties);



            if (files.Count > 0)
                foreach (PortableDeviceFile file in files)
                {
                    PortableDeviceApiLib.IPortableDeviceKeyCollection keys;
                    pProperties.GetSupportedProperties(file.Id, out keys);


                    PortableDeviceApiLib.IPortableDeviceValues values;
                    pProperties.GetValues(file.Id, keys, out values);

                    string name = string.Empty;
                    var property = new PortableDeviceApiLib._tagpropertykey();
                    property.fmtid = new Guid(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C);
                    property.pid = 12;
                    try
                    {
                        values.GetStringValue(property, out name);
                    }
                    catch (Exception ex) { name = string.Empty; } //OVDE DA SE NAPRAVI NESHTO


                    string createdOn = string.Empty;
                    property = new PortableDeviceApiLib._tagpropertykey();
                    property.fmtid = new Guid(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C);
                    property.pid = 18;
                    try
                    {
                        values.GetStringValue(property, out createdOn);
                    }
                    catch (Exception ex) { createdOn = string.Empty; } //OVDE DA SE NAPRAVI NESHTO

                    if (name.ToLower().EndsWith(".mp4"))
                        result.Add(new PortableDeviceFileClass() { CreatedOn = string.IsNullOrEmpty(createdOn) ? new DateTime?() : DateTime.Parse(createdOn), Location = System.IO.File.Exists(name) ? name : string.Empty, Name = name, PortableDeviceFileObject = file });
                }


            return result;
        }

        public static void GetFile(PortableDeviceFile file, out StreamWrapper outputStream)
        {

            gopro.DownloadFile(file, out outputStream);

            //outputStream.Flush();
            //outputStream.Close();
        }


        static void GetAllFilesFromFolder(PortableDeviceFolder folder)
        {
            foreach (var file in folder.Files)
            {
                if (file is PortableDeviceFile)
                    files.Add(file as PortableDeviceFile);
                else if (file is PortableDeviceFolder)
                    GetAllFilesFromFolder(file as PortableDeviceFolder);
            }
        }
    }
}
