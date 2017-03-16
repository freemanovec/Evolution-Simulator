using Evolution_Simulator.Computing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulator.Positioning
{
    struct Vector2
    {
        private double _x, _y;
        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = X;
            }
        }
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = Y;
            }
        }
        public double Magnitude
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }
        public Vector2 Normalized
        {
            get
            {
                return Mathf.Normalize(this);
            }
        }

        public Vector2(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public override string ToString()
        {
            return "Vector2D(" + X + ", " + Y + ")";
        }
    }
}
