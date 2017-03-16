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
            //Populate(2, 16);
            //Populate(1, 1);
        }

        public bool AddCell(Cell cell)
        {
            _cells.Add(cell);
            GetTile(out Tile tile, cell.Position);
            tile.Cells.Add(cell);
            return true;
        }
        public bool RemoveCell(Cell cell)
        {
            GetTile(out Tile tile, cell.Position);
            return _cells.Remove(cell) && tile.Cells.Remove(cell);
        }
        public bool MoveCell(Cell cell, Vector2 newPosition)
        {
            Logger.Logger.Log("Moving cell");
            bool success;
            GetTile(out Tile tile, cell.Position);
            if (tile.Cells.Contains(cell))
                Logger.Logger.Log("Initial list contains cell (Length: " + tile.Cells.Count + ")");
            else
                Logger.Logger.Log("Initial list does not contain cell (Length: " + tile.Cells.Count + ")");
            if(tile.Cells.Remove(cell))
            {
                GetTile(out Tile newTile, newPosition);
                newTile.Cells.Add(cell);
                cell.Position = newPosition;
                if (newTile.Cells.Contains(cell))
                    Logger.Logger.Log("Latter list contains cell (Length: " + newTile.Cells.Count + ")");
                else
                    Logger.Logger.Log("Latter list does not contain cell (Length: " + newTile.Cells.Count + ")");
                success = true;
            }else
                success = false;

            Logger.Logger.Log("Done moving cell");
            return success;
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
        public bool GetTile(out Tile tile, Vector2 position)
        {
            try
            {
                tile = _tiles[(int)position.X, (int)position.Y];
                return true;
            }
            catch(IndexOutOfRangeException)
            {
                //throw new ArgumentOutOfRangeException("Requested Tile is out of range (" + position.X + ", " + position.Y + ")");
                Logger.Logger.Log("[WARNING] Requested Tile is out of range (" + position.X + ", " + position.Y + ")");
                tile = null;
                return false;
            }
            catch(Exception e)
            {
                Logger.Logger.Log("Exotic exception in GetTile: " + e.Message + ", stacktrace: " + e.StackTrace);
                throw e;
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
                GetTile(out Tile currentTile, cell.Position);
                CellManagerData data = new CellManagerData(currentTile, surroundings.Item2, cell, surroundings.Item1, this);
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
                new Vector2(cell.Position.X+1, cell.Position.Y+1),
                cell.Position
            };

            Tile[] tiles = new Tile[9];
            for(int i = 0; i < 9; i++)
            {
                GetTile(out Tile tile, positions[i]);
                tiles[i] = tile;
                //tiles[i] = GetTile(positions[i]);
            }

            List<Cell>[] cells = new List<Cell>[9];
            for(int i = 0; i < 9; i++)
            {
                Tile tile = tiles[i];
                cells[i] = new List<Cell>();
                if (tile == null)
                    continue;
                foreach(Cell _cell in tile.Cells)
                {
                    if (_cell != cell)
                        cells[i].Add(_cell);
                }
            }
            return new Tuple<List<Cell>[], Tile[]>(cells, tiles);

            /*List<Cell>[] cells =
            {
                new List<Cell>(),
                new List<Cell>(),
                new List<Cell>(),
                new List<Cell>(),
                new List<Cell>(),
                new List<Cell>(),
                new List<Cell>(),
                new List<Cell>(),
                new List<Cell>()
            };
            Logger.Logger.Log("Started getting surrounding cells");
            foreach(Cell _cell in _cells)
            {
                Logger.Logger.Log("Iterating over a cell");
                int indexOf = Array.IndexOf(positions, _cell.Position);
                if (indexOf != -1 && indexOf != 8)
                {
                    cells[indexOf].Add(_cell);
                    Logger.Logger.Log("Adding cell to surroundings");
                }
            }
            Logger.Logger.Log("Done getting surrounding cells");
            Tile[] tiles = new Tile[8];
            for(int i = 0; i < 8; i++)
            {
                if(IsValidPosition(positions[i], Size))
                    tiles[i] = GetTile(positions[i]);
            }

            return new Tuple<List<Cell>[], Tile[]>(cells, tiles);*/
        }

        private bool IsValidPosition(Vector2 position, int size)
        {
            int x = (int)position.X;
            int y = (int)position.Y;
            if (
                x < 0 ||
                y < 0 ||
                x >= size ||
                y >= size
                )
                return false;
            return true;
        }
    }

    struct CellManagerData
    {
        private readonly Tile _tile;
        private readonly Tile[] _surroundingsTiles;
        private readonly Cell _cell;
        private readonly List<Cell>[] _surroundingsCells;
        private readonly Map _map;

        public Tile TileCurrent { get => _tile; }
        public Tile[] TilesSurrounding { get => _surroundingsTiles; }
        public Cell CellCurrent { get => _cell; }
        public List<Cell>[] CellsSurrounding { get => _surroundingsCells; }
        public Map Map { get => _map; }
        
        public CellManagerData(Tile tile, Tile[] surroundingsTiles, Cell cell, List<Cell>[] surroundingsCells, Map map)
        {
            _tile = tile;
            _surroundingsTiles = surroundingsTiles;
            _cell = cell;
            _surroundingsCells = surroundingsCells;
            _map = map;
        }
    }
}
