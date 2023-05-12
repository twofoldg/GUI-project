using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    public class CircleShape : Shape
    {
        public CircleShape(RectangleF rect) : base(rect)
        {
        }

        public CircleShape(CircleShape circle) : base(circle)
        {
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);
            grfx.DrawEllipse(Pens.Black, Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

            DrawSelection(grfx);
        }
    }

}
