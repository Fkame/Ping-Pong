using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            int stepX = new Random().Next(-10, 10);
            stepX = (stepX == 0) ? 1 : stepX;
            int stepY = new Random().Next(-10, 10);
            stepY = (stepY == 0) ? 3 : stepY;
            int nod = NOD(stepX, stepY);
            if (nod != stepX || nod != stepY)
            {
                stepX = stepX / nod;
                stepY = stepY / nod;
            }
            return (stepX, stepY);
        }

        private static int NOD(int a, int b)
        {
            while (a != b)
            {
                if (a >b)
                    a = a - b;
                else
                    b = b - a;
            }
            return b;
        }        
    }
}
