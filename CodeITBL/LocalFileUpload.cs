using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using CodeITLicence;
using log4net;
using System.Security.AccessControl;

namespace CodeITBL
{
    internal class LocalFileUpload : FileTransfer, IDisposable, IFileTransfer
    {
        private readonly int _bufferLength = (int)Math.Pow(2, 19);
		private readonly ILog _logger = LogManager.GetLogger(typeof(LocalFileUpload));
        private readonly String _localPath;
        private readonly String _username;
        private readonly String _password;

        public LocalFileUpload(String name, String sourceFilePath, Stream stream, String localPath, String username, String password, string originalFileName, string note, DateTime originalFileDate, bool isDownload, string md5, Int32 duration = 0, string resolution = "")
            : base(name, sourceFilePath, stream, originalFileName, note, originalFileDate, isDownload, duration, resolution)
        {
			_localPath = GetLocalPath(localPath);
            _username = username;
            _password = password;
            if (!string.IsNullOrEmpty(md5))
                base._md5 = md5;
        }

		private static string GetLocalPath(string ls_ShareName)
		{
			string ipadress = "";
			string hostname = "";
			string result = string.Empty;
			try
			{
				if (ls_ShareName.Split('\\').Length > 2)
				{
					hostname = ls_ShareName.Split('\\')[2];
					if (!String.IsNullOrEmpty(hostname))
					{
						IPAddress address = NetworkDrive.ResolveHostNameToIP(hostname);
						if (address != null)
						{
							ipadress = address.ToString();
						}
					}
				}
			}
			catch (Exception ex)
			{
				//_logger.Error(ex.Message);
			}

			if (!string.IsNullOrEmpty(ipadress))
			{
				result = ls_ShareName.Replace(hostname, ipadress);
			}
			return result;
		}

        public String Url()
        {
            string url = Path.Combine(_localPath, string.Format("{0}{1}",_name,".mp4"));
            //return url.StartsWith("\\") ? string.Empty : url;
			return url;
        }

        public Int32 Duration()
        {
            return _duration;
        }

        public string Resolution()
        {
            return _resolution;
        }

