using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ESRT.Entities;
using ESRT.Entities.Lighting;

namespace ESRT.Rendering
{
    public class Scene
    {
        public Color SkyColor { get; set; }
        public Camera MainCamera { get; set; }
        public List<IIntersectable> ObjectList { get; set; }
        public List<ILight> LightList { get; set; }

        public Scene(Color skyColor, Camera mainCamera)
        {
            SkyColor = skyColor;
            MainCamera = mainCamera;
            ObjectList = new List<IIntersectable>();
            LightList = new List<ILight>();
        }

        public bool Intersect(Ray ray, out HitData hitData)
        {
            // TODO: Use acceleration Data structures
            HitData closestHit = HitData.NoHit;
            HitData lastHit;
            ObjectList.ForEach((IIntersectable) =>
            {
                if (IIntersectable.Intersect(ray.Start, ray.Direction, out lastHit))
                {
                    if (closestHit.exists() && (lastHit.HitPosition.Distance(ray.Start) < closestHit.HitPosition.Distance(ray.Start)))
                    {
                        closestHit = lastHit;
                    }
                    else
                    {
                        closestHit = lastHit;
                    }
                }
            });

            hitData = closestHit;
            return hitData.exists();
        }

        public bool Intersect(Vector3 startPosition, Vector3 targetPosition, out HitData hitData)
        {
            Ray ray = new Ray(startPosition, targetPosition - startPosition);

            if (Intersect(ray, out HitData hit))
            {
                hitData = hit;
                return (startPosition.Distance(hit.HitPosition) < startPosition.Distance(targetPosition));
            } 
            else
            {
                hitData = HitData.NoHit;
                return false;
            }
        }
    }
}
