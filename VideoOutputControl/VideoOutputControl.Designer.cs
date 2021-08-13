namespace VideoOutputControl
{
    partial class VideoOutputControl
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
            this.PictureBoxVideo = new System.Windows.Forms.PictureBox();
            this.PanelBorderLeft = new System.Windows.Forms.Panel();
            this.PanelBorderTop = new System.Windows.Forms.Panel();
            this.PanelBorderRight = new System.Windows.Forms.Panel();
            this.PanelBorderBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBoxVideo
            // 
            this.PictureBoxVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PictureBoxVideo.Location = new System.Drawing.Point(0, 0);
            this.PictureBoxVideo.Name = "PictureBoxVideo";
            this.PictureBoxVideo.Size = new System.Drawing.Size(200, 200);
            this.PictureBoxVideo.TabIndex = 0;
            this.PictureBoxVideo.TabStop = false;
            this.PictureBoxVideo.Click += new System.EventHandler(this.PictureBoxVideo_Click);
            this.PictureBoxVideo.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBoxVideo_Paint);
            // 
            // PanelBorderLeft
            // 
            this.PanelBorderLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.PanelBorderLeft.BackColor = System.Drawing.Color.Yellow;
            this.PanelBorderLeft.Location = new System.Drawing.Point(0, 0);
            this.PanelBorderLeft.Name = "PanelBorderLeft";
            this.PanelBorderLeft.Size = new System.Drawing.Size(10, 200);
            this.PanelBorderLeft.TabIndex = 1;
            // 
            // PanelBorderTop
            // 
            this.PanelBorderTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelBorderTop.BackColor = System.Drawing.Color.Yellow;
            this.PanelBorderTop.Location = new System.Drawing.Point(0, 0);
            this.PanelBorderTop.Name = "PanelBorderTop";
            this.PanelBorderTop.Size = new System.Drawing.Size(200, 10);
            this.PanelBorderTop.TabIndex = 2;
            // 
            // PanelBorderRight
            // 
            this.PanelBorderRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelBorderRight.BackColor = System.Drawing.Color.Yellow;
            this.PanelBorderRight.Location = new System.Drawing.Point(190, 0);
            this.PanelBorderRight.Name = "PanelBorderRight";
            this.PanelBorderRight.Size = new System.Drawing.Size(10, 200);
            this.PanelBorderRight.TabIndex = 3;
            // 
            // PanelBorderBottom
            // 
            this.PanelBorderBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelBorderBottom.BackColor = System.Drawing.Color.Yellow;
            this.PanelBorderBottom.Location = new System.Drawing.Point(0, 190);
            this.PanelBorderBottom.Name = "PanelBorderBottom";
            this.PanelBorderBottom.Size = new System.Drawing.Size(200, 10);
            this.PanelBorderBottom.TabIndex = 4;
            // 
            // VideoOutputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PanelBorderBottom);
            this.Controls.Add(this.PanelBorderRight);
            this.Controls.Add(this.PanelBorderTop);
            this.Controls.Add(this.PanelBorderLeft);
            this.Controls.Add(this.PictureBoxVideo);
            this.Name = "VideoOutputControl";
            this.Size = new System.Drawing.Size(200, 200);
            this.Resize += new System.EventHandler(this.VideoOutputControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxVideo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBoxVideo;
        private System.Windows.Forms.Panel PanelBorderLeft;
        private System.Windows.Forms.Panel PanelBorderTop;
        private System.Windows.Forms.Panel PanelBorderRight;
        private System.Windows.Forms.Panel PanelBorderBottom;
    }
}
