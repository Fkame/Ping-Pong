using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PingPong
{
    /// <summary>
    /// Класс, расширяющий класс простого игрока, дополнительно имеет ряд функций по автоматизации 
    /// движения по полю.
    /// </summary>
    /// <remarks>
    /// Дополнительные классы: движение в сторону мяча.
    /// Дополнительные состояния: скорость - speed.
    /// </remarks>
    class AIPlayer : Player
    {
        public int speed = 5;

        /// <summary>
        /// Единственный конструктор, вызывающий конструктор родителя.
        /// </summary>
        /// <param name="xCoord">X-координата левого верхнего угла прямоугольника</param>
        /// <param name="yCoord">Y-координата левого верхнего угла прямоугольника</param>
        /// <param name="width">Ширина прямоульнике</param>
        /// <param name="height">Высота прямоугольника</param>
        public AIPlayer(int xCoord, int yCoord, int width, int height) : base(xCoord, yCoord, width, height)
        { }

        /// <summary>
        /// Данный метод реализует движение планки по оси OY в направлении мяча.
        /// Дальность шага за 1 раз определяется скоростью этого игрока (параметр speed) - количество пикселей,
        /// на которое сдвигается планка в сторону мяча по OY.
        /// </summary>
        /// <remarks>
        /// Планка сдвигается в том случае, если расстояние по OY 
        /// между центрами планки и мяча больше половины от радиуса мяча.
        /// Также предусмотрены упирания в верхнюю и нижниюю стены.
        /// </remarks>
        /// <param name="ball">Игровой мяч</param>
        /// <param name="area">Игровое поле</param>
        public void makeAIMove(Ball ball, PictureBox area)
        {
            int yMidBall = ball.CoordOfCenterY;
            int yMidStick = figure.Y + figure.Height / 2;

            if (yMidStick > yMidBall & yMidStick - yMidBall > ball.Radius / 2)
            {
                if (figure.Top - speed >= 0) figure.Y -= speed;
                return;
            }
            if (yMidStick < yMidBall & yMidBall - yMidStick > ball.Radius / 2)
            {
                if (figure.Bottom + speed <= area.Height) figure.Y += speed;
                return;
            }
        }
    }
}
