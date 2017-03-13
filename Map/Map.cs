using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolution_Simulator.Computing;

namespace Evolution_Simulator.Map
{
    class Map
    {
        public readonly Tile[,] MapArray;

        public Map(int sizeSide)
        {
            double[,,] perlin = new double[sizeSide, sizeSide, 5]; //5 = Temperature, Moisture, Water availability, Food availibility, Terrain
            for(int x = 0; x < sizeSide; x++)
                for(int y = 0; y < sizeSide; y++)
                {
                    //TODO tweak values
                    /*perlin[x, y, 0] = PerlinNoise.Noise(x, y, 0, 5, 1); //Temperature
                    perlin[x, y, 1] = PerlinNoise.Noise(x, y, 1, 5, 1); //Moisture
                    perlin[x, y, 2] = PerlinNoise.Noise(x, y, 2, 5, 1); //Water
                    perlin[x, y, 3] = PerlinNoise.Noise(x, y, 3, 5, 1); //Food
                    perlin[x, y, 4] = PerlinNoise.Noise(x, y, 4, 5, 1); //Terrain*/
                }
            MapArray = new Tile[sizeSide, sizeSide];
            for (int i = 0; i < sizeSide; i++)
                for (int j = 0; j < sizeSide; j++)
                    MapArray[i, j] = new Tile(perlin[i, j, 0], perlin[i, j, 1], perlin[i, j, 3], perlin[i, j, 2], perlin[i, j, 4]);
        }

        public void Tick()
        {
            Logger.Logger.Log("Ticking Map");
        }
    }
}
