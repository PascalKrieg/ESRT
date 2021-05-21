using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Drawing;

namespace ESRT.Rendering
{
    public class Renderer
    {
        readonly RenderSettings settings;
        readonly Raytracer raytracer;
        Thread[] threads;

        // Framebuffer related variables
        private byte[] frameBuffer;

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

        ConcurrentQueue<RenderSection> sectionQueue = new ConcurrentQueue<RenderSection>();

        /// <summary>
        /// Constructs a Renderer object.
        /// </summary>
        /// <param name="raytracer">The Raytracer to be used. Has to be fully initialized.</param>
        public Renderer(Raytracer raytracer)
        {
            this.settings = raytracer.Settings;
            this.raytracer = raytracer;
            this.frameBuffer = new byte[3 * settings.Resolution.width * settings.Resolution.height];
        }


        /// <summary>
        /// Renders the scene that is set in the raytracer object.
        /// </summary>
        /// <returns>Returns the rendered image.</returns>
        public byte[] StartRendering()
        {
            sectionQueue = new ConcurrentQueue<RenderSection>();
            StartRenderThreads();
            return frameBuffer;
        }


        /// <summary>
        /// Starts the rendering threads according to the render settings and only returns when all rendering threads have terminated.
        /// Important: The Framebuffer has to be initialized beforehand.
        /// </summary>
        private void StartRenderThreads()
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

                    RenderSection renderSection = new RenderSection(frameBuffer,
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
