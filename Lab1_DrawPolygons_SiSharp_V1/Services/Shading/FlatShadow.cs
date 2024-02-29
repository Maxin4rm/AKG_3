using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPolygons.Services.Shading
{
    public class FlatShadow
    {
       /* public struct Vertex
        {
            public float x, y, z;  // Координаты вершины
            public float r, g, b;  // Цвет вершины
        }

        public static void GouraudShadingAlgorithm(Vertex v0, Vertex v1, Vertex v2)
        {
            // Расчет нормалей для каждой вершины (может потребоваться нормализация)
            // Здесь предполагается, что нормали уже известны
            // Для упрощения давайте предположим, что нормали равны цветам вершин

            // Расчет цветов вершин
            double intensity0 = (v0.r + v0.g + v0.b) / 3.0;
            double intensity1 = (v1.r + v1.g + v1.b) / 3.0;
            double intensity2 = (v2.r + v2.g + v2.b) / 3.0;

            // Расчет шагов интенсивности для каждой из сторон треугольника
            double stepX0 = (intensity1 - intensity0) / (v1.x - v0.x);
            double stepX1 = (intensity2 - intensity0) / (v2.x - v0.x);

            // Нахождение пределов сканирования
            double intensityLeft = intensity0;
            double intensityRight = intensity0;

            // Начало сканирования
            for (int y = (int)v0.y; y <= (int)v1.y; y++)
            {
                for (int x = (int)v0.x; x <= (int)v1.x; x++)
                {
                    // Рендеринг пикселя соответственно интенсивности и цвету
                    RenderPixel(x, y, intensityLeft, v0);
                }
                intensityLeft += stepX0;
            }

            intensityLeft = intensity1;

            for (int y = (int)v1.y; y <= (int)v2.y; y++)
            {
                for (int x = (int)v1.x; x <= (int)v2.x; x++)
                {
                    // Рендеринг пикселя соответственно интенсивности и цвету
                    RenderPixel(x, y, intensityLeft, v1);
                }
                intensityLeft += stepX1;
            }
        }

        public static void RenderPixel(int x, int y, double intensity, Vertex v)
        {
            // Здесь происходит рендеринг пикселя на экране, используя значение intensity и цвет из вершины v
            Console.WriteLine($"Rendering pixel at ({x}, {y}) with intensity: {intensity}, color: ({v.r}, {v.g}, {v.b})");
        }

        public static void Main()
        {
            // Пример использования
            Vertex v0 = new Vertex { x = 10, y = 10, z = 0, r = 255, g = 0, b = 0 };
            Vertex v1 = new Vertex { x = 50, y = 100, z = 0, r = 0, g = 255, b = 0 };
            Vertex v2 = new Vertex { x = 90, y = 10, z = 0, r = 0, g = 0, b = 255 };

            GouraudShadingAlgorithm(v0, v1, v2);
        }*/
    }
}
