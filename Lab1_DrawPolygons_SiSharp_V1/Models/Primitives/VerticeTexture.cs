using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPolygons.Models.Primitives
{
    public class VerticeTexture
    {
        private const int COORDINATES_VALUE_MIN = 0;
        private const int COORDINATES_VALUE_MAX = 1;
        private const int V_DEFAULT = 0;
        private const int W_DEFAULT = 0;

        private float u;
        private float v;
        private float w;

        public VerticeTexture(float u, float v, float w)
        {
            this.u = u;
            this.v = v;
            this.w = w;
        }

        public VerticeTexture(float u, float v)
        {
            if (u < COORDINATES_VALUE_MIN || u > COORDINATES_VALUE_MAX || v < COORDINATES_VALUE_MIN || v > COORDINATES_VALUE_MAX)
            {
                throw new Exception($"Texture vertice coordinates should be in range {COORDINATES_VALUE_MIN}...{COORDINATES_VALUE_MAX}. u = {u}, v = {v}, w = {W_DEFAULT}");
            }

            this.u = u;
            this.v = v;
            w = W_DEFAULT;
        }

        public VerticeTexture(float u)
        {
            if (u < COORDINATES_VALUE_MIN || u > COORDINATES_VALUE_MAX)
            {
                throw new Exception($"Texture vertice coordinates should be in range {COORDINATES_VALUE_MIN}...{COORDINATES_VALUE_MAX}. u = {u}, v = {V_DEFAULT}, w = {W_DEFAULT}");
            }

            this.u = u;
            v = V_DEFAULT;
            w = W_DEFAULT;
        }
    }
}
