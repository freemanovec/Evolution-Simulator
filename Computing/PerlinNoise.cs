using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulator.Computing
{
    class PerlinNoise
    {

        private int[] hash = {
        151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
        140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
        247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
        57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
        74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
        60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
        65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
        200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
        52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
        207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
        119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
        129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
        218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
        81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
        184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
        222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180,

        151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
        140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
        247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
        57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
        74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
        60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
        65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
        200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
        52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
        207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
        119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
        129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
        218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
        81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
        184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
        222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180
        };

        private readonly int _octaves;
        private readonly double _frequency, _lacunarity, _persistence;

        public PerlinNoise(double frequency, int octaves, double lacunarity, double persistence)
        {
            _frequency = frequency;
            _octaves = octaves;
            _lacunarity = lacunarity;
            _persistence = persistence;

            Random rand = new Random();
            hash = RandomHash(rand);
        }

        int[] RandomHash(Random rand)
        {
            return hash.OrderBy(x => rand.Next()).ToArray();
        }

        private double[][] gradients2D =
        {
            new double[] {1, 0 },
            new double[] {-1, 0 },
            new double[] {0,1 },
            new double[] {0, -1 },
            new double[] {1/Mathf.Sqrt2, 1/Mathf.Sqrt2 },
            new double[] {-(1/Mathf.Sqrt2), 1/Mathf.Sqrt2 },
            new double[] {1/Mathf.Sqrt2, -(1/Mathf.Sqrt2) },
            new double[] {-(1/Mathf.Sqrt2), -(1/Mathf.Sqrt2) }
        };

        public double Noise(double x, double y)
        {
            double frequency = _frequency;
            int octaves = _octaves;
            double sum = Perlin(x, y, frequency);
            double amplitude = 1d;
            double range = 1d;
            for(int i = 1; i < octaves; i++)
            {
                frequency *= _lacunarity;
                amplitude *= _persistence;
                range += amplitude;
                sum += Perlin(x, y, frequency) * frequency;
            }
            double befResult = sum / range;
            return befResult > 1 ? 1 : befResult;
        }
        public double[,] NoiseArray(int size)
        {
            return NoiseArray(size, size);
        }
        public double[,] NoiseArray(int sizeX, int sizeY)
        {
            double[,] noise = new double[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                    noise[i, j] = Noise(i, j);
            return noise;
        }

        private double Dot(double xg, double yg, double x, double y)
        {
            return xg * x + yg * y;
        }
        private double Smooth(double t)
        {
            return t * t * t * (t * (t * 6d - 15d) + 10d);
        }
        private double Perlin(double x, double y, double frequency)
        {
            x *= frequency;
            y *= frequency;

            int ix0 = (int)Math.Floor(x);
            int iy0 = (int)Math.Floor(y);

            double tx0 = x - ix0;
            double ty0 = y - iy0;

            double tx1 = tx0 - 1d;
            double ty1 = ty0 - 1d;

            ix0 &= 255;
            iy0 &= 255;

            int ix1 = ix0 + 1;
            int iy1 = iy0 + 1;

            int h0 = hash[ix0];
            int h1 = hash[ix1];

            double[] g00 = gradients2D[hash[h0 + iy0] & 7];
            double[] g10 = gradients2D[hash[h1 + iy0] & 7];
            double[] g01 = gradients2D[hash[h0 + iy1] & 7];
            double[] g11 = gradients2D[hash[h1 + iy1] & 7];

            double v00 = Dot(g00[0], g00[1], tx0, ty0);
            double v10 = Dot(g10[0], g10[1], tx1, ty0);
            double v01 = Dot(g01[0], g01[1], tx0, ty1);
            double v11 = Dot(g11[0], g11[1], tx1, ty1);

            double tx = Smooth(tx0);
            double ty = Smooth(ty0);

            double p0 = Mathf.Lerp(v00, v10, tx);
            double p1 = Mathf.Lerp(v01, v11, tx);

            double f0 = Mathf.Lerp(p0, p1, ty);
            double f1 = f0 * Mathf.Sqrt2;

            return f1;
        }
    }
}
