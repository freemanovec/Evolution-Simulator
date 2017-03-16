using Evolution_Simulator.Computing;
using Evolution_Simulator.Organism;
using Evolution_Simulator.Positioning;
using Evolution_Simulator.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Evolution_Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map(64);
            map.Populate(1, 2);
            CellManager manager = new CellManager();
            BitmapPlotter plotter = new BitmapPlotter();
            int iterations = 1500;
            for(int i = 0; i < iterations; i++)
            {
                int alive = map.CellsAlive;
                Console.WriteLine("Epoch #" + i + ", " + alive + " alive");
                plotter.filename = @"test\year_" + i + ".png";
                plotter.PlotMap(map);
                if (alive == 0)
                    break;
                map.Tick(manager);
            }
            Console.ReadLine();
        }
    }
}
