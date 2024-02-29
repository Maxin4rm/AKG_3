using DrawPolygons.Models.Primitives;
using DrawPolygons.Models.RenderElements;
using DrawPolygons.Services.Drawing;
using DrawPolygons.Services.ParserObjectFile;
using System.Drawing.Imaging;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.InteropServices;
using DrawPolygons.Services.SpaceTransform;
using DrawPolygons.Services;
using DrawPolygons.Services.Lighting;

namespace Lab1_DrawPolygons_SiSharp_V1
{
    public partial class FormMain : Form
    {
        private const string PATH_TO_OBJECT_FILE_DIRECTORY = "..\\..\\..";

        private string pathObjFile;

        private ParserObjFile parserObjFile;
        private RenderObject? renderObject;

        private Color colorBackground;
        private Color colorModelTriangles;

        private WorldSpaceTransform worldSpaceTransorm;
        private ObserverSpaceTransform observerSpaceTransform;
        private ProjectionSpaceTransform projectionSpaceTransform;
        private ViewportSpaceTransform viewportSpaceTransform;        

        private MatrixFourByFour matrixTranslation;
        private MatrixFourByFour matrixRotateZ;
        private MatrixFourByFour matrixRotateY;
        private MatrixFourByFour matrixRotateX;
        private MatrixFourByFour matrixScale;
        private MatrixFourByFour matrixObserver;
        private MatrixFourByFour matrixProjection;
        private MatrixFourByFour matrixViewport;

        private DrawingPixels drawingPixels;
        private LightSource lightSource;

        private VectorFourCoord vectorEye;
        private float backgroundLightingCoeff;
        private float surfaceGlossCoefficient;
        private float lightIntensivity;

        public static float ConvertDegreesToRadians(float angle)
        {
            return (float)((Math.PI / 180) * angle);
        }

        public FormMain()
        {
            InitializeComponent();

            pathObjFile = PATH_TO_OBJECT_FILE_DIRECTORY + "\\" + "model1.obj";
            parserObjFile = ParserObjFile.CreateInstance();
            renderObject = null;

            Bitmap bitmap = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height, PixelFormat.Format32bppArgb);
            Rectangle bitmapClientRectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            pictureBox.Image = bitmap;          

            VectorFourCoord vectorTranslation = new VectorFourCoord(0, 0, 0);
            VectorFourCoord vectorScale = new VectorFourCoord(1, 1, 1);
            float angleDegreesX = 0;
            float angleDegreesY = 0;
            float angleDegreesZ = 0;

            vectorEye = new VectorFourCoord(0, 6, 30);
            VectorFourCoord vectorTarget = new VectorFourCoord(0, 6, 0);
            VectorFourCoord vectorUp = new VectorFourCoord(0, 1, 0);

            float cameraHeight = pictureBox.Size.Height;
            float cameraWidth = pictureBox.Size.Width;
            float fovDegrees = 90;
            float zNear = 0.1f;
            float zFar = 100.0f;

            float windowHeight = pictureBox.Size.Height;
            float windowWidth = pictureBox.Size.Width;
            float xMin = 0;
            float yMin = 0;

            colorBackground = Color.FromArgb(255, 0, 0, 0);
            colorModelTriangles = Color.FromArgb(255, 255, 255, 255);

            worldSpaceTransorm = new WorldSpaceTransform(vectorTranslation, vectorScale, ConvertDegreesToRadians(angleDegreesX), ConvertDegreesToRadians(angleDegreesY), ConvertDegreesToRadians(angleDegreesZ));
            observerSpaceTransform = new ObserverSpaceTransform(vectorEye, vectorTarget, vectorUp);
            projectionSpaceTransform = new ProjectionSpaceTransform(cameraHeight, cameraWidth, ConvertDegreesToRadians(fovDegrees), zNear, zFar);
            viewportSpaceTransform = new ViewportSpaceTransform(windowWidth, windowHeight, xMin, yMin);

            matrixTranslation = worldSpaceTransorm.TakeMatrixTranslation();
            matrixRotateZ = worldSpaceTransorm.TakeMatrixRotateZ();
            matrixRotateY = worldSpaceTransorm.TakeMatrixRotateY();
            matrixRotateX = worldSpaceTransorm.TakeMatrixRotateX();
            matrixScale = worldSpaceTransorm.TakeMatrixScale();
            matrixObserver = observerSpaceTransform.TakeViewMatrix();
            matrixProjection = projectionSpaceTransform.TakeMatrixProjection();
            matrixViewport = viewportSpaceTransform.TakeMatrixViewport();

            drawingPixels = new DrawingPixels(pictureBox, bitmap, bitmapClientRectangle, ImageLockMode.ReadWrite);

            lightSource = new LightSource(new VectorFourCoord(40, 0, 20));
            backgroundLightingCoeff = 0.05f;
            surfaceGlossCoefficient = 100f;
            lightIntensivity = 100.0f;
        }

