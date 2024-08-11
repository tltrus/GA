using System.Windows;
using System.Windows.Media;

namespace WpfApplication46
{
    class Rocket
    {
        Vector2D pos, vel, acc;
        public DNA dna;
        public double fitness;
        bool isCrashed;   // Checks if rocket had crashed
        bool isCompleted; // Checkes rocket has reached target

        public Rocket(DNA dna)
        {
            if (dna == null)
                this.dna = new DNA(null);
            else
                this.dna = dna;
          
            pos = new Vector2D(MainWindow.width / 2, MainWindow.height - 10);
            vel = new Vector2D();
            acc = new Vector2D();
        }

        public void ApplyForce(Vector2D force) => acc.Add(force);

        public void CalcFitness()
        {
            // Takes distance to target
            var d = Vector2D.Dist(pos, MainWindow.target);

            // Maps range of fitness
            fitness = Tools.Map(d, 0, MainWindow.width, MainWindow.width, 0);
            // If rocket gets to target increase fitness of rocket
            if (isCompleted)
            {
                fitness *= 10;
            }
            // If rocket does not get to target decrease fitness
            if (isCrashed)
            {
                fitness /= 10;
            }
        }

        public void Update()
        {
            // Checks distance from rocket to target
            var d = Vector2D.Dist(pos, MainWindow.target);
            // If distance less than 10 pixels, then it has reached target
            if (d < 10)
            {
                isCompleted = true;
                pos = MainWindow.target.CopyToVector();
            }

            // Rocket hit the barrier
            if (
              pos.X > MainWindow.rect.X &&
              pos.X < MainWindow.rect.X + MainWindow.rect.Width &&
              pos.Y > MainWindow.rect.Y &&
              pos.Y < MainWindow.rect.Y + MainWindow.rect.Height
            )
            {
                isCrashed = true;
            }
            // Rocket has hit left or right of window
            if (pos.X > MainWindow.width || pos.X < 0)
            {
                isCrashed = true;
            }
            // Rocket has hit top or bottom of window
            if (pos.Y > MainWindow.height || pos.Y < 0)
            {
                isCrashed = true;
            }

            ApplyForce(dna.genes[MainWindow.count]);

            // if rocket has not got to goal and not crashed then update physics engine
            if (!isCompleted && !isCrashed)
            {
                vel.Add(acc);
                pos.Add(vel);
                acc.Mult(0);
                vel.Limit(4);
            }
        }

        public void Show(DrawingContext dc) => dc.DrawEllipse(Brushes.Red, null, new Point(pos.X, pos.Y), 2, 2);
    }
}
