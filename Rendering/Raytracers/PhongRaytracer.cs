using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRT.Entities;
using ESRT.Entities.Lighting;

namespace ESRT.Rendering
{
    /// <summary>
    /// A raytracer that implements the phong lighting model.
    /// </summary>
    public class PhongRaytracer : Raytracer
    {
        public float PhongExponent = 4;

        public PhongRaytracer(Scene currentScene, RenderSettings settings) : base(currentScene, settings)
        {
        }

        protected override Color calculateColorPerLight(Ray ray, HitData hitPoint, ILight light, bool isCovered)
        {
            if (!Settings.CastShadows)
                return Color.Black;

            Vector3 lightDirection = Vector3.Normalize(hitPoint.Position - light.Position);
            Color lightIntensity = (float)(1 / Math.Pow(Vector3.Distance(hitPoint.Position, light.Position), 2)) * light.GetIntensity(lightDirection);

            Color ambient = lightIntensity * hitPoint.Material.Ambient(hitPoint.TextureCoords);
            Color diffuse = Color.Black;
            Color specular = Color.Black;

            if (!isCovered)
            {
                diffuse = lightIntensity
                    * Math.Max(hitPoint.Normal * (-1 * lightDirection), 0)
                    * hitPoint.Material.Diffuse(hitPoint.TextureCoords);

                specular = lightIntensity
                    * hitPoint.Material.Specular(hitPoint.TextureCoords)
                    * (float)Math.Pow((-1 * CurrentScene.MainCamera.ViewDirection).CosAngle(lightDirection.Reflect(hitPoint.Normal)), PhongExponent);

            }

            Color lightInflux = diffuse + ambient + specular;
            Color color = hitPoint.Material.Color(hitPoint.TextureCoords);
            return new Color(Math.Min(lightInflux.r, color.r), Math.Min(lightInflux.g, color.g), Math.Min(lightInflux.b, color.b));
        }

        protected override Color calculateDefaultColor(Ray ray, HitData hitPoint)
        {
            if (Settings.CastShadows)
            {
                return Color.Black;
            }
            return hitPoint.Material.Color(hitPoint.TextureCoords);
        }

        protected override Color calculateNoHitColor(Ray ray)
        {
            return CurrentScene.Environment.GetEnvironmentColor(ray);
        }
    }
}
