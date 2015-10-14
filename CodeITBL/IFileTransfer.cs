using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeITBL
{
    public delegate void StateChangedEventHandler(object sender, StateChangeEventArg e);

    public interface IFileTransfer
    {
        event StateChangedEventHandler StateChanged;

        String Url();
        Task DownloadFile(IProgress<TaskProgressReport> taskProgress, String md5);
        Task CopyFile(IProgress<TaskProgressReport> taskProgress);
        void Cancel();
        String Name();
        String SourceFilePath();
        string Note();
        DateTime OriginalFileDate();
        string OriginalFileName();
        string Md5();        
        bool IsDownload();
        Int32 Duration();
        string Resolution();
        
    }
}
