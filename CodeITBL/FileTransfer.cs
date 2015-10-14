using System;
using System.Data;
using System.IO;
using System.Threading;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using log4net;

namespace CodeITBL
{
    abstract class FileTransfer
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(FileTransfer));

        public event StateChangedEventHandler StateChanged;

        public virtual void OnStateChanged(StateChangeEventArg arg)
        {
            if (StateChanged != null)
            {
                StateChanged(this, arg);
            }
        }

        internal readonly String _name;
        internal readonly String _sourceFilePath;
        internal readonly string _originalFileName;
        internal readonly string _note;
        internal readonly DateTime _originalFileDate;
        internal String _md5;
        internal bool _isDownload;
        internal Stream _stream;
        internal Int32 _duration;
        internal string _resolution;

        internal IProgress<TaskProgressReport> _progress;
        private readonly CancellationTokenSource _tokenSource;
        internal CancellationToken _token;

        protected FileTransfer(String name, String sourceFilePath, Stream stream, string originalFileName, string note, DateTime originalFileDate, bool isDownload, Int32 duration, string resolution)
        {
            if (StateChanged != null)
            {
                StateChanged(this, new StateChangeEventArg(FileTransferState.Inital));
            }

            this._name = name;
            this._sourceFilePath = sourceFilePath;
            this._originalFileDate = originalFileDate;
            this._originalFileName = originalFileName;
            this._note = note;
            this._isDownload = isDownload;
            this._stream = stream;     
            this._duration = duration;
            this._resolution = resolution;

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
        }

        internal string CalculateMd5()
        {
            byte[] hash;
            var md5 = System.Security.Cryptography.MD5.Create();
            
            if (_stream != null)
            {
                hash = md5.ComputeHash(_stream);
            }
            else
            {
                using (var stream = new FileStream(_sourceFilePath, FileMode.Open, FileAccess.Read))
                {
                    hash = md5.ComputeHash(stream);
                }
            }
            

            return Convert.ToBase64String(hash);
        }

        internal string CalculateMd5(String localPath)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            using (var stream = new FileStream(localPath, FileMode.Open, FileAccess.Read))
            {
                var hash = md5.ComputeHash(stream);
                return Convert.ToBase64String(hash);
            }
        }


        internal void UploadProgressEvent(object sender, UploadProgressArgs e)
        {
            _logger.Info(sender.ToString() + " -> " + e.TransferredBytes);
            var progressReport = new TaskProgressReport
            {
                Name = _name,
                PercentComplete = e.PercentDone,
                CurrentStep = e.TransferredBytes,
                TotalSteps = e.TotalBytes
            };
            
            _progress.Report(progressReport);
        }

        internal void DownloadProgressEvent(object sender, WriteObjectProgressArgs e)
        {
            var progressReport = new TaskProgressReport
            {
                Name = _name,
                PercentComplete = e.PercentDone,
                CurrentStep = e.TransferredBytes,
                TotalSteps = e.TotalBytes
            };

            _progress.Report(progressReport);
        }

        public void Cancel()
        {
            _tokenSource.Cancel();
        }

        public String Name()
        {
            return _name;
        }

        public String SourceFilePath()
        {
            return _sourceFilePath;
        }

        public String Md5()
        {
            return _md5;
        }

        public string Note()
        {
            return _note;
        }

        public DateTime OriginalFileDate()
        {
            return _originalFileDate;
        }

        public string OriginalFileName()
        {
            return _originalFileName;
        }
    }

    public class StateChangeEventArg : EventArgs
    {
        public StateChangeEventArg(FileTransferState arg)
        {
            FileTransferState = arg;
        }

        public FileTransferState FileTransferState { get; set; }
    }

    public enum FileTransferState
    {
        Inital,
        CalculatingChecksum,
        InProgress,
        Verifying,
        ErrorVerification,
        Done,
    }
}
