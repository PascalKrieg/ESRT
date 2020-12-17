using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities.Materials
{
    /// <summary>
    /// The shading modes available.
    /// </summary>
    public enum Shading
    {
        /// <summary>
        /// Normals on a triangle will always be the actual mathematical normal of the surface.
        /// </summary>
        Flat,
        /// <summary>
        /// Normals on a trianlge will be interpolated between the vertices.
        /// </summary>
        Smooth
    }
}
