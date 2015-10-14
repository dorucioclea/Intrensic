using System.Threading;
using System.Windows;
using CodeITBL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using PodcastUtilities.PortableDevices;
using System.IO;
using System.Runtime.InteropServices;

namespace Intrensic
{
	public partial class frmProgressStatus : Form
	{

		private readonly ILog _logger = LogManager.GetLogger(typeof(frmProgressStatus));

		private string downloadLocation = string.Empty;

		System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog() { ShowNewFolderButton = true, Description = "Please select download location" };

		Dictionary<int, IFileTransfer> taskList = new Dictionary<int, IFileTransfer>();

		Dictionary<int, TaskStatus> taskStatusList = new Dictionary<int, TaskStatus>();

		List<Task> allUploadTasks = new List<Task>();
		int totalItemRows = 0;

		ProgressBarWithPercentage pbFiles = new ProgressBarWithPercentage();
		bool cancelCopyFromGoPro = false;

		bool shouldDeleteGoProFiles = false;
		bool isFromGoPro = false;
		IDevice goPro = null;
		CodeITDL.User goProUser = null;
		bool hasFinishedGoProOperation = false;


		List<IDeviceObject> goProFiles = null;

		private CodeITDL.User currentUser;


		private bool shouldDeleteCompletedFiles;

		List<Task> completeAll = new List<Task>();

		Button btnRetry = new Button();
		Button btnCancel = new Button();

		public CodeITDL.User getOwnerOfUploadProcess
		{
			get { return currentUser; }
		}

		public bool hasUploadCompleted
		{
			get
			{			
				if (allUploadTasks.Count == 0)
					return true;
				if (allUploadTasks.Where(task => task.Status != TaskStatus.RanToCompletion).Count() > 0 || pbFiles.Value < pbFiles.Maximum)
					return false;
				return true;
			}
		}


		public void DisableCancelButtonsOnLogout(bool shouldDisable)
		{
			foreach (Control ctrl in tlpItems.Controls)
			{
				if (ctrl is Button)
					ctrl.Enabled = !shouldDisable;
			}
		}

		public frmProgressStatus()
		{
			InitializeComponent();

			counter = 0;
			DrawHeader();
		}

		protected override void OnLoad(EventArgs e)
		{
			PlaceLowerRight();
			base.OnLoad(e);
		}

