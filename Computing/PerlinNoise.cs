using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulator.Computing
{
    static class PerlinNoise
    {
        /*private readonly static byte[] _permutation =
        {
            87,239,104,102,186,53,158,17,215,57,64,133,157,230,5,176,192,112,200,214,105,180,69,191,55,124,96,100,224,123,171,4,189,97,170,89,251,196,13,254,88,59,99,164,244,246,115,142,28,219,154,125,66,41,14,208,81,61,206,152,146,221,220,205,185,83,250,140,67,197,80,204,240,101,31,36,150,2,177,137,38,29,108,168,129,8,198,90,68,70,63,98,188,52,161,58,7,222,95,228,203,126,179,111,121,139,34,136,109,162,33,120,130,45,47,44,11,216,20,18,187,85,141,9,46,65,248,30,73,75,118,210,37,60,247,25,127,92,117,233,181,77,114,155,144,156,50,212,91,74,82,107,253,174,23,149,147,134,213,227,151,110,211,199,167,62,22,138,243,207,165,249,255,236,231,24,116,32,145,235,252,182,26,21,48,143,245,131,166,6,54,135,184,217,10,229,190,49,16,1,194,71,39,27,113,238,201,119,76,163,159,173,172,132,3,93,78,202,72,122,84,178,56,148,12,128,51,175,43,15,160,40,193,232,223,226,153,242,234,241,94,42,103,209,195,237,0,218,35,79,225,169,183,86,19,106
        };
        private static readonly int[] _p;

        static PerlinNoise()
        {
            _p = new int[512];
            for (int i = 0; i < 512; i++)
            {
                _p[i] = _permutation[i % 256];
            }
        }

        public static double Noise(double _x, double _y, double _z, int _octaves, double _persistence)
        {
            double sum = 0, frequency = 1, amplitude = 1;
            for(int i = 0; i < _octaves; i++)
            {
                sum += NoiseInternal(_x * frequency, _y * frequency, _z * frequency) * amplitude;
                amplitude *= _persistence;
                frequency *= 2;
            }
            return sum;
        }

        private static double NoiseInternal(double x, double y, double z)
        {
            int xi = (int)x & 255;
            int yi = (int)y & 255;
            int zi = (int)z & 255;

            double xf = x - (int)x;
            double yf = y - (int)y;
            double zf = z - (int)z;

            double u = Mathf.Fade(xf);
            double v = Mathf.Fade(yf);
            double w = Mathf.Fade(zf);

            int a = _p[xi] + yi;
            int aa = _p[a] + zi;
            int ab = _p[a + 1] + zi;
            int b = _p[xi + 1] + yi;
            int ba = _p[b] + zi;
            int bb = _p[b + 1] + zi;

            double x1, x2, y1, y2;
            x1 = Mathf.Lerp(Mathf.Gradient(_p[aa], xf, yf, zf), Mathf.Gradient(_p[ba], xf - 1, yf, zf), u);
            x2 = Mathf.Lerp(Mathf.Gradient(_p[ab], xf, yf - 1, zf), Mathf.Gradient(_p[bb], xf - 1, yf - 1, zf), u);
            y1 = Mathf.Lerp(x1, x2, v);
            x1 = Mathf.Lerp(Mathf.Gradient(_p[aa + 1], xf, yf, zf - 1), Mathf.Gradient(_p[ba + 1], xf - 1, yf, zf - 1), u);
            x2 = Mathf.Lerp(Mathf.Gradient(_p[ab + 1], xf, yf - 1, zf - 1), Mathf.Gradient(_p[bb + 1], xf - 1, yf - 1, zf - 1), u);
            y2 = Mathf.Lerp(x1, x2, v);

            return (Mathf.Lerp(y1, y2, w) + 1) / 2;
        }*/

        private static int[] hash = {
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
        private static double[][] gradients2D =
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

        public static double Noise(double x, double y, double frequency, int octaves, double lacunarity, double persistence)
        {
            double sum = Perlin(x, y, frequency);
            double amplitude = 1d;
            double range = 1d;
            for(int i = 1; i < octaves; i++)
            {
                frequency *= lacunarity;
                amplitude *= persistence;
                range += amplitude;
                sum += Perlin(x, y, frequency) * frequency;
            }
            double befResult = sum / range;
            return befResult > 1 ? 1 : befResult;
        }
        private static double Dot(double xg, double yg, double x, double y)
        {
            return xg * x + yg * y;
        }
        private static double Smooth(double t)
        {
            return t * t * t * (t * (t * 6d - 15d) + 10d);
        }
        private static double Perlin(double x, double y, double frequency)
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
