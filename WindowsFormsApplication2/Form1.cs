using Inq.BPMN;
//using Org.Omg.BPMN20;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var graph = pictureBox1.CreateGraphics();
            var bpmn = new BPMNEngine();
            tDefinitions def = bpmn.ReadBPMN("");

          //  BPMNDiagram diag = def.BPMNDiagram[0];
                 
                   foreach(var diag in def.BPMNDiagram){

               foreach(DiagramElement elem in diag.BPMNPlane.diagramElement){

                //   var elem1 = elem;
                   if(elem.GetType() == typeof(BPMNShape)){

                       DrowConvas(graph, (BPMNShape)elem); 
                   }

                   if (elem.GetType() == typeof(BPMNEdge))
                   {

                       DrowConvas(graph, (BPMNEdge)elem);
                   }
               
               }
            }
        }


        private void DrowConvas(Graphics graphics, BPMNShape shape)
        {

            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0),2);
            graphics.DrawRectangle(pen,(int)shape.Bounds.x, (int)shape.Bounds.y, (int)shape.Bounds.width, (int)shape.Bounds.height);

           // shape.Bounds.
         
        }

        private void DrowConvas(Graphics graphics, BPMNEdge shape)
        {

            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
            List<PointF> points = new List<PointF>();

            foreach(var wpoint in shape.waypoint){
            
              points.Add(new PointF((float)wpoint.x,(float)wpoint.y));

            }

            
          
                graphics.DrawLines(pen,points.ToArray());
          


            
            // shape.Bounds.

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
           // DrowConvas(e.Graphics);


        }

    }
}
