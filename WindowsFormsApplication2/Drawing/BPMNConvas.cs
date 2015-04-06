using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2.Drawing;

namespace WindowsFormsApplication2
{
    public enum PageOriantation{
        
       [EnumDescription("Альбомная")]
        Landscape,

       [EnumDescription("Книжная")]
        Portrait
    }

    public partial class BPMNConvas : UserControl
    {


        private PageOriantation _convasOrientation = PageOriantation.Portrait;

        public PageOriantation ConvasOrientation
        {
            get { return _convasOrientation; }
            set { _convasOrientation = value; }
        }

        public delegate void Renderer(PaintEventArgs e);

        public Renderer OnGraphicRender;

        public BPMNConvas()
        {
            InitializeComponent();     
            SetStyle(ControlStyles.UserPaint,true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
 
         


           // toolStripComboBox1.Items.Clear();
                      
            var items = EnumHelper.ToList(typeof(PageOriantation));

        //    toolStripComboBox1.ComboBox.DataSource = items;
           // toolStripComboBox1.ComboBox.DisplayMember = "Key";
          //  toolStripComboBox1.ComboBox.ValueMember = "Value";

        }
    


        public void InvalidateConvas() {

            this.Invalidate();
        
        }

        public void InvalidateConvasRect(Rectangle rect)
        {
            this.Invalidate(rect);        

        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
         
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;

            var v1 = (int)(8.27 * e.Graphics.DpiX);
            var v2 = (int)(11.69 * e.Graphics.DpiY);

            this.Width = _convasOrientation == PageOriantation.Portrait ? v1 : v2;
            this.Height = _convasOrientation == PageOriantation.Portrait ? v2 : v1;



            DrawGrid(e);
        }

        protected void DrawGrid(PaintEventArgs e)
        {

          //  base.OnPaintBackground(e);

            var g = e.Graphics;

            Rectangle workRect = new Rectangle();

            g.FillRectangle(new SolidBrush(Color.FromArgb(150, 150, 180)), e.ClipRectangle);
       



            int offset = 20;
           // workRect.Offset(20, 20);

            var convasRect = this.DisplayRectangle;

            workRect.X = convasRect.X + offset;
            workRect.Y = convasRect.Y + offset;

            workRect.Width = convasRect.Width - 40;
            workRect.Height = convasRect.Height - 40;

            var shadow_rect = new Rectangle(new Point(workRect.X + 5, workRect.Y + 5), workRect.Size);

            
            g.FillRectangle(new SolidBrush(Color.FromArgb(67, 74, 94)), shadow_rect);

            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255)), workRect);

            g.DrawRectangle(new Pen(Color.FromArgb(0, 0, 0)), workRect);

            int grid_vertical_lines = workRect.Width / 10;
            int grid_horizontal_lines = workRect.Height / 10;

            var GrayPen = new Pen(Color.Gray, 1);
            GrayPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            for (int i = 0; i <= grid_vertical_lines; i++)
            {
                var px = new Point(i *10, 0);
                    px.Offset(offset, offset);
                var py = new Point(i * 10, workRect.Bottom-offset);
                    py.Offset(offset, offset);

                g.DrawLine(GrayPen, px, py);

            }

            for (int i = 0; i <= grid_horizontal_lines; i++)
            {
                var px = new Point(0, i * 10);
                px.Offset(offset, offset);
                var py = new Point(workRect.Right - offset, i * 10);
                py.Offset(offset, offset);

                g.DrawLine(GrayPen,px, py);

            }

            g.Clip = new Region(workRect);

            if (OnGraphicRender != null) OnGraphicRender(e);
        
        }


    }

    
}
