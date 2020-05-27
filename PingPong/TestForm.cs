using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace PingPong
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(new Pen(System.Drawing.Color.Red),
                new Point(0, 0), 
                new Point(this.panel1.Width, this.panel1.Height));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Tick-tick-tack!");
            panel1.Invalidate();
        }

        /*
         * Запуск консоли для Debug-штук
         */
        private void Form2_Load(object sender, EventArgs e)
        {
            AllocConsole();
            timer1.Start();
            timer1.Interval = 1000;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
