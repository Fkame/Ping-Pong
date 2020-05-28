using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Diagnostics;

namespace PingPong
{
    public partial class ChangeCanvas : Form
    {
        private Ball ball;
        private int counter = 0;
        private const int MSECONDSTOSPEEDUP = 3000;

        public ChangeCanvas()
        {
            InitializeComponent();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Разделительная линия
            Pen blackPen = new Pen(System.Drawing.Color.Black);
            blackPen.Width = 6;
            int xCenter = pictureBox1.Width / 2;
            g.DrawLine(blackPen, new Point(xCenter, 0), new Point(xCenter, pictureBox1.Height));

            // Края карты
            Pen endPen = new Pen(System.Drawing.Color.IndianRed);
            endPen.Width = 4;
            g.DrawLine(endPen, new Point(0, 0), new Point(0, pictureBox1.Height));
            g.DrawLine(endPen, new Point(pictureBox1.Width, 0), new Point(pictureBox1.Width, pictureBox1.Height));

            // Отрисовка мяча
            int x = ball.CoordOfCenterX - ball.Radius;
            int y = ball.CoordOfCenterY - ball.Radius;

            SolidBrush brush = new SolidBrush(ball.ColorOfBall);
            Rectangle rect = new Rectangle(x, y, 2 * ball.Radius, 2 * ball.Radius);
            g.FillEllipse(brush, rect);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*
            Console.Write(System.DateTime.Now);
            Console.WriteLine($"\nBefore: Ball.X = {ball.CoordOfCenterX}, " +
                $"Ball.Y = {ball.CoordOfCenterY}\n");
            */

            int newX = ball.CoordOfCenterX + ball.Steps.stepX;
            int newY = ball.CoordOfCenterY + ball.Steps.stepY;

            int newLeftBorderX = newX - ball.Radius;
            int newRightBorderX = newX + ball.Radius;

            int newTopBorderY = newY - ball.Radius;
            int newBottomBorderY = newY + ball.Radius;

            // Если шар пытается преодолеть 2 границы сразу
            if (newLeftBorderX <= 0 & newTopBorderY <= 0)
            {
                ball.CoordOfCenterX = 0 + ball.Radius;
                ball.CoordOfCenterY = 0 + ball.Radius;
                ball.Steps.stepX = -ball.Steps.stepX;
                ball.Steps.stepY = -ball.Steps.stepY;
                pictureBox1.Invalidate();
                return;
            }
            if (newLeftBorderX <= 0 & newBottomBorderY >= pictureBox1.Width)
            {
                ball.CoordOfCenterX = 0 + ball.Radius;
                ball.CoordOfCenterY = pictureBox1.Height - ball.Radius;
                ball.Steps.stepX = -ball.Steps.stepX;
                ball.Steps.stepY = -ball.Steps.stepY;
                pictureBox1.Invalidate();
                return;
            }
            if (newRightBorderX >= pictureBox1.Width & newTopBorderY <= 0)
            {
                ball.CoordOfCenterX = pictureBox1.Width - ball.Radius;
                ball.CoordOfCenterY = 0 + ball.Radius;
                ball.Steps.stepX = -ball.Steps.stepX;
                ball.Steps.stepY = -ball.Steps.stepY;
                pictureBox1.Invalidate();
                return;
            }
            if (newRightBorderX >= pictureBox1.Width & newBottomBorderY >= pictureBox1.Height)
            {
                ball.CoordOfCenterX = pictureBox1.Width - ball.Radius;
                ball.CoordOfCenterY = pictureBox1.Height - ball.Radius;
                ball.Steps.stepX = -ball.Steps.stepX;
                ball.Steps.stepY = -ball.Steps.stepY;
                pictureBox1.Invalidate();
                return;
            }

            // Если шар пытается пересечь левую или правую границы
            if (newLeftBorderX <= 0)
            {
                ball.CoordOfCenterX = 0 + ball.Radius;
                ball.CoordOfCenterY = newY;
                ball.Steps.stepX = -ball.Steps.stepX;
                pictureBox1.Invalidate();
                return;
            }
            if (newRightBorderX >= pictureBox1.Width)
            {
                ball.CoordOfCenterX = pictureBox1.Width - ball.Radius;
                ball.CoordOfCenterY = newY;
                ball.Steps.stepX = -ball.Steps.stepX;
                pictureBox1.Invalidate();
                return;
            }

            // Если шар пытается преодолеть верхюю или нижнюю границы
            if (newTopBorderY <= 0)
            {
                ball.CoordOfCenterX = newX;
                ball.CoordOfCenterY = 0 + ball.Radius;
                ball.Steps.stepY = -ball.Steps.stepY;
                pictureBox1.Invalidate();
                return;
            }
            if (newBottomBorderY >= pictureBox1.Height)
            {
                ball.CoordOfCenterX = newX;
                ball.CoordOfCenterY = pictureBox1.Height - ball.Radius;
                ball.Steps.stepY = -ball.Steps.stepY;
                pictureBox1.Invalidate();
                return;
            }

            ball.CoordOfCenterX = newX;
            ball.CoordOfCenterY = newY;

            /*
            Console.WriteLine($"After: Ball.X = {ball.CoordOfCenterX}, " +
               $"Ball.Y = {ball.CoordOfCenterY}\n");
            */

            // Увеличение скорости мяча
            counter += timer1.Interval;
            if (counter >= MSECONDSTOSPEEDUP)
            {
                counter = 0;
                this.speedUpAnimation();
            }

            // Перерисовка
            pictureBox1.Invalidate();
        }

        private void speedUpAnimation()
        {
            /*
            if (timer1.Interval != 1) timer1.Interval = timer1.Interval / 2;
            if (timer1.Interval <= 1 & ball.Steps.stepX < 50 & ball.Steps.stepY < 50)
            {
                ball.Steps.stepX *= 2;
                ball.Steps.stepY *= 2;
            }
            */
            //Console.WriteLine("speed = " + timer1.Interval);
            if (timer1.Interval == 1)
            {
                if (ball.Steps.stepX < 50 & ball.Steps.stepY < 50)
                {
                    ball.Steps.stepX += 5;
                    ball.Steps.stepY += 5;
                }
            }
            else
            {
                if (timer1.Interval >= 6) timer1.Interval -= 5;
                else timer1.Interval = 1;
            }

        }

        /*
         * Запуск консоли для Debug-штук
         */
        private void ChangeCanvas_Load(object sender, EventArgs e)
        {
            AllocConsole();

            ball = BallHelper.generateRandomBallInMiddle(pictureBox1.Width / 2, pictureBox1.Width / 2, 
                0, pictureBox1.Height);

            timer1.Start();
            timer1.Interval = 30;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

    }
}
