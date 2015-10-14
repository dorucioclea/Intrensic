using PodcastUtilities.PortableDevices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intrensic
{

	#region DELEGATES
	public delegate void UserMenuClick(object sender, EventArgs e, string menuItem);
	public delegate void ContextClick(object sender, EventArgs e, ContextAction action, List<string> videos);
	public delegate void ShowUploadButton(object sender, EventArgs e, bool show);
	public delegate void StartUploadProcess(object sender, EventArgs e);

	#endregion DELEGATES

	#region ENUMS
	public enum ContextAction
	{
		Preview,
		Download,
		DownloadAndVerify,
		Note
	}

	public enum Role
	{
		Administrator = 1,
		User = 2,
		Investigator = 3,
		PatrolOfficer = 4,
		Prosecutor = 5,
		Sergeant = 6
	}

	#endregion ENUMS

	#region CONST
	public static class CodeITConstants
	{
		public const string LOGIN_INCORECT_CREDENTIALS_HAS_USERS = "InvalidLoginWithUsers";
		public const string LOGIN_INCORECT_CREDENTIALS_NO_USERS = "InvalidLoginWithoutUsers";
		public const string LOGIN_WHILE_UPLOAD_IN_PROGRESS_BY_DIFFERENT_USER = "InvalidUploadByOtherUserNotCompleted";
		public const string LOGIN_SUCCESSFULL = "LoginSuccessfull";
		public const string LOGOUT_SUCCESSFULL = "LogoutSuccessfull";



		//Settings
		public const string SETTINGS_TEMP_LOCATION = "settingstmplocation";
	}
	#endregion CONST

	#region PUBLIC
	public static class Context
	{
		private static bool _isUploadFromGoProStarted = false;
		private static CodeITDL.User _currentUser = null;
		public static int UserId { get; set; }
		public static Guid CustomerId
		{
			get
			{
				if (CodeITLicence.Licence.ClientId == "testtestovski")
					return new Guid();
				return Guid.Parse(CodeITLicence.Licence.ClientId);
			}
		}

		public static bool IsUploadFromGoProStarted
		{
			get { return _isUploadFromGoProStarted; }
			set { _isUploadFromGoProStarted = value; }
		}

		public static frmUserMainScreen mainForm { get; set; }

		private static frmProgressStatus _progressForm;

		public static frmProgressStatus progressForm
		{
			get
			{
				if (_progressForm == null)
					_progressForm = new frmProgressStatus();

				return _progressForm;
			}
			set { _progressForm = value; }
		}


		public static System.Windows.Forms.ContextMenuStrip contextMenu { get; set; }

		public static CodeITDL.User getCurrentUser
		{
			get
			{
				if (UserId <= 0)
					return new CodeITDL.User();
				if (_currentUser != null)
					if (_currentUser.Id == UserId)
						return _currentUser;

				using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(UserId))
				{
					_currentUser = ctx.Users.Where(x => x.Id == UserId).FirstOrDefault();
				}

				return _currentUser;
			}
		}

		public static bool CheckForGoProDevice()
		{
			bool result = false;
			IDeviceManager manager = new DeviceManager();
			IEnumerable<IDevice> devices = manager.GetAllDevices();

			IDevice gopro = null;
			foreach (var device in devices)
			{
				if (device.Name.ToLower().Contains("hero"))
				{
                    gopro = device;
                    result = true;
                }
			}

			if (gopro != null)
			{
				for (int i = 0; i < System.Windows.Forms.Application.OpenForms.Count; i++)
				{
					System.Windows.Forms.Form frm = System.Windows.Forms.Application.OpenForms[i];
					if (frm is IFormWithGoProDetector)
					{
						((IFormWithGoProDetector)frm).GoProMTPDeviceDetected();
					}
				}
			}
			return result;
		}



		public static void ProcessGoProCameraForUser()
		{

			if (IsUploadFromGoProStarted) return;

			IDeviceManager manager = new DeviceManager();
			IEnumerable<IDevice> devices = manager.GetAllDevices();

			IDevice gopro = null;

			CodeITDL.User uploadUser = null;

			List<IDeviceObject> mp4Files = new List<IDeviceObject>();
			List<CodeITBL.FileFromDB> mp4DbFiles = new List<CodeITBL.FileFromDB>();

			bool shouldProceed = false;

			bool shouldDelete = false;

			#region CHECK FOR DEVICE ID AND USER
			if (devices.Count() > 0)
				using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(UserId))
				{
					foreach (var device in devices)
					{
                        string deviceID = string.Format("{0}||{1}", device.Id, device.Serial);
                        //string deviceID = "\\\\?\\usb#vid_2672&pid_000d#c3131124781379#{6ac27878-a6fa-4155-ba85-f98f491d4f33}||C3131124781379";
                        var item = ctx.Users.Where(x => x.CustomerId == CustomerId && x.DeviceId == device.Id);
                        if (ctx.Users.Where(x => x.CustomerId == CustomerId && x.DeviceId == deviceID) != null)
                        {
                            if (ctx.Users.Where(x => x.CustomerId.ToString().ToUpper() == CustomerId.ToString().ToUpper() && x.DeviceId == deviceID && x.Id == UserId).Count() > 0)
                            {
                                uploadUser = ctx.Users.Where(x => x.CustomerId == CustomerId && x.DeviceId == deviceID && x.Id == UserId).FirstOrDefault();
                                shouldProceed = true;
                                gopro = device;
                                IsUploadFromGoProStarted = true;
                                break;
                            }
                            else
                            {
								System.Windows.MessageBox.Show("This camera is not mapped to your user. Please contact the administrator!", "Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                            }
                        }
					}
				}
			#endregion CHECK FOR DEVICE ID AND USER

			if (!shouldProceed) return;

			#region CHECK FOR ACTIVE UPLOAD AND USER
			if (!progressForm.hasUploadCompleted)
				if (progressForm.getOwnerOfUploadProcess != uploadUser)
				{
					System.Windows.MessageBox.Show("There is active upload in progress, please wait until it is complete", "Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
					shouldProceed = false;
				}
			#endregion CHECK FOR ACTIVE UPLOAD AND USER

			if (!shouldProceed) return;

			#region CHECK FOR VIDEO FILES


			if (gopro != null)
			{
				foreach (var rootFolder in gopro.GetDeviceRootStorageObjects())
					mp4Files.AddRange(rootFolder.GetFiles("*.mp4"));
			}

			if (mp4Files.Count > 0)
				shouldProceed = true;

			#endregion CHECK FOR VIDEO FILES

			if (!shouldProceed) return;

			#region CONFIRMATION BOX
			if (!Context.IsUploadMessageBoxVisible)
			{
				filesForUpload = new List<IDeviceObject>();
				getFilesRecursive(gopro.GetDeviceRootStorageObjects().ToList(), "*.mp4");


				frmUploadMessageBox msgBox = new frmUploadMessageBox("message", "GoPro Upload Confirmation", filesForUpload, uploadUser, gopro);
				msgBox.StartPosition = FormStartPosition.CenterScreen;
				msgBox.TopMost = true;
				msgBox.TopLevel = true;

				msgBox.Show();
				//on form closing, check if some file is selected for upload
				//this is because of posibility to minimize upload form
				msgBox.FormClosing += (sender, args) =>
				{
					mp4Files = msgBox.SelectedFiles;
					mp4DbFiles = msgBox.SelectedDbFiles;
					Context.IsUploadMessageBoxVisible = false;
					shouldDelete = msgBox.Response == CustomMessageBoxResult.UploadAndDeleteSuccesfullyCompleted;
					if (msgBox.Response == CustomMessageBoxResult.Cancel)
						shouldProceed = false;
					else if (mp4Files.Count() == 0)
					{
						shouldProceed = false;
					}
					else
						shouldProceed = true;

					if (!shouldProceed)
					{
						IsUploadFromGoProStarted = false;
						return;
					}

					progressForm.StartUploadProcessFromGoPro(uploadUser, gopro, mp4Files, mp4DbFiles, shouldDelete);
					progressForm.Show();
				};
			}
			#endregion CONFIRMATION BOX
		}


		private static List<IDeviceObject> filesForUpload = new List<IDeviceObject>();
		private static void getFilesRecursive(List<IDeviceObject> root, string pattern)
		{
			foreach (var devObject in root)
			{
				if (devObject.GetFolders("*").Count() > 0)
					getFilesRecursive(devObject.GetFolders("*").ToList(), pattern);
				else
					filesForUpload.AddRange(devObject.GetFiles(pattern));
			}
		}

		public static bool IsUploadMessageBoxVisible { get; set; }

		public static System.Drawing.Image GetImageForCustomer()
		{
			if (System.IO.File.Exists(Application.StartupPath.TrimEnd('\\') + "\\" + CodeITLicence.Licence.ClientId + ".jpg"))
				return System.Drawing.Image.FromFile(Application.StartupPath.TrimEnd('\\') + "\\" + CodeITLicence.Licence.ClientId + ".jpg");
			else
				return Properties.Resources.default_inapp;
		}

		static string _tmpLocation = System.IO.Path.GetTempPath();
		public static string getTempFolderLocation()
		{
			if (_tmpLocation != System.IO.Path.GetTempPath())
				return _tmpLocation;

			using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(Context.UserId))
			{
				if (ctx.Settings.Where(x => x.CustomerId == Context.CustomerId && x.Name == CodeITConstants.SETTINGS_TEMP_LOCATION).Count() > 0)
					_tmpLocation = ctx.Settings.Where(x => x.CustomerId == Context.CustomerId && x.Name == CodeITConstants.SETTINGS_TEMP_LOCATION).Select(x => x.Value).FirstOrDefault();
			}

			return _tmpLocation;
		}

		public static Process checkForRunningInstance()
		{
			try
			{
				string thisModuleName = Process.GetCurrentProcess().MainModule.ModuleName;
				thisModuleName = Path.GetFileNameWithoutExtension(thisModuleName);

				int thisProcessId = Process.GetCurrentProcess().Id;

				Process[] similarProcesses = Process.GetProcessesByName(thisModuleName);
				if (similarProcesses.Length > 1)
				{
					foreach (Process proc in similarProcesses)
					{
						// Return an instance which is NOT the current instance.
						if (proc.Id != thisProcessId)
						{
							return proc;
						}
					}
				}
			}
			catch (Exception) { }
			return null;
		}
	}

	public static class LoginAudit
	{
		public static void WriteLoginAudit(string status, int userId)
		{

			string pcUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
			string pcName = Environment.MachineName;
			string pcIpAddress = LocalIPAddress();


			using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(Context.UserId))
			{
				CodeITDL.UserLoginAudit ula = new CodeITDL.UserLoginAudit();
				ula.Action = status;
				ula.PcIpAddress = pcIpAddress;
				ula.PcName = pcName;
				ula.PcUserName = pcUserName;
				ula.UserId = userId;
				ula.Id = Guid.NewGuid();
				ula.CreatedOn = DateTime.Now;

				ctx.UserLoginAudits.Add(ula);
				ctx.SaveChangesWithoutAudit();
			}

		}
		public static void WriteLoginAudit(string status)
		{
			WriteLoginAudit(status, Context.UserId);
			if (status == CodeITConstants.LOGOUT_SUCCESSFULL)
				Context.UserId = 0;
		}

		private static string LocalIPAddress()
		{
			System.Net.IPHostEntry host;
			string localIP = "";
			host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (IPAddress ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					localIP = ip.ToString();
					break;
				}
			}
			return localIP;
		}
	}
	#endregion PUBLIC
}
