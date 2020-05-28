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
    /// Одна из частей основной формы. Содержит в себе все методы, кроме событий, 
    /// конструкторов, и кроме состояний
    /// </summary>
    public partial class Ping_Pong_Field : Form
    {
        /// <summary>
        /// Увеличивает шаг мяча на 10% от текущего
        /// </summary>
        private void speedUpAnimation()
        {
            if (Math.Abs(ball.Steps.stepX) < 15 & Math.Abs(ball.Steps.stepY) < 15)
            {
                // SpeedUp Ball
                ball.Steps.stepX += (int)Math.Floor(ball.Steps.stepX * 0.1);
                ball.Steps.stepY += (int)Math.Floor(ball.Steps.stepY * 0.1);

                // SpeedUp AI
                if (aiPlayer.speed < 10) aiPlayer.speed++;
            }
        }

        /// <summary>
        /// Вызывается при забитии гола одной из сторон. Меняет счёт и выводит его.
        /// </summary>
        /// <param name="isPlayer">Указывается true, если гол забил игрок, false, если гол забил ИИ </param>
        private void scoreUp(bool isPlayer)
        {
            initializeElementsOnStart();

            if (isPlayer) scorePanel.Player1++;
            else scorePanel.PlayerAI++;

            label1.Text = scorePanel.ToString();
        }

        /// <summary>
        /// Пересоздаёт все элементы, располагает их в начальные позиции. Как правило, вызывается при запуске и
        /// после каждого гола
        /// </summary>
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
            timer1.Interval = 1;

            timer1.Start();
            timer2.Start();

            timer3.Interval = 20;
            timer3.Start();
        }
    }
}
