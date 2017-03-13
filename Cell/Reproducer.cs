﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulator.Cell
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
            Logger.Logger.Log("Reproducing");
        }
    }
}
