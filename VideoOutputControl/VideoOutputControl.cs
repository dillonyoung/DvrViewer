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

        private void PictureBoxVideo_Click(object sender, EventArgs e)
        {
            VideoClicked?.Invoke(this, e);
        }

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
    }
}
