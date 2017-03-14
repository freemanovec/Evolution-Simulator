using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Evolution_Simulator.Positioning;
using Evolution_Simulator.Organism;

namespace Evolution_Simulator.Visualization
{
    class BitmapPlotter
    {
        private string _path;

        public BitmapPlotter(string filename)
        {
            _path = filename;
        }
        public bool Plot(double[,] array)
        {
            int[] size = new int[] { array.GetLength(0), array.GetLength(1) };
            if (size[0] == 0 || size[1] == 0)
                throw new InvalidOperationException("Array size cannot be zero");
            Bitmap bitmap = new Bitmap(size[0], size[1]);
            for(int i = 0; i < size[0]; i++)
                for(int j = 0; j < size[1]; j++)
                {
                    double value = array[i, j];
                    if (value < 0)
                        value = 0;
                    else if (value > 1)
                        value = 1;
                    bitmap.SetPixel(i, j, Color.FromArgb(255, (int)(value * 255), (int)(value * 255), (int)(value * 255)));
                }
            try
            {
                bitmap.Save(_path);
            }
            catch(System.Runtime.InteropServices.ExternalException)
            {
                return false;
            }
            return true;
        }
        public bool PlotMap(Map map)
        {
            Bitmap bitmap = new Bitmap(map.Size, map.Size);
            for(int i = 0; i < map.Size; i++)
                for(int j = 0; j < map.Size; j++)
                {
                    Tile tile = map.GetTile(new Vector2(i, j));
                    double food = tile.Food;
                    int green = (int)(food * 255);
                    //int red = (tile.CellCount != 0) ? 255 : 0;
                    int red = 0;
                    int blue = 0;
                    bitmap.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            foreach(Cell cell in map.Cells)
            {
                Vector2 position = cell.Position;
                bitmap.SetPixel((int)position.X, (int)position.Y, Color.Red);
            }
            try
            {
                bitmap.Save(_path);
            }
            catch (System.Runtime.InteropServices.ExternalException)
            {
                return false;
            }
            return true;
        }
    }
}
