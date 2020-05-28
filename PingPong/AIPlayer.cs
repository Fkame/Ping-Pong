using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace PingPong
{
    class AIPlayer : Player
    {
        private int speed = 5;
        public AIPlayer(int xCoord, int yCoord, int width, int height) : base(xCoord, yCoord, width, height)
        { }

        /*
        * Двигается по ОСИ OY за мячём
        */
        public void makeAIMove(Ball ball)
        {
            int yMidBall = ball.CoordOfCenterY;

            if (figure.Contains(figure.X, yMidBall)) return;

            /*
             * Для немного большей интеллектуальности добавим возможность сдвига не в ту сторону или пропуска хода
             */
            if (yMidBall < figure.Bottom)
            {
                this.figure.Y -= speed;
            }
            else
            {
                this.figure.Y += speed;
            }
        }

        /*
         * Увеличение скорости передвижения ИИ
         */
        public void upgradeAISpeed()
        {
            if (speed < figure.Height / 3) speed += 2;
        }
    }
}
