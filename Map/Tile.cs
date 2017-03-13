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
        private double _moisture;
        public double Moisture { get; set; }
        private double _foodSupply;
        public double FoodSupply { get; set; }
        private double _waterSupply;
        public double WaterSupply { get; set; }
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

        public Tile(double _temperature, double _moisture, double _foodSupply, double _waterSupply, double _terrain)
        {
            Temperature = _temperature;
            Moisture = _moisture;
            FoodSupply = _foodSupply;
            WaterSupply = _waterSupply;
            Terrain = _terrain;
        }
    }
}
