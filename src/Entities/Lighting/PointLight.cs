using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities.Lighting
{
    /// <summary>
    /// A point light that has the same intensity in every direction.
    /// </summary>
    public class PointLight : ILight
    {
        /// <summary>
        /// The light intensity as a color.
        /// </summary>
        private Color lightIntensity;

        public Vector3 Position { get; set; }

        /// <summary>
        /// Constructs a point light with given color/intensity and position
        /// </summary>
        /// <param name="lightIntensity">The intensity of the light as color.</param>
        /// <param name="position">The position of the light.</param>
        public PointLight(Color lightIntensity, Vector3 position)
        {
            this.lightIntensity = lightIntensity;
            Position = position;
        }

        public Color GetIntensity(Vector3 outDirection)
        {
            return lightIntensity;
        }
    }
}
