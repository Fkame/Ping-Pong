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
    public partial class Ping_Pong_Field : Form
    {
        private Ball ball;
        private Player player;
        private AIPlayer aiPlayer;
        private ScorePanel scorePanel;

        // Контроль ускорения мячика в процессе игры
        private const int MSECONDSTOSPEEDUP = 3000;

        // Ширина средней линии
        private const int MIDLINEWIDTH = 6;

        public Ping_Pong_Field()
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

            // Отрисовка игроков
            player.drawYourSelf(g);
            aiPlayer.drawYourSelf(g);
        }

        /*
         * Отвечает за движения мяча и ИИ-игрока
         */
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Следующие координаты центра мяча
            int newX = ball.CoordOfCenterX + ball.Steps.stepX;
            int newY = ball.CoordOfCenterY + ball.Steps.stepY;

            int newLeftBorderX = newX - ball.Radius;
            int newRightBorderX = newX + ball.Radius;

            // Забите гола - боковые стены
            if (newLeftBorderX <= 0)
            {
                scoreUp(true);
                return;
            }
            if (newRightBorderX >= pictureBox1.Width)
            {
                scoreUp(false);
                return;
            }

            // Отскакивание мяча от стен и игроков
            if (!BallHelper.makeARebountFromWalls(ball, pictureBox1))
            {
                if (!BallHelper.makeARebountFromPlayerIfNeed(ball, player) &
                    !BallHelper.makeARebountFromPlayerIfNeed(ball, aiPlayer))
                {
                    ball.CoordOfCenterX = newX;
                    ball.CoordOfCenterY = newY;
                }
            }

            // Перерисовка
            pictureBox1.Invalidate();
        }

        private void speedUpAnimation()
        {
            if (timer1.Interval == 1)
            {
                if (Math.Abs(ball.Steps.stepX) < 15 & Math.Abs(ball.Steps.stepY) < 15)
                {
                    // SpeedUp Ball
                    ball.Steps.stepX += (int)Math.Floor(ball.Steps.stepX * 0.2);
                    ball.Steps.stepY += (int)Math.Floor(ball.Steps.stepY * 0.2);

                    // SpeedUp AI
                    if (aiPlayer.speed < 10) aiPlayer.speed++;
                }
                // Увеличвение скорости ИИ до максимума при максимальной скорости мяча
                aiPlayer.speed = 10;
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
        private void Ping_Pong_Field_Load(object sender, EventArgs e)
        {
            //AllocConsole();

            scorePanel = new ScorePanel();
            Cursor.Hide();

            this.initializeElementsOnStart();
        }

        /*
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        */

        /*
         * Примечание: первое же движение происходит при первоначальной настройке положения курсора
         * Отвечает за движение планки игрока за курсором
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

        /*
         * Отвечает за увеличение скорости анимации
         */
        private void timer2_Tick(object sender, EventArgs e)
        {
            speedUpAnimation();
        }

        /*
         * Отвечает за ход ИИ
         */
        private void timer3_Tick(object sender, EventArgs e)
        {
            // Ход ИИ
            aiPlayer.makeAIMove(ball, pictureBox1);

            // Перерисовка
            pictureBox1.Invalidate();
        }

        /*
         * Срабатывает при забитии гола
         */
        private void scoreUp(bool isPlayer)
        {
            initializeElementsOnStart();

            if (isPlayer) scorePanel.Player1++;
            else scorePanel.PlayerAI++;

            label1.Text = scorePanel.ToString();
        }

        /*
         * Расположение в начальном положении (на старте и при забитии гола) 
         */
        private void initializeElementsOnStart()
        {
            ball = BallHelper.generateRandomBall(pictureBox1.Width / 2, pictureBox1.Width / 2,
                0, pictureBox1.Height);

            int width = 20;
            int height = 100;

            // Создание игрока
            int xCoord = pictureBox1.Width - 2 * width;
            int yCoord = pictureBox1.Height / 2 - height / 2;
            player = new Player(xCoord, yCoord, width, height);

            // Создание ИИ игрока
            xCoord = 0 + width;
            yCoord = pictureBox1.Height / 2 - height / 2;
            aiPlayer = new AIPlayer(xCoord, yCoord, width, height);

            /*
             * pictureBox1.PointToClient(Cursor.Position) - проецирует расположение курсора на экране в разположение на элементе
             * pictureBox1.PointToScreen(Cursor.Position) - обратное от предыдущего преобразование
             */
            Cursor.Position = pictureBox1.PointToScreen(player.getMiddlePoint());

            timer2.Interval = MSECONDSTOSPEEDUP;
            timer1.Interval = 20;

            timer1.Start();
            timer2.Start();

            timer3.Interval = 20;
            timer3.Start();
        }
    }
}
