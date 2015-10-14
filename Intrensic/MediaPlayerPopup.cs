using CodeITBL;
using PodcastUtilities.PortableDevices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intrensic
{
    public partial class frmMediaPlayerPopup : Form
    {
        public AxWMPLib.AxWindowsMediaPlayer MediaPlayer {
            get { return axWindowsMediaPlayer1; }
        }
        public frmMediaPlayerPopup()
        {
            InitializeComponent();

            axWindowsMediaPlayer1.enableContextMenu = false;
            axWindowsMediaPlayer1.fullScreen = false;
            axWindowsMediaPlayer1.windowlessVideo = true;            
            axWindowsMediaPlayer1.network.bufferingTime = 30;          
            
        }
               
        public void LoadVideos(List<string> videos)
        {
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

        private void pbClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmMediaPlayerPopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsBuffering || axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            }

            axWindowsMediaPlayer1.close();
        }

        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "(mp3,wav,mp4,mov,wmv,mpg)|*.mp3;*.wav;*.mp4;*.mov;*.wmv;*.mpg|all files|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                axWindowsMediaPlayer1.URL = openFileDialog1.FileName;
        }
    }
}
