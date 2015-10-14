using Intrensic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace IntrensicService
{
    public partial class IntrensicService : ServiceBase
    {
        private ILog _logger = null;// LogManager.GetLogger(typeof(IntrensicService));
        public IntrensicService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _logger = LogManager.GetLogger(typeof(IntrensicService));
            RegisterDeviceNotification();

            fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Created += new System.IO.FileSystemEventHandler(fileSystemWatcher_Created);
            fileSystemWatcher.Deleted += new System.IO.FileSystemEventHandler(fileSystemWatcher_Deleted);
            fileSystemWatcher.Changed += new System.IO.FileSystemEventHandler(fileSystemWatcher_Changed);
            fileSystemWatcher.Renamed += new System.IO.RenamedEventHandler(fileSystemWatcher_Renamed);

        }

        protected override void OnStop()
        {
           
        }

        

        #region Fields

        private FileSystemWatcher fileSystemWatcher;
        private IntPtr deviceNotifyHandle;
        private IntPtr deviceEventHandle;
        private IntPtr directoryHandle;
        private Win32.ServiceControlHandlerEx myCallback;

        #endregion

        #region USB Detection

        private int ServiceControlHandler(int control, int eventType, IntPtr eventData, IntPtr context)
        {
            if (control == Win32.SERVICE_CONTROL_STOP || control == Win32.SERVICE_CONTROL_SHUTDOWN)
            {
                UnregisterHandles();
                Win32.UnregisterDeviceNotification(deviceEventHandle);

                base.Stop();
            }
            else if (control == Win32.SERVICE_CONTROL_DEVICEEVENT)
            {
                switch (eventType)
                {
                    case Win32.DBT_DEVICEARRIVAL:
                        //Win32.DEV_BROADCAST_HDR hdr;
                        //hdr = (Win32.DEV_BROADCAST_HDR)
                        //    Marshal.PtrToStructure(eventData, typeof(Win32.DEV_BROADCAST_HDR));

                        //if (hdr.dbcc_devicetype == Win32.DBT_DEVTYP_DEVICEINTERFACE)
                        //{
                        //    Win32.DEV_BROADCAST_DEVICEINTERFACE deviceInterface;
                        //    deviceInterface = (Win32.DEV_BROADCAST_DEVICEINTERFACE)
                        //        Marshal.PtrToStructure(eventData, typeof(Win32.DEV_BROADCAST_DEVICEINTERFACE));
                        //    string name = new string(deviceInterface.dbcc_name);
                        //    name = name.Substring(0, name.IndexOf('\0')) + "\\";

                        //    StringBuilder stringBuilder = new StringBuilder();
                        //    Win32.GetVolumeNameForVolumeMountPoint(name, stringBuilder, 100);

                        //    uint stringReturnLength = 0;
                        //    string driveLetter = "";

                        //    Win32.GetVolumePathNamesForVolumeNameW(stringBuilder.ToString(), driveLetter, (uint)driveLetter.Length, ref stringReturnLength);
                        //    if (stringReturnLength == 0)
                        //    {
                        //        // TODO handle error
                        //    }

                        //    driveLetter = new string(new char[stringReturnLength]);

                        //    if (!Win32.GetVolumePathNamesForVolumeNameW(stringBuilder.ToString(), driveLetter, stringReturnLength, ref stringReturnLength))
                        //    {
                        //        // TODO handle error
                        //    }

                        //    RegisterForHandle(driveLetter[0]);

                        //    fileSystemWatcher.Path = driveLetter[0] + ":\\";
                        //    fileSystemWatcher.EnableRaisingEvents = true;
                        //}
                        //break;
                        GoProMTPDeviceDetected();
                        break;
                    case Win32.DBT_DEVICEQUERYREMOVE:
                        UnregisterHandles();
                        fileSystemWatcher.EnableRaisingEvents = false;
                        break;
                }
            }

            return 0;
        }

        private void GoProMTPDeviceDetected()
        {           
            if(Context.CheckForGoProDevice())
            {
                _logger.Info("Check if exists Intrensic.exe in processes");
                if (Process.GetProcessesByName("Intrensic").Length == 0)
                {
                    _logger.Info("Intrensic.exe not exists in processes, start Intrensic app");
                    string apppath = string.Empty;
                    apppath = Path.Combine("C:\\Program Files (x86)\\Intrensic\\Intrensic Installer", "Intrensic.exe");
                    //Process.Start(apppath);
                    ApplicationLoader.PROCESS_INFORMATION procInfo;
                    ApplicationLoader.StartProcessAndBypassUAC(apppath, out procInfo);                    
                }
            }//.ProcessGoProCameraForService(apppath);
        }

        private void UnregisterHandles()
        {
            if (directoryHandle != IntPtr.Zero)
            {
                Win32.CloseHandle(directoryHandle);
                directoryHandle = IntPtr.Zero;
            }
            if (deviceNotifyHandle != IntPtr.Zero)
            {
                Win32.UnregisterDeviceNotification(deviceNotifyHandle);
                deviceNotifyHandle = IntPtr.Zero;
            }
        }

        private void RegisterForHandle(char c)
        {
            Win32.DEV_BROADCAST_HANDLE deviceHandle = new Win32.DEV_BROADCAST_HANDLE();
            int size = Marshal.SizeOf(deviceHandle);
            deviceHandle.dbch_size = size;
            deviceHandle.dbch_devicetype = Win32.DBT_DEVTYP_HANDLE;
            directoryHandle = CreateFileHandle(c + ":\\");
            deviceHandle.dbch_handle = directoryHandle;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(deviceHandle, buffer, true);
            deviceNotifyHandle = Win32.RegisterDeviceNotification(this.ServiceHandle, buffer, Win32.DEVICE_NOTIFY_SERVICE_HANDLE);
            if (deviceNotifyHandle == IntPtr.Zero)
            {
                // TODO handle error
            }
        }

        private void RegisterDeviceNotification()
        {
            myCallback = new Win32.ServiceControlHandlerEx(ServiceControlHandler);
            Win32.RegisterServiceCtrlHandlerEx(this.ServiceName, myCallback, IntPtr.Zero);

            if (this.ServiceHandle == IntPtr.Zero)
            {
                // TODO handle error
            }

            Win32.DEV_BROADCAST_DEVICEINTERFACE deviceInterface = new Win32.DEV_BROADCAST_DEVICEINTERFACE();
            int size = Marshal.SizeOf(deviceInterface);
            deviceInterface.dbcc_size = size;
            deviceInterface.dbcc_devicetype = Win32.DBT_DEVTYP_DEVICEINTERFACE;
            IntPtr buffer = default(IntPtr);
            buffer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(deviceInterface, buffer, true);
            deviceEventHandle = Win32.RegisterDeviceNotification(this.ServiceHandle, buffer, Win32.DEVICE_NOTIFY_SERVICE_HANDLE | Win32.DEVICE_NOTIFY_ALL_INTERFACE_CLASSES);
            if (deviceEventHandle == IntPtr.Zero)
            {
                // TODO handle error
            }
        }

        #endregion

        #region Private Helper Methods

        public static IntPtr CreateFileHandle(string driveLetter)
        {
            // open the existing file for reading          
            IntPtr handle = Win32.CreateFile(
                  driveLetter,
                  Win32.GENERIC_READ,
                  Win32.FILE_SHARE_READ | Win32.FILE_SHARE_WRITE,
                  0,
                  Win32.OPEN_EXISTING,
                  Win32.FILE_FLAG_BACKUP_SEMANTICS | Win32.FILE_ATTRIBUTE_NORMAL,
                  0);

            if (handle == Win32.INVALID_HANDLE_VALUE)
            {
                return IntPtr.Zero;
            }
            else
            {
                return handle;
            }
        }

        #endregion

        #region Events

        void fileSystemWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            // TODO handle event
        }

        void fileSystemWatcher_Renamed(object sender, System.IO.RenamedEventArgs e)
        {
            // TODO handle event
        }

        void fileSystemWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            // TODO handle event
        }

        void fileSystemWatcher_Deleted(object sender, System.IO.FileSystemEventArgs e)
        {
            // TODO handle event
        }

        #endregion
    }
}
