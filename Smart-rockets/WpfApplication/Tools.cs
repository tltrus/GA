using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication46
{
    internal class Tools
    {
        /// <summary>
        /// Constrains a value between a minimum and maximum value
        /// </summary>
        /// <param name="n">number to constrain</param>
        /// <param name="low">low minimum limit</param>
        /// <param name="high">high maximum limit</param>
        /// <returns>constrained number</returns>
        public static double Constrain(double n, double low, double high) => Math.Max(Math.Min(n, high), low);

        /// <summary>
        /// Re-maps a number from one range to another
        /// </summary>
        /// <param name="n">value  the incoming value to be converted</param>
        /// <param name="start1">start1 lower bound of the value's current range</param>
        /// <param name="stop1">stop1  upper bound of the value's current range</param>
        /// <param name="start2">start2 lower bound of the value's target range</param>
        /// <param name="stop2">stop2  upper bound of the value's target range</param>
        /// <param name="withinBounds">[withinBounds] constrain the value to the newly mapped range</param>
        /// <returns></returns>
        public static double Map(double n, double start1, double stop1, double start2, double stop2, bool withinBounds = false)
        {
            var newval = (n - start1) / (stop1 - start1) * (stop2 - start2) + start2;
            if (!withinBounds)
            {
                return newval;
            }
            if (start2 < stop2)
            {
                return Constrain(newval, start2, stop2);
            }
            else
            {
                return Constrain(newval, stop2, start2);
            }
        }
    }
}
