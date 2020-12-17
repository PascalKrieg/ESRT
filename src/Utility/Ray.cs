using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT
{
    /// <summary>
    /// Class that represents a Ray: a starting point and a direction with no length limit.
    /// </summary>
    public class Ray
    {
        /// <summary>
        /// The position where the Ray starts.
        /// </summary>
        public Vector3 Start { get; private set; }
        /// <summary>
        /// The direction vector of the ray.
        /// </summary>
        public Vector3 Direction { get; private set; }

        /// <summary>
        /// Constructs a new Ray.
        /// </summary>
        /// <param name="start">The position where the Ray starts.</param>
        /// <param name="direction">The direction vector of the ray.</param>
        public Ray(Vector3 start, Vector3 direction)
        {
            Start = start;
            Direction = direction;
        }
    }
}
