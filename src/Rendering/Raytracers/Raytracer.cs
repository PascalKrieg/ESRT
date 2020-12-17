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
    /// Abstract class that can be inherited from to create custom raytracing behaviour.
    /// The class handles ray generation, tracing and light calculation.
    /// </summary>
    public abstract class Raytracer
    {
        /// <summary>
        /// The scene that will be rendered by this raytracer instance.
        /// </summary>
        public Scene CurrentScene { get; private set; }
        /// <summary>
        /// The settings used to render the scene.
        /// </summary>
        public RenderSettings Settings { get; private set; }

        // used for ray generation
        Vector3 sidewaysVector;
        float distance;

        /// <summary>
        /// Constructs a Raytracer to render a scene using given settings.
        /// </summary>
        /// <param name="currentScene">The scene that will be rendered by this raytracer instance.</param>
        /// <param name="settings">The settings used to render the scene.</param>
        public Raytracer(Scene currentScene, RenderSettings settings)
        {
            CurrentScene = currentScene;
            Settings = settings;

            distance = Settings.Resolution.height / ((float)Math.Tan((Math.PI / 360) * CurrentScene.MainCamera.FieldOfView) * 2);
            CurrentScene.MainCamera.Up.Normalize();
            CurrentScene.MainCamera.ViewDirection.Normalize();
            sidewaysVector = CurrentScene.MainCamera.ViewDirection.CrossProduct(CurrentScene.MainCamera.Up);
            sidewaysVector.Normalize();
        }

        /// <summary>
        /// Calculates the color of the given pixel coordinates.
        /// </summary>
        /// <param name="x">x coordinate of the pixel.</param>
        /// <param name="y">y coordinate of the pixel.</param>
        /// <returns>Returns the color of the pixel at (x, y).</returns>
        public Color CalculatePixelColor(int x, int y)
        {
            int xSteps = x - (int)(Settings.Resolution.width / 2);
            int ySteps = y - (int)(Settings.Resolution.height / 2);
            Vector3 result = distance * CurrentScene.MainCamera.ViewDirection + xSteps * sidewaysVector + ySteps * - 1 * CurrentScene.MainCamera.Up;
            result.Normalize();
            return Trace(new Ray(CurrentScene.MainCamera.Position, result));
        }

        /// <summary>
        /// Traces a ray and calculates the color using the methods implemented in raytracer implementations.
        /// </summary>
        /// <param name="ray">The ray that will be traced through the scene.</param>
        /// <param name="recursionDepth">The current recursion depth. Optional, but should never be given at the first call.</param>
        /// <returns>Returns the color that results when tracing the ray.</returns>
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
            color += calculateDefaultColor(ray, closestHit);

            // Check for each light if it is blocked by scene objects and calculate color contribution
            CurrentScene.LightList.ForEach((ILight light) =>
            {
                //Offset start position the prevent casting of a shadow on itself.
                Vector3 shadowRayDirection = light.Position - closestHit.Position;
                shadowRayDirection.Normalize();
                Vector3 offsettedStartPosition = closestHit.Position + Settings.SecondaryRayOffset * shadowRayDirection;

                bool isOccluded = CurrentScene.Intersect(offsettedStartPosition, light.Position, out HitData shadowRayHit, true);
                color += calculateColorPerLight(ray, closestHit, light, isOccluded);
            });

            // Handle reflective materials
            if (closestHit.Material.Reflective(closestHit.TextureCoords) > 0.001f)
            {
                Vector3 reflectionDirection = ray.Direction.Reflect(closestHit.Normal);
                Ray reflectionRay = new Ray(closestHit.Position + Settings.SecondaryRayOffset * reflectionDirection, reflectionDirection);
                color += closestHit.Material.Reflective(closestHit.TextureCoords) * Trace(reflectionRay, recursionDepth + 1);
            }

            return color;
        }

        /// <summary>
        /// Calculates the color that will be unconditionally added on every surface hit. 
        /// </summary>
        /// <param name="ray">The ray that led to the hit.</param>
        /// <param name="hitPoint">The hit information.</param>
        /// <returns>Returns the color that will be added on every surface hit.</returns>
        protected abstract Color calculateDefaultColor(Ray ray, HitData hitPoint);
        /// <summary>
        /// Calculates the color that will be added on a surface hit for a specific light.
        /// </summary>
        /// <param name="ray">The ray that led to the hit.</param>
        /// <param name="hitPoint">The hit information.</param>
        /// <param name="light">The light of which the contributuib is calculated.</param>
        /// <param name="isCovered">True, if the light is occluded by another scene object.</param>
        /// <returns>Returns the color that will be added on a surface hit for a specific light.</returns>
        protected abstract Color calculateColorPerLight(Ray ray, HitData hitPoint, ILight light, bool isCovered);
        /// <summary>
        /// Calculates the color the pixel will have when there is no scene object hit.
        /// </summary>
        /// <param name="ray">The ray that was traced.</param>
        /// <returns>Returns the color the pixel will have when there is no scene object hit.</returns>
        protected abstract Color calculateNoHitColor(Ray ray);

    }
}
