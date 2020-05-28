using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingPong
{
    /// <summary>
    /// Класс BallHelper являеся абстрактным, и содержит только статические методы.
    /// Он реализует дополнительные операции над Ball, которые дополняют функционал Ball.
    /// </summary>
    abstract class BallHelper
    {
        /// <summary>
        /// Этот метод генерирует случайный цвет, и передаёт его в виде члена структуры
        /// </summary>
        /// <returns> Возвращает случайно сгенереированый цвет</returns>
        public static System.Drawing.Color getRandomColor()
        {
            Random r = new Random();
            System.Drawing.Color color = System.Drawing.Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            return color;
        }

        /// <summary>
        /// Этот метод генерирует случайную точку на основе входящих ограничений по осям. 
        /// Т.е. он генерирует точку в определяемом входными параметрами прямоугольнике.
        /// </summary>
        /// <param name="x1">Левая граница генерации x-значения</param>
        /// <param name="x2">Права граница генерации x-значения</param>
        /// <param name="y1">Верхняя граница генерации y-значения</param>
        /// <param name="y2">Нижняя граница генерации y-значения</param>
        /// <returns>Возвращает кортеж из x и y значения точки</returns>
        public static (int x, int y) generateRandomPosition(int x1, int x2, int y1, int y2)
        {
            Random r = new Random();
            int x = r.Next(x1, x2);
            int y = r.Next(y1, y2);

            return (x, y);
        }

        /// <summary>
        /// Генерирует случайные числа, которые в последствии станут шагами (скоростями, смещениями) 
        /// передвижения шара по полю. Это те числа, которые описывают, на сколько и в какую сторону будет
        /// сдвигаться шар при каждом "тике" таймера
        /// </summary>
        /// <returns>Возвращает пару смещений по x, y</returns>
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

        /// <summary>
        /// Алгоритм Евклида по нахождению общего делителя. Применяется для сокращения смещений.
        /// </summary>
        /// <param name="a">Первое число для нахождения НОД</param>
        /// <param name="b">Второе число для нахождения НОД</param>
        /// <returns>Их общий наибольший делитель</returns>
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

        /// <summary>
        /// Создаёт объект "Мяч" со случайными характеристиками в заданных пределах по OX и OY
        /// </summary>
        /// <param name="leftLimit">Левая граница генерации x-значения</param>
        /// <param name="rightLimit">Правая граница генерации x-значения</param>
        /// <param name="topLimit">Верхняя граница генерации y-значения</param>
        /// <param name="bottomLimit">Нижняя граница генерации y-значения</param>
        /// <returns>Возвращает готовый к использованию объект типа Ball</returns>
        public static Ball generateRandomBall(int leftLimit, int rightLimit, int topLimit, int bottomLimit)
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

        /// <summary>
        /// Определяет сталкивается ли мяч с какой-либо гранью входещей ракетки,
        /// и если да - обрабатывает "отскок" от одной из граней ракетки
        /// </summary>
        /// <param name="ball">Игровой мяч</param>
        /// <param name="player">Ракетка, который управляет игрок, или AI</param>
        /// <returns>Сталкивается ли ракетка с мяём в следущем координате движения мяча</returns>
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

        /// <summary>
        /// Определяет, сталкивается ли мяч с верхней или нижней стеной при следующем шаге, если да,
        /// обрабатывает "отскок" мяча от соответствующей стены
        /// </summary>
        /// <param name="ball">Игровой мя</param>
        /// <param name="pictureBox1">Игровое поле</param>
        /// <returns>Сталкивается ли мяч с верхней или нижней стеной в следущем координате движения мяча</returns>
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
