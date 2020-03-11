using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities
{
    public interface IIntersectable
    {
        bool Intersect(Vector3 incomingStart, Vector3 incomingDirection, out HitData hitData);
    }
}
