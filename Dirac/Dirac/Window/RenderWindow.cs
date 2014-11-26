using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Dirac.GameServer;
using Dirac;
using Rectangle = System.Drawing.Rectangle;

namespace Dirac.Window
{
    public partial class RenderWindow : UserControl
    {
        public Boolean EnableDrawWorld { get; set; }
        public RenderWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.EnableDrawWorld = false;
        }

        private void RenderWindow_Load(object sender, EventArgs e)
        {

        }

        private void RenderWindow_Paint(object sender, PaintEventArgs e)
        {
            /*List<Actor> actors = WorldVisualizer.WorldVisualizer.Instance.DataToDraw();
            foreach (var actor in actors)
            {
                //Size size = new System.Drawing.Size(1, 1);
                Point result = new Point(center.X + ((int)actor.Position.x * 2), center.Y + ((int)actor.Position.z * 2));
                if (actor.ActorType == ActorType.Player)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Blue), new Rectangle(result, new Size(10, 10)));
                }
                if (actor.ActorType == ActorType.Monster)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), new Rectangle(result, new Size(5, 5)));
                }
                if (actor.ActorType == ActorType.Projectile)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Green), new Rectangle(result, new Size(2, 2)));
                }
            }*/
        }

        private Stopwatch sw = new Stopwatch();
        private TimeSpan oldts;
        Font font = new Font(FontFamily.Families[9], 16, FontStyle.Regular);
        protected override void OnPaint(PaintEventArgs e)
        {
            //Font font = new Font(FontFamily.Families[9], 16, FontStyle.Regular);
            //e.Graphics.DrawString("AAAAAAAAAA", font, Brushes.Blue, new PointF(10, 10));
            if (this.EnableDrawWorld)
            {
                /*e.Graphics.DrawString("Enabled", font, Brushes.DarkRed, new PointF(10, 40));

                List<Actor> actors = WorldVisualizer.WorldVisualizer.Instance.DataToDraw();
                foreach (var actor in actors)
                {
                    Point center = new Point(200, 200);
                    Point result = new Point(center.X + ((int)actor.Position.x), center.Y + ((int)actor.Position.z));
                    if (actor.ActorType == ActorType.Player)
                    {
                        e.Graphics.FillEllipse(new SolidBrush(Color.Blue), new Rectangle(result, new Size(20, 20)));
                    }
                    if (actor.ActorType == ActorType.Monster)
                    {
                        e.Graphics.FillEllipse(new SolidBrush(Color.Red), new Rectangle(result, new Size(15, 15)));
                    }
                    if (actor.ActorType == ActorType.Projectile)
                    {
                        e.Graphics.FillEllipse(new SolidBrush(Color.Green), new Rectangle(result, new Size(3, 3)));
                    }
                }*/
            }

            oldts = sw.Elapsed;
            //e.Graphics.DrawString("ms : " + oldts.TotalMilliseconds.ToString(), font, Brushes.DarkRed, new PointF(10, 80));
            //e.Graphics.DrawString("fps : " + ((float)1 / (float)oldts.TotalSeconds).ToString(), font, Brushes.DarkRed, new PointF(10, 120));
            sw.Restart();
            base.OnPaint(e);
        }

    }
}
