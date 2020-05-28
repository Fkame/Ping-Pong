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
        private Player player;

        // Контроль ускорения мячика в процессе игры
        private int counter = 0;
        private const int MSECONDSTOSPEEDUP = 500;

        private const int MIDLINEWIDTH = 6;
        //private Rectangle MidLine = new Rectangle();

        public ChangeCanvas()
        {
            InitializeComponent();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Разделительная линия
            Pen blackPen = new Pen(System.Drawing.Color.Black);
            blackPen.Width = MIDLINEWIDTH;
            int xCenter = pictureBox1.Width / 2 - MIDLINEWIDTH / 2;
            g.DrawLine(blackPen, new Point(xCenter, 0), new Point(xCenter, pictureBox1.Height));

            // Края карты
            Pen endPen = new Pen(System.Drawing.Color.IndianRed);
            endPen.Width = 4;
            g.DrawLine(endPen, new Point(0, 0), new Point(0, pictureBox1.Height));
            g.DrawLine(endPen, new Point(pictureBox1.Width, 0), new Point(pictureBox1.Width, pictureBox1.Height));

            // Отрисовка мяча
            ball.drawYourSelf(g);

            // Отрисовка игрока
            player.drawYourSelf(g);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
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

            // Если шар споткнулся об игрока слева (не ИИ)
            if (player.isIncludeThisPoint(newRightBorderX, newY))
            {
                ball.CoordOfCenterX = player.getLocation().X - ball.Radius;
                ball.CoordOfCenterY = newY;
                ball.Steps.stepX = -ball.Steps.stepX;
                pictureBox1.Invalidate();
                return;
            }
            // Если шар споткнулся об игрока справа (не ИИ)
            if (player.isIncludeThisPoint(newLeftBorderX, newY))
            {
                ball.CoordOfCenterX = player.getLocation().X + ball.Radius + player.getWidth();
                ball.CoordOfCenterY = newY;
                ball.Steps.stepX = -ball.Steps.stepX;
                pictureBox1.Invalidate();
                return;
            }
            // Если ударлися об игрока верхней частью (не ИИ)
            if (player.isIncludeThisPoint(newX, newTopBorderY))
            {
                ball.CoordOfCenterX = newX;
                ball.CoordOfCenterY = player.getLocation().Y + player.getHeight() + ball.Radius;
                ball.Steps.stepY = -ball.Steps.stepY;
                pictureBox1.Invalidate();
                return;
            }
            // Если ударлися об игрока нижней частью (не ИИ)
            if (player.isIncludeThisPoint(newX, newBottomBorderY))
            {
                ball.CoordOfCenterX = newX;
                ball.CoordOfCenterY = player.getLocation().Y - ball.Radius;
                ball.Steps.stepY = -ball.Steps.stepY;
                pictureBox1.Invalidate();
                return;
            }

            ball.CoordOfCenterX = newX;
            ball.CoordOfCenterY = newY;

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
            if (timer1.Interval == 1)
            {
                if (ball.Steps.stepX < 20 & ball.Steps.stepY < 20)
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

            //Cursor.Hide();

            ball = BallHelper.generateRandomBallInMiddle(pictureBox1.Width / 2, pictureBox1.Width / 2, 
                0, pictureBox1.Height);

            int width = 20;
            int height = 100;
            int xCoord = pictureBox1.Width - 2 * width;
            int yCoord = pictureBox1.Height / 2 - height / 2;
            player = new Player(xCoord, yCoord, width, height);

            /*
             * pictureBox1.PointToClient(Cursor.Position) - проецирует расположение курсора на экране в разположение на элементе
             * pictureBox1.PointToScreen(Cursor.Position) - обратное от предыдущего преобразование
             */
            Cursor.Position = pictureBox1.PointToScreen(player.getMiddlePoint());

            timer1.Start();
            timer1.Interval = 30;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /*
         * Примечание: первое же движение происходит при первоначальной настройке положения курсора
         */
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point l = e.Location;
            Point pl = player.getMiddlePoint();

            // Если игрок пытается пересечь линию по середине
            if (l.X <= pictureBox1.Width / 2 + MIDLINEWIDTH / 2 + player.getWidth() / 2)
            {
                Cursor.Position = pictureBox1.PointToScreen(pl);
                return;
            }

            // Если игрок пытается уйти в правый край
            if (l.X >= pictureBox1.Width - player.getWidth() / 2) { l = pl; }

            // Если игро пытается уйти слишком вверх
            if (l.Y <= 0 + player.getHeight() / 2) { l = pl; }

            // Если игрок пытается уйти слишком вниз
            if (l.Y >= pictureBox1.Height - player.getHeight() / 2) { l = pl; }

            // Вычисляет x и y угловые точки прямоугольника по заданной середине прямоугольника
            player.changeLocationByMiddlePosition(l.X, l.Y);        
            pictureBox1.Invalidate();
        }
    }
}
