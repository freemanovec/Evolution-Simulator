using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;

namespace Evolution_Simulator.Organism
{
    class Cell
    {
        private static ulong identificationInternalLast = 0;
        private ulong identificationInternal = identificationInternalLast++;
        private static double InitialEnergy = 15d;
        private double _food = 0, _energy = InitialEnergy, _health = 1d, _intelligence, _strength;
        
        public override string ToString()
        {
            return "Cell #" + identificationInternal + " (IQ: " + _intelligence + ", ST: " + _strength + ")";
        }
        public Cell(double intelligence, double strength)
        {
            _intelligence = intelligence;
            _strength = strength;
            Logger.Logger.Log("Cell created, IQ=" + _intelligence + ", ST=" + _strength);
        }
    }
}
