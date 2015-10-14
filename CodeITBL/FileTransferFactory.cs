using System;
using System.IO;
using System.Threading.Tasks;
using CodeITLicence;

namespace CodeITBL
{
    public static class FileTransferFactory
    {
        static FileTransferFactory() { }

        public static IFileTransfer GetFileTransfer(String fileName, String filePath, Stream stream, string originalFileName, string note, DateTime originalFileDate, bool isDownload, string md5, Int32 duration = 0, string resolution = "")
        {
            if (Licence.StorageType == StorageType.Cloud)
            {
                return new AwsFileUpload(fileName, filePath, stream, Licence.ClientId, Licence.CloudId, Licence.CloudSecret, originalFileName, note, originalFileDate, isDownload, md5, duration, resolution);
            }

            return new LocalFileUpload(fileName, filePath, stream, Licence.LocalPath, Licence.LocalUsername, Licence.LocalPassword, originalFileName, note, originalFileDate, isDownload, md5, duration, resolution);
        }
    }
}
