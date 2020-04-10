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

        Vector3 sidewaysVector;
        float distance;

        public Raytracer(Scene currentScene, RenderSettings settings)
        {
            CurrentScene = currentScene;
            Settings = settings;

            distance = (float)Math.Tan((Math.PI / 360) * CurrentScene.MainCamera.FieldOfView) * Settings.Resolution.width / 2;
            CurrentScene.MainCamera.Up.Normalize();
            CurrentScene.MainCamera.ViewDirection.Normalize();
            sidewaysVector = (-1 * CurrentScene.MainCamera.ViewDirection).CrossProduct(CurrentScene.MainCamera.Up);
            sidewaysVector.Normalize();
        }

        public Color CalculatePixelColor(int x, int y)
        {
            int xSteps = x - (int)(Settings.Resolution.width / 2);
            int ySteps = y - (int)(Settings.Resolution.height / 2);
            Vector3 result = -1 * distance * CurrentScene.MainCamera.ViewDirection + xSteps * sidewaysVector + ySteps * CurrentScene.MainCamera.Up;
            result.Normalize();
            return Trace(new Ray(CurrentScene.MainCamera.Position, result));
        }

        protected Color Trace(Ray ray, int recursionDepth = 0)
        {
            if (recursionDepth > Settings.RecursionDepth)
            {
                return Color.Black;
            }

            // Intersect ray with scene objects and save to closestHit
            if (!CurrentScene.Intersect(ray, out HitData closestHit))
                return calculateNoHitColor(ray);

            Color color = Color.Black;
            color += calculateDefaultColor(closestHit);

            // Check for each light if it is blocked by scene objects
            CurrentScene.LightList.ForEach((ILight light) =>
            {
                float LightDistance = light.Position.Distance(closestHit.HitPosition);
                Vector3 shadowRayDirection = light.Position - closestHit.HitPosition;
                shadowRayDirection.Normalize();

                if (CurrentScene.Intersect(closestHit.HitPosition + Settings.SecondaryRayOffset * shadowRayDirection, light.Position, out HitData shadowRayHit))
                {
                    color += calculateColorPerLight(closestHit, light, false);
                }
                else
                {
                    color += calculateColorPerLight(closestHit, light, true);
                }
            });

            if (closestHit.Material.Reflective(closestHit.TextureCoords) > 0.001f)
            {
                Vector3 reflectionDirection = ray.Direction.Reflect(closestHit.Normal);
                Ray reflectionRay = new Ray(closestHit.HitPosition + Settings.SecondaryRayOffset * reflectionDirection, reflectionDirection);
                color += closestHit.Material.Reflective(closestHit.TextureCoords) * Trace(reflectionRay, recursionDepth + 1);
            }

            return color;
        }

        protected abstract Color calculateDefaultColor(HitData hitPoint);
        protected abstract Color calculateColorPerLight(HitData hitPoint, ILight light, bool isCovered);
        protected abstract Color calculateNoHitColor(Ray ray);

    }
}
