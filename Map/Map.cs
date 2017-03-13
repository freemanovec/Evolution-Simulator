using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolution_Simulator.Computing;
using Evolution_Simulator.Organism;

namespace Evolution_Simulator.Map
{
    class Map
    {
        private readonly Tile[,] _tiles;

        public Map(int sizeSide)
        {
            _tiles = new Tile[sizeSide, sizeSide];

            Generate(sizeSide);
        }

        private void Generate(int sizeSide)
        {
            
            double[,] noiseBase = new PerlinNoise(.015d, 6, 1.83d, .32d).NoiseArray(sizeSide);
            double[,] noiseTemp = new PerlinNoise(.008d, 3, 1.83d, .32d).NoiseArray(sizeSide);
            double[,] noiseFood = new PerlinNoise(.012d, 3, 1.83d, .32d).NoiseArray(sizeSide);

            noiseBase = Mathf.ArrayClamp(noiseBase, .2d, .8d);
            noiseTemp = Mathf.ArrayClamp(noiseTemp, .25d, .75d);
            noiseFood = Mathf.ArrayClamp(noiseFood, .05d, .5d);

            double[,] noiseTemporal0 = Mathf.ArrayClamp(noiseBase, 0, 1);
            double[,] noiseTemporal1 = Mathf.ArrayClamp(noiseTemp, 0, 1);
            double[,] noiseTemporal2 = Mathf.ArrayAverage(noiseTemporal0, noiseTemporal1);
            noiseFood = Mathf.ArrayClamp(noiseTemporal2, .15d, .95d);
            
            for(int i = 0; i < sizeSide; i++)
                for(int j = 0; j < sizeSide; j++)
                {
                    _tiles[i, j] = new Tile(noiseTemp[i, j], noiseFood[i, j], noiseBase[i, j]);
                }
        }

        public void Populate(int groups, int groupSize)
        {
            Random rand = new Random();
            int[] size = { _tiles.GetLength(0), _tiles.GetLength(1) };
            for(int i = 0; i < groups; i++)
            {
                int[] coords = { rand.Next(size[0]), rand.Next(size[1]) };
                for (int j = 0; j < groupSize; j++)
                {
                    Cell cell = new Cell(0.05d, 0.05d);
                    _tiles[coords[0], coords[1]].AddCell(cell);
                }
            }
        }

        public void Tick()
        {
            Logger.Logger.Log("Ticking Map");
            foreach (Tile tile in _tiles)
                tile.Tick();
            Logger.Logger.Log("Ticking Map done");
        }
    }
}
