using System;
using System.Collections.Generic;

namespace WpfApplication46
{
    class DNA
    {
        public List<Vector2D> genes = new List<Vector2D>();

        public DNA(List<Vector2D> genes)
        {
            if (genes != null)
            {
                this.genes = genes;
            }
            else
            {
                for (var i = 0; i < MainWindow.lifespan; i++)
                {
                    // Gives random vectors
                    this.genes.Add(Vector2D.Random2D(MainWindow.rnd));
                    // Sets maximum force of vector to be applied to a rocket
                    this.genes[i].SetMag(0.2);
                }
            }
        }

        public DNA Crossover(DNA partner)
        {
            List<Vector2D> newgenes = new List<Vector2D>();
            // Picks random midpoint
            var mid = Math.Floor((decimal)MainWindow.rnd.Next(genes.Count));
            for (var i = 0; i < genes.Count; i++)
            {
                // If i is greater than mid the new gene should come from this partner
                if (i > mid)
                {
                    newgenes.Add(genes[i]);
                }
                // If i < mid new gene should come from other partners gene's
                else
                {
                    newgenes.Add(partner.genes[i]);
                }
            }
            // Gives DNA object an array
            return new DNA(newgenes);
        }

        public void Mutation()
        {
            for (var i = 0; i < genes.Count; i++)
            {
                if (MainWindow.rnd.NextDouble() < 0.01)
                {
                    genes[i] = Vector2D.Random2D(MainWindow.rnd);
                    genes[i].SetMag(0.2);
                }
            }
        }
    }
}
