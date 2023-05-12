using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;

namespace Draw.src.Model
{
    public class LineShape : Shape
    {
        public PointF Start { get; set; }
        public PointF End { get; set; }

        public LineShape(PointF start, PointF end) : base(new RectangleF())
        {
            this.Start = start;
            this.End = end;

            // Update the Rectangle based on the start and end points
            float x = Math.Min(start.X, end.X);
            float y = Math.Min(start.Y, end.Y);
            float width = Math.Abs(end.X - start.X);
            float height = Math.Abs(end.Y - start.Y);
            this.Rectangle = new RectangleF(x, y, width, height);
        }

        public LineShape(LineShape line) : base(line)
        {
            this.Start = line.Start;
            this.End = line.End;
        }

        public override bool Contains(PointF point)
        {
            // Check if the distance between the point and the line is less than a threshold (e.g., 3f)
            float distance = DistanceFromPointToLine(point, Rectangle.Location, new PointF(Rectangle.Right, Rectangle.Bottom));
            return distance < 3f;
        }

        private float DistanceFromPointToLine(PointF point, PointF lineStart, PointF lineEnd)
        {
            float lineLength = DistanceBetweenPoints(lineStart, lineEnd);
            float determinant = ((point.X - lineStart.X) * (lineEnd.Y - lineStart.Y)) - ((point.Y - lineStart.Y) * (lineEnd.X - lineStart.X));
            return (float)(Math.Abs(determinant) / lineLength);
        }

        private float DistanceBetweenPoints(PointF point1, PointF point2)
        {
            return (float)Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }


        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);
            grfx.DrawLine(Pens.Black, Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);

            DrawSelection(grfx);
        }
    }

}
