using Evolution_Simulator.Organism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulator.Positioning
{
    class Tile
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
        private List<Cell> _cells = new List<Cell>();
        public List<Cell> Cells { get => _cells; }

        public Tile(Vector2 position, double height, double temperature, double food)
        {
            _position = position;
            _height = height;
            _temperature = temperature;
            _food = food;
        }
    }
}
