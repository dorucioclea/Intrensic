using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace CodeITBL
{
    public class VideoInfo
    {
        public string FilePath { get; set; }
        public string Format { get; set; }
        public string Resolution { get; set; }
        public double Duration { get; set; }
        public string DurationFormat { get; set; }

        public VideoInfo(string filepath, double width = 0, double heigth = 0, double duration = 0)
        {
            FilePath = filepath;
            Duration = duration;

            if (width != 0 && heigth != 0)
            {
                Resolution = String.Format("{0}x{1}", width, heigth);
            }

            if (duration != 0)
            {
                TimeSpan span = TimeSpan.FromSeconds(duration);
                string hours = (span.Hours > 10) ? span.Hours.ToString() : "0" + span.Hours.ToString();
                string minutes = (span.Minutes > 10) ? span.Minutes.ToString() : "0" + span.Minutes.ToString();
                string seconds = (span.Seconds > 10) ? span.Seconds.ToString() : "0" + span.Seconds.ToString();
                DurationFormat = String.Format("{0}:{1}:{1}", hours, minutes, seconds);
            }
        }

        public static VideoInfo GetVideoInfo(string filePath)
        {
            int videoWidth = 0;
            int videoHeight = 0;

            try
            {
                var player = new WindowsMediaPlayer();
                IWMPMedia mediaInfo = player.newMedia(filePath);

                ShellFile shellFile = ShellFile.FromFilePath(filePath);

                if (shellFile != null)
                {
                    if (shellFile.Properties.System.Video.FrameWidth.Value != null)
                    {
                        videoWidth = (int)shellFile.Properties.System.Video.FrameWidth.Value;
                    }
					else
					{
						videoWidth = 0;
					}

                    if (shellFile.Properties.System.Video.FrameHeight.Value != null)
                    {
                        videoHeight = (int)shellFile.Properties.System.Video.FrameHeight.Value;
                    }
					else
					{
						videoHeight = 0;
					}
                }
                
                VideoInfo video = new VideoInfo(filePath, videoWidth, videoHeight, mediaInfo.duration);
                return video;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
