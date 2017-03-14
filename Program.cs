using Evolution_Simulator.Computing;
using Evolution_Simulator.Organism;
using Evolution_Simulator.Positioning;
using Evolution_Simulator.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map(250);
            CellManager manager = new CellManager();
            int iterations = 10000;
            for(int i = 0; i < iterations; i++)
            {
                map.Tick(manager);
                int alive = map.CellsAlive;
                Console.WriteLine("Epoch #" + i + ", " + alive + " alive");
                /*BitmapPlotter plotter = new BitmapPlotter(@"test\year_" + i + ".png");
                plotter.PlotMap(map);*/
                if (alive == 0)
                    break;
            }
            Console.ReadLine();
        }
    }
}
