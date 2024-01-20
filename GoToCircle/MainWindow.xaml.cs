using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace GA
{
    class Target
    {
        public Point pos;
        public int size = 20;
        public Color color = Colors.Green;

        public Target(Point pos)
        {
            this.pos = pos;
        }
    }

    class Gen
    {
        public Point pos;
        public int dist;
        public double fitness;
        public int size = 3;
        public Color color = Colors.Red;

        public Gen(Point pos)
        {
            this.pos = pos;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        double width, height;
        System.Windows.Threading.DispatcherTimer timer;
        Gen[] P;
        Target target;

        int POPULATION = 20; //
        double GA_ELITRATE = 0.3;

        DrawingVisual visual;
        DrawingContext dc;

        Brush target_brush, parts_brush;


        public MainWindow()
        {
            InitializeComponent();

            width = g.Width; height = g.Height;
            visual = new DrawingVisual();

            P = new Gen[POPULATION]; // Популяция 

            // Таймер
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);

            // Инициализация
            Init();
            // Отрисовка
            Paint();
        }

        void Init()
        {
            parts_brush = Brushes.Yellow;
            target_brush = Brushes.Red;

            // target
            target = new Target(new Point(width - 50, height / 2));

            // population
            for (int i = 0; i < POPULATION; ++i)
            {
                Point pos = new Point(rnd.Next(10, (int)width / 2), rnd.Next(10, (int)height - 10));
                Gen gen = new Gen(pos);
                gen.dist = CalculateDist(pos, target.pos);
                P[i] = gen;
            }
        }


        // Отрисовка процесса
        void Paint()
        {
            g.RemoveVisual(visual);

            using (dc = visual.RenderOpen())
            {
                // target
                dc.DrawEllipse(target_brush, null, target.pos, target.size, target.size);

                // population
                foreach (var p in P)
                {
                    dc.DrawEllipse(parts_brush, null, p.pos, p.size, p.size);
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }


        // Управление 
        void Control()
        {
            calc_fitness();
            sort_by_fitness();
            Paint();

            Crossover();
            Mutate();
        }

        // *********************************

        void calc_fitness()
        {
            // population
            for (int i = 0; i < POPULATION; ++i)
            {
                P[i].dist = CalculateDist(P[i].pos, target.pos);
            }
        }

        // distance calculation between 2 points
        int CalculateDist(Point a, Point b) => (int)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));

        void sort_by_fitness() => Array.Sort(P, new ChromosomeComparer()); // Используется интерфейс IComparer

        public sealed class ChromosomeComparer : IComparer
        {
            public int Compare(object a, object b) // a - первый по порядку член популяции; b - следующий за ним член
            {
                if (!(a is Gen) || !(b is Gen))
                    throw new ArgumentException("Not of type Chromosome");
                if (((Gen)a).dist > ((Gen)b).dist)
                    return 1;
                else if (((Gen)a).dist == ((Gen)b).dist)
                    return 0;
                else
                    return -1;
            }
        }

        void Crossover()
        {
            int esize = (int)(POPULATION * GA_ELITRATE); // рассичтываем количество элиты, которую не будем скрещивать

            for (int k = esize; k < POPULATION / 2; k++)
            {
                int a = 2 * k;
                int b = 2 * k + 1;
                Swap(ref P[a].pos, ref P[b].pos);
            }
        }

        void Swap(ref Point a, ref Point b) 
        {
            Point c = new Point();
            c.X = a.X; 
            a.X = b.X; 
            b.X = c.X; 
        }

        void Mutate()
        {
            double pmut = 0.05;

            int esize = (int)(POPULATION * GA_ELITRATE); // рассичтываем количество элиты, которую не будем скрещивать

            for (int k = esize; k < POPULATION; k++)
                if (rnd.NextDouble() < pmut)
                {
                    int x = rnd.Next((int)P[0].pos.X - 5, (int)P[0].pos.X + 5);
                    int y = rnd.Next((int)P[0].pos.Y - 5, (int)P[0].pos.Y + 5);
                    Point pos = new Point(x, y);
                    ((Gen)P[k]).pos = pos;
                }
        }

        // *********************************

        private void timerTick(object sender, EventArgs e) => Control();

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
                timer.Stop();
            else
            {
                Init();
                timer.Start();
            }
        }

        private void BtnStep_Click(object sender, RoutedEventArgs e) => Control();
    }
}
