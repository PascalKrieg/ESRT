using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT
{
    public class Ray
    {
        public Vector3 Start { get; private set; }
        public Vector3 Direction { get; private set; }

        public Ray(Vector3 start, Vector3 direction)
        {
            Start = start;
            Direction = direction;
        }
    }
}
