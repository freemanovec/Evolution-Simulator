using Evolution_Simulator.Computing;
using Evolution_Simulator.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            int octaves = 1;
            double frequency = .35d;
            double lacunarity = 1.83d;
            double persistence = .32d;
            int size = 100;
            double[,] perlinBase = new double[size, size];
            double[,] perlinFood = new double[size, size];
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    double perlinBaseValue = PerlinNoise.Noise(x, y, frequency, octaves, lacunarity, persistence);
                    perlinBase[x, y] = perlinBaseValue;
                    double perlinFoodValue = ((perlinBaseValue - .5d) / 2) + perlinBaseValue;
                    perlinFood[x, y] = perlinFoodValue;
                }
            BitmapPlotter plotterBase = new BitmapPlotter(@"test\base.png");
            BitmapPlotter plotterFood = new BitmapPlotter(@"test\food.png");
            plotterBase.Plot(perlinBase);
            plotterFood.Plot(perlinFood);
        }
    }
}
