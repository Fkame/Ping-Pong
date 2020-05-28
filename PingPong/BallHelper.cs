using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingPong
{
    abstract class BallHelper
    {
        public static System.Drawing.Color getRandomColor()
        {
            Random r = new Random();
            System.Drawing.Color color = System.Drawing.Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            return color;
        }

        public static (int x, int y) generateRandomPosition(int x1, int x2, int y1, int y2)
        {
            Random r = new Random();
            int x = r.Next(x1, x2);
            int y = r.Next(y1, y2);

            return (x, y);
        }

        /*
         * Генерируются случайные числа - шаги в пикселях по OX и OY и по возможности сокращаются
         */
        public static (int stepX, int stepY) generateRandomSteps()
        {
            Random r = new Random();
            int stepX = 0;
            int stepY = 0;

            int limit = 0;
            while (stepX < stepY |
                (double)Math.Abs(stepX) / Math.Abs(stepY) < 1.5 |
                stepX < 3)
            {
                if (++limit > 20)
                {
                    stepX = 6;
                    stepY = 2;
                    break;
                }
                stepX = r.Next(-10, 10);
                stepX = (stepX == 0) ? 3 : stepX;
                stepY = r.Next(-10, 10);
                stepY = (stepY == 0) ? 1 : stepY;

                int nod = NOD(stepX, stepY);
                stepX = stepX / nod;
                stepY = stepY / nod;
            }

            return (stepX, stepY);
        }

        private static int NOD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            while (a != b)
            {
                if (a >b)
                    a = a - b;
                else
                    b = b - a;
            }
            return b;
        }        

        public static Ball generateRandomBallInMiddle(int leftLimit, int rightLimit, int topLimit, int bottomLimit)
        {
            int radius = 20;

            // Координаты вдоль линии по середине поля
            var coords = BallHelper.generateRandomPosition(leftLimit, rightLimit,
                topLimit + radius, bottomLimit - radius);

            // Сдвиг мяча по ОХ и ОУ
            var steps = BallHelper.generateRandomSteps();

            Ball ball = new Ball(radius, coords.x, coords.y, 
                steps.stepX, steps.stepY, 
                BallHelper.getRandomColor());

            return ball;
        }

        public static bool makeARebountFromPlayerIfNeed(Ball ball, Player player) 
        {
            int newX = ball.CoordOfCenterX + ball.Steps.stepX;
            int newY = ball.CoordOfCenterY + ball.Steps.stepY;

            int newLeftBorderX = newX - ball.Radius;
            int newRightBorderX = newX + ball.Radius;
            int newTopBorderY = newY - ball.Radius;
            int newBottomBorderY = newY + ball.Radius;

            // Если шар споткнулся об игрока слева
            if (player.isIncludeThisPoint(newRightBorderX, newY))
            {
                ball.CoordOfCenterX = player.getLocation().X - ball.Radius;
                ball.CoordOfCenterY = newY;
                ball.Steps.stepX = -ball.Steps.stepX;
                return true;
            }
            // Если шар споткнулся об игрока справа
            if (player.isIncludeThisPoint(newLeftBorderX, newY))
            {
                ball.CoordOfCenterX = player.getLocation().X + ball.Radius + player.getWidth();
                ball.CoordOfCenterY = newY;
                ball.Steps.stepX = -ball.Steps.stepX;
                return true;
            }
            // Если ударлися об игрока верхней частью
            if (player.isIncludeThisPoint(newX, newTopBorderY))
            {
                ball.CoordOfCenterX = newX;
                ball.CoordOfCenterY = player.getLocation().Y + player.getHeight() + ball.Radius;
                ball.Steps.stepY = -ball.Steps.stepY;
                return true;
            }
            // Если ударлися об игрока нижней частью
            if (player.isIncludeThisPoint(newX, newBottomBorderY))
            {
                ball.CoordOfCenterX = newX;
                ball.CoordOfCenterY = player.getLocation().Y - ball.Radius;
                ball.Steps.stepY = -ball.Steps.stepY;
                return true;
            }

            return false;
        }

        public static bool makeARebountFromWalls(Ball ball, PictureBox pictureBox1)
        {
            int newX = ball.CoordOfCenterX + ball.Steps.stepX;
            int newY = ball.CoordOfCenterY + ball.Steps.stepY;

            int newTopBorderY = newY - ball.Radius;
            int newBottomBorderY = newY + ball.Radius;

            // Если шар пытается преодолеть верхюю или нижнюю границы
            if (newTopBorderY <= 0)
            {
                ball.CoordOfCenterX = newX;
                ball.CoordOfCenterY = 0 + ball.Radius;
                ball.Steps.stepY = -ball.Steps.stepY;
                return true;
            }
            if (newBottomBorderY >= pictureBox1.Height)
            {
                ball.CoordOfCenterX = newX;
                ball.CoordOfCenterY = pictureBox1.Height - ball.Radius;
                ball.Steps.stepY = -ball.Steps.stepY;
                return true;
            }

            return false;
        }
    }
}
