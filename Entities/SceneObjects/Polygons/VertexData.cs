using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities.SceneObjects.Polygons
{
    public class VertexData
    {
        public Vector3 Position { get; private set; }
        public Vector3 Normal { get; set; }

        public VertexData(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }
    }
}
