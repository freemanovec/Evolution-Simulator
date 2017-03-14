using Evolution_Simulator.Organism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulator.Positioning
{
    struct Tile
    {
        private double
            _height,
            _temperature,
            _food;
        public double Height { get => _height; set => _height = value; }
        public double Temperature { get => _temperature; set => _temperature = value; }
        public double Food { get => _food; set => _food = value; }

        public double Hazardness { get
            {
                //TODO add function to calculate hazardness
                throw new NotImplementedException();
            }
        }

        private Vector2 _position;
        public Vector2 Position { get => _position; set => _position = value; }

        public Tile(Vector2 position, double height, double temperature, double food)
        {
            _position = position;
            _height = height;
            _temperature = temperature;
            _food = food;
        }

        /*public readonly int[] position;
        public readonly Map parentMap;
        private double _temperature;
        public double Temperature { get; set; }
        private double _foodSupply;
        public double FoodSupply { get; set; }
        private double _terrain;
        public double Terrain { get; set; }
        public double Hazardness
        {
            get
            {
                //TODO add function to calculate hazardness
                throw new NotImplementedException();
            }
        }
        private List<Cell> _cells = new List<Cell>();
        private const uint MAX_CELLS = 75;
        public int CellCount
        {
            get
            {
                return _cells.Count;
            }
        }
        
        public Tile(Map parentMap, int[] position, double _temperature, double _foodSupply, double _terrain)
        {
            this.parentMap = parentMap;
            this.position = position;
            Temperature = _temperature;
            FoodSupply = _foodSupply;
            Terrain = _terrain;
        }*/
    }
}
