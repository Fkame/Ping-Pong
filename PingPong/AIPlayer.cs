using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Threading;

namespace PingPong
{
    class AIPlayer : Player
    {
        public int speed = 2;
        public AIPlayer(int xCoord, int yCoord, int width, int height) : base(xCoord, yCoord, width, height)
        { }

        /*
        * Двигается по ОСИ OY за мячём
        */
        public void makeAIMove(Ball ball)
        {
            int yMidBall = ball.CoordOfCenterY;
            int yMidStick = figure.Y - figure.Height / 2;

            if (figure.Contains(figure.X, yMidBall)) return;

            if (yMidBall < figure.Bottom)
            {
                this.figure.Y -= speed;     
            }
            else
            {
                this.figure.Y += speed;
            }
        }
    }
}
