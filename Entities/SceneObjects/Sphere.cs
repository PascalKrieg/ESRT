using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRT.Entities.Materials;

namespace ESRT.Entities
{
    public class Sphere : IIntersectable
    {
        public Material material { get; private set;}

        public Vector3 Center { get; set; }

        public float Radius { get; set; }

        public Sphere(Material material, Vector3 center, float radius)
        {
            this.material = material;
            Center = center;
            Radius = radius;
        }

        public bool Intersect(Vector3 incomingStart, Vector3 incomingDirection, out HitData hitData)
        {
            bool hitFound = false;

            float a = incomingDirection * incomingDirection;
            float b = (2.0f * incomingDirection) * (incomingStart - Center);
            float c = (incomingStart - Center) * (incomingStart - Center) - Radius * Radius;

            float disc = b * b - 4 * a * c;
            float sqrtDisc = (float)Math.Sqrt(disc);

            float t = 0;

            if (disc == 0)
            {
                t = -b / (2 * a);
                hitFound = true;
            }
            else if (disc < 0)
            {
                hitFound = false;
            }
            else
            {
                float t1 = (-b + sqrtDisc) / (2 * a);
                float t2 = (-b - sqrtDisc) / (2 * a);

                if (t1 > 0 && t2 > 0)
                {
                    t = Math.Min(t1, t2);
                    hitFound = true;
                }
                else if (t1 < 0 && t2 < 0)
                {
                    hitFound = false;
                }
                else
                {
                    if (t1 > 0)
                    {
                        t = t1;
                        hitFound = true;
                    }
                    else
                    {
                        t = t2;
                        hitFound = true;
                    }
                }
            }

            if (hitFound)
            {
                Vector3 hitPosition = incomingStart + t * incomingDirection;
                Vector3 normal = Vector3.Normalize(hitPosition - Center);
                (float u, float v) texCoords = normal.CalculateAzimut();
                texCoords.u /= 2 * (float)Math.PI;
                texCoords.v = (texCoords.v + (float)Math.PI / 2) / (2 * (float)Math.PI);

                hitData = new HitData(hitPosition, normal, texCoords, material);
            } else
            {
                hitData = HitData.NoHit;
            }
            return hitFound;
        }
    }
}
