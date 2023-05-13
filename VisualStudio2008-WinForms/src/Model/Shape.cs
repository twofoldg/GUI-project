using Draw.src.util;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Draw
{
	/// <summary>
	/// Базовия клас на примитивите, който съдържа общите характеристики на примитивите.
	/// </summary>
	public abstract class Shape
	{
        #region Constructors

        public bool IsSelected { get; set; }

        public Shape()
		{
		}
		
		public Shape(RectangleF rect)
		{
			rectangle = rect;
		}
		
		public Shape(Shape shape)
		{
			this.Height = shape.Height;
			this.Width = shape.Width;
			this.Location = shape.Location;
			this.rectangle = shape.rectangle;
			
			this.FillColor =  shape.FillColor;
		}
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Обхващащ правоъгълник на елемента.
		/// </summary>
		private RectangleF rectangle;		
		public virtual RectangleF Rectangle {
			get { return rectangle; }
			set { rectangle = value; }
		}
		
		/// <summary>
		/// Широчина на елемента.
		/// </summary>
		public virtual float Width {
			get { return Rectangle.Width; }
			set { rectangle.Width = value; }
		}
		
		/// <summary>
		/// Височина на елемента.
		/// </summary>
		public virtual float Height {
			get { return Rectangle.Height; }
			set { rectangle.Height = value; }
		}
		
		/// <summary>
		/// Горен ляв ъгъл на елемента.
		/// </summary>
		public virtual PointF Location {
			get { return Rectangle.Location; }
			set { rectangle.Location = value; }
		}
		
		/// <summary>
		/// Цвят на елемента.
		/// </summary>
		private Color fillColor;		
		public virtual Color FillColor {
			get { return fillColor; }
			set { fillColor = value; }
		}

        #endregion


        /// <summary>
        /// Проверка дали точка point принадлежи на елемента.
        /// </summary>
        /// <param name="point">Точка</param>
        /// <returns>Връща true, ако точката принадлежи на елемента и
        /// false, ако не пренадлежи</returns>
        public virtual bool Contains(PointF point)
        {
            // Expanding the shape's rectangle by the width of the dashed rectangle border
            float borderOffset = 10f;
            RectangleF expandedRectangle = 
				new RectangleF(Rectangle.X - borderOffset, Rectangle.Y - borderOffset, Rectangle.Width + 2 * borderOffset, Rectangle.Height + 2 * borderOffset);

            return expandedRectangle.Contains(point);
        }


        /// <summary>
        /// Визуализира елемента.
        /// </summary>
        /// <param name="grfx">Къде да бъде визуализиран елемента.</param>
        public virtual void DrawSelf(Graphics grfx)
		{
			// shape.Rectangle.Inflate(shape.BorderWidth, shape.BorderWidth);
		}

        public virtual void DrawSelection(Graphics grfx)
        {
            if (IsSelected)
            {
                using (Pen pen = new Pen(Color.Black, 1.3f))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    grfx.DrawRectangle(pen, Rectangle.X - 10, Rectangle.Y - 10, Rectangle.Width + 20, Rectangle.Height + 20);
                }

                float handleRadius = 5f;
                Brush handleBrush = Brushes.Red;

                grfx.FillEllipse(handleBrush, Rectangle.Left - 10, Rectangle.Top - 10, handleRadius, handleRadius);
                grfx.FillEllipse(handleBrush, Rectangle.Right + 5, Rectangle.Top - 10, handleRadius, handleRadius);
                grfx.FillEllipse(handleBrush, Rectangle.Left - 10, Rectangle.Bottom + 5, handleRadius, handleRadius);
                grfx.FillEllipse(handleBrush, Rectangle.Right + 5, Rectangle.Bottom + 5, handleRadius, handleRadius);

                grfx.FillEllipse(handleBrush, Rectangle.X + (Rectangle.Width / 2) - handleRadius / 2, Rectangle.Y - 20, handleRadius, handleRadius);
            }
        }

        public virtual HandleTypeEnum GetClickedHandle(PointF point)
		{
            if (!IsSelected) return HandleTypeEnum.None;

            float handleRadius = 5f;
            RectangleF topLeft = new RectangleF(Rectangle.Left - 10, Rectangle.Top - 10, handleRadius, handleRadius);
            RectangleF topRight = new RectangleF(Rectangle.Right + 5, Rectangle.Top - 10, handleRadius, handleRadius);
            RectangleF bottomLeft = new RectangleF(Rectangle.Left - 10, Rectangle.Bottom + 5, handleRadius, handleRadius);
            RectangleF bottomRight = new RectangleF(Rectangle.Right + 5, Rectangle.Bottom + 5, handleRadius, handleRadius);
            RectangleF rotate = new RectangleF(Rectangle.X + (Rectangle.Width / 2) - handleRadius / 2, Rectangle.Y - 20, handleRadius, handleRadius);

            if (topLeft.Contains(point)) return HandleTypeEnum.TopLeft;
            if (topRight.Contains(point)) return HandleTypeEnum.TopRight;
            if (bottomLeft.Contains(point)) return HandleTypeEnum.BottomLeft;
            if (bottomRight.Contains(point)) return HandleTypeEnum.BottomRight;
            if (rotate.Contains(point)) return HandleTypeEnum.Rotate;

            return HandleTypeEnum.None;
        }

        public virtual void ResizeShape(HandleTypeEnum handle, PointF newLocation)
        {
            if (IsSelected)
            {
                RectangleF newRect = Rectangle;
                switch (handle)
                {
                    case HandleTypeEnum.TopLeft:
                        newRect = new RectangleF(newLocation.X, newLocation.Y, newRect.Width + (newRect.X - newLocation.X), newRect.Height + (newRect.Y - newLocation.Y));
                        break;
                    case HandleTypeEnum.TopRight:
                        newRect = new RectangleF(newRect.X, newLocation.Y, newRect.Width + (newLocation.X - newRect.Right), newRect.Height + (newRect.Y - newLocation.Y));
                        break;
                    case HandleTypeEnum.BottomLeft:
                        newRect = new RectangleF(newLocation.X, newRect.Y, newRect.Width + (newRect.X - newLocation.X), newRect.Height + (newLocation.Y - newRect.Bottom));
                        break;
                    case HandleTypeEnum.BottomRight:
                        newRect = new RectangleF(newRect.X, newRect.Y, newRect.Width + (newLocation.X - newRect.Right), newRect.Height + (newLocation.Y - newRect.Bottom));
                        break;
                }
                Rectangle = newRect;
            }
        }

    }
}
