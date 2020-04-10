using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRT.Entities.Materials;

namespace ESRT.Entities
{
    /// <summary>
    /// A class that contains all information about a hit position on an object.
    /// </summary>
    public class HitData
    {
        /// <summary>
        /// The position of the hit point in the world.
        /// </summary>
        public Vector3 Position { get; private set; }
        /// <summary>
        /// The normal vector of the surface that was hit.
        /// </summary>
        public Vector3 Normal { get; private set; }
        /// <summary>
        /// The texture coordinatesof the hit point on the Material.
        /// </summary>
        public (float u, float v) TextureCoords { get; private set; }
        /// <summary>
        /// The Material of the object that was hit.
        /// </summary>
        public Material Material { get; private set; }

        /// <summary>
        /// Constructs a new HitData object.
        /// </summary>
        /// <param name="position">The position of the hit point in the world.</param>
        /// <param name="normal">The normal vector of the surface that was hit.</param>
        /// <param name="textureCoords">The texture coordinatesof the hit point on the Material.</param>
        /// <param name="material">The Material of the object that was hit.</param>
        public HitData(Vector3 position, Vector3 normal, (float u, float v) textureCoords, Material material)
        {
            Position = position;
            Normal = normal;
            TextureCoords = textureCoords;
            Material = material;
        }

        /// <summary>
        /// Checks if this is a valid hit data.
        /// This should always be the case, because NoHit should only be used in mandatory out parameters when no hit occurs.
        /// </summary>
        /// <returns></returns>
        public bool exists()
        {
            return this.GetHashCode() != noHit.GetHashCode();
        }
        private static HitData noHit;
        /// <summary>
        /// Represents a non existant hit point. Should never be encountered, 
        /// because NoHit should only be returned in mandatory out parameters when no hit occurs.
        /// </summary>
        public static HitData NoHit
        {
            get
            {
                if (noHit == null)
                {
                    // Pink constant material
                    Material mat = new ConstantMaterial(new Color(1, 0.07f, 0.576f), 1, 0, 0, 0, 0);
                    noHit = new HitData(Vector3.Zero, Vector3.Up, (0, 0), mat);
                }
                return noHit;
            }
        }
    }
}
