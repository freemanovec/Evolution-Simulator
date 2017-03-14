using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolution_Simulator.Computing;
using Evolution_Simulator.Organism;

namespace Evolution_Simulator.Positioning
{
    class Map
    {
        private readonly Tile[,] _tiles;
        private readonly List<Cell> _cells = new List<Cell>();
        private readonly int _size;

        /// <summary>
        /// Orientation - Bottom Left = X: 0; Y: 0
        /// </summary>
        /// <param name="sizeSide"></param>
        public Map(int sizeSide)
        {
            _tiles = new Tile[sizeSide, sizeSide];
            _size = sizeSide;

            Generate(sizeSide);
            Populate(2, 16);
        }

        public bool AddCell(Cell cell)
        {
            _cells.Add(cell);
            return true;
        }
        public bool RemoveCell(Cell cell)
        {
            if (_cells.Contains(cell))
            {
                _cells.Remove(cell);
                return true;
            }
            return false;
        }
        public int Size
        {
            get
            {
                return _size;
            }
        }
        public int CellsAlive
        {
            get
            {
                return _cells.Count;
            }
        }
        public List<Cell> Cells { get => _cells; }
        public Tile GetTile(Vector2 position)
        {
            try
            {
                return _tiles[(int)position.X, (int)position.Y];
            }
            catch
            {
                throw new ArgumentOutOfRangeException("Requested Tile is out of range");
            }
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
                    _tiles[i, j] = new Tile(new Vector2(i,j), noiseBase[i, j], noiseTemp[i, j], noiseFood[i, j]);
                }
        }

        public void Populate(int groups, int groupSize)
        {
            Random rand = new Random();
            int[] size = { _tiles.GetLength(0), _tiles.GetLength(1) };
            for(int i = 0; i < groups; i++)
            {
                Vector2 coords = new Vector2(rand.Next(size[0]), rand.Next(size[1]));
                for (int j = 0; j < groupSize; j++)
                {
                    Cell cell = new Cell(coords);
                    AddCell(cell);
                }
            }
        }

        public void Tick(CellManager manager)
        {
            for(int i = 0; i < CellsAlive; i++)
            {
                Cell cell = _cells[i];
                var surroundings = GetSurroundings(cell);
                CellManagerData data = new CellManagerData(GetTile(cell.Position), surroundings.Item2, cell, surroundings.Item1);
                manager.Tick(data);
            }
        }

        private Tuple<List<Cell>[], Tile[]> GetSurroundings(Cell cell)
        {
            Vector2[] positions =
            {
                new Vector2(cell.Position.X-1, cell.Position.Y-1),
                new Vector2(cell.Position.X, cell.Position.Y-1),
                new Vector2(cell.Position.X+1, cell.Position.Y-1),
                new Vector2(cell.Position.X-1, cell.Position.Y),
                new Vector2(cell.Position.X+1, cell.Position.Y),
                new Vector2(cell.Position.X-1, cell.Position.Y+1),
                new Vector2(cell.Position.X, cell.Position.Y+1),
                new Vector2(cell.Position.X+1, cell.Position.Y+1)
            };
            List<Cell>[] cells =
            {
                new List<Cell>(),
                new List<Cell>(),
                new List<Cell>(),
                new List<Cell>(),
                new List<Cell>(),
                new List<Cell>(),
                new List<Cell>(),
                new List<Cell>()
            };
            foreach(Cell _cell in _cells)
            {
                int indexOf = Array.IndexOf(positions, _cell.Position);
                if (indexOf != -1)
                {
                    cells[indexOf].Add(_cell);
                }
            }
            Tile[] tiles = new Tile[8];
            for(int i = 0; i < 8; i++)
            {
                tiles[i] = GetTile(positions[i]);
            }

            return new Tuple<List<Cell>[], Tile[]>(cells, tiles);
        }
    }

    struct CellManagerData
    {
        private readonly Tile _tile;
        private readonly Tile[] _surroundingsTiles;
        private readonly Cell _cell;
        private readonly List<Cell>[] _surroundingsCells;

        public Tile TileCurrent { get => _tile; }
        public Tile[] TilesSurrounding { get => _surroundingsTiles; }
        public Cell CellCurrent { get => _cell; }
        public List<Cell>[] CellsSurrounding { get => _surroundingsCells; }
        
        public CellManagerData(Tile tile, Tile[] surroundingsTiles, Cell cell, List<Cell>[] surroundingsCells)
        {
            _tile = tile;
            _surroundingsTiles = surroundingsTiles;
            _cell = cell;
            _surroundingsCells = surroundingsCells;
        }
    }
}
