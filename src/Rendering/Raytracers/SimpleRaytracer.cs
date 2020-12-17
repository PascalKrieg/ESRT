using ESRT.Entities;
using ESRT.Entities.Lighting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Rendering
{
    /// <summary>
    /// A very simple Raytracer that only shades based on light angle.
    /// </summary>
    public class SimpleRaytracer : Raytracer
    {
        public SimpleRaytracer(Scene currentScene, RenderSettings settings) : base(currentScene, settings)
        {
        }

        protected override Color calculateColorPerLight(Ray ray, HitData hitPoint, ILight light, bool isCovered)
        {
            return Color.Black;
        }

        protected override Color calculateDefaultColor(Ray ray, HitData hitPoint)
        {
            return Math.Max(0, hitPoint.Normal * -1 * CurrentScene.MainCamera.ViewDirection) * hitPoint.Material.Color(hitPoint.TextureCoords);
        }

        protected override Color calculateNoHitColor(Ray ray)
        {
            return CurrentScene.Environment.GetEnvironmentColor(ray);
        }
    }
}
