﻿using Inq.BPMN;
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
        Rectangle _rect;

        public Rectangle Rect
        {
            get { return _rect; }
            set { _rect = value; }
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
