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
            Console.WriteLine(System.DateTime.Now);

            // Вычисления положения мяча


            // Перерисовка
            panel1.Invalidate();
        }


        /*
         * Запуск консоли для Debug-штук
         */
        private void Form2_Load(object sender, EventArgs e)
        {
            AllocConsole();

            /*
             * Создание мяча
             */
            int radius = 20;
            var coords = BallHelper.generateRandomPosition(panel1.Width / 2, panel1.Width / 2,
                radius, panel1.Height - radius);

            ball = new Ball(radius, coords.x, coords.y, BallHelper.getRandomColor());


            timer1.Start();
            timer1.Interval = 1000;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
