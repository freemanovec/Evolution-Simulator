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
    struct Cell
    {
        private const double
            INITIAL_ENERGY = 35d,
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
                if ((Energy + value) > EnergyCapacity)
                    _energy = EnergyCapacity;
                else
                    _energy = Energy + value;
            }
        }
        public double EnergyCapacity { get => _capacity_energy; }
        public double RatioFoodEnergy { get => RATIO_FOOD_TO_ENERGY; }
        public double MaxFoodEatenOnTick { get => MAX_FOOD_EATEN_ON_TICK; }

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

        /*public bool DrainEnergy(double level, bool fatal=false)
        {
            if(_energy >= level || fatal)
            {
                double result = _energy - level;
                _energy = result < 0 ? 0 : result;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AddEnergy(double level)
        {
            double result = _energy + level;
            _energy = _energy > _capacity_energy ? _capacity_energy : result;
            return true;
        }*/

        /*private static ulong identificationInternalLast = 0;
        private ulong identificationInternal = identificationInternalLast++;
        private static double InitialEnergy = 35d;
        public double _food = 0, _energy = InitialEnergy, _health = 1d, _intelligence, _strength;
        private const double
            NEEDED_ENERGY_MOVE = 1.5d,
            NEEDED_ENERGY_EAT = 0.75d,
            NEEDED_ENERGY_TICK = 0.15d,
            NEEDED_ENERGY_REPRODUCE = 30d,
            MAX_FOOD_EATEN_ON_TICK = 0.05d, 
            FOOD_TO_ENERGY_RATIO = 50d;
        public Tile parentTile;
        enum Direction { Up, Down, Left, Right};
        
        public override string ToString()
        {
            return "Cell #" + identificationInternal + " (IQ: " + _intelligence + ", ST: " + _strength + ")";
        }
        public Cell(Tile parentTile, double intelligence, double strength)
        {
            _intelligence = intelligence;
            _strength = strength;
            this.parentTile = parentTile;
            Logger.Logger.Log("Cell created, IQ=" + _intelligence + ", ST=" + _strength);
        }

        public void Tick()
        {
            Logger.Logger.Log("Ticking " + this);
            TryMove();
            Eat();

            if (HaveEnergy(NEEDED_ENERGY_REPRODUCE))
                TryReproduce();

            if (!HaveEnergy(NEEDED_ENERGY_TICK))
            {
                Kill("Quod non habeant cibos");
            }
            else
            {
                DrainEnergy(NEEDED_ENERGY_TICK);
            }
        }
        
        public void TryReproduce()
        {
            if (parentTile.CellCount < 1)
                return;
            Cell partner = parentTile.GetMatingCell(this, NEEDED_ENERGY_REPRODUCE);
            if (partner == null)
                return;
            Reproducer reproducer = new Reproducer(this, partner);
            reproducer.Reproduce();
        }
        public void DrainEnergyReproduce()
        {
            DrainEnergy(NEEDED_ENERGY_REPRODUCE);
        }
        public void Kill(string reason = "Mighty Thor's hammer struck this cell")
        {
            Logger.Logger.Log(this + " has died (" + reason + ")");
            parentTile.RemoveCell(this);
        }
        public bool HaveEnergy(double level)
        {
            if (_energy - level >= 0)
                return true;
            return false;
        }
        public void DrainEnergy(double level)
        {
            _energy -= level;
            Logger.Logger.Log(this + " drained " + this + " of " + level + " energy to " + _energy);
        }

        private void Eat()
        {
            if (!HaveEnergy(NEEDED_ENERGY_EAT))
                return;
            double foodAvailable = parentTile.FoodSupply;
            double ate = 0;
            if (foodAvailable >= MAX_FOOD_EATEN_ON_TICK)
            {
                ate = MAX_FOOD_EATEN_ON_TICK;
                parentTile.FoodSupply -= MAX_FOOD_EATEN_ON_TICK;
            }
            else
            {
                ate = parentTile.FoodSupply;
                parentTile.FoodSupply = 0;
            }
            if (ate != 0)
            {
                DrainEnergy(NEEDED_ENERGY_EAT);
                ProcessFood(ate);
            }
        }
        private void ProcessFood(double level)
        {
            double gainedEnergy = level * FOOD_TO_ENERGY_RATIO;
            _energy += gainedEnergy;
            Logger.Logger.Log(this + " ate " + level + " yums and gained " + gainedEnergy + " energy");
        }        
        private void TryMove()
        {
            if(HaveEnergy(NEEDED_ENERGY_MOVE))
            {
                int[] toMoveTo = BestDirection(CanMove);
                if(toMoveTo != null)
                {
                    ForceMove(toMoveTo);
                    DrainEnergy(NEEDED_ENERGY_MOVE);
                }
            }
        }
        private void ForceMove(int[] newPosition)
        {
            Logger.Logger.Log("Moving " + this + " from [" + parentTile.position[0] + ";" + parentTile.position[1] + "] to [" + newPosition[0] + ";" + newPosition[1] + "]");
            Tile newTile = parentTile.parentMap.GetTile(newPosition);
            newTile.AddCell(this);
            parentTile.RemoveCell(this);
            parentTile = newTile;
        }
        private int[] BestDirection(int[][] possible)
        {
            double bestFood = 0;
            int[] bestDirection = null;
            for(int i = 0; i < possible.Length; i++)
            {
                double foodAtPosition = parentTile.parentMap.GetTile(possible[i]).FoodSupply;
                if(foodAtPosition > bestFood)
                {
                    bestFood = foodAtPosition;
                    bestDirection = possible[i];
                }
            }
            return bestDirection;
        }
        private int[][] CanMove
        {
            get
            {
                int[] position = parentTile.position;
                int[][] positions = //up, down, left, right
                {
                    new int[]{position[0], position[1] + 1 },
                    new int[]{position[0], position[1] - 1 },
                    new int[]{position[0] - 1, position[1] },
                    new int[]{position[0] + 1, position[1] },
                };
                List<int[]> directions = new List<int[]>();
                int boundary = parentTile.parentMap.Size;
                if ((positions[0][0] >= 0 && positions[0][0] < boundary) && (positions[0][1] >= 0 && positions[0][1] < boundary))
                    directions.Add(positions[0]);
                if ((positions[1][0] >= 0 && positions[1][0] < boundary) && (positions[1][1] >= 0 && positions[1][1] < boundary))
                    directions.Add(positions[1]);
                if ((positions[2][0] >= 0 && positions[2][0] < boundary) && (positions[2][1] >= 0 && positions[2][1] < boundary))
                    directions.Add(positions[2]);
                if ((positions[3][0] >= 0 && positions[3][0] < boundary) && (positions[3][1] >= 0 && positions[3][1] < boundary))
                    directions.Add(positions[3]);
                return directions.ToArray();
            }
        }*/
    }
}
