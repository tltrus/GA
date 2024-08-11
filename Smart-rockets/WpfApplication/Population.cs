using System.Collections.Generic;
using System.Windows.Media;

namespace WpfApplication46
{
    class Population
    {
        List<Rocket> rockets = new List<Rocket>(); // Array of rockets
        List<Rocket> matingpool = new List<Rocket>(); // Amount parent rocket partners
        int rockets_num = 25; // Amount rockets

        public Population()
        {
            // Associates a rocket to an array index
            for (var i = 0; i < rockets_num; i++)
            {
                rockets.Add(new Rocket(null));
            }
        }

        public void Evaluate()
        {
            double maxfit = 0;
            // Iterate through all rockets and calcultes their fitness
            for (var i = 0; i < rockets_num; i++)
            {
                // Calculates fitness
                rockets[i].CalcFitness();
                // If current fitness is greater than max, then make max equal to current
                if (rockets[i].fitness > maxfit)
                {
                    maxfit = rockets[i].fitness;
                }
            }
            // Normalises fitnesses
            for (var i = 0; i < rockets_num; i++)
            {
                rockets[i].fitness /= maxfit;
            }

            matingpool.Clear();
            // Take rockets fitness make in to scale of 1 to 100
            // A rocket with high fitness will highly likely will be in the mating pool
            for (var i = 0; i < rockets_num; i++)
            {
                var n = rockets[i].fitness * 100;
                for (var j = 0; j < n; j++)
                {
                    matingpool.Add(rockets[i]);
                }
            }
        }

        public void Selection()
        {
            List<Rocket> newRockets = new List<Rocket>();

            for (var i = 0; i < rockets.Count; i++)
            {
                // Picks random dna
                var parentA = Random(matingpool).dna;
                var parentB = Random(matingpool).dna;
                // Creates child by using crossover function
                var child = parentA.Crossover(parentB);
                child.Mutation();
                // Creates new rocket with child dna
                newRockets.Add(new Rocket(child));
            }
            // This instance of rockets are the new rockets
            //this.rockets = newRockets;
            CopyTo(newRockets, rockets);
        }

        public void Run(DrawingContext dc)
        {
            foreach(var rocket in rockets)
            {
                rocket.Update();
                rocket.Show(dc);
            }
        }

        private Rocket Random(List<Rocket> rockets)
        {
            var index = MainWindow.rnd.Next(rockets.Count);
            return rockets[index];
        }

        private void CopyTo(List<Rocket> source, List<Rocket> dest)
        {
            for(int i = 0; i < source.Count; ++i)
            {
                dest[i] = source[i];
            }
        }
    }
}
