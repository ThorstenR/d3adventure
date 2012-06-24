using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using D3_Adventures;
using D3_Adventures.Enumerations;
using D3_Adventures.Structures;

namespace Maphack
{
    public partial class Form1 : Form
    {
        public System.Drawing.Graphics formGraphics;
        public System.Drawing.Graphics formGraphics1;
        public Form1()
        {
            InitializeComponent();
            formGraphics = this.CreateGraphics();
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                updateMap();
                Thread.Sleep(10);
            }
        }

        private void drawDot(int x, int y, String col)
        {
            if (col.Equals("black"))
            {
                this.formGraphics.DrawRectangle(Pens.Black, x, y, 3, 3);
                this.formGraphics.FillRectangle(Brushes.Black, x, y, 3, 3);
            }
            if (col.Equals("red"))
            {
                this.formGraphics.DrawRectangle(Pens.Red, x, y, 3, 3);
                this.formGraphics.FillRectangle(Brushes.Red, x, y, 3, 3);
            }
            if (col.Equals("blue"))
            {
                this.formGraphics.DrawRectangle(Pens.Blue, x, y, 3, 3);
                this.formGraphics.FillRectangle(Brushes.Blue, x, y, 3, 3);
            }
        }

        private void updateMap()
        {

            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
            //formGraphics = this.CreateGraphics();

            Actor[] _monsters;
            _monsters = Data.getMapItems();


            //formGraphics.DrawLine(myPen, 0, 0, 2000, 2000);
            int centrum_x = this.Size.Width / 2;
            int centrum_y = this.Size.Height / 2;
 
            this.formGraphics.Clear(Color.Gray);
            drawDot(centrum_x, centrum_y, "black");

            foreach (Actor actor in _monsters)
            {
                int x = (int)((actor.Pos1.x - Data.GetMe().Pos1.x));
                int y = (int)((actor.Pos1.y - Data.GetMe().Pos1.y));
                if (actor.Alive == -1)
                {
                    drawDot(centrum_x - x, centrum_y + y, "blue");
                }
                else if(actor.Alive == 0)
                {
                    drawDot(centrum_x - x, centrum_y + y, "red");
                }
            }
            myPen.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
