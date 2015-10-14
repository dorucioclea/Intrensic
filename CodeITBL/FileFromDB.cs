using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeITBL
{
    public class FileFromDB
    {
        private const string DateFormatStr = "MM/dd/yyyy hh:mm:ss a";

        public int Id { get; set; }
        public int UserId { get; set; }
        public string OriginalFileName { get; set; }
        public System.DateTime OriginalFileDate { get; set; }
        public string NewFileName { get; set; }
        public string MD5Checksum { get; set; }
        public string OriginalFileLocation { get; set; }
        public string NewFileLocation { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public bool IsArchived { get; set; }
		public bool IsCloudFileSystem { get; set; }
        public string Note { get; set; }
        public Int32 Duration { get; set; }
        

        public string DurationFormat
        {
            get
            {
                TimeSpan span = TimeSpan.FromSeconds(Duration);
                string hours = (span.Hours > 10) ? span.Hours.ToString() : "0" + span.Hours.ToString();
                string minutes = (span.Minutes > 10) ? span.Minutes.ToString() : "0" + span.Minutes.ToString();
                string seconds = (span.Seconds > 10) ? span.Seconds.ToString() : "0" + span.Seconds.ToString();
                return String.Format("{0}:{1}:{2}s", hours, minutes, seconds);
            }
        }

        public string Resolution { get; set; }

        public bool isFromCard { get; set; }
        public bool HasNote
        {
            get
            {
                return !string.IsNullOrEmpty(Note);
            }
        }


        public string UserName { get; set; }

        public object FileObject { get; set; }

        public FileFromDB() 
        {
            CreatedBy = -1;
            CreatedOn = DateTime.Now;
            FileObject = null;
            UserName = String.Empty;
            Id = -1;
            IsArchived = false;
            MD5Checksum = string.Empty;
            NewFileLocation = string.Empty;
            NewFileName = string.Empty;
            Note = string.Empty;
            OriginalFileDate = DateTime.Now;
            OriginalFileLocation = string.Empty;
            OriginalFileName = string.Empty;
            UserId = -1;
            isFromCard = false;
            Duration = 0;
            Resolution = string.Empty;        
        }

        public FileFromDB(CodeITDL.File file, Int32 index)
        {
            CreatedBy = file.CreatedBy;
            CreatedOn = file.CreatedOn;
            FileObject = file;
            UserName = file.UserName;
            Id = file.Id;
            IsArchived = file.IsArchived;
            MD5Checksum = file.MD5Checksum;
            NewFileLocation = file.NewFileLocation;
            NewFileName = file.NewFileName;
            Note = file.Note;
            OriginalFileDate = file.OriginalFileDate;
            OriginalFileLocation = file.OriginalFileLocation;
            OriginalFileName = file.OriginalFileName;
            UserId = file.UserId;
            isFromCard = false;
            Duration = (int)file.Duration;
            Resolution = file.Resolution;
			IsCloudFileSystem = (bool)file.IsCloudFileSystem;
        }
    }
}
