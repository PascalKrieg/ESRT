using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Environment
{
    /// <summary>
    /// Interface that requires a method to return a color based on a ray.
    /// </summary>
    public interface IEnvironment
    {
        /// <summary>
        /// Gets the color of the environment based on the ray.
        /// </summary>
        /// <param name="ray">The ray that was shot into the environment.</param>
        /// <returns>The color of the environment.</returns>
        Color GetEnvironmentColor(Ray ray);
    }
}
