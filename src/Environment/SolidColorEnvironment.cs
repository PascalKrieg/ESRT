using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Environment
{
    /// <summary>
    /// Environment that always returns a solid color.
    /// </summary>
    public class SolidColorEnvironment : IEnvironment
    {
        /// <summary>
        /// The color that will always be returned.
        /// </summary>
        public Color BackgroundColor { get; set; }

        public Color GetEnvironmentColor(Ray ray)
        {
            return BackgroundColor;
        }

        /// <summary>
        /// Creates a solid color environment, that returns the same color for every ray.
        /// </summary>
        /// <param name="backgroundColor">The default color that is returned every time.</param>
        public SolidColorEnvironment(Color backgroundColor)
        {
            BackgroundColor = backgroundColor;
        }
    }
}
