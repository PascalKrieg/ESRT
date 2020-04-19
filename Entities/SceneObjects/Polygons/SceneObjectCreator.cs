using ESRT.Entities.Materials;
using ESRT.Entities.SceneObjects.Polygons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities
{
    /// <summary>
    /// Static helper class for simply creating complex polygon objects.
    /// </summary>
    public static class SceneObjectCreator
    {

        /// <summary>
        /// Constructs a new polygon object.
        /// </summary>
        /// <param name="material">The material of the object.</param>
        /// <param name="vertices">Vector3 array containing all vertices that will be used to construct the object.</param>
        /// <param name="triangleVertexIndices">Array containing 3-tuples of integers.
        /// Each tuple represents one triangle that is constructed out of the vertices in the vertices array at the three indices. </param>
        /// <param name="triangleTextureCoords">Defines the texture coordinates for each vertex in every triangle. Optional.</param>
        /// <returns>The constructed polygon object.</returns>
        public static IRenderableObject Construct(Material material,
            Vector3[] vertices,
            (int A, int B, int C)[] triangleVertexIndices,
            ((float u, float v) aTexCoords, (float u, float v) bTexCoords, (float u, float v) cTexCoords)[] triangleTextureCoords)
        {
            if (triangleVertexIndices.Length != triangleTextureCoords.Length)
                throw new ArgumentException("TriangleVertexIndices and TriangleTextureCoords have to be the same length.");

            List<Triangle> triangleList = new List<Triangle>();
            Dictionary<Vector3, VertexData> vertexDictionary = new Dictionary<Vector3, VertexData>();
            foreach(Vector3 vertex in vertices)
            {
                vertexDictionary.Add(vertex, new VertexData(vertex, Vector3.Zero));
            }

            // Add triangles
            for (int i = 0; i < triangleVertexIndices.Length; i++)
            {
                (int A, int B, int C) vertexIndices = triangleVertexIndices[i];
                if (vertexIndices.A > vertices.Length || vertexIndices.B > vertices.Length || vertexIndices.C > vertices.Length)
                    throw new ArgumentException("Vertex index referenced a vertex that does not exist,");

                vertexDictionary.TryGetValue(vertices[vertexIndices.A], out VertexData A_Data);
                vertexDictionary.TryGetValue(vertices[vertexIndices.B], out VertexData B_Data);
                vertexDictionary.TryGetValue(vertices[vertexIndices.C], out VertexData C_Data);

                Triangle triangle = new Triangle(material, true, A_Data, B_Data, C_Data,
                    triangleTextureCoords[i].aTexCoords,
                    triangleTextureCoords[i].bTexCoords,
                    triangleTextureCoords[i].cTexCoords);

                triangleList.Add(triangle);
            }


            List<VertexData> vertexDataList = new List<VertexData>();

            // Calculate Normals for flat shading
            foreach (VertexData vertexData in vertexDictionary.Values)
            {
                List<Triangle> connectedTriangles = new List<Triangle>();
                foreach (Triangle triangle in triangleList)
                {
                    if (triangle.A == vertexData || triangle.B == vertexData || triangle.C == vertexData)
                    {
                        connectedTriangles.Add(triangle);
                    }
                }
                if (connectedTriangles.Count == 0)
                    continue;

                vertexDataList.Add(vertexData);

                float totalArea = 0;
                foreach (Triangle triangle in connectedTriangles)
                {
                    totalArea += triangle.SurfaceArea;
                }
                foreach (Triangle triangle in connectedTriangles)
                {
                    vertexData.Normal += (triangle.SurfaceArea / totalArea) * triangle.FlatNormal;
                }
            }

            return new PolygonObject(triangleList, true, Vector3.Zero, Vector3.Zero);
        }

        /// <summary>
        /// Constructs a new polygon object.
        /// </summary>
        /// <param name="material">The material of the object.</param>
        /// <param name="vertices">Vector3 array containing all vertices that will be used to construct the object.</param>
        /// <param name="triangleVertexIndices">Array containing 3-tuples of integers.
        /// Each tuple represents one triangle that is constructed out of the vertices in the vertices array at the three indices. </param>
        /// <returns>The constructed polygon object.</returns>
        public static IRenderableObject Construct(Material material,
            Vector3[] vertices,
            (int A, int B, int C)[] triangleVertexIndices)
        {
            int triangleAmount = triangleVertexIndices.Length;
            var triangleTextureCoords = new ((float, float), (float, float), (float, float))[triangleAmount];
            for (int i = 0; i < triangleAmount; i++)
            {
                triangleTextureCoords[i] = ((0, 0), (0, 0), (0, 0));
            }
            return Construct(material, vertices, triangleVertexIndices, triangleTextureCoords);
        }

        /// <summary>
        /// Creates a cube.
        /// </summary>
        /// <param name="material">The material of the cube.</param>
        /// <param name="position">The position of the cube in world space.</param>
        /// <param name="sideLength">The side length of the cube.</param>
        /// <returns>Returns the constructed cube.</returns>
        public static IRenderableObject CreateCube(Material material, Vector3 position, Vector3 sideLength)
        {
            Vector3 halfSideLength = 0.5f * sideLength;
            Vector3[] vertexArray = new Vector3[]
            {
                new Vector3(  halfSideLength.x, -halfSideLength.y,  halfSideLength.z),
                new Vector3(  halfSideLength.x, -halfSideLength.y, -halfSideLength.z),
                new Vector3(  halfSideLength.x,  halfSideLength.y, -halfSideLength.z),
                new Vector3(  halfSideLength.x,  halfSideLength.y,  halfSideLength.z),

                new Vector3( -halfSideLength.x, -halfSideLength.y,  halfSideLength.z),
                new Vector3( -halfSideLength.x, -halfSideLength.y, -halfSideLength.z),
                new Vector3( -halfSideLength.x,  halfSideLength.y, -halfSideLength.z),
                new Vector3( -halfSideLength.x,  halfSideLength.y,  halfSideLength.z),
            };

            (int A, int B, int C)[] triangleVertexIndices = new (int A, int B, int C)[]
            {
                (0, 1, 3), (3, 1, 2),
                (0, 5, 1), (0, 4, 5),
                (4, 7, 5), (7, 6, 5),
                (7, 3, 6), (6, 3, 2),
                (4, 0, 7), (7, 0, 3),
                (1, 5, 6), (6, 2, 1)
            };

            IRenderableObject result = Construct(material, vertexArray, triangleVertexIndices);
            result.Position = position;
            return result;
        }

        /// <summary>
        /// Creates a plane.
        /// </summary>
        /// <param name="material">The material of the plane.</param>
        /// <param name="position">The position of the plane in world space.</param>
        /// <param name="sizeX">The side length of the plane in x direction.</param>
        /// <param name="sizeZ">The side length of the plane in z direction.</param>
        /// <returns>Returns the constructed plane.</returns>
        public static IRenderableObject CreatePlane(Material material, Vector3 position, float sizeX, float sizeZ)
        {
            float halfSideLengthX = 0.5f * sizeX;
            float halfSideLengthZ = 0.5f * sizeZ;
            Vector3[] vertexArray = new Vector3[]
            {
                new Vector3(  halfSideLengthX,  0,  halfSideLengthZ),
                new Vector3(  halfSideLengthX,  0, -halfSideLengthZ),
                new Vector3( -halfSideLengthX,  0, -halfSideLengthZ),
                new Vector3( -halfSideLengthX,  0,  halfSideLengthZ),
            };

            (int A, int B, int C)[] triangleVertexIndices = new (int A, int B, int C)[]
            {
                (0, 1, 3), (3, 1, 2)
            };

            IRenderableObject result = Construct(material, vertexArray, triangleVertexIndices);
            result.Position = position;
            return result;
        }
    }
}
