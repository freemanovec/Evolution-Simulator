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
            /*
             *We need:
             *    Height
             *    Temperature
             *    Food
             */

            double[,] noiseBase = new PerlinNoise(.015d, 6, 1.83d, .32d).NoiseArray(100);
            double[,] noiseFood = new PerlinNoise(.012d, 3, 1.83d, .32d).NoiseArray(100);
            double[,] noiseTemp = new PerlinNoise(.008d, 3, 1.83d, .32d).NoiseArray(100);

            noiseBase = Mathf.ArrayClamp(noiseBase, .2d, .8d);
            noiseFood = Mathf.ArrayClamp(noiseFood, .05d, .5d);
            noiseTemp = Mathf.ArrayClamp(noiseTemp, .25d, .75d);

            double[,] noiseTemporal0 = Mathf.ArrayClamp(noiseBase, 0, 1);
            double[,] noiseTemporal1 = Mathf.ArrayClamp(noiseTemp, 0, 1);
            double[,] noiseTemporal2 = Mathf.ArrayAverage(noiseTemporal0, noiseTemporal1);
            double[,] noiseTemporal3 = Mathf.ArrayClamp(noiseTemporal2, 0.15d, 0.95d);

            noiseFood = noiseTemporal3;

            //noiseFood = Mathf.ArrayMerge(noiseBase, noiseFood);

            new BitmapPlotter(@"test\base.png").Plot(noiseBase);
            new BitmapPlotter(@"test\food.png").Plot(noiseFood);
            new BitmapPlotter(@"test\temp.png").Plot(noiseTemp);
        }
    }
}