		public void BeginDownloadProcess(List<CodeITBL.FileFromDB> filePaths, bool shouldValidate)
		{
			this.shouldDeleteCompletedFiles = false;

			if (filePaths == null)
			{
				this.Hide();
				return;
			}
			if (filePaths.Count == 0)
			{
				this.Hide();
				return;
			}


			if (string.IsNullOrEmpty(downloadLocation))
			{
				folderDialog.ShowDialog();
				downloadLocation = folderDialog.SelectedPath;
			}

			if (string.IsNullOrEmpty(downloadLocation))
			{
				System.Windows.MessageBox.Show("Please select download directory", "Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
				this.Hide();
				return;
			}

			if (!System.IO.Directory.Exists(downloadLocation))
			{
				System.Windows.MessageBox.Show("Please select existing download directory", "Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
				this.Hide();
				return;
			}

			try
			{
				currentUser = Context.getCurrentUser;

				var tasks = new List<Task>();

                foreach (CodeITBL.FileFromDB file in filePaths)
                {
                    string name = file.NewFileName;

                    var items = tlpItems.Controls;
                    var control = items.Cast<Control>().FirstOrDefault(x => x.Tag == name && x.GetType() == typeof(Label));
                    if (control != null)
                    {
						System.Windows.MessageBox.Show(string.Format("File \"{0}\" is already added to the queue", name), "Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                        continue;

                    }

                    var fileTransfer = FileTransferFactory.GetFileTransfer(file.NewFileName, string.Format(@"{0}\{1}.mp4", downloadLocation.TrimEnd('\\'), file.NewFileName), null, string.Empty, string.Format("Recorded Date: {0}{1}{2}", file.OriginalFileDate.ToString(), Environment.NewLine, file.Note), new DateTime(), true, string.Empty);
                    fileTransfer.StateChanged += fileTransfer_StateChanged;

                    var fileProgress = new Progress<TaskProgressReport>();
                    fileProgress.ProgressChanged += ProgressChanged;

                    Task task = fileTransfer.DownloadFile(fileProgress, shouldValidate ? file.MD5Checksum : string.Empty);

                    taskList.Add(task.Id, fileTransfer);
                    tasks.Add(task);

                    AddItem(fileTransfer.Name(), fileTransfer.SourceFilePath(), task, totalItemRows);
                    totalItemRows += 1;

                    task.ContinueWith(UploadDone);

                }

				foreach (var task in tasks)
				{
					task.Start();
				}

				if (tasks.Count > 0)
					allUploadTasks.AddRange(tasks);
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
			}
		}

		private int counter = 0;

        private Int32 GetUploadId(string newFilename)
        {
            if (!String.IsNullOrEmpty(newFilename))
            {
                if (newFilename.Split('.').Length == 2)
                {
                    return Convert.ToInt32(newFilename.Split('.')[1]);
                }
            }
            return 0;
        }

        private void beginUploadProcess(List<FileFromDB> filePaths, bool shouldDeleteCompletedFiles, CodeITDL.User usr, string md5)
		{
			this.shouldDeleteCompletedFiles = shouldDeleteCompletedFiles;

			currentUser = usr;
			if (filePaths == null)
				return;
			if (filePaths.Count == 0)
				return;

			var tasks = new List<Task>();

			
			foreach (FileFromDB file in filePaths)
			{
				counter++;

				try
				{
					VideoInfo info = VideoInfo.GetVideoInfo(file.OriginalFileLocation);

					if (info != null)
					{
						file.Resolution = info.Resolution;
						file.Duration = (int)info.Duration;
						file.CreatedOn = File.GetCreationTime(file.OriginalFileLocation);
					}

                    //string name = Guid.NewGuid().ToString().Replace("-", string.Empty);
                    file.NewFileName = file.NewFileName;

                    var fileTransfer = FileTransferFactory.GetFileTransfer(file.NewFileName, file.OriginalFileLocation, null, file.OriginalFileName, file.Note, file.OriginalFileDate, false, md5, (int)file.Duration, file.Resolution);
					fileTransfer.StateChanged += fileTransfer_StateChanged;

					var fileProgress = new Progress<TaskProgressReport>();
					fileProgress.ProgressChanged += ProgressChanged;

					Task task = fileTransfer.CopyFile(fileProgress);

					taskList.Add(task.Id, fileTransfer);
					tasks.Add(task);

					AddItem(fileTransfer.Name(), fileTransfer.SourceFilePath(), task, totalItemRows);
					totalItemRows += 1;

					task.ContinueWith(UploadDone);
				}
				catch (Exception ex)
				{
					_logger.Error(ex.Message);
				}
			}

				

				

			foreach (var task in tasks)
			{
				task.Start();
			}

			if (tasks.Count > 0)
				allUploadTasks.AddRange(tasks);
		}



        public void BeginUploadProcess(List<FileFromDB> filePaths, bool shouldDeleteCompletedFiles)
		{
			beginUploadProcess(filePaths, shouldDeleteCompletedFiles, Context.getCurrentUser, string.Empty);
		}



		void fileTransfer_StateChanged(object sender, StateChangeEventArg arg)
		{
			String status = "";

			String name = ((IFileTransfer)sender).Name();
			switch (arg.FileTransferState)
			{
				case FileTransferState.Inital:
					status = "Starting";
					if (!((IFileTransfer)sender).IsDownload())
						logStatusInDB(status, name);
					break;
				case FileTransferState.CalculatingChecksum:
					status = "Calculating Checksum";
					if (!((IFileTransfer)sender).IsDownload())
						logStatusInDB(status, name);
					break;
				case FileTransferState.InProgress:
					status = "In Progress";
					if (!((IFileTransfer)sender).IsDownload())
						logStatusInDB(status, name);
					break;
				case FileTransferState.Verifying:
					status = "Verifying";
					if (!((IFileTransfer)sender).IsDownload())
						logStatusInDB(status, name);
					break;
				case FileTransferState.ErrorVerification:
					status = "Error";
					if (!((IFileTransfer)sender).IsDownload())
						logStatusInDB(status, name);
					changeProgress(name, 100);
					break;
				case FileTransferState.Done:
					changeProgress(name, 100);
					status = "Completed";
					if (!((IFileTransfer)sender).IsDownload())
					{
						logStatusInDB(status, name);
						IFileTransfer ift = ((IFileTransfer)sender);
						writeFileInfoInDB(ift.Md5(), ift.Url(), ift.Name(), ift.Note(), ift.OriginalFileDate(), ift.SourceFilePath(), ift.OriginalFileName(), ift.Duration(), ift.Resolution(), GetUploadId(ift.Name()));

						if (shouldDeleteCompletedFiles)
							if (System.IO.File.Exists(ift.SourceFilePath()))
								System.IO.File.Delete(ift.SourceFilePath());

						if (shouldDeleteGoProFiles && hasFinishedGoProOperation)
							foreach (var file in goProFiles)
							{
								((Device)goPro).DeleteByObjectId(file.Id);
							}

					}
					else
					{

						if (!string.IsNullOrEmpty(((IFileTransfer)sender).Note()))
						{
							using (System.IO.StreamWriter writer = System.IO.File.CreateText(((IFileTransfer)sender).SourceFilePath().Replace(".mp4", ".txt")))
							{
								writer.Write(((IFileTransfer)sender).Note());
								writer.Flush();
							}
						}


					}
					break;
				default:
					status = "";
					break;
			}

			changeLabelStatus(name, status);
		}

		void logStatusInDB(string status, string name)
		{
			if (string.IsNullOrEmpty(status)) return;

			using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(Context.UserId))
			{
				ctx.FileUploadStatusLogs.Add(new CodeITDL.FileUploadStatusLog() { Status = status, FileName = name, CreatedOn = DateTime.Now });
				ctx.SaveChangesWithoutAudit();
			}
		}

		void writeFileInfoInDB(string checkSum, string newFileLocation, string name, string note, DateTime originalFileDate, string originalFileLocation, string originalFileName, Int32 duration, string resolution, Int32 uploadId = 0)
		{
			try
			{
				using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(currentUser.Id))
				{

					CodeITDL.File f =
						new CodeITDL.File()
						{
							CreatedBy = goProUser == null ? Context.UserId : goProUser.Id,
							IsArchived = false,
							isFromCard = false,
							MD5Checksum = checkSum,
							NewFileLocation = newFileLocation,
							NewFileName = name,
							OriginalFileDate = originalFileDate,
							OriginalFileLocation = originalFileLocation,
							OriginalFileName = originalFileName,
							UserId = goProUser == null ? Context.UserId : goProUser.Id,
							Note = note,
							Duration = duration,
							Resolution = resolution,
							CreatedOn = DateTime.Now,
							IsCloudFileSystem = CodeITLicence.Licence.StorageType == CodeITLicence.StorageType.Cloud ? true : false,
                            UploadId = uploadId
						};
					ctx.Files.Add(f);

					ctx.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
			}
		}

		void UploadDone(Task task)
		{

			try
			{
				updateButtonText(task);

				IFileTransfer fileTransfer = null;
				taskList.TryGetValue(task.Id, out fileTransfer);

				if (fileTransfer != null)
				{
					String name = fileTransfer.Name();

					if (task.AsyncState is CancellationToken &&
							((CancellationToken)task.AsyncState).IsCancellationRequested)
					{

						hideButtonOnCancel(task);
						changeProgress(name, 0);
						taskStatusList.Add(task.Id, TaskStatus.Canceled);
						changeLabelStatus(name, "Canceled");
					}
					else
					{
						if (task.IsFaulted)
						{
							hideButtonOnCancel(task);
							changeProgress(name, 0);
							taskStatusList.Add(task.Id, TaskStatus.Faulted);
							changeLabelStatus(name, "");
							hideButtonOnCancel(task, "Retry", true);
						}
						else if (task.IsCanceled)
						{
							hideButtonOnCancel(task);
							changeProgress(name, 0);
							taskStatusList.Add(task.Id, TaskStatus.Canceled);
							changeLabelStatus(name, "Canceled");

						}
						else if (task.IsCompleted)
						{
							changeProgress(name, 100);
							taskStatusList.Add(task.Id, TaskStatus.RanToCompletion);
							changeLabelStatus(name, "Completed");
						}
					}
				}

				if (hasUploadCompleted)
				{
					Thread.Sleep(3000);
					Invoke(new Action(() => Context.progressForm.Close()));
					Context.progressForm = null;
					Context.contextMenu.Items["tsmiShowProgress"].Visible = false;
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
			}
		}

		void ProgressChanged(object sender, TaskProgressReport report)
		{
			changeProgress(report.Name, report.PercentComplete);
		}

		private void changeProgress(string name, int value)
		{
			try
			{
				if (value < 0)
					value = 0;

				var items = tlpItems.Controls;
				var control = items.Cast<Control>().FirstOrDefault(x => x.Tag == name && x.GetType() == typeof(ProgressBarWithPercentage));
				if (control != null)
				{
					if (((ProgressBarWithPercentage)control).InvokeRequired)
					{

						((ProgressBarWithPercentage)control).Invoke(new MethodInvoker(delegate
						{
							if (((ProgressBarWithPercentage)control).Value != value)
							{
								((ProgressBarWithPercentage)control).Value = value;
							}
						}));

					}
					else
					{
						if (((ProgressBarWithPercentage)control).Value != value)
						{
							((ProgressBarWithPercentage)control).Value = value;
						}
					}
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
			}
		}

		private void changeLabelStatus(string name, string status)
		{
			var items = tlpItems.Controls;
			var control = items.Cast<Control>().FirstOrDefault(x => x.Tag == name && x.GetType() == typeof(Label));
			if (control != null)
			{
				if (((Label)control).InvokeRequired)
					((Label)control).Invoke(new Action(() => { ((Label)control).Text = status; }));
				else
					((Label)control).Text = status;

			}
		}

		private void updateButtonText(Task task)
		{
			var items = tlpItems.Controls;
			var control = items.Cast<Control>().FirstOrDefault(x => x.Tag == task && x.GetType() == typeof(Button));
			if (control != null)
			{
				if (task.Status == TaskStatus.RanToCompletion)
					if (control.InvokeRequired)
						control.Invoke(new Action(() => { control.Parent.Controls.Remove(control); }));
					else
						control.Parent.Controls.Remove(control);
			}
		}

		private void togleDeleteAllButton(bool show)
		{
		}

		private void hideButtonOnCancel(Task task, string buttonText = "Cancel", bool showButton = false)
		{
			var items = tlpItems.Controls;
			var control = items.Cast<Control>().FirstOrDefault(x => x.Tag == task && x.GetType() == typeof(Button) && x.Text == buttonText);
			if (control != null)
			{
				if (((Button)control).InvokeRequired)
					((Button)control).Invoke(new Action(() => { ((Button)control).Visible = showButton; }));
				else
					((Button)control).Visible = showButton;
			}
		}

		private void PlaceLowerRight()
		{
			Screen rightmost = Screen.AllScreens[0];
			foreach (Screen screen in Screen.AllScreens)
			{
				if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
					rightmost = screen;
			}

			this.Left = rightmost.WorkingArea.Right - this.Width;
			this.Top = rightmost.WorkingArea.Bottom - this.Height;
		}

		private void DrawHeader()
		{
			tlpHeader.Controls.Add(new Label() { Text = "File Name", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 0, 0);
			tlpHeader.Controls.Add(new Label() { Text = "Progress", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 1, 0);
			tlpHeader.Controls.Add(new Label() { Text = "Status", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 2, 0);
			tlpHeader.Controls.Add(new Label() { Text = "Action", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 3, 0);
			tlpHeader.RowCount = 1;
		}

		private void DrawFooter()
		{
			Button btnDeleteAll = new Button();
			btnDeleteAll.Text = "Delete All";
			btnDeleteAll.Click -= btnDeleteAll_Click;
			btnDeleteAll.Click += btnDeleteAll_Click;
			btnDeleteAll.Tag = "DeleteAll";
			btnDeleteAll.Visible = false;


		}

		void btnDeleteAll_Click(object sender, EventArgs e)
		{
			foreach (KeyValuePair<int, IFileTransfer> item in taskList)
			{
				if (System.IO.File.Exists(item.Value.SourceFilePath()))
					System.IO.File.Delete(item.Value.SourceFilePath());
			}

			taskList = new Dictionary<int, IFileTransfer>();
			tlpItems.Controls.Clear();
			this.Close();
		}



		private void AddItem(string name, string fileName, Task task, int rowNumber)
		{
			try
			{
				if (rowNumber == 0)
					if (!tlpItems.InvokeRequired)
						tlpItems.Controls.Clear();
					else
						tlpItems.Invoke(new Action(() => { tlpItems.Controls.Clear(); }));

				if (!tlpItems.InvokeRequired)
					tlpItems.Controls.Add(new Label() { Text = name, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 0, rowNumber);
				else
					tlpItems.Invoke(new Action(() => { tlpItems.Controls.Add(new Label() { Text = name, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 0, rowNumber); }));


				var pbar = new ProgressBarWithPercentage
				{
					Maximum = 100,
					Width = 200,
					Height = 20,
					MaximumSize = new System.Drawing.Size(200, 20),
					MinimumSize = new System.Drawing.Size(200, 20),
					Tag = name,
					Value = 0,
					BackColor = Color.ForestGreen,
					Dock = DockStyle.Fill,
				};

				if (!tlpItems.InvokeRequired)
					tlpItems.Controls.Add(pbar, 1, rowNumber);
				else
					tlpItems.Invoke(new Action(() => { tlpItems.Controls.Add(pbar, 1, rowNumber); }));

				if (!tlpItems.InvokeRequired)
					tlpItems.Controls.Add(new Label() { Text = task.Status.ToString(), Tag = name, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 2, rowNumber);
				else
					tlpItems.Invoke(new Action(() => { tlpItems.Controls.Add(new Label() { Text = task.Status.ToString(), Tag = name, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 2, rowNumber); }));

				btnCancel = new Button
				{
					Height = 20,
					MaximumSize = new System.Drawing.Size(60, 20),
					MinimumSize = new System.Drawing.Size(60, 20),
					Text = task.Status == TaskStatus.RanToCompletion ? "Delete" : "Cancel",
					Tag = task
				};

				btnCancel.Click += btnCancel_Click;
				btnCancel.BackColor = Color.FromArgb(0, 35, 58);
				btnCancel.ForeColor = Color.White;
				btnCancel.FlatStyle = FlatStyle.Popup;

				if (!tlpItems.InvokeRequired)
					tlpItems.Controls.Add(btnCancel, 3, rowNumber);
				else
					tlpItems.Invoke(new Action(() => { tlpItems.Controls.Add(btnCancel, 3, rowNumber); }));

				btnRetry = new Button
				{
					Height = 20,
					MaximumSize = new System.Drawing.Size(60, 20),
					MinimumSize = new System.Drawing.Size(60, 20),
					Text = "Retry",
					Tag = task
				};

				btnRetry.Click += btnRetry_Click;
				btnRetry.BackColor = Color.FromArgb(0, 35, 58);
				btnRetry.ForeColor = Color.White;
				btnRetry.FlatStyle = FlatStyle.Popup;
				btnRetry.Visible = false;

				if (!tlpItems.InvokeRequired)
					tlpItems.Controls.Add(btnRetry, 4, rowNumber);
				else
					tlpItems.Invoke(new Action(() => { tlpItems.Controls.Add(btnRetry, 4, rowNumber); }));
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
			}
		}

		void btnCancel_Click(object sender, EventArgs e)
		{
			int taskid = ((Task)((Button)sender).Tag).Id;
			if (((Button)sender).Text == "Cancel")
			{

				IFileTransfer fileTransfer = null;
				taskList.TryGetValue(taskid, out fileTransfer);

				if (fileTransfer != null)
					fileTransfer.Cancel();
			}
			else if (((Button)sender).Text == "Delete")
			{
				IFileTransfer fileTransfer = null;
				taskList.TryGetValue(taskid, out fileTransfer);
				if (fileTransfer != null)
				{
					if (System.IO.File.Exists(fileTransfer.SourceFilePath()))
						System.IO.File.Delete(fileTransfer.SourceFilePath());
					int rowId = tlpItems.GetRow((Button)sender);
					changeLabelStatus(fileTransfer.Name(), "Deleted");
					((Button)sender).Visible = false;
				}
			}
		}

		/// <summary>
		/// Retry button event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btnRetry_Click(object sender, EventArgs e)
		{
			try
			{
				if (((Button)sender).Text == "Retry")
				{
					//get taskid from failed task
					int taskid = ((Task)((Button)sender).Tag).Id;

					IFileTransfer fileTransfer = null;
					taskList.TryGetValue(taskid, out fileTransfer);
					var fileProgress = new Progress<TaskProgressReport>();
					fileProgress.ProgressChanged += ProgressChanged;

					if (fileTransfer != null)
					{
						Task task = null;
						if (!fileTransfer.IsDownload())
						{
							task = fileTransfer.CopyFile(fileProgress);
						}
						else
						{
							task = fileTransfer.DownloadFile(fileProgress, string.Empty);
						}
						task.ContinueWith(UploadDone);
						//start again failed task
						task.Start();
						//hide retry button
						hideButtonOnCancel(task, "Retry", false);
						//show cancel button
						hideButtonOnCancel(task, "Cancel", true);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
			}
		}

		private void pbClose_Click(object sender, EventArgs e)
		{
			try
			{
				Context.progressForm.Close();
				Context.progressForm = null;
				Context.contextMenu.Items["tsmiShowProgress"].Visible = false;
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
			}
		}

		List<IDeviceObject> filesRec = new List<IDeviceObject>();

		public void getFilesRecursive(List<IDeviceObject> root, string pattern)
		{
			foreach (var devObject in root)
			{
				if (devObject.GetFolders("*").Count() > 0)
					getFilesRecursive(devObject.GetFolders("*").ToList(), pattern);
				else
					filesRec.AddRange(devObject.GetFiles(pattern));
			}
		}

		public void StartUploadProcessFromGoPro(CodeITDL.User uploadUser, IDevice goProDevice, List<IDeviceObject> files, List<CodeITBL.FileFromDB> dbFiles, bool shouldDeleteFiles)
		{
			try
			{
				IDeviceManager manager = new DeviceManager();
				IEnumerable<IDevice> devices = manager.GetAllDevices();

				//getFilesRecursive(goProDevice.GetDeviceRootStorageObjects().ToList(), "*.mp4");

				//files = filesRec;           

				if (files.Count == 0)
				{
					this.Close();
				}

				goProUser = uploadUser;
				Label lblFTransfer = new Label();
				lblFTransfer.ForeColor = Color.White;
				lblFTransfer.Text = "File Copy";
				shouldDeleteGoProFiles = shouldDeleteFiles;

				tlpFileCopyProgress.Controls.Add(lblFTransfer);



				Int32 totalMBytes = 0;

				tlpFileCopyProgress.Visible = true;
				tlpFileCopyProgress.Controls.Add(pbFiles);
				pbFiles.Dock = DockStyle.Fill;

				var btnCancelCopy = new Button
				{
					Height = 20,
					Text = "Cancel Copy"
				};
				btnCancelCopy.Click += btnCancelCopy_Click;
				btnCancelCopy.BackColor = Color.FromArgb(0, 35, 58);
				btnCancelCopy.ForeColor = Color.White;
				btnCancelCopy.FlatStyle = FlatStyle.Popup;

				tlpFileCopyProgress.Controls.Add(btnCancelCopy);


				long mbsize = 1024 * 1024;


				foreach (IDeviceObject file in files)
				{
					totalMBytes += (int)(((Device)goProDevice).getFileSize(file.Id) / mbsize);
				}
				pbFiles.Maximum = totalMBytes;

				//string tempLocation = Context.getTempFolderLocation().TrimEnd('\\');
				string tempLocation = Properties.Settings.Default.TempVideoLocation.TrimEnd('\\');
				using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(uploadUser.Id))
				{
					Guid clientId = new Guid();
					Guid.TryParse(CodeITLicence.Licence.ClientId, out clientId);
					//CodeITDL.Setting tmpSetting = ctx.Settings.Where(x => x.CustomerId == clientId && x.Name == CodeITConstants.SETTINGS_TEMP_LOCATION).FirstOrDefault();

					//if (tmpSetting != null)
					//    if (tmpSetting.Id > 0)
					//        tempLocation = tmpSetting.Value;

					long availableMB = getAvailableSpaceInMB(tempLocation);

					if (availableMB > 0 && availableMB < totalMBytes)
					{
						System.Windows.MessageBox.Show("There is not enough space left on the disk to perform the file transfer. Please free some space.", "Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
						return;
					}
				}

				isFromGoPro = true;
				goPro = goProDevice;
				goProFiles = new List<IDeviceObject>();

				var task = new Task(new Action(() =>
				{

					List<tmpFile> tmpFiles = new List<tmpFile>();
					string folderName = Guid.NewGuid().ToString().Replace("-", string.Empty);

					string folderPath = Properties.Settings.Default.TempVideoLocation.TrimEnd('\\') + "\\" + folderName;
					if (!System.IO.Directory.Exists(folderPath))
						System.IO.Directory.CreateDirectory(folderPath);

					decimal totalMBWritten = 0;
					int retryCount = 0;
					Stream w = null;
					Stream r = null;
					int bufferSize = 524288; //512K
					int i = 0;

					while (i < files.Count)
					{
						IDeviceObject file = files[i];

						try
						{

							tmpFile tmpFile = new tmpFile();

							tmpFile.name = file.Name;
							tmpFile.date = ((Device)goProDevice).getFileCreationDate(file.Id);
							

							w = System.IO.File.Create(folderPath + "\\" + file.Name);
							r = ((Device)goProDevice).OpenReadForObjectId(file.Id);
							tmpFile.path = folderPath + "\\" + file.Name;

							byte[] buffer = new byte[bufferSize];
							int read = 0;


							CodeITBL.CRCHelper crcHelper = new CRCHelper();

							if (cancelCopyFromGoPro)
								break;

							while ((read = r.Read(buffer, 0, buffer.Length)) > 0)
							{
								if (cancelCopyFromGoPro)
								{
									w.Flush();
									w.Close();
									if (System.IO.File.Exists(folderPath + "\\" + file.Name))
										System.IO.File.Delete(folderPath + "\\" + file.Name);
									break;
								}


								w.Write(buffer, 0, read);
								if (read > 0)
									totalMBWritten += (decimal)((decimal)read / (decimal)mbsize);
								else
								{
									_logger.Warn("Bytes read is negative");
								}

								crcHelper.CalculateMD5Incremental(buffer, read);

								if (pbFiles.InvokeRequired)
								{

									pbFiles.Invoke(new MethodInvoker(delegate
									{
										if (pbFiles.Maximum >= (int)totalMBWritten)
											pbFiles.Value = (int)totalMBWritten;
										else
											pbFiles.Value = pbFiles.Maximum;
									}));

								}
								else
								{
									if (pbFiles.Maximum >= (int)totalMBWritten)
										pbFiles.Value = (int)totalMBWritten;
									else
										pbFiles.Value = pbFiles.Maximum;
								}
							}

							tmpFile.md5 = crcHelper.GetMD5IncrementalResult();

                            FileFromDB fileObject = new FileFromDB();

							try
							{
								fileObject.OriginalFileDate = tmpFile.date;
								fileObject.OriginalFileLocation = tmpFile.path;
								fileObject.OriginalFileName = tmpFile.name;
								fileObject.Note = dbFiles.Where(c=>c.OriginalFileName == tmpFile.name).FirstOrDefault().Note;
                                fileObject.NewFileName = dbFiles[i].NewFileName;
							}
							catch (Exception ex)
							{
								_logger.Error(ex.Message);
							}




							//delete files from tmp location

							//sekogash should delete deka ke gi brishe od temp
							try
							{

								new Task(new Action(() =>
								{


									Task t = new Task(new Action(() =>
									{
                                        beginUploadProcess(new List<FileFromDB> { fileObject }, true, uploadUser, tmpFile.md5);
									}));
									try
									{
										t.Start();
										t.Wait();

									}
									catch (Exception ex) { }
								})).Start();

							}
							catch (Exception exception)
							{
								_logger.Error(exception);
							}
							retryCount = 0;
							goProFiles.Add(file);
							i++;
						}
						catch (Exception ex)
						{
							if (retryCount < 10)
							{

								if (w != null)
									if (w.CanWrite)
									{
										w.Flush();
										w.Close();

										if (System.IO.File.Exists(folderPath + "\\" + file.Name))
											System.IO.File.Delete(folderPath + "\\" + file.Name);

									}
								if (r != null)
									r.Close();


								retryCount++;
								bufferSize = 131072;//128K  //262144; //256K //decrease buffer size                            
							}
							else
							{
								totalMBWritten += (decimal)((decimal)(((Device)goProDevice).getFileSize(file.Id)) / (decimal)mbsize);
								if (pbFiles.InvokeRequired)
								{

									pbFiles.Invoke(new MethodInvoker(delegate
									{
										if (pbFiles.Value != (int)totalMBWritten)
										{
											if (pbFiles.Maximum >= (int)totalMBWritten)
												pbFiles.Value = (int)totalMBWritten;
											else
												pbFiles.Value = pbFiles.Maximum;
										}
									}));

								}
								else
								{
									if (pbFiles.Maximum >= (int)totalMBWritten)
										pbFiles.Value = (int)totalMBWritten;
									else
										pbFiles.Value = pbFiles.Maximum;
								}
								retryCount = 0;
								i++; //continue this file fails 10 times
								bufferSize = 524288; // restore buffer size
							}
							//if timeout retry
						}
						finally
						{
							if (w != null)
								if (w.CanWrite)
								{
									w.Flush();
									w.Close();
								}
							if (r != null)
								r.Close();
						}
					}

					if (btnCancelCopy.InvokeRequired)
					{
						btnCancelCopy.Invoke(new Action(() => { btnCancelCopy.Visible = false; }));
					}
					else
						btnCancelCopy.Visible = false;

					hasFinishedGoProOperation = true;

					Context.IsUploadFromGoProStarted = false;

				}));
				task.Start();
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
			}
		}


		void btnCancelCopy_Click(object sender, EventArgs e)
		{
			cancelCopyFromGoPro = true;
		}


		private Int64 getAvailableSpaceInMB(string path)
		{
			ulong FreeBytesAvailable;
			ulong TotalNumberOfBytes;
			ulong TotalNumberOfFreeBytes;

			if (!path.EndsWith("\\"))
				path += '\\';
			bool success = GetDiskFreeSpaceEx(path, out FreeBytesAvailable, out TotalNumberOfBytes,
							   out TotalNumberOfFreeBytes);
			if (!success)
				throw new System.ComponentModel.Win32Exception();

			long mbsize = 1024 * 1024;

			return (long)FreeBytesAvailable / mbsize;
		}

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
		out ulong lpFreeBytesAvailable,
		out ulong lpTotalNumberOfBytes,
		out ulong lpTotalNumberOfFreeBytes);

		private void pbMinimize_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}

	}

	public class tmpFile
	{
		public string path { get; set; }
		public string name { get; set; }
		public DateTime date { get; set; }
		public string md5 { get; set; }
	}
}
