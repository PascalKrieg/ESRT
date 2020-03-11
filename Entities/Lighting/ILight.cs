using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities.Lighting
{
    public interface ILight
    {
        Vector3 Position { get; }

        Color GetIntensity(Vector3 outDirection);
    }
}
