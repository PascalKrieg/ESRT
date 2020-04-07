using ESRT.Entities;
using ESRT.Entities.Lighting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Rendering
{

    public abstract class Raytracer
    {
        public Scene CurrentScene { get; set; }
        public RenderSettings Settings { get; set; }

        public Raytracer(Scene currentScene, RenderSettings settings)
        {
            CurrentScene = currentScene;
            Settings = settings;
        }

        public Color Trace(Vector3 start, Vector3 direction)
        {
            Ray ray = new Ray(start, direction);
            if (CurrentScene.Intersect(ray, out HitData closestHit))
            {
                Color color = Color.Black;
                color += calculateDefaultColor(closestHit);

                CurrentScene.LightList.ForEach((ILight light) =>
                {
                    float LightDistance = light.Position.Distance(closestHit.HitPosition);
                    Vector3 shadowRayDirection = light.Position - closestHit.HitPosition;
                    shadowRayDirection.Normalize();

                    if (CurrentScene.Intersect(closestHit.HitPosition + 0.003f * shadowRayDirection, light.Position, out HitData shadowRayHit)) {
                        color += calculateColorPerLight(closestHit, light, false);
                    }
                    else
                    {
                        color += calculateColorPerLight(closestHit, light, true);
                    }
                });
                return color;
            }
            else
            {
                return calculateNoHitColor(ray);
            }
        }

        protected abstract Color calculateDefaultColor(HitData hitPoint);
        protected abstract Color calculateColorPerLight(HitData hitPoint, ILight light, bool isCovered);
        protected abstract Color calculateNoHitColor(Ray ray);

    }
}
