using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
    }
}
