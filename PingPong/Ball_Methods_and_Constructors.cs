﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace PingPong
{
    partial class Ball
    {
        public Ball(int radius, int coordOfCenterX, 
            int coordOfCenterY, int stepX, int stepY, 
            System.Drawing.Color color)
        {
            this.Radius = radius;
            this.CoordOfCenterX = coordOfCenterX;
            this.CoordOfCenterY = coordOfCenterY;
            this.Steps.stepX = stepX;
            this.Steps.stepY = stepY;
            this.ColorOfBall = color;
        }

        /*
         * Рассчёты базируются на формуле попадания точки в окружность: (x - x0)^2 + (y - y0)^2 <= R^2
         * где x0, y0 - координаты центра, r - радиус,
         * x, y - координаты интересующей точки
         */
        public bool isTouchBorders(int x, int y)
        {
            int x0 = this.CoordOfCenterX;
            int y0 = this.CoordOfCenterY;
            int r = this.Radius;
            return (Math.Pow((x - x0), 2) + Math.Pow((y - y0), 2) <= r * r);
        }
        
     }
}
