using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoOutputControl
{
    public partial class VideoOutputControl: UserControl
    {
        private int _highlightBorderWidth = 2;
        private Color _highlightBorderColor = Color.Yellow;
        private bool _highlightBorderVisible = false;
        private Color _backgroundColor = Color.Black;

        public IntPtr VideoHandle => PictureBoxVideo.Handle;

        public VideoOutputControl()
        {
            InitializeComponent();
        }

        private void VideoOutputControl_Resize(object sender, EventArgs e)
        {
            PictureBoxVideo.Width = Width;
            PictureBoxVideo.Height = Height;

            UpdateBorder();
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Event which is raised when the video area is clicked")]
        public event EventHandler VideoClicked;

        [Browsable(true)]
        [Category("Action")]
        [Description("Event which is raised when the video area is double clicked")]
        public event EventHandler VideoDoubleClicked;

        [Browsable(true)]
        [Category("Action")]
        [Description("Event which is raised when the video area is right clicked")]
        public event EventHandler VideoRightClicked;

        [Browsable(true)]
        [Category("Action")]
        [Description("Event which is raised when the video area is double right clicked")]
        public event EventHandler VideoDoubleRightClicked;

        [Category("Appearance")]
        [Description("The border width of the control highlight")]
        public int HighlightBorderWidth
        {
            get => _highlightBorderWidth;
            set
            {
                _highlightBorderWidth = value;
                UpdateBorder();
            }
        }

        [Category("Appearance")]
        [Description("The color of the border for control highlight")]
        public Color HighlightBorderColor
        {
            get => _highlightBorderColor;
            set
            {
                _highlightBorderColor = value;
                UpdateBorder();
            }
        }

        [Category("Appearance")]
        [Description("Identifies whether the highlight border is visible")]
        public bool HighlightBorderVisible
        {
            get => _highlightBorderVisible;
            set
            {
                _highlightBorderVisible = value;
                UpdateBorder();
            }
        }

        [Category("Appearance")]
        [Description("The background color of the video area")]
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                PictureBoxVideo.BackColor = _backgroundColor;
            }
        }

        private void UpdateBorder()
        {
            PanelBorderLeft.Visible = _highlightBorderVisible;
            PanelBorderTop.Visible = _highlightBorderVisible;
            PanelBorderRight.Visible = _highlightBorderVisible;
            PanelBorderBottom.Visible = _highlightBorderVisible;

            PanelBorderLeft.Width = _highlightBorderWidth;
            PanelBorderTop.Height = _highlightBorderWidth;
            PanelBorderRight.Width = _highlightBorderWidth;
            PanelBorderRight.Left = Width - PanelBorderRight.Width;
            PanelBorderBottom.Height = _highlightBorderWidth;
            PanelBorderBottom.Top = Height - PanelBorderBottom.Height;

            PanelBorderLeft.BackColor = _highlightBorderColor;
            PanelBorderTop.BackColor = _highlightBorderColor;
            PanelBorderRight.BackColor = _highlightBorderColor;
            PanelBorderBottom.BackColor = _highlightBorderColor;
        }

        private void PictureBoxVideo_Paint(object sender, PaintEventArgs e)
        {
            PictureBoxVideo.BackColor = _backgroundColor;
        }

        private void PictureBoxVideo_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    VideoClicked?.Invoke(this, new EventArgs());
                    break;
                case MouseButtons.Right:
                    VideoRightClicked?.Invoke(this, new EventArgs());
                    break;
            }
        }

        private void PictureBoxVideo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    VideoDoubleClicked?.Invoke(this, new EventArgs());
                    break;
                case MouseButtons.Right:
                    VideoDoubleRightClicked?.Invoke(this, new EventArgs());
                    break;
            }
        }

        public void ClearOutput()
        {
            PictureBoxVideo.Invalidate();
        }
    }
}
