using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities.Lighting
{
    public class PointLight : ILight
    {
        private Color lightIntensity;
        public Vector3 Position { get; set; }

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
