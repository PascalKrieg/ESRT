using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities.Materials
{
    public class GradientMaterial : Material
    {
        private Vector3 color;
        private float ambient;
        private float diffuse;
        private float specular;
        private float reflective;
        private float transmissive;

        public override float Ambient(float u, float v)
        {
            throw new NotImplementedException();
        }

        public override Color Color(float u, float v)
        {
            return new Color(u, u, v);
        }

        public override float Diffuse(float u, float v)
        {
            throw new NotImplementedException();
        }

        public override float Reflective(float u, float v)
        {
            throw new NotImplementedException();
        }

        public override float Specular(float u, float v)
        {
            throw new NotImplementedException();
        }

        public override float Transmissive(float u, float v)
        {
            throw new NotImplementedException();
        }

        public GradientMaterial(Vector3 color, Shading shadingMode, float ambient, float diffuse, float specular, float reflective, float transmissive) : base(shadingMode)
        {
            this.color = color;
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
            this.reflective = reflective;
            this.transmissive = transmissive;
        }
    }
}
