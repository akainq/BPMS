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

        public AnyShape(tFlowElement taskElement, DiagramElement shape, Graphics surface)
            : base(taskElement, shape, surface)
        {
         
    
        }

        override public void Draw(Graphics graph){
            base.Draw(graph);



        }
    }
}
