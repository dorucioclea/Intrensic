using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeITLicence;
using Intrensic.Util;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using System.IO;
using System.Globalization;

namespace Intrensic
{
    static class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Program));

        static UsbDetector usbDetector = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			if (Context.checkForRunningInstance() != null)
			{
				return;
			}

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            CultureInfo TempCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            TempCulture.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy hh:mm:ss a";
            Thread.CurrentThread.CurrentCulture = TempCulture;


            ConfigureLog4Net();
            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            // Set the unhandled exception mode to force all Windows Forms errors to go through our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

			usbDetector = new UsbDetector();

			Application.Run(new frmLogIn());
			


        }

        

       

        private static void ConfigureLog4Net()
        {
            log4net.Config.XmlConfigurator.Configure();

            var hierarchy = LogManager.GetRepository() as Hierarchy;
            if (hierarchy != null && hierarchy.Configured)
            {
                foreach (IAppender appender in hierarchy.GetAppenders())
                {
                    if (appender is AdoNetAppender)
                    {
                        var adoNetAppender = (AdoNetAppender)appender;
                        adoNetAppender.ConnectionString = Licence.GetConnectionString();
                        adoNetAppender.ActivateOptions(); //Refresh AdoNetAppenders Settings
                    }
                }
            }
        }
        
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            _logger.Error(e.Exception.Message, e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                    "with the following information:\n\n";

                // Since we can't prevent the app from terminating, log this to the event log. 
                if (!EventLog.SourceExists("ThreadException"))
                {
                    EventLog.CreateEventSource("ThreadException", "Intrensic");
                }

                // Create an EventLog instance and assign its source.
                var myLog = new EventLog();
                myLog.Source = "ThreadException";
                myLog.WriteEntry(errorMsg + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
                _logger.Error(errorMsg + ex.Message, ex);
            }
            catch (Exception exc)
            {
                try
                {
                    MessageBox.Show("Fatal Non-UI Error",
                        "Fatal Non-UI Error. Could not write the error to the event log. Reason: "
                        + exc.Message, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }


        // Called by DriveDetector when removable device in inserted
        private static void OnDriveArrived(object sender, DriveDetectorEventArgs e)
        {
           
            String dcimFolder = string.Format("{0}DCIM", e.Drive);
            bool isGoPro = false;
            bool hasVideos = false;

           
            if (Directory.Exists(dcimFolder) && System.IO.Directory.GetFiles(dcimFolder, "*.mp4", SearchOption.AllDirectories).Length > 0)
            {
                isGoPro = true;
                hasVideos = true;
            }

            if (hasVideos && isGoPro)
            {
                for (int i = 0; i < System.Windows.Forms.Application.OpenForms.Count; i++)
                {
                    System.Windows.Forms.Form frm = System.Windows.Forms.Application.OpenForms[i];
                    if (frm is IFormWithGoProDetector)
                    {
                        ((IFormWithGoProDetector)frm).GoProDeviceDetected(e.Drive);
                    }
                }
            }           
            else
                e.HookQueryRemove = true;
        }

        // Called by DriveDetector after removable device has been unplugged
        private static void OnDriveRemoved(object sender, DriveDetectorEventArgs e)
        {

            for (int i = 0; i < System.Windows.Forms.Application.OpenForms.Count; i++)
            {
                System.Windows.Forms.Form frm = System.Windows.Forms.Application.OpenForms[i];
                if (frm is IFormWithGoProDetector)
                {
                    ((IFormWithGoProDetector)frm).DeviceDisconnected();
                }
            }           
        }

        // Called by DriveDetector when removable drive is about to be removed
        private static void OnQueryRemove(object sender, DriveDetectorEventArgs e)
        {
            return;
        }
    }
}
