using ESRT.Entities;
using ESRT.Entities.Lighting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Rendering
{
    public class SimpleRaytracer : Raytracer
    {
        public SimpleRaytracer(Scene currentScene, RenderSettings settings) : base(currentScene, settings)
        {
        }

        protected override Color calculateColorPerLight(HitData hitPoint, ILight light, bool isCovered)
        {
            return Color.Black;
        }

        protected override Color calculateDefaultColor(HitData hitPoint)
        {
            return Math.Max(0, (hitPoint.Normal * CurrentScene.MainCamera.ViewDirection)) * hitPoint.Material.Color(hitPoint.TextureCoords.u, hitPoint.TextureCoords.v);
        }

        protected override Color calculateNoHitColor(Ray ray)
        {
            return CurrentScene.SkyColor;
        }
    }
}
