using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities.SceneObjects.Polygons
{
    /// <summary>
    /// Class that contains information for vertices.
    /// </summary>
    public class VertexData
    {
        /// <summary>
        /// The position of the vertex in world space.
        /// </summary>
        public Vector3 Position { get; private set; }
        /// <summary>
        /// The normal of the position, averaged between the connected triangles and weighed by their surface area.
        /// This value should only be modified when modifying the polygon the vertex belongs to.
        /// </summary>
        public Vector3 Normal { get; set; }

        /// <summary>
        /// Constructs a new VertexData
        /// </summary>
        /// <param name="position">The position of the vertex.</param>
        /// <param name="normal">The normal of the vertex.</param>
        public VertexData(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }
    }
}
