using Inq.BPMN;
//using Org.Omg.BPMN20;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        tDefinitions def = null;

        Dictionary<string, BitmapShape> shapes;


        public Form1()
        {
            shapes = new Dictionary<string,BitmapShape>();
            InitializeComponent();
            pictureBox1.Focus();
            pictureBox1.Invalidate();
           ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Render();

        }


        private void DrowShape(Graphics graphics, BPMNShape shape)
        {


            var root = def.rootElement;
 
            

            tFlowElement flowElement = null;

            var rect =  new RectangleF((int)shape.Bounds.x, (int)shape.Bounds.y, (int)shape.Bounds.width, (int)shape.Bounds.height);
            var rect2 = new Rectangle((int)shape.Bounds.x, (int)shape.Bounds.y, (int)shape.Bounds.width, (int)shape.Bounds.height);

            rect.Offset(10, 10);
            rect.Inflate(-10, -10);

            foreach (var item in ((tProcess)root[0]).flowElement) {

                if (item.id == shape.bpmnElement.Name)
                {
                    flowElement = item;                   
                    graphics.DrawString(flowElement.name, new System.Drawing.Font("Arial", 7), new SolidBrush(Color.Black), rect);
                }
            
            }

    

            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0),2);



            if (flowElement!=null)
            {

                if (!shapes.ContainsKey(flowElement.id))
                {
                    var bmap = new BitmapShape();
                    bmap.element = flowElement;
                    bmap.rect = rect2;
                    bmap.pen = pen;
                    shapes.Add(flowElement.id, bmap);
                }
                     

                if (flowElement.GetType().IsSubclassOf(typeof(tEvent)))
                {
                    graphics.DrawEllipse(shapes[flowElement.id].pen, rect2);
                }
                else
                    if (flowElement.GetType().IsSubclassOf(typeof(tGateway)))
                    {
                  
                      //  graphics.DrawRectangle(pen, rect2);
                        RotateRectangle(graphics, shapes[flowElement.id].pen, rect2, 45);
                    }
                    else 
                {
                    graphics.DrawRectangle(shapes[flowElement.id].pen, rect2);
                }


            }
            else
            {

                graphics.DrawRectangle(pen, rect2);
            }
       
           // shape.Bounds.
         
        }
        public void RotateRectangle(Graphics g, Pen pen,Rectangle r, float angle)
        {
            using (Matrix m = new Matrix())
            {
                m.RotateAt(angle, new PointF(r.Left + (r.Width / 2),
                                          r.Top + (r.Height / 2)));
                g.Transform = m;
                g.DrawRectangle(pen, r);
                g.ResetTransform();
            }
        }
        private void DrowEdge(Graphics graphics, BPMNEdge shape)
        {

            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
            List<PointF> points = new List<PointF>();

            foreach(var wpoint in shape.waypoint){
            
              points.Add(new PointF((float)wpoint.x,(float)wpoint.y));

            }

            
                graphics.DrawLines(pen,points.ToArray());
          

        }



        void Render() {

            var graph = pictureBox1.CreateGraphics();
            graph.SmoothingMode = SmoothingMode.HighQuality;
    

            var bpmn = new BPMNEngine();
            def = bpmn.ReadBPMN("");
 

            foreach (var diag in def.BPMNDiagram)
            {

                foreach (DiagramElement elem in diag.BPMNPlane.diagramElement)
                {


                    //   var elem1 = elem;
                    if (elem.GetType() == typeof(BPMNShape))
                    {

                        DrowShape(graph, (BPMNShape)elem);
                    }

                    if (elem.GetType() == typeof(BPMNEdge))
                    {

                        DrowEdge(graph, (BPMNEdge)elem);
                    }

                }
            }
        
        }




        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
           // DrowConvas(e.Graphics);
            Render();

        }

        private void pictureBox1_Paint_1(object sender, PaintEventArgs e)
        {
            Render();
         
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (var shape in shapes)
            {

                if (shape.Value.rect.Contains(e.Location))
                {


                

               //     MessageBox.Show(shape.Value.element.id);
                    shape.Value.pen = new Pen(new SolidBrush(Color.Blue));
                    Render();
                    //pictureBox1.Invalidate();
                
              
                    break;
                }

            }
        }

    }


    public class BitmapShape {

        public Pen pen;
        public Rectangle rect;
        public tFlowElement element;

    }
}
