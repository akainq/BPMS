using Inq.BPMN;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.Shapes
{
    public class TaskShape: BaseGraphicShape
    {
        readonly Rectangle _rect;

        public  Rectangle Rect
        {
            get {

                var shape = (Shape)DiagElement;

                return new Rectangle((int)shape.Bounds.x, (int)shape.Bounds.y, (int)shape.Bounds.width, (int)shape.Bounds.height);; 
            }

            set { }
        }

        public TaskShape(tTask taskElement, BPMNShape shape, Graphics surface)
            : base(taskElement, shape, surface)
        {

            Rect = new Rectangle((int)shape.Bounds.x, (int)shape.Bounds.y, (int)shape.Bounds.width, (int)shape.Bounds.height);
        }

        override public void Draw(Graphics graph){
            base.Draw(graph);



        }


    }
}
