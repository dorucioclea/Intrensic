using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intrensic
{
    public partial class VideoListItem : UserControl
    {

        

        public enum Mode
        {
            Preview,
            Edit
        }

        private Mode itemMode = Mode.Preview;

        [Description("The Mode of the control")]
        [Category("Custom")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Bindable(true)]
        public Mode ItemMode
        {
            get
            {
                return itemMode;
            }

            set
            {
                itemMode = value;
                if (value == Mode.Preview)
                {
                    this.txtNote.Visible = false;
                    this.Width -= 150;
                    this.pbIcon.Visible = true;
                }
                else
                {
                    this.txtNote.Visible = true;
                    this.txtNote.Visible = true;
                    this.pbIcon.Visible = false;
                }
            }
        }

        private string note = string.Empty;

        [Description("The Note for the file")]
        [Category("Custom")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Bindable(true)]
        public string Note
        {
            get { return this.note; }
            set
            {
                this.note = value;
                txtNote.Text = note;
            }
        }

        private string userName = string.Empty;
        private string date = string.Empty;
        private string videoId = string.Empty;

        [Description("The Name of the user")]
        [Category("Custom")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Bindable(true)]
        public string UserName
        {
            get { return this.userName; }
            set
            {
                this.userName = value;
                lblName.Text = value;
            }
        }

        [Description("The Date of the video")]
        [Category("Custom")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Bindable(true)]
        public string Date
        {
            get { return this.date; }
            set
            {
                this.date = value;
                lblDate.Text = value;
            }
        }

        [Description("The Unique Id of the video")]
        [Category("Custom")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Bindable(true)]
        public string VideoId
        {
            get { return this.videoId; }
            set
            {
                this.videoId = value;
                lblVideoId.Text = value;
            }
        }
        public VideoListItem()
        {
            InitializeComponent();


        }
    }
}
