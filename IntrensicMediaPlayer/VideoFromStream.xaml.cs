using CodeITBL;
using log4net;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IntrensicMediaPlayer
{
    /// <summary>
    /// Interaction logic for VideoFromStream.xaml
    /// </summary>
    public partial class VideoFromStream : Window
    {
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;

        DispatcherTimer timer = null;

        private readonly ILog _logger = LogManager.GetLogger(typeof(VideoFromStream));

        public VideoFromStream(bool isUpload = false)
        {
            InitializeComponent();

            InitPlayerElements(isUpload);

            axMediaPlayer.enableContextMenu = false;
            axMediaPlayer.fullScreen = false;
            
            axMediaPlayer.windowlessVideo = true;
            axMediaPlayer.network.bufferingTime = 30;
            axMediaPlayer.uiMode = "none";
        }

		private Int32 counter = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (mediaPlayerIsPlaying && (axMediaPlayer.currentMedia != null && axMediaPlayer.currentMedia.duration > 0) && (!userIsDraggingSlider))
                {
                    counter++;

                    sliProgress.Minimum = 0;
                    sliProgress.Maximum = axMediaPlayer.currentMedia.duration;
                    sliProgress.Value = counter;
                }
                
                if (axMediaPlayer.Ctlcontrols.currentPosition == 0.0)
                {
                    lblProgressStatus.Text = "00:00:00s";
                    sliProgress.Value = 0;
                }
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void InitPlayerElements(bool isUpload)
        {
            if (isUpload)
            {
                pnlDuration.Visibility = Visibility.Hidden;
                pnlResolution.Visibility = Visibility.Hidden;
                pnlUsername.Visibility = Visibility.Hidden;
                pnlUsername.Visibility = Visibility.Hidden;
                pnlCreatedOn.Visibility = Visibility.Hidden;
            }
            
        }

        private List<FileFromDB> selectedFiles = new List<FileFromDB>();

        private FileFromDB GetFileFromSelected(string url)
        {
            return selectedFiles.FirstOrDefault(x => x.NewFileLocation == url);
        }

        public List<string> GetUrls(List<FileFromDB> selectedFiles)
        {
            this.selectedFiles = selectedFiles;
            List<string> urls = new List<string>();

            foreach (FileFromDB file in selectedFiles)
            {
                urls.Add(file.NewFileLocation);
            }

            return urls;
        }

        public void LoadVideos(List<string> videoUris)
        {
            try
            {
                if (videoUris.Count > 0)
                {
                    axMediaPlayer.URL = videoUris[0];
                    if (videoUris.Count == 1)
                    {
                        return;
                    }
                    else
                    {
                        for (int i = 1; i < videoUris.Count; i++)
                        {
                            WMPLib.IWMPMedia media = axMediaPlayer.newMedia(videoUris[i]);
                            axMediaPlayer.currentPlaylist.appendItem(media);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        public void LoadVideos(List<FileFromDB> selectedFiles)
        {
            try
            {
                List<string> videos = GetUrls(selectedFiles);

                axMediaPlayer.URL = videos[0];
                if (videos.Count == 1)
                {
                    return;
                }
                else
                {
                    for (int i = 1; i < videos.Count; i++)
                    {
                        WMPLib.IWMPMedia media = axMediaPlayer.newMedia(videos[i]);
                        axMediaPlayer.currentPlaylist.appendItem(media);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    WMPLib.IWMPMedia media = axMediaPlayer.newMedia(openFileDialog.FileName);
                    axMediaPlayer.currentPlaylist.appendItem(media);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (axMediaPlayer != null) && (axMediaPlayer.currentMedia != null);
        }

        private void InitTimer()
        {
            timer = null;
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        private void StartPlaying()
        {
            try
            {
                if (axMediaPlayer != null)
                {
                    InitTimer();

                    timer.Start();

                    axMediaPlayer.Ctlcontrols.play();

                    SetVideoInfo(axMediaPlayer.URL);

                    mediaPlayerIsPlaying = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                timer.Start();
                mediaPlayerIsPlaying = true;
                axMediaPlayer.Ctlcontrols.play();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void SetVideoInfo(string urlStr)
        {
            try
            {
                FileFromDB file = GetFileFromSelected(urlStr);
                
                if (file != null)
                {
                    Duration.Text = file.DurationFormat;
                    Resolution.Text = file.Resolution;
                    Username.Text = file.UserName;
                    CreatedOn.Text = file.OriginalFileDate.ToString("MM/dd/yyyy hh:mm:ss tt");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }


        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            axMediaPlayer.Ctlcontrols.pause();
            mediaPlayerIsPlaying = false;
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
           
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                axMediaPlayer.Ctlcontrols.stop();
                mediaPlayerIsPlaying = false;

                if (timer != null)
                {
                    timer.Stop();
                }
                sliProgress.Value = 0;
                lblProgressStatus.Text = "00:00:00s";
                counter = 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }


        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            try
            {
                userIsDraggingSlider = false;

                if (axMediaPlayer != null)
                {
                    axMediaPlayer.Ctlcontrols.currentPosition = sliProgress.Value;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (axMediaPlayer != null && axMediaPlayer.Ctlcontrols != null)
                {
                    string position = String.Format("{0:0.00}", axMediaPlayer.Ctlcontrols.currentPositionString);
                    if (!string.IsNullOrEmpty(position))
                    {
                        double currentPosition = Convert.ToDouble(position.Split(':')[1]) + Convert.ToDouble(position.Split(':')[0]) * 60;
                        
                        lblProgressStatus.Text = TimeSpan.FromSeconds(currentPosition).ToString(@"hh\:mm\:ss") + "s";
                        sliProgress.Value = currentPosition;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //axMediaPlayer.settings.volume += (e.Delta > 0) ? 0.1 : -0.1;
            //axMediaPlayer..Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }        

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void CommandBinding_Executed_2(object sender, ExecutedRoutedEventArgs e)
        {
            if (axMediaPlayer.settings.mute == false)
            {
                axMediaPlayer.settings.mute = true;
            }
            else
            {
                axMediaPlayer.settings.mute = false;
            }
        }

        private void Previous_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (axMediaPlayer != null) && (axMediaPlayer.currentMedia != null);            

        }

        private void Next_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (axMediaPlayer != null) && (axMediaPlayer.currentMedia != null);
        }

        private void Previous_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            axMediaPlayer.Ctlcontrols.previous();
            //mediaPlayerIsPlaying = false;
			counter = 0;
        }

        private void Next_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            axMediaPlayer.Ctlcontrols.next();
            //mediaPlayerIsPlaying = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            axMediaPlayer.Ctlcontrols.stop();
            axMediaPlayer.close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartPlaying();
        }

        private void FastBackward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void FastBackward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (axMediaPlayer.Ctlcontrols.currentPosition > 5)
                {
                    axMediaPlayer.Ctlcontrols.fastReverse();
                }
                else
                {
                    axMediaPlayer.Ctlcontrols.stop();
                    sliProgress.Value = 0;
                    lblProgressStatus.Text = "00:00:00s";
                    mediaPlayerIsPlaying = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

        }

        private void FastForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void FastForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (axMediaPlayer.currentMedia.duration - axMediaPlayer.Ctlcontrols.currentPosition > 5)
                {
                    axMediaPlayer.Ctlcontrols.fastForward();
                }
                else
                {
                    axMediaPlayer.Ctlcontrols.stop();
                    sliProgress.Value = 0;
                    lblProgressStatus.Text = "00:00:00s";
                    mediaPlayerIsPlaying = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void btnFullScreen(object sender, RoutedEventArgs e)
        {
            axMediaPlayer.fullScreen = true;
        }
    }
}
