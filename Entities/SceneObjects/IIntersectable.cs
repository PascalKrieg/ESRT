using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities
{
    /// <summary>
    /// Describes an Object a ray can be intersected with.
    /// </summary>
    public interface IIntersectable
    {
        /// <summary>
        /// Tests for an intersection of a ray with the object.
        /// </summary>
        /// <param name="ray">The ray that will be intersected with the object.</param>
        /// <param name="hitData">Output of the hit data of the closest hit.</param>
        /// <returns>Returns true, if there is an intersection with the object, false otherwise.</returns>
        bool Intersect(Ray ray, out HitData hitData);
    }
}
