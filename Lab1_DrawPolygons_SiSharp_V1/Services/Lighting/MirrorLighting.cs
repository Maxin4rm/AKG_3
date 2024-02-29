using DrawPolygons.Models.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPolygons.Services.Lighting
{
    public class MirrorLighting
    {
        public static float TakeMirrorLightIntensity(VectorFourCoord vectorLightDirection, VectorFourCoord vectorTriangleNormal, VectorFourCoord vectorEye, float surfaceGlossCoefficient, float lightIntensityCoefficient)
        {
            float mirrorLightCoefficient = (float)Math.Pow(Math.Max(TakeReflectiveBeamOfLight(vectorLightDirection, vectorTriangleNormal).MultiplyWithScalarResultWithoutW(vectorEye), 0), surfaceGlossCoefficient) * lightIntensityCoefficient;
            return mirrorLightCoefficient;
        }

        private static VectorFourCoord TakeReflectiveBeamOfLight(VectorFourCoord vectorLightDirection, VectorFourCoord vectorTriangleNormal)
        {
            return vectorTriangleNormal.MultiplyWithScalarNumberWithoutW(2.0f * vectorLightDirection.MultiplyWithScalarResultWithoutW(vectorTriangleNormal)).SubstractWithVectorWithoutW(vectorLightDirection);
        }
    }
}
