using Evolution_Simulator.Organism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulator.Map
{
    class Tile
    {
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
        
        public Tile(double _temperature, double _foodSupply, double _terrain)
        {
            Temperature = _temperature;
            FoodSupply = _foodSupply;
            Terrain = _terrain;
        }

        public void Tick()
        {
            foreach (Cell cell in _cells)
                cell.Tick();
        }
        public bool AddCell(Cell cell)
        {
            if (_cells.Count >= MAX_CELLS)
                return false;
            if (_cells.Contains(cell))
                return false;
            _cells.Add(cell);
            return true;
        }
        public bool RemoveCell(Cell cell)
        {
            if (!_cells.Contains(cell))
                return false;
            _cells.Remove(cell);
            return true;
        }
    }
}
