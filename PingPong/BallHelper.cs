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
            int stepX = r.Next(-10, 10);
            stepX = (stepX == 0) ? 1 : stepX;
            int stepY = r.Next(-10, 10);
            stepY = (stepY == 0) ? 3 : stepY;

            int nod = NOD(stepX, stepY);
            stepX = stepX / nod;
            stepY = stepY / nod;

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

            Console.WriteLine($"stepX = {steps.stepX}, stepY = {steps.stepY}");

            return ball;
        }
    }
}
