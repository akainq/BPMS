using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inq.BPMN;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WindowsFormsApplication2.Shapes
{
    public abstract class BaseGraphicShape: IGraphicShape
    {
        tFlowElement _elment;
        DiagramElement _diagElement;
        Graphics _context;
        bool _isSelected;
   

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public tFlowElement FlowElment
        {
            get { return _elment; }
            set { _elment = value; }
        }

        public DiagramElement DiagElement
        {
            get { return _diagElement; }
            set { _diagElement = value; }
        }

        private BaseGraphicShape() { }

        public BaseGraphicShape(tFlowElement element, DiagramElement shape, Graphics surface)
        {
        
           _elment= element;
           _diagElement = shape;
           _context = surface;
   

        }

        virtual public void Draw(Graphics graph)
        {
            if (_diagElement.GetType() == typeof(BPMNShape))
            {
                var tmp_shape = (BPMNShape)_diagElement;
                Rectangle rect = new Rectangle((int)tmp_shape.Bounds.x, (int)tmp_shape.Bounds.y, (int)tmp_shape.Bounds.width, (int)tmp_shape.Bounds.height);


                graph.FillRectangle(new SolidBrush(Color.White), rect);



                var pen = new Pen(Color.Black);

                if(IsSelected) {
                    pen.Width = 2;
                    pen.Color = Color.Red;
                }

                graph.DrawRectangle(pen, rect);
            
                
            }

            if (_diagElement.GetType() == typeof(BPMNEdge))
            {
                var tmp_shape = (BPMNEdge)_diagElement;
               // Rectangle rect = new Rectangle((int)tmp_shape.Bounds.x, (int)tmp_shape.Bounds.y, (int)tmp_shape.Bounds.width, (int)tmp_shape.Bounds.height);



                Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);

                if (IsSelected)
                {
                    pen.Width = 1;
                    pen.Color = Color.Red;
                }

                List<PointF> points = new List<PointF>();

                foreach (var wpoint in tmp_shape.waypoint)
                {
                    points.Add(new PointF((float)wpoint.x, (float)wpoint.y));
                }

                AdjustableArrowCap cap1 = new AdjustableArrowCap(3, 4);
                cap1.WidthScale = 3;
                cap1.BaseCap = LineCap.Square;
                cap1.Height = 4;
                pen.CustomEndCap = cap1;
                //pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
              

                graph.DrawLines(pen, points.ToArray());

             /*   if (tmp_shape.BPMNLabel != null)
                {
                    
                    Rectangle rectLabel = new Rectangle((int)tmp_shape.BPMNLabel.Bounds.x, (int)tmp_shape.BPMNLabel.Bounds.y, (int)tmp_shape.BPMNLabel.Bounds.width, (int)tmp_shape.BPMNLabel.Bounds.height);
                    graph.DrawString(this._elment.name, new System.Drawing.Font("Arial", 8), new SolidBrush(Color.Black),rectLabel);
                }*/

              
            }


 

        }




        public void DrawRoundedRectangle(Graphics gfx, Rectangle Bounds, int CornerRadius, Pen DrawPen, Color FillColor)
        {
            int strokeOffset = Convert.ToInt32(Math.Ceiling(DrawPen.Width));
            Bounds = Rectangle.Inflate(Bounds, -strokeOffset, -strokeOffset);

            DrawPen.EndCap = DrawPen.StartCap = LineCap.Round;

            GraphicsPath gfxPath = new GraphicsPath();
            gfxPath.AddArc(Bounds.X, Bounds.Y, CornerRadius, CornerRadius, 180, 90);
            gfxPath.AddArc(Bounds.X + Bounds.Width - CornerRadius, Bounds.Y, CornerRadius, CornerRadius, 270, 90);
            gfxPath.AddArc(Bounds.X + Bounds.Width - CornerRadius, Bounds.Y + Bounds.Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
            gfxPath.AddArc(Bounds.X, Bounds.Y + Bounds.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
            gfxPath.CloseAllFigures();

            gfx.FillPath(new SolidBrush(FillColor), gfxPath);
            gfx.DrawPath(DrawPen, gfxPath);
        }
}




}
