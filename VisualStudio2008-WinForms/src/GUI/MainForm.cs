using Draw.src.util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Draw
{
	/// <summary>
	/// Върху главната форма е поставен потребителски контрол,
	/// в който се осъществява визуализацията
	/// </summary>
	public partial class MainForm : Form
    {

        /// <summary>
        /// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
        /// </summary>
        private DialogProcessor dialogProcessor = new DialogProcessor();
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		/// <summary>
		/// Изход от програмата. Затваря главната форма, а с това и програмата.
		/// </summary>
		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
		/// </summary>
		void ViewPortPaint(object sender, PaintEventArgs e)
		{
			dialogProcessor.ReDraw(sender, e);
		}

		
		/// <summary>
		/// Бутон, който поставя на произволно място правоъгълник със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();
			
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
			
			viewPort.Invalidate();
		}

        /// <summary>
        /// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
        /// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
        /// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
        /// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
        /// </summary>
        void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (pickUpSpeedButton.Checked)
            {
                // First, clear the selection for all shapes
                foreach (var shape in dialogProcessor.ShapeList)
                {
                    shape.IsSelected = false;
                }

                dialogProcessor.Selection = dialogProcessor.ContainsPoint(e.Location);

                if (dialogProcessor.Selection != null)
                {
                    // Set the IsSelected property for the selected shape
                    dialogProcessor.Selection.IsSelected = true;

                    // Check if the clicked point is within any of the handles
                    dialogProcessor.currentHandle = dialogProcessor.Selection.GetClickedHandle(e.Location);

                    statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
                    dialogProcessor.IsDragging = true;
                    dialogProcessor.LastLocation = e.Location;
                    viewPort.Invalidate();
                }
                else
                {
                    dialogProcessor.currentHandle = HandleTypeEnum.None;
                }
            }
        }


        /// <summary>
        /// Прихващане на преместването на мишката.
        /// Ако сме в режм на "влачене", то избрания елемент се транслира.
        /// </summary>
        void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (dialogProcessor.IsDragging && dialogProcessor.Selection != null)
            {
                switch (dialogProcessor.currentHandle)
                {
                    case HandleTypeEnum.None:
                        statusBar.Items[0].Text = "Последно действие: Влачене";
                        dialogProcessor.TranslateTo(e.Location);
                        break;
                    case HandleTypeEnum.TopLeft:
                    case HandleTypeEnum.TopRight:
                    case HandleTypeEnum.BottomLeft:
                    case HandleTypeEnum.BottomRight:
                        statusBar.Items[0].Text = "Последно действие: Промяна на размер";
                        dialogProcessor.ResizeShape(dialogProcessor.currentHandle, e.Location);
                        break;
                    case HandleTypeEnum.Rotate:
                        statusBar.Items[0].Text = "Последно действие: Ротация";
                        dialogProcessor.RotateShape(e.Location);
                        break;
                }
                viewPort.Invalidate();
            }
        }


        /// <summary>
        /// Прихващане на отпускането на бутона на мишката.
        /// Излизаме от режим "влачене".
        /// </summary>
        void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			dialogProcessor.IsDragging = false;
		}

        private void drawCircleButton_Click(object sender, EventArgs e)
        {
            
                dialogProcessor.AddRandomCircle();

                statusBar.Items[0].Text = "Последно действие: Рисуване на кръг";

                viewPort.Invalidate();

        }

        private void drawStarButton_Click(object sender, EventArgs e)
        {

            dialogProcessor.AddRandomStar();

            statusBar.Items[0].Text = "Последно действие: Рисуване на звезда";

            viewPort.Invalidate();
        }

        private void drawLineButton_Click(object sender, EventArgs e)
        {

            dialogProcessor.AddRandomLine();

            statusBar.Items[0].Text = "Последно действие: Рисуване на линия";

            viewPort.Invalidate();
        }

        private void drawElipseButton_Click(object sender, EventArgs e)
        {
			dialogProcessor.AddRandomEllipse();

            statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

            viewPort.Invalidate();

        }
    }
}