        private void DrawRenderObject(RenderObject renderObject)
        {
            MatrixFourByFour matrixModel = matrixTranslation.Myltiply(matrixRotateZ.Myltiply(
                matrixRotateY.Myltiply(matrixRotateX.Myltiply(matrixScale))));
            MatrixFourByFour matrixObserverProjection = matrixProjection.Myltiply(matrixObserver);

            PreparingBitmapData preparingBitmapData = drawingPixels.PreparingFillAndLockBitmapBuffer();

            Parallel.ForEach(renderObject.TrianglesIndexes, triangleIndex =>
            //for (int i = 0; i < renderObject.TrianglesIndexes.Length; i++)
            {
                VectorFourCoord[] vectorsTransformed = new VectorFourCoord[3];
                for (int j = 0; j < 3; j++)
                {
                    vectorsTransformed[j] = matrixModel.Myltiply(
                        new VectorFourCoord(
                            renderObject.VerticesGeometric[triangleIndex.VerticesGeometricIndexes[j] - 1].X,
                            renderObject.VerticesGeometric[triangleIndex.VerticesGeometricIndexes[j] - 1].Y,
                            renderObject.VerticesGeometric[triangleIndex.VerticesGeometricIndexes[j] - 1].Z,
                            renderObject.VerticesGeometric[triangleIndex.VerticesGeometricIndexes[j] - 1].W
                    ));
                }

                VectorFourCoord vectorTriangleNormal = vectorsTransformed[0].TakeVectorNormalWithoutW(vectorsTransformed[1], vectorsTransformed[2]);
                VectorFourCoord vectorLightDirection = vectorsTransformed[0].SubstractWithVectorWithoutW(lightSource.VectorLight).TakeNormalizedVectorWithoutW();
                VectorFourCoord vectorEyeDirection = vectorsTransformed[0].SubstractWithVectorWithoutW(vectorEye).TakeNormalizedVectorWithoutW();

                if (RejectingInvisibleTriangles.IsVisibleTriangle(vectorTriangleNormal, observerSpaceTransform.Eye, vectorsTransformed[0]))
                {                        
                    for (int j = 0; j < 3; j++)
                    {
                        vectorsTransformed[j] = matrixObserverProjection.Myltiply(vectorsTransformed[j]);
                        if (vectorsTransformed[j].W > 0)
                        {
                            vectorsTransformed[j].X = vectorsTransformed[j].X / vectorsTransformed[j].W;
                            vectorsTransformed[j].Y = vectorsTransformed[j].Y / vectorsTransformed[j].W;
                            vectorsTransformed[j].Z = vectorsTransformed[j].Z / vectorsTransformed[j].W;
                            vectorsTransformed[j].W = 1;
                            vectorsTransformed[j] = matrixViewport.Myltiply(vectorsTransformed[j]);
                        }
                        else
                        {
                            goto l1;
                        }
                    }

                    float coeff = backgroundLightingCoeff +
                        LambertLighting.TakeLightIntensityForTriangle(vectorLightDirection, vectorTriangleNormal) +
                        MirrorLighting.TakeMirrorLightIntensity(vectorLightDirection, vectorTriangleNormal, vectorEyeDirection, surfaceGlossCoefficient, lightIntensivity);
                    
                    Color colorTriangle = Color.FromArgb((byte)(Math.Min(coeff * colorModelTriangles.R, 255)), (byte)(Math.Min(coeff * colorModelTriangles.G, 255)), (byte)(Math.Min(coeff * colorModelTriangles.B, 255)));

                    DrawingPixels.DrawFilledTriangle(preparingBitmapData, colorTriangle, vectorsTransformed);
                }
                    
                l1:;
            }
            );

            DrawingPixels.FillBitmapBufferDrawBackground(preparingBitmapData, colorBackground);

            drawingPixels.ShowAndUnlockBitmapBuffer(preparingBitmapData);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                renderObject = parserObjFile.ParseObjFile(pathObjFile);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(
                    $"File \"{pathObjFile}\" is not found!",
                    "ParserObjFile error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly
                );

                Close();
                return;
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "Cannot read file \"{pathObjFile}\"!",
                    "ParserObjFile error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly
                );

                Close();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "ParserObjFile error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly
                );

                Close();
                return;
            }

            if (renderObject != null)
            {
                DrawRenderObject(renderObject);
            }
        }



        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            //while (renderObject != null)
            //{
            //    worldSpaceTransorm.AngleRadiansY = worldSpaceTransorm.AngleRadiansY - 0.05f;
            //    matrixRotateY = worldSpaceTransorm.TakeMatrixRotateY();
            //    DrawRenderObject(renderObject);
            //}
            if (renderObject != null)
            {
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
                {
                    worldSpaceTransorm.AngleRadiansY = worldSpaceTransorm.AngleRadiansY - 0.1f;
                    matrixRotateY = worldSpaceTransorm.TakeMatrixRotateY();
                }
                else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
                {
                    worldSpaceTransorm.AngleRadiansY = worldSpaceTransorm.AngleRadiansY + 0.1f;
                    matrixRotateY = worldSpaceTransorm.TakeMatrixRotateY();
                }
                else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
                {
                    worldSpaceTransorm.AngleRadiansX = worldSpaceTransorm.AngleRadiansX - 0.1f;
                    matrixRotateX = worldSpaceTransorm.TakeMatrixRotateX();
                }
                else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
                {
                    worldSpaceTransorm.AngleRadiansX = worldSpaceTransorm.AngleRadiansX + 0.1f;
                    matrixRotateX = worldSpaceTransorm.TakeMatrixRotateX();
                }

                DrawRenderObject(renderObject);
            }

            //worldSpaceTransorm.AngleRadiansY = worldSpaceTransorm.AngleRadiansY - 0.1f;
            //matrixRotateY = worldSpaceTransorm.TakeMatrixRotateY();
            //DrawRenderObject(renderObject);
        }
    }
}
