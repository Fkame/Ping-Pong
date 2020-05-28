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
    class AIPlayer : Player
    {
        public int speed = 5;
        public AIPlayer(int xCoord, int yCoord, int width, int height) : base(xCoord, yCoord, width, height)
        { }

        /*
        * Двигается по ОСИ OY за мячём
        */
        public void makeAIMove(Ball ball, PictureBox area)
        {
            int yMidBall = ball.CoordOfCenterY;
            int yMidStick = figure.Y + figure.Height / 2;

            int ballBottom = ball.CoordOfCenterY + ball.Radius;
            int ballTop = ball.CoordOfCenterY + ball.Radius;

            /*
             * Проверка границ
             */
            if (yMidStick > yMidBall & yMidStick - yMidBall > ball.Radius / 3)
            {
                if (figure.Top - speed >= 0) figure.Y -= speed;
                return;
            }
            if (yMidStick < yMidBall & yMidBall - yMidStick > ball.Radius / 3)
            {
                if (figure.Bottom + speed <= area.Height) figure.Y += speed;
                return;
            }
        }
    }
}
