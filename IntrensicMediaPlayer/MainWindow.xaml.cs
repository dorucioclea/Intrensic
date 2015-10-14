using CodeITBL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IntrensicMediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        private Queue<Uri> playList = new Queue<Uri>();
        private List<Uri> playListUrls = new List<Uri>();
        private Uri currentVideo = null;
        private string sourcePath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private Int32 userId = -1;

        private List<FileFromDB> GoProVideos = new List<FileFromDB>();

        public MainWindow(List<string> videos, bool fromStream = false, List<FileFromDB> goProVideos = null)
        {
            InitializeComponent();

            if (videos.Count > 0)
            {
                foreach (var item in videos)
                {
                    playList.Enqueue(new Uri(item, UriKind.RelativeOrAbsolute));
                    playListUrls.Add(new Uri(item, UriKind.RelativeOrAbsolute));
                }
            }

            if (goProVideos != null)
            {
                GoProVideos = goProVideos;
            }

            //PlayAudioPlaylist();

            PlayNext();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void PlayAudioPlaylist()
        {
            if (playList.Count > 0)
            {
                mePlayer.Source = playList.Dequeue();
                mePlayer.LoadedBehavior = MediaState.Manual;
                mePlayer.Play();
                mediaPlayerIsPlaying = true;
            }
        }

        

        private void PlayNext()
        {
            if(playListUrls.Count() > 0)
            {
                if (currentVideo != null)
                {
                    if (playListUrls.Contains(currentVideo))
                    {
                        int index = playListUrls.IndexOf(currentVideo);
                        if (index < playListUrls.Count() - 1)
                        {
                            currentVideo = playListUrls[index + 1];                            
                        }
                    }
                }
                else
                {
                    currentVideo = playListUrls.FirstOrDefault();                    
                }

                //CodeITDL.File file = PlayerVideoInfo.GetVideoInfoFromEntity(currentVideo, userId);
                FileFromDB file = GoProVideos.FirstOrDefault(x => currentVideo.LocalPath.Contains(x.OriginalFileName));
                
                if (file != null)
                {
                    Duration.Text = file.DurationFormat;
                    Resolution.Text = file.Resolution;
                    Username.Text = file.UserName;
                    CreatedOn.Text = file.CreatedOn.ToString("MM/dd/yyyy hh:mm:ss tt");
                }

                mePlayer.Source = currentVideo;
                mePlayer.LoadedBehavior = MediaState.Manual;
                mePlayer.Play();
                mediaPlayerIsPlaying = true;
            }
        }

        private void PlayPrevious()
        {
            if (playListUrls.Count() > 0)
            {
                if (currentVideo != null)
                {
                    if (playListUrls.Contains(currentVideo))
                    {
                        int index = playListUrls.IndexOf(currentVideo);
                        if (index > 0)
                        {
                            currentVideo = playListUrls[index - 1];
                        }
                    }
                }
                else
                {
                    currentVideo = playListUrls.LastOrDefault();
                }
                mePlayer.Source = currentVideo;
                mePlayer.LoadedBehavior = MediaState.Manual;
                mePlayer.Play();
                mediaPlayerIsPlaying = true;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!FastBackwardEnabled)
            {
                if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
                {
                    sliProgress.Minimum = 0;
                    sliProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
                    sliProgress.Value = mePlayer.Position.TotalSeconds;
                }
            }
            else
            {
                if (mePlayer.Position.TotalSeconds - 3 > 0)
                {
                    mePlayer.Position = new TimeSpan(0, 0, Convert.ToInt32(mePlayer.Position.TotalSeconds - 3));
                    sliProgress.Value = mePlayer.Position.TotalSeconds;
                }
                else
                {
                    mePlayer.Stop();
                }
            }
            
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg;*.MP4)|*.mp3;*.mpg;*.mpeg;*.MP4|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                mePlayer.Source = new Uri(openFileDialog.FileName);
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (mePlayer != null) && (mePlayer.Source != null);
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.SpeedRatio = 1.0;
            mePlayer.Play();
            mediaPlayerIsPlaying = true;
            FastBackwardEnabled = false;
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FastBackwardEnabled = false;
            mePlayer.SpeedRatio = 1.0;
            mePlayer.Pause();
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Previous_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(currentVideo != null)
            {
                int index = playListUrls.IndexOf(currentVideo);
                if (index >  0)
                {
                    e.CanExecute = true;
                }
                else
                {
                    e.CanExecute = false;
                }
            }
            else
            {
                e.CanExecute = false;
            }
            
        }

        private void Next_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (currentVideo != null)
            {
                //PlayAudioPlaylist();
                int index = playListUrls.IndexOf(currentVideo);
                if (index < playListUrls.Count() - 1)
                {
                    e.CanExecute = true;
                }
                else
                {
                    e.CanExecute = false;
                }
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.SpeedRatio = 1;
            mePlayer.Stop();
            mediaPlayerIsPlaying = false;
        }

        private void Previous_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PlayPrevious();
            mediaPlayerIsPlaying = false;
        }

        private void Next_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PlayNext();
            mediaPlayerIsPlaying = true;
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mePlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }
        
        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mePlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        private void mePlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            //PlayAudioPlaylist();
            PlayNext();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void CommandBinding_Executed_2(object sender, ExecutedRoutedEventArgs e)
        {
            if (mePlayer.IsMuted == false)
            {
                mePlayer.IsMuted = true;
            }
            else
            {
                mePlayer.IsMuted = false;
            }
        }

        private void mePlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show(e.ErrorException.Message);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mePlayer.Stop();            
        }

        private void FastForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void FastForward(object sender, ExecutedRoutedEventArgs e)
        {
            FastBackwardEnabled = false;
            mePlayer.SpeedRatio = 3.0;
        }

        private void FastBackward(object sender, ExecutedRoutedEventArgs e)
        {
            DoFastBackward();
        }

        private bool FastBackwardEnabled = false;

        private void DoFastBackward()
        {
            FastBackwardEnabled = true;
        }

        private void FastBackward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
