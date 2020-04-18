using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities.SceneObjects.Polygons
{
    public class PolygonObject : IRenderableObject
    {
        private List<Triangle> triangles = new List<Triangle>();

        public bool CastShadows { get; set; }
        public Vector3 Position { get; set; }

        public PolygonObject(List<Triangle> triangles, bool castShadows, Vector3 position, Vector3 rotation)
        {
            this.triangles = triangles;
            CastShadows = castShadows;
            Position = position;
            Position = rotation;
        }

        public bool Intersect(Ray ray, out HitData hitData)
        {
            // Outsource this into the scene to enable accelerating data structures.
            Ray offsettedRay = new Ray(ray.Start - Position, ray.Direction);
            HitData closestHit = HitData.NoHit;
            HitData lastHit;
            triangles.ForEach((IIntersectable) =>
            {
                if (IIntersectable.Intersect(offsettedRay, out lastHit))
                {
                    HitData offsettedHit = new HitData(lastHit.Position + Position, lastHit.Normal, lastHit.TextureCoords, lastHit.Material);
                    if (!closestHit.exists() && lastHit.exists())
                    {
                        closestHit = offsettedHit;
                        return;
                    }
                    if (offsettedRay.Start.Distance(offsettedHit.Position) < offsettedRay.Start.Distance(closestHit.Position))
                    {
                        closestHit = offsettedHit;
                    }
                }
            });

            hitData = closestHit;
            return hitData.exists();
        }
    }
}
