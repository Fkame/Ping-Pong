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
        Ball ball;

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

            int newX = ball.CoordOfCenterX + ball.steps.stepX;
            int newY = ball.CoordOfCenterY + ball.steps.stepY;

            int newLeftBorderX = newX - ball.Radius;
            int newRightBorderX = newX + ball.Radius;
            if (newLeftBorderX <= 0)
            {
                ball.CoordOfCenterX = 0 + ball.Radius;
                ball.CoordOfCenterY = newY;
                ball.steps.stepX = -ball.steps.stepX;
                panel1.Invalidate();
                return;
            }
            if (newRightBorderX >= panel1.Width)
            {
                ball.CoordOfCenterX = panel1.Width - ball.Radius;
                ball.CoordOfCenterY = newY;
                ball.steps.stepX = -ball.steps.stepX;
                panel1.Invalidate();
                return;
            }

            int newTopBorderY = newY - ball.Radius;
            int newBottomBorderY = newY + ball.Radius;
            if (newTopBorderY <= 0)
            {
                ball.CoordOfCenterX = newX;
                ball.CoordOfCenterY = 0 + ball.Radius;
                ball.steps.stepY = -ball.steps.stepY;
                panel1.Invalidate();
                return;
            }
            if (newBottomBorderY >= panel1.Height)
            {
                ball.CoordOfCenterX = newX;
                ball.CoordOfCenterY = panel1.Height - ball.Radius;
                ball.steps.stepY = -ball.steps.stepY;
                panel1.Invalidate();
                return;
            }

            ball.CoordOfCenterX = newX;
            ball.CoordOfCenterY = newY;
            
            Console.WriteLine($"After: Ball.X = {ball.CoordOfCenterX}, " +
               $"Ball.Y = {ball.CoordOfCenterY}\n");

            // Перерисовка
            panel1.Invalidate();
        }

        /*
         * Запуск консоли для Debug-штук
         */
        private void Form2_Load(object sender, EventArgs e)
        {
            AllocConsole();

            int radius = 20;

            // Координаты вдоль линии по середине поля
            var coords = BallHelper.generateRandomPosition(panel1.Width / 2, panel1.Width / 2,
                radius, panel1.Height - radius);

            // Сдвиг мяча по ОХ и ОУ
            var steps = BallHelper.generateRandomSteps();

            ball = new Ball(radius, coords.x, coords.y, BallHelper.getRandomColor());

            ball.steps.stepX = steps.stepX * 2;
            ball.steps.stepY = steps.stepY * 2;

            timer1.Start();
            timer1.Interval = 100;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
