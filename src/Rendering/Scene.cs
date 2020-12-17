using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ESRT.Entities;
using ESRT.Entities.Lighting;
using ESRT.Environment;

namespace ESRT.Rendering
{
    /// <summary>
    /// Class that contains all scene objects and lights, aswell as other scene related information.
    /// This class also handles data structures to accelerate intersection tests.
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// The environment that can be used to calculate the color of the surroundings.
        /// </summary>
        public IEnvironment Environment { get; set; }

        /// <summary>
        /// The main camera that will be used to render this scene.
        /// </summary>
        public Camera MainCamera { get; set; }

        /// <summary>
        /// The list of all Visible objects contained in the scene.
        /// </summary>
        public List<IIntersectable> ObjectList { get; set; }

        /// <summary>
        /// The list of all light sources contained in the scene.
        /// </summary>
        public List<ILight> LightList { get; set; }

        /// <summary>
        /// Creates an empty scene, containing only a camera and an environment surrounding it.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="mainCamera">The camera used to render.</param>
        public Scene(IEnvironment environment, Camera mainCamera)
        {
            Environment = environment;
            MainCamera = mainCamera;
            ObjectList = new List<IIntersectable>();
            LightList = new List<ILight>();
        }

        /// <summary>
        /// Intersect a ray with scene objects. Writes out hit data.
        /// </summary>
        /// <param name="ray">The ray that will be intersected with the scene.</param>
        /// <param name="hitData">The hit data of the closest hit that will be written out if there is an intersection.
        /// Will output HitData.NoHit otherwise and can be checked with the exists() method on the object.</param>
        /// <returns>Returns true, if there is an intersection with at least one scene object, false otherwise.</returns>
        public bool Intersect(Ray ray, out HitData hitData, bool isShadowRay = false)
        {
            // TODO: Use acceleration Data structures
            HitData closestHit = HitData.NoHit;
            HitData lastHit;
            ObjectList.ForEach((IIntersectable) =>
            {
                if (isShadowRay && IIntersectable.CastShadows == false)
                    return;

                if (IIntersectable.Intersect(ray, out lastHit))
                {
                    if (!closestHit.exists() && lastHit.exists())
                    {
                        closestHit = lastHit;
                        return;
                    }
                    if (ray.Start.Distance(lastHit.Position) < ray.Start.Distance(closestHit.Position))
                    {
                        closestHit = lastHit;
                    }
                }
            });

            hitData = closestHit;
            return hitData.exists();
        }

        /// <summary>
        /// Intersect a line between two positions with scene objects. Writes out hit data.
        /// </summary>
        /// <param name="startPosition">The start position of the line.</param>
        /// <param name="targetPosition">The end position of the line.</param>
        /// <param name="hitData">The hit data of the closest hit that will be written out if there is an intersection.
        /// If there is an intersection behind the target position, that hit data will still be written out.
        /// Will output HitData.NoHit if there was no hit at all, which can be checked with the exists() method on the object.</param>
        /// <returns>Returns true, if there is an intersection with at least one scene object 
        /// between startPosition and targetPosition, false otherwise.</returns>
        public bool Intersect(Vector3 startPosition, Vector3 targetPosition, out HitData hitData, bool isShadowRay = false)
        {
            Ray ray = new Ray(startPosition, targetPosition - startPosition);

            if (Intersect(ray, out HitData hit, isShadowRay))
            {
                hitData = hit;
                return (startPosition.Distance(hit.Position) < startPosition.Distance(targetPosition));
            } 
            else
            {
                hitData = HitData.NoHit;
                return false;
            }
        }
    }
}
