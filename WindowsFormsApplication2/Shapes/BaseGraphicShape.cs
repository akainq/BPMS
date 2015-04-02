using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inq.BPMN;
using System.Drawing;

namespace WindowsFormsApplication2.Shapes
{
    public abstract class BaseGraphicShape: IGraphicShape
    {
        tFlowElement _elment;
        DiagramElement _diagElement;
        Graphics _context;

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
                graph.DrawRectangle(new Pen(Color.Black), rect);
            }

            if (_diagElement.GetType() == typeof(BPMNEdge))
            {
                var tmp_shape = (BPMNEdge)_diagElement;
               // Rectangle rect = new Rectangle((int)tmp_shape.Bounds.x, (int)tmp_shape.Bounds.y, (int)tmp_shape.Bounds.width, (int)tmp_shape.Bounds.height);

                Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
                List<PointF> points = new List<PointF>();

                foreach (var wpoint in tmp_shape.waypoint)
                {
                    points.Add(new PointF((float)wpoint.x, (float)wpoint.y));
                }
                graph.DrawLines(pen, points.ToArray());

             /*   if (tmp_shape.BPMNLabel != null)
                {
                    
                    Rectangle rectLabel = new Rectangle((int)tmp_shape.BPMNLabel.Bounds.x, (int)tmp_shape.BPMNLabel.Bounds.y, (int)tmp_shape.BPMNLabel.Bounds.width, (int)tmp_shape.BPMNLabel.Bounds.height);
                    graph.DrawString(this._elment.name, new System.Drawing.Font("Arial", 8), new SolidBrush(Color.Black),rectLabel);
                }*/

              
            }


 

        }
}
}
