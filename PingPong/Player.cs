using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PingPong
{
    class Player
    {
        private Rectangle figure;
        private SolidBrush brush;
        private System.Drawing.Color color;
        public System.Drawing.Color Color { 
            get { return this.color; }
            set 
            {
                this.color = value;
                brush = new SolidBrush(value);
            } 
        }

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

        // Вычисляет x и y угловые точки прямоугольника по заданной середине прямоугольника
        public void changeLocationByMiddlePosition(int midX, int midY)
        {
            figure.X = midX - figure.Width / 2;
            figure.Y = midY - figure.Height / 2;
        }

    }
}
