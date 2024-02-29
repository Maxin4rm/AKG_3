using DrawPolygons.Models.Primitives;
using DrawPolygons.Models.RenderElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPolygons.Services.Lighting
{
    public static class LambertLighting
    {
        public static float TakeLightIntensityForTriangle(VectorFourCoord vectorLightDirection, VectorFourCoord vectorTriangleNormal)
        {
            return Math.Max(vectorLightDirection.MultiplyWithScalarResultWithoutW(vectorTriangleNormal), 0);
        }
    }
}
