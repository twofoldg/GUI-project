using Draw.src.util;
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
            float distance = DistanceFromPointToLine(point, Start, End);
            bool isOnLine = distance < 3f;

            // Check if the point is within a threshold distance of either handle
            float handleRadius = 5f;
            RectangleF topLeft = new RectangleF(Start.X - handleRadius / 2, Start.Y - handleRadius / 2, handleRadius, handleRadius);
            RectangleF topRight = new RectangleF(End.X - handleRadius / 2, End.Y - handleRadius / 2, handleRadius, handleRadius);

            bool isOnHandle = topLeft.Contains(point) || topRight.Contains(point);

            return isOnLine || isOnHandle;
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
            grfx.DrawLine(Pens.Black, Start.X, Start.Y, End.X, End.Y);

            DrawSelection(grfx);
        }

        public override void ResizeShape(HandleTypeEnum handleType, PointF point)
        {
            if (IsSelected) {
                PointF newStart = Start;
                PointF newEnd = End;

                switch (handleType)
                {
                    case HandleTypeEnum.TopLeft:
                    case HandleTypeEnum.BottomLeft:
                        newStart = point;
                        break;
                    case HandleTypeEnum.TopRight:
                    case HandleTypeEnum.BottomRight:
                        newEnd = point;
                        break;
                }

                Start = newStart;
                End = newEnd;
            }
        }

        public override HandleTypeEnum GetClickedHandle(PointF point)
        {
            float handleRadius = 10f;
            RectangleF topLeft = new RectangleF(Start.X - handleRadius / 2, Start.Y - handleRadius / 2, handleRadius, handleRadius);
            RectangleF topRight = new RectangleF(End.X - handleRadius / 2, End.Y - handleRadius / 2, handleRadius, handleRadius);

            if (topLeft.Contains(point)) { var a = "Top Left"; return HandleTypeEnum.TopLeft; }
            if (topRight.Contains(point)) { var a = "Top Right"; return HandleTypeEnum.TopRight; }

            return HandleTypeEnum.None;
        }

        public override void DrawSelection(Graphics grfx)
        {
            if (IsSelected)
            {
                using (Pen pen = new Pen(Color.Black, 1.3f))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    grfx.DrawLine(pen, Start, End);
                }

                float handleRadius = 5f;
                Brush handleBrush = Brushes.Red;

                grfx.FillEllipse(handleBrush, Start.X - handleRadius / 2, Start.Y - handleRadius / 2, handleRadius, handleRadius);
                grfx.FillEllipse(handleBrush, End.X - handleRadius / 2, End.Y - handleRadius / 2, handleRadius, handleRadius);
            }
        }


    }
}
