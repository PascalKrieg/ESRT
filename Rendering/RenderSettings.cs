using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Rendering
{
    public class RenderSettings
    {
        /// <summary>
        /// The resolution of the resulting image.
        /// </summary>
        public (int width, int height) Resolution { get; private set; }
        /// <summary>
        /// The amount of threads that will be used to render the image.
        /// </summary>
        public int AmountThreads { get; private set; }
        /// <summary>
        ///  The maximum recursion depth for secondary rays like reflections.
        /// </summary>
        public int RecursionDepth { get; private set; }
        /// <summary>
        ///  The offset of secondary rays to prevent casting a shadow on the object the ray started on.
        /// </summary>
        public float SecondaryRayOffset { get; private set; }
        /// <summary>
        /// Indicates whether shadows will be rendered.
        /// </summary>
        public bool CastShadows { get; private set; }

        /// <summary>
        /// Constructs a render setting object.
        /// </summary>
        /// <param name="resolution">The resolution of the resulting image.</param>
        /// <param name="amountThreads">The amount of threads that will be used to render the image.</param>
        /// <param name="recursionDepth">The maximum recursion depth for secondary rays like reflections.</param>
        /// <param name="secondaryRayOffset">The offset of secondary rays to prevent casting a shadow on the object the ray started on.</param>
        /// <param name="castShadows">Indicates whether shadows will be rendered.</param>
        public RenderSettings((int width, int height) resolution, int amountThreads, int recursionDepth, float secondaryRayOffset, bool castShadows)
        {
            Resolution = resolution;
            AmountThreads = amountThreads;
            RecursionDepth = recursionDepth;
            SecondaryRayOffset = secondaryRayOffset;
            CastShadows = castShadows;
        }

        /// <summary>
        /// Represents a default Setting of 1920x1080 single threaded rendered image with recursion depth of 5.
        /// </summary>
        public static RenderSettings Default
        {
            get => new RenderSettings((1920, 1080), 1, 5, 0.003f, true);
        }

        /// <summary>
        /// Represents a default Setting of 1920x1080 multithreaded rendered image with recursion depth of 5, using 2 threads.
        /// </summary>
        public static RenderSettings Default2Threads
        {
            get => new RenderSettings((1920, 1080), 2, 5, 0.003f, true);
        }

        /// <summary>
        /// Represents a default Setting of 1920x1080 multithreaded rendered image with recursion depth of 5, using 4 threads.
        /// </summary>
        public static RenderSettings Default4Threads
        {
            get => new RenderSettings((1920, 1080), 4, 5, 0.003f, true);
        }

        /// <summary>
        /// Represents a default Setting of 1920x1080 multithreaded rendered image with recursion depth of 5, using 8 threads.
        /// </summary>
        public static RenderSettings Default8Threads
        {
            get => new RenderSettings((1920, 1080), 8, 5, 0.003f, true);
        }

        /// <summary>
        /// Represents a default Setting of 1920x1080 multithreaded rendered image with recursion depth of 5, using 16 threads.
        /// </summary>
        public static RenderSettings Default16Threads
        {
            get => new RenderSettings((1920, 1080), 16, 5, 0.003f, true);
        }
    }
}
