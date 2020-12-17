using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;

namespace ESRT.Rendering
{
    public unsafe class ArrayRenderer
    {
        readonly RenderSettings settings;
        readonly Bitmap image;
        readonly Raytracer raytracer;
        Thread[] threads;

        public bool isRendering
        {
            get
            {
                return !sectionQueue.IsEmpty;
            }
        }

        public void StopRendering()
        {
            foreach (Thread t in threads)
            {
                t.Abort();
            }
        }

        public void WaitForFinish()
        {
            foreach (Thread t in threads)
            {
                t.Join();
            }
        }

        // Framebuffer related variables
        BitmapData frameBuffer;
        byte* bufferPointer;

        ConcurrentQueue<RenderSection> sectionQueue = new ConcurrentQueue<RenderSection>();

        /// <summary>
        /// Constructs a Renderer object.
        /// </summary>
        /// <param name="raytracer">The Raytracer to be used. Has to be fully initialized.</param>
        public ArrayRenderer(Raytracer raytracer)
        {
            this.settings = raytracer.Settings;
            this.raytracer = raytracer;
            image = new Bitmap(settings.Resolution.width, settings.Resolution.height, PixelFormat.Format24bppRgb);
        }


        /// <summary>
        /// Renders the scene that is set in the raytracer object.
        /// </summary>
        /// <returns>Returns the rendered image.</returns>
        public IntPtr StartRendering()
        {
            frameBuffer = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            bufferPointer = (byte*)frameBuffer.Scan0.ToPointer();
            sectionQueue = new ConcurrentQueue<RenderSection>();
            RunRenderThreads();
            return (IntPtr)frameBuffer.Scan0.ToPointer();
        }


        /// <summary>
        /// Starts the rendering threads according to the render settings and only returns when all rendering threads have terminated.
        /// Important: The Framebuffer has to be initialized beforehand.
        /// </summary>
        private void RunRenderThreads()
        {
            int gridResolution = 8;

            // Generate sections
            int sectionWidth = settings.Resolution.width / gridResolution;
            int sectionHeight = settings.Resolution.height / gridResolution;
            for (int i = 0; i < gridResolution; i++)
            {
                for (int j = 0; j < gridResolution; j++)
                {
                    (int start, int end) xDimensions = (i * sectionWidth, (i + 1) * sectionWidth);
                    (int start, int end) yDimensions = (j * sectionHeight, (j + 1) * sectionHeight);

                    // Handle edge cases where width and height aren't evenly divisible by the grid resolution
                    if (i == gridResolution - 1)
                        xDimensions.end = settings.Resolution.width;

                    if (j == gridResolution - 1)
                        yDimensions.end = settings.Resolution.height;

                    RenderSection renderSection = new RenderSection(bufferPointer,
                        xDimensions.start, xDimensions.end,
                        yDimensions.start, yDimensions.end, raytracer);

                    sectionQueue.Enqueue(renderSection);
                }
            }

            // Start threads with the correct sections
            threads = new Thread[settings.AmountThreads];
            for (int i = 0; i < settings.AmountThreads; i++)
            {
                threads[i] = new Thread(new ThreadStart(() =>
                {
                    while (sectionQueue.TryDequeue(out RenderSection section))
                    {
                        section.CalculateSection();
                    }
                }));
                threads[i].Start();
            }
        }
    }
}
