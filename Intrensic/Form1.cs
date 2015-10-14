using CodeITDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intrensic
{

    public partial class Form1 : Form, IDisposable
    {
        public Form1()
        {
            InitializeComponent();
            BindUserControl(@"C:\test\");
        }

        DriveDetector driveDetector = null;


        // Called by DriveDetector when removable device in inserted
        private void OnDriveArrived(object sender, DriveDetectorEventArgs e)
        {
            // e.Drive is the drive letter, e.g. "E:\\"
            // If you want to be notified when drive is being removed (and be
            // able to cancel it),
            // set HookQueryRemove to true

            String miscFolder = string.Format("{0}MISC", e.Drive);
            String versionFile = string.Format("{0}MISC\\version.txt", e.Drive);
            String dcimFolder = string.Format("{0}DCIM", e.Drive);
            bool isGoPro = false;
            bool hasVideos = false;

            if (Directory.Exists(miscFolder) && System.IO.File.Exists(versionFile))
                isGoPro = true;

            if (Directory.Exists(dcimFolder) && System.IO.Directory.GetFiles(dcimFolder, "*.mp4", SearchOption.AllDirectories).Length > 0)
                hasVideos = true;

            if (hasVideos && isGoPro)
                BindUserControl(e.Drive);
            //MessageBox.Show("THIS IS A GO PRO CAMERA WITH VIDEOS");
            else
                //MessageBox.Show("New drive arrived is not GO PRO camera!!! and assigned drive letter is: " + e.Drive);


                e.HookQueryRemove = true;
        }

        // Called by DriveDetector after removable device has been unplugged
        private void OnDriveRemoved(object sender, DriveDetectorEventArgs e)
        {
            // TODO: do clean up here, etc. Letter of the removed drive is in
            // e.Drive;
            MessageBox.Show("Drive removed, maybe perform logout????");
        }

        // Called by DriveDetector when removable drive is about to be removed
        private void OnQueryRemove(object sender, DriveDetectorEventArgs e)
        {
            // Should we allow the drive to be unplugged?
            if (MessageBox.Show("Allow remove?", "Query remove",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                e.Cancel = false;        // Allow removal
            else
                e.Cancel = true;         // Cancel the removal of the device
        }

        string filename = @"C:\GOPR0007.MP4";
        string filename_destination = @"C:\test_copy.mp4";
        string filename_destination_custom = @"C:\test_custom_copy.mp4";
        string filename_destination_custom2 = @"C:\test_custom_copy2.mp4";


        private void BindUserControl(string driveLetter)
        {

            DirectoryInfo dirInfo = new DirectoryInfo(driveLetter);
            FileInfo[] allFiles = dirInfo.GetFiles("*.MP4", SearchOption.AllDirectories);




            List<demo> list = new List<demo>();


            foreach (FileInfo fi in allFiles)
            {
                list.Add(new demo() { Date = fi.CreationTime.ToString(), Name = fi.Name, UniqueId = string.Format("{0}_{1}", fi.Name, new Random().Next(1, 1000)), FullPath = fi.FullName, HasNote = false, isFromCard = false, Note = string.Empty });
            }

            ucUserVideos uv = new ucUserVideos();

            //uv.ContextMenuClick += uv_ContextMenuClick;
            uv.lvItemsContainer.ContextMenu.Visibility = System.Windows.Visibility.Hidden;
            uv.lvItemsContainer.ContextMenu.IsOpen = false;


            ehVideos.Child = uv;

            uv.lvItemsContainer.ItemsSource = list;

        }

        void uv_ContextMenuClick(object sender, EventArgs e, ContextAction action, List<string> videos)
        {
            if (action == ContextAction.Preview && videos.Count > 0)
            {
                //axWindowsMediaPlayer1.URL = @"C:\GOPR0007.MP4";
                axWindowsMediaPlayer1.URL = videos[0];
                if (videos.Count == 1)
                {
                    return;
                }
                else
                {

                    for (int i = 1; i < videos.Count; i++)
                    {
                        WMPLib.IWMPMedia media = axWindowsMediaPlayer1.newMedia(videos[i]);
                        axWindowsMediaPlayer1.currentPlaylist.appendItem(media);
                    }
                }
            }
            //throw new NotImplementedException();
        }


        public void TestDB()
        {
            //testInsert();

           // testModify();


            //testDelete();
           
        }

        public void testInsert()
        {

            using (CodeITDbContext ctx = new CodeITDbContext(1))
            {
                for (int i = 1; i <= 10; i++)
                {
                    Customer newCustomer = new Customer() { Id = Guid.NewGuid(), Name = string.Format("Customer{0}", i) };
                    ctx.Customers.Add(newCustomer);
                }

                ctx.SaveChanges();
            }
        }

        public void testModify()
        {
            using (CodeITDbContext ctx = new CodeITDbContext(2))
            {
                List<Customer> customerList = ctx.Customers.Take(3).ToList();

                foreach (Customer customer in customerList)
                    customer.Name = string.Format("{0}_{1}", "UPDATED", customer.Name);

                ctx.SaveChanges();
            }
        }

        public void testDelete()
        {
            using (CodeITDbContext ctx = new CodeITDbContext(3))
            {
                List<Customer> customerList = ctx.Customers.Take(3).ToList();
                ctx.Customers.RemoveRange(customerList);
                ctx.SaveChanges();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Intrensic.Administration.frmUsers frmUsers = new Administration.frmUsers();
            //frmUsers.Show();

            //using (CodeITDbContext ctx = new CodeITDbContext(1))
            //{
                
            //}
            //CodeITDL.IntrensicEntities ie = new IntrensicEntities();
            
            //CodeITDL.CodeITDbContext ctx = new CodeITDbContext(1);
            //ctx.AuditLogs.

            TestDB();
            //frmProgressStatus progressStatus = new frmProgressStatus(new List<string>() { "" });
            //progressStatus.Show();
            //axWindowsMediaPlayer1.uiMode = "none";
            axWindowsMediaPlayer1.enableContextMenu = false;
            //axWindowsMediaPlayer1.fullScreen = false;
            //axWindowsMediaPlayer1.Ctlenabled = false;
            axWindowsMediaPlayer1.windowlessVideo = true;
            //axWindowsMediaPlayer1.EditMode = false;
            axWindowsMediaPlayer1.DoubleClickEvent += axWindowsMediaPlayer1_DoubleClickEvent;


            //driveDetector = new DriveDetector();
            //driveDetector.DeviceArrived += new DriveDetectorEventHandler(
            //    OnDriveArrived);
            //driveDetector.DeviceRemoved += new DriveDetectorEventHandler(
            //    OnDriveRemoved);
            //driveDetector.QueryRemove += new DriveDetectorEventHandler(
            //    OnQueryRemove);


            deleteFiles();


            frmUserMainScreen usrMainScreen = new frmUserMainScreen();
            usrMainScreen.Show();
            //CodeITBL.CopyFileAndComputeMD5Async copyMD5 = new CodeITBL.CopyFileAndComputeMD5Async();
            //copyMD5.AsyncCheckProgress += copyMD5_AsyncCheckProgress;
            //copyMD5.AsyncCopy(filename, filename_destination_custom);

            return;

            //CodeITBL.AsyncCheckEventArgs eventArgs = new CodeITBL.AsyncCheckEventArgs(CodeITBL.AsyncCheckState.Checking, "");
            //CodeITBL.AsyncMD5 asyncMD5 = new CodeITBL.AsyncMD5();
            //asyncMD5.AsyncCheckProgress += asyncMD5_AsyncCheckProgress;
            //asyncMD5.AsyncCheck(filename);

            //System.Threading.Thread.Sleep(10000);
            //SDCardMonitor.addInsetSDCard();
            //SDCardMonitor.AddremovalSD();  


            //ON APP START SCAN ALL DRIVES FOR GOPRO FOLDER STRUCTURE SINCE THEY MIGHT INSERTED THE SD CARD BEFORE
            //STARTING THE APPLICATION

            //INCREMENTAL CHECKSUM C#
            //CodeITBL.CRC32 crc32 = new CodeITBL.CRC32();

            //private uint crc = 0xFFFFFFFF;
            CopyAndCalculateCRC();
            System.Threading.Thread.Sleep(4000);
            CopyAndCalculateCRCAsync();
            CopyAndCalculateCRC_noasync();
            //CopyAndCalculateCRCAsync();

            //helper = new CodeITBL.CRCHelper();
            //helper.CalculaterAdler32Incremental(fileContent);
            //MessageBox.Show(helper.GetAdler32IncrementalValue().ToString());
            ////lblInfo.Text = result.ToString();
            //MessageBox.Show((DateTime.Now - start).TotalSeconds.ToString());
            //axVLCPlugin21.playlist.add(@"H:\DCIM\100GOPRO\GOPR0006.MP4");
            //axVLCPlugin21.playlist.play();
            //wmPlayer.st
            //WMPLib.WindowsMediaPlayerClass player = new WMPLib.WindowsMediaPlayerClass();


            //CodeITBL.VideoPlayback game = new CodeITBL.VideoPlayback();
            //game.Run();

            //using (CodeITBL.VideoPlayback game = new CodeITBL.VideoPlayback())
            //{
            //    game.Run();
            //}

            //Microsoft.Xna.Framework.Media.VideoPlayer vplayer = new Microsoft.Xna.Framework.Media.VideoPlayer();
            //Microsoft.Xna.Framework.Media.Video video = new Microsoft.Xna.Framework.Media.Video();
            //Microsoft.Xna.Framework.Graphics.GraphicsDevice device = new Microsoft.Xna.Framework.Graphics.GraphicsDevice();
            //Microsoft.Xna.Framework.Media.Video v = new Microsoft.Xna.Framework.Media.Video();
            //v.

            //vplayer.pl
            //Microsoft.Xna.Framework.Media.MediaPlayer player;

            //VideoPlayer play = new VideoPlayer()

            //*/ 


        }

        void axWindowsMediaPlayer1_DoubleClickEvent(object sender, AxWMPLib._WMPOCXEvents_DoubleClickEvent e)
        {
            //e.
            //throw new NotImplementedException();
        }

        void copyMD5_AsyncCheckProgress(CodeITBL.AsyncCheckEventArgs e)
        {
            if (e.State == CodeITBL.AsyncCheckState.Completed)
                MessageBox.Show("OD OVDE SUM: " + e.Value);
            //throw new NotImplementedException();
        }


        private void deleteFiles()
        {
            System.IO.File.Delete(filename_destination);
            System.IO.File.Delete(filename_destination_custom);
            System.IO.File.Delete(filename_destination_custom2);
        }

        private void CopyAndCalculateCRC()
        {
            uint result = 0xFFFFFFFF;

            DateTime start = DateTime.Now;

            byte[] fileContent = System.IO.File.ReadAllBytes(filename);

            //result = CodeITBL.CRCHelper.CalculateAdler32(fileContent);
            //result = new CodeITBL.CRCHelper().CalculateMD5(fileContent);




            Object[] res = CodeITBL.CRC32.Compute(fileContent, 0, fileContent.Length);

            //CRC32.Compute(dataArray, 0, read);
            result = CodeITBL.CRCCombine.Combine(result, checked((uint)res[0]), (int)res[1]);

            System.IO.File.Copy(filename, filename_destination);

            MessageBox.Show((DateTime.Now - start).TotalSeconds.ToString());
            MessageBox.Show(BitConverter.ToString(MD5.Create().ComputeHash(fileContent, 0, fileContent.Length)).Replace("-", ""));
            lblInfo.Text = result.ToString();
            MessageBox.Show(result.ToString());
        }


        private void CopyAndCalculateCRCAsync2()
        {
            uint result = 0xFFFFFFFF;

            DateTime start = DateTime.Now;

            start = DateTime.Now;
            int Readed = 0;
            IAsyncResult ReadResult;
            IAsyncResult WriteResult;
            FileStream sourceStream = System.IO.File.OpenRead(filename);
            FileStream destStream = System.IO.File.Open(filename_destination_custom, FileMode.OpenOrCreate);
            byte[] ActiveBuffer = new byte[4096];
            byte[] BackBuffer = new byte[4096];
            //uint c = 0xffffffff;
            result = 0xFFFFFFFF;
            //CodeITBL.CRCHelper helper = new CodeITBL.CRCHelper();
            //MD5
            //int read = 0;
            //while ((read = sourceStream.Read(ActiveBuffer, 0, ActiveBuffer.Length)) > 0)
            //{
            //    destStream.Write(ActiveBuffer, 0, read);
            //    Object[] res = CodeITBL.CRC32.Compute(ActiveBuffer, 0, ActiveBuffer.Length);

            //    result = CodeITBL.CRCCombine.Combine(result, checked((uint)res[0]), (int)res[1]);
            //}
            //destStream.Flush(true);
            //System.Threading.Interlocked.
            //sourceStream.be


            using (var md5 = MD5.Create())
            {

                ReadResult = sourceStream.BeginRead(ActiveBuffer, 0, ActiveBuffer.Length, null, null);
                //int offset = 0;
                do
                {
                    Readed = sourceStream.EndRead(ReadResult);

                    WriteResult = destStream.BeginWrite(ActiveBuffer, 0, Readed, null, null);
                    byte[] WriteBuffer = ActiveBuffer;



                    //helper.CalculaterAdler32Incremental(ActiveBuffer);

                    //CodeITBL.CRCHelper.countCrcContinius(WriteBuffer, ref c);

                    //md5.TransformBlock(ActiveBuffer, 0, ActiveBuffer.Length, null, 0);


                    if (Readed > 0)
                    {

                        md5.TransformBlock(ActiveBuffer, 0, ActiveBuffer.Length, ActiveBuffer, 0);

                        ReadResult = sourceStream.BeginRead(BackBuffer, 0, BackBuffer.Length, null, null);
                        BackBuffer = System.Threading.Interlocked.Exchange(ref ActiveBuffer, BackBuffer);
                    }
                    else
                        md5.TransformFinalBlock(ActiveBuffer, 0, ActiveBuffer.Length);


                    destStream.EndWrite(WriteResult);


                    //Object[] res = CodeITBL.CRC32.Compute(ActiveBuffer, 0, ActiveBuffer.Length);

                    //result = CodeITBL.CRCCombine.Combine(result, checked((uint)res[0]), (int)res[1]);
                }
                while (Readed > 0);

                //md5.TransformFinalBlock(new byte[0], 0, 0);

                MessageBox.Show((DateTime.Now - start).TotalSeconds.ToString());
                //MessageBox.Show( result.ToString());
                MessageBox.Show(BitConverter.ToString(md5.Hash).Replace("-", ""));

            }

        }


        private void CopyAndCalculateCRCAsync()
        {
            uint result = 0xFFFFFFFF;

            DateTime start = DateTime.Now;

            start = DateTime.Now;
            int Readed = 0;
            IAsyncResult ReadResult;
            IAsyncResult WriteResult;
            FileStream sourceStream = System.IO.File.OpenRead(filename);
            FileStream destStream = System.IO.File.Open(filename_destination_custom, FileMode.OpenOrCreate);
            byte[] ActiveBuffer = new byte[4096];
            byte[] BackBuffer = new byte[4096];
            //uint c = 0xffffffff;
            result = 0xFFFFFFFF;
            //CodeITBL.CRCHelper helper = new CodeITBL.CRCHelper();
            //MD5
            //int read = 0;
            //while ((read = sourceStream.Read(ActiveBuffer, 0, ActiveBuffer.Length)) > 0)
            //{
            //    destStream.Write(ActiveBuffer, 0, read);
            //    Object[] res = CodeITBL.CRC32.Compute(ActiveBuffer, 0, ActiveBuffer.Length);

            //    result = CodeITBL.CRCCombine.Combine(result, checked((uint)res[0]), (int)res[1]);
            //}
            //destStream.Flush(true);
            //System.Threading.Interlocked.
            //sourceStream.be


            using (var md5 = MD5.Create())
            {

                ReadResult = sourceStream.BeginRead(ActiveBuffer, 0, ActiveBuffer.Length, null, null);
                //int offset = 0;
                do
                {
                    Readed = sourceStream.EndRead(ReadResult);

                    WriteResult = destStream.BeginWrite(ActiveBuffer, 0, Readed, null, null);
                    byte[] WriteBuffer = ActiveBuffer;



                    //helper.CalculaterAdler32Incremental(ActiveBuffer);

                    //CodeITBL.CRCHelper.countCrcContinius(WriteBuffer, ref c);

                    //md5.TransformBlock(ActiveBuffer, 0, ActiveBuffer.Length, null, 0);


                    if (Readed > 0)
                    {

                        md5.TransformBlock(ActiveBuffer, 0, ActiveBuffer.Length, ActiveBuffer, 0);

                        ReadResult = sourceStream.BeginRead(BackBuffer, 0, BackBuffer.Length, null, null);
                        BackBuffer = System.Threading.Interlocked.Exchange(ref ActiveBuffer, BackBuffer);
                    }
                    else
                        md5.TransformFinalBlock(ActiveBuffer, 0, ActiveBuffer.Length);


                    destStream.EndWrite(WriteResult);


                    //Object[] res = CodeITBL.CRC32.Compute(ActiveBuffer, 0, ActiveBuffer.Length);

                    //result = CodeITBL.CRCCombine.Combine(result, checked((uint)res[0]), (int)res[1]);
                }
                while (Readed > 0);

                //md5.TransformFinalBlock(new byte[0], 0, 0);

                MessageBox.Show((DateTime.Now - start).TotalSeconds.ToString());
                //MessageBox.Show( result.ToString());
                MessageBox.Show(BitConverter.ToString(md5.Hash).Replace("-", ""));

            }

        }





        private void CopyAndCalculateCRC_noasync()
        {
            DateTime start = DateTime.Now;

            byte[] buffer;
            byte[] oldBuffer;
            int bytesRead;
            int oldBytesRead;
            long size;
            int totalBytesRead = 0;
            FileStream destStream = System.IO.File.Open(filename_destination_custom2, FileMode.OpenOrCreate);

            using (Stream stream = System.IO.File.OpenRead(filename))
            {
                using (var md5 = MD5.Create())
                {
                    size = stream.Length;

                    buffer = new byte[4096];

                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    totalBytesRead += bytesRead;

                    do
                    {
                        oldBytesRead = bytesRead;
                        oldBuffer = buffer;

                        buffer = new byte[4096];
                        bytesRead = stream.Read(buffer, 0, buffer.Length);


                        // destStream.Write(buffer, totalBytesRead, buffer.Length);


                        totalBytesRead += bytesRead;

                        if (bytesRead == 0)
                        {
                            md5.TransformFinalBlock(oldBuffer, 0, oldBytesRead);
                        }
                        else
                        {
                            md5.TransformBlock(oldBuffer, 0, oldBytesRead, oldBuffer, 0);
                        }

                        //BackgroundWorker.ReportProgress((int)​((double)totalBytesRead * 100 / size));
                    } while (bytesRead != 0);

                    MessageBox.Show(BitConverter.ToString(md5.Hash).Replace("-", ""));

                    //result = md5.Hash;
                }

            }

            MessageBox.Show((DateTime.Now - start).TotalSeconds.ToString());

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //axVLCPlugin21.AutoPlay = true;
            axWindowsMediaPlayer1.URL = "https://new-bucket-dcee6397.s3.amazonaws.com/test.avi?AWSAccessKeyId=AKIAJLTUZKRE6HYDWFDQ&Expires=1433401004&Signature=fWFWFw62bMqj1a7Fcb%2BHTjZvt7w%3D";//@"C:\GOPR0007.MP4";  
            //WMPLib.IWMPMedia media = axWindowsMediaPlayer1.newMedia(@"C:\GOPR0007.MP4");
            //axWindowsMediaPlayer1.currentPlaylist.appendItem(media);            
            axWindowsMediaPlayer1.Ctlcontrols.play();
            //axVLCPlugin21.playlist.play();
        }

        private void tsmLogin_Click(object sender, EventArgs e)
        {
            frmLogIn frmLogin = new frmLogIn();
            frmLogin.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.next();
        }
    }

    public class demo
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string UniqueId { get; set; }
        public string FullPath { get; set; }
        public bool HasNote { get; set; }
        public bool isFromCard { get; set; }
        public string Note { get; set; }
    }
}
