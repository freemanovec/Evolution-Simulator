using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulator.Organism
{
    class Reproducer
    {
        private readonly Cell _cell0, _cell1;

        public Reproducer(Cell cell0, Cell cell1)
        {
            _cell0 = cell0;
            _cell1 = cell1;
        }

        public void Reproduce()
        {
            Logger.Logger.Log("Reproducing " + _cell0 + " with " + _cell1);
            _cell0.DrainEnergyReproduce();
            _cell1.DrainEnergyReproduce();
            Cell newCell = new Cell(_cell0.parentTile, _cell0._intelligence, _cell0._strength);
            _cell0.parentTile.AddCell(newCell);
        }
    }
}
