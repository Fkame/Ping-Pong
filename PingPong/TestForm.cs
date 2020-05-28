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
    public partial class TestForm : Form
    {
        private Ball ball;
        private int counter = 0;
        private const int MSECONDSTOSPEEDUP = 10000;

        public TestForm()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Разделительная линия
            Pen blackPen = new Pen(System.Drawing.Color.Black);
            blackPen.Width = 6;
            int xCenter = panel1.Width / 2;
            g.DrawLine(blackPen, new Point(xCenter, 0), new Point(xCenter, panel1.Height));

            // Края карты
            Pen endPen = new Pen(System.Drawing.Color.IndianRed);
            endPen.Width = 4;
            g.DrawLine(endPen, new Point(0, 0), new Point(0, panel1.Height));
            g.DrawLine(endPen, new Point(panel1.Width, 0), new Point(panel1.Width, panel1.Height));

            // Отрисовка мяча
            int x = ball.CoordOfCenterX - ball.Radius;
            int y = ball.CoordOfCenterY - ball.Radius;

            SolidBrush brush = new SolidBrush(ball.ColorOfBall);
            Rectangle rect = new Rectangle(x, y, 2 * ball.Radius, 2 * ball.Radius);
            g.FillEllipse(brush, rect);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Console.Write(System.DateTime.Now);
            Console.WriteLine($"\nBefore: Ball.X = {ball.CoordOfCenterX}, " +
                $"Ball.Y = {ball.CoordOfCenterY}\n");

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
                panel1.Invalidate();
                return;
            }
            if (newLeftBorderX <= 0 & newBottomBorderY >= panel1.Width)
            {
                ball.CoordOfCenterX = 0 + ball.Radius;
                ball.CoordOfCenterY = panel1.Height - ball.Radius;
                ball.Steps.stepX = -ball.Steps.stepX;
                ball.Steps.stepY = -ball.Steps.stepY;
                panel1.Invalidate();
                return;
            }
            if (newRightBorderX >= panel1.Width & newTopBorderY <= 0)
            {
                ball.CoordOfCenterX = panel1.Width - ball.Radius;
                ball.CoordOfCenterY = 0 + ball.Radius;
                ball.Steps.stepX = -ball.Steps.stepX;
                ball.Steps.stepY = -ball.Steps.stepY;
                panel1.Invalidate();
                return;
            }
            if (newRightBorderX >= panel1.Width & newBottomBorderY >= panel1.Height)
            {
                ball.CoordOfCenterX = panel1.Width - ball.Radius;
                ball.CoordOfCenterY = panel1.Height - ball.Radius;
                ball.Steps.stepX = -ball.Steps.stepX;
                ball.Steps.stepY = -ball.Steps.stepY;
                panel1.Invalidate();
                return;
            }

            // Если шар пытается пересечь левую или правую границы
            if (newLeftBorderX <= 0)
            {
                ball.CoordOfCenterX = 0 + ball.Radius;
                ball.CoordOfCenterY = newY;
                ball.Steps.stepX = -ball.Steps.stepX;
                panel1.Invalidate();
                return;
            }
            if (newRightBorderX >= panel1.Width)
            {
                ball.CoordOfCenterX = panel1.Width - ball.Radius;
                ball.CoordOfCenterY = newY;
                ball.Steps.stepX = -ball.Steps.stepX;
                panel1.Invalidate();
                return;
            }

            // Если шар пытается преодолеть верхюю или нижнюю границы
            if (newTopBorderY <= 0)
            {
                ball.CoordOfCenterX = newX;
                ball.CoordOfCenterY = 0 + ball.Radius;
                ball.Steps.stepY = -ball.Steps.stepY;
                panel1.Invalidate();
                return;
            }
            if (newBottomBorderY >= panel1.Height)
            {
                ball.CoordOfCenterX = newX;
                ball.CoordOfCenterY = panel1.Height - ball.Radius;
                ball.Steps.stepY = -ball.Steps.stepY;
                panel1.Invalidate();
                return;
            }

            ball.CoordOfCenterX = newX;
            ball.CoordOfCenterY = newY;
            
            Console.WriteLine($"After: Ball.X = {ball.CoordOfCenterX}, " +
               $"Ball.Y = {ball.CoordOfCenterY}\n");
    
            // Увеличение скорости мяча
            counter += timer1.Interval;
            if (counter >= MSECONDSTOSPEEDUP)
            {
                counter = 0;
                this.speedUpAnimation();
            }

            // Перерисовка
            panel1.Invalidate();
        }

        private void speedUpAnimation()
        {
            if (timer1.Interval != 1) timer1.Interval = timer1.Interval / 2;
            if (timer1.Interval <= 10 & ball.Steps.stepX < 50 & ball.Steps.stepY < 50)
            {
                ball.Steps.stepX *= 2;
                ball.Steps.stepY *= 2;
            }
        }

        /*
         * Запуск консоли для Debug-штук
         */
        private void Form2_Load(object sender, EventArgs e)
        {
            AllocConsole();

            this.DoubleBuffered = true;

            ball = BallHelper.generateRandomBallInMiddle(panel1);

            timer1.Start();
            timer1.Interval = 60;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
