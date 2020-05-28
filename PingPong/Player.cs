using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingPong
{
    /// <summary>
    /// Класс, реализующий планку-игрока
    /// </summary>
    /// <remarks>
    /// Планка стоит из: прямоугольника (figure), кисти (Brush), цвета (Color).
    /// Планка может: изменить позицию в пространстве на координатам, 
    /// изменить позицию по координатам потенциальной середины фигуры, входит ли точка в пределы планки,
    /// нарисовать себя.
    /// </remarks>
    class Player
    {
        protected Rectangle figure;
        protected SolidBrush brush;
        protected System.Drawing.Color color;
        public System.Drawing.Color Color { 
            get { return this.color; }
            set 
            {
                this.color = value;
                brush = new SolidBrush(value);
            } 
        }

        public Player() { }

        public Player (int startPositionX, int startPositionY, int width, int height)
            : this(startPositionX, startPositionY, width, height, System.Drawing.Color.Black) { }
        public Player (int startPositionX, int startPositionY, int width, int height, System.Drawing.Color color)
        {
            figure = new Rectangle(startPositionX, startPositionY, width, height);
            this.Color = color;
        }

        public (int oldX, int oldY) changePosition(int newX, int newY)
        {
            int oldX = figure.X;
            int oldY = figure.Y;
            figure.X = newX;
            figure.Y = newY;

            return (oldX, oldY);
        }

        public void drawYourSelf(Graphics g)
        {
            g.FillRectangle(this.brush, figure);
        }

        public bool isIncludeThisPoint(int x, int y)
        {
            return figure.Contains(x, y);
        }

        public int getWidth() { return figure.Width; }
        public int getHeight() { return figure.Height; }
        public Point getLocation() { return figure.Location; }

        public Point getMiddlePoint()
        {
            return new Point(figure.X + figure.Width / 2, figure.Y + figure.Height / 2);
        }

        /// <summary>
        /// Сдивнуть центр прямоуголька по заданным коорднатам
        /// </summary>
        /// <param name="midX">Новая середина прямоугольника оп OX</param>
        /// <param name="midY">Новая середина прямоугольника оп OY</param>
        public void changeLocationByMiddlePosition(int midX, int midY)
        {
            figure.X = midX - figure.Width / 2;
            figure.Y = midY - figure.Height / 2;
        }

    }
}
