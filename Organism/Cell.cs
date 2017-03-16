using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;
using Evolution_Simulator.Positioning;
using Evolution_Simulator.Computing;

namespace Evolution_Simulator.Organism
{
    class Cell
    {
        private static ulong _lastID = 0;
        private static ulong _ID { get
            {
                return _lastID++;
            }
        }
        private readonly ulong internalID;

        private const double
            INITIAL_ENERGY = 1.0d,
            INITIAL_IQ = 0.05d,
            INITIAL_STRENGTH = 0.05d,

            INITIAL_CAPACITY_ENERGY = 150d,

            ENERGY_NEEDED_MOVE = 1.5d,
            ENERGY_NEEDED_EAT = 0.75d,
            ENERGY_NEEDED_TICK = 0.15d,
            ENERGY_NEEDED_REPRODUCE = 30d,

            MAX_FOOD_EATEN_ON_TICK = 0.05d,
            RATIO_FOOD_TO_ENERGY = 50d;


        private double 
            _energy,
            _iq,
            _strength;

        private double
            _capacity_energy;

        public double Energy { get => _energy; set
            {
                if ((value) > EnergyCapacity)
                    _energy = EnergyCapacity;
                else
                    _energy = value;
            }
        }
        public double EnergyCapacity { get => _capacity_energy; }
        public double RatioFoodEnergy { get => RATIO_FOOD_TO_ENERGY; }
        public double MaxFoodEatenOnTick { get => MAX_FOOD_EATEN_ON_TICK; }
        public double EnergyNeededToReproduce { get => ENERGY_NEEDED_REPRODUCE; }
        public double EnergyNeededToMove { get => ENERGY_NEEDED_MOVE; }
        public double EnergyNeededToTick { get => ENERGY_NEEDED_TICK; }

        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public Cell(Vector2 position)
        {
            _energy = INITIAL_ENERGY;
            _iq = INITIAL_IQ;
            _strength = INITIAL_STRENGTH;

            _capacity_energy = INITIAL_CAPACITY_ENERGY;

            _position = position;
        }

        public override string ToString()
        {
            return "Cell #" + internalID;
        }
    }
}
