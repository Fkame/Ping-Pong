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
    /// <summary>
    /// Одна из частей основной формы. Содержит в себе события, конструкторы и состояния
    /// </summary>
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

        /// <summary>
        /// Событие перерисовки компонента. Косвенно вызывается через element.Invalidate().
        /// Отрисовывает поле, мяч, игроков согласно их состояниям.
        /// </summary>
        /// <param name="sender">Ссылка на объект, вызвавший событие</param>
        /// <param name="e">Объект, специфичный для обрабатываемого события</param>
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

        /// <summary>
        /// Обработка срабатывания таймера. Управляет перемещением мяча по полю. 
        /// Реализует анимацию мяча, так как после изменений запрашивает перерисовку компонента для отображения.
        /// Так как вызывается часто его срабатывание зацикленно, позволяет легко управлять скоростью анимации мяча.
        /// </summary>
        /// <param name="sender">Ссылка на объект, вызвавший событие</param>
        /// <param name="e">Объект, специфичный для обрабатываемого события</param>
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
        /// <summary>
        /// Метод, вызываемый при передвижении курсора по полю. Данный метод контролирует движения игрока,
        /// фиксирует их в объекте, и при необходимости ограничивает - например, 
        /// не допускает пересечение центральной линии игроком.
        /// Также реализует анимацию, так как после получения изменений - запрашивает перерисовку поля.
        /// </summary>
        /// <param name = "sender" > Ссылка на объект, вызвавший событие</param>
        /// <param name="e">Объект, специфичный для обрабатываемого события</param>
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

        /// <summary>
        /// При каждом срабатывании таймера увеличивается скорость мяча (увеличивается шаг) через вызов метода.
        /// </summary>
        /// <param name = "sender" > Ссылка на объект, вызвавший событие</param>
        /// <param name="e">Объект, специфичный для обрабатываемого события</param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            speedUpAnimation();
        }

        /// <summary>
        /// Таймер - время на раздумия ИИ. Подобие задержки на принятие решения. 
        /// Данный метод - некоторые действие после "задержки на раздумия" в виде таймера.
        /// В нём вызывается метод, реализующий движение планки ИИ в сторону мяча на некоторую величину - скорость.
        /// Также реализует анимацию, так как запрашивает перерисовку поля после передвижения планки ИИ.
        /// </summary>
        /// <param name = "sender" > Ссылка на объект, вызвавший событие</param>
        /// <param name="e">Объект, специфичный для обрабатываемого события</param>
        private void timer3_Tick(object sender, EventArgs e)
        {
            // Ход ИИ
            aiPlayer.makeAIMove(ball, pictureBox1);

            // Перерисовка
            pictureBox1.Invalidate();
        }     
    }
}
