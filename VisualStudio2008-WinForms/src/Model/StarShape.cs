using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    public class StarShape : Shape
    {
        public int OuterRadius { get; set; }
        public int InnerRadius { get; set; }
        public int Points { get; set; }

        public StarShape(PointF center, int outerRadius, int innerRadius, int points) : base(new RectangleF())
        {
            this.OuterRadius = outerRadius;
            this.InnerRadius = innerRadius;
            this.Points = points;

            // Update the Rectangle based on the center and outerRadius
            this.Rectangle = new RectangleF(center.X - outerRadius, center.Y - outerRadius, 2 * outerRadius, 2 * outerRadius);
        }

        public StarShape(StarShape star) : base(star)
        {
            this.OuterRadius = star.OuterRadius;
            this.InnerRadius = star.InnerRadius;
            this.Points = star.Points;
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            int numberOfPoints = 5;
            double rotationAngle = Math.PI / numberOfPoints;
            PointF[] starPoints = new PointF[numberOfPoints * 2];

            double radius1 = Math.Min(Rectangle.Width, Rectangle.Height) / 2;
            double radius2 = radius1 * 0.5;
            double centerX = Rectangle.X + Rectangle.Width / 2;
            double centerY = Rectangle.Y + Rectangle.Height / 2;

            for (int i = 0; i < numberOfPoints * 2; i++)
            {
                double angle = (2 * Math.PI * i) / (numberOfPoints * 2) - Math.PI / 2;
                double radius = i % 2 == 0 ? radius1 : radius2;
                starPoints[i] = new PointF((float)(centerX + radius * Math.Cos(angle)), (float)(centerY + radius * Math.Sin(angle)));
            }

            grfx.DrawPolygon(Pens.Black, starPoints);
            DrawSelection(grfx);
        }

    }

}
