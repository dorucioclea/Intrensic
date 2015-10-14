namespace Intrensic
{
    partial class VideoListItem
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblVideoId = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.pbVideo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.lblName.Location = new System.Drawing.Point(86, 4);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(72, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Jimmy Barden";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.lblDate.Location = new System.Drawing.Point(86, 17);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(107, 13);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "25.05.2015, 3:20 PM";
            // 
            // lblVideoId
            // 
            this.lblVideoId.AutoSize = true;
            this.lblVideoId.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.lblVideoId.Location = new System.Drawing.Point(86, 30);
            this.lblVideoId.Name = "lblVideoId";
            this.lblVideoId.Size = new System.Drawing.Size(55, 13);
            this.lblVideoId.TabIndex = 3;
            this.lblVideoId.Text = "#3243435";
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(199, 2);
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(150, 45);
            this.txtNote.TabIndex = 4;
            // 
            // pbIcon
            // 
            this.pbIcon.Image = global::Intrensic.Properties.Resources.icons_09;
            this.pbIcon.Location = new System.Drawing.Point(178, 29);
            this.pbIcon.Margin = new System.Windows.Forms.Padding(0);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(20, 20);
            this.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbIcon.TabIndex = 5;
            this.pbIcon.TabStop = false;
            // 
            // pbVideo
            // 
            this.pbVideo.Image = global::Intrensic.Properties.Resources.Video;
            this.pbVideo.Location = new System.Drawing.Point(0, 2);
            this.pbVideo.Name = "pbVideo";
            this.pbVideo.Size = new System.Drawing.Size(80, 46);
            this.pbVideo.TabIndex = 0;
            this.pbVideo.TabStop = false;
            // 
            // VideoListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(254)))));
            this.Controls.Add(this.pbIcon);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.lblVideoId);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.pbVideo);
            this.Name = "VideoListItem";
            this.Size = new System.Drawing.Size(350, 50);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbVideo;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblVideoId;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.PictureBox pbIcon;
    }
}
