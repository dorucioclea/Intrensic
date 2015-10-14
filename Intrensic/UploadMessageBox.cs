using CodeITDL;
using PodcastUtilities.PortableDevices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shell32;
using System.Threading;
using CodeITBL;
using System.IO;
using IntrensicMediaPlayer;
using log4net;

namespace Intrensic
{
	public enum CustomMessageBoxResult
	{
		UploadAndDeleteSuccesfullyCompleted,
		UploadOnly,
		Cancel
	}
	public partial class frmUploadMessageBox : Form
	{

		public CustomMessageBoxResult Response { get; set; }
		private readonly ILog _logger = LogManager.GetLogger(typeof(frmUploadMessageBox));
		public List<IDeviceObject> SelectedFiles
		{
			get { return _selectedFiles; }
			set { _selectedFiles = value; }
		}

		public List<CodeITBL.FileFromDB> SelectedDbFiles
		{
			get { return _selectedDbFiles; }
			set { _selectedDbFiles = value; }
		}
		private List<IDeviceObject> allfiles = new List<IDeviceObject>();
		private List<IDeviceObject> _selectedFiles = new List<IDeviceObject>();
		private List<CodeITBL.FileFromDB> _selectedDbFiles = new List<CodeITBL.FileFromDB>();
		private IDevice _goproDevice = null;

		List<string> fileNames = new List<string>();
		public frmUploadMessageBox(string text, string title, List<IDeviceObject> files = null, User uploadUser = null, IDevice goproDevice = null)
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			InitializeComponent();
			allfiles = files;			
			_goproDevice = goproDevice;
			this.Text = title;
			this.lblMessage.Text = text;
			this.Icon = Intrensic.Properties.Resources.IntrensicDark;
			ucUserVideos.IsUpload = true;
			this.BackgroundImage = Context.GetImageForCustomer();
			CheckForVideos(files, uploadUser);
			
			//ChangeSize(pnlMain.Width -5, pnlMain.Height);           

		}

        List<FileFromDB> GoProVideos = new List<FileFromDB>();
		private int maxValue = 0;

