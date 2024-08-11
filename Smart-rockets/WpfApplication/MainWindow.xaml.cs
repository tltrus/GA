using System;
using System.Windows;
using System.Windows.Media;

namespace WpfApplication46
{
    /// #29 — Smart Rockets in p5.js https://thecodingtrain.com/challenges/29-smart-rockets-in-p5js

    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public static Random rnd = new Random();
        DrawingVisual visual;
        DrawingContext dc;
        public static double width, height;

        public static Vector2D target;
        public static int lifespan = 400; // of rocket
        public static int count;

        public static Rect rect;
        Population population;

        public MainWindow()
        {
            InitializeComponent();
            
            width = g.Width;
            height = g.Height;

            visual = new DrawingVisual();

            rect = new Rect(new Point(100, 150), new Point(300, 160));  // wall

            target = new Vector2D(width / 2, 50);
            population = new Population();

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e) => Draw();

        void Draw()
        {
            lbCount.Content = count.ToString();
            
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                // wall
                dc.DrawRectangle(Brushes.White, null, rect);

                // target
                var p = new Point(target.X, target.Y);
                dc.DrawEllipse(Brushes.Green, null, p, 16, 16);

                // Rockets
                population.Run(dc);

                count++;
                if (count == lifespan)
                {
                    population.Evaluate();
                    population.Selection();
                    count = 0;
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }
    }
}
