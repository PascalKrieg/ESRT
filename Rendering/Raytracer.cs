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
            HitData closestHit = null;
            HitData lastHit;
            CurrentScene.ObjectList.ForEach((IIntersectable) =>
            {
                if (IIntersectable.Intersect(start, direction, out lastHit))
                {
                    if (closestHit != null)
                    {
                        if (lastHit.HitPosition.Distance(CurrentScene.MainCamera.Position) < closestHit.HitPosition.Distance(CurrentScene.MainCamera.Position))
                        {
                            closestHit = lastHit;
                        }
                    }
                    else
                    {
                        closestHit = lastHit;
                    }
                }
            });

            if (closestHit != null)
            {
                Color color = Color.Black;

                color += calculateDefaultColor(closestHit);

                CurrentScene.LightList.ForEach((ILight light) =>
                {
                    bool isCovered = false;
                    foreach(IIntersectable obj in CurrentScene.ObjectList)
                    {
                        HitData hit = null;
                        float LightDistance = light.Position.Distance(closestHit.HitPosition);
                        Vector3 shadowRayDirection = light.Position - closestHit.HitPosition;
                        shadowRayDirection.Normalize();
                        if (obj.Intersect(closestHit.HitPosition + 0.003f * shadowRayDirection, shadowRayDirection, out hit))
                        {
                            if (hit.HitPosition.Distance(closestHit.HitPosition) < LightDistance)
                            {
                                isCovered = true;
                                break;
                            }   
                        }
                    }

                    color += calculateColorPerLight(closestHit, light, isCovered);

                });

                return color;
            }
            else
            {
                return calculateNoHitColor(new Ray(start, direction));
            }
        }

        protected abstract Color calculateDefaultColor(HitData hitPoint);
        protected abstract Color calculateColorPerLight(HitData hitPoint, ILight light, bool isCovered);
        protected abstract Color calculateNoHitColor(Ray ray);

    }
}
