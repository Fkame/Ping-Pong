﻿using System;
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
        private AIPlayer aiPlayer;

        // Контроль ускорения мячика в процессе игры
        private int counter = 0;
        private const int MSECONDSTOSPEEDUP = 500;

        private const int MIDLINEWIDTH = 6;

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

            // Ход ИИ
            aiPlayer.makeAIMove(ball);

            // Увеличение скорости мяча
            counter += timer1.Interval;

            Console.WriteLine("count = {0}", counter);
            if (counter >= MSECONDSTOSPEEDUP)
            {
                Console.WriteLine("\nSpeedUp! Interval = {0}\n", timer1.Interval);
                counter = 0;
                aiPlayer.upgradeAISpeed();
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
                    ball.Steps.stepX += (int)(ball.Steps.stepX * 0.2);
                    ball.Steps.stepY += (int)(ball.Steps.stepY * 0.2);
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

            Console.WriteLine("Form load");
            ball = BallHelper.generateRandomBallInMiddle(pictureBox1.Width / 2, pictureBox1.Width / 2, 
                0, pictureBox1.Height);

            Console.WriteLine("Ball created");
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