        public Task DownloadFile(IProgress<TaskProgressReport> taskProgress, String fileMd5)
        {
            _progress = taskProgress;

            var task = new Task((t) =>
            {
                if (_token.IsCancellationRequested)
                {
                    _token.ThrowIfCancellationRequested();
                }

                OnStateChanged(new StateChangeEventArg(FileTransferState.InProgress));

                if (_localPath.StartsWith("\\") && !string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                {
                    using (new NetworkConnection(_localPath, new NetworkCredential(_username, _password)))
                    {
                        DownloadFile();
                        VerifyDownload(fileMd5);
                    }
                }
                else
                {
                    DownloadFile();
                    VerifyDownload(fileMd5);
                }

            }, _token);


            return task;
        }

        private void VerifyDownload(String fileMd5)
        {
            OnStateChanged(new StateChangeEventArg(FileTransferState.Verifying));

            String md5 = CalculateMd5(_sourceFilePath);

            OnStateChanged(md5 != fileMd5
                ? new StateChangeEventArg(FileTransferState.ErrorVerification)
                : new StateChangeEventArg(FileTransferState.Done));
        }

        private void DownloadFile()
        {
            var dataArray = new byte[_bufferLength];
            string filename = "";

            try
            {
                filename = Path.Combine(_localPath, string.Format("{0}{1}", _name, ".mp4"));
                
                using (FileStream fsread = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, _bufferLength))
                {

                    if (fsread != null)
                    {
                        long fileLength = fsread.Length;
                        using (var bwread = new BinaryReader(fsread))
                        {
                            using (var fswrite = new FileStream(_sourceFilePath, FileMode.Create, FileAccess.Write, FileShare.None, _bufferLength))
                            {
                                if (fswrite != null)
                                {
                                    using (var bwwrite = new BinaryWriter(fswrite))
                                    {
                                        long totalBytes = 0;
                                        int read;
                                        while ((read = bwread.Read(dataArray, 0, _bufferLength)) > 0)
                                        {
                                            if (_token.IsCancellationRequested)
                                            {
                                                _token.ThrowIfCancellationRequested();
                                            }

                                            bwwrite.Write(dataArray, 0, read);

                                            totalBytes += read;
                                            UploadProgressEvent(this, new UploadProgressArgs(read, totalBytes, fileLength));
                                        }
                                    }
                                }
                                else
                                {
                                    _logger.Error("Stream write for filename: " + filename + " error. Cannot open stream!");
                                }
                            }
                        }
                    }
                    else
                    {
                        _logger.Error("Stream read for filename: " + filename + " error. Cannot open stream!");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Exception occuredi in DownloadFile: " + filename);
                _logger.Error(ex.Message);
            }
        }


        public Task CopyFile(IProgress<TaskProgressReport> taskProgress)
        {
            _progress = taskProgress;

            try
            {
                if (_token.IsCancellationRequested)
                {
                    _token.ThrowIfCancellationRequested();
                }

                var task = new Task((t) =>
                {
                    OnStateChanged(new StateChangeEventArg(FileTransferState.CalculatingChecksum));

                    _md5 = CalculateMd5();

                    OnStateChanged(new StateChangeEventArg(FileTransferState.InProgress));

                    if (_localPath.StartsWith("\\") && !string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                    {
                        _logger.Info("Try to map location on network");
                        using (new NetworkConnection(_localPath, new NetworkCredential(_username, _password)))
                        {
                            CopyFile();
                            VerifyUpload();
                        }
                    }
                    else
                    {
                        CopyFile();
                        VerifyUpload();
                    }

                }, _token);

                return task;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception occured in Copy file: " + ex.Message);
            }

            return null;
        }

        private void VerifyUpload()
        {
            OnStateChanged(new StateChangeEventArg(FileTransferState.Verifying));

            String md5 = CalculateMd5(Path.Combine(_localPath, string.Format("{0}{1}",_name, ".mp4")));

            OnStateChanged(md5 != _md5
                ? new StateChangeEventArg(FileTransferState.ErrorVerification)
                : new StateChangeEventArg(FileTransferState.Done));
        }

        private void CopyFile()
        {
            if (_stream != null)
            {
                CopyFileWithStream(_stream);
            }
            else
            {
                using (var fsread = new FileStream(_sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.None, _bufferLength))
                {
                    CopyFileWithStream(fsread);
                }
            }
        }

        private void CopyFileWithStream(Stream stream)
        {
            var dataArray = new byte[_bufferLength];
            long fileLength = stream.Length;
            string filename = "";

            try
            {
                filename = Path.Combine(_localPath, string.Format("{0}{1}", _name, ".mp4"));

                using (var bwread = new BinaryReader(stream))
                {
                    using (var fswrite = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, _bufferLength))
                    {
                        if (fswrite != null)
                        {
                            using (var bwwrite = new BinaryWriter(fswrite))
                            {
                                if (bwwrite != null)
                                {
                                    long totalBytes = 0;
                                    int read;
                                    while ((read = bwread.Read(dataArray, 0, _bufferLength)) > 0)
                                    {
                                        if (_token.IsCancellationRequested)
                                        {
                                            _token.ThrowIfCancellationRequested();
                                        }

                                        bwwrite.Write(dataArray, 0, read);

                                        totalBytes += read;
                                        UploadProgressEvent(this, new UploadProgressArgs(read, totalBytes, fileLength));
                                    }
                                }
                                else
                                {
                                    _logger.Error("Write stream is null: " + filename);
                                }
                            }
                        }
                        else
                        {
                            _logger.Error("Read stream is null: " + filename);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured in CopyFileWithStream: " + ex.Message);
            }
        }

        public void Dispose()
        {
            // do nothing
        }


        public bool IsDownload()
        {
            return base._isDownload;
        }
    }
}

