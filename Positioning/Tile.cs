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
        public readonly int[] position;
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
        }

        public void Tick()
        {
            try
            {
                for (int i = 0; i < _cells.Count; i++)
                    _cells[i].Tick();
            }catch(IndexOutOfRangeException e)
            {
                Logger.Logger.Log("Out of range when trying to iterate over the cells");
            }
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
        public Cell GetMatingCell(Cell caller, double neededEnergy)
        {
            for(int i = 0; i < _cells.Count; i++)
            {
                if (_cells[i] != caller && _cells[i].HaveEnergy(neededEnergy))
                    return _cells[i];
            }
            return null;
        }
    }
}