		public string GetFilenameFormat(DateTime originalFileDate, string originalFileName, bool increaseCounter = false)
		{
			try
			{
				using (CodeITDL.CodeITDbContext ctx = new CodeITDL.CodeITDbContext(Context.UserId))
				{
					DateTime yesterday = originalFileDate.Date;
					DateTime tomorrow = originalFileDate.AddDays(1).Date;

					var items = ctx.Files.Where(c => c.UserId == Context.UserId && c.OriginalFileDate == originalFileDate && c.OriginalFileName == originalFileName).ToList();
					if (items.Count() > 0)
					{
						return items.FirstOrDefault().NewFileName;
					}

					List<CodeITDL.File> files = ctx.Files.Where(f => f.UserId == Context.UserId && f.OriginalFileDate > yesterday && f.OriginalFileDate < tomorrow).ToList();
					int value = 0;

					if (files != null && files.Count > 0)
					{
						if (files.Max(x => x.UploadId).HasValue)
						{
							value = files.Max(x => x.UploadId).Value;
						}
					}

					try
					{
						if (fileNames.Where(c => c.Split('.')[0] == "#" + originalFileDate.ToString("yyyyMMdd")).Count() > 0)
						{
							value = fileNames.Where(c => c.Split('.')[0] == "#" + originalFileDate.ToString("yyyyMMdd")).Max(c => Convert.ToInt32(c.Split('.')[1]));
						}
					}
					catch (Exception ex)
					{
						_logger.Error(ex.Message);
					}


					return String.Format("#{0}.{1}", originalFileDate.ToString("yyyyMMdd"), (value + 1).ToString().PadLeft(4, '0'));
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
			}
			return "";
		}


		private void CheckForVideos(List<IDeviceObject> _files, User _uploadUser)
		{
            string lastFilename = "";
            string newfilename = "";

			try
			{
				if (_files != null && _files.Count > 0)
				{
                    GoProVideos = new List<FileFromDB>();
					

                    for (int i = 0; i < _files.Count; i++)
                    {
                        var item = _files[i];
                        VideoInfo info = VideoInfo.GetVideoInfo(item.Id);
                        FileFromDB file = new FileFromDB();

                        FileInfo fileInfo = new FileInfo(item.Id);

                        file.OriginalFileDate = ((Device)_goproDevice).getFileCreationDate(item.Id);

                        file.OriginalFileName = item.Name;
                        file.OriginalFileLocation = item.Name;
						                        
						file.NewFileName = GetFilenameFormat(file.OriginalFileDate, file.OriginalFileName);
						fileNames.Add(file.NewFileName);                       

                        file.CreatedOn = DateTime.Now;
                        file.isFromCard = true;
                        file.UserName = item.Name;

                        if (info != null)
					    {
                            file.Duration = (int)info.Duration;
                            file.Resolution = info.Resolution;
                        }

                        GoProVideos.Add(file);
					}

					ucUserVideos.lvItemsContainer.ContextMenu.Visibility = System.Windows.Visibility.Hidden;
					ucUserVideos.lvItemsContainer.ContextMenu.IsOpen = false;
					ucUserVideos.IsUpload = true;
                    ucUserVideos.lvItemsContainer.ItemsSource = GoProVideos;

					if (ucUserVideos.itemContainer != null)
					{
						ucUserVideos.itemContainer.Width = pnlMain.Width - 21 > 0 ? pnlMain.Width - 21 : pnlMain.Width;
					}

					ucUserVideos.ContextMenuClick -= uv_ContextMenuClick;
					ucUserVideos.ContextMenuClick += uv_ContextMenuClick;
				}
			}
			catch (Exception)
			{

			}
		}

		void uv_ContextMenuClick(object sender, EventArgs e, ContextAction action, List<string> videos)
		{
			if (action == ContextAction.Preview && videos.Count > 0)
			{
				string folderName = Guid.NewGuid().ToString().Replace("-", string.Empty);
				string folderPath = Properties.Settings.Default.TempVideoLocation.TrimEnd('\\') + "\\" + folderName;
				if (!System.IO.Directory.Exists(folderPath))
					System.IO.Directory.CreateDirectory(folderPath);

				List<string> allVideos = new List<string>();
				SelectedItems();
				DownloadVideoFromMTPDevice(_selectedFiles, _goproDevice, folderPath);

				foreach (var item in _selectedFiles)
				{
					allVideos.Add(System.IO.Path.Combine(folderPath, item.Name));
				}

                VideoFromStream player = new VideoFromStream(true);
                player.LoadVideos(allVideos);
                player.Topmost = true;
                player.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                player.ShowDialog();
			}
		}

		public void DownloadVideoFromMTPDevice(List<IDeviceObject> videos, IDevice goProDevice = null, string folderPath = "")
		{
			long mbsize = 1024 * 1024;
			try
			{
				var task = new Task(new Action(() =>
				{

					List<tmpFile> tmpFiles = new List<tmpFile>();

					decimal totalMBWritten = 0;
					int retryCount = 0;
					Stream w = null;
					Stream r = null;
					const int bufferSize = 524288; //512K                   
					int i = 0;
					while (i < videos.Count)
					{
						IDeviceObject file = videos[i];

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

							while ((read = r.Read(buffer, 0, buffer.Length)) > 0)
							{
								w.Write(buffer, 0, read);
								crcHelper.CalculateMD5Incremental(buffer, read);
							}

							tmpFile.md5 = crcHelper.GetMD5IncrementalResult();
							retryCount = 0;
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
								//bufferSize = 131072;//128K  //262144; //256K //decrease buffer size                            
							}
							else
							{
								totalMBWritten += (decimal)((decimal)(((Device)goProDevice).getFileSize(file.Id)) / (decimal)mbsize);

								retryCount = 0;
								i++; //continue this file fails 10 times
								//bufferSize = 524288; // restore buffer size
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
				}));
				task.Start();
				task.Wait();
			}
			catch (Exception ex)
			{

			}

		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (ucUserVideos.lvItemsContainer.SelectedItems.Count > 0)
			{
				Response = CustomMessageBoxResult.UploadAndDeleteSuccesfullyCompleted;
				SelectedItems();
				this.Close();
			}
			else
			{
				MessageBox.Show(this, "Select one or more videos to be uploaded", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void btnUploadAndKeep_Click(object sender, EventArgs e)
		{
			if (ucUserVideos.lvItemsContainer.SelectedItems.Count > 0)
			{
				Response = CustomMessageBoxResult.UploadOnly;
				SelectedItems();
				this.Close();
			}
			else
			{
				MessageBox.Show(this, "Select one or more videos to be uploaded", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Response = CustomMessageBoxResult.Cancel;
			_selectedFiles = new List<IDeviceObject>();
			this.Close();
		}

		private void frmUploadMessageBox_Shown(object sender, EventArgs e)
		{
			Context.IsUploadMessageBoxVisible = true;
		}

		private void frmUploadMessageBox_FormClosing(object sender, FormClosingEventArgs e)
		{
			Context.IsUploadMessageBoxVisible = false;
		}

		public System.Windows.Forms.Integration.ElementHost getElementHostControl { get { return this.ehUserVideos; } }
		public ucUserVideos getUserVideosConrtol { get { return this.ucUserVideos; } }

		private void frmUploadMessageBox_SizeChanged(object sender, EventArgs e)
		{
			ehUserVideos.Dock = DockStyle.Fill;
			ehUserVideos.Width = pnlMain.Width;
			ehUserVideos.Height = pnlMain.Height;
			ucUserVideos.Width = pnlMain.Width;
			ucUserVideos.Height = pnlMain.Height;
			ucUserVideos.ControlWidht = pnlMain.Width;

			if (ucUserVideos.itemContainer != null)
			{
				ucUserVideos.itemContainer.Width = pnlMain.Width - 25 > 0 ? pnlMain.Width - 25 : pnlMain.Width;
			}
		}

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			ucUserVideos.lvItemsContainer.SelectAll();
		}

		private void btnClearSelction_Click(object sender, EventArgs e)
		{
			ucUserVideos.lvItemsContainer.SelectedItem = null;
		}

		private void SelectedItems()
		{
			_selectedFiles = new List<IDeviceObject>();
			_selectedDbFiles = new List<CodeITBL.FileFromDB>();
			foreach (var item in ucUserVideos.lvItemsContainer.SelectedItems)
			{
				//file = allfiles.Where(c => c.Name == item.ToString()).FirstOrDefault();
				var file = item as FileFromDB;
				if (file != null)
				{
					
					_selectedDbFiles.Add(file);
					_selectedFiles.Add(allfiles.Where(c => c.Name == file.OriginalFileName).FirstOrDefault());
				}
			}
		}

		private void frmUploadMessageBox_Resize(object sender, EventArgs e)
		{
			ucUserVideos.InvalidateVisual();
			ehUserVideos.Invalidate();
		}

		private void frmUploadMessageBox_Load(object sender, EventArgs e)
		{
			pnlMain.Width = pnlMain.Width + 5;
			this.Width = this.Width + 5;
		}
	}
}
