﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPolygons.Models.Primitives
{
    public class PolygonIndexes
    {
        public int[] VerticesGeometricIndexes { get; set; }
        public int[] VerticesTextureIndexes { get; set; }
        public int[] VectorsNormalIndexes { get; set; }

        public PolygonIndexes(int[] verticesGeometricIndexes, int[] verticesTextureIndexes, int[] vectorsNormalIndexes)
        {
            VerticesGeometricIndexes = verticesGeometricIndexes;
            VerticesTextureIndexes = verticesTextureIndexes;
            VectorsNormalIndexes = vectorsNormalIndexes;
        }
    }
}
