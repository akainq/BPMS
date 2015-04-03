using Inq.BPMN;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.Shapes
{
    public class AnyShape:BaseGraphicShape
    {
        Rectangle _rect;

        public Rectangle Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }

        public AnyShape(tFlowElement taskElement, DiagramElement shape, Graphics surface)
            : base(taskElement, shape, surface)
        {

           // Rect = new Rectangle((int)tmp_shape.Bounds.x, (int)tmp_shape.Bounds.y, (int)tmp_shape.Bounds.width, (int)tmp_shape.Bounds.height);
        }

        override public void Draw(Graphics graph){
            base.Draw(graph);



        }
    }
}
