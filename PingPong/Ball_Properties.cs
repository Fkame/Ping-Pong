using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPong
{
    partial class Ball
    {
        private int radius;
        private int coordOfCenterX;
        private int coordOfCenterY;
        public System.Drawing.Color ColorOfBall { get; set; }
        
        // Количество пикселей в секунду
       // public int Speed { get; set; }

        public (int stepX, int stepY) steps;

        public int Radius
        {
            get { return this.radius; }
            set
            {
                if (value > 0) this.radius = value;
                else throw new ArgumentException("No-no-no, not lower that 0");
            }
        }

        public int CoordOfCenterX
        {
            get { return this.coordOfCenterX; }
            set
            {
                if (value >= 0) this.coordOfCenterX = value;
                else throw new ArgumentException("No-no-no, not lower that 0");
            }
        }

        public int CoordOfCenterY
        {
            get { return this.coordOfCenterY; }
            set
            {
                if (value >= 0) this.coordOfCenterY = value;
                else throw new ArgumentException("No-no-no, not lower that 0");
            }
        }
    }
}
