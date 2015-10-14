using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntrensicService
{
    public class ServiceUtility
    {
        public static bool StopStartServices(string serviceName, ServiceControllerStatus status)
        {
            bool result = false;
            try
            {
                ServiceController sc = new ServiceController(serviceName);
                if (status == ServiceControllerStatus.StopPending)
                {
                    if (sc.Status != ServiceControllerStatus.Stopped)
                        sc.Stop();
                }
                else if (status == ServiceControllerStatus.StartPending)
                {
                    if (sc.Status != ServiceControllerStatus.Running)
                        sc.Start();
                }
                result = true;
            }
            catch (Exception)
            {
              
            }
            return result;
        }

        public static int RunProcess(string workingDirectory, string fileName, string arguments, int sleepInterval, out string m_OutputErrMsg, out string m_OutputMsg, int waitForExit = -1, bool errorHandler = false)
        {
            Process proc = null;
            string stderr = string.Empty;
            int exitCode = 0;
            try
            {
                proc = new Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                if (errorHandler)
                {
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.ErrorDataReceived += new DataReceivedEventHandler(ErrorDataHandler);
                    proc.OutputDataReceived += new DataReceivedEventHandler(OutputDataHandler);
                }
                if (workingDirectory != string.Empty)
                {
                    proc.StartInfo.WorkingDirectory = workingDirectory;
                }
                proc.StartInfo.FileName = fileName;
                if (arguments != string.Empty)
                {
                    proc.StartInfo.Arguments = arguments;
                }
                proc.StartInfo.Verb = "runas";
                proc.Start();
                if (errorHandler)
                {
                    proc.BeginErrorReadLine();
                    proc.BeginOutputReadLine();
                }
                if (waitForExit > 0)
                {
                    proc.WaitForExit();
                    exitCode = proc.ExitCode;
                }
                if (sleepInterval > 0)
                {
                    Thread.Sleep(sleepInterval);
                }
            }
            catch (Exception ex)
            {
                
                exitCode = -1;
                if (errorHandler)
                {
                    m_errorMessages.Append(exitCode.ToString());
                }
            }
            finally
            {
                if (proc != null)
                {
                    proc.Dispose();
                }
            }
            m_OutputErrMsg = m_errorMessages.ToString();
            m_OutputMsg = m_regularMessages.ToString();

            return exitCode;
        }

        private static StringBuilder m_errorMessages = new StringBuilder();
        private static StringBuilder m_regularMessages = new StringBuilder();
        private static void ErrorDataHandler(object sender, DataReceivedEventArgs args)
        {
            string message = args.Data;

            if (message != null && message.StartsWith("Error"))
            {
                // The vsinstr.exe process reported an error
                m_errorMessages.Append(message);                
            }
        }

        private static void OutputDataHandler(object sender, DataReceivedEventArgs args)
        {
            string message = args.Data;
            if (!string.IsNullOrEmpty(message))
            {
                m_regularMessages.Append(message);
            }
        }		
    }

    public class Win32
    {
        public const int DEVICE_NOTIFY_SERVICE_HANDLE = 1;
        public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;

        public const int SERVICE_CONTROL_STOP = 1;
        public const int SERVICE_CONTROL_DEVICEEVENT = 11;
        public const int SERVICE_CONTROL_SHUTDOWN = 5;

        public const uint GENERIC_READ = 0x80000000;
        public const uint OPEN_EXISTING = 3;
        public const uint FILE_SHARE_READ = 1;
        public const uint FILE_SHARE_WRITE = 2;
        public const uint FILE_SHARE_DELETE = 4;
        public const uint FILE_ATTRIBUTE_NORMAL = 128;
        public const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        public const int DBT_DEVTYP_DEVICEINTERFACE = 5;
        public const int DBT_DEVTYP_HANDLE = 6;

        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

        public const int WM_DEVICECHANGE = 0x219;

        public delegate int ServiceControlHandlerEx(int control, int eventType, IntPtr eventData, IntPtr context);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern IntPtr RegisterServiceCtrlHandlerEx(string lpServiceName, ServiceControlHandlerEx cbex, IntPtr context);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetVolumePathNamesForVolumeNameW(
                [MarshalAs(UnmanagedType.LPWStr)]
					string lpszVolumeName,
                [MarshalAs(UnmanagedType.LPWStr)]
					string lpszVolumePathNames,
                uint cchBuferLength,
                ref UInt32 lpcchReturnLength);

        [DllImport("kernel32.dll")]
        public static extern bool GetVolumeNameForVolumeMountPoint(string
           lpszVolumeMountPoint, [Out] StringBuilder lpszVolumeName,
           uint cchBufferLength);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr IntPtr, IntPtr NotificationFilter, Int32 Flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint UnregisterDeviceNotification(IntPtr hHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(
              string FileName,                    // file name
              uint DesiredAccess,                 // access mode
              uint ShareMode,                     // share mode
              uint SecurityAttributes,            // Security Attributes
              uint CreationDisposition,           // how to create
              uint FlagsAndAttributes,            // file attributes
              int hTemplateFile                   // handle to template file
              );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DEV_BROADCAST_DEVICEINTERFACE
        {
            public int dbcc_size;
            public int dbcc_devicetype;
            public int dbcc_reserved;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
            public byte[] dbcc_classguid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] dbcc_name;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_HDR
        {
            public int dbcc_size;
            public int dbcc_devicetype;
            public int dbcc_reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_HANDLE
        {
            public int dbch_size;
            public int dbch_devicetype;
            public int dbch_reserved;
            public IntPtr dbch_handle;
            public IntPtr dbch_hdevnotify;
            public Guid dbch_eventguid;
            public long dbch_nameoffset;
            public byte dbch_data;
            public byte dbch_data1;
        }
    }
}
