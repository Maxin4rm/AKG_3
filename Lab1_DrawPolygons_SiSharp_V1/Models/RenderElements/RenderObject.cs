using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawPolygons.Models.Primitives;

namespace DrawPolygons.Models.RenderElements
{
    public class RenderObject
    {
        public VerticeGeometric[] VerticesGeometric { get; set; }
        public VerticeTexture[] VerticesTexture { get; set; }
        public VectorFourCoord[] VectorsNormal { get; set; }
        public TriangleIndexes[] TrianglesIndexes { get; set; }

        public RenderObject(VerticeGeometric[] verticesGeometric, VerticeTexture[] verticesTexture, VectorFourCoord[] vectorsNormal, TriangleIndexes[] trianglesIndexes)
        {
            VerticesGeometric = verticesGeometric;
            VerticesTexture = verticesTexture;
            VectorsNormal = vectorsNormal;
            TrianglesIndexes = trianglesIndexes;
        }
    }
}
