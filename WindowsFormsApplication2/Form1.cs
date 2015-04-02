using Inq.BPMN;
//using Org.Omg.BPMN20;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using WindowsFormsApplication2.Shapes;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        tDefinitions def = null;

        Dictionary<string, BitmapShape> shapes;

        List<BaseGraphicShape> gshape;


        string TestBPMN = @"E:\MyWork\BPM\BPMN\pool.bpmn.bpmn";
        string TestBPMN2 = @"C:\Users\kuznetsov\Documents\BPM\test\Order.bpmn";


        public Form1()
        {
            shapes = new Dictionary<string,BitmapShape>();
            gshape = new List<BaseGraphicShape>();

            InitializeComponent();
            pictureBox1.Focus();
            pictureBox1.Invalidate();
           ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var bpmn = new BPMNEngine();
            def = bpmn.ReadBPMN(TestBPMN2);
            loadTree();
        }


        public void loadTree() {

            TreeNode root = new TreeNode(def.name);
            root.Tag = def;
            foreach (var elem in def.rootElement) {

                TreeNode node = new TreeNode(elem.GetType().ToString());

                node.Tag = elem;


                if (elem.GetType() == typeof(tProcess)) {

                    var proc = (tProcess)elem;

                    TreeNode proc_node = new TreeNode(proc.name);
                    proc_node.Tag = proc;

                   

                    foreach (var flow_elem in proc.flowElement)
                    {
                        TreeNode flow_node = new TreeNode(flow_elem.GetType().ToString());
                        flow_node.Tag = flow_elem;
                        if (flow_elem.GetType() == (typeof(tTask)))
                        {
                            gshape.Add(new TaskShape((tTask)flow_elem, (BPMNShape)GetDiagramElementById(flow_elem.id), pictureBox1.CreateGraphics()));
                        }
                        else {
                            var shh = GetDiagramElementById(flow_elem.id);
                            if(shh!=null)
                                gshape.Add(new AnyShape(flow_elem, shh, pictureBox1.CreateGraphics()));
                        
                        }



                        proc_node.Nodes.Add(flow_node);
                    }



                    node.Nodes.Add(proc_node);       
                }


        
                root.Nodes.Add(node);
            
            }





            treeView1.Nodes.Add(root);

            pictureBox1.Refresh();
        }


        public DiagramElement GetDiagramElementById(string Id){


            foreach (var sh in def.BPMNDiagram) {
            
             foreach(var d_elem in sh.BPMNPlane.diagramElement){

                 if (d_elem.GetType() == typeof(BPMNShape)) {
                     if (((BPMNShape)d_elem).bpmnElement.Name == Id)
                     return d_elem;                 
                 }
                 if (d_elem.GetType() == typeof(BPMNEdge))
                 {
                     if (((BPMNEdge)d_elem).bpmnElement.Name == Id)
                         return d_elem;
                 }

                 if (d_elem.GetType() == typeof(BPMNPlane))
                 {
                     if (((BPMNPlane)d_elem).bpmnElement.Name == Id)
                         return d_elem;
                 }   
             }
            
            }

            return null;

        }


        private void DrowShape(Graphics graphics, BPMNShape shape)
        {


            var root = def.rootElement;
 
            

            tFlowElement flowElement = null;

            var rect =  new RectangleF((int)shape.Bounds.x, (int)shape.Bounds.y, (int)shape.Bounds.width, (int)shape.Bounds.height);
            var rect2 = new Rectangle((int)shape.Bounds.x, (int)shape.Bounds.y, (int)shape.Bounds.width, (int)shape.Bounds.height);

            rect.Offset(10, 10);
            rect.Inflate(-10, -10);

            List<tProcess> proc_list = new List<tProcess>();

            foreach (var item in root) {

                if (item.GetType() == typeof(tProcess))

                    proc_list.Add((tProcess)item);
            
            }


            foreach (var proc in proc_list)
            {

                foreach (var itemFlow in proc.flowElement)
                {

                    if (itemFlow.id == shape.bpmnElement.Name)
                    {
                        flowElement = itemFlow;
                        graphics.DrawString(flowElement.name, new System.Drawing.Font("Arial", 8), new SolidBrush(Color.Black), rect);
                    }

                }



                Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0), 2);



                if (flowElement != null)
                {

                    if (!shapes.ContainsKey(flowElement.id))
                    {

                        if (flowElement.GetType()==(typeof(tTask)))
                        {
                            gshape.Add(new TaskShape((tTask)flowElement, shape, graphics));
                        }
                        else
                        {

                            var bmap = new BitmapShape();
                            bmap.element = flowElement;
                            bmap.rect = rect2;
                            bmap.pen = pen;
                            shapes.Add(flowElement.id, bmap);
                        }

           

                    }


                    if (flowElement.GetType().IsSubclassOf(typeof(tEvent)))
                    {
                        graphics.DrawEllipse(shapes[flowElement.id].pen, rect2);
                    }
                    else
                        if (flowElement.GetType().IsSubclassOf(typeof(tGateway)))
                        {

                            //  graphics.DrawRectangle(pen, rect2);
                            RotateRectangle(graphics, pen, rect2, 45);
                        }
                        else
                        {
                          //  graphics.DrawRectangle(pen, rect2);
                        }


                }
                else
                {

                    graphics.DrawRectangle(pen, rect2);
                }
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



        void Render(Graphics graph)
        {

           // var graph = pictureBox1.CreateGraphics();
            graph.SmoothingMode = SmoothingMode.HighQuality;
            graph.Clear(Color.White);

           // graph.RenderingOrigin = new System.Drawing.Point(0, 0);
           // graph.PageScale = 1.1f;

        /*    if (def!=null)
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
            }*/

            foreach(var draw_gshape in  gshape){

                draw_gshape.Draw(graph);
            
            }

      
        
        }


        private void pictureBox1_Paint_1(object sender, PaintEventArgs e)
        {
           // base.OnPaint(e);
            Render(e.Graphics);
       
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
             
                    pictureBox1.Invalidate();
                
              
                    break;
                }

            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Node.Tag;
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            def.SaveToFile("test.bpmn");
        }

    }


    public class BitmapShape {

        public Pen pen;
        public Rectangle rect;
        public tFlowElement element;

    }
}
