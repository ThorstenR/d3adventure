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
        Bitmap BackBuffer;
        Utilities.FollowWindow.FW follower;

        public Form1()
        {
            InitializeComponent();
            formGraphics = this.CreateGraphics();
            backgroundWorker1.RunWorkerAsync();
            follower = new Utilities.FollowWindow.FW(this.Handle, Globals.winHandle, true);
            follower.Start();

            Utilities.WinControl.RECT rect;
            Utilities.WinControl.WC.GetWindowRect(Globals.winHandle, out rect);
            this.Width = rect.Width;
            this.Height = rect.Height;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                updateMapV2();
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

        // draw directly to the bitmap via reference
        private void drawDotBuffer(int x, int y, String col, ref Bitmap buffer)
        {
            drawRectBuffer(x-1, y-1, 3, 3, col, ref buffer); // centered on point
        }

        private void drawRectBuffer(int x, int y, int width, int height, string col, ref Bitmap buffer)
        {
            Graphics g = Graphics.FromImage(buffer);
            Color c = Color.FromName(col);
            Pen p = new Pen(c);
            Brush b = new SolidBrush(c);
            g.DrawRectangle(p, x, y, width, height);
            g.FillRectangle(b, x, y, width, height);
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
                else if (actor.Alive == 0)
                {
                    drawDot(centrum_x - x, centrum_y + y, "red");
                }
            }
            myPen.Dispose();
        }

        private void updateMapV2()
        {
            BackBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            drawRectBuffer(0, 0, ClientSize.Width, ClientSize.Height, "Gray", ref BackBuffer);

            Actor[] _monsters;
            _monsters = Data.getMapItems();//getMapItems();

            foreach (Actor actor in _monsters)
            {
                PointF pf = D3_Adventures.GameUtilities.FromD3toScreenCoords(actor.Pos1, ClientSize.Width, ClientSize.Height);
                int x = (int)Math.Round(pf.X, 0);
                int y = (int)Math.Round(pf.Y, 0);

                if (x == 0 && y == 0) // false point
                    return;
                else if (actor.id_acd == Data.toonID)
                    drawDotBuffer(x, y, "LawnGreen", ref BackBuffer);
                else if (actor.Alive == -1)
                    drawDotBuffer(x, y, "blue", ref BackBuffer);
                else if(actor.Alive == 0)
                    drawDotBuffer(x, y, "red", ref BackBuffer);
            }

            try
            {
                this.formGraphics.DrawImage(BackBuffer, 0, 0);
            }
            catch { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            follower.Start();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            formGraphics = this.CreateGraphics();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            follower.Toggle(false);
            follower.Follow.Suspend();
        }

    }
}
