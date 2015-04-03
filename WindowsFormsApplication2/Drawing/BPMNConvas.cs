using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class BPMNConvas : UserControl
    {


        public delegate void Renderer(PaintEventArgs e);

        public Renderer OnGraphicRender;

        public BPMNConvas()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
       
        }

 


        protected void DrawGrid(PaintEventArgs e)
        {

          //  base.OnPaintBackground(e);

            var g = e.Graphics;

            Rectangle workRect = new Rectangle();

        //    g.FillRectangle( new SolidBrush(Color.FromArgb(141,150,179)),e.ClipRectangle);
       



            int offset = 20;
           // workRect.Offset(20, 20);

            var convasRect = panel1.DisplayRectangle;

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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            panel1.Width = (int)(8.27 * e.Graphics.DpiX);
            panel1.Height = (int)(11.69 * e.Graphics.DpiY);



            DrawGrid(e);
        }

        private void panel1_Layout(object sender, LayoutEventArgs e)
        {
            
          
        }
    }

    
}
