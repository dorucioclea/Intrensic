using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace IntrensicService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {

                if (args[0].ToUpper() == "-INSTALL")
                {
                    InstallService(args[1], args[2]);
                    return;
                }
                else if (args[0].ToUpper() == "-UNINSTALL")
                {
                    UnInstallService(args[1]);
                    return;
                }

            }
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new IntrensicService() };
            ServiceBase.Run(ServicesToRun);
        }

        private static bool InstallService(string serviceName, string displayName)
        {

            bool success = false;
            try
            {

                string exeFullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string workingPath = System.IO.Path.GetDirectoryName(exeFullPath);
                string logPath = System.IO.Path.Combine(workingPath, "Install.log");
                ServiceStartMode startmode = ServiceStartMode.Automatic;
                ServiceAccount account = ServiceAccount.LocalService;
                string username = "";
                string password = "";

                startmode = ServiceStartMode.Automatic;
                account = ServiceAccount.LocalSystem;

                Hashtable savedState = new Hashtable();
                ProjectInstaller myProjectInstaller = new ProjectInstaller();
                InstallContext myInstallContext = new InstallContext(logPath, new string[] { });
                myProjectInstaller.Context = myInstallContext;
                myProjectInstaller.ServiceName = serviceName;
                myProjectInstaller.DisplayName = displayName;
                //myProjectInstaller.Description = "Self Install Service test";
                myProjectInstaller.StartType = startmode;
                myProjectInstaller.Account = account;
                if (account == ServiceAccount.User)
                {

                    myProjectInstaller.ServiceUsername = username;
                    myProjectInstaller.ServicePassword = password;

                }
                myProjectInstaller.Context.Parameters["AssemblyPath"] = exeFullPath;

                myProjectInstaller.Install(savedState);                
                ServiceUtility.StopStartServices(serviceName, ServiceControllerStatus.StartPending);
                success = true;

            }
            catch (Exception ex)
            {

            }
            return success;
        }

        private static bool UnInstallService(string serviceName)
        {

            bool success = false;
            try
            {
                ServiceUtility.StopStartServices(serviceName, ServiceControllerStatus.StopPending);
                string exeFullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string workingPath = System.IO.Path.GetDirectoryName(exeFullPath);
                string logPath = System.IO.Path.Combine(workingPath, "Install.log");

                ServiceInstaller myServiceInstaller = new ServiceInstaller();
                InstallContext Context = new InstallContext(logPath, null);
                myServiceInstaller.Context = Context;
                myServiceInstaller.ServiceName = serviceName;
                myServiceInstaller.Uninstall(null);                
                success = true;

            }
            catch (Exception ex)
            {

            }
            return success;
        }
    }
}
