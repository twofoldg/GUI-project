using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    public class EllipseShape : Shape
    {
        public EllipseShape(RectangleF rect) : base(rect)
        {
        }

        public EllipseShape(EllipseShape ellipse) : base(ellipse)
        {
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);
            grfx.DrawEllipse(Pens.Black, Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
        }
    }

}
