using DrawPolygons.Models.Primitives;
using DrawPolygons.Models.RenderElements;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace DrawPolygons.Services.ParserObjectFile
{
    public class ParserObjFile
    {
        private static ParserObjFile? instance;
        private List<PolygonIndexes> PoligonsIndexes;

        private List<VerticeGeometric> VerticesGeometric;
        private List<VerticeTexture> VerticesTexture;
        private List<VectorFourCoord> VectorsNormal;
        private List<TriangleIndexes> TrianglesIndexes;


        private ParserObjFile()
        {
            VerticesGeometric = new List<VerticeGeometric>();
            VerticesTexture = new List<VerticeTexture>();
            VectorsNormal = new List<VectorFourCoord>();
            PoligonsIndexes = new List<PolygonIndexes>();
            TrianglesIndexes = new List<TriangleIndexes>();
        }

        public static ParserObjFile CreateInstance()
        {
            if (instance == null)
            {
                instance = new ParserObjFile();
            }               

            return instance;
        }

        private TypeDataFileObj? TakeTypeLineDataOfObjFile(string lineTypeData)
        {
            switch (lineTypeData)
            {
                case "v":
                    return TypeDataFileObj.VerticeGeometric;
                case "vt":
                    return TypeDataFileObj.VerticeTexture;
                case "vn":
                    return TypeDataFileObj.VectorNormal;
                case "f":
                    return TypeDataFileObj.Polygon;
                default:
                    return null;
            }
        }

        private VerticeGeometric ParseLineVerticeGeometric(string lineData)
        {
            VerticeGeometric verticeGeometric;
            string[] dataValuesLines = lineData.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (dataValuesLines.Length == 4)
            {
                try
                {
                    verticeGeometric = new VerticeGeometric(float.Parse(dataValuesLines[0], CultureInfo.InvariantCulture),
                        float.Parse(dataValuesLines[1], CultureInfo.InvariantCulture), float.Parse(dataValuesLines[2], CultureInfo.InvariantCulture), float.Parse(dataValuesLines[3], CultureInfo.InvariantCulture));
                }
                catch
                {
                    throw new Exception($"{lineData}: data of vertice geometric parameters cannot convert from line to float values!");
                }
            }
            else if (dataValuesLines.Length == 3)
            {
                try
                {
                    verticeGeometric = new VerticeGeometric(float.Parse(dataValuesLines[0], CultureInfo.InvariantCulture),
                        float.Parse(dataValuesLines[1], CultureInfo.InvariantCulture), float.Parse(dataValuesLines[2], CultureInfo.InvariantCulture));
                }
                catch
                {
                    throw new Exception($"{lineData}: data of vertice geometric parameters cannot convert from line to float values!");
                }
            }
            else
            {
                throw new Exception($"{lineData}: data of vertice geometric parameters should have 4 or 3 parameters!");
            }

            return verticeGeometric;
        }

        private VerticeTexture ParseLineVerticeTexture(string lineData)
        {
            VerticeTexture verticeGeometric;

            string[] dataValuesLines = lineData.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (dataValuesLines.Length == 3)
            {
                try
                {
                    verticeGeometric = new VerticeTexture(float.Parse(dataValuesLines[0], CultureInfo.InvariantCulture),
                        float.Parse(dataValuesLines[1], CultureInfo.InvariantCulture), float.Parse(dataValuesLines[2], CultureInfo.InvariantCulture));
                }
                catch
                {
                    throw new Exception($"{lineData}: data of vertice texture parameters cannot convert from line to float values!");
                }
            }
            else if (dataValuesLines.Length == 2)
            {
                try
                {
                    verticeGeometric = new VerticeTexture(float.Parse(dataValuesLines[0], CultureInfo.InvariantCulture),
                        float.Parse(dataValuesLines[1], CultureInfo.InvariantCulture));
                }
                catch
                {
                    throw new Exception($"{lineData}: data of vertice texture parameters cannot convert from line to float values!");
                }
            }
            else if (dataValuesLines.Length == 1)
            {
                try
                {
                    verticeGeometric = new VerticeTexture(float.Parse(dataValuesLines[0], CultureInfo.InvariantCulture));
                }
                catch
                {
                    throw new Exception($"{lineData}: data of vertice texture parameters cannot convert from line to float values!");
                }
            }
            else
            {
                throw new Exception($"{lineData}: data of vertice texture parameters should have 3 or 2 or 1 parameters!");
            }

            return verticeGeometric;
        }

        private VectorFourCoord ParseLineVectorNormal(string lineData)
        {
            VectorFourCoord verticeGeometric;
            string[] dataValuesLines = lineData.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (dataValuesLines.Length == 3)
            {
                try
                {
                    verticeGeometric = new VectorFourCoord(float.Parse(dataValuesLines[0], CultureInfo.InvariantCulture),
                        float.Parse(dataValuesLines[1], CultureInfo.InvariantCulture), float.Parse(dataValuesLines[2], CultureInfo.InvariantCulture));
                }
                catch
                {
                    throw new Exception($"{lineData}: data of vector normal parameters cannot convert from line to float values!");
                }
            }
            else
            {
                throw new Exception($"{lineData}: data of vector normal parameters should have 3 parameters!");
            }

            return verticeGeometric;
        }

        private PolygonIndexes ParseLinePolygonIndexes(string lineData)
        {            
            PolygonIndexes polygonIndexes;
            
            string[] dataGroupsLines = lineData.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (dataGroupsLines.Length > 2)
            {
                int[] verticesGeometricIndexes = new int[dataGroupsLines.Length];
                int[] verticesTextureIndexes = new int[verticesGeometricIndexes.Length];
                int[] vectorsNormalIndexes = new int[verticesGeometricIndexes.Length];

                for (int i = 0; i < dataGroupsLines.Length; i++)
                {
                    string[] dataValuesLines = dataGroupsLines[i].Split("/");
                    bool isCorrectCountParameters = false;

                    try
                    {
                        if (dataValuesLines.Length == 1)
                        {
                            isCorrectCountParameters = true;
                            verticesGeometricIndexes[i] = int.Parse(dataValuesLines[0]);
                            verticesTextureIndexes[i] = 0;
                            vectorsNormalIndexes[i] = 0;
                        }
                        else if (dataValuesLines.Length == 2)
                        {
                            isCorrectCountParameters = true;
                            verticesGeometricIndexes[i] = int.Parse(dataValuesLines[0]);
                            verticesTextureIndexes[i] = int.Parse(dataValuesLines[1]);
                            vectorsNormalIndexes[i] = 0;
                        }
                        else if (dataValuesLines.Length == 3 && dataValuesLines[1] == "")
                        {
                            isCorrectCountParameters = true;
                            verticesGeometricIndexes[i] = int.Parse(dataValuesLines[0]);
                            verticesTextureIndexes[i] = 0;
                            vectorsNormalIndexes[i] = int.Parse(dataValuesLines[2]);
                        }
                        else if (dataValuesLines.Length == 3)
                        {
                            isCorrectCountParameters = true;
                            verticesGeometricIndexes[i] = int.Parse(dataValuesLines[0]);
                            verticesTextureIndexes[i] = int.Parse(dataValuesLines[1]);
                            vectorsNormalIndexes[i] = int.Parse(dataValuesLines[2]);
                        }
                    }
                    catch
                    {
                        throw new Exception($"{dataGroupsLines[i]}: data should have correct information about indexes of polygon!");
                    }
                                        
                    if (!isCorrectCountParameters)
                    {
                        throw new Exception($"{dataGroupsLines[i]}: data should have correct information about indexes of polygon!");
                    }    
                }

                polygonIndexes = new PolygonIndexes(verticesGeometricIndexes, verticesTextureIndexes, vectorsNormalIndexes);
            }
            else
            {
                throw new Exception($"{lineData}: data should have information about at least 3 vertices!");
            }

            return polygonIndexes;
        }

        private void ParseObjFileLine(string fileLine)
        {
            int spaceIndex = fileLine.IndexOf(' ');
            if (spaceIndex != -1)
            {
                string lineTypeData = fileLine.Substring(0, spaceIndex);

                TypeDataFileObj? typeData = TakeTypeLineDataOfObjFile(lineTypeData);
                if (typeData != null)
                {
                    string lineData = fileLine.Substring(spaceIndex + 1);

                    try
                    {
                        switch (typeData)
                        {
                            case TypeDataFileObj.VerticeGeometric:
                                VerticesGeometric.Add(ParseLineVerticeGeometric(lineData));
                                break;
                            case TypeDataFileObj.VerticeTexture:
                                VerticesTexture.Add(ParseLineVerticeTexture(lineData));
                                break;
                            case TypeDataFileObj.VectorNormal:
                                VectorsNormal.Add(ParseLineVectorNormal(lineData));
                                break;
                            case TypeDataFileObj.Polygon:
                                PoligonsIndexes.Add(ParseLinePolygonIndexes(lineData));
                                break;
                        }
                    }
                    catch
                    {
                        throw;   
                    }
                }
            }
        }

        private int[] ExchageArrayValuesLessThanNullToLengthMinusValue(int[] array, int length)
        {
            int lengthMinusOne = length - 1;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] < 0)
                {
                    array[i] = lengthMinusOne + array[i];
                }                
            }

            return array;
        }        

        private List<TriangleIndexes> ConvertPolygonsToTriangles()
        {
            List<TriangleIndexes> triangleIndexes = new List<TriangleIndexes>();

            for (int i = 0; i < PoligonsIndexes.Count; i++)
            {
                for (int j = 1; j < PoligonsIndexes[i].VerticesGeometricIndexes.Length - 1; j++)
                {
                    int jPlusOneIndex = j + 1;

                    triangleIndexes.Add(new TriangleIndexes(
                        new int[3] { PoligonsIndexes[i].VerticesGeometricIndexes[0], PoligonsIndexes[i].VerticesGeometricIndexes[j], PoligonsIndexes[i].VerticesGeometricIndexes[jPlusOneIndex] },
                        new int[3] { PoligonsIndexes[i].VerticesTextureIndexes[0], PoligonsIndexes[i].VerticesTextureIndexes[j], PoligonsIndexes[i].VerticesTextureIndexes[jPlusOneIndex] },
                        new int[3] { PoligonsIndexes[i].VectorsNormalIndexes[0], PoligonsIndexes[i].VectorsNormalIndexes[j], PoligonsIndexes[i].VectorsNormalIndexes[jPlusOneIndex] }
                    ));
                }
            }

            return triangleIndexes;
        }

        public RenderObject? ParseObjFile(string filePath)
        {
            VerticesGeometric = new List<VerticeGeometric>();
            VerticesTexture = new List<VerticeTexture>();
            VectorsNormal = new List<VectorFourCoord>();
            PoligonsIndexes = new List<PolygonIndexes>();            
            
            try
            {
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    string? fileLine = streamReader.ReadLine();
                    while (fileLine != null)
                    {                        
                        try
                        {
                            ParseObjFileLine(fileLine);
                        }
                        catch
                        {                            
                            throw;                            
                        }

                        fileLine = streamReader.ReadLine();
                    }

                    for (int i = 0; i < PoligonsIndexes.Count; i++)
                    {
                        PoligonsIndexes[i].VerticesGeometricIndexes = ExchageArrayValuesLessThanNullToLengthMinusValue(PoligonsIndexes[i].VerticesGeometricIndexes, VerticesGeometric.Count);
                        PoligonsIndexes[i].VerticesTextureIndexes = ExchageArrayValuesLessThanNullToLengthMinusValue(PoligonsIndexes[i].VerticesTextureIndexes, VerticesGeometric.Count);
                        PoligonsIndexes[i].VectorsNormalIndexes = ExchageArrayValuesLessThanNullToLengthMinusValue(PoligonsIndexes[i].VectorsNormalIndexes, VerticesGeometric.Count);
                    }

                    TrianglesIndexes = ConvertPolygonsToTriangles();
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (IOException)
            {
                throw;
            }
            catch
            {
                throw;
            }
            
            return new RenderObject(VerticesGeometric.ToArray(), VerticesTexture.ToArray(), VectorsNormal.ToArray(), TrianglesIndexes.ToArray());
        }
    }
}
