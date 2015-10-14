using System.IO;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Threading.Tasks;
using log4net;

namespace CodeITBL
{
    internal class AwsFileUpload : FileTransfer, IDisposable, IFileTransfer
    {

        private readonly ILog _logger = LogManager.GetLogger(typeof(AwsFileUpload));
        private const long FileSize = 5 * 1024 * 1024;

        private readonly AmazonS3Client _amazonS3Client;
        private readonly TransferUtility _fileTransferUtility;


        private readonly String _bucketName;


        public AwsFileUpload(String name, String sourceFilePath, Stream stream, String bucketName, String accessKey, String secretKey, string originalFileName, string note, DateTime originalFileDate, bool isDownload, string md5, Int32 duration = 0, string resolution = "")
            : base(name, sourceFilePath, stream, originalFileName, note, originalFileDate, isDownload, duration, resolution)
        {
            this._bucketName = bucketName;

            if (!string.IsNullOrEmpty(md5))
                base._md5 = md5;


            var fileTransferUtilityConfig = new TransferUtilityConfig { ConcurrentServiceRequests = 10 };
            AWSConfigs.S3Config.UseSignatureVersion4 = true;
            string regionFromConfig = System.Configuration.ConfigurationSettings.AppSettings.Get("EndpointRegion");


            RegionEndpoint regionEndpoint = RegionEndpoint.APNortheast1;

            switch (regionFromConfig.ToLower())
            {
                case "apsoutheast1":
                    regionEndpoint = RegionEndpoint.APSoutheast1;
                    break;
                case "apsoutheast2":
                    regionEndpoint = RegionEndpoint.APSoutheast2;
                    break;
                case "cnnorth1":
                    regionEndpoint = RegionEndpoint.CNNorth1;
                    break;
                case "eucentral1":
                    regionEndpoint = RegionEndpoint.EUCentral1;
                    break;
                case "euwest1":
                    regionEndpoint = RegionEndpoint.EUWest1;
                    break;
                case "saeast1":
                    regionEndpoint = RegionEndpoint.SAEast1;
                    break;
                case "useast1":
                    regionEndpoint = RegionEndpoint.USEast1;
                    break;
                case "usgovcloudwest1":
                    regionEndpoint = RegionEndpoint.USGovCloudWest1;
                    break;
                case "uswest1":
                    regionEndpoint = RegionEndpoint.USWest1;
                    break;
                case "uswest2":
                    regionEndpoint = RegionEndpoint.USWest2;
                    break;
                default: break; //APNortheast1
            }


            _amazonS3Client = new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), regionEndpoint);
            _amazonS3Client.ExceptionEvent += amazonS3Client_ExceptionEvent;

            _fileTransferUtility = new TransferUtility(_amazonS3Client, fileTransferUtilityConfig);
        }


        void amazonS3Client_ExceptionEvent(object sender, ExceptionEventArgs e)
        {
            _logger.Error(e.ToString());
            throw new Exception(e.ToString());
        }

        public String Url()
        {
            var fileUrlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = _bucketName,
                Key = _name,
                Expires = DateTime.Now.AddMinutes(60)
            };

            return _amazonS3Client.GetPreSignedURL(fileUrlRequest);
        }

        public String Resolution()
        {
            return _resolution;
        }

        public Int32 Duration()
        {
            return _duration;
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

                try
                {
                    var fileDownloadUtilityRequest = new TransferUtilityDownloadRequest
                    {
                        BucketName = _bucketName,
                        FilePath = _sourceFilePath,
                        Key = _name,
                    };

                    fileDownloadUtilityRequest.WriteObjectProgressEvent += DownloadProgressEvent;

                    OnStateChanged(new StateChangeEventArg(FileTransferState.InProgress));

                    _fileTransferUtility.DownloadAsync(fileDownloadUtilityRequest, _token).Wait(_token);

                    VerifyDownload(fileMd5);

                }
                catch (AmazonS3Exception e)
                {
                    _logger.Error(e.Message, e);
                    //throw;
                }
            }, _token);

            return task;
        }

        private void VerifyDownload(String fileMd5)
        {
            if (string.IsNullOrEmpty(fileMd5))
            {
                OnStateChanged(new StateChangeEventArg(FileTransferState.Done));
                return;
            }
            OnStateChanged(new StateChangeEventArg(FileTransferState.Verifying));

            String md5 = CalculateMd5(_sourceFilePath);

            OnStateChanged(md5 != fileMd5
                ? new StateChangeEventArg(FileTransferState.ErrorVerification)
                : new StateChangeEventArg(FileTransferState.Done));
        }

        public Task CopyFile(IProgress<TaskProgressReport> taskProgress)
        {
            _progress = taskProgress;

            var task = new Task((t) =>
            {
                if (_token.IsCancellationRequested)
                {
                    _token.ThrowIfCancellationRequested();
                }

                try
                {
                    var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                    {
                        BucketName = _bucketName,
                        StorageClass = S3StorageClass.Standard,
                        PartSize = FileSize,
                        Key = _name,
                        AutoCloseStream = false,
                        AutoResetStreamPosition = true,
                    };
                    
                    if (_stream != null)
                    {
                        fileTransferUtilityRequest.InputStream = _stream;
                    }
                    else
                    {
                        fileTransferUtilityRequest.FilePath = _sourceFilePath;
                    }

                    OnStateChanged(new StateChangeEventArg(FileTransferState.CalculatingChecksum));
                    if (string.IsNullOrEmpty(_md5))
                        _md5 = CalculateMd5();
                    fileTransferUtilityRequest.Headers.ContentMD5 = _md5;

                    fileTransferUtilityRequest.UploadProgressEvent += UploadProgressEvent;

                    OnStateChanged(new StateChangeEventArg(FileTransferState.InProgress));

                    _fileTransferUtility.UploadAsync(fileTransferUtilityRequest, _token).Wait(_token);

                    OnStateChanged(new StateChangeEventArg(FileTransferState.Done));

                    //throw new Exception("test");
                }
                catch (Exception e)
                {
                    if (e is OperationCanceledException)
                    {
                        _logger.Error(e.Message, e);
                    }
                    else
                    {
                        throw;
                    }
                }
            }, _token);

            return task;
        }

        public void Dispose()
        {
            _fileTransferUtility.Dispose();
            _amazonS3Client.Dispose();
            if (_stream != null)
            {
                _stream.Close();
                _stream.Dispose();
            }
        }


        public bool IsDownload()
        {
            return base._isDownload;
        }
    }
}

