using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities.Materials
{
    public abstract class Material
    {
        public abstract Color Color(float u, float v);
        public Color Color((float u, float v) textureCoords)
        {
            return Color(textureCoords.u, textureCoords.v);
        }

        public abstract float Ambient(float u, float v);
        public float Ambient((float u, float v) textureCoords)
        {
            return Ambient(textureCoords.u, textureCoords.v);
        }

        public abstract float Diffuse(float u, float v);
        public float Diffuse((float u, float v) textureCoords)
        {
            return Diffuse(textureCoords.u, textureCoords.v);
        }

        public abstract float Specular(float u, float v);
        public float Specular((float u, float v) textureCoords)
        {
            return Specular(textureCoords.u, textureCoords.v);
        }

        public abstract float Transmissive(float u, float v);
        public float Transmissive((float u, float v) textureCoords)
        {
            return Transmissive(textureCoords.u, textureCoords.v);
        }

        public abstract float Reflective(float u, float v);
        public float Reflective((float u, float v) textureCoords)
        {
            return Reflective(textureCoords.u, textureCoords.v);
        }
    }
}
