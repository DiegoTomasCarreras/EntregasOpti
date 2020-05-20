using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolvesAndRabbitsSimulation.Simulation;

namespace WolvesAndRabbitsSimulation.Engine
{
   public class World
    {
        private Random rnd = new Random();

        private const int width = 255;
        private const int height = 255;
        private Size size = new Size(width, height);
        //private GameObject[] objects = new GameObject[0];
        public List<Grass> grasslist = new List<Grass>();
        public List<Rabbit> rabbitList= new List<Rabbit>();
       /* public IEnumerable<GameObject> GameObjects
        {
            get
            {
                return objects.ToArray(); //tengo que buscar donde modificar para devolver pasto donde se necesita y conejos donde se necesitan
            }
        }*/

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public float Random()
        {
            return (float)rnd.NextDouble();
        }

        public Point RandomPoint()
        {
            return new Point(rnd.Next(width), rnd.Next(height));
        }

        public int Random(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public void AddRabbit(Rabbit rb)
        {
            //objects = objects.Concat(new GameObject[] { obj }).ToArray();
            rabbitList.Add(rb);
        }
        public void AddGrass(Grass gr)
        {
            grasslist.Add(gr);
        }

        public void RemoveRabbit(Rabbit rb)
        {
            //objects = objects.Where(o => o != obj).ToArray();
            rabbitList.Remove(rb);
        }

        public virtual void Update()
        {
            foreach(Grass gr in grasslist)
                {
                gr.UpdateOn(this);
                }
            var r = rabbitList.Count();
            for(int cont=0;cont<r;cont++)
                {
                rabbitList[cont].UpdateOn(this);
                rabbitList[cont].Position = PositiveMod(rabbitList[cont].Position, size);
                }
        }

        public virtual void DrawOn(Graphics graphics)
        {
            foreach(Grass gr in grasslist)
                {
                graphics.FillRectangle(new Pen(gr.Color).Brush, gr.Bounds);
                }
            foreach(Rabbit rb in rabbitList)
                {
                graphics.FillRectangle(new Pen(rb.Color).Brush, rb.Bounds);
                }
        }

        // http://stackoverflow.com/a/10065670/4357302
        private static int PositiveMod(int a, int n)
        {
            int result = a % n;
            if ((a < 0 && n > 0) || (a > 0 && n < 0))
                result += n;
            return result;
        }
        private static Point PositiveMod(Point p, Size s)
        {
            return new Point(PositiveMod(p.X, s.Width), PositiveMod(p.Y, s.Height));
        }

        public double Dist(PointF a, PointF b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public IEnumerable<GameObject> ObjectsAt(Point pos)
        {
            return grasslist.Where(each =>
            {
                Rectangle bounds = each.Bounds;
                PointF center = new PointF((bounds.Left + bounds.Right - 1) / 2.0f,
                                           (bounds.Top + bounds.Bottom - 1) / 2.0f);
                return Dist(pos, center) <= bounds.Width / 2.0f
                    && Dist(pos, center) <= bounds.Height / 2.0f;
            });
        }
    }
}
