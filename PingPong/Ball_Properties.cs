﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPong
{
    /// <summary>
    /// Часть класса мяча, включающая поля и свойства
    /// </summary>
    /// <remarks>
    /// Имеет: координаты середины мяча, радиус мяча, цвет, кисть, кортеж из сдвигов мяча при прижении.
    /// </remarks>
    partial class Ball
    {
        private int radius;
        private int coordOfCenterX;
        private int coordOfCenterY;
        private System.Drawing.Color color;
        private SolidBrush brush;
        private Pen pen;
        public System.Drawing.Color ColorOfBall
        {
            get { return this.color; }
            set 
            {
                this.color = value;
                brush = new SolidBrush(value);
            }
        }

        public (int stepX, int stepY) Steps;

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
            }
        }

        public int CoordOfCenterY
        {
            get { return this.coordOfCenterY; }
            set
            {
                if (value >= 0) this.coordOfCenterY = value;
            }
        }
    }
}
