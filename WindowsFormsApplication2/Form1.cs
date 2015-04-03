using Inq.BPMN;
//using Org.Omg.BPMN20;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using WindowsFormsApplication2.Debug;
using WindowsFormsApplication2.Shapes;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {

        MemoryStream mem_s;

        tDefinitions def = null;

        Dictionary<string, BitmapShape> shapes;

        Dictionary<string, BaseGraphicShape> gshape;

        string slectedElementId = "";


        string TestBPMN = @"E:\MyWork\BPM\BPMN\pool.bpmn.bpmn";
        string TestBPMN2 = @"C:\Users\kuznetsov\Documents\BPM\test\Order.bpmn";


        public Form1()
        {
            shapes = new Dictionary<string,BitmapShape>();
            gshape = new Dictionary<string, BaseGraphicShape>();

            InitializeComponent();
            //pictureBox1.Focus();
           // pictureBox1.Invalidate();

         

        

              
           //;
        }


        void myRenderer(PaintEventArgs e) {


            Render(e.Graphics);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.Error.WriteLine("Test error");

            var bpmn = new BPMNEngine();
            def = bpmn.ReadBPMN(TestBPMN);
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
                            gshape.Add(flow_elem.id, new TaskShape((tTask)flow_elem, (BPMNShape)GetDiagramElementById(flow_elem.id), bpmnConvas1.CreateGraphics()));
                        }
                        else {
                            var shh = GetDiagramElementById(flow_elem.id);
                            if(shh!=null)
                                gshape.Add(flow_elem.id, new AnyShape(flow_elem, shh, bpmnConvas1.CreateGraphics()));
                               // gshape.Add(new AnyShape(flow_elem, shh, pictureBox1.CreateGraphics()));
                              
                        
                        }



                        proc_node.Nodes.Add(flow_node);
                    }



                    node.Nodes.Add(proc_node);       
                }


        
                root.Nodes.Add(node);
            
            }





            treeView1.Nodes.Add(root);

            bpmnConvas1.Refresh();
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
                            gshape.Add(flowElement.id, new TaskShape((tTask)flowElement, shape, graphics));
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
           // graph.Clear(Color.White);

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
          ///  graph.ScaleTransform(0.6f, 0.6f, MatrixOrder.Append);
          ///  

            //Matrix mx = new Matrix(1, 0, 0, 1, 1, 1);

           // graph.TranslateTransform(-50, -150);

            foreach(var draw_gshape in  gshape){

                draw_gshape.Value.Draw(graph);
            
            }
            //bpmnConvas1.Invalidate();
      
        
        }


        private void pictureBox1_Paint_1(object sender, PaintEventArgs e)
        {
           // base.OnPaint(e);
        //    Render(e.Graphics);
       
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
             
                  //  pictureBox1.Invalidate();
                
              
                    break;
                }

            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Node.Tag;

            if (e.Node.Tag.GetType().IsSubclassOf(typeof(tFlowElement)))
            {
                if (!string.IsNullOrEmpty(slectedElementId))
                    gshape[slectedElementId].IsSelected = false;
                gshape[((tFlowElement)e.Node.Tag).id].IsSelected = true;
                slectedElementId = ((tFlowElement)e.Node.Tag).id;

                bpmnConvas1.Invalidate();
            }


        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (def != null) 
            def.SaveToFile("test.bpmn");
        }

        private void bpmnConvas1_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);

        }

        private void bpmnConvas1_Resize(object sender, EventArgs e)
        {
            bpmnConvas1.Invalidate();
        }


        protected void DrawGrid(PaintEventArgs e)
        {

       //     base.OnPaintBackground(e);

            var g = e.Graphics;

            Rectangle workRect = new Rectangle();

            g.FillRectangle(new SolidBrush(Color.FromArgb(141, 150, 179)), e.ClipRectangle);


            int offset = 20;
            // workRect.Offset(20, 20);
            workRect.X = e.ClipRectangle.X + offset;
            workRect.Y = e.ClipRectangle.Y + offset;

            workRect.Width = e.ClipRectangle.Width - 40;
            workRect.Height = e.ClipRectangle.Height - 40;

            var shadow_rect = new Rectangle(new System.Drawing.Point(workRect.X + 5, workRect.Y + 5), workRect.Size);


            g.FillRectangle(new SolidBrush(Color.FromArgb(67, 74, 94)), shadow_rect);

            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255)), workRect);

            g.DrawRectangle(new Pen(Color.FromArgb(0, 0, 0)), workRect);

            int grid_vertical_lines = workRect.Width / 10;
            int grid_horizontal_lines = workRect.Height / 10;

            var GrayPen = new Pen(Color.Gray, 1);
            GrayPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            for (int i = 0; i <= grid_vertical_lines; i++)
            {
                var px = new System.Drawing.Point(i * 10, 0);
                px.Offset(offset, offset);
                var py = new System.Drawing.Point(i * 10, workRect.Bottom - offset);
                py.Offset(offset, offset);

                g.DrawLine(GrayPen, px, py);

            }

            for (int i = 0; i <= grid_horizontal_lines; i++)
            {
                var px = new System.Drawing.Point(0, i * 10);
                px.Offset(offset, offset);
                var py = new System.Drawing.Point(workRect.Right - offset, i * 10);
                py.Offset(offset, offset);

                g.DrawLine(GrayPen, px, py);

            }

        }

        private void bpmnConvas1_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (var shape in gshape)
            {
               // if (shape.Value.Rect.)     
           
                
                if (shape.Value.GetType() == (typeof(TaskShape)))
                {
                     var taskShape =  (TaskShape)shape.Value;

                     if (taskShape.Rect.Contains(e.Location)){

                            if (!string.IsNullOrEmpty(slectedElementId)){
                                gshape[slectedElementId].IsSelected = false;
                            }

                            gshape[((tFlowElement)shape.Value.FlowElment).id].IsSelected = true;
                            slectedElementId = ((tFlowElement)shape.Value.FlowElment).id;

                            bpmnConvas1.Invalidate();
                     }
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mem_s != null)
            mem_s.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        //    myListener.outMsgControl = richTextBox1;

       //     System.Diagnostics.
       //     TraceListener myTextListener = new
      // TraceListener(myFile);
            BPMNLogger logger = new BPMNLogger(richTextBox1);
           // BPMNLogger.

            Trace.Listeners.Add(logger);
            // System.Diagnostics.Debug.

            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic);

            Application.ThreadException += Application_ThreadException;

        }

        void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Trace.TraceError("UnhandledException:" + e.Exception.Message);

            if(e.Exception.InnerException != null)
                Trace.TraceError("InnerException:" + e.Exception.InnerException);
        

        }

  

    }


    public class BitmapShape {

        public Pen pen;
        public Rectangle rect;
        public tFlowElement element;

    }
}
