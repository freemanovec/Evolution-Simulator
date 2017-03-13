using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulator.Computing
{
    static class Mathf
    {
        public static readonly double Sqrt2 = Math.Sqrt(2);
        public static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x));
        }
        public static double Tanh(double x)
        {
            return Math.Tanh(x);
        }
        public static double Lerp(double a, double b, double val)
        {
            return (1d - val) * a + val * b;
        }
        public static double Gradient(int hash, double x, double y, double z)
        {
            int h = hash % 15;
            double u = h < 8 ? x : y;
            double v = z;
            if (h < 4)
                v = y;
            else if (h == 12 || h == 14)
                v = x;
            return ((h & 1) == 0 ? u : -u) + ((h % 2) == 0 ? v : -v);
        }
        public static double Fade(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }
        public static Tuple<double, double> Normalize(Tuple<double, double> input)
        {
            double magnitude = Math.Sqrt(input.Item1 * input.Item1 + input.Item2 * input.Item2);
            return new Tuple<double, double>(input.Item1 / magnitude, input.Item2 / magnitude);
        }
    }
}
