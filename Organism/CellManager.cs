using Evolution_Simulator.Positioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulator.Organism
{
    class CellManager
    {
        public void Tick(CellManagerData data)
        {
            PhaseDigestive(data);
            PhaseProductive(data);
            PhaseMurrative(data);
            PhaseEliminative(data);
        }

        private void PhaseDigestive(CellManagerData data)
        {
            Eat(data);
        }
        private void PhaseProductive(CellManagerData data)
        {

        }
        private void PhaseMurrative(CellManagerData data)
        {

        }
        private void PhaseEliminative(CellManagerData data)
        {

        }

        private void Eat(CellManagerData data)
        {
            Tile tile = data.TileCurrent;
            Cell cell = data.CellCurrent;

            double foodAvailable = tile.Food;
            double foodCapacity = cell.EnergyCapacity / cell.RatioFoodEnergy;
            double foodConsumable = foodCapacity - (cell.Energy / cell.RatioFoodEnergy);
            double foodToEat = (foodConsumable >= foodAvailable) ? foodAvailable : foodConsumable;
            foodToEat = (foodToEat > cell.MaxFoodEatenOnTick) ? cell.MaxFoodEatenOnTick : foodToEat;

            tile.Food -= foodToEat;
            cell.Energy += foodToEat * cell.RatioFoodEnergy;
        }
    }
}
