using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Rendering
{
    public class RenderSettings
    {
        public (int width, int height) Resolution { get; set; }
        public int AmountThreads { get; set; }
        public int RecursionDepth { get; set; }
        public bool UseLighing { get; set; }
    }
}
