using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Content;
using D3_Adventures;
using Utilities;
using System.Collections.Generic;
using D3_Adventures.Structures;



namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        // Directx graphics device
        GraphicsDevice dev = null;
        BasicEffect effect = null;
        public ContentManager content;
        public int width;
        public int height;
        Vector2 minimapCenter;
        public List<Vector2> plot_obj;
        Utilities.FollowWindow.FW follower;
        public Texture2D fileTexture;

        // Wheel vertexes
        VertexPositionColor[] v = new VertexPositionColor[100];

        public Form1()
        {

            InitializeComponent();
            plot_obj = new List<Vector2>();
            StartPosition = FormStartPosition.CenterScreen;

            follower = new Utilities.FollowWindow.FW(this.Handle, Globals.winHandle, false);
            follower.Start();

            Utilities.WinControl.RECT rect;
            Utilities.WinControl.WC.GetWindowRect(Globals.winHandle, out rect);
            this.width = rect.Width;
            this.height = rect.Height;





            Size = new System.Drawing.Size(this.width, this.height);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;  // no borders

            TopMost = true;        // make the form allways on top                     
            Visible = true;        // Important! if this isn't set, then the form is not shown at all

            // Set the form click-through
            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);

            // Create device presentation parameters
            PresentationParameters p = new PresentationParameters();
            p.IsFullScreen = false;
            p.DeviceWindowHandle = this.Handle;
            p.BackBufferFormat = SurfaceFormat.Vector4;
            p.PresentationInterval = PresentInterval.One;

            // Create XNA graphics device
            dev = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.Reach, p);
            // Init basic effect
            effect = new BasicEffect(dev);

            // Extend aero glass style on form init
            OnResize(null);

            minimapCenter = CalcMinimapCenter();

            
            using (FileStream fileStream = new FileStream(@"..\..\icons\red_icon.png", FileMode.Open))
            {
                fileTexture = Texture2D.FromStream(dev, fileStream);
            }

        }
        // this only work for fullscreen window - cant test it whit the set i have, so might not work of other then 1920* 1080
        public Vector2 CalcMinimapCenter()
        {
            float x = 0.903f * (float)this.width;// - 24.847f;
            float y = 0.1870f * this.height;// + 23.105f;
            return new Vector2(x, y);

        }
        
        // for now only works for 1920*1080
        public Vector2 CalcMinimapPos(double dx, double dy, double dyx)
        {
            Vector2 centrum = this.CalcMinimapCenter();
            /*
            //convert to diablo coordinats - rotating
           25
            double theta = Math.Asin(dy / dyx);
            if (theta < 0)
            {
                theta += Math.PI * 2;
            }
             * */
            float K = 0.65f;//0.7854f;
            dx = (float)(Math.Cos(K) * dx - dy * Math.Sin(K));
            dy = (float)(Math.Sin(K) * dx + dy * Math.Cos(K));
            
            
            Vector2 result = new Vector2();
            result.X = (float)(centrum.X + 1 * 0.8 * dx);
            result.Y = (float)(centrum.Y - 1 * 1.58 * dy);
            
            



            return result;

        }

        protected override void OnResize(EventArgs e)
        {
            int[] margins = new int[] { 0, 0, Width, Height };

            // Extend aero glass style to whole form
            DwmExtendFrameIntoClientArea(this.Handle, ref margins);
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // do nothing here to stop window normal background painting
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            dev.Clear(new Microsoft.Xna.Framework.Color(0, 0, 0, 0.0f));



            //Stream file = File.Open("icons\\tux.png", FileMode.Open);
            //Texture2D imageTexture = Texture2D.FromStream(dev, file);
            PresentationParameters pp = dev.PresentationParameters;
            SpriteBatch spriteBatch = new SpriteBatch(dev);
            Vector2 position = new Vector2(0, 0);

            double me_X = Data.GetMe().Pos1.x;
            double me_Y = Data.GetMe().Pos1.y;
            Actor[] actors = Data.GetMapItems();
            this.plot_obj.Clear();
            foreach (Actor actor in actors)
            {

                double dx = (me_X - actor.Pos1.x);
                double dy =  (me_Y - actor.Pos1.y);
                if (Math.Abs(dx) < 200 && Math.Abs(dy) < 200)
                {
                    Vector2 fiks = this.CalcMinimapPos(dx, dy, actor.distanceFromMe);
                    this.plot_obj.Add(fiks);
                }
            }
            spriteBatch.Begin();
            foreach (Vector2 vec in this.plot_obj)
            {
                spriteBatch.Draw(fileTexture, vec, Color.White);
            }

            

            spriteBatch.Draw(fileTexture, minimapCenter, Color.White);
            spriteBatch.End();
            dev.Present();
            Invalidate();
        }


            /*
        protected override void OnPaint(PaintEventArgs e)
        {
            // Clear device with fully transparent black
            dev.Clear(new Microsoft.Xna.Framework.Color(0, 0, 0, 0.0f));

            // Rotate wheel a bit
            rot += 0.1f;

            // Make the wheel vertexes and colors for vertexes
            for (int i = 0; i < v.Length; i++)
            {
                if (i % 3 == 1)
                    v[i].Position = new Microsoft.Xna.Framework.Vector3((float)Math.Sin((i + rot) * (Math.PI * 2f / (float)v.Length)), (float)Math.Cos((i + rot) * (Math.PI * 2f / (float)v.Length)), 0);
                else if (i % 3 == 2)
                    v[i].Position = new Microsoft.Xna.Framework.Vector3((float)Math.Sin((i + 2 + rot) * (Math.PI * 2f / (float)v.Length)), (float)Math.Cos((i + 2 + rot) * (Math.PI * 2f / (float)v.Length)), 0);

                v[i].Color = new Microsoft.Xna.Framework.Color(1 - (i / (float)v.Length), i / (float)v.Length, 0, i / (float)v.Length);
            }

            // Enable position colored vertex rendering
            effect.VertexColorEnabled = true;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes) pass.Apply();

            // Draw the primitives (the wheel)
            dev.DrawUserPrimitives(PrimitiveType.TriangleList, v, 0, v.Length / 3, VertexPositionColor.VertexDeclaration);

            // Present the device contents into form
            dev.Present();

            // Redraw immediatily
            Invalidate();
        }
            */

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("dwmapi.dll")]
        static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref int[] pMargins);

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}