using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Rendering
{
    public class RenderSettings
    {
        public (int width, int height) Resolution { get; private set; }
        public int AmountThreads { get; private set; }
        public int RecursionDepth { get; private set; }
        public float SecondaryRayOffset { get; private set; }
        public bool UseLighing { get; private set; }

        public RenderSettings((int width, int height) resolution, int amountThreads, int recursionDepth, float secondaryRayOffset, bool useLighing)
        {
            Resolution = resolution;
            AmountThreads = amountThreads;
            RecursionDepth = recursionDepth;
            SecondaryRayOffset = secondaryRayOffset;
            UseLighing = useLighing;
        }

        public static RenderSettings Default
        {
            get => new RenderSettings((1920, 1080), 1, 5, 0.003f, true);
        }
    }
}
